using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// Класс кнопки с выподающем меню
	/// </summary>
	[ToolboxBitmap(typeof(ToolStripDropDownButton))]
	[DefaultProperty("Image")]
	public class SimDropDownButton	: SimButtonBase
	{
		private ToolStripDropDown _dropDown = null;
		private bool _autoSize = false;
		private bool _forceDocksize = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#region Overrides
		#pragma warning disable	1591
		protected override Size DefaultSize
		{
			get { return new Size(60, 20); }
		}
		[DefaultValue(typeof(TextImageRelation), "TextBeforeImage")]
		public override TextImageRelation TextImageRelation
		{
			get	{	return base.TextImageRelation;		}
			set	{	base.TextImageRelation = value;	}
		}
		[DefaultValue(false)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get { return _autoSize; }
			set 
			{ 
				_autoSize = value;
				base.AutoSize = value;
				if(_autoSize)
					OnResize(EventArgs.Empty);
			}
		}
		public override string Text
		{
			get { return base.Text; }
			set 
			{ 
				base.Text = value;
				if(_autoSize)
					OnResize(EventArgs.Empty);
			}
		}
		public override Image Image
		{
			get { return base.Image; }
			set
			{
				base.Image = value;
				if(_autoSize)
					OnResize(EventArgs.Empty);
			}
		}
		public override bool ShowBorder
		{
			get	{return base.ShowBorder;	}
			set
			{
				base.ShowBorder = value;
				if(_autoSize)
					OnResize(EventArgs.Empty);
			}
		}
		#pragma warning restore	1591 
		#endregion Overrides
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет пользовательский выпадающий список.
		/// </summary>
		[Category("Data")]
		[Description("Определяет пользовательский выпадающий список.")]
		[DefaultValue(null)]
		public ToolStripDropDown DropDown
		{
			get { return _dropDown; }
			set { _dropDown = value; }
		}
		/// <summary>
		/// Определяет форсирование определения размера при AutoSize=true и Dock != None по правилам докинга.
		/// </summary>
		[Category("Layout")]
		[Description("Определяет форсирование определения размера при AutoSize=true и Dock != None по правилам докинга.")]
		[DefaultValue(false)]
		public bool ForceDockSize
		{
			get { return _forceDocksize; }
			set
			{
				_forceDocksize = value;
				if(_autoSize)
					OnResize(EventArgs.Empty);
			}
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDropDownButton()
			: base()
		{
			base.TextImageRelation = TextImageRelation.TextBeforeImage;
			base.Image = Properties.Resources.DropDownArrow;
			base.SetAutoSizeMode(AutoSizeMode.GrowAndShrink); 

			//BackColorStart = RaisedBackColorStart = Color.Transparent;
			//InactiveBorderColor = Color.Transparent;
			//ActiveBorderColor = SystemColors.ControlDark;
			//PushedBackColorStart = SystemColors.ControlDark;
			//TabStop = false;
			//base.NoDarkIfDisable = true;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		protected override void OnClick(EventArgs e)
		{
			if(_dropDown != null)
			{
				Point p = new Point(Width-_dropDown.Width, Height-1);
				p = this.PointToScreen(p);
				_dropDown.Show(p);
			}
			base.OnClick(e);
		}
		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.OnAutoSizeChanged(e);
		}
		public override Size GetPreferredSize(Size proposedSize)
		{
			if(AutoSize)
			{
				Size s;
				using(Graphics g = this.CreateGraphics())
					s = g.MeasureString(this.Text, this.Font).ToSize();
				s.Width += Padding.Horizontal + 8;
				s.Height += Padding.Vertical + 7;
				if(Image != null)
					if(TextImageRelation == TextImageRelation.ImageBeforeText || TextImageRelation == TextImageRelation.TextBeforeImage)
					{
						s.Width += TextImageSpace + Image.Width;
						s.Height = Math.Max(s.Height, Image.Height);
					}
					else	if(TextImageRelation == TextImageRelation.ImageAboveText || TextImageRelation == TextImageRelation.TextAboveImage)
					{
						s.Height += TextImageSpace	+ Image.Height;
						s.Width += Math.Max(s.Width, Image.Width);
					}
				if(Dock != DockStyle.None && _forceDocksize && Parent != null)
				{
					if(Dock == DockStyle.Left || Dock == DockStyle.Right || Dock == DockStyle.Fill)
						s.Height = Parent.Height - Parent.Padding.Vertical - (ShowBorder ? 2 : 1);
					if(Dock == DockStyle.Top || Dock == DockStyle.Bottom || Dock == DockStyle.Fill)
						s.Width = Parent.Width - Parent.Padding.Horizontal - (ShowBorder ? 2 : 1);
				}

				if(MinimumSize.Width != 0 && s.Width < MinimumSize.Width)
					s.Width = MinimumSize.Width;
				if(MaximumSize.Width != 0 && s.Width > MaximumSize.Width)
					s.Width = MaximumSize.Width;
				if(MinimumSize.Width != 0 && s.Height < MinimumSize.Height)
					s.Height = MinimumSize.Height;
				if(MaximumSize.Width != 0 && s.Height > MaximumSize.Height)
					s.Height = MaximumSize.Height;
				return s;
			}
			else
				return base.GetPreferredSize(proposedSize);
		}
		protected override void OnResize(EventArgs e)
		{
			if(_autoSize)
			{
				Size = GetPreferredSize(Size);
			}
			base.OnResize(e);
		}
		#endregion << Methods >>

	}
}
