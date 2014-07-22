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
using Pulsar.Clients;
using Pulsar.Security;

namespace Sim.AdminForms
{
	// Sim.AdminForms.frmSecurityGroups
	/// <summary>
	/// Класс формы просмотра, создания и редактирования групп безопасности
	/// </summary>
	public partial class FormSecurityGroups : Sim.ClientBaseForm
	{
		Persons users = null;
		PulsarSecurity psec = null;

		PList<SecurityItem> parentsList = new PList<SecurityItem>();
		PList<SecurityItem> childsList = new PList<SecurityItem>();
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public FormSecurityGroups()
		{
			InitializeComponent();

			fdgvGroups.AllowAutoGenerateColumns = false;
			fdgvParents.AllowAutoGenerateColumns = false;
			fdgvParents.DataSource = new ListBinder(parentsList);
			fdgvChilds.AllowAutoGenerateColumns = false;
			fdgvChilds.DataSource = new ListBinder(childsList);
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
			try
			{
				base.OnLoad(e);
				ShowProgressWindow();
				TaskManager.RunQueue(
					new AsyncTask("Persons", this, () => PulsarConnection.Default.Get("Persons")),
					new AsyncTask("Security", this, () => PulsarConnection.Default.Get("Security"))
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
			fdgvGroups.DataSource = null;
			fdgvParents.DataSource = null;
			fdgvChilds.DataSource = null;
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
			}
			#endregion Persons
			#region Security
			if(task.TaskName == "Security")
			{
				psec = (PulsarSecurity)task.Result;
				fdgvGroups.DataSource = new DictionaryBinder(psec.SecurityGroups);
			}
			#endregion Security
			#region AddGroup
			if(task.TaskName == "AddGroup")
			{
				SecurityGroup gr = (SecurityGroup)task.Tag;
				psec.SecurityGroups.Add(gr.SID, gr);
			}
			#endregion AddGroup
			#region UpdNameGroup
			if(task.TaskName == "UpdNameGroup")
			{
				SecurityGroup gr = (SecurityGroup)task.Tag;
				psec.SecurityGroups[gr.SID].Name = gr.Name;
			}
			#endregion UpdNameGroup
			#region UpdDescGroup
			if(task.TaskName == "UpdDescGroup")
			{
				SecurityGroup gr = (SecurityGroup)task.Tag;
				psec.SecurityGroups[gr.SID].Description = gr.Description;
			}
			#endregion UpdDescGroup
			#region DelGroup
			if(task.TaskName == "DelGroup")
			{
				SecurityGroup gr = (SecurityGroup)task.Tag;
				psec.SecurityGroups.Remove(gr.SID);
			}
			#endregion DelGroup
			#region SetParents
			if(task.TaskName == "SetParents")
			{
				ValuesPair<OID,List<OID>> i = (ValuesPair<OID,List<OID>>)task.Tag;
				psec.SetParentsSidLinks(i.Value1, i.Value2);
				fdgvGroups_SelectionChanged(null, null);
			}
			#endregion SetParents
			#region SetChilds
			if(task.TaskName == "SetChilds")
			{
				ValuesPair<OID,List<OID>> i = (ValuesPair<OID,List<OID>>)task.Tag;
				psec.SetChildSidLinks(i.Value1, i.Value2);
				fdgvGroups_SelectionChanged(null, null);
			}
			#endregion SetChilds
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << toolStripGroups buttons handlers >>
		/// <summary>
		/// Создание новой группы.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolStripButtonAddGroup_Click(object sender, EventArgs e)
		{
			try
			{
				SimModalDualInputBox box = new SimModalDualInputBox();
				box.CaptionImage = global::Sim.AdminForms.Properties.Resources.Group;
				box.CaptionText = "Создание группы безопасности";
				box.Parent = this.PanelBack;
				box.DialogClosed += new Sim.Controls.DialogClosedEventHandler(frm_DialogClosed);
				box.Tag = null;
				box.Text1 = "Наименование группы безопасности";
				box.Show();
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
				HideProgressWindow();
			}
		}
		//-------------------------------------------------------------------------------------
		void frm_DialogClosed(object sender, DialogResult result)
		{
			try
			{
				SimModalDualInputBox frm = (SimModalDualInputBox)sender;
				frm.DialogClosed -= new Sim.Controls.DialogClosedEventHandler(frm_DialogClosed);
				if(result == DialogResult.Cancel)
					return;
				if(frm.Tag == null)
				{
					foreach(SecurityGroup g in psec.SecurityGroups.Values)
						if(g.Name.ToLower() == frm.Value1.ToLower())
						{
							string text = String.Format("Группа безопасности \"{0}\" уже существует.\nВведите другое имя группы.",
									frm.Value1);
							SimModalMessageBox.Show(PanelBack, text, "Ошибка создания группы безопасности", MessageBoxIcon.Error);
							return;
						}
					SecurityGroup gr = new SecurityGroup();
					gr.Name = frm.Value1;
					gr.Description = frm.Value2;

					ShowProgressWindow();
					PulsarQuery q = new PulsarQuery("Security", "SecurityGroups.Add", new { Add = new object[] {gr.SID, gr } },
																																					PulsarQueryParams.Modify);
					TaskManager.Run("AddGroup", this, () => PulsarConnection.Default.Exec(q), gr);
				}
				else
				{
					SecurityGroup gr = ((SecurityGroup)frm.Tag).Clone();
					gr.Name = frm.Value1;
					gr.Description = frm.Value2;

					ShowProgressWindow();
					PulsarQuery q = new PulsarQuery("Security", "SecurityGroups.Item.Name", new { Item = gr.SID, Name = gr.Name },
																																PulsarQueryParams.Modify);
					TaskManager.Run("UpdNameGroup", this, () => PulsarConnection.Default.Exec(q), gr);
					PulsarQuery q2 = new PulsarQuery("Security", "SecurityGroups.Item.Description", 
																								new { Item = gr.SID, Description = gr.Description }, PulsarQueryParams.Modify);
					TaskManager.Run("UpdDescGroup", this, () => PulsarConnection.Default.Exec(q2), gr);
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			} 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаление группы.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelGroup_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvGroups.SelectedRows.Count == 0)
				{
					btnDelGroup.Enabled = false;
					return;
				}
				SecurityGroup gr = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				if(SimModalMessageBox.Show(PanelBack, "Вы действительно желаете удалить группу " + gr.Name + "?",
								"Удаление группы безопасности", MessageBoxIcon.Question, MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;

				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Security", "SecurityGroups.Remove", new { Remove = gr.SID },
																																PulsarQueryParams.Modify);
				TaskManager.Run("DelGroup", this, () => PulsarConnection.Default.Exec(q), gr);
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Пререименование группы.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRenameGroup_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvGroups.SelectedRows.Count == 0)
				{
					btnRenameGroup.Enabled = false;
					return;
				}
				SecurityGroup g = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;

				SimModalDualInputBox box = new SimModalDualInputBox();
				box.CaptionImage = global::Sim.AdminForms.Properties.Resources.Group;
				box.CaptionText = "Переименование группы безопасности";
				box.Parent = this.PanelBack;
				box.DialogClosed += new Sim.Controls.DialogClosedEventHandler(frm_DialogClosed);
				box.Text1 = "Наименование группы безопасности";
				box.Tag = g;
				box.Value1 = g.Name;
				box.Value2 = g.Description;
				box.Show();
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
				HideProgressWindow();
			}
		}
		#endregion << toolStripGroups buttons handlers >>
		//-------------------------------------------------------------------------------------
		#region << DataGridViews Handlers >>
		private void fdgvGroups_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				childsList.Clear();
				parentsList.Clear();
				if(fdgvGroups.SelectedRows.Count == 0)
				{
					labelGroupName.Text = "";
					flSID.Text = "";
					btnDelGroup.Enabled = false;
					btnRenameGroup.Enabled = false;

					toolStripParentGroups.Enabled = false;
					toolStripChildGroups.Enabled = false;
					return;
				}

				btnDelGroup.Enabled = true;
				btnRenameGroup.Enabled = true;

				toolStripParentGroups.Enabled = true;
				toolStripChildGroups.Enabled = true;

				SecurityGroup group = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				labelGroupName.Text = group.Name;
				flSID.Text = group.SID.ToString();

				if(psec.SidsLinks.ContainsKey(group.SID))
					foreach(OID sid in psec.SidsLinks[group.SID])
					{
						if(psec.SecurityGroups.ContainsKey(sid))
							parentsList.Add(SecurityItem.FromGroup(psec.SecurityGroups[sid]));
						else
						{
							SecurityItem i = new SecurityItem();
							i.SID = sid;
							i.Name = "(Unknown)";
							i.Desc = sid.ToString();
							i.Image = global::Sim.AdminForms.Properties.Resources.UnknownGroup;
							parentsList.Add(i);
						}
					}

				foreach(OID sid in psec.SidsLinks.Keys)
					if(psec.SidsLinks[sid].Contains(group.SID))
					{
						if(psec.SecurityGroups.ContainsKey(sid))
						{
							childsList.Add(SecurityItem.FromGroup(psec.SecurityGroups[sid]));
							continue;
						}

						SecurityItem i = new SecurityItem();
						foreach(Person u in users)
							if(u.OID == sid)
							{
								i.SID =  u.OID;
								i.Name = u.ShortName;
								i.Desc = u.FullName;
								i.Image = global::Sim.AdminForms.Properties.Resources.User;
								break;
							}

						if(i.Image == null)
						{
							i.SID = sid;
							i.Name = "(Unknown)";
							i.Desc = sid.ToString();
							i.Image = global::Sim.AdminForms.Properties.Resources.UnknownGroup;
						}
						childsList.Add(i);
					}
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void fdgvParents_SelectionChanged(object sender, EventArgs e)
		{
			if(fdgvParents.SelectedRows.Count == 0)
				btnParentDel.Enabled = false;
			else
				btnParentDel.Enabled = true;
		}
		//-------------------------------------------------------------------------------------
		private void fdgvChilds_SelectionChanged(object sender, EventArgs e)
		{
			if(fdgvChilds.SelectedRows.Count == 0)
				btnChildDel.Enabled = false;
			else
				btnChildDel.Enabled = true;
		}
		#endregion << DataGridViews Handlers >>
		//-------------------------------------------------------------------------------------
		#region << toolstrip handlers >> 
		/// <summary>
		/// Добавление родительских групп
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnParentAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvGroups.SelectedRows.Count == 0)
					return;
				SecurityGroup cur = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				PList<SecurityItem> list = new PList<SecurityItem>();
				foreach(SecurityGroup g in psec.SecurityGroups.Values)
					if(!(g == cur || parentsList.FirstOrDefault(x => x.SID == g.SID) != null ||
										childsList.FirstOrDefault(x => x.SID == g.SID) != null))
						list.Add(SecurityItem.FromGroup(g));

