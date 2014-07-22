using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using System.Net.Sockets;


namespace Sim
{
	/// <summary>
	/// Класс формы выбора параметров сервера.
	/// </summary>
	public partial class FormUpdateProgress :Form
	{
		public string StepText
		{
			get { return labelStepText.Text; }
			set { labelStepText.Text = value; }
		}
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public FormUpdateProgress()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		private void radioButtonType_CheckedChanged(object sender, EventArgs e)
		{
			if(((RadioButton)sender).Checked == false)
				return;
			switch(((RadioButton)sender).Name)
			{
				case "radioButtonAuto":
					Program.ConParams["ConnectionType"] = 1;
					textBoxServer.Enabled = false;
					nudPort.Enabled = false;
					break;
				case "radioButtonCentral":
					Program.ConParams["ConnectionType"] = 2;
					textBoxServer.Enabled = false;
					nudPort.Enabled = false;
					break;
				case "radioButtonOther":
					Program.ConParams["ConnectionType"] = 3;
					textBoxServer.Enabled = true;
					nudPort.Enabled = true;
					break;
			}
			Program.InitConParams((int)Program.ConParams["ConnectionType"]);
			textBoxServer.Text = (string)Program.ConParams["Address"];
			nudPort.Value = (int)Program.ConParams["Port"];
		}
		//-------------------------------------------------------------------------------------
		private void buttonTest_Click(object sender, EventArgs e)
		{
			try
			{
				SetParams();
				pictureBoxLoad.BringToFront();
				labelStepText.Text = "Проверка соединения с сервером ...";
				panelParams.Enabled = false;

				BackgroundWorker w = new BackgroundWorker();
				w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(w_RunWorkerCompleted);
				w.DoWork += new DoWorkEventHandler(w_DoWork);
				w.RunWorkerAsync();
			}
			catch(Exception Err)
			{
				MessageBox.Show(Err.Message + Err.StackTrace, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		void w_DoWork(object sender, DoWorkEventArgs e)
		{
			TcpClient client = null;
			try
			{
			 client = new TcpClient();
			 client.ReceiveTimeout = (int)Program.ConParams["ReceiveTimeOut"];
			 client.ReceiveBufferSize = (int)Program.ConParams["ReceiveBufferSize"];
			 client.SendTimeout = (int)Program.ConParams["SendTimeOut"];
			 client.SendBufferSize = (int)Program.ConParams["SendBufferSize"];
			 client.Connect((string)Program.ConParams["Address"], (int)Program.ConParams["Port"]);
			}
			catch(Exception err)
			{
				e.Result = err.Message;
			}
			finally
			{
				if(client != null)
					client.Close();
			}
		}
		void w_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if(e.Result == null)
			{
				this.SuspendLayout();
				panelError.BringToFront();
				labelStepText.Text = "";
				labelError.Text = "Соединение с сервером успешно установленно!";
				pictureBox2.Image = global::Sim.Properties.Resources.Info48;
				this.ResumeLayout();
			}
			else
			{
				this.SuspendLayout();
				panelError.BringToFront();
				labelStepText.Text = "Ошибка соединения с сервером!";
				labelError.Text = (string)e.Result; 
				pictureBox2.Image = global::Sim.Properties.Resources.Error48;
				this.ResumeLayout();
			}
			panelParams.Enabled = true;
		}
		//-------------------------------------------------------------------------------------
		private void buttonOk_Click(object sender, EventArgs e)
		{
		 SetParams();
			if(checkBoxAlways.Checked)
		  Program.SaveParams();
			DialogResult = DialogResult.OK;
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
		//-------------------------------------------------------------------------------------
		#endregion << Controls Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		public void SetError(string text)
		{
			this.SuspendLayout();
			labelError.Text = text;
			pictureBox2.Image = global::Sim.Properties.Resources.Error48;
			panelError.BringToFront();
			labelStepText.Text = "Ошибка соединения с сервером!";
			if(panelParams.Visible == false)
			{
				switch((int)Program.ConParams["ConnectionType"])
				{
					case 1: radioButtonAuto.Checked = true; break;
					case 2: radioButtonCentral.Checked = true; break;
					case 3: radioButtonOther.Checked = true; break;
				}
				textBoxServer.Text = (string)Program.ConParams["Address"];
				nudPort.Value = (int)Program.ConParams["Port"];
				nudReceiveTimeout.Value = (int)Program.ConParams["ReceiveTimeOut"] / 1000;
				nudReceiveBuf.Value = (int)Program.ConParams["ReceiveBufferSize"];
				nudSendTimeout.Value = (int)Program.ConParams["SendTimeOut"] / 1000;
				nudSendBuf.Value = (int)Program.ConParams["SendBufferSize"];

				panelParams.Visible = true;
			}
			this.ResumeLayout();
			this.CenterToScreen();
			
			this.DialogResult = DialogResult.None;

   while(DialogResult == DialogResult.None|| this.FindForm() == null)
			{
			 if(this.IsDisposed)
				 break;
			 Application.DoEvents();
			 System.Threading.Thread.Sleep(5);
			}

		}
		public void SetProgress()
		{
			pictureBoxLoad.BringToFront();
			panelParams.Visible = false;
			this.CenterToScreen();
		}
		//-------------------------------------------------------------------------------------
		private void SetParams()
		{
		 if(radioButtonOther.Checked)
			{
			 Program.ConParams["Address"] = textBoxServer.Text.TrimStart().TrimEnd();
			 Program.ConParams["Port"] = (int)nudPort.Value;
			}
			Program.ConParams["ReceiveBufferSize"] = (int)nudReceiveBuf.Value;
			Program.ConParams["ReceiveTimeOut"] = (int)nudReceiveTimeout.Value * 1000;
			Program.ConParams["SendBufferSize"]= (int)nudSendBuf.Value;
			Program.ConParams["SendTimeOut"] = (int)nudSendTimeout.Value * 1000;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
						
	}
}