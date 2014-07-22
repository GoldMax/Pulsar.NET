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
	/// ����� ���������� �������� ����� ������ � ��� ����.
	/// </summary>
	public partial class SimModalDualInputBox : Sim.Controls.SimModalControl
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
		/// ��������� ������� ����.
		/// </summary>
		public string Text1
		{
			get { return finistLabelName.Text; }
			set { finistLabelName.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ��������� ������� ����.
		/// </summary>
		public string Text2
		{
			get { return finistLabelDesc.Text; }
			set { finistLabelDesc.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������ ������ ����� ������� ����.
		/// </summary>
		public TextBoxFormat ValueFormat
		{
			get { return finistTextBoxName.Format; }
			set { finistTextBoxName.Format = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ������ �������� ���������� ������� ����� ������� ����.
		/// </summary>
		public string ValueFormatException
		{
			get { return finistTextBoxName.FormatException; }
			set { finistTextBoxName.FormatException = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ��������� �������� ������� ����.
		/// </summary>
		public string Value1
		{
			get { return finistTextBoxName.Text; }
			set { finistTextBoxName.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ��������� �������� ������� ����.
		/// </summary>
		public string Value2
		{
			get { return finistTextBoxDesc.Text; }
			set { finistTextBoxDesc.Text = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public SimModalDualInputBox()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		private void Buttons_Click(object sender, EventArgs e)
		{
			try
			{
				this.Hide();
				if (((Control)sender).Name == "buttonOK")
					result = DialogResult.OK;
				else
					result = DialogResult.Cancel;
				OnDialogClosed(result);
			}
			catch (Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void finistTextBoxName_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (finistTextBoxName.Text.Length == 0)
					buttonOK.Enabled = false;
				else if (buttonOK.Enabled == false)
					buttonOK.Enabled = true;
			}
			catch (Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void finistTextBoxDesc_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
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
			finistTextBoxName.SelectAll();
			finistTextBoxName.Select();
			finistTextBoxName.Focus();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� �������.
		/// </summary>
		public override void Show()
		{
			base.Show();
			finistTextBoxName.Select();
			finistTextBoxName.Focus();
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
		/// <param name="text1">��������� ������� ����.</param>
		/// <param name="text2">��������� ������� ����.</param>
		/// <param name="value1">������, ���������� �������� ����� ������� ����.</param>
		/// <param name="value2">������, ���������� �������� ����� ������� ����.</param>
		/// <param name="format">������ ������ �����.</param>
		/// <param name="formatException">������ �������� ���������� ������� �����.</param>
		/// <returns>DialogResult.OK ��� DialogResult.Cancel</returns>
		public static DialogResult Show(Control parent, string caption, Image image, string text1, string text2,
								 ref string value1, ref string value2, TextBoxFormat format, string formatException)
		{
			SimModalDualInputBox box = new SimModalDualInputBox();
			box.CaptionText = caption;
			box.CaptionImage = image;
			box.Text1 = text1;
			box.Text2 = text2;
			box.Value1 = value1;
			box.Value2 = value2;
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
			{
				value1 = box.Value1;
				value2 = box.Value2;
			}
			return box.result == DialogResult.None ? DialogResult.Cancel : box.result;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// ���������� �������.
		/// </summary>
		/// <param name="parent">������������ �������, ������������ �������� ����������� �����������.</param>
		/// <param name="caption">����� ��������� ��������.</param>
		/// <param name="image">����������� ��������� ��������.</param>
		/// <param name="value1">������, ���������� �������� ����� ������� ����.</param>
		/// <param name="value2">������, ���������� �������� ����� ������� ����.</param>
		/// <returns>DialogResult.OK ��� DialogResult.Cancel</returns>
		public static DialogResult Show(Control parent, string caption, Image image, ref string value1, ref string value2)
		{
			SimModalDualInputBox box = new SimModalDualInputBox();
			box.CaptionText = caption;
			box.CaptionImage = image;
			box.Value1 = value1;
			box.Value2 = value2;
			box.Show(parent);
			while (box.result == DialogResult.None && box.Visible)
			{
				if (box.IsDisposed || parent.IsDisposed || box.FindForm() == null)
					break;
				Application.DoEvents();
				Thread.Sleep(50);
			}
			if (box.result == DialogResult.OK)
			{
				value1 = box.Value1;
				value2 = box.Value2;
			}
			return box.result == DialogResult.None ? DialogResult.Cancel : box.result;
		}
		#endregion << Static Methods >>
		//-------------------------------------------------------------------------------------

	}
}

