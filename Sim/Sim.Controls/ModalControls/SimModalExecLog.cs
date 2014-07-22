using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Sim.Controls;

namespace Sim.Controls
{
	/// <summary>
	/// Класс контрола отображения хода выполнения операции.
	/// </summary>
	public partial class SimModalExecLog : Sim.Controls.SimModalControl
	{
		private DateTime start;

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Заголовок контрола
		/// </summary>
		public string Caption
		{
			get { return finistLabelCaption.Text; }
			set { finistLabelCaption.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, может ли выполнение быть прерванным.
		/// </summary>
		public bool CanCancel
		{
			get { return buttonStop.Enabled; }
			set { buttonStop.Enabled = value; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimModalExecLog()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void timer1_Tick(object sender, EventArgs e)
		{
			string s = DateTime.Now.Subtract(start).ToString();
			if(s.IndexOf('.') != -1)
				s = s.Substring(0, s.IndexOf('.'));
			labelTime.Text = s;
		}
		//-------------------------------------------------------------------------------------
		private void SimModalBatchExec_VisibleChanged(object sender, EventArgs e)
		{
			timer1.Enabled = this.Visible;
			start = DateTime.Now;
			labelTime.Text = "00:00:00";
		}
		//-------------------------------------------------------------------------------------
		private void buttonStop_Click(object sender, EventArgs e)
		{
			this.Hide();
			OnDialogClosed(buttonStop.DialogResult);
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Устанавливет контрол в успешное финальное сосотояние.
		/// </summary>
		public void SetFinalState(DialogResult result)
		{
			pictureBox1.Visible = false;
			timer1.Enabled = false;
			buttonStop.DialogResult = result;
			if(result == DialogResult.OK)
			{
				buttonStop.Image = global::Sim.Controls.Properties.Resources.OK;
				buttonStop.Text = "ОК";
			}
			else if(result == DialogResult.Cancel)
			{
				buttonStop.Image = global::Sim.Controls.Properties.Resources.Cancel;
				buttonStop.Text = "Выход";
			}
			buttonStop.Enabled = true;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет текст к логу
		/// </summary>
		public void AppendLog(byte shift, string s, params object[] args)
		{
			if(rtbLog.InvokeRequired)
				this.Invoke(new Action<byte, string, bool, Color, object[]>(AppendLog),
					shift, s, false, rtbLog.ForeColor, args); 
			else
				AppendLog(shift, s, false, rtbLog.ForeColor, args);
		}
		/// <summary>
		/// Добавляет текст к логу
		/// </summary>
		public void AppendLog(byte shift, string s, bool bold, params object[] args)
		{
			if(rtbLog.InvokeRequired)
				this.Invoke(new Action<byte, string, bool, Color, object[]>(AppendLog),
					shift, s, bold, rtbLog.ForeColor, args); 
			else
				AppendLog(shift, s, bold, rtbLog.ForeColor, args);
		}
		/// <summary>
		/// Добавляет текст к логу
		/// </summary>
		public void AppendLog(byte shift, string s, Color foreColor, params object[] args)
		{
			if(rtbLog.InvokeRequired)
				this.Invoke(new Action<byte, string, bool, Color, object[]>(AppendLog),
					shift, s, false, foreColor, args); 
			else
				AppendLog(shift, s, false, foreColor, args);
		}
		/// <summary>
		/// Добавляет текст к логу
		/// </summary>
		public void AppendLog(byte shift, string s, bool bold, Color foreColor, params object[] args)
		{
			if(rtbLog.InvokeRequired)
				this.Invoke(new Action<byte, string, bool, Color, object[]>(AppendLog),
					shift, s, bold, foreColor, args);
			else if(bold || foreColor != rtbLog.ForeColor)
			{
				int st = rtbLog.TextLength;
				rtbLog.AppendText(new String(' ',shift) + String.Format(s, args));
				rtbLog.Select(st, rtbLog.TextLength);
				if(bold)
					rtbLog.SelectionFont = new Font(rtbLog.Font, FontStyle.Bold);
				if(foreColor != rtbLog.ForeColor)
					rtbLog.SelectionColor = foreColor;
				rtbLog.Select(rtbLog.TextLength, rtbLog.TextLength);
				if(bold)
					rtbLog.SelectionFont = rtbLog.Font;
				if(foreColor != rtbLog.ForeColor)
					rtbLog.SelectionColor = rtbLog.ForeColor;
			}
			else
				rtbLog.AppendText(new String(' ',shift) + String.Format(s, args));
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет пустую строку к логу
		/// </summary>
		public void AppendLogLine()
		{
			if(rtbLog.InvokeRequired)
				this.Invoke(new Action<byte, string, bool, Color, object[]>(AppendLogLine),
					0, "", false, rtbLog.ForeColor, new object[0]);
			else
				AppendLogLine(0, "", false, rtbLog.ForeColor, new object[0]);
		}
		/// <summary>
		/// Добавляет строку к логу
		/// </summary>
		public void AppendLogLine(byte shift, string s, params object[] args)
		{ 
			s += "\r\n";
			AppendLog(shift, s, args);
		}
		/// <summary>
		/// Добавляет строку к логу
		/// </summary>
		public void AppendLogLine(byte shift, string s, bool bold, params object[] args)
		{ 
			s += "\r\n";
			AppendLog(shift, s, bold, args);
		}
		/// <summary>
		/// Добавляет строку к логу
		/// </summary>
		public void AppendLogLine(byte shift, string s, Color foreColor, params object[] args)
		{ 
			s += "\r\n";
			AppendLog(shift, s, foreColor, args);
		}
		/// <summary>
		/// Добавляет строку к логу
		/// </summary>
		public void AppendLogLine(byte shift, string s, bool bold, Color foreColor, params object[] args)
		{ 
			s += "\r\n";
			AppendLog(shift, s, bold, foreColor, args);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Очищает лог, сбрасывает таймер 
		/// </summary>
		public void Reset()
		{
			rtbLog.Clear();
			buttonStop.Image = global::Sim.Controls.Properties.Resources.Stop;
			buttonStop.Text = "Прервать";
			buttonStop.DialogResult = DialogResult.Abort;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
}
