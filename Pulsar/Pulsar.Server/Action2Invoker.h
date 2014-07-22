#pragma once
namespace Pulsar
{
	namespace Server
	{
		interface class IAction2Invoker
		{
			void Invoke(Delegate^ del, Object^ arg1, Object^ arg2);
		};

		generic <class T1, class T2>	
		where T1 : ref class
		where T2 : ref class
		ref class Action2Invoker	 : IAction2Invoker
		{
			public:
			Action2Invoker()	{ }

			virtual void Invoke(Delegate^ del, Object^ arg1, Object^ arg2) = IAction2Invoker::Invoke
			{
			 if(del == nullptr)
				 throw gcnew ArgumentNullException("del");
			 T1 t1 = safe_cast<T1>(arg1);
				T2 t2 = safe_cast<T2>(arg2);
				//Action<Object^,T1,T2>^ act = dynamic_cast<Action<Object^,T1,T2>^>(del);
				Action<T1,T2>^ act = dynamic_cast<Action<T1,T2>^>(del);
				if(act == nullptr)
				 throw gcnew Exception(String::Format("Ну удалось привести делегат к типу {0}!", act::typeid));
				act(t1,t2);
			}

		};
	}
}
