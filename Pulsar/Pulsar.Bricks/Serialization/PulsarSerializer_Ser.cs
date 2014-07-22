using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Pulsar.Reflection.Dynamic;

namespace Pulsar.Serialization
{
	/// <summary>
	/// 
	/// </summary>
	public static partial class PulsarSerializer
	{
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Сериализует объект в поток.
		/// </summary>
		/// <param name="stream">Поток сериализации/</param>
		/// <param name="obj">Сериализуемый объект.</param>
		/// <param name="pars">Параметры сериализации.</param>
		public static void Serialize(Stream stream, object obj, PulsarSerializationParams pars)
		{
			try
			{
				if(obj == null)
					return;
				if(stream == null)
					throw new ArgumentNullException("stream");
				if(stream.CanWrite == false)
					throw new Exception("Указанный поток сериализации не позволяет запись!");
				
				SerContext cox = new SerContext(stream, pars);

				Type objType = obj.GetType();
				if(IsPrimitive(objType) || objType == typeof(string) || objType == typeof(RefString) || obj is Array || 
								obj is Type || obj is ISelfSerialization)
					obj = new PulsarPrimitiveHolder(obj);
				else if(cox.mode != PulsarSerializationMode.Backup && obj is GlobalObject)
				{
					if(StubGOLObject((GlobalObject)obj, cox, 0, 0))	// cox.noStubObjects.Contains(obj) == false &&
					{
						stream.WriteUInt16(ushort.MaxValue);
						return;
					}
				}
				if(cox.stack.Contains(obj) == false)
					cox.stack.Push(obj);


				ushort objTypeID = 0;
				uint objID = 0;
				SerObjectInfo si = null;
				while(cox.stack.Count > 0)
				{
					obj = cox.stack.Pop();
					objType = obj.GetType();
					objTypeID = cox.types.GetTypeID(objType);
					objID = cox.objs.GetObjID(objTypeID, obj);

					if(si == null)
					{
						si = new SerObjectInfo() { fields = new List<SerFieldInfo>() };
						si.fields.Add(new SerFieldInfo() { name = "$root$" });
						if(obj is GlobalObject)
						{
							SerFieldInfo fsi = new SerFieldInfo();
							fsi.typeID = (ushort)PrimitiveTypes.OID;
							fsi.name = "oid";
							fsi.value = ToBytes(((GlobalObject)obj).OID);
							si.fields.Add(fsi);
						}
					}
					else
						si = new SerObjectInfo();

					if(cox.asEmptyTypes.Contains(objType) || cox.asEmptyObjects.Contains(obj))
					{
						#region
						si.typeID = (ushort)InfraTypes.PulsarEmptyStub;
						si.objID = objID;
						if(si.fields == null)
						 si.fields = new List<SerFieldInfo>(1);

						SerFieldInfo fsi = new SerFieldInfo();
						fsi.typeID = objTypeID;
						si.fields.Add(fsi);

						if(obj is GlobalObject)
						{
							fsi = new SerFieldInfo();
							fsi.typeID = (ushort)PrimitiveTypes.OID;
							fsi.name = "oid";
							fsi.value = ToBytes(((GlobalObject)obj).OID);
							si.fields.Add(fsi);
						}

						si.Save(stream);
						#endregion
						continue;
					}

					si.typeID = objTypeID;
					si.objID = objID;
					CallOnSerializing(obj, cox);

					if(obj is ISerializable)
					{
						#region
						SerializationInfo serInfo = new SerializationInfo(objType, nativeConverter);
						((ISerializable)obj).GetObjectData(serInfo, new StreamingContext());

						FieldWrap m_members = null;
						FieldWrap m_data = null;

						TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(typeof(SerializationInfo));
						foreach(FieldWrap fw in tw.Fields)
							if(fw.Name == "m_members")
								m_members = fw;
							else if(fw.Name == "m_data")
								m_data = fw;

						if(si.fields == null)
							si.fields = new List<SerFieldInfo>(2);
						SerFieldInfo fsi = new SerFieldInfo();
						fsi.typeID = (ushort)InfraTypes.StringArr;
						fsi.name = "m_members";
						fsi.value = PackArray((Array)m_members.Get(serInfo), cox);
						si.fields.Add(fsi);

						fsi = new SerFieldInfo();
						fsi.typeID = (ushort)InfraTypes.ObjectArr;
						fsi.name = "m_data";
						fsi.value = PackArray((Array)m_data.Get(serInfo), cox);
						si.fields.Add(fsi);

						si.Save(stream);
						CallOnSerialized(obj, cox);
						#endregion
						continue;
					}

					string pref = "";
					while(objType != typeof(object))
					{
						//foreach(FieldInfo fi in objType.GetFields(fieldsDiscovery))
						foreach(FieldWrap fw in TypeSerializationWrap.GetTypeSerializationWrap(objType).Fields)
						{
							if(fw.NoSerMode != null)
								if(fw.NoSerMode.Value == PulsarSerializationMode.Default || 
												cox.mode == PulsarSerializationMode.Default || 
											(fw.NoSerMode & cox.mode) > 0)
												continue;

							object val = fw.Get(obj);

							if(val == null || val is Delegate)
								continue;
							Type valType = val is Type ? typeof(Type) : val.GetType();
							if(valType.IsValueType && valType == fw.Type && val.Equals(Activator.CreateInstance(valType)))
								continue;

							if(fw.ByDemandModes != null)
								if(fw.ByDemandModes.Value == PulsarSerializationMode.Default || 
												cox.mode == PulsarSerializationMode.Default ||
												(fw.ByDemandModes & cox.mode) == 0)
							{
								if(cox.opts.HasFlag(PulsarSerializationOptions.IgnoreAllByDemandSerialization) == false &&
											cox.byDemandTypes.Contains(valType) == false)
									continue;
							}

							SerFieldInfo fsi = new SerFieldInfo();
							fsi.typeID = cox.types.GetTypeID(valType);
							fsi.name = pref + fw.Name;

							// При добавлении, посмотреть PackArray
							if(fsi.typeID <= 25 || valType.IsEnum || fsi.typeID == (uint)InfraTypes.RefString)
								fsi.value = ToBytes(val);
							else if(val is ISelfSerialization)
							{
								CallOnSerializing(val, cox);
								fsi.value = ((ISelfSerialization)val).GetSerializedData();
								CallOnSerialized(val, cox);
							}
							else if(val is Type)
								fsi.value = ToBytes(cox.types.GetTypeID((Type)val));
							else if(val is Array)
								fsi.value = PackArray((Array)val, cox);
							else
							{
								uint id;
								if(cox.objs.AddAsNew(fsi.typeID, val, out id))
									if(val is GlobalObject && pars != null && pars.Options.HasFlag(PulsarSerializationOptions.DeepSerialization) == false)
										StubGOLObject((GlobalObject)val, cox, fsi.typeID, id);
									else
										cox.stack.Push(val);
								fsi.value = ToBytes(id);
							}
							if(si.fields == null)
								si.fields = new List<SerFieldInfo>();
							si.fields.Add(fsi);
						}
						objType = objType.BaseType;
						if(objType == null || objType == typeof(object))
							break;
						pref += '.';
					}
					si.Save(stream);
					CallOnSerialized(obj, cox);
				}
				stream.WriteUInt16(ushort.MaxValue);
			}
			catch
			{
				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Возвращает true, если объект заглушен, false - если зарегистрирован
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="cox"></param>
		/// <param name="typeid"></param>
		/// <param name="objid"></param>
		/// <returns></returns>
		private static bool StubGOLObject(GlobalObject obj, SerContext cox, ushort typeid, uint objid)
		{
			if(cox.asEmptyTypes.Contains(obj.GetType()) || cox.asEmptyObjects.Contains(obj))
			{
				#region
				SerObjectInfo si = new SerObjectInfo();
				si.typeID = (ushort)InfraTypes.PulsarEmptyStub;
				si.objID = objid;
				si.fields = new List<SerFieldInfo>(1);

				SerFieldInfo fsi = new SerFieldInfo();
				fsi.typeID = typeid;
				si.fields.Add(fsi);

				if(obj is GlobalObject)
				{
					fsi = new SerFieldInfo();
					fsi.typeID = (ushort)PrimitiveTypes.OID;
					fsi.name = "oid";
					fsi.value = ToBytes(((GlobalObject)obj).OID);
					si.fields.Add(fsi);
				}

				si.Save(cox.stream);
				#endregion 
				return true;
			}

			bool noStub = cox.noStubObjects.Contains(obj);
			if(noStub == false && cox.noStubTypes.Count > 0)
			{
			 Type t = obj.GetType();
				while(t != null)
				 if((noStub = cox.noStubTypes.Contains(t)) == true)
					 break;
					else
					 t = t.BaseType;
			}


			if(typeid == 0)
				typeid = cox.types.GetTypeID(obj.GetType());
			if(objid == 0)
				objid = cox.objs.GetObjID(typeid, obj);

			if(noStub)
			{
				cox.stream.WriteUInt16((ushort)InfraTypes.GOLObjectRegistrator);
				if(Pulsar.Server.ServerParamsBase.IsServer == false && GOL.Contains(obj) == false)
				 GOL.Add(obj);
			}
			else
				cox.stream.WriteUInt16((ushort)InfraTypes.GOLObjectStub);

			cox.stream.WriteUInt16(typeid);
			cox.stream.WriteUInt32(objid);
			cox.stream.WriteBytes(ToBytes(obj.OID));
			if(noStub)
			{
			 if(((IReadWriteLockObject)obj).IsLocked == false)
			  ((IReadWriteLockObject)obj).BeginRead();
				cox.stack.Push(obj);
			}
			return !noStub;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] PackArray(Array arr, SerContext cox)
		{
			if(arr.LongLength > arr.Length)
				throw new SerializationException("Слишком большой массив!");

			Type t = arr.GetType().GetElementType();

			byte[] dims = new byte[arr.Rank*4 + 1];
			dims[0] = (byte)arr.Rank;
			for(int a = 0; a < arr.Rank; a++)
				Array.Copy(BitConverter.GetBytes(arr.GetLength(a)), 0, dims, a*4+1, 4);

			byte[] res = null;
			MemoryStream ms = null;
			try
			{
				if(IsPrimitive(t))
				{
					#region
					int sizeOfT = SizeOfPrimitive(t);
					res = new byte[sizeOfT * arr.Length + dims.Length];
					ms = new MemoryStream(res, true);
					ms.WriteBytes(dims);
					foreach(var x in arr)
						if(x == null)
							ms.WriteBytes(new byte[sizeOfT]);
						else
							ms.WriteBytes(ToBytes(x));
					#endregion
				}
				else if(t == typeof(String) || t == typeof(RefString))
				{
					#region
					ms = new MemoryStream();
					ms.WriteBytes(dims);
					foreach(var x in arr)
					{
						if(x == null)
							ms.WriteByte(0xFE);
						else
						{
							ms.WriteBytes(PulsarSerializer.ToBytes(x));
							ms.WriteByte(0xFF);
						}
					}
					ms.Position = 0;
					res = ms.ToArray();
					#endregion
				}
				else if(t == typeof(Type))
				{
					#region
					res = new byte[2 * arr.Length + dims.Length];
					ms = new MemoryStream(res, true);
					ms.WriteBytes(dims);
					foreach(var x in arr)
						if(x == null)
							ms.WriteUInt16(0);
						else
							ms.WriteUInt16(cox.types.GetTypeID((Type)x));
					#endregion
				}
				else
				{
					ms = new MemoryStream();
					ms.WriteBytes(dims);
					foreach(var x in arr)
					{
						if(x == null)
						{
							ms.WriteUInt16(0);
							continue;
						}
						t = x.GetType();
						ushort tid = cox.types.GetTypeID(t);
						if(x is Array)
						{
							#region
							ms.WriteUInt16(tid);
							byte[] buf = PackArray((Array)x, cox);
							if(buf.Length != buf.LongLength)
								throw new SerializationException("Буфер массива слишком большой!");
							ms.WriteUInt32((uint)buf.Length);
							ms.WriteBytes(buf);
							#endregion
						}
						else if(t.IsEnum || tid < 25)
						{
							ms.WriteUInt16(tid);
							ms.WriteBytes(ToBytes(x));
						}
						else if(x is ISelfSerialization)
						{
							ms.WriteUInt16(tid);
							CallOnSerializing(x,cox);
							byte[] val = ((ISelfSerialization)x).GetSerializedData();
							ms.WriteByte((byte)(val == null ? 0 : val.Length));
							if(val != null)
								ms.WriteBytes(val);
							CallOnSerialized(x, cox);
						}
						else if(tid == (ushort)InfraTypes.String || tid == (ushort)InfraTypes.RefString)
						{
							ms.WriteUInt16(tid);
							ms.WriteBytes(ToBytes(x));
							ms.WriteByte(0xFF);
						}
						else if(tid == (ushort)InfraTypes.Type)
						{
							ms.WriteUInt16(tid);
							ms.WriteUInt16(cox.types.GetTypeID((Type)x));
						}
						else
						{
							uint id;
							if(cox.objs.AddAsNew(tid, x, out id))
							{
								if(x is GlobalObject)
									StubGOLObject((GlobalObject)x, cox, tid, id);
								else
									cox.stack.Push(x);
							}
							ms.WriteUInt16(tid);
							ms.WriteUInt32(id);
						}
					}
					res = ms.ToArray();
				}

			}
			finally
			{
				if(ms != null)
					ms.Dispose();
			}
			return res;
		}
		//-------------------------------------------------------------------------------------
		private static void CallOnSerializing(object obj, SerContext cox)
		{
			TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(obj.GetType());
			tw.OnSerializing(obj);
		}
		//-------------------------------------------------------------------------------------
		private static void CallOnSerialized(object obj, SerContext cox)
		{
			TypeSerializationWrap tw = TypeSerializationWrap.GetTypeSerializationWrap(obj.GetType());
			tw.OnSerialized(obj);
		}
		#endregion << Methods >>
	}
}
