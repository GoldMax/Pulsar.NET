// PulsarConsole.cpp : main project file.

#include "stdafx.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Reflection;
using namespace System::Xml;
using namespace Pulsar::AOP;
using namespace Pulsar::Server;

List<String^>^ NoAOPAssemblies();

int main(array<System::String ^> ^args)
{
	try
	{
		System::Diagnostics::Debug::WriteLine("Pulsar:Применение аспектов ...");
		AOPRoot::Init();
		List<String^>^ noAOP = NoAOPAssemblies();

		String^ path = System::IO::Path::GetDirectoryName( AppDomain::CurrentDomain->BaseDirectory);
		for each(String^ f in System::IO::Directory::EnumerateFiles(path, "*.dll"))
		{
		 String^ fName = System::IO::Path::GetFileName(f);
			if(fName->StartsWith("Sim.") == false || noAOP->Contains(fName))
				continue;
			if(AOPRoot::ApplyAspects(f))
				System::Diagnostics::Debug::WriteLine(String::Format("Pulsar: {0}", fName));
		}
		System::Diagnostics::Debug::WriteLine("Pulsar:Применение аспектов завершено.");

		//Assembly::LoadFrom("Mono.Cecil.dll");
		//Assembly^ assm = Assembly::LoadFrom(AppDomain::CurrentDomain->BaseDirectory + "Bin\\Pulsar.AOP.dll");
		//assm->GetType("Pulsar.AOP.AOPRoot")->InvokeMember("Init", 
		// BindingFlags::InvokeMethod | BindingFlags::Static | BindingFlags::Public,
		//                                                   nullptr,nullptr,nullptr);

		//Assembly::LoadFrom("Pulsar.dll");
		//Assembly::Load("Pulsar.Serialization.dll");

		//Assembly^ assm = Assembly::LoadFrom("Pulsar.Core.dll");
		//Type^ t = assm->GetType("Pulsar.Server.PulsarCore" );
		//Object^ core = Activator::CreateInstance(t, true);
		//t->InvokeMember("Start", BindingFlags::InvokeMethod | BindingFlags::Instance | BindingFlags::Public,
		//                nullptr,core,nullptr);


		noAOP = null;

		PulsarCore^ core = 
			PulsarCore::CreateWithOID<PulsarCore^>(gcnew Pulsar::OID("BB8E4DD6-3D91-4543-BB95-6F16C6EC56F4"),nullptr,nullptr);	  
		core->Start();
		return 0;
	}
	catch(Exception^ exc)
	{
		System::Diagnostics::Debug::WriteLine(exc->Message);
		System::Diagnostics::Debug::WriteLine(exc->StackTrace);
	}
}

List<String^>^ NoAOPAssemblies()
{
 List<String^>^ res = gcnew  List<String^>(0);
 String^ path = System::IO::Path::GetDirectoryName(AppDomain::CurrentDomain->BaseDirectory);
	path += "\\debug.xml";
 if(System::IO::File::Exists(path) == false)
	 return res;
	XmlDocument doc;
	doc.Load(path);
	XmlNode^ root = doc.DocumentElement;

	for each(Object^ n in root)
	{
		XmlNode^ node = static_cast<XmlNode^>(n);
		if(node->HasChildNodes == false || node->Name != "NoAOP")
			continue;

		for each(XmlNode^ cn in node->ChildNodes)
		{
			if(cn->Name != "File")
				continue;
			res->Add(cn->InnerText);
		}
	}
	return res;
}
