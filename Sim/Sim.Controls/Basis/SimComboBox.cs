using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Forms.VisualStyles;

using Sim.Controls.WinAPI;
using Pulsar;

namespace Sim.Controls
{
	/// <summary>
	/// Класс комбобокса
	/// </summary>
	[Designer(typeof(SimComboBoxDesigner))]
	[ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
	[DefaultEvent("UISelectedItemChanged")]
	public class SimComboBox : Control
	{
		private StringFormat sf = new StringFormat();
		private Color bColor = SystemColors.ControlDark;
		private BorderStyle bStyle = BorderStyle.FixedSingle;
		private Border3DKind b3Dstyle = Border3DKind.Sunken;
		private bool useVisualStyle = true;
		private bool isResizeble = false;
		private bool disableButton = false;
		private int _selectedIndex = -1;

		private IList items = new List<object>();
		private ToolStripDropDown dropDown = null;
		private SimSelectList nativePopup = null;
		private object selectedItem = null;
		private ToolTip toolTip = null;
		private bool sorted = true;

		private bool overBtn = false;
		private bool isDropDownOpened = false;
		private int btnWidth = 16;
		private Rectangle btnRect;

		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		#region public event CancelEventHandler DropDownOpening;
		/// <summary>
		/// Событие, вызываемое при открытии выпадающего списка.
		/// </summary>
		public event CancelEventHandler DropDownOpening;
		/// <summary>
		/// Вызывает событие DropDownOpening
		/// </summary>
		protected virtual bool OnDropDownOpening()
		{
			if (DropDownOpening != null)
			{
				CancelEventArgs c = new CancelEventArgs();
				DropDownOpening(this, c);
				return c.Cancel;
			}
			return false;
		}
		#endregion public event CancelEventHandler DropDownOpening;
		#region public event EventHandler DropDownOpened;
		/// <summary>
		/// Событие, вызываемое после открытии выпадающего списка.
		/// </summary>
		public event EventHandler DropDownOpened;
		/// <summary>
		/// Вызывает событие DropDownOpened
		/// </summary>
		protected virtual void OnDropDownOpened()
		{
			if (DropDownOpened != null)
				DropDownOpened(this, EventArgs.Empty);
		}
		#endregion public event EventHandler DropDownOpened;
		#region public event EventHandler DropDownClosed;
		/// <summary>
		/// Событие, вызываемое после закрытия выпадающего списка.
		/// </summary>
		public event EventHandler DropDownClosed;
		/// <summary>
		/// Вызывает событие DropDownClosed
		/// </summary>
		protected virtual void OnDropDownClosed()
		{
			if (DropDownClosed != null)
				DropDownClosed(this, EventArgs.Empty);
		}
		#endregion public event EventHandler DropDownClosed;
		#region public event EventHandler<object,object> SelectedItemChanged
		/// <summary>
		/// Событие, вызываемое при изменении свойства SelectedItem.
		/// </summary>
		public event EventHandler SelectedItemChanged;
		/// <summary>
		/// Вызывает событие SelectedItemChanged
		/// </summary>
		protected virtual void OnSelectedItemChanged()
		{
			if (SelectedItemChanged != null)
				SelectedItemChanged(this, EventArgs.Empty);
		}
		#endregion public event EventHandler<object,object> SelectedItemChanged
		#region public event EventHandler<object,object> UISelectedItemChanged
		/// <summary>
		/// Событие, вызываемое при выборе элемента выпадающего списка.
		/// </summary>
		public event EventHandler UISelectedItemChanged;
		/// <summary>
		/// Вызывает событие UISelectedItemChanged
		/// </summary>
		protected virtual void OnUISelectedItemChanged()
		{
			if (UISelectedItemChanged != null)
				UISelectedItemChanged(this, EventArgs.Empty);
		}
		#endregion public event EventHandler<object,object> UISelectedItemChanged
		#region public event EventHandler<object, object> SelectedIndexChanged

		/// <summary>
		/// Событие, вызываемое при изменение свойства Selected Index
		/// </summary>
		public event EventHandler SelectedIndexChanged;

		/// <summary>
		/// Вызывает событие SelectedIndexChanged
		/// </summary>
		protected virtual void OnSelectedIndexChanged()
		{
			if (SelectedIndexChanged != null)
				SelectedIndexChanged(this, EventArgs.Empty);
		}


		#endregion public event EventHandler<object, object> SelectedIndexChanged
		#endregion << Events >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Цвет фона
		/// </summary>
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get { return base.BackgroundImageLayout; }
			set { base.BackgroundImageLayout = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет стиль рисования рамки.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет стиль рамки")]
		[DefaultValue(typeof(BorderStyle), "FixedSingle")]
		public BorderStyle BorderStyle
		{
			get { return bStyle; }
			set
			{
				if (bStyle != value)
				{
					bStyle = value;
					this.RefreshBorder();
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет стиль рисования рамки при BorderStyle = Fixed3D.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет стиль рисования рамки при BorderStyle = Fixed3D.")]
		[DefaultValue(typeof(Border3DKind), "Sunken")]
		public Border3DKind Border3DStyle
		{
			get { return b3Dstyle; }
			set
			{
				b3Dstyle = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет рамки при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет цвет рамки при BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BorderColor
		{
			get { return bColor; }
			set
			{
				bColor = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли использован цвет рамки и кнопки схемы при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет, будет ли использован цвет рамки и кнопки схемы при BorderStyle = FixedSingle.")]
		[DefaultValue(true)]
		public bool UseVisualStyle
		{
			get { return useVisualStyle; }
			set
			{
				useVisualStyle = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет возможность изменения размера выпадающего списка.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет возможность изменения размера выпадающего списка.")]
		[DefaultValue(false)]
		public bool IsResizeble
		{
			get { return isResizeble; }
			set { isResizeble = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет активность кнопки.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет активность кнопки выпадающего списка.")]
		[DefaultValue(false)]
		public bool DisableButton
		{
			get { return disableButton; }
			set { disableButton = value; Refresh(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли список отображен сортированным.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("Определяет, будет ли список отображен сортированным.")]
		public bool Sorted
		{
			get { return sorted; }
			set { sorted = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет элементы выпадающего списка.
		/// </summary>
		[Category("Data")]
		[Description("Определяет элементы выпадающего списка.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList Items
		{
			get { return items; }
			set
			{
				items = value;
				if (selectedItem != null)
					if (items == null || items.Contains(selectedItem) == false)
						SelectedItem = null;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет пользовательский выпадающий список.
		/// </summary>
		[Category("Data")]
		[Description("Определяет пользовательский выпадающий список.")]
		[DefaultValue(null)]
		public ToolStripDropDown DropDown
		{
			get { return dropDown; }
			set { dropDown = value; }
		}

		/// <summary>
		/// Определяет производить ли автоматическое выставление ширины выпадающего списка
		/// </summary>
		[DefaultValue(false)]
		[Category("Appearance")]
		[Description("Определяет производить ли автоматическое выставление ширины выпадающего списка")]
		public bool IsDropDownAutoSize
		{
			get;
			set;
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Non browsable Properties >>
		/// <summary>
		/// Preferred Height
		/// </summary>
		[Browsable(false)]
		public int PreferredHeight
		{
			get { return 21; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Height
		/// </summary>
		public new int Height
		{
			get { return PreferredHeight; }
			set { }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Size
		/// </summary>
		public new Size Size
		{
			get { return base.Size; }
			set
			{
				if (value.Height != PreferredHeight)
					value.Height = PreferredHeight;
				base.Size = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, открыт ли выпадающий список.
		/// </summary>
		[Browsable(false)]
		public bool IsDropDownOpened
		{
			get { return isDropDownOpened; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет выбранный элемент.
		/// </summary>
		[Browsable(false)]
		public virtual object SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				OnSelectedItemChanged();
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет выбранный элемент по позиции в списке.
		/// </summary>
		[Browsable(false)]
		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				if (_selectedIndex != value)
				{
					int count = 0;
					if (items != null)
						count = items.Count;
					if (value < -1 || value >= count)
						throw new ArgumentOutOfRangeException(
						 "SelectedIndex", string.Format("Значиние [{0}] выходит за границы.", value));
					_selectedIndex = value;
					OnSelectedIndexChanged();
					SelectedItem = items[value];
				}
			}
		}
		#endregion <<  Non browsable Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimComboBox()
			: base()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			sf.Alignment = StringAlignment.Near;
			sf.FormatFlags = StringFormatFlags.NoWrap;
			sf.LineAlignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.EllipsisCharacter;

			base.BackColor = SystemColors.Window;
			base.Height = PreferredHeight;
			base.Width = 120;
			nativePopup = new SimSelectList(items);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Override Methods >>
		/// <summary>
		/// GetPreferredSize
		/// </summary>
		/// <param name="proposedSize"></param>
		/// <returns></returns>
		public override Size GetPreferredSize(Size proposedSize)
		{
			return new Size(proposedSize.Width, PreferredHeight);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// WndProc
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			switch ((uint)m.Msg)
			{
				case WM.NCCALCSIZE:
					if (bStyle == BorderStyle.FixedSingle)
					{
						NCCALCSIZE_PARAMS p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));
						p.rgc[0].left += 1;
						p.rgc[0].right -= 1;
						p.rgc[0].top += 1;
						p.rgc[0].bottom -= 1;
						Marshal.StructureToPtr(p, m.LParam, true);
					}
					else if (bStyle == BorderStyle.Fixed3D)
					{
						int w = 2;
						if (b3Dstyle == Border3DKind.RaisedFlat || b3Dstyle == Border3DKind.SunkenFlat)
							w = 1;
						NCCALCSIZE_PARAMS p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));
						p.rgc[0].left += w;
						p.rgc[0].right -= w;
						p.rgc[0].top += w;
						p.rgc[0].bottom -= w;
						Marshal.StructureToPtr(p, m.LParam, true);
					}
					break;
				case WM.NCPAINT:
					if (bStyle != BorderStyle.None)
					{
						base.WndProc(ref m);
						IntPtr hdc = IntPtr.Zero;
						try
						{
							hdc = APIWrappers.GetWindowDC(this.Handle);
							if (hdc != IntPtr.Zero)
								using (Graphics g = Graphics.FromHdc(hdc))
									OnNonClientPaint(g);
						}
						catch (Exception ex)
						{
							throw ex;
						}
						finally
						{
							if (hdc != IntPtr.Zero)
								APIWrappers.ReleaseDC(this.Handle, hdc);
						}
						return;
					}
					break;
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
			base.OnPaint(e);
			Rectangle r = this.ClientRectangle;

			r.X += 1;
			r.Width -= btnWidth + 3;
			r.Y += 1;
			r.Height -= 2;

			string val = "";
			if (selectedItem != null)
			{
				TypeConverter tc = TypeDescriptor.GetConverter(selectedItem);
				if (tc == null || tc.CanConvertTo(typeof(string)) == false)
					val = selectedItem.ToString();
				else
					val = tc.ConvertToString(selectedItem);
			}
			if (this.Enabled)
			{
				if (this.Focused)
					using (SolidBrush b = new SolidBrush(ProfessionalColors.ButtonSelectedHighlight))
						e.Graphics.FillRectangle(b, r);
				r.Y += 1;
				e.Graphics.DrawString(val.ToString(), this.Font, SystemBrushes.ControlText, r, sf);
				//this.Focused ? SystemBrushes.HighlightText : 
			}
			else
			{
				e.Graphics.FillRectangle(SystemBrushes.Control, this.ClientRectangle);
				r.Y += 1;

				e.Graphics.DrawString(val, this.Font, SystemBrushes.GrayText, r, sf);
			}


			btnRect = new Rectangle(this.ClientRectangle.Width - btnWidth - 1, 1, btnWidth,
											  this.ClientRectangle.Height - 3);
			OnButtonPaint(e.Graphics);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseHover
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
			Rectangle r = this.ClientRectangle;

			r.X += 1;
			r.Width -= btnWidth + 4;
			r.Y += 1;
			r.Height -= 2;

			Size s = TextRenderer.MeasureText((selectedItem ?? "").ToString(), this.Font);
			if (s.Width >= r.Width && this.RectangleToScreen(r).Contains(Control.MousePosition))
				(toolTip = new ToolTip()).Show((selectedItem ?? "").ToString(), this, 5, 5, 3000);

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseMove
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (btnRect.Contains(e.Location))
			{
				if (overBtn == false)
				{
					overBtn = true;
					Invalidate();
				}
			}
			else
				if (overBtn)
				{
					overBtn = false;
					Invalidate();
				}
			base.OnMouseMove(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseLeave
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(EventArgs e)
		{
			if (overBtn)
			{
				overBtn = false;
				Invalidate();
			}
			if (toolTip != null)
				toolTip.Hide(this);
			base.OnMouseLeave(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseDown
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (disableButton)
				return;
			OpenDropDown();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnGotFocus
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnLostFocus
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnSizeChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// SetBoundsCore
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="specified"></param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, PreferredHeight, specified);
		}
		#endregion << Override  Methods >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Осуществляет рисование в неклиенской области.
		/// </summary>
		/// <param name="g"></param>
		protected void OnNonClientPaint(Graphics g)
		{
			Rectangle r = Rectangle.Round(g.VisibleClipBounds);
			if (bStyle == BorderStyle.FixedSingle)
			{
				SolidBrush b;
				bool useVS = VisualStyleInformation.IsEnabledByUser &&
							 VisualStyleInformation.IsSupportedByOS &&
							 Application.RenderWithVisualStyles &&
							 useVisualStyle;

				if (useVS)
					b = new SolidBrush(VisualStyleInformation.TextControlBorder);
				else
					b = new SolidBrush(this.BorderColor);
				using (b)  //
				{
					g.FillRectangle(b, 0, 0, r.Width, 1);
					g.FillRectangle(b, r.Width - 1, 0, r.Width, r.Height);
					g.FillRectangle(b, 0, r.Height - 1, r.Width, r.Height);
					g.FillRectangle(b, 0, 0, 1, r.Height);
				}
			}
			else if (bStyle == BorderStyle.Fixed3D)
				switch (b3Dstyle)
				{
					case Border3DKind.Sunken:
						g.DrawLine(SystemPens.ControlDarkDark, 1, 1, r.Width - 3, 1);  // Top
						g.DrawLine(SystemPens.ControlDarkDark, 1, 1, 1, r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlLight, 1, r.Height - 2, r.Width - 2, r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlLight, r.Width - 2, 1, r.Width - 2, r.Height - 2); // Right
						goto case Border3DKind.SunkenFlat;
					case Border3DKind.SunkenFlat:
						g.DrawLine(SystemPens.ControlDark, 0, 0, r.Width - 2, 0);  // Top
						g.DrawLine(SystemPens.ControlDark, 0, 0, 0, r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlLightLight, 0, r.Height - 1, r.Width - 1, r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlLightLight, r.Width - 1, 0, r.Width - 1, r.Height - 1); // Right
						break;
					case Border3DKind.Raised:
						g.DrawLine(SystemPens.ControlLight, 0, 0, r.Width - 2, 0);  // Top
						g.DrawLine(SystemPens.ControlLight, 0, 0, 0, r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlDarkDark, 0, r.Height - 1, r.Width - 1, r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlDarkDark, r.Width - 1, 0, r.Width - 1, r.Height - 1); // Right
						g.DrawLine(SystemPens.ControlLightLight, 1, 1, r.Width - 3, 1);  // Top
						g.DrawLine(SystemPens.ControlLightLight, 1, 1, 1, r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlDark, 1, r.Height - 2, r.Width - 2, r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.Width - 2, 1, r.Width - 2, r.Height - 2); // Right
						break;
					case Border3DKind.RaisedFlat:
						g.DrawLine(SystemPens.ControlLightLight, 0, 0, r.Width - 2, 0);  // Top
						g.DrawLine(SystemPens.ControlLightLight, 0, 0, 0, r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlDark, 0, r.Height - 1, r.Width - 1, r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.Width - 1, 0, r.Width - 1, r.Height - 1); // Right
						break;
					case Border3DKind.Etched:
						g.DrawLine(SystemPens.ControlDark, 0, 0, r.Width - 2, 0);  // Top
						g.DrawLine(SystemPens.ControlDark, 0, 0, 0, r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlLightLight, 0, r.Height - 1, r.Width - 1, r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlLightLight, r.Width - 1, 0, r.Width - 1, r.Height - 1); // Right
						g.DrawLine(SystemPens.ControlLightLight, 1, 1, r.Width - 3, 1);  // Top
						g.DrawLine(SystemPens.ControlLightLight, 1, 1, 1, r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlDark, 1, r.Height - 2, r.Width - 2, r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.Width - 2, 1, r.Width - 2, r.Height - 2); // Right
						break;
					case Border3DKind.Bump:
						g.DrawLine(SystemPens.ControlLightLight, 0, 0, r.Width - 2, 0);  // Top
						g.DrawLine(SystemPens.ControlLightLight, 0, 0, 0, r.Height - 2); // Left
						g.DrawLine(SystemPens.ControlDark, 0, r.Height - 1, r.Width - 1, r.Height - 1); // Bottom
						g.DrawLine(SystemPens.ControlDark, r.Width - 1, 0, r.Width - 1, r.Height - 1); // Right
						g.DrawLine(SystemPens.ControlDark, 1, 1, r.Width - 3, 1);  // Top
						g.DrawLine(SystemPens.ControlDark, 1, 1, 1, r.Height - 3); // Left
						g.DrawLine(SystemPens.ControlLightLight, 1, r.Height - 2, r.Width - 2, r.Height - 2); // Bottom
						g.DrawLine(SystemPens.ControlLightLight, r.Width - 2, 1, r.Width - 2, r.Height - 2); // Right
						break;
				}
			g.Flush();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обновляет рисование рамки.
		/// </summary>
		public void RefreshBorder()
		{
			APIWrappers.SetWindowPos(new HandleRef(this, this.Handle), new HandleRef(), 0, 0, 0, 0,
			 0x0001 | 0x0002 | 0x0004 | 0x0010 | 0x0020 | 0x0200);
			Application.DoEvents();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Назначает первый элемент выпадающего списка в качестве выбранного.
		/// </summary>
		/// <param name="raiseUIevent">Определяет, будет ли сгенерировано событие UISelectedItemChanged</param>
		public void SelectFirstDropDownItem(bool raiseUIevent = false)
		{
			try
			{
				if (items == null || items.Count == 0)
					return;
				IList list = null;
				if (sorted)
				{
					list = new List<object>(items.Cast<object>());
					((List<object>)list).Sort(Comparer);
				}
				else
					list = items;
				SelectedItem = list[0];
				if (raiseUIevent)
					OnUISelectedItemChanged();
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Осуществляет рисование кнопки.
		/// </summary>
		/// <param name="g"></param>
		protected void OnButtonPaint(Graphics g)
		{
			Rectangle r = btnRect;
			using (SolidBrush b = new SolidBrush(Enabled && disableButton == false ? BackColor : SystemColors.ControlDark))
				g.FillRectangle(b, r);
			r.Width -= 1;
			r.Height += 1;

			GraphicsPath path = new GraphicsPath();
			path.AddLine(r.X + r.Width, r.Y + 1, r.X + r.Width, r.Height - 1);
			path.AddLine(r.X + r.Width - 1, r.Height, r.X + 1, r.Height);
			path.AddLine(r.X, r.Height - 1, r.X, r.Y + 1);
			path.AddLine(r.X + 1, r.Y, r.X + r.Width - 1, r.Y);

			if (Enabled == false || disableButton)
				g.FillPath(SystemBrushes.Control, path);
			else if (isDropDownOpened)
				using (LinearGradientBrush b =
				 new LinearGradientBrush(r, SystemColors.ControlDark, SystemColors.ControlLightLight, LinearGradientMode.Vertical))
					g.FillPath(b, path);
			else if (overBtn)
				using (LinearGradientBrush b =
				  new LinearGradientBrush(r, SystemColors.Control, SystemColors.ControlLightLight, LinearGradientMode.Vertical))
					g.FillPath(b, path);
			else
				using (LinearGradientBrush b =
				  new LinearGradientBrush(r, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical))
					g.FillPath(b, path);


			g.DrawImageUnscaled(Properties.Resources.DropDownArrow, r.X + r.Width / 2 - 3, r.Y + r.Height / 2 - 3);

			Pen p;
			bool useVS = VisualStyleInformation.IsEnabledByUser &&
						  VisualStyleInformation.IsSupportedByOS &&
						  Application.RenderWithVisualStyles &&
						  useVisualStyle;

			if (useVS)
				p = new Pen(VisualStyleInformation.TextControlBorder);
			else
				p = new Pen(BorderColor);
			using (p)  //
				g.DrawPath(p, path);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Открывает выпадающий список.
		/// </summary>
		public void OpenDropDown()
		{
			try
			{
				if (OnDropDownOpening())
					return;
				if (dropDown == null && (items == null || items.Count == 0))
					return;

				if (dropDown == null)
				{
					IList list = null;
					if (sorted)
					{
						list = new List<object>(items.Cast<object>());
						((List<object>)list).Sort(Comparer);
					}
					else
						list = items;
					nativePopup = new SimSelectList(list);
					nativePopup.Opened += new EventHandler(DropDown_Opened);
					nativePopup.Closed += new ToolStripDropDownClosedEventHandler(DropDown_Closed);
					nativePopup.ItemSelected += new Pulsar.EventHandler<SimSelectList, object>(nativePopup_ItemSelected);
					nativePopup.SetAutoWidth(this.Width - 1);
					nativePopup.IsResizeble = isResizeble;
					nativePopup.SelectedItem = selectedItem;
					nativePopup.Show(this.PointToScreen(new Point(-1, this.Height - 1)));
				}
				else
				{
					dropDown.Opened += new EventHandler(DropDown_Opened);
					dropDown.Closed += new ToolStripDropDownClosedEventHandler(DropDown_Closed);
					if (dropDown is SimPopupControl)
					{
						((SimPopupControl)dropDown).IsResizeble = isResizeble;
						((SimPopupControl)dropDown).Show(this.PointToScreen(new Point(-1, this.Height - 1)));
					}
					else
					 dropDown.Show(this.PointToScreen(new Point(-1, this.Height - 1)));
				}
			}
			catch (Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		void DropDown_Opened(object sender, EventArgs e)
		{
			isDropDownOpened = true;
			OnDropDownOpened();
		}
		//-------------------------------------------------------------------------------------
		void DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			isDropDownOpened = false;
			((ToolStripDropDown)sender).Opened -= new EventHandler(DropDown_Opened);
			((ToolStripDropDown)sender).Closed -= new ToolStripDropDownClosedEventHandler(DropDown_Closed);
			OnDropDownClosed();
			this.Focus();
		}
		//-------------------------------------------------------------------------------------
		void nativePopup_ItemSelected(SimSelectList sender, object sel)
		{
			nativePopup.ItemSelected -= new Pulsar.EventHandler<SimSelectList, object>(nativePopup_ItemSelected);
			SelectedItem = sel;
			OnUISelectedItemChanged();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вызывает событие UISelectedItemChanged
		/// </summary>
		public void RaiseUISelectedItemChanged()
		{
			OnUISelectedItemChanged();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnKeyDown
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Down)
			{
				if (isDropDownOpened == false)
					OpenDropDown();
			}
			else if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Up)
				if (isDropDownOpened)
					(dropDown ?? nativePopup).Hide();

		}
		//-------------------------------------------------------------------------------------
		private int Comparer(object x, object y)
		{
			if (x == null && y == null)
				return 0;
			if (x == null)
				return -1;
			if (y == null)
				return 1;
			string sx;
			TypeConverter tc = TypeDescriptor.GetConverter(x);
			if (tc == null || tc.CanConvertTo(typeof(string)) == false)
				sx = x.ToString();
			else
				sx = tc.ConvertToString(x);
			string sy;
			tc = TypeDescriptor.GetConverter(y);
			if (tc == null || tc.CanConvertTo(typeof(string)) == false)
				sy = y.ToString();
			else
				sy = tc.ConvertToString(y);
			return String.Compare(sx, sy);
		}
		#endregion << Methods >>
	}
	//**************************************************************************************
	#region internal class SimComboBoxDesigner : ControlDesigner
	internal class SimComboBoxDesigner : ControlDesigner
	{
#pragma warning disable
		private EventHandler propChanged;
		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = base.SnapLines as ArrayList;
				int num = GetTextBaseline(this.Control, System.Drawing.ContentAlignment.TopLeft);
				num += 3;
				arrayList.Add(new SnapLine(SnapLineType.Baseline, num, SnapLinePriority.Medium));
				return arrayList;
			}
		}
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
				return selectionRules;
			}
		}
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				//if(this._actionLists == null)
				//{
				// this._actionLists = new DesignerActionListCollection();
				// this._actionLists.Add(new ListControlBoundActionList(this));
				//}
				return base.ActionLists;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.propChanged != null)
			{
			}
			base.Dispose(disposing);
		}
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			this.propChanged = new EventHandler(this.OnControlPropertyChanged);
		}
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
			{
				propertyDescriptor.SetValue(base.Component, "");
			}
		}
		public SimComboBoxDesigner()
		{
		}
		private void OnControlPropertyChanged(object sender, EventArgs e)
		{
			if (base.BehaviorService != null)
			{
				base.BehaviorService.SyncSelection();
			}
		}

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);
		// System.Design.SafeNativeMethods
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetTextMetrics(HandleRef hdc, TEXTMETRIC tm);
		// System.Design.SafeNativeMethods
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(HandleRef hObject);

		public static int GetTextBaseline(Control ctrl, System.Drawing.ContentAlignment alignment)
		{
			Rectangle clientRectangle = ctrl.ClientRectangle;
			int num = 0;
			int num2 = 0;
			using (Graphics graphics = ctrl.CreateGraphics())
			{
				IntPtr hdc = graphics.GetHdc();
				IntPtr handle = ctrl.Font.ToHfont();
				try
				{
					IntPtr handle2 = SelectObject(new HandleRef(ctrl, hdc), new HandleRef(ctrl, handle));
					TEXTMETRIC tEXTMETRIC = new TEXTMETRIC();
					GetTextMetrics(new HandleRef(ctrl, hdc), tEXTMETRIC);
					num = tEXTMETRIC.tmAscent + 1;
					num2 = tEXTMETRIC.tmHeight;
					SelectObject(new HandleRef(ctrl, hdc), new HandleRef(ctrl, handle2));
				}
				finally
				{
					DeleteObject(new HandleRef(ctrl.Font, handle));
					graphics.ReleaseHdc(hdc);
				}
			}
			if ((alignment & (System.Drawing.ContentAlignment)7) != (System.Drawing.ContentAlignment)0)
			{
				return clientRectangle.Top + num;
			}
			if ((alignment & (System.Drawing.ContentAlignment)112) != (System.Drawing.ContentAlignment)0)
			{
				return clientRectangle.Top + clientRectangle.Height / 2 - num2 / 2 + num;
			}
			return clientRectangle.Bottom - num2 + num;
		}
		//***************
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
			public TEXTMETRIC()
			{
			}
		}
#pragma warning restore
	}
	#endregion internal class SimComboBoxDesigner : ControlDesigner
}
