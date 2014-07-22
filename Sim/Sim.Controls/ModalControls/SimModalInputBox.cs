using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// ����� ���������� �������� ����� ��������
	/// </summary>
	public partial class SimModalInputBox : Sim.Controls.SimModalControl
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ����� ��������� ��������.
		/// </summary>
		public string CaptionText
		{
			get { return this.finistLabelCaption.Text.Trim(); }
			set { this.finistLabelCaption.Text = "  " + value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ����������� ��������� ��������.
		/// </summary>
		public Image CaptionImage
		{
			get { return this.finistLabelCaption.Image; }
			set { this.finistLabelCaption.Image = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������� ����� ��������.
		/// </summary>
		public new string Text
		{
			get { return finistLabelText.Text; }
			set { finistLabelText.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������ ������ �����.
		/// </summary>
		public TextBoxFormat ValueFormat
		{
			get { return finistTextBoxValue.Format; }
			set { finistTextBoxValue.Format = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������ �������� ���������� ������� �����.
		/// </summary>
		public string ValueFormatException
		{
			get { return finistTextBoxValue.FormatException; }
			set { finistTextBoxValue.FormatException = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ��������� ��������.
		/// </summary>
		public string Value
		{
			get { return finistTextBoxValue.Text; }
			set { finistTextBoxValue.Text = value; }
		}

		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public SimModalInputBox()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.Hide();
			result = DialogResult.OK;
			OnDialogClosed(DialogResult.OK);
		}
		//-------------------------------------------------------------------------------------
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Hide();
			result = DialogResult.Cancel;
			OnDialogClosed(DialogResult.Cancel);
		}
		//-------------------------------------------------------------------------------------
		private void finistTextBoxValue_TextChanged(object sender, EventArgs e)
		{
			if (finistTextBoxValue.Text.Length == 0)
				buttonOK.Enabled = false;
			else if (buttonOK.Enabled == false)
				buttonOK.Enabled = true;
		}
		//-------------------------------------------------------------------------------------
		private void finistTextBoxValue_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				buttonOK_Click(buttonOK, EventArgs.Empty);
			}
		}
		#endregion << Controls Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ���������� �������.
		/// </summary>
		/// <param name="parent">������������ �������, ������������ �������� ����������� �����������.</param>
		public override void Show(Control parent)
		{
			base.Show(parent);
			finistTextBoxValue.SelectAll();
			finistTextBoxValue.Select();
			finistTextBoxValue.Focus();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� �������.
		/// </summary>
		public override void Show()
		{
			base.Show();
			finistTextBoxValue.Select();
			finistTextBoxValue.Focus();
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		/// <summary>
		/// ���������� �������.
		/// </summary>
		/// <param name="parent">������������ �������, ������������ �������� ����������� �����������.</param>
		/// <param name="caption">����� ��������� ��������.</param>
		/// <param name="image">����������� ��������� ��������.</param>
		/// <param name="text">�������� ����� ��������.</param>
		/// <param name="value">������, ���������� �������� �����.</param>
		/// <param name="format">������ ������ �����.</param>
		/// <param name="formatException">������ �������� ���������� ������� �����.</param>
		/// <returns>DialogResult.OK ��� DialogResult.Cancel</returns>
		public static DialogResult Show(Control parent, ref string value,
										string caption = "���� ��������",
										string text = "������� ����� ��������:",
										Image image = null,
										TextBoxFormat format = TextBoxFormat.NotSet,
										string formatException = "")
		{
			SimModalInputBox box = new SimModalInputBox();
			box.CaptionText = caption;
			box.CaptionImage = image;
			box.Text = text;
			box.Value = value;
			box.ValueFormat = format;
			box.ValueFormatException = formatException;
			box.Show(parent);
			while (box.result == DialogResult.None && box.Visible)
			{
				if (box.IsDisposed || parent.IsDisposed || box.FindForm() == null)
					break;
				Application.DoEvents();
				Thread.Sleep(50);
			}
			if (box.result == DialogResult.OK)
				value = box.Value;
			return box.result == DialogResult.None ? DialogResult.Cancel : box.result;
		}
		#endregion << Static Methods >>

		//-------------------------------------------------------------------------------------

	}
}

