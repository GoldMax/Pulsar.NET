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
	/// Класс модального контрола сообщения
	/// </summary>
	public partial class SimModalMessageBox : Sim.Controls.SimModalControl
	{
		private MessageBoxButtons buttons = MessageBoxButtons.OK;
		private MessageBoxIcon image = MessageBoxIcon.Information;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Текст заголовка контрола.
		/// </summary>
		public string CaptionText
		{
			get { return this.finistLabelCaption.Text.Trim(); }
			set { this.finistLabelCaption.Text = "  " + value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Изображение заголовка контрола.
		/// </summary>
		public Image CaptionImage
		{
			get { return this.finistLabelCaption.Image; }
			set { this.finistLabelCaption.Image = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Основной текст контрола.
		/// </summary>
		public new string Text
		{
			get { return finistLabelText.Text; }
			set { finistLabelText.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Кнопки контрола.
		/// </summary>
		public MessageBoxButtons Buttons
		{
			get { return buttons; }
			set
			{
				flowLayoutPanel1.Controls.Clear();
				switch (value)
				{
					case MessageBoxButtons.OK:
						flowLayoutPanel1.Controls.Add(buttonOK);
						break;
					case MessageBoxButtons.OKCancel:
						flowLayoutPanel1.Controls.Add(buttonOK);
						flowLayoutPanel1.Controls.Add(buttonCancel);
						break;
					case MessageBoxButtons.YesNo:
						flowLayoutPanel1.Controls.Add(buttonYes);
						flowLayoutPanel1.Controls.Add(buttonNo);
						break;
					case MessageBoxButtons.YesNoCancel:
						flowLayoutPanel1.Controls.Add(buttonYes);
						flowLayoutPanel1.Controls.Add(buttonNo);
						flowLayoutPanel1.Controls.Add(buttonCancel);
						break;
					default:
						throw new Exception(String.Format("Комбинация кнопок [{0}] не поддерживается!", value));
				}
				Point p = new Point(finistPanelButtons.Width / 2 - flowLayoutPanel1.Width / 2, flowLayoutPanel1.Location.Y);
				flowLayoutPanel1.Location = p;
				buttons = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Изображение.
		/// </summary>
		public MessageBoxIcon Icon
		{
			get { return image; }
			set
			{
				switch ((int)value)
				{
					case 16:
						pictureBox1.Image = global::Sim.Controls.Properties.Resources.Error48;
						break;
					case 32:
						pictureBox1.Image = global::Sim.Controls.Properties.Resources.Question48;
						break;
					case 48:
						pictureBox1.Image = global::Sim.Controls.Properties.Resources.Warning48;
						break;
					case 64:
						pictureBox1.Image = global::Sim.Controls.Properties.Resources.Info48;
						break;
					default:
						pictureBox1.Image = global::Sim.Controls.Properties.Resources.Info48;
						break;
				}
				image = value;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimModalMessageBox()
		{
			InitializeComponent();
			Buttons = MessageBoxButtons.OK;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers >>
		private void Buttons_Click(object sender, EventArgs e)
		{
			this.Hide();
			switch (((Control)sender).Name)
			{
				case "buttonOK": result = DialogResult.OK; break;
				case "buttonCancel": result = DialogResult.Cancel; break;
				case "buttonYes": result = DialogResult.Yes; break;
				case "buttonNo": result = DialogResult.No; break;
				default: result = DialogResult.Cancel; break;
			}
			OnDialogClosed(result);
		}
		#endregion << Controls Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Отображает контрол.
		/// </summary>
		/// <param name="parent">Родительский контрол, относительно которого эмулируется модальность.</param>
		public override void Show(Control parent)
		{
			base.Show(parent);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Отображает контрол.
		/// </summary>
		public override void Show()
		{
			base.Show();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="result"></param>
		protected override void OnDialogClosed(DialogResult result)
		{
			this.result = result;
			base.OnDialogClosed(result);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Static Methods >>
		/// <summary>
		/// Отображает контрол.
		/// </summary>
		/// <param name="parent">Родительский контрол, относительно которого эмулируется модальность.</param>
		/// <param name="caption">Текст заголовка контрола.</param>
		/// <param name="text">Основной текст контрола.</param>
		/// <param name="icon">Тип отображаемой иконки.</param>
		/// <param name="buttons">Набор кнопок контрола.</param>
		/// <returns>Значение из перечисления DialogResult</returns>
		public static DialogResult Show(Control parent, string text, string caption = "Сообщение",
		 MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxButtons buttons = MessageBoxButtons.OK)
		{
			SimModalMessageBox box = new SimModalMessageBox();
			box.CaptionText = caption;
			box.Text = text;
			box.Icon = icon;
			box.Buttons = buttons;
			box.Show(parent);
			while (box.result == DialogResult.None && box.Visible)
			{
				if (box.IsDisposed || parent.IsDisposed || box.FindForm() == null)
					break;
				Application.DoEvents();
				Thread.Sleep(50);
			}
			return box.result == DialogResult.None ? DialogResult.Cancel : box.result;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Отображает сообщение об ошибке.
		/// </summary>
		/// <param name="parent">Родительский контрол, относительно которого эмулируется модальность.</param>
		/// <param name="caption">Текст заголовка контрола.</param>
		/// <param name="text">Основной текст контрола.</param>
		/// <returns>Значение из перечисления DialogResult</returns>
		public static DialogResult ShowError(Control parent, string text, string caption = "Ошибка")
		{
			return Show(parent, text, caption, MessageBoxIcon.Error, MessageBoxButtons.OK);
		}
		#endregion << Static Methods >>
		//-------------------------------------------------------------------------------------

	}
}