				SimModalMultiChoiceBox frm = new SimModalMultiChoiceBox();
				frm.Parent = PanelBack;
				frm.DialogClosed += new DialogClosedEventHandler(SimModalMultiChoiceBox_DialogClosed);
				frm.Tag = 1;
				frm.CaptionImage = global::Sim.AdminForms.Properties.Resources.Group;
				frm.CaptionText = "Добавление родительских групп";
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
				SimModalMultiChoiceBox frm = (SimModalMultiChoiceBox)sender;
				frm.DialogClosed -= new DialogClosedEventHandler(SimModalMultiChoiceBox_DialogClosed);
				if(result == DialogResult.Cancel || frm.Tag == null)
					return;

				SecurityGroup gr = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				if((int)frm.Tag == 1)
				{
					List<OID> list = new List<OID>();
					foreach(object i in (System.Collections.IList)frm.Choices)
						list.Add(((SecurityItem)i).SID);
					foreach(SecurityItem i in parentsList)
						list.Add(i.SID);

					ShowProgressWindow();
					PulsarQuery q = new PulsarQuery("Security", "SetParentsSidLinks", 
																							new { SetParentsSidLinks = new Object[] { gr.SID, list } }, PulsarQueryParams.Modify);
					TaskManager.Run("SetParents", this, () => PulsarConnection.Default.Exec(q), new ValuesPair<OID, List<OID>>(gr.SID, list));
				}
				else if((int)frm.Tag == 2)
				{
					List<OID> list = new List<OID>();
					foreach(object i in (System.Collections.IList)frm.Choices)
						list.Add(((SecurityItem)i).SID);
					foreach(SecurityItem i in childsList)
						list.Add(i.SID);

					ShowProgressWindow();
					PulsarQuery q = new PulsarQuery("Security", "SetChildSidLinks",
																							new { SetChildSidLinks = new Object[] { gr.SID, list } }, PulsarQueryParams.Modify);
					TaskManager.Run("SetChilds", this, () => PulsarConnection.Default.Exec(q), new ValuesPair<OID, List<OID>>(gr.SID, list));
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаление родительских групп
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnParentDel_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvParents.SelectedRows.Count == 0)
					return;
				SecurityGroup gr = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				if(SimModalMessageBox.Show(PanelBack,
							String.Format("Вы действительно желаете исключить группу \"{0}\" из выбранных групп?", gr.Name),
							"Исключение группы", MessageBoxIcon.Question, MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;

				foreach(DataGridViewRow r in fdgvParents.SelectedRows)
					parentsList.Remove((SecurityItem)r.GetData());
				List<OID> list = new List<OID>();
				foreach(SecurityItem i in parentsList)
					list.Add(i.SID);

				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Security", "SetParentsSidLinks",
																						new { SetParentsSidLinks = new Object[] { gr.SID, list } }, PulsarQueryParams.Modify);
				TaskManager.Run("SetParents", this, () => PulsarConnection.Default.Exec(q), new ValuesPair<OID, List<OID>>(gr.SID, list));
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавление дочерних групп
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChildGroupAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvGroups.SelectedRows.Count == 0)
					return;
				SecurityGroup cur = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				PList<SecurityItem> list = new PList<SecurityItem>();
				foreach(SecurityGroup g in psec.SecurityGroups.Values)
					if(!(g == cur || parentsList.FirstOrDefault(x => x.SID == g.SID) != null ||
										childsList.FirstOrDefault(x => x.SID == g.SID) != null))
						list.Add(SecurityItem.FromGroup(g));

