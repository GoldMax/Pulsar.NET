namespace Sim.AdminForms
{
	partial class FormMainMenu
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainMenu));
			Sim.Controls.SimToolStripColorTable simToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ftvTree = new Sim.Controls.SimTreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnAdd = new System.Windows.Forms.ToolStripButton();
			this.btnDel = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.lblSortOrder = new Sim.Controls.SimLabel();
			this.lblEditSortOrder = new Sim.Controls.SimLabelEditor();
			this.finButtonSave = new Sim.Controls.SimButton();
			this.PanelBack.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.ftvTree.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PanelBack
			// 
			this.PanelBack.Controls.Add(this.splitContainer1);
			this.PanelBack.Size = new System.Drawing.Size(952, 657);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.ftvTree);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer1.Panel2.Controls.Add(this.finistPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(952, 657);
			this.splitContainer1.SplitterDistance = 443;
			this.splitContainer1.TabIndex = 0;
			// 
			// ftvTree
			// 
			this.ftvTree.AllowInternalDragDrop = true;
			this.ftvTree.BackColor = System.Drawing.SystemColors.Control;
			// 
			// 
			// 
			this.ftvTree.ContextMenuStrip.Name = "contextMenuStrip1";
			this.ftvTree.ContextMenuStrip.Size = new System.Drawing.Size(259, 76);
			this.ftvTree.CurrentView = Sim.Controls.TreeViewKind.FirstView;
			this.ftvTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ftvTree.FirstViewButtonImage = ((System.Drawing.Image)(resources.GetObject("ftvTree.FirstViewButtonImage")));
			this.ftvTree.FirstViewButtonToolTipText = "Основной вид дерева";
			this.ftvTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ftvTree.HideSelection = false;
			this.ftvTree.ImageList = this.imageList1;
			this.ftvTree.Location = new System.Drawing.Point(0, 0);
			// 
			// ftvTree.MainToolStrip
			// 
			simToolStripColorTable1.UseSystemColors = false;
			this.ftvTree.MainToolStrip.ColorTable = simToolStripColorTable1;
			this.ftvTree.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ftvTree.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btnAdd,
            this.btnDel,
            this.toolStripButton1,
            this.toolStripButton2});
			this.ftvTree.MainToolStrip.Location = new System.Drawing.Point(0, 0);
			this.ftvTree.MainToolStrip.Name = "MainToolStrip";
			this.ftvTree.MainToolStrip.Size = new System.Drawing.Size(443, 25);
			this.ftvTree.MainToolStrip.TabIndex = 0;
			this.ftvTree.MainToolStrip.Text = "toolStrip1";
			this.ftvTree.Name = "ftvTree";
			this.ftvTree.NodeItemsImageIndex = 1;
			this.ftvTree.SecondViewButtonImage = ((System.Drawing.Image)(resources.GetObject("ftvTree.SecondViewButtonImage")));
			this.ftvTree.SecondViewButtonToolTipText = "Дополнительный вид дерева";
			this.ftvTree.Size = new System.Drawing.Size(443, 657);
			this.ftvTree.Sorted = true;
			this.ftvTree.TabIndex = 0;
			this.ftvTree.SelectedNodeChanged += new Sim.Controls.SimTreeView.SelectedNodeChangedHandler(this.ftvTree_SelectedNodeChanged);
			this.ftvTree.ItemDropped += new System.EventHandler<Sim.Controls.SimTreeView.DragDropItemEventArgs>(this.ftvTree_ItemDropped);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnAdd
			// 
			this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAdd.Image = global::Sim.AdminForms.Properties.Resources.Add;
			this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(23, 22);
			this.btnAdd.Text = "toolStripButton1";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDel
			// 
			this.btnDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnDel.Image = global::Sim.AdminForms.Properties.Resources.Delete_big;
			this.btnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(23, 22);
			this.btnDel.Text = "toolStripButton2";
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::Sim.AdminForms.Properties.Resources.Collapce_Raised;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "Перенести в корень";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::Sim.AdminForms.Properties.Resources.Refresh;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// propertyGrid
			// 
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(0, 43);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(505, 614);
			this.propertyGrid.TabIndex = 0;
			// 
			// finistPanel1
			// 
			this.finistPanel1.Controls.Add(this.lblSortOrder);
			this.finistPanel1.Controls.Add(this.lblEditSortOrder);
			this.finistPanel1.Controls.Add(this.finButtonSave);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanel1.Location = new System.Drawing.Point(0, 0);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(505, 43);
			this.finistPanel1.TabIndex = 1;
			// 
			// lblSortOrder
			// 
			this.lblSortOrder.Location = new System.Drawing.Point(3, 14);
			this.lblSortOrder.Name = "lblSortOrder";
			this.lblSortOrder.Size = new System.Drawing.Size(55, 13);
			this.lblSortOrder.TabIndex = 2;
			this.lblSortOrder.Text = "SortOrder";
			// 
			// lblEditSortOrder
			// 
			this.lblEditSortOrder.AutoSize = false;
			this.lblEditSortOrder.BackColor = System.Drawing.SystemColors.Window;
			this.lblEditSortOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblEditSortOrder.Format = Sim.Controls.TextBoxFormat.Digits;
			this.lblEditSortOrder.Image = ((System.Drawing.Image)(resources.GetObject("lblEditSortOrder.Image")));
			this.lblEditSortOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblEditSortOrder.Location = new System.Drawing.Point(64, 12);
			this.lblEditSortOrder.MinimumSize = new System.Drawing.Size(50, 17);
			this.lblEditSortOrder.Name = "lblEditSortOrder";
			this.lblEditSortOrder.Size = new System.Drawing.Size(75, 17);
			this.lblEditSortOrder.TabIndex = 1;
			this.lblEditSortOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblEditSortOrder.Value = "";
			this.lblEditSortOrder.UIValueChanged += new Pulsar.EventHandler<Sim.Controls.SimLabelEditor, string>(this.lblEditSortOrder_UIValueChanged);
			// 
			// finButtonSave
			// 
			this.finButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.finButtonSave.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.finButtonSave.Image = global::Sim.AdminForms.Properties.Resources.Save;
			this.finButtonSave.Location = new System.Drawing.Point(344, 7);
			this.finButtonSave.Name = "finButtonSave";
			this.finButtonSave.Size = new System.Drawing.Size(153, 29);
			this.finButtonSave.TabIndex = 0;
			this.finButtonSave.Text = "Сохранить изменения";
			this.finButtonSave.Click += new System.EventHandler(this.finButtonSave_Click);
			// 
			// FormMainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(952, 657);
			this.Name = "FormMainMenu";
			this.Text = "FormMainMenu";
			this.PanelBack.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ftvTree.ResumeLayout(false);
			this.ftvTree.PerformLayout();
			this.finistPanel1.ResumeLayout(false);
			this.finistPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private Controls.SimTreeView ftvTree;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private Controls.SimPanel finistPanel1;
		private Controls.SimButton finButtonSave;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnAdd;
		private System.Windows.Forms.ToolStripButton btnDel;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private Controls.SimLabel lblSortOrder;
		private Controls.SimLabelEditor lblEditSortOrder;
	}
}