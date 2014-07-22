using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Sim.Controls.Common
{
	/// <summary>
	/// Класс модифицированного ListBox
	/// </summary>
	public class PulsarListBox : ListBox
	{
		private ToolTip toolTip1;
		private System.ComponentModel.IContainer components;

		private DrawMode drawMode = DrawMode.Normal;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// 
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(typeof(DrawMode),"Normal")]
		[Description("Определяет режим рисования элементов списка.")]
		public new DrawMode DrawMode
		{
			get { return drawMode; }
			set 
			{ 
				drawMode = value;
				if(drawMode == DrawMode.Normal)
					base.DrawMode = DrawMode.OwnerDrawFixed;
				else
					base.DrawMode = drawMode; 
			}
		}
	
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public PulsarListBox() : base()
		{
			InitializeComponent();
			DoubleBuffered = true;
			base.DrawMode = DrawMode.OwnerDrawFixed;
		}
		//-------------------------------------------------------------------------------------
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			this.ResumeLayout(false);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Overrides Methods >>
		/// <summary>
		/// OnDrawItem
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			try
			{
				if(DesignMode)
					return;
				if(drawMode != DrawMode.Normal)
				{
					base.OnDrawItem(e);
					return;
				}
				
				Graphics g = e.Graphics;
				if((e.State & DrawItemState.Selected) == DrawItemState.Selected && this.SelectionMode == SelectionMode.None)
					e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
				else 
					e.DrawBackground();
				if(e.Index == -1)
					return;
				string text = this.Items[e.Index].ToString();
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				g.DrawString(text, this.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnMouseMove
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			try
			{
				int pos = this.IndexFromPoint(this.PointToClient(Control.MousePosition));
				if(pos == ListBox.NoMatches)
					return;
				if(toolTip1.GetToolTip(this) == this.Items[pos].ToString() && toolTip1.Active)
					return;
				Graphics g = this.CreateGraphics();

				Size preferredSize = g.MeasureString(this.Items[pos].ToString(), this.Font).ToSize();
				if(preferredSize.Width > this.ClientRectangle.Width)
					toolTip1.SetToolTip(this, this.Items[pos].ToString());
				else
					toolTip1.RemoveAll();
				g.Dispose();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnSizeChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			this.Refresh();
			base.OnSizeChanged(e);
		}
		#endregion << Overrides Methods >>

		//-------------------------------------------------------------------------------------
										
	}
}
