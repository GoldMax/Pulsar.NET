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
	[DefaultProperty("Image")]
	[DefaultEvent("Click")]
	public class SimPopupButton : SimButtonBase
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#pragma warning disable	1591
		protected override Size DefaultSize
		{
			get { return new Size(20, 20); }
		}
		[DefaultValue("")]
		public override string Text
		{
			get {	return base.Text == Name ? "" : base.Text;	}
			set	{ base.Text = value; }
		}
		[DefaultValue(typeof(TextImageRelation), "Overlay")]
		public override TextImageRelation TextImageRelation
		{
			get	{	return base.TextImageRelation;		}
			set	{	base.TextImageRelation = value;	}
		}
		[DefaultValue(true)]
		public override bool NoShiftOnPush
		{
			get {	return base.NoShiftOnPush;	}
			set	{	base.NoShiftOnPush = value;	}
		}
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColorStart
		{
			get { return base.BackColorStart; }
			set { base.BackColorStart = value; }
		}
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color RaisedBackColorStart
		{
			get { return base.RaisedBackColorStart; }
			set { base.RaisedBackColorStart = value; }
		}
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color InactiveBorderColor
		{
			get { return base.InactiveBorderColor; }
			set { base.InactiveBorderColor = value; }
		}
		[DefaultValue(typeof(Color), "ControlDark")]
		public override Color ActiveBorderColor
		{
			get { return base.ActiveBorderColor; }
			set { base.ActiveBorderColor = value; }
		}
		[DefaultValue(typeof(Color), "ControlDark")]
		public override Color PushedBackColorStart
		{
			get { return base.PushedBackColorStart; }
			set { base.PushedBackColorStart = value; }
		}
		#pragma warning restore	1591
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimPopupButton()	: base()
		{
			BackColorStart = RaisedBackColorStart = Color.Transparent;
			InactiveBorderColor = Color.Transparent;
			ActiveBorderColor = SystemColors.ControlDark;
			PushedBackColorStart = SystemColors.ControlDark;
			base.TextImageRelation = TextImageRelation.Overlay;
			TabStop = false;
			NoShiftOnPush = true;
			base.NoDarkIfDisable = true;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		#endregion << Methods >>
	}
}
