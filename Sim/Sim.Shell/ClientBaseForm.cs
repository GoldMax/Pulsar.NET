using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using Sim.Controls;
using Pulsar;
using Pulsar.Clients;
using Pulsar.Server;

//[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Sim3")]

namespace Sim
{
	/// <summary>
	/// Базовый класс клиентских форм СИМ.
	/// </summary>
	public partial class ClientBaseForm : Form, IAsyncTaskDoneHandler
	{
		//private static dynamic loginUser = null;
		internal static Action<string> SetShellStatusText = null;
			
		private System.ComponentModel.IContainer components = null;

		private Control focusedControl = null;
		Dictionary<Control, NetProgressControl> progressList = new Dictionary<Control, NetProgressControl>(1);

		private Action<Control> progressAbortMethod = null;

		private FormInfo formInfo = new FormInfo();
		private string statusText = "";
		
			
		/// <summary>
		/// Основная панель формы
		/// </summary>
		protected SimPanel PanelBack;

		#region << Events >>

		/// <summary>
		/// Событие, возникающее после обработки завершения асинхронной задачи
		/// </summary>
		[Description("Событие, возникающее после обработки завершения асинхронной задачи")]
		public event EventHandler<EventArgs<AsyncTask>> AsyncTaskCompleted;

		/// <summary>
		/// Вызывает событие AsyncTaskCompleted
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnAsyncTaskCompleted(EventArgs<AsyncTask> e)
		{
			EventHandler<EventArgs<AsyncTask>> handler = AsyncTaskCompleted;
			if (handler != null) handler(this, e);
		}

