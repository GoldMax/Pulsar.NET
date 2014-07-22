using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


using Sim.Controls;
using Pulsar;
using Pulsar.Server;
using Pulsar.Serialization;

namespace Sim.AdminForms
{
	// Sim.AdminForms.frmConsole
	/// <summary>
	/// Класс формы консоли Пульсара.
	/// </summary>
	public partial class FormConsole : Sim.ClientBaseForm
	{
		private bool showNonPublic = false;
		private bool showIEnumProps = false;
		private PDictionary<string, StoreItem> store = new PDictionary<string, StoreItem>();
		private PList<Type> noStubList = new PList<Type>();
		private PList<Type> noSerList = new PList<Type>();
		private CtrlSelectTypes ctrlNoStubList = null;
		private CtrlSelectTypes ctrlNoSerList = null;
		private CtrlCreateObject ctrlCreateObject = new CtrlCreateObject();
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public FormConsole()
		{
			InitializeComponent();

			fdgvList.AllowAutoGenerateColumns = false;
			fdgvStore.DataSource = new DictionaryBinder(store);
			noStubList.ObjectChanged += (s,e)=> 
			{ 
				flNoStubCount.Text = String.Format("[{0}]", noStubList.Count); 
				flNoStubCount.Refresh();
			};
			noSerList.ObjectChanged += (s,e)=> 
			{ 
				flNoSerCount.Text = String.Format("[{0}]", noSerList.Count); 
				flNoSerCount.Refresh();
			};
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Form Handlers >>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			try
			{
				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Pulsar", "GetObjectsList");
				ToLog(true, q.ToString());
				TaskManager.Run("List", this, () => PulsarConnection.Default.Query(q));
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			fdgvList.DataSource = null;
		}
		//-------------------------------------------------------------------------------------
		protected override void AsyncTaskDoneBody(AsyncTask task)
		{
			#region List
			if(task.TaskName == "List")
			{
				PulsarAnswer ans = (PulsarAnswer)task.Result;
				ToLog(ans.ToString());
				fdgvList.DataSource = new ListBinder((IList)ans.Return);
				FillAnswer(ans, "Answer");
			}
			#endregion List
			#region Query
			if(task.TaskName == "Query")
			{
				PulsarAnswer ans = (PulsarAnswer)task.Result;
				ToLog(ans.ToString());
				FillAnswer(ans, "Answer");
			}
			#endregion Query
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void buttonSend_Click(object sender, EventArgs e)
		{
			try
			{
				ftgvView.Tree = null;

				List<Object> noStub = new List<Object>();

				PulsarQuery q = new PulsarQuery();
				q.RootObject = tbObj.Text;
				q.Query = tbQuery.Text.TrimAll().Length == 0 ? null : tbQuery.Text.TrimAll();
				if(tbArgs.Text.TrimAll().Length > 0)
				{
					q.Args = ParamsDic.FromString(tbArgs.Text.Replace('|', '¦'));

					foreach(string p in q.Args.Params.ToArray())
						if(q.Args[p] is string && ((string)q.Args[p]).StartsWith("$") && ((string)q.Args[p]).EndsWith("$"))
							q.Args[p] = store[((string)q.Args[p]).Replace("$", "")].Object;
						else if(q.Args[p] is string && ((string)q.Args[p]).StartsWith("&") && ((string)q.Args[p]).EndsWith("&"))
						{
							q.Args[p] = store[((string)q.Args[p]).Replace("&", "")].Object;
							noStub.Add(q.Args[p]);
						}
				}
				q.NoStubTypes = noStubList.Count > 0 ? noStubList.ToArray() : null;
				q.NoSerTypes = noSerList.Count > 0 ? noSerList.ToArray() : null;
				

				if(chbModify.Checked)
					q.Params |= PulsarQueryParams.Modify;
				if(chbNonPublic.Checked)
					q.Params |= PulsarQueryParams.NonPublic;
				if(chbServant.Checked)
					q.Params |= PulsarQueryParams.Servant;
				if(chbCode.Checked)
					q.Params |= PulsarQueryParams.Code;
				if(chbVerbose.Checked)
					q.Params |= PulsarQueryParams.Verbose;
				if(chbNoStubEs.Checked)
					q.Params |= PulsarQueryParams.NoStubEssences;

				PulsarSerializationParams pars = new PulsarSerializationParams();
				pars.Mode = PulsarSerializationMode.ForClient;
				pars.NoStubObjects = noStub.ToArray();

				ShowProgressWindow();
				if(q.Params.HasFlag(PulsarQueryParams.Verbose))
					TaskManager.Run("Query", this,
						() => PulsarConnection.Default.Query(q, pars, messageHandler: (a) => ToLog(null, a.ToString())));
				else
					TaskManager.Run("Query", this, () => PulsarConnection.Default.Query(q, pars));
				ToLog(true, q.ToString());
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void tbArgs_TextChanged(object sender, EventArgs e)
		{
			if(tbArgs.LineCount > 2)
				tbArgs.ScrollBars = ScrollBars.Vertical;
				else
				tbArgs.ScrollBars = ScrollBars.None;
		}
		//-------------------------------------------------------------------------------------
		private void fdgvList_SelectionChanged(object sender, EventArgs e)
		{
			if(fdgvList.CurrentRow == null)
				flPulsarObjectType.Text = tbObj.Text = "";
			else
			{
				Type t = ((ValuesTrio<string, Type, string>)fdgvList.CurrentRow.GetData()).Value2;
				flPulsarObjectType.Text = t == null ? "" : t.FullName;
				tbObj.Text = ((ValuesTrio<string, Type, string>)fdgvList.CurrentRow.GetData()).Value1;
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnRefreshPulsarList_Click(object sender, EventArgs e)
		{
			try
			{
				ShowProgressWindow();
				PulsarQuery q = new PulsarQuery("Pulsar", "GetObjectsList");
				ToLog(true, q.ToString());
				TaskManager.Run("List", this, () => PulsarConnection.Default.Query(q));
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnClearLog_Click(object sender, EventArgs e)
		{
			rtbLog.Clear();
		}
		//-------------------------------------------------------------------------------------
		private void tbQuery_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)Keys.Enter)
				buttonSend_Click(sender, e);
		}
		//-------------------------------------------------------------------------------------
		private void contextMenuStripView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			try
			{
				if(ftgvView.CurrentRow == null)
					return;
				ViewItem vi = (ViewItem)ftgvView.CurrentRow.GetData();
				switch(e.ClickedItem.Name)
				{
					case "menuItemShowNonPublic":
					{
						showNonPublic = !showNonPublic;
						ITree tree = new Tree<ViewItem>() ;
						foreach(ITreeItem i in ftgvView.Tree.GetRootItems())
							tree.Add(i.Object, null);
						ftgvView.Tree = tree;
						ftgvView.Expand((ITreeItem)tree.GetRootItems().First());
					} break;
						case "menuItemShowIEnumProps" : 
					{
						showIEnumProps = !showIEnumProps;
						ITree tree = new Tree<ViewItem>();
						foreach(ITreeItem i in ftgvView.Tree.GetRootItems())
							tree.Add(i.Object, null);
						ftgvView.Tree = tree;
						ftgvView.Expand((ITreeItem)tree.GetRootItems().First());
					} break;
				case "menuItemCopyName" :
						Clipboard.SetText(vi.Name);
						break;
					case "menuItemCopyValue" :
						Clipboard.SetText(vi.Value.ToString());
						break;
					case "menuItemCopyType" :
						Clipboard.SetText(vi.Type.ToString());
						break;
					case "menuItemToStore" : 
					{
						e.ClickedItem.Owner.Hide();
						string s = vi.Name;
						while(true)
						{
							if(SimModalInputBox.Show(PanelBack,ref s, "Добавление объекта", "Введите имя объекта:",
												global::Sim.AdminForms.Properties.Resources.Object_Add) == System.Windows.Forms.DialogResult.Cancel)
								return;
							if(store.ContainsKey(s))
								SimModalMessageBox.Show(PanelBack, "Объект с таким именем уже существует!", "Ошибка имени объекта",
									MessageBoxIcon.Error);
							else
								break;
						}
						store.Add(s, new StoreItem(s, vi.Value)); 

					} break;
					case "menuItemObjectRefresh" :
					{
						ITreeItem item = (ITreeItem)ftgvView.Tree.Items.First();
						ViewItem v = (ViewItem)item.Object;
						FillAnswer(v.obj ?? v.parObj, v.Name);
					} break;
				}
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void cbQuery_SelectionChangeCommitted(object sender, EventArgs e)
		{
			tbArgs.Text = "";
			chbModify.Checked = false;
			chbNonPublic.Checked = false;
		}
		//-------------------------------------------------------------------------------------
		private void ftgvView_HasChildItems(object sender, SimTreeGridView.SimTreeGridItemEventArgs args)
		{
			try
			{
				Type t = ((ViewItem)args.Item.Object).Type;
				if(t != null)
					args.HasChildren = !(t.IsPrimitive || t.IsAssignableFrom(typeof(string)) || t.FullName == "System.RuntimeType");
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void ftgvView_NeedChildItems(object sender, SimTreeGridView.SimTreeGridItemEventArgs args)
		{
			try
			{
				object obj = ((ViewItem)args.Item.Object).Value;
				Type t = obj.GetType();
				
				if(obj is IEnumerable == false || showIEnumProps == true)
				{
					BindingFlags fl = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty;
					if(showNonPublic)
						fl |= BindingFlags.NonPublic;
					foreach(MemberInfo mi in t.GetMembers(fl))
					{
						ViewItem i = new ViewItem();
						i.Name = mi.Name;
						switch(mi.MemberType)
						{
							case MemberTypes.Field:
								i.mi = mi;
								i.parObj = obj;
							break;
							case MemberTypes.Property:
								if(((PropertyInfo)mi).GetIndexParameters().Length > 0 )//I i.Name == "Item" || i.Name.EndsWith(".Item"))
									continue;
								goto case MemberTypes.Field;
							default: continue;
						}
						args.Tree.Add(i, args.Item);
					}
				}
				if(obj is IEnumerable)
				{
					int c = 0;
					foreach(object i in (IEnumerable)obj)
					{
						ViewItem vi = new ViewItem();
						vi.Name = String.Format("[{0}]", c++);
						vi.obj = i;
						args.Tree.Add(vi, args.Item);
					}
				}
			}
			catch(Exception Err)
			{
				//ModalErrorBox.Show(Err, PanelBack);
				ViewItem vi = new ViewItem();
				vi.Name = "ERROR";
				vi.obj = Err;
				args.Tree.Add(vi, args.Item);

			}

		}
		//-------------------------------------------------------------------------------------
		private void fdgvStore_SelectionChanged(object sender, EventArgs e)
		{
			if(fdgvStore.SelectedRows.Count == 0)
				btnObjectRename.Enabled = btnObjectDel.Enabled = false;
			else
			{
				btnObjectRename.Enabled = btnObjectDel.Enabled = true;
				flObjectType.Text = ((StoreItem)((IKeyedValue)fdgvStore.CurrentRow.GetData()).Value).Type;
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnObjectCreate_Click(object sender, EventArgs e)
		{
			try
			{
			 ctrlCreateObject.ObjectStore = store;
				ctrlCreateObject.Parent = PanelBack;
				ctrlCreateObject.DialogClosed += new DialogClosedEventHandler(CtrlCreateObject_DialogClosed);
				ctrlCreateObject.Show();
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		void CtrlCreateObject_DialogClosed(object sender, DialogResult result)
		{
			try
			{
				CtrlCreateObject box = (CtrlCreateObject)sender;
				box.DialogClosed -= new DialogClosedEventHandler(CtrlCreateObject_DialogClosed);
				if(result == System.Windows.Forms.DialogResult.Cancel)
					return;
				string s = box.ObjectName;
				while(true)
				{
					if(store.ContainsKey(s))
						SimModalMessageBox.Show(PanelBack, "Объект с таким именем уже существует!", "Ошибка имени объекта",
							MessageBoxIcon.Error);
					else
						break;
					if(SimModalInputBox.Show(PanelBack, ref s, "Добавление объекта", "Введите имя объекта:",
										global::Sim.AdminForms.Properties.Resources.Object_Add) == System.Windows.Forms.DialogResult.Cancel)
						return;
				}
				store.Add(s, new StoreItem(s, box.Object)); 
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnObjectRename_Click(object sender, EventArgs e)
		{
			try
			{																																																		
				if(fdgvStore.SelectedRows.Count == 0)
					return;
				StoreItem i = (StoreItem)((IKeyedValue)fdgvStore.SelectedRows[0].GetData()).Value;
				string s = i.Name;
				if(SimModalInputBox.Show(PanelBack, ref s, "Переименование объекта", "Введите имя объекта:",
					global::Sim.AdminForms.Properties.Resources.Rename) == System.Windows.Forms.DialogResult.Cancel)
					return;
				if(store.ContainsKey(s))
				{
					SimModalMessageBox.Show(PanelBack, "Объект с таким именем уже существует!", "Ошибка имени объекта",
						MessageBoxIcon.Error);
					return;
				}
				store.Remove(i.Name);
				i.Name = s;
				store.Add(s, i);
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnObjectDel_Click(object sender, EventArgs e)
		{
			try
			{
				if(fdgvStore.SelectedRows.Count == 0)
					return;
				StoreItem i = (StoreItem)((IKeyedValue)fdgvStore.CurrentRow.GetData()).Value;
				store.Remove(i.Name);
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}

		}
		//-------------------------------------------------------------------------------------
		private void fdgvStore_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			StoreItem si = (StoreItem)((IKeyedValue)fdgvStore.Rows[e.RowIndex].GetData()).Value;
			if(si.Object is PulsarCodeTransfer.CodeQuery)
			{
				CtrlCodeEditor box = new CtrlCodeEditor();
				box.Parent = PanelBack;
				box.Width = PanelBack.Width - 30;
				box.Height = PanelBack.Height - 30;
				box.DialogClosed += new DialogClosedEventHandler(CtrlCodeEditor_DialogClosed);

				List<Type> types = new List<Type>();
				Type curType = null;
				foreach(ValuesTrio<string, Type, string> i in (IPulsarBinder)fdgvList.DataSource)
				{
					if(i.Value2 == null)
						continue;
					types.Add(i.Value2);
					if(i.Value2 == curType || i.Value2.FullName == ((PulsarCodeTransfer.CodeQuery)si.Object).ObjectType)
						curType = i.Value2;
				}
				box.Tag = si.Name;
				box.Show(types, curType, ((PulsarCodeTransfer.CodeQuery)si.Object).CodeText);
			}
			else 
				FillAnswer(si.Object, si.Name);
		}
		//-------------------------------------------------------------------------------------
		private void ftgvView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if(e.RowIndex == -1 || e.ColumnIndex == -1)
					return;
				if(ftgvView.CurrentRow == null || e.RowIndex != ftgvView.CurrentRow.Index)
					ftgvView.CurrentCell = ftgvView[e.ColumnIndex, e.RowIndex];
				ViewItem vi = (ViewItem)ftgvView.Rows[e.RowIndex].GetData();
				if(vi.mi == null || (vi.mi.MemberType == MemberTypes.Property && ((PropertyInfo)vi.mi).CanWrite == false))
					return;
				SimModalInputBox box = new SimModalInputBox();
				box.Parent = PanelBack;
				box.DialogClosed += new DialogClosedEventHandler(ObjectEdit_DialogClosed);
				box.CaptionImage = global::Sim.AdminForms.Properties.Resources.Object;
				box.CaptionText = "Изменение значения объекта";
				box.Text = "Введите текстовый эквивалент значения объекта или [объект из хранилища]:";
				box.Value = vi.Value.ToString();
				box.Tag = vi;
				box.Show();
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		void ObjectEdit_DialogClosed(object sender, DialogResult result)
		{
			try
			{
				SimModalInputBox box = (SimModalInputBox)sender;
				box.DialogClosed -= new DialogClosedEventHandler(ObjectEdit_DialogClosed);
				if(result == DialogResult.Cancel)
					return;
				ViewItem vi = (ViewItem)box.Tag;
				object pObj = ((ViewItem)ftgvView.Tree[vi].Parent.Object).Value;
				object val = null;
				if(box.Value.StartsWith("[") && box.Value.EndsWith("]"))
					val = store[box.Value.Replace("[","").Replace("]","")].Object;
				else
				{
					TypeConverter tc = TypeDescriptor.GetConverter(vi.Value);
					val = tc.ConvertFromString(box.Value);
				}
				if(vi.mi is FieldInfo)
					((FieldInfo)vi.mi).SetValue(pObj, val);
				else
					((PropertyInfo)vi.mi).SetValue(pObj, val, null);
				ftgvView.Refresh();
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void ftgvView_SelectionChanged(object sender, EventArgs e)
		{
			//comboBoxMethods.Items.Clear();
			comboBoxMethods.Text = "";
			ShellStatusText = "";
			if(ftgvView.SelectedRows.Count == 0)
				comboBoxMethods.Enabled = false;
			else if(comboBoxMethods.Enabled == false)
				comboBoxMethods.Enabled = true;
		}
		//-------------------------------------------------------------------------------------
		private void comboBoxMethods_TextChanged(object sender, EventArgs e)
		{
			//if(prevValue.Length == 0)
			// prevValue = comboBoxMethods.Text;
			//if(prevValue.Length != 0 && comboBoxMethods.Text.Length == 0)
			// comboBoxMethods.Text = prevValue; 
			buttonRunMethod.Enabled = comboBoxMethods.Text.Length != 0;
		}
		//-------------------------------------------------------------------------------------
		private void comboBoxMethods_DropDown(object sender, EventArgs e)
		{
			try
			{
				if(ftgvView.SelectedRows.Count == 0)
					return;
				comboBoxMethods.Items.Clear();
				Type t = ((ViewItem)ftgvView.SelectedRows[0].GetData()).Type;
				foreach(MethodInfo mi in t.GetMethods(BindingFlags.Instance | BindingFlags.Public))
					if(mi.IsSpecialName == false)
						comboBoxMethods.Items.Add(new ComboBoxItem<MethodInfo>(mi));
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void buttonRunMethod_Click(object sender, EventArgs e)
		{
			try
			{
				if(ftgvView.SelectedRows.Count == 0)
					return;
				object obj = ((ViewItem)ftgvView.SelectedRows[0].GetData()).Value;
				Type t = obj.GetType();
				string name = comboBoxMethods.Text.Substring(0, comboBoxMethods.Text.IndexOf("(")).Trim();
				if(name.Contains(" "))
					name = name.Remove(0, name.IndexOf(" ")).Trim();
				object[] args = ParseArgs();
				Type[] ts = new Type[args.Length];
				if(args.Length > 0)
					for(int a = 0; a < args.Length; a++)
						ts[a] = args[a].GetType();

				MethodInfo mi = t.GetMethod(name, ts);
				if(mi == null)
					throw new Exception("Метод не найден!");
				object res = mi.Invoke(obj, args);
				ShellStatusText = String.Format("Возврат метода: {0} ({1})", (res ?? "null").ToString(), 
																																								res == null ? "" : res.GetType().FullName);
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void comboBoxMethods_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (Char)Keys.Enter)
				buttonRunMethod_Click(sender, e);
		}
		//-------------------------------------------------------------------------------------
		private void btnAddCode_Click(object sender, EventArgs e)
		{
			try
			{
				CtrlCodeEditor box = new CtrlCodeEditor();
				box.Parent = PanelBack;
				box.Width = PanelBack.Width - 30;
				box.Height = PanelBack.Height - 30;
				box.DialogClosed += new DialogClosedEventHandler(CtrlCodeEditor_DialogClosed);

				List<Type> types = new List<Type>();
				Type curType = null;
				if(fdgvList.CurrentRow != null)
					curType = ((ValuesTrio<string, Type, string>)fdgvList.CurrentRow.GetData()).Value2;
				foreach(ValuesTrio<string, Type, string> i in (IPulsarBinder)fdgvList.DataSource)
				{
					if(i.Value2 == null)
						continue;
					types.Add(i.Value2);
					if(i.Value2 == curType)
						curType = i.Value2;
				}
				box.Show(types, curType);
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		void CtrlCodeEditor_DialogClosed(object sender, DialogResult result)
		{
			try
			{
				CtrlCodeEditor box = (CtrlCodeEditor)sender;
				box.DialogClosed -= new DialogClosedEventHandler(CtrlCodeEditor_DialogClosed);
				if(result == System.Windows.Forms.DialogResult.Cancel)
					return;

				string s = "";
				if(box.Tag == null)
				{
					while(true)
					{
						if(store.ContainsKey(s))
							SimModalMessageBox.Show(PanelBack, "Объект с таким именем уже существует!", "Ошибка имени объекта",
								MessageBoxIcon.Error);
						else if(s.Length != 0)
							break;
						if(SimModalInputBox.Show(PanelBack, ref s, "Добавление кодового запроса", "Введите имя кодового запроса:",
											global::Sim.AdminForms.Properties.Resources.Code) == System.Windows.Forms.DialogResult.Cancel)
							return;
					}
				}
				else
				{
					s = box.Tag.ToString();
					store.Remove(s);
				}
				store.Add(s, new StoreItem(s, box.CodeQuery));
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnCreateArray_Click(object sender, EventArgs e)
		{
			try
			{
				string s = "";
				while(true)
				{
					if(store.ContainsKey(s))
						SimModalMessageBox.Show(PanelBack, "Объект с таким именем уже существует!", "Ошибка имени объекта",
							MessageBoxIcon.Error);
					else if(s.Length != 0)
						break;
					if(SimModalInputBox.Show(PanelBack, ref s, "Добавление массива объектов", "Введите имя массива объектов:",
										global::Sim.AdminForms.Properties.Resources.Array) == System.Windows.Forms.DialogResult.Cancel)
						return;
				}
				string len = "3";
				if(SimModalInputBox.Show(PanelBack, ref len, "Ввод размерности", 
							"Введите размерность массива:", null, TextBoxFormat.Digits) == System.Windows.Forms.DialogResult.Cancel)
					return;
				store.Add(s, new StoreItem(s, new object[Int32.Parse(len)]));
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private void buttonNoStub_Click(object sender, EventArgs e)
		{
			SimButton btn = (SimButton)sender;
			CtrlSelectTypes ctrl = null;
			if(btn.Name == "buttonNoStub")
			{
				if(ctrlNoStubList == null)
					ctrlNoStubList = new CtrlSelectTypes() { Types = noStubList };
				ctrl = ctrlNoStubList;
			}
			else
			{
				if(ctrlNoSerList == null)
					ctrlNoSerList = new CtrlSelectTypes() { Types = noSerList };
				ctrl = ctrlNoSerList;
			}
			Point p = btn.Parent.PointToScreen(new Point(0, btn.Parent.Height));
			SimPopupControl box = new SimPopupControl(ctrl);
			box.BackColor = SystemColors.Control;
			box.IsResizeble = true;
			box.Show(p);
		}
		//-------------------------------------------------------------------------------------
		private void finistPanel1_EnabledChanged(object sender, EventArgs e)
		{
			if(((Control)sender).Enabled)
			{
				((Control)sender).BackColor = SystemColors.Window;
				((Control)sender).ForeColor = SystemColors.ControlText;
			}
			else
			{
				((Control)sender).BackColor = SystemColors.Control;
				((Control)sender).ForeColor = SystemColors.GrayText;
			}
		}
		//-------------------------------------------------------------------------------------
		private void btnLoadAssembly_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog box = new OpenFileDialog();
				box.AddExtension = true;
				box.AutoUpgradeEnabled = true;
				box.CheckFileExists = true;
				box.Filter = "DLL файлы(*.dll)|*.dll|EXE файлы(*.exe)|*.exe|Все файлы (*.*)|*.*";
				box.FilterIndex = 0;
				box.Multiselect = false;
				box.Title = "Загрузка сборок";
				if(box.ShowDialog() != DialogResult.OK)
					return;
				Assembly.LoadFrom(box.FileName);
			}
			catch(Exception Err)
			{
				Sim.Controls.ModalErrorBox.Show(Err, PanelBack);
			}
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		private void ToLog(string text, params object[] args) { ToLog(false, text, args); }
		private void ToLog(bool? isEnter, string text, params object[] args)
		{
			if(rtbLog.InvokeRequired)
				rtbLog.Invoke(new Action<bool?, string, object[]>(ToLog), 
																		new object[] { isEnter, text, args });
			else
			{
			 if(args.Length == 0)
				{
			  text = text.Replace("{","{{");
				 text = text.Replace("}", "}}");
				}
				else
					text = String.Format(text, args);
				rtbLog.AppendText(string.Format("{0}{1}\r\n", isEnter == null ? "    " : 
																														((bool)isEnter ? "▪► " : "└► "), text));
		 }
		}
		//-------------------------------------------------------------------------------------
		private void FillAnswer(object obj, string name)
		{
			try
			{
				Tree<ViewItem> tree = new Tree<ViewItem>();
				ViewItem vi = new ViewItem();
				vi.Name = name;
				vi.obj = obj;
				TreeItem<ViewItem> item = new TreeItem<ViewItem>(vi);
				tree.Add(item, (ViewItem)null);
				ftgvView.Tree = tree;
				ftgvView.Expand(item);
			}
			catch(Exception Err)
			{
				ModalErrorBox.Show(Err, PanelBack);
			}
		}
		//-------------------------------------------------------------------------------------
		private object[] ParseArgs()
		{
			string s = comboBoxMethods.Text;
			s = s.Remove(0, s.IndexOf('(')+1).Replace(")","");

			string[] sss = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if(sss.Length == 0)
				return new object[0];
			object[] res = new object[sss.Length];
			for(int a = 0; a < sss.Length; a++)
			{
				s = sss[a].Trim();
				if(s[0] == '$' && s[s.Length-1] == '$')
					res[a] = store[s.Replace("$", "")].Object;
				else if(s.StartsWith("i_"))
					res[a] = Int32.Parse(s.Replace("i_", ""));
				else if(s.StartsWith("ui_"))
					res[a] = UInt32.Parse(s.Replace("ui_", ""));
				else if(s.StartsWith("b_"))
					res[a] = Byte.Parse(s.Replace("b_", ""));
				else if(s.StartsWith("s_"))
					res[a] = Int16.Parse(s.Replace("s_", ""));
				else if(s.StartsWith("us_"))
					res[a] = UInt16.Parse(s.Replace("us_", ""));
				else if(s.StartsWith("l_"))
					res[a] = Int64.Parse(s.Replace("l_", ""));
				else if(s.StartsWith("ul_"))
					res[a] = UInt64.Parse(s.Replace("ul_", ""));
				else if(s.StartsWith("f_"))
					res[a] = float.Parse(s.Replace("f_", ""));
				else if(s.StartsWith("d_"))
					res[a] = Decimal.Parse(s.Replace("d_", ""));
				else
				{
					bool bVal;
					int iVal;
					DateTime dtVal;
					Guid gVal;

					if(Boolean.TryParse(s, out bVal))
						res[a] = bVal;
					else if(Int32.TryParse(s, out iVal))
						res[a] = iVal;
					else if(Guid.TryParse(s, out gVal))
						res[a] = gVal;
					else if(DateTime.TryParse(s, out dtVal))
						res[a] = dtVal;
					else
						res[a] = s.Replace("\"", "");
				}
			}
			return res;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		//*************************************************************************************
		#region << private class ViewItem >>
		private class ViewItem
		{
			internal MemberInfo mi = null;
			internal object obj = null;
			internal object parObj = null;
			[DisplayName("Имя")]
			public string Name { get; set; }
			[DisplayName("Значение")]
			public object Value 
			{ 
				get
				{
					try
					{
						if(mi == null)
							return obj ?? "null";
						if(mi.MemberType == MemberTypes.Property)
							if(mi.Name == "Item")
								return "";
							else
								return ((PropertyInfo)mi).GetValue(parObj, null) ?? "null";
						if(mi.MemberType == MemberTypes.Field)
							return ((FieldInfo)mi).GetValue(parObj) ?? "null";
						throw new Exception("Ошибка данных в ViewItem!");
					}
					catch(Exception exc)
					{
						return exc;
					}
				}
			}
			[DisplayName("Тип")]
			public Type Type 
			{ 
				get
				{
					try
					{
						if(mi == null)
							if(obj == null && parObj == null)
								return null;
							else
								return (obj ?? parObj).GetType();
						if(mi.MemberType == MemberTypes.Property)
						{
							object o = null;
							if(mi.Name != "Item")
								o = ((PropertyInfo)mi).GetValue(parObj, null);
							return o == null ? ((PropertyInfo)mi).PropertyType : o.GetType();
						}
						if(mi.MemberType == MemberTypes.Field)
						{
							object o = ((FieldInfo)mi).GetValue(parObj);
							return o == null ? ((FieldInfo)mi).FieldType : o.GetType();
						}
						throw new Exception("Ошибка данных в ViewItem!");
					}
					catch(Exception exc)
					{
						return exc.GetType();
					}
				}
			}

			public ViewItem() { }
			public override string ToString()
			{
				return String.Format("{0}={1},{2}", Name, Value, Type.Name);
			}
		}
		#endregion << private class ViewItem >>
		//*************************************************************************************
		#region << private class StoreItem >>
		internal class StoreItem
		{
			internal object Object { get; set; }
			public string Name { get; set; }
			public string Type 
			{ 
				get 
				{
					if(Object == null)
						return "";
					if(Object is PulsarCodeTransfer.CodeQuery)
						return ((PulsarCodeTransfer.CodeQuery)Object).ObjectType;
					return (Object ?? "").GetType().ToString();
				}
			}
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			public StoreItem(string name, object obj) 
			{
				Name = name;
				Object = obj;
			}
		}
		#endregion << private class StoreItem >>

	}
}
