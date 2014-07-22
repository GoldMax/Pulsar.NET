using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Sim.Controls
{
	/// <summary>
	/// Класс панели с заголовком
	/// </summary>
	[Designer(typeof(PulsarGroupBoxDesigner))]
	public partial class SimGroupBox : UserControl
	{
		private int prevHeight = 0;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		/// <summary>
		/// Событие, вызываемое до изменения свернутости
		/// </summary>
		public event EventHandler UICollapseChanging;
		/// <summary>
		/// Вызывает событие UICollapseChanging
		/// </summary>
		protected void OnUICollapseChanging()
		{
			if(UICollapseChanging != null)
				UICollapseChanging(this, EventArgs.Empty);
		}
		/// <summary>
		/// Событие, вызываемое после изменения свернутости
		/// </summary>
		public event EventHandler UICollapseChanged;
		/// <summary>
		/// Вызывает событие UICollapseChanged
		/// </summary>
		protected void OnUICollapseChanged()
		{
			if(UICollapseChanged != null)
				UICollapseChanged(this,EventArgs.Empty);
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет отступы вне контрола.
		/// </summary>
		public new Padding Margin
		{
			get { return this.Padding; }
			set { this.Padding = value; }
		}
		/// <summary>
		/// Текст заголовка
		/// </summary>
		[Category("Appearance")]
		[Description("Текст заголовка")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Caption
		{
			get { return this.finistLabel1.Text; }
			set { this.finistLabel1.Text = value; }
		}
		/// <summary>
		/// Изображение заголовка
		/// </summary>
		[Category("Appearance")]
		[Description("Изображение заголовка")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(null)]
		public Image CaptionImage
		{
			get { return this.finistLabel1.Image; }
			set { this.finistLabel1.Image = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Объект панели
		/// </summary>
		[Category("Appearance")]
		[Description("Объект панели")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SimPanel BodyPanel
		{
			get { return fpBody; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет отображение кнопки сворачивания/разворачивания контрола.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет отображение кнопки сворачивания/разворачивания контрола.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool ShowCollapseButton
		{
			get { return btnCollapse.Visible; }
			set { btnCollapse.Visible = value; }
		}
		/// <summary>
		/// Определяет, свернута ли панель.
		/// </summary>
		public bool IsCollapsed
		{
			get { return !fpBody.Visible; }
		}
		/// <summary>
		/// Высота панели в свернутом состоянии.
		/// </summary>
		public int CollapsedHeight
		{
			get { return fpHeader.Height + 2 + Margin.Vertical; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimGroupBox()
		{ 
			InitializeComponent();
			this.finistPanel1.BackColor = ControlPaint.Light(SystemColors.Control, 0.5f);
				
			finistLabel1.ContextMenuStrip = null;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		private void btnCollapse_Click(object sender, EventArgs e)
		{
		 OnUICollapseChanging();
			if(fpBody.Visible)
			{
				btnCollapse.Image = Sim.Common.Properties.Resources.Expand_Normal;
				btnCollapse.ImagePushed = Sim.Common.Properties.Resources.Expand_Pushed;
				btnCollapse.ImageRaised = Sim.Common.Properties.Resources.Expand_Raised;
				prevHeight = this.Height;
				fpBody.Visible = false;
				this.Height = CollapsedHeight;
			}
			else
			{
				btnCollapse.Image = Sim.Common.Properties.Resources.Collapce_Normal;
				btnCollapse.ImagePushed = Sim.Common.Properties.Resources.Collapce_Pushed;
				btnCollapse.ImageRaised = Sim.Common.Properties.Resources.Collapce_Raised;
				fpBody.Visible = true;
				this.Height = prevHeight;
			}
			OnUICollapseChanged();
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// Изменяет размер по содержимому
		/// </summary>
		public void ShrinkSize()
		{
			if(fpBody.Controls.Count == 0)
			{
				Collapse();
				return;
			}
			Control c = fpBody.Controls[0];
			int h = c.Location.Y + fpBody.Padding.Bottom + c.Height;
			this.Height -= fpBody.Height - h;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Сворачивает панель
		/// </summary>
		public void Collapse()
		{
			if(fpBody.Visible)
			{
				btnCollapse.Image = Sim.Common.Properties.Resources.Expand_Normal;
				btnCollapse.ImagePushed = Sim.Common.Properties.Resources.Expand_Pushed;
				btnCollapse.ImageRaised = Sim.Common.Properties.Resources.Expand_Raised;
				prevHeight = this.Height;
				fpBody.Visible = false;
				this.Height = fpHeader.Height + 2 + Margin.Vertical;
			}
		}

		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
						
	}
	//**************************************************************************************
	#region << public class PulsarGroupBoxDesigner : ParentControlDesigner >>
		/// <summary>
	/// 
	/// </summary>
	public class PulsarGroupBoxDesigner : ParentControlDesigner
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize(component);

			if(this.Control is SimGroupBox)
			{
				this.EnableDesignMode(((SimGroupBox)this.Control).fpBody, "BodyPanel");
			}
		}
	} 
	#endregion << public class PulsarGroupBoxDesigner : ParentControlDesigner >>

}
