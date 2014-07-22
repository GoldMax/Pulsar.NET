using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// Базовый класс кнопок
	/// </summary>
	public class SimButtonBase	: Control
	{
		private StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center,
																																																	LineAlignment = StringAlignment.Center,
																																																	Trimming = StringTrimming.EllipsisCharacter,
																																																	FormatFlags = StringFormatFlags.NoWrap
																																															};

		private GradientMode _gradientMode = GradientMode.None;
		private Color _backColorStart = SystemColors.Control;
		private Color _backColorMid = SystemColors.Control;
		private Color _backColorEnd = SystemColors.Control;
		private float _backColorMiddlePosition = 0.5f;
		private Color _raisedBackColorStart = SystemColors.Control;
		private Color _raisedBackColorMid = SystemColors.Control;
		private Color _raisedBackColorEnd = SystemColors.Control;
		private float _raisedBackColorMiddlePosition = 0.5f;
		private Color _pushedBackColorStart = SystemColors.Control;
		private Color _pushedBackColorMid = SystemColors.Control;
		private Color _pushedBackColorEnd = SystemColors.Control;
		private float _pushedBackColorMiddlePosition = 0.5f;

		private bool _showBorder = true;
		private bool _roundCorner = true;
		private Color _ibColor = SystemColors.ControlDark;
		private Color _abColor = SystemColors.ControlDarkDark;

		private Image _image = null;
		private Image _imageGray = null;
		private Image _imageRaised = null;
		private Image _imagePushed = null;
		private TextImageRelation _textImgRel = TextImageRelation.ImageBeforeText;

		private bool _noShift = false;
		private ToolTip _toolTip = new ToolTip();
		/// <summary>
		/// 
		/// </summary>
		protected bool _pushed = false;
		private int _textImageSpace = 2;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#region Background
		/// <summary>
		/// Определяет вид градиентной заливки.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет вид градиентной заливки.")]
		[DefaultValue(typeof(GradientMode), "None")]
		public virtual GradientMode GradientMode
		{
			get { return _gradientMode; }
			set	{ _gradientMode = value; Invalidate(); }
		}
		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get	{	return base.BackColor;	}
			set	{ base.BackColor = value;	Invalidate();	}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет начальный цвет градиентной заливки.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет начальный цвет градиентной заливки.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color BackColorStart
		{
			get { return _backColorStart; }
			set	{ _backColorStart = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет внутренний цвет градиентной заливки для трехцветных заливок.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет внутренний цвет градиентной заливки для трехцветных заливок")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color BackColorMiddle
		{
			get { return _backColorMid; }
			set	{ _backColorMid = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет конечный цвет градиентной заливки.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет конечный цвет градиентной заливки.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color BackColorEnd
		{
			get { return _backColorEnd; }
			set { _backColorEnd = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок.")]
		[DefaultValue(0.5f)]
		public virtual float BackColorMiddlePosition
		{
			get { return _backColorMiddlePosition; }
			set
			{
				if(value < 0)
					_backColorMiddlePosition = 0f;
				else if(value > 1f)
					_backColorMiddlePosition = 1f;
				else
					_backColorMiddlePosition = value;
				this.Invalidate();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет начальный цвет градиентной заливки при наведении курсора мыши.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет начальный цвет градиентной заливки при наведении курсора мыши.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color RaisedBackColorStart
		{
			get { return _raisedBackColorStart; }
			set { _raisedBackColorStart = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет внутренний цвет градиентной заливки для трехцветных заливок при наведении курсора мыши.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет внутренний цвет градиентной заливки для трехцветных заливок при наведении курсора мыши.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color RaisedBackColorMiddle
		{
			get { return _raisedBackColorMid; }
			set { _raisedBackColorMid = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет конечный цвет градиентной заливки при наведении курсора мыши.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет конечный цвет градиентной заливки при наведении курсора мыши.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color RaisedBackColorEnd
		{
			get { return _raisedBackColorEnd; }
			set { _raisedBackColorEnd = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок при наведении курсора мыши.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок при наведении курсора мыши.")]
		[DefaultValue(0.5f)]
		public virtual float RaisedBackColorMiddlePosition
		{
			get { return _raisedBackColorMiddlePosition; }
			set
			{
				if(value < 0)
					_raisedBackColorMiddlePosition = 0f;
				else if(value > 1f)
					_raisedBackColorMiddlePosition = 1f;
				else
					_raisedBackColorMiddlePosition = value;
				this.Invalidate();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет начальный цвет градиентной заливки при нажатии на кнопку.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет начальный цвет градиентной заливки при нажатии на кнопку.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color PushedBackColorStart
		{
			get { return _pushedBackColorStart; }
			set { _pushedBackColorStart = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет внутренний цвет градиентной заливки для трехцветных заливок при нажатии на кнопку.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет внутренний цвет градиентной заливки для трехцветных заливок при нажатии на кнопку.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color PushedBackColorMiddle
		{
			get { return _pushedBackColorMid; }
			set { _pushedBackColorMid = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет конечный цвет градиентной заливки при нажатии на кнопку.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет конечный цвет градиентной заливки при нажатии на кнопку.")]
		[DefaultValue(typeof(Color), "Control")]
		public virtual Color PushedBackColorEnd
		{
			get { return _pushedBackColorEnd; }
			set { _pushedBackColorEnd = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок при нажатии на кнопку.
		/// </summary>
		[Category("SimButtonBase Background")]
		[Description("Определяет позицию внутреннего цвета градиентной заливки для трехцветных заливок при нажатии на кнопку.")]
		[DefaultValue(0.5f)]
		public virtual float PushedBackColorMiddlePosition
		{
			get { return _pushedBackColorMiddlePosition; }
			set
			{
				if(value < 0)
					_pushedBackColorMiddlePosition = 0f;
				else if(value > 1f)
					_pushedBackColorMiddlePosition = 1f;
				else
					_pushedBackColorMiddlePosition = value;
				this.Invalidate();
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет затемнение фона при Enable = false
		/// </summary>
		[DefaultValue(false)]
		protected bool NoDarkIfDisable { get; set; }
		#endregion Background
		#region Border
		/// <summary>
		/// Определяет отображение рамки
		/// </summary>
		[Category("SimButtonBase Border")]
		[Description("Определяет отображение рамки")]
		[DefaultValue(true)]
		public virtual bool ShowBorder
		{
			get { return _showBorder; }
			set { _showBorder = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет цвет рамки без наведения.
		/// </summary>
		[Category("SimButtonBase Border")]
		[Description("Определяет цвет рамки без наведения.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public virtual Color InactiveBorderColor
		{
			get { return _ibColor; }
			set	{ _ibColor = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет цвет рамки при наведении.
		/// </summary>
		[Category("SimButtonBase Border")]
		[Description("Определяет цвет рамки при наведении.")]
		[DefaultValue(typeof(Color), "ControlDarkDark")]
		public virtual Color ActiveBorderColor
		{
			get { return _abColor; }
			set { _abColor = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет скругление углов рамки
		/// </summary>
		[Category("SimButtonBase Border")]
		[Description("Определяет скругление углов рамки")]
		[DefaultValue(true)]
		public bool RoundCorner
		{
			get { return _roundCorner; }
			set { _roundCorner = value; Invalidate(); }
		}
		
		#endregion Border
		#region Others
		/// <summary>
		/// Определяет всплывающую подсказку для кнопки
		/// </summary>
		[Category("SimButtonBase")]
		[Description("Определяет всплывающую подсказку для кнопки")]
		[DefaultValue(null)]
		public virtual string ToolTip { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Основное изображение кнопки.
		/// </summary>
		[Category("SimButtonBase")]
		[Description("Основное изображение кнопки.")]
		[DefaultValue(null)]
		public virtual Image Image
		{
			get	{ return _image; }
			set	
			{	
				_image = value;
				_imageGray = BitmapEffects.GetGrayImage(value);
				TextImageRelation = TextImageRelation; 
			}
		}
		/// <summary>
		/// Изображение кнопки при наведении курсора мыши.
		/// </summary>
		[Category("SimButtonBase")]
		[Description("Изображение кнопки при наведении курсора мыши.")]
		[DefaultValue(null)]
		public virtual Image ImageRaised
		{
			get { return _imageRaised; }
			set { _imageRaised = value;  Invalidate(); }
		}
		/// <summary>
		/// Изображение кнопки при нажатии на кнопку.
		/// </summary>
		[Category("SimButtonBase")]
		[Description("Изображение кнопки при нажатии на кнопку.")]
		[DefaultValue(null)]
		public virtual Image ImagePushed
		{
			get { return _imagePushed; }
			set { _imagePushed = value;  Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Текст на кнопке
		/// </summary>
		[Category("SimButtonBase")]
		[Description("Текст на кнопке")]
		public override string Text
		{
			get	{ return base.Text; }
			set { base.Text = value; Invalidate(); }
		}
		/// <summary>
		/// Определяет взаимоположение катринки и текста.
		/// </summary>
		[Description("Определяет взаимоположение катринки и текста.")]
		[Category("SimButtonBase")]
		[DefaultValue(typeof(TextImageRelation), "ImageBeforeText")]
		public virtual TextImageRelation TextImageRelation
		{
			get { return _textImgRel; }
			set
			{
				_textImgRel = value;
				if(Image == null)
				{
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Center;
				}
				else
					switch(_textImgRel)
					{
						case TextImageRelation.ImageAboveText:
							sf.Alignment = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Near;
							break;
						case TextImageRelation.ImageBeforeText:
							sf.Alignment = StringAlignment.Near;
							sf.LineAlignment = StringAlignment.Center;
							break;
						case TextImageRelation.Overlay:
							sf.Alignment = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Center;
							break;
						case TextImageRelation.TextAboveImage:
							sf.Alignment = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Far;
							break;
						case TextImageRelation.TextBeforeImage:
							sf.Alignment = StringAlignment.Far;
							sf.LineAlignment = StringAlignment.Center;
							break;
					}
				Invalidate();
			}
		}
		/// <summary>
		/// Определяет расстояние между катринкой и текстом.
		/// </summary>
		[Description("Определяет расстояние между катринкой и текстом")]
		[Category("SimButtonBase")]
		[DefaultValue(2)]
		public virtual int TextImageSpace
		{
			get { return _textImageSpace; }
			set { _textImageSpace = value < 0 ? 0 : value; Invalidate(); }
		}
		/// <summary>
		/// Определяет, нужно ли сдвигать изображение и текст при нажатии
		/// </summary>
		[Description("Определяет, нужно ли сдвигать изображение и текст при нажатии")]
		[Category("SimButtonBase")]
		[DefaultValue(false)]
		public virtual bool NoShiftOnPush
		{
			get { return _noShift; }
			set { _noShift = value; }
		}
		
		#endregion Others
		#region NonBrowsable
		/// <summary>
		/// DefaultSize
		/// </summary>
		protected override Size DefaultSize
		{
			get	{ return new Size(80, 25); }
		}
		/// <summary>
		/// BackgroundImage
		/// </summary>
		[Browsable(false)]
		public new Image BackgroundImage
		{
			get { return null; }
		}
		/// <summary>
		/// BackgroundImage
		/// </summary>
		[Browsable(false)]
		public new ImageLayout BackgroundImageLayout
		{
			get { return base.BackgroundImageLayout; }
		}
		/// <summary>
		/// 
		/// </summary>
		protected override bool ShowKeyboardCues
		{
			get { return false; }
		}
		#endregion NonBrowsable
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimButtonBase() : base()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
												ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
												ControlStyles.UserPaint /*| ControlStyles.Opaque */, true);
			SetStyle(ControlStyles.ContainerControl, false);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// OnPaint
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			try
			{
				Rectangle r = this.ClientRectangle;
				Graphics g = pevent.Graphics;
				if(_backColorStart == Color.Transparent && Parent != null)
				{
					g.TranslateTransform(-1*this.Location.X, -1*this.Location.Y);		//
					PaintEventArgs args = new PaintEventArgs(g, Parent.ClientRectangle);
					WinFormsUtils.CallPaint(Parent, args);
					g.TranslateTransform(this.Location.X, this.Location.Y);
				}
				else
					using(SolidBrush sb = new SolidBrush(_backColorStart))
						g.FillRectangle(sb, this.ClientRectangle);

				#region Calc
				if(ShowBorder)
				{
				 r.Width--;
				 r.Height--;
				}
				Rectangle tr = new Rectangle(r.X + this.Padding.Left + 3, r.Y + this.Padding.Top + 3,
																													r.Width - this.Padding.Horizontal - 6, r.Height - this.Padding.Vertical - 6);
				Size textSize = g.MeasureString(this.Text, this.Font).ToSize();

				GraphicsPath path = new GraphicsPath();
				if(_showBorder)
				{
					int shift = _roundCorner ? 2 : 0;
					path.AddLine(shift, 0, r.Width - shift, 0);
					path.AddLine(r.Width, shift, r.Width, r.Height - shift);
					path.AddLine(r.Width - shift, r.Height, shift, r.Height);
					path.AddLine(0, r.Height - shift, 0, shift);
					path.AddLine(shift, 0, shift, 0);
				}
				else
				{
					path.AddLine(0, 0, r.Width, 0);
					path.AddLine(r.Width, 0, r.Width, r.Height);
					path.AddLine(r.Width, r.Height, 0, r.Height);
					path.AddLine(0, r.Height, 0, 0);
				}
				bool over = this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)); 
				#endregion Calc
				#region Background
				Brush backBrush = null;
				Color cS = _backColorStart, cM = _backColorMid, cE = _backColorEnd;
				float ff = _backColorMiddlePosition;
				if(_pushed)
				{
					cS = _pushedBackColorStart;
					cM = _pushedBackColorMid;
					cE = _pushedBackColorEnd;
					ff = _pushedBackColorMiddlePosition;
				}
				else if(over)
				{
					cS = _raisedBackColorStart;
					cM = _raisedBackColorMid;
					cE = _raisedBackColorEnd;
					ff = _raisedBackColorMiddlePosition;
				}

				if(Enabled == false)
					backBrush = new SolidBrush(NoDarkIfDisable ? cS : ColorEffects.Dark(cS, 0.1f));
				else if(_gradientMode == GradientMode.None)
					backBrush = new SolidBrush(cS);
				else if((int)_gradientMode < 10)
					backBrush = new LinearGradientBrush(this.ClientRectangle, cS, cE,	(LinearGradientMode)(int)_gradientMode);
				else
				{
					backBrush = new LinearGradientBrush(this.ClientRectangle, cS, cE, (LinearGradientMode)((int)_gradientMode - 10));
					ColorBlend cb = new ColorBlend(3);
					cb.Colors = new Color[] { cS, cM, cE };
					cb.Positions = new float[] { 0.0f, ff, 1.0f };
					((LinearGradientBrush)backBrush).InterpolationColors = cb;
				}
				//if(backBrush is SolidBrush && ((SolidBrush)backBrush).Color == Color.Transparent && Parent != null)
				//{
				// PaintEventArgs args = new PaintEventArgs(g, Parent.RectangleToClient(this.RectangleToScreen(r)));
				// WinFormsUtils.CallPaint(Parent,args);
				//}
				//else
					using(backBrush)
						g.FillPath(backBrush, path);
				//g.DrawLine(Pens.Black, 0, r.Height/2, r.Width, r.Height/2);
				//g.DrawLine(Pens.Black, r.Width/2, 0, r.Width/2, r.Height);
				#endregion Background



				#region Text & Image
				if(_image != null)
				{
					Image image = _image;
					if(Enabled == false)
						image = _imageGray;
					else if(_pushed)
						image = _imagePushed ?? image;
					else if(over)
						image = _imageRaised ?? image;

					Rectangle ir = new Rectangle(0,0,image.Width, image.Height);
					switch(_textImgRel)
					{
						case TextImageRelation.ImageAboveText:
							{
								#region 
								ir.X = tr.X + tr.Width/2 - image.Width/2;
								ir.Y = tr.Y + tr.Height/2 - (image.Height + _textImageSpace + textSize.Height)/2;
								if(ir.Y < tr.Y)
									ir.Y = tr.Y;
								tr.Height -= ir.Y - tr.Y + ir.Height + _textImageSpace;
								tr.Y = ir.Y + ir.Height + _textImageSpace; 
								#endregion 
							} break;
						case TextImageRelation.ImageBeforeText:
							{
								#region 
								ir.X = tr.X + tr.Width/2 - (image.Width + _textImageSpace + textSize.Width)/2;
								if(ir.X < tr.X)
									ir.X = tr.X;
								ir.Y = tr.Y + tr.Height/2 - image.Height/2;
								tr.Width -= ir.X - tr.X + ir.Width + _textImageSpace;
								tr.X = ir.X + ir.Width + _textImageSpace;
								//ir.Y += ir.Y % 2 > 0 ? 1 : 0;
								//tr.Y += tr.Y % 2 > 0 ? 2 : 1;
								#endregion 
							} break;
						case TextImageRelation.Overlay:
							#region 
							ir.X = tr.X + tr.Width/2 - image.Width/2;
							ir.Y = tr.Y + tr.Height/2 - image.Height/2; 
							#endregion 
							break;
						case TextImageRelation.TextAboveImage:
							{
								#region 
								ir.X = tr.X + tr.Width/2 - image.Width/2;
								ir.Y = tr.Y + tr.Height/2 + (image.Height + _textImageSpace)/2;
								if(ir.Y + image.Height > tr.Bottom)
									ir.Y = tr.Bottom - image.Height;
								tr.Height -= tr.Height - ir.Y + _textImageSpace + tr.Y; 
								#endregion 
							} break;
						case TextImageRelation.TextBeforeImage:
							{
								#region MyRegion
								ir.X = tr.X + tr.Width/2 + (image.Width + _textImageSpace + textSize.Width)/2 - image.Width;
								ir.X ++;
								if(ir.X + image.Width > tr.X + tr.Width)
									ir.X = tr.X + tr.Width - image.Width;
								ir.Y = tr.Y + tr.Height/2 - image.Height/2;
								//tr.Width -= tr.Right - ir.X + _textImageSpace;
								tr.Width = ir.X - _textImageSpace - tr.X;
								//ir.Y += ir.Y % 2 > 0 ? 1 : 0;
								//tr.Y += tr.Y % 2 > 0 ? 2 : 1;
								#endregion MyRegion
							} break;
					}
					if(_noShift == false && _pushed)
						ir.Offset(1, 1);
					g.DrawImageUnscaledAndClipped(image, ir);
					//g.DrawRectangle(Pens.Green, ir);
				}

				if(_noShift == false && _pushed)
				{
					tr.Offset(1, 1);
					//tr.Inflate(-1, -1);
				}
				using(SolidBrush b = new SolidBrush(Enabled ? this.ForeColor : SystemColors.GrayText))
					g.DrawString(this.Text, this.Font, b, tr, sf);
				//g.DrawRectangle(Pens.Red, tr);
				#endregion Text & Image

				#region Border & Focus
				if(_showBorder)
				{
					g.SmoothingMode = SmoothingMode.HighQuality;
					if(Enabled == false)
						using(Pen p = new Pen(_ibColor))
							g.DrawPath(p, path);
					else if(over)
						using(Pen p = new Pen(_abColor))
							g.DrawPath(p, path);
					else
						using(Pen p = new Pen(_ibColor))
							g.DrawPath(p, path);
				}
				if(this.Focused && this.ShowFocusCues)
				{
					r.X +=  2;
					r.Y +=  2;
					r.Width -= 3;
					r.Height -= 3;
					ControlPaint.DrawFocusRectangle(g, r);
				}
				#endregion Border & Focus
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnMouseEnter(EventArgs e)
		{
			Refresh();
			base.OnMouseEnter(e);
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnMouseDown(MouseEventArgs e)
		{
			_pushed = true;
			Refresh();
			base.OnMouseDown(e);
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool wasPushed = _pushed;
			_pushed = false;
			Refresh();
			//if(base.ClientRectangle.Contains(e.Location) && wasPushed)
			// OnClick(new EventArgs());
			base.OnMouseUp(e);
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnMouseLeave(EventArgs e)
		{
			_pushed = false;
			Refresh();
			base.OnMouseLeave(e);
			if(_toolTip != null)
				_toolTip.Hide(this);
			_toolTip = null;
		}
		//-------------------------------------------------------------------------------------
		///
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(this.ClientRectangle.Contains(e.Location) == false)
			{
				_pushed = false;
				Refresh();
			}
			base.OnMouseMove(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseHover
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			if(String.IsNullOrEmpty(ToolTip) == false && _toolTip == null)
			{
				Point p = this.PointToClient(Control.MousePosition);
				_toolTip = new ToolTip();
				_toolTip.Show(ToolTip, this, p.X + 10, p.Y + 10, 5000);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			Refresh();
			base.OnGotFocus(e);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			Refresh();
			base.OnLostFocus(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает событие Click
		/// </summary>
		public void RaiseClick()
		{
			this.OnClick(EventArgs.Empty);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
						
	}
}
