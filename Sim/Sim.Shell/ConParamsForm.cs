using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


using Pulsar;
using Pulsar.Clients;
using Pulsar.Server;

namespace Sim
{
	/// <summary>
	/// Класс формы выбора параметров сервера.
	/// </summary>
	public partial class ConParamsForm :Form
	{
		ParamsDic backup = null;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public ConParamsForm()
		{
			InitializeComponent();

			backup = new ParamsDic();
			foreach(PropertyInfo pi in typeof(PulsarConnection).GetProperties(BindingFlags.Static | BindingFlags.Public))
				backup[pi.Name] = pi.GetValue(null, null);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		private void FormConnections_Load(object sender, EventArgs e)
		{
			try
			{
				switch(PulsarConnection.Default.ConnectionType)
				{
					case 1: radioButtonAuto.Checked = true; break;
					case 2: radioButtonCentral.Checked = true; break;
					case 3: radioButtonOther.Checked = true; break;
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void radioButtonType_CheckedChanged(object sender, EventArgs e)
		{
			if(((RadioButton)sender).Checked == false)
				return;
			switch(((RadioButton)sender).Name)
			{
				case "radioButtonAuto":
					PulsarConnection.Default.ConnectionType = 1;
					textBoxServer.Enabled = false;
					nudPort.Enabled = false;
				break;
				case "radioButtonCentral"  :
				PulsarConnection.Default.ConnectionType = 2;
					textBoxServer.Enabled = false;
					nudPort.Enabled = false;
				break;
				case "radioButtonOther":
				PulsarConnection.Default.ConnectionType = 3;
					textBoxServer.Enabled = true;
					nudPort.Enabled = true;
				break;
			}
			if(PulsarConnection.Default.ConnectionType	!= 3)
			{
			 Tuple<string,int> add = PulsarConnection.Default.GetServerAddress(PulsarConnection.Default.ConnectionType);
		 	PulsarConnection.Default.Address = add.Item1;
			 PulsarConnection.Default.Port = add.Item2;
			}
			textBoxServer.Text = PulsarConnection.Default.Address;
			nudPort.Value = PulsarConnection.Default.Port;
			nudReceiveTimeout.Value = PulsarConnection.Default.ReceiveTimeOut / 1000;
			nudReceiveBuf.Value = PulsarConnection.Default.ReceiveBufferSize;
			nudSendTimeout.Value = PulsarConnection.Default.SendTimeOut / 1000;
			nudSendBuf.Value = PulsarConnection.Default.SendBufferSize;

		}
		//-------------------------------------------------------------------------------------
		private void buttonTest_Click(object sender, EventArgs e)
		{
			try
			{
				PulsarConnection.Default.Address = textBoxServer.Text;
				PulsarConnection.Default.Port = (int)nudPort.Value;
				PulsarConnection.Default.ReceiveBufferSize = (int)nudReceiveBuf.Value;
				PulsarConnection.Default.ReceiveTimeOut = (int)nudReceiveTimeout.Value * 1000;
				PulsarConnection.Default.SendBufferSize = (int)nudSendBuf.Value;
				PulsarConnection.Default.SendTimeOut = (int)nudSendTimeout.Value * 1000;

				try
				{
					this.UseWaitCursor = true;
					Application.DoEvents();
					
					PulsarQuery q = new PulsarQuery("Pulsar", "Status");
					q.Context = new Context();
					q.Params = PulsarQueryParams.Servant;

					object obj = PulsarConnection.Default.Get(q);
					if(obj == null)
						throw new Exception("Статус сервера не определен!");
					//if(query["Status"] == null || (PulsarServantStatus)query["Status"] != PulsarServantStatus.Ready)
					// throw new Exception("Статус сервера не положителен!");

					string msg = String.Format("Соединение с сервером {0} успешно установлено!", PulsarConnection.Default.ToString());
					MessageBox.Show(msg, "Тест соединения", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch(Exception exc)
				{
					string msg = String.Format("Ошибка соединения с сервером {0}:\n\n{1}", PulsarConnection.Default.ToString(), exc.Message);
					MessageBox.Show(msg, "Тест соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
			this.UseWaitCursor = false;
		}
		//-------------------------------------------------------------------------------------
		private void buttonOk_Click(object sender, EventArgs e)
		{
			PulsarConnection.Default.Address = textBoxServer.Text;
			PulsarConnection.Default.Port = (int)nudPort.Value;
			PulsarConnection.Default.ReceiveBufferSize = (int)nudReceiveBuf.Value;
			PulsarConnection.Default.ReceiveTimeOut = (int)nudReceiveTimeout.Value * 1000;
			PulsarConnection.Default.SendBufferSize = (int)nudSendBuf.Value;
			PulsarConnection.Default.SendTimeOut = (int)nudSendTimeout.Value * 1000;
			if(checkBoxAlways.Checked)
			{
				ServerParamsBase.SetParam("Connection", "ConnectionType", PulsarConnection.Default.ConnectionType);
				if(radioButtonOther.Checked)
				{
					ServerParamsBase.SetParam("Connection", "Address", PulsarConnection.Default.Address);
					ServerParamsBase.SetParam("Connection", "Port", PulsarConnection.Default.Port);
				}
			}
			ServerParamsBase.SetParam("Connection", "ReceiveBufferSize", PulsarConnection.Default.ReceiveBufferSize);
			ServerParamsBase.SetParam("Connection", "ReceiveTimeOut", PulsarConnection.Default.ReceiveTimeOut);
			ServerParamsBase.SetParam("Connection", "SendBufferSize", PulsarConnection.Default.SendBufferSize);
			ServerParamsBase.SetParam("Connection", "SendTimeOut", PulsarConnection.Default.SendTimeOut);
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}
		//-------------------------------------------------------------------------------------
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			foreach(string s in backup.Params)
				typeof(PulsarConnection).GetProperty(s,BindingFlags.Public | BindingFlags.Static).SetValue(null, backup[s],null);
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}
		//-------------------------------------------------------------------------------------
		#endregion << Controls Handlers >>
	}
}