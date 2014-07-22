using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// Класс контрола сообщения об ошибке.
	/// </summary>
	public partial class ModalErrorBox : Sim.Controls.SimModalControl
	{
		private bool isClosed = false;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public ModalErrorBox()
		{
			InitializeComponent();
			flErrText.BackColor = ftbStack.BackColor = ControlPaint.Light(SystemColors.Control, 0.7f);
			//flAsm.BackColor = flClass.BackColor = flMethod.BackColor = flFile.BackColor = flLine.BackColor = 
			// ControlPaint.Light(SystemColors.Control, 0.7f);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void button2_Click(object sender, EventArgs e)
		{
			if (button2.Text == "+")
			{
				fpStack.Visible = true;
				button2.Text = "-";
			}
			else
			{
				fpStack.Visible = false;
				button2.Text = "+";
			}

		}
		//-------------------------------------------------------------------------------------
		private void buttonOK_Click(object sender, EventArgs e)
		{
			isClosed = true;
			Hide();
			OnDialogClosed(DialogResult.OK);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Отображает контрол
		/// </summary>
		/// <param name="Exc">Исключение для отображения.</param>
		public static void Show(Exception Exc)
		{
			if (Application.OpenForms[0].Name != "MainForm")
			{
				ErrorBox.Show(Exc);
				return;
			}
			Control[] ct = Application.OpenForms[0].Controls.Find("tabViewer1", true);
			TabControl tab = null;
			foreach (Control c in ct)
				if (c.GetType().Name == "TabViewer")
				{
					tab = (TabControl)c;
					break;
				}
			if (tab == null)
			{
				ErrorBox.Show(Exc);
				return;
			}
			if (tab.SelectedTab.Controls.Count == 0 || tab.SelectedTab.Controls[0].Name != "PanelBack")
			{
				ErrorBox.Show(Exc);
				return;
			}
			Show(Exc, tab.SelectedTab.Controls[0]);
		}
		/// <summary>
		/// Отображает контрол
		/// </summary>
		/// <param name="Exc">Исключение для отображения.</param>
		/// <param name="parent">Родительский контрол.</param>
		public static void Show(Exception Exc, Control parent)
		{
			ModalErrorBox box = null;
			try
			{
				box = new ModalErrorBox();
				box.flAsm.Text = Exc.Source;
				box.flClass.Text = Exc.TargetSite.DeclaringType.FullName;
				box.flMethod.Text = Exc.TargetSite.ToString();
				box.flErrText.Text = Exc.Message;

				System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(Exc, true);
				for (int a = 0; a < trace.FrameCount; a++)
					if (trace.GetFrame(a).GetFileName() != null)
					{
						box.flFile.Text = System.IO.Path.GetFileName(trace.GetFrame(a).GetFileName());
						box.flLine.Text = trace.GetFrame(a).GetFileLineNumber().ToString();
						break;
					}

				string s = "Стек исключения:\r\n";
				s += trace.ToString();
				s += "\r\nСтек программы:\r\n";
				s += (new System.Diagnostics.StackTrace(1)).ToString();
				s = s.Replace("\t", "").Trim();
				s = s.Replace("at ", "\u2514\u25ba ");
				s = s.Replace("в ", "\u2514\u25ba ");
				box.ftbStack.Text = s; //\u2500

				System.Media.SystemSounds.Exclamation.Play();

				box.Show(parent);
				while (box.isClosed == false)
				{
					if (box.IsDisposed || parent.IsDisposed || box.FindForm() == null)
						break;
					Application.DoEvents();
					System.Threading.Thread.Sleep(50);
				}
			}
			catch
			{
				ErrorBox.Show(Exc);
			}
			finally
			{
				if (box != null)
					box.Dispose();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Отображает контрол для серверной ошибки
		/// </summary>
		/// <param name="error">Текст ошибки</param>
		/// <param name="parent">Родительский контрол.</param>
		public static void ShowServer(string error, Control parent)
		{
			ModalErrorBox box = null;
			try
			{
				box = new ModalErrorBox();
				box.finistLabelCaption.Text = "Ошибка сервера!";
				box.flErrCaption.Text = "Сервер сообщает:";
				box.fpServMessage.Visible = true;
				box.fpServMessage.Dock = DockStyle.Fill;
				box.fpServMessage.BringToFront();
				box.flErrText.Text = error;
				box.pictureBox1.Image = global::Sim.Controls.Properties.Resources.ErrorServer;
				box.button2.Enabled = false;

				System.Media.SystemSounds.Exclamation.Play();

				box.Show(parent);
				while (box.isClosed == false)
				{
					if (box.IsDisposed || parent.IsDisposed || box.FindForm() == null)
						break;
					Application.DoEvents();
					System.Threading.Thread.Sleep(50);
				}
			}
			catch (Exception err)
			{
				ErrorBox.Show(err);
			}
			finally
			{
				if (box != null)
					box.Dispose();
			}
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------

	}
}
