#include "Stdafx.h"

using namespace Pulsar::Server;
using namespace Pulsar::Serialization;
using namespace System::Collections;
using namespace System::Data;
using namespace System::Data::SqlClient;
using namespace System::Reflection;
using namespace System::IO;

void Servant::Init(String^ objType)
{
	try
	{
		if(objType == nullptr)
			throw gcnew ArgumentNullException("objType");
		auto_handle<Type> t = Type::GetType(objType);
		if(t.get() == nullptr)
			throw gcnew PulsarException("Не удалось найти тип [{0}]!", objType);
		if(t.get() == this->GetType())
			ServedObject = this;
		else
		this->ServedObject = System::Runtime::Serialization::FormatterServices::GetUninitializedObject(t.get());


		//---
		System::Diagnostics::Stopwatch^ watch = nullptr;
		if(Logger::LogLevel >= 3)
			watch = System::Diagnostics::Stopwatch::StartNew();
		//---
		if(GOL::LoadGlobalObject(this, false))
			Status = ServantStatus::Ready;
		else	if(this->Equals(ServedObject) == false)
		{
			auto_handle<MethodBase> mi = t->GetConstructor(BindingFlags::Public | BindingFlags::NonPublic | BindingFlags::Instance,
				nullptr, Type::EmptyTypes, nullptr);
			if(mi.get() == nullptr)
				throw gcnew PulsarException("Не найден конструктор по умолчанию для типа [{0}]!", t->FullName);
			mi->Invoke(ServedObject, nullptr);
		}

			if(this->Equals(ServedObject) == false)
			{
				IObjectChangeNotify^ oi = dynamic_cast<IObjectChangeNotify^>(ServedObject);
				if(oi != nullptr)
				{
					oi->ObjectChanging += gcnew System::EventHandler<ObjectChangeNotifyEventArgs^>(this, &Servant::OnServedObjectChanging);
					oi->ObjectChanged += gcnew System::EventHandler<ObjectChangeNotifyEventArgs^>(this, &Servant::OnServedObjectChanged);
				}
			}
		//---
		if(watch != nullptr)
		{
			watch->Stop();
			Logger::Log(3, "\t({0:%s\\.fff})\t", false, watch->Elapsed);
			delete watch;
		}
		//---
	}
	catch(...)
	{
		Status = ServantStatus::Error;
		throw;
	}
}

void Servant::Save()
{	
	try
	{
		//---
		System::Diagnostics::Stopwatch^ watch = nullptr;
		if(Logger::LogLevel >= 3)
			watch = System::Diagnostics::Stopwatch::StartNew();
		//---

		GlobalObjectsManager::SaveGO(this);	

		//---
		if(watch != nullptr)
		{
			watch->Stop();
			Logger::Log(3, "   [{0}] saved ({1:%s\\.fff})", true, 
				static_cast<IGlobalObjectMeta^>(this)->GlobalName, watch->Elapsed);
			delete watch;
		}
		//---
	}
	catch(...)
	{
		this->Status = ServantStatus::Error;
		throw;
	}
}

Object^ Servant::Exec(PulsarQuery^ query)
{
	if(query == nullptr)
		if(ThreadContext::Query != nullptr)
			query = ThreadContext::Query;
		else
			throw gcnew Exception("Для метода Servant.Exec требуется объект запроса!");

	if(query->Query == nullptr || query->Query->Length == 0)
		if(CanGetFullObject)
			return _obj;
		else
			throw gcnew PulsarException("Слуга [{0}] объекта не поддерживает полную копию объекта!",
			  static_cast<IGlobalObjectMeta^>(this)->GlobalName);

	BindingFlags bf = BindingFlags::FlattenHierarchy | BindingFlags::Instance |
		BindingFlags::Public | BindingFlags::Static;

	if (query->Params.HasFlag(PulsarQueryParams::NonPublic))
		bf = bf | BindingFlags::NonPublic;

	Object^ res = TypesMembersChainingCall(_obj, query->Query, query->Args, bf,
	                                        query->Params.HasFlag(PulsarQueryParams::Modify));

	return res;
}

