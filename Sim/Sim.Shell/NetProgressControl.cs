using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Timer = System.Threading.Timer;

//using Sim.Controls;

namespace Sim
{
	/// <summary>
	/// Класс контрола диалога прогресса загрузки данных.
	/// </summary>
	public partial class NetProgressControl : UserControl
	{
		//private System.Timers.Timer timer = null;
		private Timer timer = null;
		private Image[] imgs = new Image[16];
		private int pos = 0;
		
		private DateTime start = DateTime.MinValue;
		private TimeSpan res = TimeSpan.Zero;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает и устанавливает текст сообщения.
		/// </summary>
		public string MessageText
		{
			get { return labelText.Text; }
			set { labelText.Text = value; }
		}
		/// <summary>
		/// Возвращает прошедшее время
		/// </summary>
		public TimeSpan ElapsedTime
		{
			get { return res; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		/// <summary>
		/// Событие, возникающее при нажатии кнопки "Отменить".
		/// </summary>
		public event Action<Control> NeedTerminate;
		/// <summary>
		/// Вызывает событие NeedTerminate.
		/// </summary>
		protected void RaiseNeedTerminate()
		{
			if(NeedTerminate != null)
				NeedTerminate(this.Parent);
		}
		#endregion << Events >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public NetProgressControl()
		{
			InitializeComponent();

			timer = new Timer(new TimerCallback(timer_Elapsed), null, Timeout.Infinite, Timeout.Infinite);

			imgs[0] = global::Sim.Shell.Properties.Resources.f1;
			imgs[1] = global::Sim.Shell.Properties.Resources.f2;
			imgs[2] = global::Sim.Shell.Properties.Resources.f3;
			imgs[3] = global::Sim.Shell.Properties.Resources.f4;
			imgs[4] = global::Sim.Shell.Properties.Resources.f5;
			imgs[5] = global::Sim.Shell.Properties.Resources.f6;
			imgs[6] = global::Sim.Shell.Properties.Resources.f7;
			imgs[7] = global::Sim.Shell.Properties.Resources.f8;
			imgs[8] = global::Sim.Shell.Properties.Resources.f9;
			imgs[9] = global::Sim.Shell.Properties.Resources.f10;
			imgs[10] = global::Sim.Shell.Properties.Resources.f11;
			imgs[11] = global::Sim.Shell.Properties.Resources.f12;
			imgs[12] = global::Sim.Shell.Properties.Resources.f13;
			imgs[13] = global::Sim.Shell.Properties.Resources.f14;
			imgs[14] = global::Sim.Shell.Properties.Resources.f15;
			imgs[15] = global::Sim.Shell.Properties.Resources.f16;

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		private void NetProgressControl_Load(object sender, EventArgs e)
		{
			if(start == DateTime.MinValue)
				start = DateTime.Now;
			if(timer != null)
				timer.Change(150,Timeout.Infinite);
		}
		//-------------------------------------------------------------------------------------
		void Parent_SizeChanged(object sender, EventArgs e)
		{
			if(this.Parent != null)
			{
				this.Top = (this.Parent.Height - this.Height) / 2;
				this.Left = (this.Parent.Width - this.Width) / 2;
				this.Invalidate(true);
			}
		}
		//-------------------------------------------------------------------------------------
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			RaiseNeedTerminate();
		}
		//-------------------------------------------------------------------------------------
		private void NetProgressControl_ParentChanged(object sender, EventArgs e)
		{
			if(this.Parent != null)
			{
				this.Top = (this.Parent.Height - this.Height) / 2;
				this.Left = (this.Parent.Width - this.Width) / 2;
				this.BringToFront();
				this.UpdateZOrder();
				this.Parent.SizeChanged += new EventHandler(Parent_SizeChanged);
			}
		}
		//-------------------------------------------------------------------------------------
		void timer_Elapsed(object sender)
		{
			lock(imgs)
				if(timer == null || this.IsDisposed)
					return;
			this.Invoke(new Action(() =>
			{
				if(this.IsDisposed)
					return;
				pos++;
				if(pos == 16)
					pos = 0;
				fpImage.BackgroundImage = imgs[pos];
				fpImage.Invalidate();

				res = DateTime.Now.Subtract(start);
				if(res.Seconds >= 1)
					labelTime.Text = res.ToString(@"hh\:mm\:ss");
			}), null);
			lock(imgs)
				if(timer == null)
					this.Invoke(new Action(() => this.Dispose(true)), null); 
				else 
					timer.Change(150, Timeout.Infinite);
				
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		private class NavigatorPanel : Panel
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

				bool useVS = VisualStyleInformation.IsEnabledByUser && 
																	VisualStyleInformation.IsSupportedByOS &&
																	Application.RenderWithVisualStyles;

				Pen p;
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
	//*************************************************************************************
}
