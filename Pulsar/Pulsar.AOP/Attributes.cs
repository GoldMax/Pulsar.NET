using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.AOP
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class AspectMethodAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class AspectNeedApplyForAssemblyMethodAttribute : AspectMethodAttribute { }

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class AspectNeedApplyForTypeMethodAttribute : AspectMethodAttribute { }

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class AspectOnMethodExecAttribute : AspectMethodAttribute { }
	

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true, Inherited=false)]
	public class AspectAppliedAttribute : Attribute
	{
		public string AspectTypeGUID { get; private set; }
		public AspectAppliedAttribute(string aspectTypeGuid)
		{
			AspectTypeGUID = aspectTypeGuid;
		}
	}

}
