using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Pulsar.Reflection.Dynamic;
//using OID = System.Guid;

namespace Pulsar.Serialization
{
	/// <summary>
	/// Класс сериализатора объектов.
	/// </summary>
	public static partial class PulsarSerializer
	{
		/// <summary>
		/// Метод десериализации объекта.
		/// </summary>
		/// <param name="stream">Поток десериализации.</param>
		/// <returns></returns>
		public static object Deserialize(Stream stream)
		{
			return Deserialize(stream, null, false);
		}
		/// <summary>
		/// Метод десериализации объекта.
		/// </summary>
		/// <param name="stream">Поток десериализации.</param>
		/// <param name="root">Объект, в который будет производится десериализация.</param>
		/// <returns></returns>
		public static object Deserialize(Stream stream, object root, bool isLoad = false)
		{
			try
			{
				if(stream == null)
					throw new ArgumentNullException("stream");
				if(stream.CanRead == false)
					throw new Exception("Указанный поток десериализации не позволяет чтение!");

				DeserContext cox = new DeserContext(stream);

				ushort typeID;
				while((typeID = stream.ReadUInt16()) < ushort.MaxValue)
				{
					if(typeID == 0)
					{
						SerTypeInfo sti = new SerTypeInfo();
						sti.Load(stream);
						cox.types.Process(sti);
						continue;
					}
					if(typeID == (ushort)InfraTypes.GOLObjectRegistrator)
					{		
						#region
						ushort xx = stream.ReadUInt16();
						Type stt = cox.types[xx];
						uint stid = stream.ReadUInt32();
						OID stoid = (OID)FromBytes(typeof(OID), stream.ReadBytes(16));
						GlobalObject go = GOL.Get(stoid);
						if(go != null)
						{
							if(GOL.IsInitMode == false && isLoad == false && Pulsar.Server.ServerParamsBase.IsServer)
								throw new Exception("Попытка перезаписать объект на сервере!");
							if(go.Equals(root) == false)
								cox.objs.Add(stid, new DeserObj(go));
						}
						else
						{
							if(root != null && root is GlobalObject && ((GlobalObject)root).OID == stoid)
							{
								go = (GlobalObject)root;
								root = null;
							}
							else
							{
								object obj = FormatterServices.GetUninitializedObject(stt);
								if(obj is GlobalObject == false)
									throw new SerializationException(String.Format("Тип [{0}] не наследует GlobalObject!", stt.FullName));
								go = (GlobalObject)obj;
								go.OID = stoid;
							}
							cox.objs.Add(stid, new DeserObj(go));
							////GOL.AddUninitialized(go, 0, null);
							GOL.Add(go);
						}
						if(isLoad == false)
							go.BeginWrite();

						#endregion
						continue;
					}
					if(typeID == (ushort)InfraTypes.GOLObjectStub)
					{
						#region
						Type stt = cox.types[stream.ReadUInt16()];
						uint stid = stream.ReadUInt32();
						OID stoid = (OID)FromBytes(typeof(OID), stream.ReadBytes(16));

						GlobalObject go = GOL.Locate(stt,stoid);
						if(go != null)
						{
							cox.objs.Add(stid, new DeserObj(go));
							if(go.IsInitialized)
							 cox.objs[stid].WaitsCount = 0;
							else
							{
								if(cox.lateLoad.Contains(stid) == false)
									cox.lateLoad.Add(stid);
							}
							////if(GOL.IsUninitialized(stoid) == false)
							//// cox.objs[stid].WaitsCount = 0;
							////else
							////{
							//// GOL.AddUninitialized(go, stid, cox);
							//// if(cox.lateLoad.Contains(stid) == false)
							////  cox.lateLoad.Add(stid);
							////}
						}
						else /*if(GOL.LateInitMode == GOLLateInitModes.AllowAll ||
														(GOL.LateInitMode == GOLLateInitModes.AllowEssences && 
															stt.IsDefined(typeof(PulsarEssenceAttribute), true)))*/
						{
							object obj = FormatterServices.GetUninitializedObject(stt);
							if(obj is GlobalObject == false)
								throw new SerializationException(String.Format("Тип [{0}] не наследует IGOLObject!", stt.FullName));

							go = (GlobalObject)obj;
							go.OID = stoid;
							cox.objs.Add(stid, new DeserObj(go));
							////GOL.AddUninitialized(go, stid, cox);
							GOL.Add(go);
							cox.lateLoad.Add(stid);
						}
						//else
						// throw new SerializationException(String.Format("Заглушка объекта {{{0}}} типа [{1}] не раскрыта!",
						//  stoid, stt.FullName));
						#endregion
						continue;
					}

					SerObjectInfo si = new SerObjectInfo();
					si.typeID = typeID;
					si.Load(stream);

					Type t = cox.types[si.typeID];
					DeserObj siObjID = null;

					if(typeID == (ushort)InfraTypes.PulsarPrimitiveHolder)
					{
						SerFieldInfo fsi = si.index["Primitive"];
						t = cox.types[fsi.typeID];
						Object val = null;
						if(typeof(ISelfSerialization).IsAssignableFrom(t))
						{
							val = root == null ? FormatterServices.GetUninitializedObject(t) : root;
							var tw = TypeSerializationWrap.GetTypeSerializationWrap(t);
							tw.OnDeserializing(val);
							((ISelfSerialization)val).Deserialize(fsi.value);
							tw.OnDeserialized(val);
						}
						else
							val = FromBytes(t, fsi.value);
						return val;
					}



					if(root == null) // && cox.objs.Count == 0)				 && t != typeof(PulsarPrimitiveHolder)
						cox.objs.TryGetValue(si.objID, out siObjID);
					else
					{
						if(t.IsAssignableFrom(root.GetType()) == false || si.index == null || si.index.ContainsKey("$root$") == false)
							throw new Exception("Данные сериализации не соответствуют объекту root!");
						cox.objs.Add(si.objID, siObjID = new DeserObj(root));
						if(si.index.ContainsKey("oid"))
							((GlobalObject)root).OID = (OID)FromBytes(typeof(OID),si.index["oid"].value);
						root = null;
					}

					if(t == typeof(PulsarEmptyStub))
					{
						#region
						t = cox.types[si.index[""].typeID];
						if(siObjID == null)
							cox.objs.Add(si.objID, siObjID = new DeserObj(t));
						siObjID.IsEmpty = true;
						ConstructorInfo ci = t.GetConstructor(fieldsDiscovery, null, Type.EmptyTypes, null);
						if(ci != null)
							ci.Invoke(siObjID.Obj, null);
						if(si.index.ContainsKey("oid") && siObjID.Obj is GlobalObject)  //  
							if(siObjID.Obj is GlobalObject)
								((GlobalObject)siObjID.Obj).OID =  new OID(si.index["oid"].value);
							else
							{
								InterfaceMapping im = t.GetInterfaceMap(typeof(IOIDObject));
								if(im.TargetMethods.Length == 0)
									throw new SerializationException(String.Format("Тип [{0}] не содержит свойство OID!", t.FullName));
								if(t != im.TargetMethods[0].DeclaringType)
									t = im.TargetMethods[0].DeclaringType;
								PropertyInfo pi = t.GetProperty("OID", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
								if(pi == null)
									throw new SerializationException(String.Format("Тип [{0}] не содержит свойство OID!", t.FullName));
								MethodInfo mi = pi.GetSetMethod(true);
								if(mi == null)
									throw new SerializationException(String.Format("Тип [{0}] не реализует set метод для свойства OID!",
																																																																				siObjID.Obj.GetType().FullName));
								mi.Invoke(siObjID.Obj, new object[] { new OID(si.index["oid"].value) });
							}
						#endregion
						siObjID.WaitsCount = 0;
					}

					if(siObjID == null)
						//if(typeof(GlobalObject).IsAssignableFrom(t))
						// if(pars
						// throw new Exception("Попытка создать объект Пульсара вне GOL!");
						//else
							cox.objs.Add(si.objID, siObjID = new DeserObj(t));
					if(siObjID.WaitsCount != 0)
						CallOnDeserializing(siObjID.Obj);

					if(siObjID.WaitsCount != 0 &&  siObjID.Obj is ISerializable)
					{
						if(siObjID.WaitsCount == -1)
							siObjID.WaitsCount = 0;
						#region
						siObjID.Tag = new object[2];
						SerFieldInfo fsi = si.index["m_members"];
						object val = null;
						UnPackArray(ref val, fsi.value, typeof(string), si.objID, cox);
						((object[])siObjID.Tag)[0] = val;

						val = null;
						fsi = si.index["m_data"];
						UnPackArray(ref val, fsi.value, typeof(object), si.objID, cox);
						((object[])siObjID.Tag)[1] = val;

						if(siObjID.WaitsCount != 0)
							continue;
						#endregion
					}

					if(siObjID.WaitsCount == 0)
					{
						OnFullDeser(cox, si.objID, true);
						continue;
					}

					string pref = "";
					if(siObjID.WaitsCount == -1)
						siObjID.WaitsCount = 0;
					while(t != typeof(object))
					{
						foreach(FieldWrap fw in TypeSerializationWrap.GetTypeSerializationWrap(t).Fields)
						{
							if(si.index == null || si.index.ContainsKey(pref + fw.Name) == false)
								continue;

							SerFieldInfo fsi = si.index[pref + fw.Name];
							Type valType = cox.types[fsi.typeID];
							object val = null; 

							if(fsi.typeID <= 25 || valType.IsEnum  || fsi.typeID == (uint)InfraTypes.RefString)
									//) && fsi.typeID != 18  && fsi.typeID != 19 )
								val = FromBytes(valType, fsi.value);
							else if(typeof(ISelfSerialization).IsAssignableFrom(valType))
							{
								val = FormatterServices.GetUninitializedObject(valType);
								TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(valType);
								tw.OnDeserializing(val);
								((ISelfSerialization)val).Deserialize(fsi.value);
								tw.OnDeserialized(val);
							}
							else if(valType == typeof(Type))
								val = cox.types[BitConverter.ToUInt16(fsi.value, 0)];
							else if(valType.IsArray)
								UnPackArray(ref val, fsi.value, valType.GetElementType(), si.objID, cox);
							else
							{
								uint id = BitConverter.ToUInt32(fsi.value, 0);

								DeserObj dObj = null;

								if(cox.objs.TryGetValue(id, out dObj)) //  cox.objs.ContainsKey(id)
								{
									if(dObj.WaitsCount == 0)
										val = dObj.Obj;
									else
									{
										siObjID.WaitsCount++;

										List<LateDeserInfo> lll;
										if(cox.lateSet.TryGetValue(id, out lll) == false)
											cox.lateSet.Add(id, lll = new List<LateDeserInfo>());
										lll.Add(new LateDeserInfo(si.objID, fw));
										continue;
									}
								}
								else
								{
									// Противоречит "глубокой" сериализации
									//if(typeof(GlobalObject).IsAssignableFrom(valType))
									// throw new Exception("Попытка создать GlobalObject вне GOL!");
									cox.objs.Add(id, new DeserObj(valType));
									siObjID.WaitsCount++;
									
									List<LateDeserInfo> lll;
									if(cox.lateSet.TryGetValue(id, out lll) == false)
										cox.lateSet.Add(id, lll = new List<LateDeserInfo>());
									lll.Add(new LateDeserInfo(si.objID, fw));
									continue;
								}
							}
							fw.Set(siObjID.Obj, val);
						}
						t = t.BaseType;
						if(t == null || t == typeof(object))
							break;
						pref += '.';
					}

					if(siObjID.WaitsCount == 0)
						OnFullDeser(cox, si.objID, true);
				}
				if(cox.lateSet.Count > 0) //!! && GOL.LateInitMode != GOLLateInitModes.AllowAll)
				{
					#region
					////List<GlobalObject> toLoad = new List<GlobalObject>(cox.lateLoad.Count);
					////foreach(uint i in cox.lateLoad)
					//// if(cox.objs[i].Obj is GlobalObject)
					////  toLoad.Add((GlobalObject)cox.objs[i].Obj);
					//// else
					////  throw new Exception("Найдено неразрешимое кольцевое замыкание объектов!");
					////if(toLoad.Count == 0)
					//// throw new Exception("Найдено неразрешимое кольцевое замыкание объектов!");
					////GOL.LoadGlobalObjects(toLoad);

					if(cox.lateLoad.Count > 0)
						GOL.LoadGlobalObjects(cox.lateLoad.ForEach((i)=> (GlobalObject)	cox.objs[i].Obj));

					foreach(uint i in cox.lateLoad)
					{
					 GlobalObject go = (GlobalObject)	cox.objs[i].Obj;
						if(go.IsInitialized)
							OnFullDeser(cox,i,false);
						else
						 throw new PulsarException("Не удалось загрузить глобальный объект [{0}] типа {1}!", go.OID, go.GetType());
					}		 
					if(cox.lateSet.Count != 0)
						throw new Exception("Найдено неразрешимое кольцевое замыкание объектов!");
					#endregion
				}
				if(cox.objs.Count == 0)
					return null;
				//if(cox.objs[1].Obj is PulsarPrimitiveHolder)
				// return ((PulsarPrimitiveHolder)cox.objs[1].Obj).Primitive;
				return cox.objs[1].Obj;
			}
			catch
			{
				//--- Debbuger Break --- //
				if(System.Diagnostics.Debugger.IsAttached)
					System.Diagnostics.Debugger.Break();
				//--- Debbuger Break --- //
				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		#region << Private Methods >>
		internal static void OnFullDeser(DeserContext cox, uint rootID, bool needDeserEventsForRoot)
		{
			try
			{
				Queue<uint> queue = new Queue<uint>();
				queue.Enqueue(rootID);
				while(queue.Count > 0)
				{
					uint id = queue.Dequeue();
					DeserObj rIDObj = cox.objs[id];
					if(needDeserEventsForRoot == true || id != rootID)
						CallOnDeserialized(rIDObj);
					if(cox.lateSet.ContainsKey(id))
						foreach(LateDeserInfo i in cox.lateSet[id])
						{
							DeserObj iId = cox.objs[i.id];

							if(i.fi != null)
								i.fi.Set(iId.Obj, rIDObj.Obj);
							else if(i.array != null)
								i.array.SetValue(rIDObj.Obj, i.index);
							else
								throw new SerializationException("Failed LateDeserInfo!");
							iId.WaitsCount--;
							if(iId.WaitsCount == 0)
							{
								if(cox.lateSet.ContainsKey(i.id))
									queue.Enqueue(i.id);
								else
								{
									if(iId.IsEmpty == false)
										CallOnDeserialized(iId);
								}
							}
						}
					cox.lateSet.Remove(id);
				}
			}
			catch
			{

				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		private static void UnPackArray(ref object arr, byte[] data, Type elemType, uint owner, DeserContext cox)
		{
			MemoryStream ms = null;
			try
			{
				ms = new MemoryStream(data);
				int[] dims = new int[ms.ReadByte()];
				for(int a = 0; a < dims.Length; a++)
					dims[a] = ms.ReadInt32();

				arr = Array.CreateInstance(elemType, dims);

				//bool fullDeser = true;

				if(IsPrimitive(elemType))
				{
					#region
					uint size = (uint)SizeOfPrimitive(elemType);
					ArrayIndexEnumerator aie = new ArrayIndexEnumerator((Array)arr);
					while(aie.MoveNext())
						((Array)arr).SetValue(FromBytes(elemType, ms.ReadBytes(size)), aie.Current);
					#endregion
				}
				else if(elemType == typeof(String) || elemType == typeof(RefString))
				{
					#region
					byte b;
					ArrayIndexEnumerator aie = new ArrayIndexEnumerator((Array)arr);
					while(aie.MoveNext())
					{
						using(MemoryStream line = new MemoryStream())
							while(true)
							{
								b = (byte)ms.ReadByte();
								if(b == 0xFE)
									((Array)arr).SetValue(null, aie.Current);
								else if(b == 0xFF)
									((Array)arr).SetValue(FromBytes(elemType, line.ToArray()), aie.Current);
								else
									line.WriteByte(b);
								if(b >= 0xFE)
									break;
							}
					}
					#endregion
				}
				else if(elemType == typeof(Type))
				{
					#region
					ArrayIndexEnumerator aie = new ArrayIndexEnumerator((Array)arr);
					while(aie.MoveNext())
					{
						ushort typeid = ms.ReadUInt16();
						if(typeid != 0)
							((Array)arr).SetValue(cox.types[typeid], aie.Current);
					}
					#endregion
				}
				else
				{
					ArrayIndexEnumerator aie = new ArrayIndexEnumerator((Array)arr);
					while(aie.MoveNext())
					{
						ushort typeid = ms.ReadUInt16();
						if(typeid == 0)
							continue;
						Type t = cox.types[typeid];

						if(t.IsArray)
						{
							object val = null;
							uint len = ms.ReadUInt32();
							UnPackArray(ref val, ms.ReadBytes(len), t.GetElementType(), owner, cox);
							((Array)arr).SetValue(val, aie.Current);
						}
						else if(typeid < 25 || t.IsEnum)
							((Array)arr).SetValue(FromBytes(t, ms.ReadBytes((uint)SizeOfPrimitive(t))), aie.Current);
						else if(t == typeof(String) || t == typeof(RefString))
						{
							#region
							using(MemoryStream line = new MemoryStream())
							{
								byte b;
								while(true)
								{
									b = (byte)ms.ReadByte();
									if(b == 0xFE)
										((Array)arr).SetValue(null, aie.Current);
									else if(b == 0xFF)
										((Array)arr).SetValue(FromBytes(t, line.ToArray()), aie.Current);
									else
										line.WriteByte(b);
									if(b >= 0xFE)
										break;
								}
							}
							#endregion
						}
						else if(typeof(ISelfSerialization).IsAssignableFrom(t))
						{
							#region
							object val = FormatterServices.GetUninitializedObject(t);
							TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(t);
							tw.OnDeserializing(val);

							byte len = (byte)ms.ReadByte();
							if(len > 0)
								((ISelfSerialization)val).Deserialize(ms.ReadBytes(len));
							
							tw.OnDeserialized(val);

							((Array)arr).SetValue(val, aie.Current); 
							#endregion
						}
						else if(t == typeof(Type))
							((Array)arr).SetValue(cox.types[ms.ReadUInt16()], aie.Current);
						else
						{
							uint id = ms.ReadUInt32();

							DeserObj dId;
							if(cox.objs.TryGetValue(id, out dId))
							{
								if(dId.WaitsCount == 0)
									((Array)arr).SetValue(dId.Obj, aie.Current);
								else
								{
									cox.objs[owner].WaitsCount++;
									List<LateDeserInfo> lll;
									if(cox.lateSet.TryGetValue(id, out lll) == false)
										cox.lateSet.Add(id, lll = new List<LateDeserInfo>());
									lll.Add(new LateDeserInfo(owner, ((Array)arr), (int[])aie.Current.Clone()));
								}
							}
							else
							{
								// Противоречит "глубокой" сериализации
								//if(typeof(GlobalObject).IsAssignableFrom(t))
								// throw new Exception("Попытка создать GlobalObject вне GOL!");
								cox.objs.Add(id, new DeserObj(t));
								cox.objs[owner].WaitsCount++;
								List<LateDeserInfo> lll;
								if(cox.lateSet.TryGetValue(id, out lll) == false)
									cox.lateSet.Add(id, lll = new List<LateDeserInfo>());
								lll.Add(new LateDeserInfo(owner, ((Array)arr), (int[])aie.Current.Clone()));
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if(ms != null)
					ms.Dispose();
			}
		}
		//-------------------------------------------------------------------------------------
		private static void CallOnDeserializing(object obj)
		{
			TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(obj.GetType());
			tw.OnDeserializing(obj);
		}
		//-------------------------------------------------------------------------------------
		private static void CallOnDeserialized(DeserObj dObj)
		{
			try
			{
				if(dObj.Obj is GlobalObject)
				{
				 GlobalObject go = (GlobalObject)dObj.Obj;
					go.IsInitialized = true;
					if(((IGlobalObjectMeta)go).GlobalName != null)
					 GOL.RegistryGlobalName(go);
				}
				if(dObj.Obj is ISerializable)
				{
					#region
					Type t = dObj.Obj.GetType();
					MethodBase ci = null;
					while(t != null || t != typeof(object))
					{
						ci = t.GetMethod("SetObjectData", fieldsDiscovery, null,
									new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
						if(ci == null)
							ci = t.GetConstructor(fieldsDiscovery, null,
										new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
						if(ci == null)
							t = t.BaseType;
						else
							break;
					}
					if(ci == null)
						throw new PulsarException(
							"Тип [{0}] реализует интерфейс ISerializable, но не имеет конструктора вида .ctor(SerializationInfo,StreamingContext)"+
						" или метода SetObjectData(SerializationInfo,StreamingContext)!",
								dObj.Obj.GetType());
					SerializationInfo serInfo = new SerializationInfo(dObj.Obj.GetType(), new FormatterConverter());
					string[] m_members = (string[])((object[])dObj.Tag)[0];
					object[] m_data = (object[])((object[])dObj.Tag)[1];
					for(int pos = 0; pos < m_members.Length; pos++)
						if(m_members[pos] != null)
							serInfo.AddValue(m_members[pos], m_data[pos]);
					//ReflectionHelper.InvokeCtor(t, dObj.Obj, new[] { typeof(SerializationInfo), typeof(StreamingContext) },
					//                             new object[] { serInfo, new StreamingContext() });
					ci.Invoke(dObj.Obj, new object[] { serInfo, new StreamingContext() });
					#endregion
				}


				TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(dObj.Obj.GetType());
				tw.OnDeserialized(dObj.Obj);

				if(dObj.Obj is IDeserializationCallback)
					((IDeserializationCallback)dObj.Obj).OnDeserialization(null);
			}
			catch
			{
				throw;
			}

		}
		#endregion << Private Methods >>
	}
}