		#endregion << Events >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет текст строки статуса оболочки.
		/// </summary>
		public string ShellStatusText
		{
			get { return statusText; }
			set
			{
				statusText = value;
				if(SetShellStatusText != null)
					SetShellStatusText(value);
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Информация о форме.
		/// </summary>
		public FormInfo FormInfo
		{
			get { return formInfo; }
			internal set { formInfo = value; }
		}
		////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		///// <summary>
		///// Объект пользователя, вошедшего в текущую копию СИМ.
		///// </summary>
		//public static dynamic LoginUser
		//{
		// get { return loginUser; }
		// set
		// {
		//  loginUser = value;
		//  PulsarQuery.SecurityContext = value;
		// }
		//}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public ClientBaseForm()
		{
			InitializeComponent();
			Sim.Controls.WinFormsUtils.SetDoubleBuffered(PanelBack, true);

			progressAbortMethod = new Action<Control>(ProgressWindowCancelClick);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
				PanelBack.Dispose();
			}
			base.Dispose(disposing);
		}
		//-------------------------------------------------------------------------------------
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientBaseForm));
			this.PanelBack = new Sim.Controls.SimPanel();
			this.SuspendLayout();
			// 
			// PanelBack
			// 
			this.PanelBack.BackColor = System.Drawing.Color.Transparent;
			this.PanelBack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelBack.Location = new System.Drawing.Point(0, 0);
			this.PanelBack.Name = "PanelBack";
			this.PanelBack.Size = new System.Drawing.Size(871, 640);
			this.PanelBack.TabIndex = 0;
			// 
			// ClientBaseForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(871, 640);
			this.Controls.Add(this.PanelBack);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ClientBaseForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "ClientBaseForm";
			this.ResumeLayout(false);

		}
		#endregion
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Form Methods >>
		/// <summary>
		/// Метод, вызываемый перед закрытием формы. Реализация по умолчанию отменяет все асинхронные задачи.
		/// </summary>
		/// <param name="e">Не используется.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			//mfes.Pulsar.BreakAllSendersAsyncTasks(this);
			TaskManager.AbortTasks(this);
			base.OnClosing(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает событие Load
		/// </summary>
		internal void RaiseLoadEvent()
		{
			this.OnLoad(EventArgs.Empty);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает событие Shown
		/// </summary>
		internal void RaiseShownEvent()
		{
			this.OnShown(EventArgs.Empty);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает событие Closing
		/// </summary>
		internal void RaiseClosingEvent(CancelEventArgs arg)
		{
			this.OnClosing(arg);
			if(arg.Cancel == false)
				this.OnFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, false));
		}
		//-------------------------------------------------------------------------------------
		private Control GetFocusedControl(Control control)
		{
			if(control.Focused)
				return control;

			for(int i = 0; i < control.Controls.Count; i++)
			{
				if(control.Controls[i].Focused)
					return control.Controls[i];
				else
				{
					Control act_cnt = GetFocusedControl(control.Controls[i]);
					if(act_cnt != null)
						return act_cnt;
				}
			}
			return null;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод, вызываемый при завершении асинхронных задач формы.
		/// Реализация по умолчанию содержит проверки на ошибки и прерывания, вызов AsyncTaskDoneBody и проверку
		/// на закрытие окна прогресса.
		/// </summary>
		/// <param name="task">Асинхронная задача.</param>
		public virtual void AsyncTaskDone(AsyncTask task)
		{
			try
			{
				#region << Error and Abort handler >>
				if(task.Error != null)
				{
					TaskManager.AbortTasks(this);
					HideProgressWindow();
					if (task.Error is System.Reflection.TargetInvocationException)
						ModalErrorBox.Show(task.Error.InnerException, PanelBack);
					else if(task.Error is PulsarServerException)
						ModalErrorBox.ShowServer(task.Error.Message, PanelBack);
					else
						ModalErrorBox.Show(task.Error, PanelBack);
					return;
				}
				if(task.IsAborted)
					return;
				#endregion << Error and Abort handler >>

				AsyncTaskDoneBody(task);
				if (AsyncTaskCompleted != null)
					OnAsyncTaskCompleted(new EventArgs<AsyncTask>(task));
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
			finally
			{
				if(task.IsQueued == false && TaskManager.ContainsTask(this) == false)
					HideProgressWindow();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод тела метода, вызываемый при завершении асинхронных задач формы.
		/// </summary>
		/// <param name="task">Асинхронная задача.</param>
		protected virtual void AsyncTaskDoneBody(AsyncTask task)
		{

		}
		#endregion << Form Methods >>
		//-------------------------------------------------------------------------------------
		#region << Params Methods >>
		/// <summary>
		/// Сохраняет значение параметра формы.
		/// </summary>
		/// <param name="param">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		protected void SaveParam(string param, object value)
		{
			ServerParamsBase.SetParam(this.GetType().FullName, param, value);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает созраненное значение параметра формы.
		/// </summary>
		/// <param name="param">Имя параметра.</param>
		/// <param name="defaultValue">Значение параметра по умолчанию.</param>
		/// <returns>Сохраненное значение параметра.</returns>
		protected object LoadParam(string param, object defaultValue)
		{
			return ServerParamsBase.GetParam(this.GetType().FullName, param, defaultValue);
		}
		#endregion << Params Methods >>
		//-------------------------------------------------------------------------------------
		#region Progress Window Methods
		/// <summary>
		/// Вызывает появление окна прогресса обмена данными с сервером.
		/// </summary>
		public void ShowProgressWindow()
		{
			ShowProgressWindow(this.PanelBack);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает появление окна прогресса обмена данными с сервером.
		/// </summary>
		/// <param name="parentControl">Контрол, который будет родительским для окна прогресса.</param>
		/// <param name="cancelEnabled">Определяет доступность кнопки Отмена.</param>
		public void ShowProgressWindow(Control parentControl, bool cancelEnabled = true)
		{
			if(progressList.ContainsKey(parentControl))
				return;

			focusedControl = GetFocusedControl(parentControl);

			NetProgressControl progressForm = new NetProgressControl();
			progressForm.buttonCancel.Enabled = cancelEnabled;
			progressForm.NeedTerminate += progressAbortMethod;
			parentControl.Controls.Add(progressForm);
			progressForm.BringToFront();


			foreach(Control ctrl in parentControl.Controls)
			{
				if(ctrl != progressForm)
					ctrl.Enabled = false;
			}
			progressList.Add(parentControl, progressForm);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает закрытие окна прогресса.
		/// </summary>
		public void HideProgressWindow()
		{
			try
			{
				Control[] ctrls = new Control[progressList.Keys.Count];
				progressList.Keys.CopyTo(ctrls, 0);
				TimeSpan res = TimeSpan.Zero;
				foreach(Control c in ctrls)
					res = HideProgressWindow(c);
				if(res != TimeSpan.Zero && System.Environment.MachineName.ToLower() == "goldaev")
				 ShellStatusText = res.ToString("s\\.fff");
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		/// <summary>
		/// Вызывает закрытие окна прогресса для указанного родительского контрола.
		/// </summary>
		/// <param name="parentControl">Контрол, который будет родительским для окна прогресса.</param>
		public TimeSpan HideProgressWindow(Control parentControl)
		{
			if(progressList.ContainsKey(parentControl) == false)
				return TimeSpan.Zero;

			NetProgressControl progressForm = progressList[parentControl];
			progressList.Remove(parentControl);
			TimeSpan res = progressForm.ElapsedTime;

			if(progressForm.Parent == null)
			{
				progressForm.Dispose();
				progressForm = null;
				return res;
			}

			Control parent = progressForm.Parent;
			parent.Controls.Remove(progressForm);
			progressForm.Dispose();
			progressForm = null;
			foreach(Control ctrl in parent.Controls)
				ctrl.Enabled = true;

			if(focusedControl != null && focusedControl.Enabled)
			{
				focusedControl.Select();
				focusedControl.Focus();
			}
			return res;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод, вызываемый при нажатии кнопки отмены окна прогресса.
		/// Реализация по умолчанию прерывает все задачи, скрывает окно прогресса и показывает предупреждение.
		/// </summary>
		/// <param name="parentControl">Контрол, являющийся родительским для окна прогресса.</param>
		protected virtual void ProgressWindowCancelClick(Control parentControl)
		{
			//this.MFES.Pulsar.BreakAllSendersAsyncTasks(this);
			TaskManager.AbortTasks(this);
			HideProgressWindow(parentControl);
			string str = "Вы остановили процесс загрузки данных.\r\n" + 
			"Это может привести к неверной работе формы и ошибкам.\r\n" + 
			"Продолжение работы с формой не рекомендуется!!!";
			SimModalMessageBox.Show(parentControl, str, "Внимание!", MessageBoxIcon.Warning);
		}
		#endregion Progress Window Methods
		//-------------------------------------------------------------------------------------
		#region IAsyncTaskDestination Members
		//-------------------------------------------------------------------------------------
		bool IAsyncTaskDoneHandler.InvokeRequired
		{
			get { return PanelBack.InvokeRequired; }
		}
		//-------------------------------------------------------------------------------------
		void IAsyncTaskDoneHandler.Invoke(AsyncTask task)
		{
			PanelBack.Invoke(new Action<AsyncTask>(AsyncTaskDone), task);
		}
		#endregion IAsyncTaskDestination Members
		//-------------------------------------------------------------------------------------
	}
	
}