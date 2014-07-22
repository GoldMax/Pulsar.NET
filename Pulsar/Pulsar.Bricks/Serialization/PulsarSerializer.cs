using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Pulsar.Reflection.Dynamic;
//using OID = System.Guid;

namespace Pulsar.Serialization
{
	/// <summary>
	/// Класс сериализации Пульсара.
	/// </summary>
	public static partial class PulsarSerializer
	{
		private static FormatterConverter nativeConverter = new FormatterConverter();

		private static BindingFlags fieldsDiscovery = BindingFlags.Instance | BindingFlags.NonPublic |
													  BindingFlags.Public | BindingFlags.DeclaredOnly;

		private static Dictionary<IntPtr, PrimitiveInfo> primitives = new Dictionary<IntPtr, PrimitiveInfo>(100);
		/// <summary>
		/// 
		/// </summary>
		private static Dictionary<Type, ushort> known = null;
		private static Dictionary<string, Type> deserTypes = new Dictionary<string, Type>(100000);

		internal enum InfraTypes : ushort
		{
			String = 25,
			StringArr = 26,
			Type = 27,
			ObjectArr = 28,
			RefString = 43,
			KeyValuePair = 30,
			GOLObjectRegistrator = 46,
			PulsarPrimitiveHolder = 47,
			GOLObjectStub = 48,
			PulsarEmptyStub = 49
		}
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		static PulsarSerializer()
		{
			primitives.Add(typeof(sbyte).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(sbyte),
				FromBytes = (b) => (sbyte)b[0]
			});
			primitives.Add(typeof(byte).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(byte),
				FromBytes = (b) => b[0]
			});
			primitives.Add(typeof(short).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(short),
				FromBytes = (b) => BitConverter.ToInt16(b, 0)
			});
			primitives.Add(typeof(ushort).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(ushort),
				FromBytes = (b) => BitConverter.ToUInt16(b, 0)
			});
			primitives.Add(typeof(int).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(int),
				FromBytes = (b) => BitConverter.ToInt32(b, 0)
			});
			primitives.Add(typeof(uint).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(uint),
				FromBytes = (b) => BitConverter.ToUInt32(b, 0)
			});
			primitives.Add(typeof(long).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(long),
				FromBytes = (b) => BitConverter.ToInt64(b, 0)
			});
			primitives.Add(typeof(ulong).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(ulong),
				FromBytes = (b) => BitConverter.ToUInt64(b, 0)
			});
			primitives.Add(typeof(char).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(char),
				FromBytes = (b) => BitConverter.ToChar(b, 0)
			});
			primitives.Add(typeof(float).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(float),
				FromBytes = (b) => BitConverter.ToSingle(b, 0)
			});
			primitives.Add(typeof(double).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(double),
				FromBytes = (b) => BitConverter.ToDouble(b, 0)
			});
			primitives.Add(typeof(bool).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(bool),
				FromBytes = (b) => BitConverter.ToBoolean(b, 0)
			});
			primitives.Add(typeof(IntPtr).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = IntPtr.Size,
				FromBytes = (b) => (IntPtr)(IntPtr.Size == 4 ? BitConverter.ToInt32(b, 0) : BitConverter.ToInt64(b, 0))
			});
			primitives.Add(typeof(UIntPtr).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = UIntPtr.Size,
				FromBytes = (b) => (UIntPtr)(UIntPtr.Size == 4 ? BitConverter.ToUInt32(b, 0) : BitConverter.ToUInt64(b, 0))
			});
			primitives.Add(typeof(Guid).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = 16,
				FromBytes = (b) => new Guid(b)
			});
			primitives.Add(typeof(OID).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = 16,
				FromBytes = (b) => (b.Length == 1 && b[0] == 0) ? null : new OID(b)
			});
			primitives.Add(typeof(DateTime).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(long),
				FromBytes = (b) => DateTime.FromBinary(BitConverter.ToInt64(b, 0))
			});
			primitives.Add(typeof(Decimal).TypeHandle.Value, new PrimitiveInfo()
			{
				Size = sizeof(Decimal),
				FromBytes = (b) => DecimalFromBytes(b)
			});
			//primitives.Add(typeof(Amount).TypeHandle.Value, new PrimitiveInfo()
			//{
			// Size = sizeof(uint),
			// FromBytes = (b)=> Amount.FromBytes(b)
			//});
			//primitives.Add(typeof(Money).TypeHandle.Value, new PrimitiveInfo()
			//{
			// Size = sizeof(Decimal),
			// FromBytes = (b)=> (Money)DecimalFromBytes(b)
			//});
			//primitives.Add(typeof(MoneyPrecise).TypeHandle.Value, new PrimitiveInfo()
			//{
			// Size = sizeof(Decimal),
			// FromBytes = (b) => (MoneyPrecise)DecimalFromBytes(b)
			//});

			known = new Dictionary<Type, ushort>();
			known.Add(typeof(Byte), (ushort)PrimitiveTypes.Byte);
			known.Add(typeof(SByte), (ushort)PrimitiveTypes.SByte);
			known.Add(typeof(Int16), (ushort)PrimitiveTypes.Int16);
			known.Add(typeof(UInt16), (ushort)PrimitiveTypes.UInt16);
			known.Add(typeof(Int32), (ushort)PrimitiveTypes.Int32);
			known.Add(typeof(UInt32), (ushort)PrimitiveTypes.UInt32);
			known.Add(typeof(Int64), (ushort)PrimitiveTypes.Int64);
			known.Add(typeof(UInt64), (ushort)PrimitiveTypes.UInt64);
			known.Add(typeof(IntPtr), (ushort)PrimitiveTypes.IntPtr);
			known.Add(typeof(UIntPtr), (ushort)PrimitiveTypes.UIntPtr);
			known.Add(typeof(Boolean), (ushort)PrimitiveTypes.Boolean);
			known.Add(typeof(Char), (ushort)PrimitiveTypes.Char);
			known.Add(typeof(Double), (ushort)PrimitiveTypes.Double);
			known.Add(typeof(Single), (ushort)PrimitiveTypes.Single);
			known.Add(typeof(Decimal), (ushort)PrimitiveTypes.Decimal);
			known.Add(typeof(DateTime), (ushort)PrimitiveTypes.DateTime);
			known.Add(typeof(Guid), (ushort)PrimitiveTypes.Guid);
			known.Add(typeof(OID), (ushort)PrimitiveTypes.OID);
			//known.Add(typeof(Money), (ushort)PrimitiveTypes.Money);
			//known.Add(typeof(Amount), (ushort)18);
			//known.Add(typeof(MoneyPrecise), (ushort)PrimitiveTypes.MoneyPrecise);

			known.Add(typeof(String), (ushort)InfraTypes.String);   //25
			known.Add(typeof(string[]), (ushort)InfraTypes.StringArr);
			known.Add(typeof(Type), (ushort)InfraTypes.Type);
			known.Add(typeof(object[]), (ushort)InfraTypes.ObjectArr); //28
			known.Add(typeof(Array), 29);
			known.Add(typeof(KeyValuePair<,>), (ushort)InfraTypes.KeyValuePair);
			known.Add(typeof(List<>), 31);
			known.Add(typeof(Dictionary<,>), 32);
			known.Add(typeof(Nullable<>), 33);
			known.Add(typeof(Object), 34);
			known.Add(typeof(SerializationInfo), 35);
			known.Add(typeof(PList<>), 36);
			known.Add(typeof(PDictionary<,>), 37);
			known.Add(typeof(PulsarQuery), 38);
			known.Add(typeof(PulsarAnswer), 39);
			//known.Add(typeof(PulsarAnswerStatus), 40);
			//known.Add(typeof(PulsarQueryParams), 41);
			known.Add(typeof(ParamsDic), 42);
			known.Add(typeof(RefString), (ushort)InfraTypes.RefString);

			known.Add(typeof(GOLObjectRegistrator), (ushort)InfraTypes.GOLObjectRegistrator);  //46
			known.Add(typeof(PulsarPrimitiveHolder), (ushort)InfraTypes.PulsarPrimitiveHolder);
			known.Add(typeof(GOLObjectStub), (ushort)InfraTypes.GOLObjectStub);
			known.Add(typeof(PulsarEmptyStub), (ushort)InfraTypes.PulsarEmptyStub); // 49

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Public Methods >>
		/// <summary>
		/// Сериализует объект в поток.
		/// </summary>
		/// <param name="obj">Сериализуемый объект.</param>
		/// <param name="pars">Параметры сериализации.</param>
		/// <returns>Поток сериализованного объекта.</returns>
		public static MemoryStream Serialize(object obj, PulsarSerializationParams pars)
		{
			MemoryStream fms = new MemoryStream();
			PulsarSerializer.Serialize(fms, obj, pars);
			fms.Position = 0;
			return fms;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Клонирует объект через сериализацию и десериализацию.
		/// </summary>
		/// <param name="obj">Клонируемый объект.</param>
		/// <returns></returns>
		public static object CloneObject(object obj)
		{
			return CloneObject(obj, null);
		}
		/// <summary>
		/// Клонирует объект через "глубокую" (GOL объекты как обычные объекты) сериализацию и десериализацию.
		/// </summary>
		/// <param name="obj">Клонируемый объект.</param>
		/// <returns></returns>
		public static object DeepCloneObject(object obj)
		{
			return CloneObject(obj, new PulsarSerializationParams()
				{
					Mode = PulsarSerializationMode.Backup,
					NoStubObjects = new object[] { obj },
					Options =  PulsarSerializationOptions.DeepSerialization
				});
		}
		/// <summary>
		/// Клонирует объект через сериализацию и десериализацию.
		/// </summary>
		/// <param name="obj">Клонируемый объект.</param>
		/// <param name="pars">Параметры сериализации.</param>
		/// <returns></returns>
		public static object CloneObject(object obj, PulsarSerializationParams pars)
		{
			if (pars == null)
				pars = new PulsarSerializationParams()
				{
					Mode = PulsarSerializationMode.Backup,
					NoStubObjects = new object[] { obj }
				};
			using (MemoryStream ms = new MemoryStream())
			{
				PulsarSerializer.Serialize(ms, obj, pars);
				ms.Position = 0;
				return Deserialize(ms);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает бэкап объекта.
		/// </summary>
		/// <param name="obj">Сериализуемый объект.</param>
		/// <returns></returns>
		public static MemoryStream Backup(object obj)
		{
			PulsarSerializationParams pars = new PulsarSerializationParams()
			{
				Mode = PulsarSerializationMode.Backup,
				NoStubObjects = new object[] { obj }
			};
			MemoryStream ms = new MemoryStream();
			PulsarSerializer.Serialize(ms, obj, pars);
			ms.Position = 0;
			return ms;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Преобразует Decimal в массив байт
		/// </summary>
		/// <param name="val">Значение типа Decimal</param>
		/// <returns></returns>
		public static byte[] DecimalToBytes(Decimal val)
		{
			int[] bits = Decimal.GetBits(val);
			byte[] buf = new byte[16];
			buf[0] = (byte)bits[0];
			buf[1] = (byte)(bits[0] >> 8);
			buf[2] = (byte)(bits[0] >> 0x10);
			buf[3] = (byte)(bits[0] >> 0x18);
			buf[4] = (byte)bits[1];
			buf[5] = (byte)(bits[1] >> 8);
			buf[6] = (byte)(bits[1] >> 0x10);
			buf[7] = (byte)(bits[1] >> 0x18);
			buf[8] = (byte)bits[2];
			buf[9] = (byte)(bits[2] >> 8);
			buf[10] = (byte)(bits[2] >> 0x10);
			buf[11] = (byte)(bits[2] >> 0x18);
			buf[12] = (byte)bits[3];
			buf[13] = (byte)(bits[3] >> 8);
			buf[14] = (byte)(bits[3] >> 0x10);
			buf[15] = (byte)(bits[3] >> 0x18);
			return buf;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Преобразует массив байт в Decimal
		/// </summary>
		/// <param name="buf">Массив байт</param>
		/// <returns></returns>
		public static Decimal DecimalFromBytes(byte[] buf)
		{
			int[] bits = new int[4];
			bits[0] = ((buf[0] | (buf[1] << 8)) | (buf[2] << 0x10)) | (buf[3] << 0x18);
			bits[1] = ((buf[4] | (buf[5] << 8)) | (buf[6] << 0x10)) | (buf[7] << 0x18);
			bits[2] = ((buf[8] | (buf[9] << 8)) | (buf[10] << 0x10)) | (buf[11] << 0x18);
			bits[3] = ((buf[12] | (buf[13] << 8)) | (buf[14] << 0x10)) | (buf[15] << 0x18);
			return new Decimal(bits);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает неинициализированный (конструкторы не вызываются) объект.
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		/// <returns></returns>
		public static T CreateUninitializedObject<T>()
		{
			return (T)FormatterServices.GetUninitializedObject(typeof(T));
		}
		/// <summary>
		/// Создает неинициализированный (конструкторы не вызываются) объект.
		/// </summary>
		/// <param name="type">Тип объекта</param>
		/// <returns></returns>
		public static object CreateUninitializedObject(Type type)
		{
			return FormatterServices.GetUninitializedObject(type);
		}
		#endregion << Public Methods >>
		//-------------------------------------------------------------------------------------
		#region << Utils Methods >>
		/// <summary>
		/// Преобразует значение в массив байт.
		/// Умеет преобразовывать примитивы, строки и перечисления
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static byte[] ToBytes(object val)
		{
			if (val == null)
				return new byte[1];
			if (val is int) return BitConverter.GetBytes((int)val);
			else if (val is byte) return new[] { (byte)val };
			else if (val is String) return UTF8Encoding.UTF8.GetBytes((string)val);
			else if (val is RefString) return UTF8Encoding.UTF8.GetBytes(((RefString)val).Value);
			else if (val is Decimal) return DecimalToBytes((decimal)val);
			else if (val is bool) return BitConverter.GetBytes((bool)val);
			else if (val.GetType().IsEnum) return ToBytes(Convert.ChangeType(val, val.GetType().GetEnumUnderlyingType()));
			else if (val is Guid) return ((Guid)val).ToByteArray();
			else if (val is OID) return ((Guid)(OID)val).ToByteArray();
			else if (val is DateTime) return BitConverter.GetBytes(((DateTime)val).ToBinary());
			else if (val is sbyte) return new[] { (byte)val };
			else if (val is short) return BitConverter.GetBytes((short)val);
			else if (val is uint) return BitConverter.GetBytes((uint)val);
			else if (val is ushort) return BitConverter.GetBytes((ushort)val);
			else if (val is long) return BitConverter.GetBytes((long)val);
			else if (val is ulong) return BitConverter.GetBytes((ulong)val);
			else if (val is char) return BitConverter.GetBytes((char)val);
			else if (val is float) return BitConverter.GetBytes((float)val);
			else if (val is double) return BitConverter.GetBytes((double)val);
			else if (val is IntPtr) return BitConverter.GetBytes(IntPtr.Size == 4 ? (int)val : (long)val);
			else if (val is UIntPtr) return BitConverter.GetBytes(UIntPtr.Size == 4 ? (uint)val : (ulong)val);
			else
				throw new Exception(String.Format("Тип [{0}] не может быть обработан!", val.GetType()));
		}
		//-------------------------------------------------------------------------------------
		internal static object FromBytes(Type t, byte[] bs)
		{
			PrimitiveInfo pi = null;
			if (primitives.TryGetValue(t.TypeHandle.Value, out pi))
				return pi.FromBytes(bs);

			if (t.IsEnum) return Enum.ToObject(t, FromBytes(t.GetEnumUnderlyingType(), bs));
			else if (t == typeof(String))
				return (bs.Length == 1 && bs[0] == 0) ? null : UTF8Encoding.UTF8.GetString(bs);
			else if (t == typeof(RefString))
				return (bs.Length == 1 && bs[0] == 0) ? null : new RefString(UTF8Encoding.UTF8.GetString(bs));
			else
				throw new Exception(String.Format("Тип [{0}] не может быть обработан!", t));
		}
		//-------------------------------------------------------------------------------------
		internal static bool IsDefault(Type type, object val)
		{
			if (val == null)
				return true;
			if (type.IsValueType == false)
				return false;
			if (type == typeof(DateTime))
				return (DateTime)val == DateTime.MinValue;
			if (type == typeof(Guid))
				return (Guid)val == Guid.Empty;
			if(type == typeof(OID))
				return (OID)val == null;
			if (type == typeof(bool))
				return (bool)val == false;
			//if(type == typeof(Amount))
			// return (Amount)val == 0;
			//if(type == typeof(Money))
			// return (Money)val == 0m;
			//if(type == typeof(MoneyPrecise))
			// return (MoneyPrecise)val == 0m;
			if (type.IsPrimitive)
				return val.Equals(0);
			return false;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static bool IsPrimitive(Type t)
		{
			return primitives.ContainsKey(t.TypeHandle.Value) || t.IsEnum;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static int SizeOfPrimitive(Type type)
		{
			PrimitiveInfo pi;
			if (type.IsEnum)
				type = type.GetEnumUnderlyingType();
			if (primitives.TryGetValue(type.TypeHandle.Value, out pi))
				return pi.Size;
			throw new PulsarException("Тип [{0}] не примитивный!", type.ToString());
		}
		#endregion << Utils Methods >>
		//-------------------------------------------------------------------------------------
		//*************************************************************************************
		#region << Classes >>
		private class SerContext
		{
			internal Stream stream = null;
			internal SerDictionary objs = new SerDictionary();
			internal SerTypeDictionary types = null;
			internal Stack stack = new Stack(500);
			internal Dictionary<Type, MethodInfo[][]> serEvents = new Dictionary<Type, MethodInfo[][]>();
			internal HashSet<Type> noStubTypes = null;
			internal HashSet<object> noStubObjects = null;
			internal HashSet<Type> asEmptyTypes = null;
			internal HashSet<object> asEmptyObjects = null;
			internal HashSet<Type> byDemandTypes = null;
			internal PulsarSerializationMode mode;
			internal PulsarSerializationOptions opts;

			public SerContext(Stream stream, PulsarSerializationParams pars)
			{
				if (pars == null)
					pars = new PulsarSerializationParams();
				this.stream = stream;
				this.types = new SerTypeDictionary(this);
				this.noStubTypes =
				 pars.NoStubTypes is HashSet<Type> ? (HashSet<Type>)pars.NoStubTypes :
													  new HashSet<Type>(pars.NoStubTypes ?? Type.EmptyTypes);

				this.noStubObjects =
				 pars.NoStubObjects is HashSet<object> ? (HashSet<object>)pars.NoStubObjects :
														 new HashSet<object>(pars.NoStubObjects ?? new object[] { });

				this.asEmptyTypes =
				 pars.AsEmptyTypes is HashSet<Type> ? (HashSet<Type>)pars.AsEmptyTypes :
													  new HashSet<Type>(pars.AsEmptyTypes ?? Type.EmptyTypes);

				this.asEmptyObjects =
				 pars.AsEmptyObjects is HashSet<object> ? (HashSet<object>)pars.AsEmptyObjects :
														 new HashSet<object>(pars.AsEmptyObjects ?? new object[] { });
				this.mode = pars.Mode;
				this.opts = pars.Options;
				this.byDemandTypes =
				 pars.ByDemandTypes is HashSet<Type> ? (HashSet<Type>)pars.ByDemandTypes :
													  new HashSet<Type>(pars.ByDemandTypes ?? Type.EmptyTypes);

			}
		}
		//*************************************************************************************
		internal class DeserContext
		{
			internal Stream stream = null;
			internal DeserTypeDictionary types = new DeserTypeDictionary();
			internal Dictionary<uint, DeserObj> objs = new Dictionary<uint, DeserObj>(5000);
			internal Dictionary<uint, List<LateDeserInfo>> lateSet = new Dictionary<uint, List<LateDeserInfo>>(1000);
			internal HashSet<uint> lateLoad = new HashSet<uint>();

			public DeserContext(Stream stream)
			{
				this.stream = stream;
			}
		}
		//*************************************************************************************
		private class SerTypeDictionary : Dictionary<IntPtr, ushort>
		{
			private HashSet<Assembly> assms = new HashSet<Assembly>();
			private SerContext cox = null;
			private ushort id = 49;
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			public SerTypeDictionary(SerContext cox)
			{
				foreach (KeyValuePair<Type, ushort> i in known)
					this.Add(i.Key.TypeHandle.Value, i.Value);
				this.cox = cox;
				assms.Add(Assembly.GetExecutingAssembly());
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
			public ushort GetTypeID(Type t)
			{
				ushort i = 0;
				if (this.TryGetValue(t.TypeHandle.Value, out i))
					return i;

				if (t.Assembly.FullName.StartsWith("mscorlib,") == false)
					if (assms.Contains(t.Assembly) == false)
					{
						SerTypeInfo sti = new SerTypeInfo();
						sti.name = t.Assembly.FullName;
						sti.value = 0;
						sti.Save(cox.stream);
						assms.Add(t.Assembly);
					}

				string s = "";
				if (t.IsGenericType && t.IsGenericTypeDefinition == false)
				{
					Type defType = t.GetGenericTypeDefinition();
					GetTypeID(defType);

					string args = "";
					foreach (Type at in t.GetGenericArguments())
						args += ",$" + GetTypeID(at).ToString() + '$';
					args = args.Remove(0, 1);
					s = String.Format("${0}$[{1}]", this[defType.TypeHandle.Value], args);
				}
				else if (t.IsArray && t.HasElementType)
				{
					Type elemType = t.GetElementType();
					ushort tid = GetTypeID(elemType);
					s = t.FullName.Replace(elemType.FullName, "$" + tid.ToString() + "$");
				}
				else
				{
					s = t.FullName;
				}
				if (id + 1 >= ushort.MaxValue)
					throw new SerializationException("Превышено максимальное число регистраций типов!");
				this.Add(t.TypeHandle.Value, ++id);

				if (cox.stream != null)
				{
					SerTypeInfo sti = new SerTypeInfo();
					sti.name = s;
					sti.value = id;
					sti.Save(cox.stream);
				}
				return id;
			}
		}
		//*************************************************************************************
		private class SerDictionary //: NoThrowDictionary<object, uint>//Dictionary<object, uint>
		{
			internal NoThrowDictionary<object, uint>[] dic =
			 new NoThrowDictionary<object, uint>[ushort.MaxValue];
			private uint id = 0;
			//-------------------------------------------------------------------------------------
			public uint GetObjID(ushort typeid, object obj)
			{
				if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
					obj = KeyValWrapper.Wrap(obj);

				if (dic[typeid] == null)
					dic[typeid] = new NoThrowDictionary<object, uint>(100);
				uint i = dic[typeid].Add(obj, id + 1);
				if (i == id + 1)
					id++;
				return i;
			}
			//-------------------------------------------------------------------------------------
			public bool AddAsNew(ushort typeid, object obj, out uint objID)
			{
				if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
					obj = KeyValWrapper.Wrap(obj);

				if (dic[typeid] == null)
					dic[typeid] = new NoThrowDictionary<object, uint>(100);
				objID = dic[typeid].Add(obj, id + 1);
				if (objID == id + 1)
				{
					id++;
					return true;
				}
				return false;
			}

		}
		//*************************************************************************************
		internal class DeserTypeDictionary : Dictionary<ushort, Type>
		{
			private HashSet<Assembly> userAsms = new HashSet<Assembly>();
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			public DeserTypeDictionary()
			{
				foreach (KeyValuePair<Type, ushort> i in known)
					this.Add(i.Value, i.Key);
				//Add(21,typeof(Guid));
			}
			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------
			public void Process(SerTypeInfo sti)
			{
				try
				{
					if(sti.value == 0)
					{
						string n = sti.name;
						if(n.Contains(','))
							n = n.Substring(0, n.IndexOf(','));

						if(n == "Pulsar.ServerTools")
							return;

						if(n == "Pulsar.Refs")
						 n = "Sim.Refs";
						else if(n == "Pulsar.CRM")
							n = "Sim.CRM";
						else if(n == "Pulsar.Connectors")
							n = "Sim.Connectors";
						else if(n == "Pulsar.Analytics")
							n = "Sim.Analytics";
						else if(n == "Pulsar.Server.Servants")
							n = "Sim.Servants";
						else if(n == "Pulsar.Analytics.Data")
							n = "Sim.Analytics.Data";

						foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
						{
							string an = asm.FullName;
							if(an.Substring(0, an.IndexOf(',')) == n)
							{
								if(userAsms.Contains(asm) == false)
									userAsms.Add(asm);
								return;
							}
						}
						Assembly a = Assembly.Load(sti.name);
						return;
					}

					string s = sti.name;
					if(s.StartsWith("Pulsar.Refs."))
						s = s.Replace("Pulsar.Refs.", "Sim.Refs.");
					else if(s.StartsWith("Pulsar.CRM."))
						s = s.Replace("Pulsar.CRM.", "Sim.CRM.");
					else if(s.StartsWith("Pulsar.Connectors."))
						s = s.Replace("Pulsar.Connectors.", "Sim.Connectors.");
					else if(s.StartsWith("Pulsar.Analytics."))
						s = s.Replace("Pulsar.Analytics.", "Sim.Analytics.");
					else if(s == "Pulsar.Server.ResourceManagerServant")
						s = "Sim.Servants.ResourceManagerServant";
					else if(s.StartsWith("Pulsar.Scheduler."))
						s = s.Replace("Pulsar.Scheduler.", "Pulsar.");

					//if(s == "Pulsar.CRM.CRM_Root")
					// s = "Pulsar.CRM.CRMRoot";
					//if(s == "Pulsar.Shared.StockList")
					// s = "Pulsar.Refs.StockList";
					//if(s.StartsWith("Pulsar.Refs.StockList."))
					// s = s.Replace("Pulsar.Refs.StockList.", "Pulsar.Refs.");
					//if(s == "Pulsar.Refs.StockPropertyUsers")
					// s = "Pulsar.Refs.StockPropertyPersons";
					//else if(s == "Pulsar.Refs.RestStore.StockPart")
					// s = "Pulsar.Refs.StockPart";
					//else if(s == "Pulsar.Refs.RestStore.StockPartsList")
					// s = "Pulsar.Refs.StockPartsList";
					//else if(s == "Pulsar.Refs.RestStore.PackAmount")
					// s = "Pulsar.PackAmount";
					//else if(s.StartsWith("Pulsar.Refs.RestStore."))
					// s = s.Replace("Pulsar.Refs.RestStore.", "Pulsar.WMS.");
					//else if(s == "Pulsar.Docs.DocOperInternalIn")
					// s = "Pulsar.WMS.Docs.DocOperInternalIn";
					//else if(s == "Pulsar.Docs.DocLineStorePlacement")
					// s = "Pulsar.WMS.Docs.DocLineStorePlacement";
					// !!!

					//if(s == "Pulsar.Refs.StockPropertyListResource")
					// s = "Pulsar.Refs.StockProperty";
					//else if(s == "Pulsar.Refs.StockPropertyValueListResource")
					// s = "Pulsar.Refs.StockProperty";
					//else if(s == "Pulsar.Refs.StockPropertyValueList")
					// s = "Pulsar.Refs.StockProperty";

					int pos = s.IndexOf('$'), pos1;
					while(pos != -1)
					{
						pos1 = pos;
						pos = s.IndexOf('$', pos1 + 1);
						string r = s.Substring(pos1, pos - pos1 + 1);
						ushort typeid = UInt16.Parse(r.Replace("$", ""));
						
						// Заглушка оида
						if(typeid == 16)
						 typeid = 21;


						if(pos1 == 0)
							s = s.Replace(r, this[typeid].FullName);// + ", " + this[typeid].Assembly.FullName;
						else
							s = s.Replace(r, "[" + this[typeid].AssemblyQualifiedName + "]");
						pos = s.IndexOf('$');
					}

					//if(s.Contains("System.Guid"))
					// if(s.Contains("System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]"))
					//  s = s.Replace("System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]","e089],[Pulsar.Security.SecurityGroup, Pulsar.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]""
					// s = s.Replace("System.Guid", "Pulsar.OID");


					Type t = null;
					bool has = false;
					lock(deserTypes)
						has = deserTypes.TryGetValue(s, out t);
					if(has == false)
					{
						t = Type.GetType(s);
						if(t == null)
							foreach(Assembly asm in userAsms)
								if((t = asm.GetType(s)) != null)
									break;
						if(t == null)
							foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
								if((t = asm.GetType(s)) != null)
									break;
						if(t == null)
							throw new SerializationException(String.Format("Не удалось найти тип [{0}]!", s));
						lock(deserTypes)
							if(deserTypes.ContainsKey(s) == false)
								deserTypes.Add(s, t);
					}
					Add(sti.value, t);
				}
				catch
				{
					throw;
				}
			}
		}
		//*************************************************************************************
		/// <summary>
		/// Класс обертки для объекта при десериализации.
		/// </summary>
		internal class DeserObj
		{
			public object Obj = null;

			//private int waits = -1;
			/// <summary>
			/// -1 - объект еще не десериализовывался
			/// 0  - объект десериализован
			/// >0 - количество объектов, десериализации которых ожидает этот объект
			/// </summary>
			public int WaitsCount = -1; //{ get { return waits; } set {  }
			public bool IsEmpty = false;
			public object Tag = null;

			public DeserObj(Type t)
			{
				Obj = FormatterServices.GetUninitializedObject(t);
			}
			public DeserObj(Object obj)
			{
				Obj = obj;
			}
			public override string ToString()
			{
				return String.Format("{{{0},{1}}}", Obj ?? "", WaitsCount);
			}
		}
		//*************************************************************************************
		internal struct LateDeserInfo
		{
			public uint id;
			public FieldWrap fi;
			public Array array;
			public int[] index;
			public LateDeserInfo(uint id, FieldWrap fi)
			{
				array = null;
				index = null;
				this.id = id;
				this.fi = fi;
			}
			public LateDeserInfo(uint id, Array arr, int[] index)
			{
				this.id = id;
				fi = null;
				array = arr;
				this.index = index;
			}
		}
		//*************************************************************************************
		private struct KeyValWrapper
		{
			public object key;
			public object val;

			public KeyValWrapper(object k, object v)
			{
				key = k;
				val = v;
			}
			public static object Wrap(object kvp)
			{
				KeyValWrapper res = new KeyValWrapper();
				TypeSerializationWrap wt = TypeSerializationWrap.GetTypeSerializationWrap(kvp.GetType());

				foreach (FieldWrap fw in wt.Fields)
					if (fw.Name.Length == 3)
						res.key = fw.Get(kvp);
					else
						res.val = fw.Get(kvp);

				//res.key = kvp.GetType().GetProperty("Key", BindingFlags.Public|BindingFlags.Instance).GetValue(kvp, null);
				//res.val = kvp.GetType().GetProperty("Value", BindingFlags.Public|BindingFlags.Instance).GetValue(kvp, null);
				return res;
			}
			public override int GetHashCode()
			{
				int hash = 0;
				if (Object.Equals(key, null) == false)
					hash = key.GetHashCode();
				if (Object.Equals(val, null) == false)
					hash ^= val.GetHashCode();
				return hash;
			}
			public override bool Equals(object obj)
			{
				if (obj == null || this.GetType() != obj.GetType())
					return false;
				return Object.Equals(key, ((KeyValWrapper)obj).key) && Object.Equals(val, ((KeyValWrapper)obj).val);
			}
		}
		//*************************************************************************************
		private class PrimitiveInfo
		{
			public int Size = 0;
			public Func<byte[], object> FromBytes = null;
			public Func<object, byte[]> ToBytes = null;
		}
		#endregion << Classes >>

	}
}