Object^ Servant::TypesMembersChainingCall(Object^ obj, String^ callChain, ParamsDic^ callsArgs, BindingFlags flag,
																																										bool writeLock)
{	
	array<String^>^ ss = nullptr; 
	array<MemberInfo^>^ mi = nullptr;
	array<Object^>^ args = nullptr;
	try
	{
		if(obj == nullptr)
			throw gcnew ArgumentNullException("obj");
		if(callChain == nullptr)
			throw gcnew ArgumentNullException("callChain");

		ss = callChain->Split('.');

		for each(String^ s in ss)
		{
			if(args != nullptr)
			{
				Array::Clear(args, 0, args->Length);
				delete args;
			}
			if(callsArgs != nullptr && callsArgs[s] != nullptr)
			{
				if(callsArgs[s]->GetType()->IsArray && dynamic_cast<array<Object^>^>(callsArgs[s]) != nullptr)
					args = dynamic_cast<array<Object^>^>(callsArgs[s]);
				else
					args = gcnew array<Object^> { callsArgs[s] };
			}

			mi = obj->GetType()->GetMember(s, MemberTypes::Method | MemberTypes::Property | MemberTypes::Field, flag);
			if(mi->Length == 0)
				throw gcnew PulsarException("Член класса {0} не найден!", s);
			if(mi->Length > 1)
			{
				if(args == nullptr)
					args = gcnew array<Object^>(0);
				array<ParameterInfo^>^ pis = nullptr;
				for each(MemberInfo^ i in mi)
				{
					if(pis != nullptr)
					{
						Array::Clear(pis,0,pis->Length);
						delete pis;
					}
					if(dynamic_cast<MethodInfo^>(i) != nullptr)
						pis = dynamic_cast<MethodInfo^>(i)->GetParameters();
					else if(dynamic_cast<PropertyInfo^>(i) != nullptr)
						pis = dynamic_cast<PropertyInfo^>(i)->GetIndexParameters();
					else
						continue;
					if(pis->Length != args->Length)
						continue;
					bool itis = true;
					for(int a = 0; a < pis->Length; a++)
						if(pis[a]->ParameterType->IsAssignableFrom(args[a]->GetType()) == false)
						{
							itis = false;
							break;
						}
						if(itis)
						{
							mi = gcnew array<MemberInfo^> { i };
							if (args->Length == 0)
								args = nullptr;
							break;
						}
				}
				if(mi->Length > 1)
					throw gcnew PulsarException("Обнаружена неоднозначность имен члена класса {0}!", s);
			}

			if(mi[0]->MemberType == MemberTypes::Property)
			{
				if(args == nullptr || dynamic_cast<PropertyInfo^>(mi[0])->GetIndexParameters()->Length > 0)
					obj = dynamic_cast<PropertyInfo^>(mi[0])->GetValue(obj, args);
				else
				{
					dynamic_cast<PropertyInfo^>(mi[0])->SetValue(obj, args[0], nullptr);
					obj = nullptr;
				}
			}
			else if(mi[0]->MemberType == MemberTypes::Field)
			{
				if(args == nullptr)
					obj = dynamic_cast<FieldInfo^>(mi[0])->GetValue(obj);
				else
				{
					dynamic_cast<FieldInfo^>(mi[0])->SetValue(obj, args[0]);
					obj = nullptr;
				}
			}
			else if(mi[0]->MemberType == MemberTypes::Method)
				obj = dynamic_cast<MethodInfo^>(mi[0])->Invoke(obj, args);
			else
				throw gcnew PulsarException("Метод Exec не может обработать тип члена {0}!", mi[0]->MemberType);
			if(obj == nullptr)
				break;
			//IReadWriteLockObject^ iobj = dynamic_cast<IReadWriteLockObject^>(obj);
			//if(iobj != null)
			//	if(writeLock)
			//	 iobj->BeginWrite();
			//	else
			//	 iobj->BeginRead();
		}
		if(dynamic_cast<System::Collections::IEnumerator^>(obj) != nullptr)
		{
			Type^ t;
			if(mi[0]->MemberType == MemberTypes::Property)
				t = dynamic_cast<PropertyInfo^>(mi[0])->PropertyType;
			else if(mi[0]->MemberType == MemberTypes::Method)
				t = dynamic_cast<MethodInfo^>(mi[0])->ReturnType;
			else
				t = Object::typeid;

			if(t->IsGenericType)
				t = t->GetGenericArguments()[0];

			Array^ arr = Array::CreateInstance(t, 100);
			int count = 0;
			for each(Object^ i in dynamic_cast<System::Collections::IEnumerable^>(obj))
			{
				if(arr->Length == count)
				{
					Array^ ar = Array::CreateInstance(t, arr->Length*2);
					Array::Copy(arr,ar,arr->Length);
					delete arr;
					arr = ar;
				}
				arr->SetValue(i, count);
				count++;
			}
			Array^ res = Array::CreateInstance(t, count);
			Array::Copy(arr,res,count);
			delete arr;
			obj = res;
			delete t;
		}
		return obj;
	}
	finally
	{
		if(ss != nullptr)
		{
			Array::Clear(ss,0, ss->Length);
			delete ss;
		}
		if(mi != nullptr)
		{
			Array::Clear(mi,0, mi->Length);
			delete mi;
		}
		if(args != nullptr)
		{
			Array::Clear(args,0, args->Length);
			delete args;
		}
	}
}
