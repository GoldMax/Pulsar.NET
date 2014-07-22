using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Linq.Expressions;

namespace Pulsar.Reflection.Dynamic
{
	/// <summary>
	/// Делегат для метода структуры с одним параметром.
	/// </summary>
	/// <typeparam name="Targ">Тип параметра</typeparam>
	/// <typeparam name="Tret">Тип возвращаемого значения</typeparam>
	/// <param name="arg"></param>
	/// <returns></returns>
	public delegate Tret RefFunc<Targ, out Tret>(ref Targ arg);


	/// <summary>
	/// Класс вспомогательных методов динамической рефлексии.
	/// </summary>
	public static class ReflectionHelper
	{
		private static BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public |BindingFlags.Instance |
																																						BindingFlags.Static | BindingFlags.DeclaredOnly;

		//-------------------------------------------------------------------------------------
		#region << Methods for fields  >>
		/// <summary>
		/// Создает динамический метод получения значения поля для указанного типа.
		/// </summary>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns></returns>
		public static Func<object, object> CreateFieldGetMethod(Type type, string fieldName)
		{
			FieldInfo fid = type.GetField(fieldName, flags);
			if(fid == null)
				throw new PulsarException("Поле \"{0}\" не найдено в типе [{1}]!", fieldName, type.FullName);
			return CreateFieldGetMethod(type, fid);
		}
		/// <summary>
		/// Создает динамический метод получения значения поля для указанного типа.
		/// </summary>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fi">Информация о поле</param>
		/// <returns></returns>
		public static Func<object, object> CreateFieldGetMethod(Type type, FieldInfo fi)
		{
			DynamicMethod meth = 
					new DynamicMethod(fi.Name + "_Get", typeof(object), new Type[] { typeof(object) }, type, true);

			ILGenerator il = meth.GetILGenerator();

			if(fi.IsStatic == false)
			{
				il.Emit(OpCodes.Ldarg_0);
				if(fi.DeclaringType.IsValueType)
					il.Emit(OpCodes.Unbox, fi.DeclaringType);
			}

			il.Emit(OpCodes.Ldfld, fi);
			if(fi.FieldType.IsValueType)
				il.Emit(OpCodes.Box, fi.FieldType);
			il.Emit(OpCodes.Ret);

			return (Func<object, object>)meth.CreateDelegate(typeof(Func<object, object>));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает динамический метод получения значения поля для указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип значений поля.</typeparam>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns></returns>
		public static Func<object, T> CreateFieldGetMethod<T>(Type type, string fieldName)
		{
			FieldInfo fid = type.GetField(fieldName, flags);
			if(fid == null)
				throw new PulsarException("Поле \"{0}\" не найдено в типе [{1}]!", fieldName, type.FullName);
			return CreateFieldGetMethod<T>(type, fid);
		}
		/// <summary>
		/// Создает динамический метод получения значения поля для указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип значений поля.</typeparam>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fi">Информация о поле</param>
		/// <returns></returns>
		public static Func<object, T> CreateFieldGetMethod<T>(Type type, FieldInfo fi)
		{

			DynamicMethod meth = 
					new DynamicMethod(fi.Name + "_Get", typeof(T), new Type[] { typeof(object) }, type, true);

			ILGenerator il = meth.GetILGenerator();

			if(fi.IsStatic == false)
			{
				il.Emit(OpCodes.Ldarg_0);
				if(fi.DeclaringType.IsValueType)
					il.Emit(OpCodes.Unbox, fi.DeclaringType);
			}

			il.Emit(OpCodes.Ldfld, fi);
			if(typeof(T).IsValueType == false)
				il.Emit(OpCodes.Box, fi.FieldType);
			il.Emit(OpCodes.Ret);

			return (Func<object, T>)meth.CreateDelegate(typeof(Func<object, T>));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает динамический метод установки значения поля для указанного типа.
		/// </summary>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns></returns>
		public static Action<object, object> CreateFieldSetMethod(Type type, string fieldName)
		{
			FieldInfo fid = type.GetField(fieldName, flags);
			if(fid == null)
				throw new PulsarException("Поле \"{0}\" не найдено в типе [{1}]!", fieldName, type.FullName);
			return CreateFieldSetMethod(type, fid);
		}
		/// <summary>
		/// Создает динамический метод установки значения поля для указанного типа.
		/// </summary>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fi">Информация о поле</param>
		/// <returns></returns>                 
		public static Action<object, object> CreateFieldSetMethod(Type type, FieldInfo fi)
		{
			DynamicMethod meth = 
					new DynamicMethod(fi.Name + "_Set",
					typeof(void), new Type[] { typeof(object), typeof(object) }, type, true);

			ILGenerator il = meth.GetILGenerator();

			if(fi.IsStatic == false)
			{
				il.Emit(OpCodes.Ldarg_0);
				if(fi.DeclaringType.IsValueType)
					il.Emit(OpCodes.Unbox, fi.DeclaringType);
			}

			il.Emit(OpCodes.Ldarg_1);
			if(fi.FieldType.IsValueType)
				il.Emit(OpCodes.Unbox_Any, fi.FieldType);
			il.Emit(OpCodes.Stfld, fi);
			il.Emit(OpCodes.Ret);

			return (Action<object, object>)meth.CreateDelegate(typeof(Action<object, object>));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает динамический метод установки значения поля для указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип значений поля.</typeparam>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns></returns>
		public static Action<object, T> CreateFieldSetMethod<T>(Type type, string fieldName)
		{
			FieldInfo fid = type.GetField(fieldName, flags);
			if(fid == null)
				throw new PulsarException("Поле \"{0}\" не найдено в типе [{1}]!", fieldName, type.FullName);
			return CreateFieldSetMethod<T>(type, fid);
		}
		/// <summary>
		/// Создает динамический метод установки значения поля для указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип значений поля.</typeparam>
		/// <param name="type">Тип, для поля которого создается метод.</param>
		/// <param name="fi">Информация о поле</param>
		/// <returns></returns>
		public static Action<object, T> CreateFieldSetMethod<T>(Type type, FieldInfo fi)
		{
			DynamicMethod meth = 
					new DynamicMethod(fi.Name + "_Set",
					typeof(void), new Type[] { typeof(object), typeof(T) }, type, true);

			ILGenerator il = meth.GetILGenerator();

			if(fi.IsStatic == false)
			{
				il.Emit(OpCodes.Ldarg_0);
				if(fi.DeclaringType.IsValueType)
					il.Emit(OpCodes.Unbox, fi.DeclaringType);
			}

			il.Emit(OpCodes.Ldarg_1);
			if(fi.FieldType.IsValueType)
				il.Emit(OpCodes.Unbox_Any, fi.FieldType);
			il.Emit(OpCodes.Stfld, fi);
			il.Emit(OpCodes.Ret);

			return (Action<object, T>)meth.CreateDelegate(typeof(Action<object, T>));
		}
		#endregion << Methods for fields >>
		//-------------------------------------------------------------------------------------
		#region << Methods for properies >>
		/// <summary>
		///  Создает открытый делегат Func на метод получения значения свойства, представленный в MethodInfo.
		/// </summary>
		/// <typeparam name="T">Тип, для метода которого создается делегат.</typeparam>
		/// <param name="propName">Имя свойства</param>
		/// <param name="selfOnly">Ограничивает поиск свойств только свойствами, напрямую определенными в типе.</param>
		/// <param name="throwError">Определяет, нужно ли выбрасывать исключения.</param>
		/// <returns></returns>
		public static Delegate GetPropertyGetMethod<T>(string propName, bool selfOnly = false,
																																																		bool throwError = true  )
		{
			return	GetPropertyGetMethod(typeof(T), propName, selfOnly, throwError);
		}
		/// <summary>
		///  Создает открытый делегат Func на метод получения значения свойства, представленный в MethodInfo.
		/// </summary>
		/// <param name="type">Тип, для метода которого создается делегат.</param>
		/// <param name="propName">Имя свойства</param>
		/// <param name="selfOnly">Ограничивает поиск свойств только свойствами, напрямую определенными в типе.</param>
		/// <param name="throwError">Определяет, нужно ли выбрасывать исключения.</param>
		/// <returns></returns>
		public static Delegate GetPropertyGetMethod(Type type, string propName,  bool selfOnly = false,
																																																		bool throwError = true  )
		{
			if(String.IsNullOrWhiteSpace(propName))
				throw new ArgumentNullException("propName");
			Type t = type;
			PropertyInfo pi = null;
			if(selfOnly)
				pi = t.GetProperty(propName, flags);
			//else if(t.IsInterface)
			//{
			// pi = t.GetProperty(propName, flags);
			// if(pi == null)
			//  foreach(Type tt in t.GetInterfaces())
			//  {
			//   InterfaceMapping im = t.GetInterfaceMap(tt);
						
			//   if((pi = t.GetProperty(propName, flags)) != null)
			//    break;
			//  }
			//}
			else 
				while(t != null && t != typeof(object))
				{
					pi = t.GetProperty(propName, flags);
					if(pi != null)
						break;
					t = t.BaseType;
				}
			if(pi == null)
				if(throwError)
					throw new PulsarException("Свойство \"{0}\" не определено в типе [{1}]!", propName, type);
				else
					return null;
			MethodInfo mi = pi.GetGetMethod();
			if(mi == null)
				if(throwError)
					throw new PulsarException("Свойство \"{0}\" класса [{1}] не имеет метода get!", propName, t);
				else
					return null;
			return GetPropertyGetMethod(mi);
		}
		/// <summary>
		/// Создает открытый делегат Func на метод получения значения свойства, представленный в MethodInfo.
		/// </summary>
		/// <param name="method">Описание метода get.</param>
		/// <returns></returns>
		public static Delegate GetPropertyGetMethod(MethodInfo method)
		{
			if(method == null)
				throw new ArgumentNullException("method");
			ParameterInfo[] pis = method.GetParameters();
			Type[] typeArgs	= new Type[pis.Length + 2];
			//if(method.DeclaringType.IsValueType)
			// typeArgs[0] = method.DeclaringType.MakeByRefType();
			//else
				typeArgs[0] = method.DeclaringType;
			for(int a = 1; a < pis.Length; a++)
				typeArgs[a] = pis[a].ParameterType;
			typeArgs[typeArgs.Length-1] = method.ReturnType;

			Type delegateType;
			if(method.DeclaringType.IsValueType)
				delegateType = typeof(RefFunc<,>).MakeGenericType(typeArgs);
			else
				delegateType = Expression.GetDelegateType(typeArgs);
			return Delegate.CreateDelegate(delegateType, method, true);
		}
		#endregion << Methods for properies >>
		//-------------------------------------------------------------------------------------
		#region << Methods for methods >>
		/// <summary>
		/// Создает делегат (Func или Action) метода.
		/// Если target == null, создается открытый делегат, иначе закрытый делегат.
		/// </summary>
		/// <param name="method">Описание метода.</param>
		/// <param name="target">Объект, для метода которого создается делегат.
		/// Для статического метода игнорируется.</param>
		/// <returns></returns>
		public static Delegate GetDelegate(MethodInfo method, object target)
		{
			if(method == null)
				throw new ArgumentNullException("method");
			ParameterInfo[] pis = method.GetParameters();
			Type[] typeArgs;
			if(method.IsStatic)
			{
			 target = null;
				typeArgs	= new Type[pis.Length + 1];
				for(int a = 0; a < pis.Length; a++)
					typeArgs[a] = pis[a].ParameterType;
			}
			else	if(target == null || target is Type)
			{
				typeArgs	= new Type[pis.Length + 2];
				typeArgs[0] = target == null ? method.DeclaringType : (Type)target;
				for(int a = 0; a < pis.Length; a++)
					typeArgs[a+1] = pis[a].ParameterType;
			}
			else
			{
				if(method.DeclaringType != target.GetType() && 
								target.GetType().IsSubclassOf(method.DeclaringType) == false)
					throw new PulsarException("Метод \"{0}\" не принадлежит типу [{1}]!", method, target.GetType().ToString());
				typeArgs	= new Type[pis.Length + 1];
				for(int a = 0; a < pis.Length; a++)
					typeArgs[a] = pis[a].ParameterType;
			}
			typeArgs[typeArgs.Length-1] =  method.ReturnType;
			Type delegateType = Expression.GetDelegateType(typeArgs);
			return Delegate.CreateDelegate(delegateType, target, method, true);

		}
		/// <summary>
		/// Создает делегат (Func или Action) метода.
		/// Если target == null, создается открытый делегат, иначе закрытый делегат.
		/// </summary>
		/// <param name="methodName">Имя метода</param>
		/// <param name="target">Объект, для которого вызывается делегат. 
		/// Для статического метода или открытого делегата указывается тип(Type)./</param>
		/// <param name="argsTypes">Типы аргументов метода.</param>
		/// <returns></returns>
		public static Delegate GetDelegate(string methodName, object target, Type[] argsTypes = null)
		{
			if(String.IsNullOrWhiteSpace(methodName))
				throw new ArgumentNullException("method");
			if(target == null)
				throw new ArgumentNullException("target");
			if(argsTypes == null)
			 argsTypes = Type.EmptyTypes;
			MethodInfo mi = null;
			if(target is Type)
			 mi = ((Type)target).GetMethod(methodName, flags, null, argsTypes, null);
			else
			 mi = target.GetType().GetMethod(methodName, flags ^ BindingFlags.Static, null, argsTypes, null);
			if(mi == null)
				throw new PulsarException("Тип [{0}] не содержит метод \"{1}\"!", target.GetType().FullName, methodName);
			return GetDelegate(mi, target);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает лямбду и открытый делегат на нее для вызова метода	без параметров
		/// </summary>
		/// <typeparam name="T">Тип, для которого создается лямбда</typeparam>
		/// <typeparam name="M">Тип возвращаемого значения.</typeparam>
		/// <param name="methodInfo">Информация о вызываемом методе</param>
		/// <returns></returns>
		public static Func<T, M> CreateCallLambda<T, M>(MethodInfo methodInfo)
		{
			var input = Expression.Parameter(typeof(T), "input");
			MethodCallExpression member = Expression.Call(input, methodInfo);
			var lambda = Expression.Lambda<Func<T, M>>(member, input);

			return lambda.Compile();
		}
		/// <summary>
		/// Создает лямбду и открытый делегат Func на нее для вызова метода	без параметров
		/// </summary>
		/// <param name="methodInfo">Информация о вызываемом методе</param>
		/// <returns></returns>
		public static Func<object, object> CreateCallLambdaFunc(MethodInfo methodInfo)
		{
			ParameterExpression obj = Expression.Parameter(typeof(object), "obj");

			Expression<Func<object, object>> expr =
								Expression.Lambda<Func<object, object>>(
											Expression.Convert(
															Expression.Call(
																			Expression.Convert(obj, methodInfo.DeclaringType),
																			methodInfo),
															typeof(object)),
											obj);

			return expr.Compile();
		}
		/// <summary>
		/// Создает лямбду и открытый делегат Func на нее для вызова метода	с двумя параметрами.
		/// </summary>
		/// <param name="methodInfo">Информация о вызываемом методе</param>
		/// <returns></returns>
		public static Func<object,TArg1, TArg2, object> CreateCallLambdaFunc2<TArg1,TArg2>(MethodInfo methodInfo)
		{
			ParameterExpression obj = Expression.Parameter(typeof(object));

			ParameterExpression[] pars = new ParameterExpression[2];
			pars[0] = Expression.Parameter(typeof(TArg1));
			pars[1] = Expression.Parameter(typeof(TArg2));

			Expression<Func<object,TArg1, TArg2, object>> expr =
								Expression.Lambda<Func<object, TArg1, TArg2, object>>(
											Expression.Convert(
															Expression.Call(methodInfo.IsStatic ? null : Expression.Convert(obj, methodInfo.DeclaringType),
																			methodInfo, pars),
															typeof(object)),
								methodInfo.Name,	new [] {	obj, pars[0], pars[1] });

			return expr.Compile();
		}
		/// <summary>
		/// Создает лямбду и открытый делегат Action на нее для вызова метода	с двумя параметрами.
		/// </summary>
		/// <param name="methodInfo">Информация о вызываемом методе</param>
		/// <returns></returns>
		public static Action<object,TArg1, TArg2> CreateCallLambdaAction2<TArg1,TArg2>(MethodInfo methodInfo)
		{
			ParameterExpression obj = Expression.Parameter(typeof(object));

			ParameterExpression[] pars = new ParameterExpression[2];
			pars[0] = Expression.Parameter(typeof(TArg1));
			pars[1] = Expression.Parameter(typeof(TArg2));

			Expression<Action<object,TArg1, TArg2>> expr =
								Expression.Lambda<Action<object, TArg1, TArg2>>(
													Expression.Call( methodInfo.IsStatic ? null : Expression.Convert(obj, methodInfo.DeclaringType),
																			methodInfo, pars),
								methodInfo.Name,	new [] {	obj, pars[0], pars[1] });

			return expr.Compile();
		}
		#endregion << Methods for methods >>
		//-------------------------------------------------------------------------------------
		#region << Methods for constructors >>
		/// <summary>
		/// Вызывает конструктор указанного объекта через Reflection.
		/// </summary>
		/// <param name="obj">Объект, для которого вызывается конструктор.</param>
		/// <param name="ctorParamsTypes">Типы параметров конструктора</param>
		/// <param name="ctorArgs">Аргументы конструктора.</param>
		public static void InvokeCtor(object obj, Type[] ctorParamsTypes = null, object[] ctorArgs = null)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			Type t = obj.GetType();
			MethodBase mi = t.GetConstructor(BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Instance,
																					null, ctorParamsTypes ?? Type.EmptyTypes, null);
			if(mi == null)
				throw new PulsarException("Конструктор типа [{0}] с указанными типами параметров не найден!", t.FullName);
			mi.Invoke(obj, ctorArgs);
		}
		/// <summary>
		/// Вызывает конструктор указанного объекта через Reflection.
		/// </summary>
		/// <param name="t">Тип, для которого следует вызывать конструктор.</param>
		/// <param name="obj">Объект, для которого вызывается конструктор.</param>
		/// <param name="ctorParamsTypes">Типы параметров конструктора</param>
		/// <param name="ctorArgs">Аргументы конструктора.</param>
		public static void InvokeCtor(Type t, object obj, Type[] ctorParamsTypes = null, object[] ctorArgs = null)
		{
			if(obj == null)
				throw new ArgumentNullException("obj");
			if(t == null)
				throw new ArgumentNullException("t");
			MethodBase mi = t.GetConstructor(BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Instance,
																					null, ctorParamsTypes ?? Type.EmptyTypes, null);
			if(mi == null)
				throw new PulsarException("Конструктор типа [{0}] с указанными типами параметров не найден!", t.FullName);
			mi.Invoke(obj, ctorArgs);
		}
		#endregion << Methods for constructors >>
	}

}
