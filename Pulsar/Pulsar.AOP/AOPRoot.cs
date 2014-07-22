using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Pulsar.Reflection.Dynamic;

namespace Pulsar.AOP
{
	/// <summary>
	/// Корневой класс AOP
	/// </summary>
	public static class AOPRoot
	{
		private static Dictionary<Type, AspectRegInfo> _dic = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Public Methods >>
		/// <summary>
		/// Инициализация
		/// </summary>
		public static void Init()
		{
			_dic = new Dictionary<Type, AspectRegInfo>();

			LoadAOPAssemblies();
		}
		public static void RegAspect(Type aspectType)
		{
			if(_dic.ContainsKey(aspectType))
				return;
			if(aspectType.GUID == Guid.Empty)
				throw new Exception("Для типа аспекта не указан GUID!");
			AspectRegInfo ari = new AspectRegInfo();
			ari.GUID = aspectType.GUID;
			foreach(MethodInfo mi in aspectType.GetMethods(BindingFlags.Static | BindingFlags.Public))
				foreach(Attribute a in mi.GetCustomAttributes(typeof(AspectMethodAttribute), true))
					if(a is AspectNeedApplyForAssemblyMethodAttribute)
						ari.NeedAsm = (Func<AssemblyDefinition,bool>)ReflectionHelper.GetDelegate(mi,null);
					else if(a is AspectNeedApplyForTypeMethodAttribute)
						ari.NeedType = (Func<TypeDefinition,bool>)ReflectionHelper.GetDelegate(mi,null);
					else if(a is AspectOnMethodExecAttribute)
						ari.OnMethodExec = (Func<MethodDefinition, Tuple<MethodReference, MethodReference>>)
							ReflectionHelper.GetDelegate(mi, null);
			_dic.Add(aspectType, ari);
		}
		internal static List<AspectRegInfo> NeedAssembly(AssemblyDefinition assm)
		{
			HashSet<Guid> applied = new HashSet<Guid>();
			foreach(CustomAttribute ca in assm.CustomAttributes)
				if(ca.AttributeType.Name == "AspectAppliedAttribute")
					applied.Add(Guid.Parse(ca.ConstructorArguments[0].Value.ToString()));

			List<AspectRegInfo> res = new List<AspectRegInfo>();
			foreach(var i in _dic.Values)
			{
				if(i.NeedAsm == null || applied.Contains(i.GUID))
					continue;
				if(i.NeedAsm(assm))
					res.Add(i);
				}
			return res;
		}
		internal static List<AspectRegInfo> NeedType(TypeDefinition tdef, IEnumerable<AspectRegInfo> subset)
		{
			List<AspectRegInfo> res = new List<AspectRegInfo>();
			foreach(var i in subset)
				if(i.NeedType != null && i.NeedType(tdef))
					res.Add(i);
			return res;
		}
		/// <summary>
		/// Применяет необходимые аспекты для сборки.
		/// </summary>
		/// <param name="name">Путь к файлу сборки</param>
		public static bool ApplyAspects(ref string name)
		{
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(name);
			bool needSave = false;
			List<AspectRegInfo> wantAssm = NeedAssembly(assembly);
			if(wantAssm.Count > 0)
			{
				//bool wasInject = false;
				foreach(TypeDefinition type in assembly.MainModule.Types)
				{
					if(type.Name == "<Module>")
						continue;
					List<AspectRegInfo> wantType = NeedType(type, wantAssm);
					if(wantType.Count > 0)
					{
						foreach(MethodDefinition md in type.Methods)
							foreach(AspectRegInfo ari in wantType)
							{
								if(ari.OnMethodExec != null)
								{
									Tuple<MethodReference, MethodReference> mr = ari.OnMethodExec(md);
									if(mr == null || md.HasBody == false)
										continue;
									ILProcessor il = md.Body.GetILProcessor();
									if(mr.Item1 != null)
									{
										Instruction ins = md.Body.Instructions[0];
										il.InsertBefore(ins, il.Create(OpCodes.Ldarg_0));
										//il.InsertBefore(ins, il.Create(OpCodes.Ldstr, md.FullName));
										il.InsertBefore(ins, il.Create(OpCodes.Call, mr.Item1));
									}
									if(mr.Item2 != null)
									{
										Instruction ins = md.Body.Instructions[md.Body.Instructions.Count-1];
										il.InsertBefore(ins, il.Create(OpCodes.Ldarg_0));
										//il.InsertBefore(ins, il.Create(OpCodes.Ldstr, md.FullName));
										il.InsertBefore(ins, il.Create(OpCodes.Call, mr.Item2));
									}
								}
								//wasInject = true;
							}
					}
				}
				//if(wasInject)
				{
					var me = assembly.MainModule.Import(typeof(AspectAppliedAttribute).GetConstructor(new[] { typeof(String) }));
					var gt = assembly.MainModule.Import(typeof(String));
					foreach(var i in wantAssm)
					{
						CustomAttribute ca = new CustomAttribute(me);
						ca.ConstructorArguments.Add(new CustomAttributeArgument(gt, i.GUID.ToString()));
						assembly.CustomAttributes.Add(ca);
					}
					needSave = true;
				}
			}
			if(needSave)
				try
				{
					assembly.Write(name);
				}
				catch
				{
					throw;
				}
			return needSave;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		private static void LoadAOPAssemblies()
		{
			// TODO : Добавить код загрузки сборок с правилами AOP
			RegAspect(typeof(ReadWriteLockAcpect));
		}
	}


}
