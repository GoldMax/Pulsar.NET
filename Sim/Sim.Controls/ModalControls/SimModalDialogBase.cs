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
	/// Базовый контрол модальных диалогов
	/// </summary>
	public partial class SimModalDialogBase : Sim.Controls.SimModalControl
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Доступность кнопки OK.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(false)]
		public bool ButtonOkEnabled
		{
			get { return buttonOK.Enabled; }
			set { buttonOK.Enabled = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Текст заголовка диалога
		/// </summary>
		[Category("Appearance")]
		[Description("Текст заголовка диалога")]
		public string Caption
		{
			get { return finistLabelCaption.Text; }
			set { finistLabelCaption.Text = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Изображение заголовка диалога
		/// </summary>
		[Category("Appearance")]
		[Description("Изображение заголовка диалога")]
		public Image CaptionImage
		{
			get { return finistLabelCaption.Image; }
			set { finistLabelCaption.Image = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimModalDialogBase()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void buttonOK_Click(object sender, EventArgs e)
		{
			ButtonOK_Click();
		}
		protected virtual void ButtonOK_Click()
		{
			this.Hide(DialogResult.OK);
		}
		//-------------------------------------------------------------------------------------
		void buttonCancel_Click(object sender, EventArgs e)
		{
			ButtonCancel_Click();
		}
		protected virtual void ButtonCancel_Click()
		{
			this.Hide(DialogResult.Cancel);
		}
		#endregion << Controls Handlers>>

	}
}
