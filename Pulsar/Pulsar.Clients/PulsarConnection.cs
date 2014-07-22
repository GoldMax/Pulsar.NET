using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Pulsar;
using Pulsar.Clients;
using Pulsar.Serialization;

//using OID = System.Guid;

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Finist")]

namespace Pulsar
{
	//**************************************************************************************
	/// <summary>
	/// Класс взаимодействия с сервером Пульсара.
	/// </summary>
	public class PulsarConnection	: Pulsar.Server.ServerParamsBase
	{
		private static HashSet<OID> essLoading = new HashSet<OID>();

		private string address = "10.0.0.123";
		private static int type = 1;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		/// <summary>
		/// Событие, возникающее в начале выполнения запроса.
		/// </summary>
		public static event EventHandler DataAccessBegin;
		internal static void OnDataAccessBegin()
		{
			if (DataAccessBegin != null)
				DataAccessBegin(null, EventArgs.Empty);
		}
		/// <summary>
		/// Событие, возникающее по окончании выполнения запроса.
		/// </summary>
		public static event EventHandler DataAccessEnd;
		internal static void OnDataAccessEnd()
		{
			if (DataAccessEnd != null)
				DataAccessEnd(null, EventArgs.Empty);
		}
		#endregion << Events >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Соединение с сервером по умолчанию.
		/// </summary>
		public static PulsarConnection Default { get; set; }
		/// <summary>
		/// Кэш загруженных именованных объектов Пульсара.
		/// </summary>
		public Dictionary<string, object> NamedObjectsCache { get; private set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// IP адрес или имя сервера.
		/// </summary>
		public string Address
		{
			get { return address; }
			set { address = value; }
		}
		/// <summary>
		/// Тип соединения.
		/// </summary>
		public int ConnectionType
		{
			get { return type; }
			set { type = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarConnection()
		{
			//PulsarSerializer.IsPulsarServer = false;
			//!! GOL.LateInitMode = GOLLateInitModes.AllowEssences;
			GOL.LoadGlobalObjectsMethod = LoadEssences;
			NamedObjectsCache = new Dictionary<string, object>();


			try
			{
				ReceiveTimeOut = (int)GetParam("Connection", "ReceiveTimeOut", 50000);
				ReceiveBufferSize = (int)GetParam("Connection", "ReceiveBufSize", 8192);
				SendTimeOut = (int)GetParam("Connection", "SendTimeOut", 50000);
				SendBufferSize = (int)GetParam("Connection", "SendBufSize", 8192);
				type = (int)GetParam("Connection", "Type", 1);
				Tuple<string, int> add = GetServerAddress(type);
				Address = add.Item1;
				Port = add.Item2;
			}
			catch
			{
				throw new Exception("Ошибка при работе с реестром!");
			}
		}
		static PulsarConnection()
		{
			Default = new PulsarConnection();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод докачки сущностей с сервера Пульсара.
		/// </summary>
		/// <param name="needs"></param>
		private void LoadEssences(IEnumerable<GlobalObject> needs)
		{
			List<GlobalObject> need;
			if(needs is List<GlobalObject>)
				need = (List<GlobalObject>)needs;
			else
			 need = new List<GlobalObject>(needs);

			List<GlobalObject> toLoad = new List<GlobalObject>();
			lock(essLoading)
				foreach(GlobalObject go in need)
				 if(essLoading.Contains(go.OID) == false && go.IsInitialized == false)
					{
						toLoad.Add(go);
						essLoading.Add(go.OID);
					}
				
			if(toLoad.Count > 0)
			{
				toLoad.TrimExcess();
				Query(new PulsarQuery("Pulsar", "ReadFullGOs", new { ReadFullGOs = toLoad }));
			}
			int wait = 0;
			int a = 0;
			while(wait < 120000)
			{
				for(; a < need.Count; a++)
					if(need[a].IsInitialized == false)
					 break;
				if(a == need.Count)
					break;
				Thread.Sleep(500);
				wait += 500;
			}
			if(wait >= 120000)
				throw new PulsarException("Не удалось загрузить глобальные объекты за отведенный период!");
		}
		//-------------------------------------------------------------------------------------
		#region << Override Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}:{1}", Address == null ? "0.0.0.0" : Address, Port);
		}
		#endregion << Override Methods >>
		#region << Pulsar Methods >>
		/// <summary>
		/// Возвращает адрес сервера Пульсара.
		/// </summary>
		/// <returns></returns>
		public Tuple<string, int> GetServerAddress(int type)
		{
			object addr = null;
			int port = 5022;
			//if(type == 2)
			// addr = "10.0.0.123";
			if(type == 3)
			{
			 addr	= GetParam("Connection", "Address", null);
				port = (int)GetParam("Connection", "Port", 5022);
			}
			if(addr == null)
			{
				IPAddress[] ips = Dns.GetHostAddresses("");
				if(ips.Length == 0)
					throw new Exception("Не удалось определить список IP адресов!");
				byte[] parts;
				//--- Проверка на хосты УК ---
				foreach(IPAddress ip in ips)
				{
					if(ip.AddressFamily != AddressFamily.InterNetwork)
						continue;
					parts = ip.GetAddressBytes();
					if(parts[0] == 10 && parts[1] < 120)   //(parts[0] == 192 && parts[1] == 168) || >= 100
						addr = "10.0.0.123";
					else if((parts[0] == 10 && parts[1] == 120) || (parts[0] == 192 && parts[1] == 168))
						addr = "10.120.0.244";
				}
			}
			if(addr != null)
				address = (string)addr;
			return new Tuple<string, int>(address, port);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод запроса к серверу Пульсара.
		/// </summary>
		/// <param name="query">Объект запроса.</param>
		/// <param name="noStub">Типы объектов, которые не должны быть заглушены при отправке запроса.</param>
		/// <param name="noSer">Типы объектов, которые не должны быть сериализованы при отправке запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public PulsarAnswer Query(PulsarQuery query, IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null,
										Action<PulsarAnswer> messageHandler = null)
		{
			PulsarSerializationParams pars = new PulsarSerializationParams();
			pars.NoStubTypes = noStub;
			pars.AsEmptyTypes = noSer;
			pars.Mode = PulsarSerializationMode.ForClient;
			return Query(query, pars, messageHandler);
		}
		/// <summary>
		/// Метод запроса к серверу Пульсара.
		/// </summary>
		/// <param name="query">Объект запроса.</param>
		/// <param name="pars">Параметры сериализации запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public PulsarAnswer Query(PulsarQuery query, PulsarSerializationParams pars,
										Action<PulsarAnswer> messageHandler = null)
		{
			TcpClient cl = null;
			Stream ns = null;
			try
			{
				OnDataAccessBegin();

				if(query == null)
					throw new ArgumentNullException("query");

				cl = new TcpClient();
				cl.ReceiveTimeout = ReceiveTimeOut;
				cl.ReceiveBufferSize = ReceiveBufferSize;
				cl.SendTimeout = SendTimeOut;
				cl.SendBufferSize = SendBufferSize;
				cl.Connect(Address.ToString(), Port);
				ns = cl.GetStream();

				ns.WriteByte((byte)QueryType.Object);

				if(messageHandler != null)
					query.Params |= PulsarQueryParams.Verbose;

				if(pars == null)
					pars = new PulsarSerializationParams() { Mode = PulsarSerializationMode.ForClient };
				using(MemoryStream temp = PulsarSerializer.Serialize(query, pars))
				{
					byte[] buf = new byte[cl.SendBufferSize];
					int bytes;
					while((bytes = temp.Read(buf, 0, buf.Length)) != 0)
						ns.Write(buf, 0, bytes);
				}

				PulsarAnswer answer = null;
				do
				{
					answer = (PulsarAnswer)PulsarSerializer.Deserialize(ns);

					if(answer.Answer == PulsarAnswerStatus.Error)
						throw new PulsarServerException((answer.Return ?? "").ToString());
					if(answer.Answer == PulsarAnswerStatus.TestMessage || answer.Answer == PulsarAnswerStatus.ErrorMessage)
					{
						if(messageHandler != null)
							messageHandler(answer);
						continue;
					}
					break;
				} while(true);
				return answer;
			}
			catch
			{
				throw;
			}
			finally
			{
				if(ns != null)
					ns.Close();
				if(cl != null)
					cl.Close();
				OnDataAccessEnd();
			}
		}

		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Получает объект Пульсара.
		/// </summary>
		/// <param name="query">Объект запроса.</param>
		/// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше</param>
		/// <returns></returns>
		public object Get(PulsarQuery query, string cacheName)
		{
			return Get(query, null, null, cacheName);
		}
		/// <summary>
		/// Получает объект Пульсара.
		/// </summary>
		/// <param name="query">Объект запроса.</param>
		/// <param name="noStub">Типы объектов, которые не должны быть заглушены при отправке запроса.</param>
		/// <param name="noSer">Типы объектов, которые не должны быть сериализованы при отправке запроса.</param>
		/// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше.</param>
		/// <returns></returns>
		public object Get(PulsarQuery query, IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null,
										string cacheName = null)
		{
			Mutex mut = null;
			try
			{
				if(cacheName != null)
				{
					lock(NamedObjectsCache)
					{
						if(NamedObjectsCache.ContainsKey(cacheName))
						{
							if(NamedObjectsCache[cacheName] is Mutex)
								mut = (Mutex)NamedObjectsCache[cacheName];
							else
								return NamedObjectsCache[cacheName];
						}
						else
							NamedObjectsCache.Add(cacheName, (mut = new Mutex()));
					}
					if(mut != null)
						mut.WaitOne();
					lock(NamedObjectsCache)
					{
						if(NamedObjectsCache.ContainsKey(cacheName) && NamedObjectsCache[cacheName] is Mutex == false)
							return NamedObjectsCache[cacheName];
					}
				}

				PulsarAnswer answer = Query(query, noStub, noSer);

				if(answer.Return == null)
					throw new PulsarServerException("Выполнив запрос \"{0}\" к объекту [{1}] сервер не передал результат!",
							query.Query, query.RootObject);

				if(cacheName != null)
					lock(NamedObjectsCache)
					{
						if(NamedObjectsCache.ContainsKey(cacheName))
							NamedObjectsCache[cacheName] = answer.Return;
						else if(cacheName != null)
							NamedObjectsCache.Add(cacheName, answer.Return);
					}
				return answer.Return;
			}
			finally
			{
				if(mut != null)
					mut.ReleaseMutex();
			}
		}
		/// <summary>
		/// Получает объект Пульсара.
		/// </summary>
		/// <param name="objName">Имя объекта Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше.</param>
		/// <returns></returns>
		public object Get(object objName, string query = null, string cacheName = null)
		{
			return Get(new PulsarQuery(objName, query), cacheName: cacheName);
		}
		/// <summary>
		/// Получает объект Пульсара.
		/// </summary>
		/// <param name="objName">Имя объекта Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="pars">Параметры запроса.</param>
		/// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше.</param>
		/// <returns></returns>
		public object Get(object objName, string query, PulsarQueryParams pars, string cacheName = null)
		{
			return Get(new PulsarQuery(objName, query, null, pars), cacheName: cacheName);
		}
		/// <summary>
		/// Получает объект Пульсара.
		/// </summary>
		/// <param name="objName">Имя объекта Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="args">Аргументы запроса.</param>
		/// <param name="pars">Параметры запроса.</param>
		/// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше.</param>
		/// <returns></returns>
		public object Get(object objName, string query, object args,
									PulsarQueryParams pars = PulsarQueryParams.None, string cacheName = null)
		{
			return Get(new PulsarQuery(objName, query, args, pars), cacheName: cacheName);
		}
		/// <summary>
		/// Получает объект Пульсара.
		/// </summary>
		/// <param name="objName">Имя объекта Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="noStub">Типы объектов, которые не должны быть заглушены при отправке запроса.</param>
		/// <param name="noSer">Типы объектов, которые не должны быть сериализованы при отправке запроса.</param>
		/// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше.</param>
		/// <returns></returns>
		public object Get(object objName, string query, IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null,
										string cacheName = null)
		{
			return Get(new PulsarQuery(objName, query, null), noStub, noSer, cacheName);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Выполняет запрос на Пульсаре.
		/// </summary>
		/// <param name="objName">Имя объекта Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="args">Аргументы запроса.</param>
		/// <param name="pars">Параметры запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Exec(object objName, string query = null, object args = null,
							PulsarQueryParams pars = PulsarQueryParams.None, Action<PulsarAnswer> messageHandler = null)
		{
			PulsarQuery q = new PulsarQuery(objName, query, args, pars);
			return Query(q, messageHandler: messageHandler).Return;
		}
		/// <summary>
		/// Выполняет запрос на Пульсаре.
		/// </summary>
		/// <param name="query">Запрос.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Exec(PulsarQuery query, Action<PulsarAnswer> messageHandler = null)
		{
			return Query(query, messageHandler: messageHandler).Return;
		}
		/// <summary>
		/// Выполняет запрос на Пульсаре.
		/// </summary>
		/// <param name="query">Запрос.</param>
		/// <param name="noStub">Типы объектов, которые не должны быть заглушены при отправке запроса.</param>
		/// <param name="noSer">Типы объектов, которые не должны быть сериализованы при отправке запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Exec(PulsarQuery query, Type[] noStub, Type[] noSer = null,
														Action<PulsarAnswer> messageHandler = null)
		{
			return Query(query, noStub, noSer, messageHandler: messageHandler).Return;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Выполняет запрос на Пульсаре (с модицикацией).
		/// </summary>
		/// <param name="objName">Имя объекта Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="args">Аргументы запроса.</param>
		/// <param name="pars">Параметры запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Modify(object objName, string query = null, object args = null,
							PulsarQueryParams pars = PulsarQueryParams.None, Action<PulsarAnswer> messageHandler = null)
		{
			PulsarQuery q = new PulsarQuery(objName, query, args, pars);
			q.Params |= PulsarQueryParams.Modify;
			return Query(q, messageHandler: messageHandler).Return;
		}
		/// <summary>
		/// Выполняет запрос на Пульсаре (с модицикацией).
		/// </summary>
		/// <param name="query">Запрос.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Modify(PulsarQuery query, Action<PulsarAnswer> messageHandler = null)
		{
			query.Params |= PulsarQueryParams.Modify;
			return Query(query, messageHandler: messageHandler).Return;
		}
		/// <summary>
		/// Выполняет запрос на Пульсаре (с модицикацией).
		/// </summary>
		/// <param name="query">Запрос.</param>
		/// <param name="noStub">Типы объектов, которые не должны быть заглушены при отправке запроса.</param>
		/// <param name="noSer">Типы объектов, которые не должны быть сериализованы при отправке запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Modify(PulsarQuery query, Type[] noStub, Type[] noSer = null,
														Action<PulsarAnswer> messageHandler = null)
		{
			query.Params |= PulsarQueryParams.Modify;
			return Query(query, noStub, noSer, messageHandler: messageHandler).Return;
		}
		/// <summary>
		/// Выполняет запрос на Пульсаре (с модицикацией).
		/// </summary>
		/// <param name="query">Запрос.</param>
		/// <param name="pars">Параметры сериализации запроса.</param>
		/// <param name="messageHandler">Делегат метода, вызываемый для обработки сообщений сервера.</param>
		/// <returns></returns>
		public object Modify(PulsarQuery query, PulsarSerializationParams pars, Action<PulsarAnswer> messageHandler = null)
		{
			query.Params |= PulsarQueryParams.Modify;
			pars.Mode = PulsarSerializationMode.ForClient;
			return Query(query, pars, messageHandler: messageHandler).Return;
		}
		#endregion << Pulsar Methods >>
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		//public static 
		#endregion << Static Methods >>
	}
	//**************************************************************************************

}
