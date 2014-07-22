using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using Pulsar;
using Pulsar.Clients;
using Sim.Controls;
using Pulsar.Server;

namespace Sim
{
	/// <summary>
	/// Класс главной формы СИМ.
	/// </summary>
	internal partial class MainForm : Form, IAsyncTaskDoneHandler
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public MainForm()
		{
			try
			{
				InitializeComponent();
				//*** Версия ***//
				string ver = Assembly.GetEntryAssembly().GetName().Version.ToString();
				this.Text += " - " + ver;

				this.toolStripStatusLabelProgress.Tag = 0;
				tabViewer1.NeedPageClose += new EventHandler<TabViewer, CancelEventArgs<TabPage>>(tabViewer1_NeedPageClose);
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
				this.Close();
			}
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region IAsyncTaskDoneHandler Members
		bool IAsyncTaskDoneHandler.InvokeRequired
		{
			get { return this.InvokeRequired; }
		}

		void IAsyncTaskDoneHandler.Invoke(AsyncTask task)
		{
			this.Invoke(new Action<AsyncTask>(((IAsyncTaskDoneHandler)this).AsyncTaskDone), task);
		}

		#endregion
		//-------------------------------------------------------------------------------------
		#region << Form Handlers >>
		private void MainForm_Load(object sender, EventArgs e)
		{
			try
			{
				PulsarQuery.ContextQuery.ClientType = ClientType.WinForms;
				PulsarQuery.ContextQuery.ClientName = "Sim";
				PulsarQuery.ContextQuery.ClientVersion = Application.ExecutablePath.Contains("bin\\Debug") == true
					? 0
					: GetLastVersion();
				navigator1.AssistSex = PulsarQuery.ContextQuery.User.Sex;

				ClientBaseForm.SetShellStatusText = (s) => toolStripStatusLabelText.Text = s;

				Rectangle bounds = this.Bounds;
				bounds.X = (int)ServerParamsBase.GetParam("MainForm", "X", 20);
				bounds.Y = (int)ServerParamsBase.GetParam("MainForm", "Y", 20);
				bounds.Width = (int)ServerParamsBase.GetParam("MainForm", "Width", 600);
				bounds.Height = (int)ServerParamsBase.GetParam("MainForm", "Height", 500);
				this.Bounds = bounds;
				this.WindowState = (FormWindowState)ServerParamsBase.GetParam("MainForm", "State", FormWindowState.Normal);

				//*** Имя сервера ***//
				toolStripStatusLabelServerName.Text = PulsarConnection.Default.ToString();

				PulsarConnection.DataAccessBegin += new EventHandler(server_DataAccessBegin);
				PulsarConnection.DataAccessEnd += new EventHandler(server_DataAccessEnd);

				#region Отображаем окно прогресса загрузки данных
				NetProgressControl progressForm = new NetProgressControl();
				//progressForm.buttonCancel.Enabled = false;
				progressForm.MessageText = "Инициализация программы.\r\nПожалуйста, подождите ...";
				foreach (Control c in this.Controls)
					c.Enabled = false;
				this.Controls.Add(progressForm);
				progressForm.BringToFront();
				#endregion Отображаем окно прогресса загрузки данных

				TaskManager.Run("MainMenu", this, () => PulsarConnection.Default.Get("MainMenu", "GetUserMainMenu",
						new { GetUserMainMenu = PulsarQuery.ContextQuery.User.OID }, PulsarQueryParams.None));
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
				this.Close();
			}
		}
		//-------------------------------------------------------------------------------------
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.WindowState != FormWindowState.Minimized)
			{
				ServerParamsBase.SetParam("MainForm", "State", (int)this.WindowState);
				ServerParamsBase.SetParam("MainForm", "X", this.Bounds.X);
				ServerParamsBase.SetParam("MainForm", "Y", this.Bounds.Y);
				ServerParamsBase.SetParam("MainForm", "Width", this.Bounds.Width);
				ServerParamsBase.SetParam("MainForm", "Height", this.Bounds.Height);
			}

