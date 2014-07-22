using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Pulsar;
using Pulsar.Clients;
using Pulsar.Server;
using Pulsar.Serialization;
using Sim.Controls;

//using OID = System.Guid;

namespace Sim
{
	/// <summary>
	/// Класс формы аутентификации пользователей.
	/// </summary>
	public partial class AuthForm :Form
	{
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public AuthForm()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public AuthForm(object conParams)	: this()
		{
			Dictionary<string, object> dic = (Dictionary<string, object>)conParams;

			PulsarConnection.Default.ConnectionType = (int)dic["ConnectionType"];
			PulsarConnection.Default.Address = (string)dic["Address"];
			PulsarConnection.Default.Port = (int)dic["Port"];
			PulsarConnection.Default.ReceiveTimeOut = (int)dic["ReceiveTimeOut"];
			PulsarConnection.Default.ReceiveBufferSize = (int)dic["ReceiveBufferSize"];
			PulsarConnection.Default.SendTimeOut = (int)dic["SendTimeOut"];
			PulsarConnection.Default.SendBufferSize = (int)dic["SendBufferSize"];

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Form Handlers >>
		private void AuthForm_Load(object sender, EventArgs e)
		{
			try
			{
				PulsarQuery q = new PulsarQuery("Persons", "GetNamesList");
				Dictionary<OID,string> us = (Dictionary<OID, string>)PulsarConnection.Default.Get(q);
				FillUsers(us);
				textBoxPassword.Select();
				textBoxPassword.Focus();
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		#endregion << Form Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Control's Handlers >>
		private void comboBoxUsers_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if(comboBoxUsers.SelectedItem != null && textBoxPassword.Text.Length > 0)
				buttonOk.Enabled = true;
			else
				buttonOk.Enabled = false;
		}
		//-------------------------------------------------------------------------------------
		private void buttonOk_Click(object sender, EventArgs e)
		{
			try
			{
				OID sid = ((ComboBoxItem<OID>)comboBoxUsers.SelectedItem).Key;
				dynamic u = null;
				PulsarQuery q = new PulsarQuery("Security", "CheckPass",
											new { CheckPass = new object[] { sid, Pulsar.Security.Hash.GetCRC32(textBoxPassword.Text) } });
				object res = PulsarConnection.Default.Exec(q);
				if(res == null || res is Boolean == false || (bool)res == false)
				{
					MessageBox.Show("Пароль указан не верно!", "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
					PulsarQuery.ContextQuery.User = null;
					return;
				}
				u = PulsarConnection.Default.Get(new PulsarQuery("Persons", "Item", new { Item = sid },
																																					PulsarQueryParams.NoStubEssences));
				if(u == null)
					throw new Exception("Сервер не вернул объект пользователя!");

				PulsarQuery.ContextQuery.User = u;
				ServerParamsBase.SetParam("Connection", "PrevLogon", PulsarQuery.ContextQuery.User.OID);
				this.DialogResult = DialogResult.OK;
				this.Close();
				return;
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void buttonConnect_Click(object sender, EventArgs e)
		{
			using(ConParamsForm frm = new ConParamsForm())
				if(frm.ShowDialog() == DialogResult.OK)
				{
					Dictionary<OID,string> us = 
						(Dictionary<OID, string>)PulsarConnection.Default.Get("Persons", "GetNamesList", null);
					FillUsers(us);
					comboBoxUsers.Select();
				}
		}
		//-------------------------------------------------------------------------------------
		private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				buttonOk_Click(this, null);
			}
		}
		#endregion << Control's Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		private void FillUsers(Dictionary<OID, string> users)
		{
			try
			{
				//*** Имя сервера ***//
				labelServer.Text = PulsarConnection.Default.ToString();

				comboBoxUsers.Items.Clear();

				OID prevUser = new OID(
					(string)ServerParamsBase.GetParam("Connection", "PrevLogon", "00000000-0000-0000-0000-000000000000"));
				ComboBoxItem<OID> cur = null;

				foreach(var u in users)
				{
					ComboBoxItem<OID> i = new ComboBoxItem<OID>(u.Key,u.Value);
					if(u.Key == prevUser)
						cur = i;
					comboBoxUsers.Items.Add(i);
				}
				if(cur != null)
					comboBoxUsers.SelectedItem = cur;
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		#endregion << Methods >>

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}
	}
	//**************************************************************************************
}