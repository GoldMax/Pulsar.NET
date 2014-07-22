using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола разделительной линии
 /// </summary>
	public class SimLine : Panel
	{
		private static StringFormat sf = new StringFormat()
		{
			LineAlignment = StringAlignment.Center
		};

		private int _lh = 1;
		private Color _lc = SystemColors.ControlDark;
		private float _gradLeftPos = 0.1f;
		private float _gradRightPos = 0.9f;
		private int _textIndent = 40;
		private int _textBeforeSpace = 5;
		private int _textAfterSpace = 5;

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Толщина линии
		/// </summary>
		[Description("Толщина линии")]
		[DefaultValue(1)]
		public int LineHeight
		{
			get { return _lh; }
			set { _lh = value; Invalidate(); }
		}
		/// <summary>
		/// Цвет линии
		/// </summary>
		[Description("Цвет линии")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color LineColor
		{
			get { return _lc; }
			set { _lc = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет позицию левого внутреннего цвета градиентной заливки.
		/// </summary>
		[Description("Определяет позицию левого внутреннего цвета градиентной заливки.")]
		[DefaultValue(0.1f)]
		public float GradientLeftPosition
		{
			get { return _gradLeftPos; }
			set
			{
				if(value < 0)
					_gradLeftPos = 0f;
				else if(value > 1f)
					_gradLeftPos = 1f;
				else
					_gradLeftPos = value;
				this.Refresh();
			}
		}
		/// <summary>
		/// Определяет позицию правого внутреннего цвета градиентной заливки.
		/// </summary>
		[Description("Определяет позицию правого внутреннего цвета градиентной заливки.")]
		[DefaultValue(0.9f)]
		public float GradientRightPosition
		{
			get { return _gradRightPos; }
			set
			{
				if(value < 0)
					_gradRightPos = 0f;
				else if(value > 1f)
					_gradRightPos = 1f;
				else
					_gradRightPos = value;
				this.Refresh();
			}
		}
		[Browsable(true)]
		public override string Text
		{
			get	{ return base.Text; }
			set { base.Text = value; Invalidate();	}
		}
		/// <summary>
		/// Определяет отступ текста слева.
		/// </summary>
		[Description("Определяет отступ текста слева.")]
		[DefaultValue(40)]
		public int TextIndent
		{
			get { return _textIndent; }
			set { _textIndent = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет ширину пустого места до текста.
		/// </summary>
		[Description("Определяет ширину пустого места до текста.")]
		[DefaultValue(5)]
		public int TextBeforeSpace
		{
			get { return _textBeforeSpace; }
			set { _textBeforeSpace = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет ширину пустого места после текста.
		/// </summary>
		[Description("Определяет ширину пустого места после текста.")]
		[DefaultValue(5)]
		public int TextAfterSpace
		{
			get { return _textAfterSpace; }
			set { _textAfterSpace = value; Invalidate(); }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimLine() : base()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Size textSize = Size.Empty;

			Rectangle r = ClientRectangle;
			r.Width -= 1;
			r.Y = r.Height/2 - _lh/2;
			r.Height = _lh;

			Region	reg = new Region(r);
			
			if(Text != null && Text.Length > 0)
			{
			 textSize =  e.Graphics.MeasureString(Text, this.Font).ToSize();
				textSize.Width ++;
				reg.Exclude(new Rectangle(_textIndent, r.Y, textSize.Width + _textAfterSpace + _textBeforeSpace, _lh)); 
			}

			//e.Graphics.FillRegion(Brushes.Aqua, reg);

			using(LinearGradientBrush b = new LinearGradientBrush(r, this.BackColor, _lc, LinearGradientMode.Horizontal))
			{
			 ColorBlend cb = new ColorBlend(3);
			 cb.Colors = new Color[] { this.BackColor, this._lc, this._lc, this.BackColor };
			 cb.Positions = new float[] { 0.0f, _gradLeftPos, _gradRightPos,  1.0f };
			 b.InterpolationColors = cb;
			 e.Graphics.FillRegion(b, reg);
			}
			
			if(Text == null || Text.Length == 0)
			 return;

			r.X = _textIndent + _textBeforeSpace;
			r.Y = ClientRectangle.Height/2 - textSize.Height/2; 
			r.Width = textSize.Width;
			r.Height = textSize.Height;
			using(SolidBrush b = new SolidBrush(Enabled ? ForeColor : SystemColors.GrayText))
			 e.Graphics.DrawString(Text, this.Font, b, r, sf);

		//	e.Graphics.DrawRectangle(Pens.Red, r);
		}
	}
}
