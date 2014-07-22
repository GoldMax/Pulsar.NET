using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Pulsar;
using Pulsar.Clients;
using Sim.Controls;

namespace Sim.AdminForms
{
	// Sim.AdminForms.FormPersons
	/// <summary>
	/// 
	/// </summary>
	public partial class FormPersons : ClientBaseForm
	{
		private Persons _pers = null;
		private PList<ISubject>	_list = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		private bool Modify
		{
			get { return finButtonSave.Enabled; }
			set
			{
				fdgvList.Enabled = !value;
				finistToolStrip1.Enabled = !value;
				finButtonSave.Enabled = value;
				finButtonUndo.Enabled = value;
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		public FormPersons()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Form Handlers >>
		///
		protected override void OnLoad(EventArgs e)
		{
			try
			{
				ShowProgressWindow();
				TaskManager.Run("Persons", this, () => PulsarConnection.Default.Get("Persons", null, PulsarQueryParams.NoStubEssences));
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnClosing(CancelEventArgs e)
		{
			fdgvList.DataSource = null;
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void AsyncTaskDoneBody(AsyncTask task)
		{
			#region Persons
			if(task.TaskName == "Persons")
			{
				_pers = (Persons)task.Result;
				ListBinder b = new ListBinder(_list = new PList<ISubject>(_pers));
				b.CacheSort = false;
				b.Sort(Sort);
				fdgvList.DataSource = b;
			}
			#endregion Persons
			#region Save
			if(task.TaskName == "Save")
			{
				ISubject s = (ISubject)task.Tag;
				if(_pers.Contains(s) == false)
				 _pers.Add((Person)s);
				else
				 Pulsar.Serialization.PulsarSerializer.Deserialize(Pulsar.Serialization.PulsarSerializer.Backup(s), _pers[s.OID]);
				Modify = false;
			}
			#endregion Save
			#region Remove
			if(task.TaskName == "Remove")
			{
				_pers.Remove((Person)task.Tag);
				_list.Remove((ISubject)task.Tag);
			}
			#endregion Remove
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void btnNew_Click(object sender, EventArgs e)
		{
			try
			{
				string s = Guid.NewGuid().ToString();
				if(SimModalInputBox.Show(PanelBack, ref s, "Создание персоны", "Введите OID персоны:") != 
			    System.Windows.Forms.DialogResult.OK)
					return;
				OID oid;
				if(OID.TryParse(s, out oid) == false)
					throw new Exception("OID неверен!");
				Person p = (Person)GOL.GetForRead(oid);
				if(p == null)
				 p = GlobalObject.CreateWithOID<Person>(oid);
				else if(_list.Contains(p))
					throw new Exception("Персона с указанным OID уже присутствует в списке!");
				_list.Add(p);
				foreach(DataGridViewRow r in fdgvList.Rows)
					if(r.GetData() == p)
					{
						fdgvList.CurrentCell = fdgvList[0, r.Index];
						break;
					}
				fdgvList_SelectionChanged(sender, e);
				Modify = true;
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		private void btnDel_Click(object sender, EventArgs e)
		{
			object obj = fdgvList.CurrentRow.GetData();
			if(SimModalMessageBox.Show(PanelBack,String.Format("Удалить [{0}]?", obj),"Удаление персоны",
			                              MessageBoxIcon.Question, MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
			 return;
			ShowProgressWindow();
			TaskManager.Run("Remove",this,()=> PulsarConnection.Default.Modify("Persons", "Remove", new { Remove = obj }), obj);

		}
		private void finButtonUndo_Click(object sender, EventArgs e)
		{
		 if(_pers.Contains(propertyGrid.SelectedObject) == false)
			 _list.Remove((ISubject)propertyGrid.SelectedObject);
			Modify = false;
			fdgvList_SelectionChanged(sender,e);
		}
		private void finButtonSave_Click(object sender, EventArgs e)
		{
			try
			{
			 object obj = fdgvList.CurrentRow.GetData();
					PulsarQuery q = new PulsarQuery();
				var pars = new Pulsar.Serialization.PulsarSerializationParams();

			 if(_pers.Contains(obj))
				{
					throw new Exception("Оставлено до лучших времен");
				}
				else
				{
					q.RootObject = "Persons";
					q.Query = "Add";
					q.Args = new ParamsDic();
					q.Args["Add"] = obj;
					pars.NoStubObjects = new [] { obj };
				}
				ShowProgressWindow();
				TaskManager.Run("Save",this,()=> PulsarConnection.Default.Modify(q, pars), obj);
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		private void fdgvList_SelectionChanged(object sender, EventArgs e)
		{
			if(propertyGrid.SelectedObject != null)
			 ((IObjectChangeNotify)propertyGrid.SelectedObject).ObjectChanged -= new EventHandler<ObjectChangeNotifyEventArgs>(Person_ObjectChanged);
			Person p = (Person)fdgvList.CurrentRow.GetData();
			if(_pers.Contains(p))
			 p = (Person)Pulsar.Serialization.PulsarSerializer.CloneObject(p);
			p.ObjectChanged += new EventHandler<ObjectChangeNotifyEventArgs>(Person_ObjectChanged);
			propertyGrid.SelectedObject = p;
		}
		void Person_ObjectChanged(object sender, ObjectChangeNotifyEventArgs e)
		{
			Modify = true;
		}
		private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
		 propertyGrid.Refresh();
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		private int Sort(object x, object y)
		{
			return String.Compare(((ISubject)x).FullName, ((ISubject)y).FullName);
		}
		#endregion << Methods >>


		//-------------------------------------------------------------------------------------
						
	}
}
