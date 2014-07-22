using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;

namespace Pulsar.AOP
{
	internal class AspectRegInfo
	{
		public Guid GUID;
		public Func<AssemblyDefinition,bool> NeedAsm = null;
		public Func<TypeDefinition,bool> NeedType = null;
		public Func<MethodDefinition,Tuple<MethodReference,MethodReference>> OnMethodExec = null;

	}
}