				SimModalMultiChoiceBox frm = new SimModalMultiChoiceBox();
				frm.Parent = PanelBack;
				frm.DialogClosed += new DialogClosedEventHandler(SimModalMultiChoiceBox_DialogClosed);
				frm.Tag = 2;
				frm.CaptionImage = global::Sim.AdminForms.Properties.Resources.Group;
				frm.CaptionText = "Добавление дочерних групп";
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
		/// <summary>
		/// Удаление дочерних групп и пользователей
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChildDel_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvChilds.SelectedRows.Count == 0)
					return;
				SecurityGroup gr = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				if(SimModalMessageBox.Show(PanelBack,
					String.Format("Вы действительно желаете исключить выбранные элементы из группы \"{0}\"?", gr.Name),
						"Исключение групп или пользователей", MessageBoxIcon.Question, MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;

				foreach(DataGridViewRow r in fdgvChilds.SelectedRows)
					childsList.Remove((SecurityItem)r.GetData());
				List<OID> list = new List<OID>();
				foreach(SecurityItem i in childsList)
					list.Add(i.SID);

				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Security", "SetChildSidLinks",
																						new { SetChildSidLinks = new Object[] { gr.SID, list } }, PulsarQueryParams.Modify);
				TaskManager.Run("SetChilds", this, () => PulsarConnection.Default.Exec(q), new ValuesPair<OID, List<OID>>(gr.SID, list));
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавление пользователей в группу
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChildUserAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvGroups.SelectedRows.Count == 0)
					return;
				SecurityGroup cur = (SecurityGroup)((IKeyedValue)fdgvGroups.SelectedRows[0].GetData()).Value;
				PList<SecurityItem> list = new PList<SecurityItem>();
				foreach(Person u in users)
					if(childsList.FirstOrDefault(x => x.SID == u.OID) == null)
						list.Add(SecurityItem.FromUser(u));

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
		#endregion << toolstrip handlers >>
	}
	//**************************************************************************************
	internal class SecurityItem
	{
		[Browsable(false)]
		public OID SID { get; set; }

		public Image Image { get; set; }
		[DisplayName("Наименование")]
		public string Name { get; set; }
		[DisplayName("Описание")]
		public string Desc { get; set; }

		public static SecurityItem FromGroup(SecurityGroup group)
		{
			SecurityItem ch = new SecurityItem();
			ch.SID = group.SID;
			ch.Image = global::Sim.AdminForms.Properties.Resources.Group;
			ch.Name = group.Name;
			ch.Desc = group.Description;
			return ch;
		}
		public static SecurityItem FromUser(Person user)
		{
			SecurityItem ch = new SecurityItem();
			ch.SID = user.OID;
			ch.Image = global::Sim.AdminForms.Properties.Resources.User;
			ch.Name = user.ShortName;
			ch.Desc = user.FullName;
			return ch;
		}
	}

}

