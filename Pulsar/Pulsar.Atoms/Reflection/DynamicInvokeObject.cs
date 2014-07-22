using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Pulsar.Reflection
{
 /// <summary>
 /// Класс динамического враппера объекта с рефлекторным вызовом свойств и методов.
 /// </summary>
	public class DynamicInvokeObject : DynamicObject
	{
		private object _obj = null;
		private Type _t = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		private DynamicInvokeObject() { }
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public DynamicInvokeObject(object obj)
		{
		 if(obj == null)
			 throw new ArgumentNullException("obj");
			_obj = obj;
			_t = _obj.GetType();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Освобождает СОМ объект	враппера.
		/// </summary>
		public void ReleaseObject()
		{
		 if(Marshal.IsComObject(_obj))
			 Marshal.ReleaseComObject(_obj);
			_obj = null;
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.GetDynamicMemberNames();
		}
		public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //
			bool res = base.TryBinaryOperation(binder, arg, out result);

			return res;
		}
		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TryConvert(binder, out result);
		}
		public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TryCreateInstance(binder, args, out result);
		}
		public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TryDeleteIndex(binder, indexes);
		}
		public override bool TryDeleteMember(DeleteMemberBinder binder)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TryDeleteMember(binder);
		}
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TryGetIndex(binder, indexes, out result);
		}
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			try
			{
				result = _t.InvokeMember(binder.Name, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static, null, _obj, null);
				if(result == null)
					return false;
				Type t = result.GetType();
				if(t.IsPrimitive == false && t != typeof(string))
				 result = new DynamicInvokeObject(result);
				return true;
			}
			catch(Exception exc)
			{
				if(exc is TargetInvocationException)
					exc = exc.InnerException;
				string s = String.Format("{0}: {1}", binder.Name, exc.Message);
				throw new DynamicInvokeException(s);
			}
			//return base.TryGetMember(binder, out result);
		}
		public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TryInvoke(binder, args, out result);
		}
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			try
			{
				if(args != null)
				 for(int a = 0; a < args.Length; a++)
					 if(args[a] is DynamicInvokeObject)
						 args[a] = ((DynamicInvokeObject)args[a])._obj;
				result = _t.InvokeMember(binder.Name, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static, null, _obj, args);
				if(result == null)
					return true;
				Type t = result.GetType();
				if(t.IsPrimitive == false && t != typeof(string))
				 result = new DynamicInvokeObject(result);
				return true;
			}
			catch(Exception exc)
			{
			 if(exc is TargetInvocationException)
				 exc = exc.InnerException;
				string s = String.Format("{0}: {1}", binder.Name, exc.Message);
				throw new DynamicInvokeException(s);
			}
			//return base.TryInvokeMember(binder, args, out result);
		}
		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			//--- Debbuger Break --- //
			if(System.Diagnostics.Debugger.IsAttached)
				System.Diagnostics.Debugger.Break();
			//--- Debbuger Break --- //

			return base.TrySetIndex(binder, indexes, value);
		}
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			try
			{
			 object[] args;
			 if(value is object[])
				 args = (object[])value;
				else
			  args = new object[] { value };
				for(int a = 0; a < args.Length; a++)
					if(args[a] is DynamicInvokeObject)
						args[a] = ((DynamicInvokeObject)args[a])._obj;
				_t.InvokeMember(binder.Name, BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static, null, _obj, args);
				return true;
			}
			catch(Exception exc)
			{
				if(exc is TargetInvocationException)
					exc = exc.InnerException;
				string s = String.Format("{0}: {1}", binder.Name, exc.Message);
				throw new DynamicInvokeException(s);
			}
			//return base.TrySetMember(binder, value);
		}
		public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
		{
			if(binder.Operation == System.Linq.Expressions.ExpressionType.IsTrue)
			{
			 result = this;
				return true;
			}
			throw new NotImplementedException();
			
		}
		public override string ToString()
		{
			return _obj.ToString();
		}
		public override bool Equals(object obj)
		{
		 if(obj is DynamicInvokeObject == false)
			 return Object.Equals(_obj, obj);
			else
			 return Object.Equals(_obj, ((DynamicInvokeObject)obj)._obj);
		}
		public override int GetHashCode()
		{
			return _obj.GetHashCode();
		}  

		public static bool operator ==(DynamicInvokeObject o1, DynamicInvokeObject o2)
		{
			return object.Equals(o1,o2);
		}
		public static bool operator !=(DynamicInvokeObject o1, DynamicInvokeObject o2)
		{
			return !object.Equals(o1,o2);
		}
	}
	//*************************************************************************************
	public class DynamicInvokeException : Exception
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		public DynamicInvokeException(string msg) : base(msg)
		{

		}
		#endregion << Constructors >>
	}

}
