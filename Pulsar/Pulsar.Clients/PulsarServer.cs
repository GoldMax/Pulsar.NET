using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
//using System.Threading;

using Pulsar;
using Pulsar.Clients;
using Pulsar.Serialization;

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Finist")]

namespace Pulsar
{
 //**************************************************************************************
 /// <summary>
 /// Класс информации о сервере Пульсара.
 /// </summary>
 public static class PulsarServer 
 {
  private static HashSet<OID> essLoading = new HashSet<OID>();
  
  private static string address = "10.0.0.122";
  private static int port = 5022;
  private static int receiveTimeout = 5;
  private static int sendTimeout = 5;
  private static int receiveBuf = 8192;
  private static int sendBuf = 8192;
  private static int type = 2;
  private static byte traceLevel = 0;
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  #region << Events >>
  /// <summary>
  /// Событие, возникающее в начале выполнения запроса.
  /// </summary>
  public static event EventHandler DataAccessBegin;
  internal static void OnDataAccessBegin()
  {
   if(DataAccessBegin != null)
    DataAccessBegin(null, EventArgs.Empty);
  }
  /// <summary>
  /// Событие, возникающее по окончании выполнения запроса.
  /// </summary>
  public static event EventHandler DataAccessEnd;
  internal static void OnDataAccessEnd()
  {
   if(DataAccessEnd != null)
    DataAccessEnd(null, EventArgs.Empty);
  }
  #endregion << Events >>
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// IP адрес сервера.
  /// </summary>
  public static string Address 
  {
   get { return address;}
   set { address = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Порт
  /// </summary>
  public static int Port
  {
   get { return port; }
   set { port = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Тайм-аут получения, сек.
  /// </summary>
  public static int ReceiveTimeOut
  {
   get { return receiveTimeout; }
   set { receiveTimeout = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Тайм-аут отправки, сек.
  /// </summary>
  public static int SendTimeOut
  {
   get { return sendTimeout; }
   set { sendTimeout = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Размер буфера получения.
  /// </summary>
  public static int ReceiveBufSize
  {
   get { return receiveBuf; }
   set { receiveBuf = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Размер буфера отправки.
  /// </summary>
  public static int SendBufSize
  {
   get { return sendBuf; }
   set { sendBuf = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Кэш загруженных объектов Пульсара.
  /// </summary>
  internal static Dictionary<string, object> ObjectsCache { get; set; }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Тип соединения.
  /// </summary>
  public static int ConnectionType
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
  static PulsarServer()
  {
   //PulsarSerializer.IsPulsarServer = false;
   GOL.LateInitMode = GOLLateInitModes.AllowEssences;
   ObjectsCache = new Dictionary<string,object>();

   try
   {
    receiveTimeout = (int)UserPreferences.GetParam("Connection", "ReceiveTimeOut", 5);
    receiveBuf = (int)UserPreferences.GetParam("Connection", "ReceiveBufSize", 8192);
    sendTimeout = (int)UserPreferences.GetParam("Connection", "SendTimeOut", 5);
    sendBuf = (int)UserPreferences.GetParam("Connection", "SendBufSize", 8192);
    type = (int)UserPreferences.GetParam("Connection", "Type", 1);
    traceLevel = (byte)(int)UserPreferences.GetParam("Debug", "TraceLevel", (int)0);
    Tuple<string,int> add = GetServerAddress(type);
    address = add.Item1;
    port = add.Item2;
   }
   catch
   {
    throw new Exception("Ошибка при работе с реестром!");
   }
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Pulsar Methods >>
  /// <summary>
  /// Возвращает адрес сервера Пульсара.
  /// </summary>
  /// <returns></returns>
  public static Tuple<string,int> GetServerAddress(int type)
  {
   if(type == 1 || type == 2)
    return new Tuple<string,int>("10.0.0.122", 5022);
   else 
    return new Tuple<string,int>((string)UserPreferences.GetParam("Connection", "Address", "127.0.0.1"),
                                 (int)UserPreferences.GetParam("Connection", "Port", 5021));
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
  public static PulsarAnswer Query(PulsarQuery query, IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null,
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
  public static PulsarAnswer Query(PulsarQuery query, PulsarSerializationParams pars,
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
    cl.ReceiveTimeout = ReceiveTimeOut * 1000;
    cl.ReceiveBufferSize = ReceiveBufSize ;
    cl.SendTimeout = SendTimeOut * 1000;
    cl.SendBufferSize = SendBufSize;
    cl.Connect(Address.ToString(), Port);
    ns = cl.GetStream();

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
    Stopwatch sw = null;
    if(traceLevel >= 3)
     sw = Stopwatch.StartNew();
    if(traceLevel >= 5)
    {
     ns = new DebugStream(ns, "c:\\temp");
     Debug.WriteLine(((DebugStream)ns).DebugFileName);
    }
     
    PulsarAnswer answer = null;
    do
    {
     answer = (PulsarAnswer)PulsarSerializer.Deserialize(ns, LoadEssences);

     if(answer.Answer == PulsarAnswerStatus.Error)
      throw new PulsarServerException((answer.Return ?? "").ToString());
     if(answer.Answer == PulsarAnswerStatus.Message || answer.Answer == PulsarAnswerStatus.ErrorMessage)
     {
      if(messageHandler != null)
       messageHandler(answer);
      continue;
     }
     break;
    } while(true);
    if(traceLevel >= 3)
    {
     sw.Stop();
     Debug.WriteLine("{0}: {1}", Thread.CurrentThread.Name,  sw.Elapsed);
     Debug.Flush();
    }
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
  /// Метод докачки сущностей с сервера Пульсара.
  /// </summary>
  /// <param name="need"></param>
  public static void LoadEssences(IEnumerable<OID> need)
  {
   List<OID> toLoad = null;
   lock(GOL.SyncRoot)
   {
    if(GOL.UninitializedCount == 0)
     return;
    toLoad = new List<OID>();
    foreach(OID i in need)
     if(GOL.IsUninitialized(i) && essLoading.Contains(i) == false)
     {
      toLoad.Add(i);
      essLoading.Add(i);
     }
   }
   List<OID> list = null;
   if(toLoad != null && toLoad.Count > 0)
   {
    list = new List<OID>(need.Except(toLoad));
    toLoad.TrimExcess();
    PulsarServer.Query(new PulsarQuery("Pulsar", "GetFromGOL", new { GetFromGOL = toLoad }, 
                       PulsarQueryParams.NonPublic|PulsarQueryParams.Servant));
   }
   else 
    list = new List<OID>(need);
   int wait = 0;
   while(wait < 20000)
   {
    for(int a = 0; a < list.Count; a++)
     lock(GOL.SyncRoot)
      if(GOL.IsInitialized(list[a]))
      {
       list.RemoveAt(a);
       a--;
      }
      else
       break;
    if(list.Count == 0)
     break;
    Thread.Sleep(500);
    wait += 500;
   }

   lock(GOL.SyncRoot)
   {
    foreach(OID o in toLoad)
     if(essLoading.Contains(o) && GOL.IsInitialized(o))
      essLoading.Remove(o);
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Получает объект Пульсара.
  /// </summary>
  /// <param name="query">Объект запроса.</param>
  /// <param name="cacheName">Имя, под которым результат будет взят и сохранен в кэше</param>
  /// <returns></returns>
  public static object Get(PulsarQuery query, string cacheName)
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
  public static object Get(PulsarQuery query, IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null, 
                            string cacheName = null)
  {
   Mutex mut = null;
   try
   {
    if(cacheName != null)
    {
     lock(ObjectsCache)
     {
      if(ObjectsCache.ContainsKey(cacheName))
      {
       if(ObjectsCache[cacheName] is Mutex)
        mut = (Mutex)ObjectsCache[cacheName];
       else
        return ObjectsCache[cacheName];
      }
      else
       ObjectsCache.Add(cacheName, (mut = new Mutex()));
     }
     if(mut != null)
      mut.WaitOne();
     lock(ObjectsCache)
     {
      if(ObjectsCache.ContainsKey(cacheName) && ObjectsCache[cacheName] is Mutex == false)
       return ObjectsCache[cacheName];
     }
    }

    PulsarAnswer answer = Query(query, noStub, noSer);

    if(answer.Return == null)
     throw new PulsarServerException("Выполнив запрос \"{0}\" к объекту [{1}] сервер не передал результат!",
       query.Query, query.Object);

    if(cacheName != null)    
     lock(ObjectsCache)
     {
      if(ObjectsCache.ContainsKey(cacheName))
       ObjectsCache[cacheName] = answer.Return;
      else if(cacheName != null)
       ObjectsCache.Add(cacheName, answer.Return);
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
  public static object Get(string objName, string query = null, string cacheName = null)
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
  public static object Get(string objName, string query, PulsarQueryParams pars, string cacheName = null)
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
  public static object Get(string objName, string query, object args,
                              PulsarQueryParams pars = PulsarQueryParams.None, string cacheName = null)
  {
   return Get(new PulsarQuery(objName, query, args, pars), cacheName:  cacheName);
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
  public static object Get(string objName, string query, IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null,
                            string cacheName = null)
  {
   return Get(new PulsarQuery(objName, query, null),  noStub, noSer , cacheName);
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
  public static object Exec(string objName, string query = null, object args = null,
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
  public static object Exec(PulsarQuery query, Action<PulsarAnswer> messageHandler = null)
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
  public static object Exec(PulsarQuery query, Type[] noStub, Type[] noSer = null, 
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
  public static object Modify(string objName, string query = null, object args = null,
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
  public static object Modify(PulsarQuery query, Action<PulsarAnswer> messageHandler = null)
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
  public static object Modify(PulsarQuery query, Type[] noStub, Type[] noSer = null, 
                                               Action<PulsarAnswer> messageHandler = null)
  {
   query.Params |= PulsarQueryParams.Modify;
   return Query(query, noStub, noSer, messageHandler: messageHandler).Return;
  }
  #endregion << Pulsar Methods >>
  //-------------------------------------------------------------------------------------  
  #region << Override Methods >>
  /// <summary>
  /// ToString()
  /// </summary>
  /// <returns></returns>
  public static new string ToString()
  {
   return String.Format("{0}:{1}", Address == null ? "0.0.0.0" : Address, Port);
  }
  #endregion << Override Methods >>
 }
 //**************************************************************************************

}
