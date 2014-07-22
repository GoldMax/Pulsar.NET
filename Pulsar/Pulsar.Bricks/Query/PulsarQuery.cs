using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pulsar
{
	/// <summary>
	/// Класс объекта запроса к Пульсару.
	/// </summary>
	public class PulsarQuery	: IDisposable
	{
		/// <summary>
		/// Контекст запроса
		/// </summary>
		public static Context ContextQuery;

		#region << Members >>

		private object o = null;
		private string q = null;
		private ParamsDic a = null;
		private PulsarQueryParams p = PulsarQueryParams.None;
		private Type[] t = null;
		private Type[] e = null;
		private Type[] d = null;
		private Context c; 

		#endregion << Members >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Глобальный объект, для которого выполняется запрос.
		/// Если строка, будет выполнена попытка найти объект по GlobalName.
		/// </summary>
		public object RootObject 
		{  
			get { return o; }
			set 
			{
				if(value is String == false && value is GlobalObject == false )
					throw new Exception("RootObject не является строкой или GlobalObject!");
				o = value; 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Строка запроса.
		/// </summary>
		public string Query { get { return q; } set { q = value; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Аргументы запроса.
		/// </summary>
		public ParamsDic Args { get { return a; } set { a = value; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Контекст запроса
		/// </summary>
		public Context Context { get { return c; } set { c = value; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Параметры запроса.
		/// </summary>
		public PulsarQueryParams Params { get { return p; } set { p = value; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Типы, объекты которых не должны быть заглушены.
		/// </summary>
		public Type[] NoStubTypes { get { return t; } set { t = value; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Типы, объекты которых не должны быть сериализованы.
		/// </summary>
		public Type[] NoSerTypes { get { return e; } set { e = value; } }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Типы полей, которые имеют атрибут PulsarByDemandSerialization, но должны быть сериализованы.
		/// </summary>
		public Type[] ByDemand { get { return d; } set { d = value; } }
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarQuery()
		{
			c = ContextQuery;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="obj">Объект Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		public PulsarQuery(object obj, string query = null)
			: this()
		{
			RootObject = obj;
			Query = query;
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="obj">Объект Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="args">Аргументы запроса.</param>
		/// <param name="pars">Параметры запроса.</param>
		/// <param name="noStub">Типы, объекты которых не должны быть заглушены.</param>
		/// <param name="noSer">Типы, объекты которых не должны быть сериализованы.</param>
		public PulsarQuery(object obj, string query = null, object args = null,
									PulsarQueryParams pars = PulsarQueryParams.None,
									IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null)
			: this()
		{
			RootObject = obj;
			Query = query;
			if (args != null)
				if (args is ParamsDic)
					Args = (ParamsDic)args;
				else
				{
					Args = new ParamsDic();
					foreach (PropertyInfo pi in args.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
						Args.Add(pi.Name, pi.GetValue(args, null));
				}
			Params = pars;
			NoStubTypes = noStub == null ? null : noStub.ToArray();
			NoSerTypes = noSer == null ? null : noSer.ToArray();
		}
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="obj">Объект Пульсара.</param>
		/// <param name="query">Строка запроса.</param>
		/// <param name="args">Аргументы запроса.</param>
		/// <param name="pars">Параметры запроса.</param>
		/// <param name="noStub">Типы, объекты которых не должны быть заглушены.</param>
		/// <param name="noSer">Типы, объекты которых не должны быть сериализованы.</param>
		public PulsarQuery(object obj, string query = null, IDictionary<string, object> args = null,
									PulsarQueryParams pars = PulsarQueryParams.None,
									IEnumerable<Type> noStub = null, IEnumerable<Type> noSer = null)
			: this()
		{
			RootObject = obj;
			Query = query;
			if (args != null)
				Args = new ParamsDic(args);
			Params = pars;
			NoStubTypes = noStub == null ? null : noStub.ToArray();
			NoSerTypes = noSer == null ? null : noSer.ToArray();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Obj={0}", RootObject);
			if (Query != null)
				sb.AppendFormat("¦Query={0}", Query);
			if (Args != null)
				sb.AppendFormat("¦Args=[{0}]", Args);
			if (Params != PulsarQueryParams.None)
				sb.AppendFormat("¦Params={0}", Params);
			if (NoStubTypes != null)
				sb.Append("¦NoStubTypes=[]");
			if (NoSerTypes != null)
				sb.Append("¦NoSerTypes=[]");
			if (ByDemand != null)
				sb.Append("¦ByDemand=[]");
			if (c.User != null)
				sb.AppendFormat("¦User={0}", c.User);
			sb.AppendFormat("¦ClientVersion={0}", c.ClientVersion);
			if (!string.IsNullOrEmpty(c.ClientName))
				sb.AppendFormat("¦ClientName={0}", c.ClientName);
			sb.AppendFormat("¦ClientType={0}", c.ClientType);
			return sb.ToString();
		}

		#region IDisposable Members
		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			o = null;
			q = null;
			if(a != null)
			{
				a.Clear();
				a.Dispose();
				a = null;
			}
			if(t != null)
			{
				Array.Clear(t, 0, t.Length);
				t = null;
			}
			if(e != null)
			{
				Array.Clear(e, 0, e.Length);
				e = null;
			}
			if(d != null)
			{
				Array.Clear(d, 0, d.Length);
				d = null;
			}
		}
		#endregion
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		/// <summary>
		/// Составляет словарь значений свойств, отличающихся у двух объектов.
		/// </summary>
		/// <typeparam name="T">Тип объектов сравнения</typeparam>
		/// <param name="obj1">Объект - эталон</param>
		/// <param name="obj2">Объект, значения которого будут занесены в словарь</param>
		/// <param name="flags">Флаги поиска свойств</param>
		/// <param name="deep">Тип, до которого будут проверяться свойства.</param>
		/// <returns></returns>
		public static ParamsDic GetPropertyDiffs<T>(T obj1, T obj2, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, Type deep = null)
		{
			ParamsDic res = new ParamsDic(false);
			Type t = typeof(T);
			object val;
			while(t != null && t != typeof(object))
			{
				foreach(PropertyInfo pi in t.GetProperties(flags))
					if(pi.GetIndexParameters().Length == 0 && pi.GetSetMethod() != null)
						if(Object.Equals(pi.GetValue(obj1, null), (val = pi.GetValue(obj2, null))) == false)
							if(res.ContainsKey(pi.Name) == false)
								res.Add(pi.Name, pi.GetValue(obj2, null));
				if(t == deep)
					break;
				else
					t = t.BaseType;
			}

			return res;
		}

		#endregion <<Static Methods >>
		//-------------------------------------------------------------------------------------
						
	}
}
