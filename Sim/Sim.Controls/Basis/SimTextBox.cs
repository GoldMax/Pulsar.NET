using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Sim.Controls.WinAPI;

namespace Sim.Controls
{
	/// <summary>
	/// Перечисление форматов данных TextBox.
	/// </summary>
	[Flags]
	public enum TextBoxFormat : uint
	{
		/// <summary>
		/// Все символы
		/// </summary>
		NotSet = 0xFFFFFFFF,
		/// <summary>
		/// Числа
		/// </summary>
		Digits = 1,
		/// <summary>
		/// Русские строчные буквы.
		/// </summary>
		RussianUpLetters = 2,
		/// <summary>
		/// Русские прописные буквы.
		/// </summary>
		RussianLowLetters = 4,
		/// <summary>
		/// Русские буквы.
		/// </summary>
		RussianLetters = RussianUpLetters | RussianLowLetters,
		/// <summary>
		/// Английские строчные буквы.
		/// </summary>
		EnglishUpLetters = 8,
		/// <summary>
		/// Английские прописные буквы.
		/// </summary>
		EnglishLowLetters = 16,
  /// <summary>
		/// Английские буквы.
		/// </summary>
		EnglishLetters = EnglishUpLetters | EnglishLowLetters,
		/// <summary>
		/// Все символы форматов
		/// </summary>
		All = Digits | RussianLetters | EnglishLetters
	}
	//**************************************************************************************
	/// <summary>
	/// Класс расширенного TextBox.
	/// </summary>
	[ToolboxBitmap(typeof(TextBox))]
	public class SimTextBox : TextBox
	{
		#region private static strings
		/// <summary>
		/// Русские строчные буквы.
		/// </summary>
		private static string RussianUpLetters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
		/// <summary>
		/// Русские прописные буквы.
		/// </summary>
		private static string RussianLowLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
		/// <summary>
		/// Английские строчные буквы.
		/// </summary>
		private static string EnglishUpLetters = "ABCDEFGHIJKLMNOPRSTQUVWXYZ";
		/// <summary>
		/// Английские прописные буквы.
		/// </summary>
		private static string EnglishLowLetters = "abcdefghijklmnoprstquvwxyz"; 
		#endregion

		private bool showContextMenu = true;
		private TextBoxFormat format = TextBoxFormat.NotSet;

		private Color bColor = SystemColors.ControlDark;
		private BorderStyle bStyle = BorderStyle.FixedSingle;
		private Border3DKind b3Dstyle = Border3DKind.Sunken;
		private bool useVisualStyleBorderColor = true;
		private Padding bWidth = new Padding(1);

		private bool raiseUITextChanged = true;

