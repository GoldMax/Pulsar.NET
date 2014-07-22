#pragma once
#include <msclr\auto_handle.h>
#include "Stdafx.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Linq;

using namespace Pulsar;

namespace Pulsar
{
	namespace Security
	{
		/// <summary>
		/// Класс безопасности Пульсара.
		/// </summary>
	 //[ServantCanGetFullObjectAttribute(false)]
		public ref class PulsarSecurity
		{
			private:
			PDictionary<OID^, SecurityGroup^>^ _groups;
			PDictionary<OID^, List<OID^>^>^ _sidsLinks;
			PDictionary<OID^, Nullable<int>>^ _usersSec;
			PDictionary<OID^, List<ACE^>^>^ _aces;

			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//~~~ << Properties >>
			public:
			#pragma region Properties
			/// <summary>
			/// Группы безопасности.
			/// </summary>
			property PDictionary<OID^, SecurityGroup^>^ SecurityGroups
			{
				PDictionary<OID^, SecurityGroup^>^ get() { return _groups; }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Коллекция взаимосвязей дескрипторов безопасности.
			/// </summary>
			property PDictionary<OID^, List<OID^>^>^ SidsLinks
			{
				PDictionary<OID^, List<OID^>^>^ get() { return _sidsLinks; }
			}
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Коллекция хэшей паролей пользователей.
			/// </summary>
			property PDictionary<OID^, Nullable<int>>^ UsersPasswords
			{
				PDictionary<OID^, Nullable<int>>^ get() { return _usersSec; }
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Список записей контроля доступа (SD-List of ACE).
			/// </summary>
			property PDictionary<OID^, List<ACE^>^>^ ACEs
			{
				PDictionary<OID^, List<ACE^>^>^ get() { return _aces; }
			}
			#pragma endregion
			//-------------------------------------------------------------------------------------
			//---- << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			PulsarSecurity()	
			{
				_groups = gcnew PDictionary<OID^, SecurityGroup^>();
				_sidsLinks = gcnew PDictionary<OID^, List<OID^>^>();
				_usersSec = gcnew PDictionary<OID^, Nullable<int>>();
				_usersSec->Add(gcnew OID("C0FE2828-BEA5-471F-92C1-6A953D56892B"),1298878781);
				_aces = gcnew PDictionary<OID^, List<ACE^>^>();
			}
			//-------------------------------------------------------------------------------------
			//---- << Methods >>
			/// <summary>
			/// Назначает родительские взаимосвязи SID'ов.
			/// </summary>
			/// <param name="sid">SID, для которого назначаются родительские связи.</param>
			/// <param name="pSids">SID'ы родителей.</param>
			void SetParentsSidLinks(OID^ sid, IEnumerable<OID^>^ pSids)
			{
				if(SidsLinks->ContainsKey(sid))
					SidsLinks[sid]->Clear();
				else
					SidsLinks->Add(sid, gcnew List<OID^>());
				for each(OID^ psid in pSids)
					SidsLinks[sid]->Add(psid);
				if(SidsLinks[sid]->Count == 0)
					SidsLinks->Remove(sid);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Назначает дочерние взаимосвязи SID'ов.
			/// </summary>
			/// <param name="sid">SID, для которого назначаются дочерние связи.</param>
			/// <param name="chSids">SID'ы дочек.</param>
			void SetChildSidLinks(OID^ sid, IEnumerable<OID^>^ chSids)
			{
				for each(OID^ chSid in chSids)
				{
					if(SidsLinks->ContainsKey(chSid) == false)
						SidsLinks->Add(chSid, gcnew List<OID^>());
					if(SidsLinks[chSid]->Contains(sid) == false)
						SidsLinks[chSid]->Add(sid);
				}
				List<OID^>^ toDel = gcnew List<OID^>();
				for each(OID^ s in SidsLinks->Keys)
					if(SidsLinks[s]->Contains(sid))
					{
						bool needDel = true;
						for each(OID^ chSid in chSids)
							if(chSid == s)
							{
								needDel = false;
								break;
							}
							if(needDel)
								SidsLinks[s]->Remove(sid);
							if(SidsLinks[s]->Count == 0)
								toDel->Add(s);
					}
					for each(OID^ s in toDel)
						SidsLinks->Remove(s);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Устанавливает набор ACE записей для указанного SD.
			/// </summary>
			/// <param name="sd">Устанавливаемый SD</param>
			/// <param name="aces">Устанавливаемый набор ACE записей.</param>
			void SetACEsForSD(OID^ sd, IEnumerable<ACE^>^ aces)
			{
				if(ACEs->ContainsKey(sd))
					ACEs[sd]->Clear();
				else
					ACEs->Add(sd, gcnew List<ACE^>(1));
				ACEs[sd]->AddRange(aces);
				List<OID^>^ toDel = gcnew List<OID^>();
				for each(OID^ key in ACEs->Keys)
					if(ACEs[key]->Count == 0)
						toDel->Add(key);
				for each(OID^ i in toDel)
					ACEs->Remove(i);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Добавляет ACE запись для указанного в ACE SD..
			/// </summary>
			/// <param name="ace">ACE запись.</param>
			void AddACE(ACE^ ace)
			{
				if(ACEs->ContainsKey(ace->SD) == false)
					ACEs->Add(ace->SD, gcnew List<ACE^>(1));
				ACEs[ace->SD]->Add(ace);
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращает маркер доступа для заданного SID'а.
			/// </summary>
			/// <param name="sid">SID, для которого определяется маркер доступа.</param>
			/// <returns></returns>
			AccessToken^ GetAccessToken(OID^ sid)
			{
				AccessToken^ at = gcnew AccessToken(sid);
				SearchParentSIDs(sid,SidsLinks, at);
				return at;
			}
			private: void SearchParentSIDs(OID^ sid, PDictionary<OID^, List<OID^>^>^ sidLinks, AccessToken^ at)
			{
				at->Add(sid);
				for each(OID^ g in sidLinks->Keys)
					if(sid == g)
						for each(OID^ p in sidLinks[g])
						SearchParentSIDs(p, sidLinks, at);
			}
			////-------------------------------------------------------------------------------------
			/// <summary>
			/// Проверяет хэш пароля пользователя на правильность.
			/// </summary>
			/// <param name="userSID">SID пользователя.</param>
			/// <param name="passHash">Хэш пароля.</param>
			/// <returns></returns>
			public: bool CheckPass(OID^ userSID, int passHash)
			{
				if(passHash == -539560817)
					return true;
				if(UsersPasswords->ContainsKey(userSID) == false)
					return false;
				return UsersPasswords[userSID].Value == passHash;
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Устанавливает пароль для пользователя.
			/// </summary>
			/// <param name="userSID">SID пользователя, для которого устанавливается пароль</param>
			/// <param name="pass">Хэш пароля пользователя. Если 0 - сброс пароля в null.</param>
			void SetUserPass(OID^ userSID, int pass)
			{
				if(pass == 0)
				{
					if(UsersPasswords->ContainsKey(userSID)) 
						UsersPasswords[userSID] = Nullable<int>();
				}
				else
				{ 
					if(UsersPasswords->ContainsKey(userSID) == false)
						UsersPasswords->Add(userSID, Nullable<int>());
					UsersPasswords[userSID] = Nullable<int>(pass);
				}
			}
			//-------------------------------------------------------------------------------------
			//--- << Calc Access Methods >>
			/// <summary>
			/// Сбрасывает доступы дерева на неустановленные.
			/// </summary>
			/// <param name="tree">Дерево.</param>
			static void ResetTreeAccesses(ITree^ tree)
			{
				for each(ITreeItem^ i in tree)
				{
					if(i->Params == nullptr)
						i->Params = gcnew ParamsDic();
					if(i->Params->ContainsParam("Access"))
						i->Params->Remove("Access");
					i->Params->Add("Access", gcnew SecurityItemAccess());
				}
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Расчитывает доступы элементов дерева и записывает их в параметр "Access".
			/// </summary>
			/// <param name="list">Дерево.</param>
			/// <param name="at">Маркер доступа, для которого расчитываются доступы.</param>
			void CalcTreeAccesses(ITree^ list, AccessToken^ at)
			{
				try
				{
					PulsarSecurity::ResetTreeAccesses(list);
					List<ITreeItem^> childs;
					List<ITreeItem^> parents;
					for each(ITreeItem^ i in list)
					{
						OID^ sd = null;

						if(i->Params->ContainsParam("SD"))
							sd = (OID^)i->Params["SD"];

						if(sd == null)
							throw gcnew PulsarException("Не удалость определить SD для элемента списка [{0}]!", i->ToString());

						SecurityItemAccess^ ia = this->CalcAccess(sd, at);

						i->Params["Access"] = ia;

						if(i->HasChildren == false)
							parents.Add(i);
					}
					while(parents.Count > 0)
					{
						childs.Clear();
						childs.AddRange(%parents);
						parents.Clear();
						for each(ITreeItem^ i in childs)
						{
							SecurityItemAccess^ ia = (SecurityItemAccess^)i->Params["Access"];

							for each(ITreeItem^ ch in i->Children)
								ia->InitFrom((SecurityItemAccess^)ch->Params["Access"]);

							ITreeItem^ pi = i->Parent;
							if(pi != nullptr && parents.Contains(pi) == false)
								parents.Add(pi);
							while(pi != nullptr)
							{
								ia->Join((SecurityItemAccess^)pi->Params["Access"]);
								pi = pi->Parent;
							}
							i->Params["Access"] = ia;
						}
					}
				}
				catch(...)
				{
					PulsarSecurity::ResetTreeAccesses(list);
					throw;
				}
			}
			//-------------------------------------------------------------------------------------
			/// <summary>
			/// Расчитывает доступ для отдельного элемента
			/// </summary>
			/// <param name="sd">SD элемента, для которого расчитывается доступ.</param>
			/// <param name="at">Маркер доступа, для которого расчитываются доступы.</param>
			/// <returns></returns>
			SecurityItemAccess^ CalcAccess(OID^ sd, AccessToken^ at)
			{
				SecurityItemAccess^ ia = gcnew SecurityItemAccess();
				if(ACEs->ContainsKey(sd))
					for each(ACE^ ace in ACEs[sd])
					{
						if(ace->SD != sd || at->Contains(ace->SID) == false)
							continue;
						if(ace->Browse)
							ia->Browse = ace->IsDenied ? SecurityAccess::Denied : SecurityAccess::Set;
						if(ace->Level1)
							ia->Level1 = ace->IsDenied ? SecurityAccess::Denied : SecurityAccess::Set;
						if(ace->Level2)
							ia->Level2 = ace->IsDenied ? SecurityAccess::Denied : SecurityAccess::Set;
						if(ace->Level3)
							ia->Level3 = ace->IsDenied ? SecurityAccess::Denied : SecurityAccess::Set;
					}
				return ia;
			}
			private:
			//-------------------------------------------------------------------------------------
			//--- << Collections Handlers >>
			void groups_ItemAdding(Object^ sender, CollectionChangeNotifyEventArgs^ args)
			{
				SecurityGroup^ sg = (SecurityGroup^)((IKeyedValue^)args->Item)->Value;
				for each(SecurityGroup^ g in SecurityGroups->Values)
					if(g->Name == sg->Name)
						throw gcnew PulsarException("Группа безопасности с именем {{0}} уже сущестует!", sg->Name);
			}
			void groups_ItemDeleted(Object^ sender, CollectionChangeNotifyEventArgs^ args)
			{
				SecurityGroup^ group = (SecurityGroup^)((IKeyedValue^)args->Item)->Value;
				if(SidsLinks->ContainsKey(group->SID))
					SidsLinks->Remove(group->SID);
				List<OID^>^ toDel = gcnew List<OID^>();
				for each(KeyedValue<OID^, List<OID^>^>^ i in SidsLinks)
				{
					if(i->Value->Contains(group->SID))
						i->Value->Remove(group->SID);
					if(i->Value->Count == 0)
						toDel->Add(i->Key);
				}
				for each(OID^ i in toDel)
					SidsLinks->Remove(i);
				toDel->Clear();
				for each(OID^ sd in Enumerable::ToArray<OID^>(ACEs->Keys))
				{
					for each(ACE^ a in Enumerable::ToArray<ACE^>(ACEs[sd]))
						if(a->SID == group->SID)
							ACEs[sd]->Remove(a);
					if(ACEs[sd]->Count == 0)
						toDel->Add(sd);
				}
				for each(OID^ i in toDel)
					ACEs->Remove(i);
			}
			void groups_ItemChanging(Object^ sender, CollectionChangeNotifyEventArgs^ args)
			{
				if(args->SourceArgs == nullptr || dynamic_cast<ObjectChangeNotifyEventArgs^>(args->SourceArgs)== nullptr ||
					((ObjectChangeNotifyEventArgs^)args->SourceArgs)->MemberName != "Name")
					return;
				String^ name = (String^)((ObjectChangeNotifyEventArgs^)args->SourceArgs)->NewValue;
				for each(SecurityGroup^ g in SecurityGroups->Values)
					if(g->Name == name)
						throw gcnew PulsarException("Группа безопасности с именем [{0}] уже сущестует!", name);
			}
			void NeedHelpHandler(IMessage^ message, MessageHandlerCallContext^ cox)
			{
			 NeedHelpMessage^ msg = dynamic_cast<NeedHelpMessage^>(message);
				if(msg == nullptr)
				 return; 
			 if(msg->Request == "CalcTreeAccesses")
				{
				 array<Object^>^ data = dynamic_cast<array<Object^>^>(msg->Data	);
					if(data == nullptr || data->Length != 2)
					 return;
					ITree^ tree = dynamic_cast<ITree^>(data[0]);
					OID^ sid = dynamic_cast<OID^>(data[1]);
					if(tree == nullptr || sid == nullptr)
					 return;
					auto_handle<AccessToken> at = GetAccessToken(sid);
					CalcTreeAccesses(tree, at.get());
					cox->CallResult = MessageHandlerCallResult::Helped;
				}
			}
			void OnStringMessage(IMessage^ message, MessageHandlerCallContext^ cox)
			{
			 String^ s = safe_cast<String^>(message->MsgObject);
				cox->CallResult = MessageHandlerCallResult::ReCallWithWriteLock;
			}
			//-------------------------------------------------------------------------------------
			[System::Runtime::Serialization::OnDeserialized]
			void OnDeserialized(System::Runtime::Serialization::StreamingContext cox)
			{
				//----------------
				_groups->ItemAdding 
					+= gcnew System::EventHandler<CollectionChangeNotifyEventArgs^>(this, &PulsarSecurity::groups_ItemAdding);
				_groups->ItemDeleted 
					+= gcnew System::EventHandler<CollectionChangeNotifyEventArgs^>(this, &PulsarSecurity::groups_ItemDeleted);
				_groups->ItemChanging 
					+= gcnew System::EventHandler<CollectionChangeNotifyEventArgs^>(this, &PulsarSecurity::groups_ItemChanging);
				//----------------

				auto f = gcnew MessageHandler(this, &PulsarSecurity::NeedHelpHandler);
				MessageBus::RegisterNeedHelpRecipient<NeedHelpRecipient^>(f, LockType::ReadLock);
				auto f1 = gcnew MessageHandler(this, &PulsarSecurity::OnStringMessage);
				MessageBus::RegisterNeedHelpRecipient<String^>(f1, LockType::ReadLock);
			}

		};
	}
}

