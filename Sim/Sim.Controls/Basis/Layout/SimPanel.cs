using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;
using System.Reflection;

using Sim.Controls.WinAPI;

namespace Sim.Controls
{
	#region << public enum Border3DKind : byte >>
	/// <summary>
	/// Перечисление видов 3D рамок.
	/// </summary>
	public enum Border3DKind : byte
	{
		/// <summary>
		/// Вдавленная
		/// </summary>
		Sunken,
		/// <summary>
		/// Слабо вдавленная
		/// </summary>
		SunkenFlat,
		/// <summary>
		/// Выпуклая
		/// </summary>
		Raised,
		/// <summary>
		/// Слабо выпуклая
		/// </summary>
		RaisedFlat,
		/// <summary>
		/// Вдавленный контур
		/// </summary>
		Etched,
		/// <summary>
		/// Выпуклый контур
		/// </summary>
		Bump
	}
	#endregion << public enum Border3DKind : byte >>
	//**************************************************************************************
	/// <summary>
	/// Класс панели.
	/// </summary>
	public class SimPanel : Panel
	{
		//private static FieldWrap styles = null;

		private Color borderColor = SystemColors.ControlDark;
		private BorderStyle borderStyle = BorderStyle.None;
		private Padding borderWidth = new Padding(1);
		private Border3DKind b3Dstyle = Border3DKind.Sunken;
		private bool useVisualStyleBorderColor = true;
		private Color backColor2 = SystemColors.ControlDark;
		private Color backColorMid = SystemColors.ControlDark;
		private GradientMode gradientMode = GradientMode.None;
		private float backColorMiddlePosition = 0.5f;
		private bool transparent = false;

