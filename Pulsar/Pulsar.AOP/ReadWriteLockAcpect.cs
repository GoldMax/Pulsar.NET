//#define LockTrace

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Pulsar;
using Mono.Cecil;

namespace Pulsar.AOP
{
 /// <summary>
	/// Класс асспекта для IReadWriteLockObject типов.
 /// </summary>
	public static class ReadWriteLockAcpect
	{
		static HashSet<string> lockAttributes = new HashSet<string>();
		static MethodInfo miBeginRead = null;
		static MethodInfo miBeginWrite = null;
		static MethodInfo miEndRead = null;
		static MethodInfo miEndWrite = null;

	 static Tuple<Guid, MethodReference> onEntryReadLock = null;
	 static Tuple<Guid, MethodReference> onEntryWriteLock = null;
		////static Tuple<Guid, MethodReference> onExitReadLock = null;
		////static Tuple<Guid, MethodReference> onExitWriteLock = null;

		static ReadWriteLockAcpect()
		{
			miBeginRead = typeof(ReadWriteLockAcpect).GetMethod("BeginRead", BindingFlags.Public | BindingFlags.Static);
			miBeginWrite = typeof(ReadWriteLockAcpect).GetMethod("BeginWrite", BindingFlags.Public | BindingFlags.Static);
			miEndRead = typeof(ReadWriteLockAcpect).GetMethod("EndRead", BindingFlags.Public | BindingFlags.Static);
			miEndWrite = typeof(ReadWriteLockAcpect).GetMethod("EndWrite", BindingFlags.Public | BindingFlags.Static);

			lockAttributes.Add(typeof(ReadWriteLockAttribute).FullName);
			lockAttributes.Add(typeof(ReadLockAttribute).FullName);
			lockAttributes.Add(typeof(WriteLockAttribute).FullName);
			lockAttributes.Add(typeof(NoLockAttribute).FullName);
		}

