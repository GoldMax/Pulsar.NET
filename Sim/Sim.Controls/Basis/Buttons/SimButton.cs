using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// Класс основной диалоговой кнопки
	/// </summary>
	[ToolboxBitmap(typeof(Button))]
	[DefaultProperty("Text")]
	[DefaultEvent("Click")]
	public class SimButton : SimButtonBase, IButtonControl
	{
		private DialogResult _res = DialogResult.None;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// 
		/// </summary>
		protected override bool ShowKeyboardCues
		{
			get { return true; }
		}
		#pragma warning disable	1591 
		[DefaultValue(typeof(GradientMode), "TrioVertical")]
		public override GradientMode GradientMode
		{
			get	{ return base.GradientMode; }
			set { base.GradientMode = value; }
		}
		[DefaultValue(typeof(Color), "ControlLightLight")]
		public override Color BackColorStart
		{
			get	{	return base.BackColorStart;	}
			set	{	base.BackColorStart = value;	}
		}
		[DefaultValue(typeof(Color), "Control")]
		public override Color BackColorMiddle
		{
			get	{	return base.BackColorMiddle;	}
			set	{	base.BackColorMiddle = value; }
		}
		[DefaultValue(typeof(Color), "ControlLightLight")]
		public override Color RaisedBackColorStart
		{
			get { return base.RaisedBackColorStart; }
			set { base.RaisedBackColorStart = value; }
		}
		[DefaultValue(1f)]
		public override float RaisedBackColorMiddlePosition
		{
			get	{	return base.RaisedBackColorMiddlePosition;	}
			set	{	base.RaisedBackColorMiddlePosition = value;	}
		}
		[DefaultValue(typeof(Color), "ControlDark")]
		public override Color PushedBackColorStart
		{
			get	{	return base.PushedBackColorStart;	}
			set	{	base.PushedBackColorStart = value;	}
		}
		[DefaultValue(1f)]
		public override float PushedBackColorMiddlePosition
		{
			get	{	return base.PushedBackColorMiddlePosition;	}
			set	{	base.PushedBackColorMiddlePosition = value;	}
		}
		#pragma warning restore	1591
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimButton() : base()
		{
			GradientMode = Sim.Controls.GradientMode.TrioVertical;
			BackColorStart = SystemColors.ControlLightLight;
			BackColorMiddle = SystemColors.Control;
			BackColorEnd = Color.FromArgb(SystemColors.Control.R - (SystemColors.Control.R - SystemColors.ControlDark.R)/2,
																																	SystemColors.Control.G - (SystemColors.Control.G - SystemColors.ControlDark.G)/2,
																																	SystemColors.Control.B - (SystemColors.Control.B - SystemColors.ControlDark.B)/2);

			RaisedBackColorStart = SystemColors.ControlLightLight;
			RaisedBackColorMiddlePosition = 1f;

			PushedBackColorStart = SystemColors.ControlDark;
			PushedBackColorMiddlePosition = 1f;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// OnKeyDown
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
			{
				_pushed = true;
				Refresh();
			}
			base.OnKeyDown(e);
		}
		/// <summary>
		/// OnKeyUp
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if(e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
			{
				_pushed = false;
				Invalidate();
				OnClick(EventArgs.Empty);
			}
		}
		/// <summary>
		/// OnClick
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e)
		{
			this.Focus();
			_pushed = false;
			Refresh();
			base.OnClick(e);
		}

		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region IButtonControl Members
		/// <summary>
		/// 
		/// </summary>
		[DefaultValue(typeof(DialogResult), "None")]
		public DialogResult DialogResult
		{
			get { return _res; }
			set { _res = value; }
		}
		//-------------------------------------------------------------------------------------
		void IButtonControl.NotifyDefault(bool value)
		{
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает событие Click
		/// </summary>
		public void PerformClick()
		{
			OnClick(EventArgs.Empty);
		}
		#endregion
						
	}
}