		const int HTTRANSPARENT = -1;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет цвет рамки при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет цвет рамки при BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BorderColor
		{
			get { return borderColor; }
			set 
			{ 
				borderColor = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет вид рамки.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет вид рамки.")]
		[DefaultValue(typeof(BorderStyle),"None")]
		public new BorderStyle BorderStyle
		{
			get { return borderStyle; }
			set 
			{
				if(borderStyle != value)
				{
					borderStyle = value; 
					this.RefreshBorder();
				}  
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет ширину рамки при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет ширину рамки при BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Padding), "1, 1, 1, 1")]
		public Padding BorderWidth
		{
			get { return borderWidth; }
			set 
			{
				borderWidth = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет стиль рисования рамки при BorderStyle = Fixed3D.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет стиль рисования рамки при BorderStyle = Fixed3D.")]
		[DefaultValue(typeof(Border3DKind), "Sunken")]
		public Border3DKind Border3DKind
		{
			get { return b3Dstyle; }
			set 
			{ 
				b3Dstyle = value;
				RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будут ли использованы цвета схемы.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет, будут ли использованы цвета схемы.")]
		[DefaultValue(true)]
		public bool UseVisualStyleBorderColor
		{
			get { return useVisualStyleBorderColor; }
			set 
			{ 
				useVisualStyleBorderColor = value;
				RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет второй цвет градиентной заливки.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет второй цвет градиентной заливки.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BackColor2
		{
			get { return backColor2; }
			set 
			{ 
				backColor2 = value; 
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет внутренний цвет градиентной заливки для трехцветных заливок.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет внутренний цвет градиентной заливки для трехцветных заливок")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BackColorMiddle
		{
			get { return backColorMid; }
			set 
			{ 
				backColorMid = value; 
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок.")]
		[DefaultValue(0.5f)]
		public float BackColorMiddlePosition
		{
			get { return backColorMiddlePosition; }
			set 
			{
				if(value < 0)
					backColorMiddlePosition = 0f;
				else if(value > 1f)
					backColorMiddlePosition = 1f;
				else  
					backColorMiddlePosition = value; 
				this.Refresh();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет вид градиентной заливки.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет вид градиентной заливки.")]
		[DefaultValue(typeof(GradientMode), "None")]
		public GradientMode GradientMode
		{
			get { return gradientMode; }
			set 
			{
				gradientMode = value; 
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет прозрачность контрола для событий.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет прозрачность контрола для событий.")]
		[DefaultValue(false)]
		public bool EventsTransparent
		{
			get { return transparent; }
			set { transparent = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Margin
		/// </summary>
		[DefaultValue(typeof(Padding), "0, 0, 0, 0")]
		public new Padding Margin
		{
			get { return base.Margin; }
			set { base.Margin = value; this.RefreshBorder(); }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimPanel() : base ()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			base.Margin = new Padding(0);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Overrides Methods >>
		/// <summary>
		/// WndProc
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if(transparent && DesignMode == false && m.Msg == WM.NCHITTEST)
			{
				m.Result = (IntPtr)HTTRANSPARENT;
				return;
			} 
			switch((uint)m.Msg)
			{
				case WM.MOVE : 
					break;
				case WM.NCCALCSIZE :
					//if(m.WParam == IntPtr.Zero)
					// return;
					m.Result = new IntPtr(0x0400); // | 0x0300
					NCCALCSIZE_PARAMS p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));

					p.rgc[0].left += Margin.Left;
					p.rgc[0].right -= Margin.Right;
					p.rgc[0].top += Margin.Top;
					p.rgc[0].bottom -= Margin.Bottom;

					if(borderStyle == BorderStyle.FixedSingle)
					{ 
						p.rgc[0].left += borderWidth.Left;
						p.rgc[0].right -= borderWidth.Right;
						p.rgc[0].top += borderWidth.Top;
						p.rgc[0].bottom -= borderWidth.Bottom;
					}
					else if(borderStyle == BorderStyle.Fixed3D)
					{
						int w = 2;
						if(b3Dstyle == Border3DKind.RaisedFlat || b3Dstyle == Border3DKind.SunkenFlat)
							w = 1;
						p.rgc[0].left += w;
						p.rgc[0].right -= w;
						p.rgc[0].top += w;
						p.rgc[0].bottom -= w;
					}  
					Marshal.StructureToPtr(p, m.LParam, true);
					break;
				case WM.NCPAINT :
					IntPtr hdc = IntPtr.Zero;
					try
					{
						//hdc = APIWrappers.GetDCEx(this.Handle, (IntPtr)m.WParam,
						//            (int)(GetDCExFlags.DCX_WINDOW | GetDCExFlags.DCX_PARENTCLIP ));
						base.WndProc(ref m);
						hdc = APIWrappers.GetWindowDC(this.Handle);
						if(hdc != IntPtr.Zero)
							using(Graphics g = Graphics.FromHdc(hdc))
								OnNonClientPaint(g);
					}
					finally
					{
						if(hdc != IntPtr.Zero)
							APIWrappers.ReleaseDC(this.Handle, hdc);
					} 
					return;
			}
			base.WndProc(ref m);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnPaint
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{  
			if(gradientMode == GradientMode.None)
			{
				//if(BackgroundImage != null)
					using(SolidBrush b = new SolidBrush(this.BackColor))
						e.Graphics.FillRectangle(b, e.ClipRectangle);
				//else
				//    base.OnPaint(e);
			}
			else 
			{ 
				if((int)gradientMode < 10)
				{
					using(LinearGradientBrush b = new LinearGradientBrush(this.ClientRectangle, this.BackColor, this.BackColor2,
																																																											(LinearGradientMode)(int)gradientMode))
						e.Graphics.FillRectangle(b, this.ClientRectangle);
				}
				else
				{
					LinearGradientBrush b = new LinearGradientBrush(this.ClientRectangle, this.BackColor, this.BackColor2,
																																																											(LinearGradientMode)((int)gradientMode - 10));
					ColorBlend cb = new ColorBlend(3);
					cb.Colors = new Color[] { this.BackColor, this.backColorMid, this.BackColor2 };
					cb.Positions = new float[] { 0.0f, backColorMiddlePosition, 1.0f };
					b.InterpolationColors = cb;
					e.Graphics.FillRectangle(b, e.ClipRectangle);
					b.Dispose();
				}
			} 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.Refresh();
		}
		//-------------------------------------------------------------------------------------
		public override string ToString()
		{
			return "["+ Name + "]-" + base.ToString();
		}
		#endregion << Overrides Methods >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Осуществляет рисование в неклиенской области.
		/// </summary>
		/// <param name="g"></param>
		protected void OnNonClientPaint(Graphics g)
		{
			Rectangle r = Rectangle.Round(g.VisibleClipBounds);
			Padding pd;
			if(borderStyle == System.Windows.Forms.BorderStyle.None)
				pd = Margin;
			else if(borderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
				pd = Padding.Add(Margin, borderWidth);
			else if(b3Dstyle == Border3DKind.RaisedFlat || b3Dstyle == Border3DKind.SunkenFlat)
				pd = Padding.Add(Margin, new Padding(1));
			else
				pd = Padding.Add(Margin, new Padding(2));

			if(pd.All != 0)
				if(Parent == null || Parent is SimPanel == false)
				{
					Color c = Parent == null ? BackColor : Parent.BackColor;
					if(c == Color.Transparent)
					{
						Control p = Parent;
						while(p != null)
						{
							c = p.BackColor;
							if(c != Color.Transparent)
								break;
							p = p.Parent;
						} 
						if(c == Color.Transparent)
							c = SystemColors.Control;
					} 
					using(SolidBrush b = new SolidBrush(c))
					{
						g.FillRectangle(b, 0, 0, r.Width, pd.Top);
						g.FillRectangle(b, r.Width - pd.Right, 0, pd.Right, r.Height);
						g.FillRectangle(b, 0, r.Height - pd.Bottom, r.Width, pd.Bottom);
						g.FillRectangle(b, 0, 0, pd.Left, r.Height);
					}
				}
				else
				{
					Rectangle rr = new Rectangle(0, 0, r.Width, pd.Top);
					if(rr.Width > 0 && rr.Height > 0)
						WinFormsUtils.CallPaint(Parent, new PaintEventArgs(g, rr) );

					rr = new Rectangle(r.Width - pd.Right, 0, pd.Right, r.Height);
					if(rr.Width > 0 && rr.Height > 0)
						WinFormsUtils.CallPaint(Parent, new PaintEventArgs(g, rr));

					rr = new Rectangle(0, r.Height - pd.Bottom, r.Width, pd.Bottom);
					if(rr.Width > 0 && rr.Height > 0)
						WinFormsUtils.CallPaint(Parent, new PaintEventArgs(g, rr));

					rr = new Rectangle(0, 0, pd.Left, r.Height);
					if(rr.Width > 0 && rr.Height > 0)
						WinFormsUtils.CallPaint(Parent, new PaintEventArgs(g, rr));
				}


			if(borderStyle != System.Windows.Forms.BorderStyle.None)
			{
				r.X += Margin.Left;
				r.Width -= Margin.Horizontal;
				r.Y += Margin.Top;
				r.Height -= Margin.Vertical;
			}

			if(borderStyle == BorderStyle.FixedSingle)
			{
				bool useVS = VisualStyleInformation.IsEnabledByUser && 
																	VisualStyleInformation.IsSupportedByOS &&
																	Application.RenderWithVisualStyles &&
																	useVisualStyleBorderColor;


				SolidBrush b;
				if(useVS)
					b = new SolidBrush(VisualStyleInformation.TextControlBorder);
				else
					b = new SolidBrush(this.BorderColor);
				using(b)  //
				{
					//g.FillRectangle(Brushes.Red, 0, 0, r.Width, r.Height);
					g.FillRectangle(b, r.X, r.Y, r.Width, borderWidth.Top);
					g.FillRectangle(b, r.X + r.Width - borderWidth.Right, r.Y, borderWidth.Right, r.Height);
					g.FillRectangle(b, r.X, r.Y + r.Height - borderWidth.Bottom, r.Width, borderWidth.Bottom);
					g.FillRectangle(b, r.X, r.Y, borderWidth.Left, r.Height);
				}
			}
			else if(borderStyle == BorderStyle.Fixed3D)
			{
				switch(b3Dstyle)
				{
					case Border3DKind.Sunken:
						g.DrawLine(SystemPens.ControlDarkDark, r.X+1, r.Y+1, r.Width, r.Y+1);  // Top
						g.DrawLine(SystemPens.ControlDarkDark, r.X+1, r.Y+1, r.X+1, r.Height); // Left
						g.DrawLine(SystemPens.ControlLight, r.X+1, r.Y + r.Height - 2, r.Width, r.Y + r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlLight, r.X + r.Width - 2, r.Y+1, r.X + r.Width - 2, r.Height - 1); // Right
						goto case Border3DKind.SunkenFlat;
					case Border3DKind.SunkenFlat:
						g.DrawLine(SystemPens.ControlDark, r.X, r.Y, r.Width, r.Y);  // Top
						g.DrawLine(SystemPens.ControlDark, r.X, r.Y, r.X, r.Y + r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlLightLight, r.X, r.Y + r.Height - 1, r.Width + 1, r.Y + r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlLightLight, r.X + r.Width - 1, r.Y, r.X + r.Width - 1, r.Height); // Right
						break;
					case Border3DKind.Raised:
						g.DrawLine(SystemPens.ControlLight, r.X, r.Y, r.X + r.Width - 2, r.Y);  // Top
						g.DrawLine(SystemPens.ControlLight, r.X, r.Y, r.X, r.Y + r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlDarkDark, r.X, r.Y + r.Height - 1, r.X + r.Width - 1, r.Y + r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlDarkDark, r.X + r.Width - 1, r.Y, r.X + r.Width - 1, r.Y + r.Height - 1); // Right
						g.DrawLine(SystemPens.ControlLightLight, r.X + 1, r.Y + 1, r.X + r.Width - 3, r.Y + 1);  // Top
						g.DrawLine(SystemPens.ControlLightLight, r.X + 1, r.Y + 1, r.X + 1, r.Y + r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlDark, r.X + 1, r.Y + r.Height - 2, r.X + r.Width - 2, r.Y + r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.X + r.Width - 2, r.Y + 1, r.X + r.Width - 2, r.Y + r.Height - 2); // Right
						break;
					case Border3DKind.RaisedFlat:
						g.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X + r.Width - 2, r.Y);  // Top
						g.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X, r.Y + r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlDark, r.X, r.Y + r.Height - 1, r.X + r.Width - 1,r.Y +  r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.X + r.Width - 1, r.Y, r.X + r.Width - 1, r.Y + r.Height - 1); // Right
						break;
					case Border3DKind.Etched:
						g.DrawLine(SystemPens.ControlDark,       r.X, r.Y, r.X + r.Width - 2, r.Y);  // Top
						g.DrawLine(SystemPens.ControlDark,       r.X, r.Y, r.X, r.Y + r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlLightLight, r.X, r.Y + r.Height - 1, r.X + r.Width - 1, r.Y + r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlLightLight, r.X + r.Width - 1, r.Y, r.X + r.Width - 1, r.Y + r.Height - 1); // Right
						g.DrawLine(SystemPens.ControlLightLight, r.X + 1, r.Y + 1, r.X + r.Width - 3, r.Y + 1);  // Top
						g.DrawLine(SystemPens.ControlLightLight, r.X + 1, r.Y + 1, r.X + 1, r.Y + r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlDark, r.X + 1, r.Y + r.Height - 2, r.X + r.Width - 2, r.Y + r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.X + r.Width - 2, r.Y + 1, r.X + r.Width - 2, r.Y + r.Height - 2); // Right
						break;
					case Border3DKind.Bump:
						g.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X + r.Width - 2, r.Y);  // Top
						g.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X, r.Y + r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlDark,       r.X, r.Y + r.Height - 1, r.X + r.Width - 1, r.Y + r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlDark,       r.X + r.Width - 1, r.Y, r.X + r.Width - 1, r.Y + r.Height - 1); // Right
						g.DrawLine(SystemPens.ControlDark,       r.X + 1, r.Y + 1, r.X + r.Width - 3, r.Y + 1);  // Top
						g.DrawLine(SystemPens.ControlDark,       r.X + 1, r.Y + 1, r.X + 1, r.Y + r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlLightLight, r.X + 1, r.Y + r.Height - 2, r.X + r.Width - 2, r.Y + r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlLightLight, r.X + r.Width - 2, r.Y + 1, r.X + r.Width - 2, r.Y + r.Height - 2); // Right
						break;
				}
			}
			g.Flush(); 
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обновляет рисование рамки.
		/// </summary>
		public void RefreshBorder()
		{
			APIWrappers.SetWindowPos(new HandleRef(this, this.Handle), new HandleRef(), 0,0,0,0,
				0x0001 | 0x0002 | 0x0004 | 0x0010 | 0x0020 | 0x0200); 
			Application.DoEvents(); 
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
}