			ServerParamsBase.SetParam("MainForm", "LastMenuTab", navigator1.SelectedTabName);
		}
		//-------------------------------------------------------------------------------------
		void IAsyncTaskDoneHandler.AsyncTaskDone(AsyncTask task)
		{
			try
			{
				#region << Hide Progress Window >>
				Control pr = null;
				foreach (Control c in this.Controls)
					if (c is NetProgressControl)
						pr = c;
					else
						c.Enabled = true;
				this.Controls.Remove(pr);
				pr.Dispose();
				pr = null;
				#endregion << Hide Progress Window >>

				#region << Error and Abort handler >>
				if (task.Error != null)
				{
					TaskManager.AbortTasks(this);
					ModalErrorBox.ShowServer(task.Error.Message, this);
					SimModalMessageBox.ShowError(this, "Инициализация программы завершилась с ошибкой!\r\n" +
						"Продолжение работы невозможно.", "Ошибка инициализации");
					this.Close();
					return;
				}
				if (task.IsAborted)
					return;
				#endregion << Error and Abort handler >>

				#region MainMenu
				if (task.TaskName == "MainMenu")
				{
					PulsarMainMenu menu = (PulsarMainMenu)task.Result;
					List<TabPage> pages = new List<TabPage>();
					foreach (TreeItem<FormInfo> root in menu.GetRootItems().OrderBy(x=> x, PulsarMainMenu.PulsarMainMenuSorter.Default))
					{
						string tab = root.Object.SD.ToString();
						navigator1.AddTab(root.Object.Caption, tab);
						foreach (TreeItem<FormInfo> child in root.Children.OrderBy(x=> x, PulsarMainMenu.PulsarMainMenuSorter.Default))
							NavigatorAddSubItems(menu, child.Object, navigator1[tab]);
					}
					navigator1.Tag = menu;
					navigator1.SelectTab((string)ServerParamsBase.GetParam("MainForm", "LastMenuTab", null));
				}
				#endregion MainMenu
			}
			catch (Exception Err)
			{
				ModalErrorBox.Show(Err, this);
				SimModalMessageBox.ShowError(this, "Инициализация программы завершилась с ошибкой!\r\n" +
					"Продолжение работы невозможно.", "Ошибка инициализации");
				this.Close();
			}
		}
		//-------------------------------------------------------------------------------------
		void server_DataAccessBegin(object sender, EventArgs e)
		{
			try
			{
				if (statusStrip1.InvokeRequired)
					statusStrip1.Invoke(new EventHandler(server_DataAccessBegin), new object[] { sender, e });
				else
				{
					this.toolStripStatusLabelProgress.Image = global::Sim.Shell.Properties.Resources.CompServerExchange;
					this.toolStripStatusLabelProgress.Tag = (int)this.toolStripStatusLabelProgress.Tag + 1;
				}
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void server_DataAccessEnd(object sender, EventArgs e)
		{
			try
			{
				if (statusStrip1.InvokeRequired)
					statusStrip1.Invoke(new EventHandler(server_DataAccessEnd), new object[] { sender, e });
				else
				{
					this.toolStripStatusLabelProgress.Tag = (int)this.toolStripStatusLabelProgress.Tag - 1;
					if ((int)this.toolStripStatusLabelProgress.Tag == 0)
						this.toolStripStatusLabelProgress.Image = null;
				}
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void toolStripStatusLabelServerName_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				using (ConParamsForm form = new ConParamsForm())
					if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						toolStripStatusLabelServerName.Text = PulsarConnection.Default.ToString();
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		protected override void OnClosing(CancelEventArgs e)
		{
			foreach (TabPage p in tabViewer1.TabPages)
			{
				((ClientBaseForm)p.Tag).RaiseClosingEvent(e);
				if (e.Cancel)
					return;
			}
			tabViewer1.TabPages.Clear();
			base.OnClosing(e);
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Navigator and TabViewer Handlers and Methods>>
		private void NavigatorAddSubItems(PulsarMainMenu menu, FormInfo child,
																																					ToolStripItemCollection col)
		{
			TreeItem<FormInfo> i = menu[child];
			ToolStripItem item;
			if (i.HasChildren == false)
			{
				if (child.Caption == "-")
				{
					item = new ToolStripSeparator();
					item.Name = child.SD.ToString();
				}
				else
				{
					if (i.Level > 1)
						item = new ToolStripMenuItem();
					else
						item = new ToolStripButton();
					item.Image = child.Image == null ? global::Sim.Shell.Properties.Resources.Point_Shadow :
																																								GetResourceImage(child.Image);
					item.Name = child.SD.ToString();
					item.Text = child.Caption;
					item.Tag = child;
				}
			}
			else
			{
				item = new ToolStripDropDownButton();
				((ToolStripDropDownButton)item).DropDownItemClicked +=
						new ToolStripItemClickedEventHandler(navigator1.ToolStrips_ItemClicked);
				item.Image = child.Image == null ? global::Sim.Shell.Properties.Resources.Arrow_Gray : GetResourceImage(child.Image);
				item.Name = child.SD.ToString();
				item.Text = child.Caption;
				((ToolStripDropDownButton)item).ShowDropDownArrow = false;
				foreach(TreeItem<FormInfo> ch in i.Children.OrderBy(x => x, PulsarMainMenu.PulsarMainMenuSorter.Default))
					NavigatorAddSubItems(menu, ch.Object, ((ToolStripDropDownButton)item).DropDownItems);
			}
			col.Add(item);
		}
		//-------------------------------------------------------------------------------------
		private void navigator1_FormSelected(object sender, FormInfo formInfo)
		{
			try
			{
				switch (formInfo.FormClassName)
				{
					case "SpecialConParams": toolStripStatusLabelServerName_DoubleClick(null, null); break;
					//case "SpecialCasheRefresh" : PulsarConnection.ObjectsCache.Clear(); break;
					default:
						if (tabViewer1.TabPages[formInfo.SD.ToString()] != null)
						{
							tabViewer1.SelectedTab = tabViewer1.TabPages[formInfo.SD.ToString()];
							return;
						}
						if (String.IsNullOrWhiteSpace(formInfo.FormClassName))
						{
							MessageBox.Show("Нет формы!");
							return;
						}
						//string formsDll = formInfo.FormClassName.Split('.')[0] + "." + formInfo.FormClassName.Split('.')[1];
						//formsDll = String.Format("{0}\\{1}.dll", Path.GetDirectoryName(Application.ExecutablePath), formsDll);
						//Assembly assem = Assembly.LoadFrom(formsDll);
						Type formType = Type.GetType(formInfo.FormClassName);
						if (formType == null)
							throw new PulsarException("Не удалось загрузить тип [{0}]!", formInfo.FormClassName);
						ClientBaseForm form =
							(ClientBaseForm)formType.InvokeMember(formInfo.FormClassName, BindingFlags.CreateInstance, null, null, null);

						form.FormInfo = formInfo;
						form.Name = formInfo.SD.ToString();
						if (formInfo.Caption != null)
							form.Text = formInfo.Caption;

						Control[] arr = null;
						if (form.Controls.Count == 1 && form.Controls[0] is Panel && form.Controls[0].Name == "panelBack")
							arr = new Control[1] { form.Controls[0] };
						else
						{
							arr = new Control[form.Controls.Count];
							form.Controls.CopyTo(arr, 0);
						}
						TabViewerPage page = new TabViewerPage(form.Text.Trim());
						page.Name = form.Name;
						page.Tag = form;
						page.Controls.AddRange(arr);
						tabViewer1.TabPages.Add(page);
						tabViewer1.SelectedTab = page;
						form.ControlAdded += new ControlEventHandler(ClientForm_ControlAdded);
						form.ShowInTaskbar = false;
						form.RaiseLoadEvent();
						//form.Show();
						//form.Hide();
						break;
				}
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void ClientForm_ControlAdded(object sender, ControlEventArgs e)
		{
			ClientBaseForm form = (ClientBaseForm)sender;
			foreach (TabPage page in tabViewer1.TabPages)
				if ((ClientBaseForm)page.Tag == form)
				{
					page.Controls.Add(e.Control);
					break;
				}
		}
		//-------------------------------------------------------------------------------------
		private Image GetResourceImage(string name)
		{
			ComponentResourceManager c = null;
			try
			{
				c = new ComponentResourceManager(typeof(global::Sim.Shell.Properties.Resources));
				Image img = (Image)c.GetObject(name);
				return img;
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
				return null;
			}
			finally
			{
				if (c != null)
					c.ReleaseAllResources();
			}
		}
		//-------------------------------------------------------------------------------------
		private void tabViewer1_Selected(object sender, TabControlEventArgs e)
		{
			try
			{
				if (e.TabPage == null)
					return;
				ClientBaseForm form = (ClientBaseForm)e.TabPage.Tag;
				if (form != null)
				{
					this.toolStripStatusLabelText.Text = form.ShellStatusText;
					//if(ApplyContext(form.UsedContext))
					// currentContext.SetValueFrom(form.UsedContext);
					//if(SimDropDownBar1.CurrentItemID != form.MFES.CurrentID)
					// SimDropDownBar1.CurrentItemID = form.MFES.CurrentID;
					form.RaiseShownEvent();
				}
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void tabViewer1_NeedPageClose(TabViewer sender, CancelEventArgs<TabPage> args)
		{
			try
			{
				TabPage page = args.Object;
				if(page == null)
					return;
				ClientBaseForm form = page.Tag as ClientBaseForm;
				if(form != null)
				{
					form.RaiseClosingEvent(args);
					if(args.Cancel == false)
						form.Close();
				}
			}
			catch
			{
				
				throw;
			}
		}
		#endregion << Navigator and TabViewer Handlers and Methods>>

		#region << Methods >>

		/// <summary>
		/// Получает директории версий
		/// </summary>
		/// <returns>Возвращает список директорий с версиями программы</returns>
		private string[] GetDirectoriesVesions()
		{
			if (!Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath)))
				return new string[0];
			string[] dirs = Directory.GetDirectories(Path.GetDirectoryName(Application.ExecutablePath));
			List<string> dirsVersions = new List<string>();
			int ver;
			for (int i = 0; i < dirs.Length; i++)
				if (int.TryParse(new DirectoryInfo(dirs[i]).Name, out ver))
					dirsVersions.Add(ver.ToString());

			return dirsVersions.ToArray();
		}

		/// <summary>
		/// Получает номер последней версии
		/// </summary>
		/// <returns>Версия</returns>
		private uint GetLastVersion()
		{
			string[] dirVersions = GetDirectoriesVesions();
			uint ver = 0, currVer;

			for (int i = 0; i < dirVersions.Length; i++)
				if (uint.TryParse(dirVersions[i], out currVer))
					if (currVer > ver)
						ver = currVer;
			return ver;
		}

		#endregion << Methods >>
	}

	
}