		string formatException = "";
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event EventHandler UITextChanged >>
		/// <summary>
		/// Событие, возникающее при изменении текста не через свойство Text.
		/// </summary>
		public event EventHandler UITextChanged;
		/// <summary>
		/// Вызывает событие UITextChanged.
		/// </summary>
		protected virtual void OnUITextChanged()
		{
			if(UITextChanged != null && raiseUITextChanged)
				UITextChanged(this, EventArgs.Empty);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Событие, возникающее до вставки текста из clipboard.
		/// </summary>
		public event CancelEventHandler BeforePaste;
		/// <summary>
		/// Вызывает событие BeforePaste
		/// </summary>
		protected virtual bool OnBeforePaste()
		{
		 CancelEventArgs arg = new CancelEventArgs(false);
			if(BeforePaste	!= null)
				BeforePaste(this, arg);
			return arg.Cancel;
		}
		#endregion << public event EventHandler UITextChanged >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет, будет ли показываться контекстное меню по правой кнопке мыши.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет, будет ли показываться контекстное меню по правой кнопке мыши.")]
		[DefaultValue(true)]
		public bool ShowContextMenu
		{
			get { return showContextMenu; }
			set { showContextMenu = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Формат разрешенных для ввода данных.
		/// </summary>
		[Category("Behavior")]
		[Description("Формат разрешенных для ввода данных.")]
		[DefaultValue(typeof(TextBoxFormat), "NotSet")]
		public TextBoxFormat Format
		{
			get { return format; }
			set { format = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Символы, разрешенные для ввода помимо символов формата.
		/// </summary>
		[Category("Behavior")]
		[Description("Символы, разрешенные для ввода помимо символов формата.")]
		[DefaultValue("")]
		public string FormatException
		{
			get { return formatException; }
			set { formatException = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет стиль рамки контрола.
		/// </summary>
		[DefaultValue(typeof(BorderStyle), "FixedSingle")]
		public new BorderStyle BorderStyle
		{
			get { return bStyle; }
			set 
			{
				if(bStyle != value)
				{
					bStyle = value; 
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
			get { return bWidth; }
			set 
			{
				bWidth = value;
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
		/// Определяет, будет ли использован цвет рамки схемы при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет, будет ли использован цвет рамки схемы при BorderStyle = FixedSingle.")]
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
		/// Определяет цвет фона
		/// </summary>
		public new Color BackColor
		{
			get { return base.BackColor; }
			set 
			{ 
					base.BackColor = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает количество строк при установленных свойствах Multiline и WordWrap.
		/// </summary>
		public int LineCount
		{
			get
			{
				Message msg = Message.Create(Handle, 0x00BA, IntPtr.Zero, IntPtr.Zero);
				DefWndProc(ref msg);
				return msg.Result.ToInt32();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Text
		/// </summary>
		public override string Text
		{
			get { return base.Text; }
			set
			{
				raiseUITextChanged = false;
				base.Text = value;
				raiseUITextChanged = true;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умочанию.
		/// </summary>
		public SimTextBox() : base() 
		{
			//base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				//this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.BorderStyle = BorderStyle.FixedSingle;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Возвращает строку, соответствующую установленному фортаму.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string GetFormatedString(string s)
		{
			for(int a = s.Length - 1;a >= 0;a--)
			{
				if(IsFormatAllow(s[a]) == false)
					s = s.Remove(a, 1);
			}
			return s;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Осуществляет рисование в неклиенской области.
		/// </summary>
		/// <param name="g"></param>
		protected void OnNonClientPaint(Graphics g)
		{
			bool useVS = VisualStyleInformation.IsEnabledByUser && 
																	VisualStyleInformation.IsSupportedByOS &&
																	Application.RenderWithVisualStyles && useVisualStyleBorderColor;

			//if(useVS)	//		bStyle == BorderStyle.Fixed3D &&   
			// return;
			////useVS = useVS;
		
			Rectangle r = Rectangle.Round(g.VisibleClipBounds);
			using(Pen p = new Pen(Enabled ? BackColor : SystemColors.Control, 2))
			{
				g.DrawLine(p, 0, 1, r.Width, 1);  // Top
				g.DrawLine(p, 1, 1, 1, r.Height); // Left
				g.DrawLine(p, 1, r.Height - 1, r.Width, r.Height - 1); // Bottom
				g.DrawLine(p, r.Width - 1, 1, r.Width - 1, r.Height); // Right
			}
			if(bStyle == BorderStyle.FixedSingle)
			{
				SolidBrush b;
				if(useVS)
					b = new SolidBrush(VisualStyleInformation.TextControlBorder);
				else
					b = new SolidBrush(this.BorderColor);         
				using(b)  //
				{
					g.FillRectangle(b, 0, 0, r.Width, bWidth.Top);
					g.FillRectangle(b, r.Width - bWidth.Right, 0, r.Width, r.Height);
					g.FillRectangle(b, 0, r.Height - bWidth.Bottom, r.Width, r.Height);
					g.FillRectangle(b, 0, 0, bWidth.Left, r.Height);
				}
			}
			else if(bStyle == BorderStyle.Fixed3D)
			{
				switch(b3Dstyle)
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
			this.Refresh(); 
			Application.DoEvents();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Проверяет указанный символ на соответствие установленному формату.
		/// </summary>
		/// <param name="ch">Проверяемый символ.</param>
		/// <returns></returns>
		public bool IsFormatAllow(char ch)
		{
			bool need = false;
			if(Char.IsDigit(ch) && (format & TextBoxFormat.Digits) == TextBoxFormat.Digits)
				need = true;
			else if(RussianUpLetters.Contains(ch.ToString()) &&
											(format & TextBoxFormat.RussianUpLetters) == TextBoxFormat.RussianUpLetters)
				need = true;
			else if(RussianLowLetters.Contains(ch.ToString()) &&
											(format & TextBoxFormat.RussianLowLetters) == TextBoxFormat.RussianLowLetters)
				need = true;
			else if(EnglishUpLetters.Contains(ch.ToString()) &&
											(format & TextBoxFormat.EnglishUpLetters) == TextBoxFormat.EnglishUpLetters)
				need = true;
			else if(EnglishLowLetters.Contains(ch.ToString()) &&
											(format & TextBoxFormat.EnglishLowLetters) == TextBoxFormat.EnglishLowLetters)
				need = true;
			else if(formatException.Contains(ch.ToString()))
				need = true;
			return need;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region << Override Methods >>
		/// <summary>
		/// WndProc
		/// </summary>
		/// <param name="m">Message</param>
		protected override void WndProc(ref Message m)
		{
			try
			{
				if(showContextMenu == false && m.Msg == WM.CONTEXTMENU)
					OnMouseClick(new MouseEventArgs(MouseButtons.Right, 1, (short)m.LParam, (short)(((uint)m.LParam >> 16)), 0));
				else if(m.Msg == WM.NCPAINT)
				{
					#region 
					if(VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS &&
									Application.RenderWithVisualStyles)
					{
						base.WndProc(ref m);
						return;
					}
					IntPtr hdc = IntPtr.Zero;
					try
					{
						//hdc = APIWrappers.GetDCEx(this.Handle, (IntPtr)m.WParam,
						//            (int)(GetDCExFlags.DCX_WINDOW | GetDCExFlags.DCX_PARENTCLIP ));
						hdc = APIWrappers.GetWindowDC(this.Handle);
						if(hdc != IntPtr.Zero)
							using(Graphics g = Graphics.FromHdc(hdc))
								OnNonClientPaint(g);
					}
					catch(Exception ex)
					{
						throw ex;
					}
					finally
					{
						if(hdc != IntPtr.Zero)
							APIWrappers.ReleaseDC(this.Handle, hdc);
					} 
					#endregion 
					return;
				}
				else if(m.Msg == WM.PASTE)
				{
					if(OnBeforePaste() == false)
					 base.WndProc(ref m);
				}
				else
					base.WndProc(ref m); 
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
				base.WndProc(ref m);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnKeyPress
		/// </summary>
		/// <param name="e">KeyPressEventArgs e</param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if(format == TextBoxFormat.NotSet)
				e.Handled = false;
			else if(e.KeyChar == '\b' || e.KeyChar == 3 || e.KeyChar == 24 || e.KeyChar == 26)
				e.Handled = false;
			else if(e.KeyChar == 22)
			{
				if(Clipboard.ContainsText(TextDataFormat.Text) || Clipboard.ContainsText(TextDataFormat.UnicodeText))
					Clipboard.SetText(GetFormatedString(Clipboard.GetText().Trim()));
			}
			else
				e.Handled = !IsFormatAllow(e.KeyChar);
			base.OnKeyPress(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnTextChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			OnUITextChanged();
		}
		#endregion << Override Methods >>
	}
}

