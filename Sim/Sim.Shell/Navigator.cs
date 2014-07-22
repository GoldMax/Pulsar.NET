using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Pulsar;
using Pulsar.Clients;
using Sim.Controls;

namespace Sim.Shell
{
	/// <summary>
	/// Класс контрола основного меню и выбора подразделения
	/// </summary>
	internal partial class Navigator : UserControl
	{
		private static StringFormat sf = new StringFormat()
		{
			Alignment = StringAlignment.Center,
			FormatFlags = StringFormatFlags.NoClip
		};

	 private int _assistMessageCount = 0;
		private Sex _assistSex = Sex.Male;
		private Font _msgFont = null;


		private Bitmap _assistImage = Properties.Resources.Man48;
		private Bitmap _assistImage_over = Properties.Resources.Man48_Over;
		private Bitmap _assistImage_push = Properties.Resources.Man48_Pushed;

		private Bitmap _assistCountImage = Properties.Resources.Man48Msg;
		private Bitmap _assistCountImage_over = Properties.Resources.Man48Msg_Over;
		private Bitmap _assistCountImage_push = Properties.Resources.Man48Msg_Pushed;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public event FormSelectedEventHandler FormSelected >>
		/// <summary>
		/// Делегат события FormSelected.
		/// </summary>
		/// <param name="sender">Объект, посылающий событие.</param>
		/// <param name="formInfo">Информация о форме.</param>
		public delegate void FormSelectedEventHandler(object sender, FormInfo formInfo);
		/// <summary>
		/// Событие, возникающее при выборе элемента меню в навигаторе.
		/// </summary>
		public event FormSelectedEventHandler FormSelected;
		/// <summary>
		/// Метод, вызывающий событие FormSelected.
		/// </summary>
		/// <param name="formInfo">Информация о форме.</param>
		protected void OnFormSelected(FormInfo formInfo)
		{
			if(FormSelected != null && formInfo != null)
				FormSelected(this, formInfo);
		}
		#endregion << public event FormSelectedEventHandler FormSelected >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает элемнты меню для указанной вкладки.
		/// </summary>
		/// <param name="tabName">Имя вкладки.</param>
		/// <returns></returns>
		public ToolStripItemCollection this[string tabName]
		{
			get { return ((ToolStrip)tabControl1.TabPages[tabName].Controls[0]).Items; }
		}
		/// <summary>
		/// Возвращает имя выбранной вкладки.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedTabName 
		{ 
			get { return tabControl1.SelectedTab == null ? null : tabControl1.SelectedTab.Name; }
		}
		/// <summary>
		/// Пол картинки помошника
		/// </summary>
		public Sex AssistSex 
		{ 
		 get { return _assistSex; }
			set 
			{ 
			 _assistSex = value;
				_assistImage = _assistSex == Sex.Male ? Properties.Resources.Man48 : Properties.Resources.Woman48;
				_assistImage_over = _assistSex == Sex.Male ? Properties.Resources.Man48_Over : Properties.Resources.Woman48_Over;
				_assistImage_push = _assistSex == Sex.Male ? Properties.Resources.Man48_Pushed : Properties.Resources.Woman48_Pushed;

				_assistCountImage = _assistSex == Sex.Male ? Properties.Resources.Man48Msg : Properties.Resources.Woman48Msg;
				_assistCountImage_over = _assistSex == Sex.Male ? Properties.Resources.Man48Msg_Over : Properties.Resources.Woman48Msg_Over;
				_assistCountImage_push = _assistSex == Sex.Male ? Properties.Resources.Man48Msg_Pushed : Properties.Resources.Woman48Msg_Pushed;
				finistLabelUser.Image = _assistMessageCount == 0 ? _assistImage : _assistCountImage; ; 
		 }
		}
		/// <summary>
		/// Количество сообщений помошника
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int AssistMessageCount 
		{ 
		 get { return _assistMessageCount; }
			set 
			{ 
			 _assistMessageCount = value< 0 ? 0 : (value > 99 ? 99 : value);
				if(_assistMessageCount > 0)
				{
					Rectangle r = new Rectangle(31, 37, 10, 8);
					Rectangle rt = new Rectangle(29, 35, 14, 11);
					using(Graphics g = Graphics.FromImage(_assistCountImage))
					{
						g.FillRectangle(Brushes.White, r);
						g.DrawString(_assistMessageCount.ToString(), _msgFont, Brushes.Black, rt, sf); 
					}
					using(Graphics g = Graphics.FromImage(_assistCountImage_over))
					{
						g.FillRectangle(Brushes.White, r);
						//g.DrawRectangle(Pens.Red, r);
						//g.DrawRectangle(Pens.Green, rt);
						g.DrawString(_assistMessageCount.ToString(), _msgFont, Brushes.Black, rt, sf);
					}
					using(Graphics g = Graphics.FromImage(_assistCountImage_push))
					{
					 rt.X ++; r.X ++;
						rt.Y ++; r.Y ++;
						g.FillRectangle(Brushes.White, r);
						g.DrawString(_assistMessageCount.ToString(), _msgFont, Brushes.Black, rt, sf);
					}
				}
				finistLabelUser.Image = _assistMessageCount == 0 ? _assistImage : _assistCountImage; ; 
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public Navigator()
		{
			InitializeComponent();
			WinFormsUtils.SetStyle(tabControl1, ControlStyles.SupportsTransparentBackColor, true);
			WinFormsUtils.SetStyle(tabControl1, ControlStyles.AllPaintingInWmPaint, true);
			WinFormsUtils.SetStyle(tabControl1, ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			_msgFont = new Font(this.Font.FontFamily, 7f); 
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void finistLabelDiv_MouseEnter(object sender, EventArgs e)
		{
			finistLabelUser.Image = _assistMessageCount == 0 ? _assistImage_over : _assistCountImage_over;
		}
		private void finistLabelUser_MouseLeave(object sender, EventArgs e)
		{
			finistLabelUser.Image = _assistMessageCount == 0 ? _assistImage : _assistCountImage;
		}
		private void finistLabelDiv_MouseDown(object sender, MouseEventArgs e)
		{
			finistLabelUser.Image = _assistMessageCount == 0 ? _assistImage_push : _assistCountImage_push;
		}
		private void finistLabelDiv_MouseUp(object sender, MouseEventArgs e)
		{
			finistLabelUser.Image = _assistMessageCount == 0 ? _assistImage_over : _assistCountImage_over; 
		}
		//-------------------------------------------------------------------------------------
		internal void ToolStrips_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
		 //AssistMessageCount++;
			if(e.ClickedItem is ToolStripDropDownItem && ((ToolStripDropDownItem)e.ClickedItem).HasDropDownItems)
				return;
			PulsarMainMenu menu = (PulsarMainMenu)this.Tag;
			FormInfo fi = (FormInfo)e.ClickedItem.Tag;
			if(e.ClickedItem.OwnerItem != null && e.ClickedItem.OwnerItem is ToolStripDropDownItem)
				((ToolStripDropDownItem)e.ClickedItem.OwnerItem).HideDropDown();
			OnFormSelected(fi);
		}
		private void finistLabelUser_Click(object sender, EventArgs e)
		{
			CtrlAssistInfo ai = new CtrlAssistInfo();
			ai.Person = PulsarQuery.ContextQuery.User;
			Point p = finistLabelUser.PointToScreen(new Point(4,finistLabelUser.Height + 1));
			SimPopupControl.Show(ai, p, false);
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Добавляет новую вкладку.
		/// </summary>
		/// <param name="text">Текст вкладки.</param>
		/// <param name="name">Имя вкладки.</param>
		public void AddTab(string text, string name)
		{
			TabPage p = new TabPage();
			p.Name = name;
			p.Text = text;
			p.UseVisualStyleBackColor = false;
			p.BackColor = Color.Transparent;
			ToolStrip tp = new ToolStrip();
			tp.ShowItemToolTips = false;
			tp.AutoSize = false;
			tp.BackColor = Color.Transparent;
			tp.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			tp.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			tp.Size = new System.Drawing.Size(595, 26);
			tp.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStrips_ItemClicked);
			p.Controls.Add(tp);
			tabControl1.TabPages.Add(p);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержит ли меню указанную вкладку.
		/// </summary>
		/// <param name="name">Имя вкладки.</param>
		/// <returns></returns>
		public bool ContainsTab(string name)
		{
			return tabControl1.TabPages.ContainsKey(name);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Выбирает вкладку
		/// </summary>
		/// <param name="name">Имя вкладки.</param>
		public void SelectTab(string name)
		{
			if(name != null && tabControl1.TabPages.ContainsKey(name))
				tabControl1.SelectTab(name);
		}
		#endregion << Methods >>

		//-------------------------------------------------------------------------------------
	}
	//**************************************************************************************
	internal class NavigatorTabControl : TabControl
	{
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Цвет фона
		/// </summary>
		public override Color BackColor
		{
			get { return Color.Transparent; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public NavigatorTabControl() : base()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer |
																	ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			GraphicsPath path = null;
			try
			{
				Graphics g = e.Graphics;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				Rectangle r = this.ClientRectangle;
				r.X += 1;
				r.Y += 1;
				r.Width -= 2;
				r.Height -= 2;
				Rectangle d = this.DisplayRectangle;
				d.Y -= 3;
				int arcX = 10, arcY = 10;

				path = new GraphicsPath();
				path.AddArc(r.Width-arcX, d.Y, arcX, arcY, -90, 90);
				path.AddLine(r.Width, d.Y+arcY/2, r.Width, r.Height-arcY/2);
				path.AddArc(r.Width-arcX, r.Height-arcY, arcX, arcY, 0, 90);
				path.AddLine(r.Width-arcX/2, r.Height, arcX/2, r.Height);
				path.AddArc(0, r.Height-arcX, arcY, arcX, 90, 90);
				path.AddLine(0, r.Height-arcY/2, 0, d.Y+arcY/2);
				if(SelectedIndex != 0)
					path.AddArc(0, d.Y, arcX, arcY, 180, 90);
				else
					path.AddArc(0, d.Y, arcX, arcY, 180, 45);
				if(TabPages.Count == 0)
					path.AddLine(arcX/2, d.Y, r.Width - arcX/2, d.Y);
				else
				{
					Rectangle t = this.GetTabRect(this.SelectedIndex);
					t.Width -= 1;
					int x = t.X;
					path.AddLine(x, d.Y, x, r.Y+arcY/2);
					path.AddArc(x, 0, arcX, arcY, 180, 90);
					path.AddLine(x+arcX/2, 0, x+t.Width-arcX, 0);
					path.AddArc(x+t.Width-arcX, 0, arcX, arcY, -90, 90);
					path.AddLine(x+t.Width, arcY/2, x+t.Width, d.Y);
					path.AddLine(x+t.Width, d.Y, r.Width-arcX/2, d.Y);
				}

				using(LinearGradientBrush b = 
					new LinearGradientBrush(r, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical))
					g.FillPath(b, path);

				Pen p;
				bool useVS = VisualStyleInformation.IsEnabledByUser && 
																	VisualStyleInformation.IsSupportedByOS &&
																	Application.RenderWithVisualStyles;

				if(useVS)
					p = new Pen(VisualStyleInformation.TextControlBorder);
				else
					p = new Pen(SystemColors.ControlDarkDark);
				using(p)  //
					g.DrawPath(p, path);

				for(int a = 0; a < TabPages.Count; a++)
				{
					Rectangle t = this.GetTabRect(a);
					t.Width -= 1;
					StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Center;
					sf.Trimming = StringTrimming.EllipsisCharacter;
					if(this.SelectedIndex == a)
						g.DrawString(TabPages[a].Text.Trim(), this.Font, SystemBrushes.HotTrack, t, sf);
					else
						g.DrawString(TabPages[a].Text.Trim(), this.Font, SystemBrushes.ControlDarkDark, t, sf);
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
			}
			finally
			{
				if(path != null)
					path.Dispose();
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	//*************************************************************************************
	internal class NavigatorPanel : Panel
	{
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public NavigatorPanel()
			: base()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | 
																	ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor |
																	ControlStyles.UserPaint, true);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			GraphicsPath path = null;
			try
			{
				Graphics g = e.Graphics;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				Rectangle r = this.ClientRectangle;
				using(Brush b = new SolidBrush(this.BackColor))
					g.FillRectangle(b, r);
				r.X += 1;
				r.Y += 1;
				r.Width -= 2;
				r.Height -= 2;
				int arcX = 10, arcY = 10;
				path = new GraphicsPath();
				path.AddArc(r.Width-arcX, r.Y, arcX, arcY, -90, 90);
				path.AddLine(r.Width, r.Y+arcY/2, r.Width, r.Height-arcY/2);
				path.AddArc(r.Width-arcX, r.Height-arcY, arcX, arcY, 0, 90);
				path.AddLine(r.Width-arcX/2, r.Height, arcX/2, r.Height);
				path.AddArc(r.X, r.Height-arcX, arcY, arcX, 90, 90);
				path.AddLine(r.X, r.Height-arcY/2, r.X, r.Y+arcY/2);
				path.AddArc(r.X, r.Y, arcX, arcY, 180, 90);
				path.AddLine(arcX/2, r.Y, r.Width-arcX/2, r.Y);

				using(LinearGradientBrush b = 
					new LinearGradientBrush(r, SystemColors.ControlLightLight, SystemColors.Control, LinearGradientMode.Vertical))
					g.FillPath(b, path);

				Pen p;
				bool useVS = VisualStyleInformation.IsEnabledByUser && 
																	VisualStyleInformation.IsSupportedByOS &&
																	Application.RenderWithVisualStyles;

				if(useVS)
					p = new Pen(VisualStyleInformation.TextControlBorder);
				else
					p = new Pen(SystemColors.ControlDarkDark);
				using(p)  //
					g.DrawPath(p, path);

			}
			finally
			{
				if(path != null)
					path.Dispose();
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------

	}

}
