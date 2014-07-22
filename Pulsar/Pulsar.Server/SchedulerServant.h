#pragma once

using namespace System;

namespace Pulsar
{
	namespace Server
	{
		public ref class SchedulerServant : Servant
		{
		 public:
		
			SchedulerServant(void)
			{
			}
			public protected:
			virtual void Init(String^ objType )  override
			{
				Servant::Init(objType);
				static_cast<Pulsar::Scheduler^>(ServedObject)->NeedSave = gcnew Action(this,&Servant::Save);
			}
		};
	}
}