		//-------------------------------------------------------------------------------------
		[AspectNeedApplyForAssemblyMethod]
		public static bool AssemblyNeedAOP(AssemblyDefinition assm)
		{
			if(CheckLockAttributes(assm.CustomAttributes, LockType.ReadLock) == LockType.NoLock)
			 return false;
			return true;
		}
		//-------------------------------------------------------------------------------------
		[AspectNeedApplyForTypeMethod]
		public static bool TypeNeedAOP(TypeDefinition tdef)
		{
			if(CheckLockAttributes(tdef.CustomAttributes, LockType.ReadLock) == LockType.NoLock) 
			 return false;

		 TypeReference t = tdef.Module.Import(typeof(IReadWriteLockObject));
			while(tdef != null)
			{
			 foreach(TypeReference itr in tdef.Interfaces)
			  if(itr.FullName == t.FullName)
				  return true;		
				tdef = tdef.BaseType == null ? null : tdef.BaseType.Resolve();
			}
			return false;

		}
		//-------------------------------------------------------------------------------------
		[AspectOnMethodExec]
		public static Tuple<MethodReference,MethodReference> OnMethodEntry(MethodDefinition mdef)
		{
			LockType lockType = LockType.ReadLock;
		 if(mdef.IsConstructor || mdef.IsStatic)
			 lockType = LockType.NoLock;
			else	if(mdef.IsPublic == false)
			{
				if(!(mdef.Attributes.HasFlag(Mono.Cecil.MethodAttributes.Assembly) ||
							mdef.Attributes.HasFlag(Mono.Cecil.MethodAttributes.Family) ||
							mdef.Attributes.HasFlag(Mono.Cecil.MethodAttributes.Virtual)))
					lockType = LockType.NoLock;
			}
			if(lockType != LockType.NoLock && (mdef.IsSetter || mdef.IsGetter))
			{
				if(mdef.IsSetter)
				 lockType = LockType.WriteLock;
				PropertyDefinition pd = null;
				if(mdef.IsSetter)
				{
					foreach(var p in mdef.DeclaringType.Properties)
						if(mdef.Equals(p.SetMethod))
						{
							pd = p;
							break;
						}
				}
				else
				{
					foreach(var p in mdef.DeclaringType.Properties)
						if(mdef.Equals(p.GetMethod))
						{
							pd = p;
							break;
						}
				}
				if(pd == null)
				 throw new PulsarException("Не найдено свойство для метода [{0}]!", mdef.FullName);
				if(pd.HasCustomAttributes)
				 lockType = CheckLockAttributes(pd.CustomAttributes, lockType);
			}

			lockType = CheckLockAttributes(mdef.CustomAttributes, lockType);

			if(lockType == LockType.NoLock)
			 return null;

			Tuple<Guid, MethodReference> onEntry = lockType == LockType.ReadLock ? onEntryReadLock : onEntryWriteLock;
			if(onEntry == null || mdef.Module.Mvid != onEntry.Item1)
			{
			 if(lockType == LockType.ReadLock)
				 onEntryReadLock = new Tuple<Guid,MethodReference>(mdef.Module.Mvid,	mdef.Module.Import(miBeginRead));
				else
				 onEntryWriteLock = new Tuple<Guid,MethodReference>(mdef.Module.Mvid,	mdef.Module.Import(miBeginWrite));
				onEntry = lockType == LockType.ReadLock ? onEntryReadLock : onEntryWriteLock;	      
			}
			////Tuple<Guid, MethodReference> onExit = lockType == LockType.ReadLock ? onExitReadLock : onExitWriteLock;
			////if(onExit == null || mdef.Module.Mvid != onExit.Item1)
			////{
			//// if(lockType == LockType.ReadLock)
			////  onExitReadLock = new Tuple<Guid, MethodReference>(mdef.Module.Mvid, mdef.Module.Import(miEndRead));
			//// else
			////  onExitWriteLock = new Tuple<Guid, MethodReference>(mdef.Module.Mvid, mdef.Module.Import(miEndWrite));
			//// onExit = lockType == LockType.ReadLock ? onExitReadLock : onExitWriteLock;
			////}
			return new Tuple<MethodReference,MethodReference>(onEntry.Item2, null); ////onExit.Item2);
		}
		//-------------------------------------------------------------------------------------
		public static void BeginRead(object obj)
		{
			IReadWriteLockObject o = obj as IReadWriteLockObject;
			if(o != null)
			 o.BeginRead();
		}
		public static void BeginWrite(object obj)
		{
			IReadWriteLockObject o = obj as IReadWriteLockObject;
			if(o != null)
				o.BeginWrite();
		}
		public static void EndRead(object obj)
		{
			IReadWriteLockObject o = obj as IReadWriteLockObject;
			if(o != null)
				o.EndRead();
		}
		public static void EndWrite(object obj)
		{
			IReadWriteLockObject o = obj as IReadWriteLockObject;
			if(o != null)
				o.EndWrite();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Если нет атрибута, возврашает defLock
		/// </summary>
		private static LockType CheckLockAttributes(Mono.Collections.Generic.Collection<CustomAttribute> col, LockType defLock)
		{
			foreach(CustomAttribute a in col)
				if(lockAttributes.Contains(a.AttributeType.FullName))
				{
					if(a.HasConstructorArguments)
						return (LockType)a.ConstructorArguments[0].Value;
					if(a.HasProperties)
						return (LockType)a.Properties[0].Argument.Value;
					if(a.AttributeType.FullName == typeof(ReadLockAttribute).FullName)
						return LockType.ReadLock;
					if(a.AttributeType.FullName == typeof(WriteLockAttribute).FullName)
						return LockType.WriteLock;
					if(a.AttributeType.FullName == typeof(NoLockAttribute).FullName)
						return LockType.NoLock;
					break;
				}
			return defLock;
		}
	}
}
