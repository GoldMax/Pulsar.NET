using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pulsar;
using Pulsar.Security;
using Pulsar.Serialization;
//using Pulsar.SQL;

//using OID = System.Guid;

namespace Pulsar.Clients
{
	// Pulsar.Clients.PulsarMainMenu
	/// <summary>
	/// Класс главного меню.
	/// </summary>
	public class PulsarMainMenu : Tree<FormInfo>
	{
		public PulsarMainMenu()
		{
			FormInfo fi = new FormInfo();
			fi.Caption = "Сервер";
			fi.SD = new OID("d31bef9d-15a6-4cfa-94c2-06ea1b27bd09");
			var mi = this.Add(fi, (FormInfo)null);
			((ITreeItem)mi).SortOrder = (byte)10;
			fi = new FormInfo();
			fi.Caption = "Параметры подключения";
			fi.FormClassName = "SpecialConParams";
			fi.SD = new OID("bdab36b1-6b52-4863-a5bc-da0b94bf36a7");
			this.Add(fi,mi);

			fi = new FormInfo();
			fi.Caption = "Администрирование";
			fi.SD = new OID("b70a706c-8a76-40a7-ac09-e4ac1fd501ec");
			mi = this.Add(fi, (FormInfo)null);
			((ITreeItem)mi).SortOrder = (byte)20;

			fi = new FormInfo();
			fi.Caption = "Глобальные справочники";
			fi.SD = new OID("5775e818-3afc-4ab5-b387-88d37a463c32");
			var mii = this.Add(fi,mi);
			((ITreeItem)mii).SortOrder = (byte)10;
			fi = new FormInfo();
			fi.Caption = "Персоны";
			fi.FormClassName = "Sim.AdminForms.FormPersons, Sim.AdminForms";
			fi.SD = new OID("7827cf75-7bf6-4c6f-b5ab-745478a77668");
			this.Add(fi, mii);

			fi = new FormInfo();
			fi.Caption = "Безопасность";
			fi.SD = new OID("3e418cdb-106f-4bee-ac59-10aceacc3529");
			mii = this.Add(fi, mi);
			((ITreeItem)mii).SortOrder = (byte)20;
			fi = new FormInfo();
			fi.Caption = "Группы безопасности";
			fi.FormClassName = "Sim.AdminForms.FormSecurityGroups, Sim.AdminForms";
			fi.SD = new OID("d70fa572-feaa-423f-ac71-17e4087c0459");
			this.Add(fi, mii);
			fi = new FormInfo();
			fi.Caption = "Назначение доступов";
			fi.FormClassName = "Sim.AdminForms.FormSetAccesses, Sim.AdminForms";
			fi.SD = new OID("13e82280-e06d-496e-8a80-d542fa29c649");
			this.Add(fi, mii);

			fi = new FormInfo();
			fi.Caption = "Пульсар";
			fi.SD = new OID("51671c2f-e567-46f1-9c2a-83e2b7d16251");
			mii = this.Add(fi, mi);
			((ITreeItem)mii).SortOrder = (byte)30;
			fi = new FormInfo();
			fi.Caption = "Консоль";
			fi.FormClassName = "Sim.AdminForms.FormConsole, Sim.AdminForms";
			fi.SD = new OID("6b502b6d-46af-4303-b29f-598c13d3aef6");
			this.Add(fi, mii);
			fi = new FormInfo();
			fi.Caption = "Редактор главного меню";
			fi.FormClassName = "Sim.AdminForms.FormMainMenu, Sim.AdminForms";
			fi.SD = new OID("fa3ec74a-7d00-4259-9083-e83aef25f28a");
			this.Add(fi, mii);

			OnDeserialized(new System.Runtime.Serialization.StreamingContext());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает объект MainMenu для указанного пользователя.
		/// </summary>
		/// <param name="userOID">OID пользователя.</param>
		/// <returns></returns>
		public PulsarMainMenu GetUserMainMenu(OID userOID)
		{
			if(userOID.ToString() == "c0fe2828-bea5-471f-92c1-6a953d56892b")
			 return this;
			
			PulsarMainMenu	menu = (PulsarMainMenu)PulsarSerializer.CloneObject(this);
			menu.EventsOff();

			if(MessageBus.NeedHelp("Security",	"CalcTreeAccesses", new object[] { menu, userOID }) == false)
			 return null;

			foreach(TreeItem<FormInfo> a in menu.GetRootItems().ToArray())
			{
				SecurityItemAccess ia  = (SecurityItemAccess)a.Params["Access"];
				if(ia.Browse != SecurityAccess.Browse && ia.Browse != SecurityAccess.Set)
					menu.Remove(a);
			}
			foreach(TreeItem<FormInfo> i in menu.GetEndItems().ToArray())
			{
				TreeItem<FormInfo> item = i;
				SecurityItemAccess ia  = (SecurityItemAccess)item.Params["Access"];
				if(ia.Browse != SecurityAccess.Browse && ia.Browse != SecurityAccess.Set)
				{
					TreeItem<FormInfo> p;
					do
					{
						p = item.Parent;
						menu.Remove(item);
						if(p == null || p.HasChildren)
							break;
						item = p;
					} while(item != null);
				}
			}
			foreach(TreeItem<FormInfo> item in menu.Items)
			{
				SecurityItemAccess ia  = (SecurityItemAccess)item.Params["Access"];
				item.Object.HasBrowseAccess = (ia.Browse == SecurityAccess.Browse || ia.Browse == SecurityAccess.Set);
				item.Object.HasLevel1Access = ia.Level1 == SecurityAccess.Set;
				item.Object.HasLevel2Access = ia.Level2 == SecurityAccess.Set;
				item.Object.HasLevel3Access = ia.Level3 == SecurityAccess.Set;
				item.Params["Access"] = null;
			}
			menu.EventsOn(false);
			return menu;
		}
		//-------------------------------------------------------------------------------------
		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(System.Runtime.Serialization.StreamingContext cox)
		{
			foreach(ITreeItem i in this)
			{
			 if(i.Params == null)
					i.Params = new ParamsDic(1);
				if(i.Params["SD"] == null)
				 i.Params["SD"] = ((FormInfo)i.Object).SD;
			}

		}
		//*************************************************************************************
		public class PulsarMainMenuSorter : IComparer<ITreeItem>
		{
			public static PulsarMainMenuSorter Default = new PulsarMainMenuSorter();

			#region IComparer<ITreeItem> Members

			public int Compare(ITreeItem x, ITreeItem y)
			{
				bool xByte = x.SortOrder is byte;
				bool yByte = y.SortOrder is byte;

				if(xByte && yByte == false)
					return -1;
				if(xByte == false && yByte)
					return 1;

				if ((xByte == true && yByte == true && (byte)x.SortOrder == (byte)y.SortOrder) 
					|| (xByte == false && yByte == false))
				{
					if (x.IsGroup && y.IsGroup == false)
						return -1;
					if (x.IsGroup == false && y.IsGroup)
						return 1;
					if (x.HasChildren && y.HasChildren == false)
						return -1;
					if (x.HasChildren == false && y.HasChildren)
						return 1;

					decimal xd, yd;
					if (decimal.TryParse(x.ItemText.Replace(".", ","), out xd)
						&& decimal.TryParse(y.ItemText.Replace(".", ","), out yd))
						return decimal.Compare(xd, yd);

					return string.Compare(x.ItemText, y.ItemText);
				}

				return ((byte)x.SortOrder).CompareTo((byte)y.SortOrder);
			}

			#endregion
		}
	}
}
