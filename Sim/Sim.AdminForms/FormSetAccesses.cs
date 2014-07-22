using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Sim.Controls;
using Pulsar;
using Sim.Refs;
using Pulsar.Clients;
using Pulsar.Security;

namespace Sim.AdminForms
{
	//**************************************************************************************
	// Sim.AdminForms.frmSetAccesses
	/// <summary>
	/// Класс формы редактирования доступа пользователя.
	/// </summary>
	public partial class FormSetAccesses : Sim.ClientBaseForm
	{
		Persons users = null;
		PulsarSecurity pSec = null;
		PulsarMainMenu mMenu = null;

		private AccessToken at = null;
		private class ACEitem
		{
			[DisplayName("")]
			public Image Image { get; set; }
			[DisplayName("Группа/Пользователь")]
			public string Name { get; set; }
			internal ACE ACE { get; set; }
			[DisplayName("Запрет")]
			public bool AD
			{
				get { return ACE.IsDenied; }
				set { ACE.IsDenied = value; }
			}
			[DisplayName("Просмотр")]
			public bool Browse
			{
				get { return ACE.Browse; }
				set { ACE.Browse = value; }
			}
			[DisplayName("Уровень 1")]
			public bool L1
			{
				get { return ACE.Level1; }
				set { ACE.Level1 = value; }
			}
			[DisplayName("Уровень 2")]
			public bool L2
			{
				get { return ACE.Level2; }
				set { ACE.Level2 = value; }
			}
			[DisplayName("Уровень 3")]
			public bool L3
			{
				get { return ACE.Level3; }
				set { ACE.Level3 = value; }
			}
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			public ACEitem()
			{
				ACE = new ACE();
			}
		}
		private PList<ACEitem> ACL = new PList<ACEitem>();

		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public FormSetAccesses()
		{
			InitializeComponent();

			ComboBoxItem<byte> i;
			i = new ComboBoxItem<byte>(0, "Просмотр");
			comboBoxMainMenu.Items.Add(i);
			comboBoxDivTree.Items.Add(i);
			i = new ComboBoxItem<byte>(1, "Уровень 1");
			comboBoxMainMenu.Items.Add(i);
			comboBoxDivTree.Items.Add(i);
			i = new ComboBoxItem<byte>(2, "Уровень 2");
			comboBoxMainMenu.Items.Add(i);
			comboBoxDivTree.Items.Add(i);
			i = new ComboBoxItem<byte>(3, "Уровень 3");
			comboBoxMainMenu.Items.Add(i);
			comboBoxDivTree.Items.Add(i);
			
			comboBoxMainMenu.SelectedIndex = 0;
			comboBoxDivTree.SelectedIndex = 0;

			fdgvACL.DataSource = new ListBinder(ACL);
			DataGridViewColumnCollection c = fdgvACL.Columns;
			c[0].HeaderText = "";
			c[0].MinimumWidth = 20;
			c[0].Resizable = DataGridViewTriState.False;
			c[0].SortMode = DataGridViewColumnSortMode.Automatic;
			c[0].Width = 20;
			c[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			c[2].Width = 70;
			c[3].Width = 70;
			c[4].Width = 70;
			c[5].Width = 70;
			c[6].Width = 70;

			treeMainMenu.Comparer = delegate(SimTreeNode n1, SimTreeNode n2)
			{
				return PulsarMainMenu.PulsarMainMenuSorter.Default.Compare(n1.TreeItem, n2.TreeItem);
			};
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Form Handlers >>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			try
			{
				ShowProgressWindow();
				TaskManager.RunAfterAll(()=> 
				{
					treeMainMenu.Tree = mMenu;
					treeMainMenu.ExpandAll();
					treeMainMenu.CollapseAll();
					treeMainMenu.SelectNodeWithItem(mMenu.GetRootItems().First(), true);
					comboBoxUsers.SelectedIndex = 0;

				},
				new AsyncTask("Persons", this,()=> PulsarConnection.Default.Get("Persons")),
				new AsyncTask("Security", this,()=> PulsarConnection.Default.Get("Security")),
				new AsyncTask("MainMenu", this,()=> PulsarConnection.Default.Get("MainMenu"))
				);
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			treeDivisions.Tree = null;
			treeMainMenu.Tree = null;
			fdgvACL.DataSource = null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="task"></param>
		protected override void AsyncTaskDoneBody(AsyncTask task)
		{
			#region Persons
			if(task.TaskName == "Persons")
			{
				users = (Persons)task.Result;
				comboBoxUsers.Items.Add(new ComboBoxItem<Person>(null, " (Bсе)"));
				foreach(Person u in users)
					comboBoxUsers.Items.Add(new ComboBoxItem<Person>(u));
			}
			#endregion Persons
			#region Security
			if(task.TaskName == "Security")
			{
				pSec = (PulsarSecurity)task.Result;
			}
			#endregion Security
			#region MainMenu
			if(task.TaskName == "MainMenu")
			{
				mMenu = (PulsarMainMenu)task.Result;
			}
			#endregion MainMenu
			#region SetACEsForSD
			if(task.TaskName == "SetACEsForSD")
			{
				ValuesPair<OID, PList<ACE>> i = (ValuesPair<OID, PList<ACE>>)task.Tag;
				pSec.SetACEsForSD(i.Value1, i.Value2);
				btnCancel_Click(btnCancel, EventArgs.Empty);
				RecheckUserAccess();
			}
			#endregion SetACEsForSD
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if(comboBoxUsers.SelectedItem == null)
					return;
				RecheckUserAccess();
				if(treeMainMenu.HideSelection == false)
					FillACLs((FormInfo)treeMainMenu.SelectedNodeItem.Object);
				if(((ComboBoxItem<Person>)comboBoxUsers.SelectedItem).Key == null)
					btnAddCurrent.Enabled = btnSetPass.Enabled = false;
				else
					btnAddCurrent.Enabled = btnSetPass.Enabled = true;
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnSetPass_Click(object sender, EventArgs e)
		{
			try
			{
				Person u = ((ComboBoxItem<Person>)comboBoxUsers.SelectedItem).Key;
				string msg = String.Format("Веедите пароль для [{0}].\r\n NULL - пустой пароль (сброс пароля).", u.FullName);
				string pass = "";
				if(SimModalInputBox.Show(PanelBack, ref pass, "Установка пароля", msg,
								global::Sim.AdminForms.Properties.Resources.Keys) != System.Windows.Forms.DialogResult.OK)
					return;

				if(String.IsNullOrWhiteSpace(pass) || pass.Length < 3)
				{
					SimModalMessageBox.Show(PanelBack, "Пароль не соответствует требованиям безопасности!",
						"Ошибка ввода пароля", MessageBoxIcon.Error);
					return;
				}

				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Security", "SetUserPass",
					new { SetUserPass = new Object[] { u.OID, pass == "NULL" ? 0 : Hash.GetCRC32(pass)} }, 
					PulsarQueryParams.Modify);
				TaskManager.Run("SetPass", this, () => PulsarConnection.Default.Exec(q));

			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		#endregion << Controls Handlers >>
		//-------------------------------------------------------------------------------------
		#region << SimTreeView Handlers >>
		private void treeMainMenu_SelectedNodeChanged(object sender, ITreeItem item)
		{
			try
			{
				if(item == null)
				{
					toolStripAccess.Enabled = false;
					ACL.Clear();
					fdgvACL.Tag = null;
					return;
				}
				if(toolStripAccess.Enabled == false)
					toolStripAccess.Enabled = true;

				if(treeMainMenu.HideSelection == true)
				{
					treeDivisions.HideSelection = true;
					treeMainMenu.HideSelection = false;
				}
				FillACLs(((FormInfo)item.Object));
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void treeDivisions_SelectedNodeChanged(object sender, object nodeKey)
		{
			//try
			//{
			// if(treeDivisions.HideSelection == true)
			// {
			//  treeMainMenu.HideSelection = true;
			//  treeDivisions.HideSelection = false;
			// }
			// FillACLs((SimLinkedListItem<object>)nodeKey);
			//}
			//catch(Exception Err)
			//{
			// Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			//}
		}
		//-------------------------------------------------------------------------------------
		private void treeMainMenu_Enter(object sender, EventArgs e)
		{
			if(treeMainMenu.SelectedNodeItem != null)
			{
				treeDivisions.HideSelection = true;
				treeMainMenu.HideSelection = false;
			}
		}
		//-------------------------------------------------------------------------------------
		private void treeDivisions_Enter(object sender, EventArgs e)
		{
			//if(treeDivisions.SelectedNodeKey != null)
			//{
			// treeMainMenu.HideSelection = true;
			// treeDivisions.HideSelection = false;
			//}
		}
		#endregion << SimTreeView Handlers >>
		//-------------------------------------------------------------------------------------
		#region << ACLs Handlers >>
		private void fdgvACL_SelectionChanged(object sender, EventArgs e)
		{
			if(fdgvACL.SelectedRows.Count == 0)
				btnDel.Enabled = false;
			else
				btnDel.Enabled = true;
		}
		//-------------------------------------------------------------------------------------
		private void fdgvACL_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if(e.ColumnIndex < 0 || e.RowIndex < 0)
					return;
				if(fdgvACL[e.ColumnIndex, e.RowIndex] is DataGridViewCheckBoxCell == false)
					return;
				ACEitem acl = (ACEitem)fdgvACL.Rows[e.RowIndex].GetData();
				PropertyDescriptor pd = fdgvACL.GetDescriptor(fdgvACL.Columns[e.ColumnIndex].DataPropertyName);
				pd.SetValue(acl, !((bool)fdgvACL[e.ColumnIndex, e.RowIndex].Value));
				if(btnSave.Enabled == false)
				{
					finistPanel1.Enabled = false;
					splitContainer1.Panel1.Enabled = false;
					btnSave.Enabled = true;
					btnCancel.Enabled = true;
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnAddGroups_Click(object sender, EventArgs e)
		{
			try
			{
				PList<SecurityItem> list = new PList<SecurityItem>();
				foreach(SecurityGroup g in pSec.SecurityGroups.Values)
				{
					bool need = true;
					foreach(ACEitem i in ACL)
						if(i.ACE.SID ==  g.SID)
						{
							need = false;
							break;
						}
					if(need)
						list.Add(SecurityItem.FromGroup(g));
				}

				SimModalMultiChoiceBox frm = new SimModalMultiChoiceBox();
				frm.Parent = PanelBack;
				frm.DialogClosed += new DialogClosedEventHandler(SimModalMultiChoiceBox_DialogClosed);
				frm.CaptionImage = global::Sim.AdminForms.Properties.Resources.Group;
				frm.CaptionText = "Добавление групп";
				frm.VariantsCaption = "Доступные группы";
				frm.ChoicesCaption = "Выбранные группы";
				frm.Variants = new ListBinder(list);
				frm.Show();
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		void SimModalMultiChoiceBox_DialogClosed(object sender, DialogResult result)
		{
			try
			{
				if(btnSave.Enabled)
				{
					finistPanel1.Enabled = false;
					splitContainer1.Panel1.Enabled = false;
					btnSave.Enabled = true;
					btnCancel.Enabled = true;
				}

				SimModalMultiChoiceBox frm = (SimModalMultiChoiceBox)sender;
				frm.DialogClosed -= new DialogClosedEventHandler(SimModalMultiChoiceBox_DialogClosed);
				if(result == DialogResult.Cancel)
					return;

				if(btnSave.Enabled == false)
				{
					finistPanel1.Enabled = false;
					splitContainer1.Panel1.Enabled = false;
					btnSave.Enabled = true;
					btnCancel.Enabled = true;
				}
				
				OID sd = (OID)fdgvACL.Tag;
				foreach(object i in frm.Choices)
				{
					ACEitem a = new ACEitem();
					a.ACE = new ACE();
					a.ACE.SID = ((SecurityItem)i).SID;
					a.ACE.SD = sd;
					a.Image = ((SecurityItem)i).Image;
					a.Name = ((SecurityItem)i).Name;
					a.Browse = true;
					ACL.Add(a);
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnAddUsers_Click(object sender, EventArgs e)
		{
			try
			{
				PList<SecurityItem> list = new PList<SecurityItem>();
				foreach(Person u in users)
				{
					bool need = true;
					foreach(ACEitem i in ACL)
						if(i.ACE.SID ==  u.OID)
						{
							need = false;
							break;
						}
					if(need)
						list.Add(SecurityItem.FromUser(u));
				}

				SimModalMultiChoiceBox frm = new SimModalMultiChoiceBox();
				frm.Parent = PanelBack;
				frm.DialogClosed += new DialogClosedEventHandler(SimModalMultiChoiceBox_DialogClosed);
				frm.Tag = 2;
				frm.CaptionImage = global::Sim.AdminForms.Properties.Resources.User;
				frm.CaptionText = "Добавление пользователей";
				frm.VariantsCaption = "Доступные пользователи";
				frm.ChoicesCaption = "Выбранные пользователи";
				frm.Variants = new ListBinder(list);
				frm.Show();
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnDel_Click(object sender, EventArgs e)
		{
			try
			{
				if(btnSave.Enabled == false)
				{
					finistPanel1.Enabled = false;
					splitContainer1.Panel1.Enabled = false;
					btnSave.Enabled = true;
					btnCancel.Enabled = true;
				}

				foreach(DataGridViewRow r in fdgvACL.SelectedRows)
					ACL.Remove((ACEitem)r.GetData());

				//RecheckUserAccess();
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void buttonAddCurrent_Click(object sender, EventArgs e)
		{
			try
			{
				OID sid = ((ComboBoxItem<Person>)comboBoxUsers.SelectedItem).Key.OID;
				if(sid == null)
					return;

				foreach(ACEitem i in ACL)
					if(i.ACE.SID == sid)
						return;

				object sd = null;
				if(treeMainMenu.HideSelection == false)
					sd = ((FormInfo)treeMainMenu.SelectedNodeItem.Object).SD;
				//else if(treeDivisions.HideSelection == false)
				// sd = ((ISimLinkedListItem)treeDivisions.SelectedNodeDataObject).Props["SD"];
				else
					throw new Exception("Не определен защищаемый элемент!");

				ACEitem ai = new ACEitem();
				ai.ACE.SD = (OID)sd;
				ai.ACE.SID = sid;
				ai.Browse = true;
				ai.Image = global::Sim.AdminForms.Properties.Resources.User;
				ai.Name = ((ComboBoxItem<Person>)comboBoxUsers.SelectedItem).Key.FullName;
				ACL.Add(ai);

				if(btnSave.Enabled == false)
				{
					finistPanel1.Enabled = false;
					splitContainer1.Panel1.Enabled = false;
					btnSave.Enabled = true;
					btnCancel.Enabled = true;
				}

			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			finistPanel1.Enabled = true;
			splitContainer1.Panel1.Enabled = true;
			btnSave.Enabled = false;
			btnCancel.Enabled = false;
			if(treeMainMenu.HideSelection == false)
				treeMainMenu_SelectedNodeChanged(treeMainMenu, treeMainMenu.SelectedNodeItem);
		}
		//-------------------------------------------------------------------------------------
		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				OID sd = (OID)fdgvACL.Tag;
				PList<ACE> list = new PList<ACE>(ACL.Count);
				foreach(ACEitem i in ACL)
					list.Add(i.ACE);
				if(pSec.ACEs.ContainsKey(sd))
					foreach(ACE i in pSec.ACEs[sd])
						if(at != null && at.Contains(i.SID) == false)
							list.Add(i);

				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Security", "SetACEsForSD",
					new { SetACEsForSD = new Object[] { sd, list } }, PulsarQueryParams.Modify);
				TaskManager.Run("SetACEsForSD",this,()=> PulsarConnection.Default.Exec(q),
					new ValuesPair<OID, PList<ACE>>(sd, list));

			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
				HideProgressWindow();
			}
		}
		#endregion << ACLs Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Others Methods >>
		private void FillACLs(FormInfo fi)
		{
			try
			{
				OID sd = fi.SD;
				ACL.Clear();
				ACL.EventsOff();

				if(pSec.ACEs.ContainsKey(sd))
					foreach(ACE ace in pSec.ACEs[sd])
					{
						if(at != null && at.Contains(ace.SID) == false)
							continue;
						if(ace.SD == sd)
						{
							ACEitem i = new ACEitem();
							i.ACE = ace.Clone();
							IPersonSubject u = null;
							if(pSec.SecurityGroups.ContainsKey(ace.SID))
							{
								i.Image = global::Sim.AdminForms.Properties.Resources.Group;
								i.Name = pSec.SecurityGroups[ace.SID].Name;
							}
							else if((u = users[ace.SID]) != null)
							{
								i.Image = global::Sim.AdminForms.Properties.Resources.User;
								i.Name = u.FullName;
							}
							else
							{
								i.Image = global::Sim.AdminForms.Properties.Resources.UnknownGroup;
								i.Name = ace.SID.ToString();
							}
							ACL.Add(i);
						}
					}
				ACL.EventsOn();
				fdgvACL.Tag = sd;
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void RecheckUserAccess()
		{
			try
			{
				if(((ComboBoxItem<Person>)comboBoxUsers.SelectedItem).Key == null)
				{
					at = null;
					PulsarSecurity.ResetTreeAccesses(mMenu);
				}
				else
				{
					OID sid = ((ComboBoxItem<Person>)comboBoxUsers.SelectedItem).Key.OID;
					at = pSec.GetAccessToken(sid);
					pSec.CalcTreeAccesses(mMenu, at);
				}

				foreach(TreeItem<FormInfo> i in mMenu)
				{
					SecurityItemAccess fia = (SecurityItemAccess)i.Params["Access"];
					SecurityAccess fa = SecurityAccess.NotSet;
					switch(((ComboBoxItem<byte>)comboBoxMainMenu.SelectedItem).Key)
					{
						case 0: fa = fia.Browse; break;
						case 1: fa = fia.Level1; break;
						case 2: fa = fia.Level2; break;
						case 3: fa = fia.Level3; break;
					}

					SimTreeNode node = treeMainMenu.FindNode(i);
					if((fa & SecurityAccess.Set) == SecurityAccess.Set)
						node.Enabled = true;
					else if((fa & SecurityAccess.Browse) == SecurityAccess.Browse)
						node.ForeColor = Color.Brown;
					else
						node.Enabled = false;
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		#endregion << Others Methods >>
	}
}

