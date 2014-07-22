namespace Sim.AdminForms
{
	partial class FormPersons
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
			Sim.Controls.SimToolStripColorTable finistToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.fdgvList = new Sim.Controls.SimDataGridView();
			this.Column2 = new System.Windows.Forms.DataGridViewImageColumn();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.finistToolStrip1 = new Sim.Controls.SimToolStrip();
			this.btnNew = new System.Windows.Forms.ToolStripButton();
			this.btnDel = new System.Windows.Forms.ToolStripButton();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.finButtonSave = new Sim.Controls.SimButton();
			this.finButtonUndo = new Sim.Controls.SimButton();
			this.PanelBack.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fdgvList)).BeginInit();
			this.finistToolStrip1.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PanelBack
			// 
			this.PanelBack.Controls.Add(this.splitContainer1);
			this.PanelBack.Size = new System.Drawing.Size(907, 628);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.fdgvList);
			this.splitContainer1.Panel1.Controls.Add(this.finistToolStrip1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
			this.splitContainer1.Panel2.Controls.Add(this.finistPanel1);
			this.splitContainer1.Size = new System.Drawing.Size(907, 628);
			this.splitContainer1.SplitterDistance = 302;
			this.splitContainer1.TabIndex = 0;
			// 
			// fdgvList
			// 
			this.fdgvList.AllowAutoGenerateColumns = false;
			this.fdgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.fdgvList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fdgvList.ColumnHeadersVisible = false;
			this.fdgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column1});
			this.fdgvList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fdgvList.Location = new System.Drawing.Point(0, 25);
			this.fdgvList.MultiSelect = false;
			this.fdgvList.Name = "fdgvList";
			this.fdgvList.RowHeadersVisible = false;
			this.fdgvList.RowTemplate.Height = 18;
			this.fdgvList.Size = new System.Drawing.Size(302, 603);
			this.fdgvList.TabIndex = 0;
			this.fdgvList.VirtualMode = true;
			this.fdgvList.SelectionChanged += new System.EventHandler(this.fdgvList_SelectionChanged);
			// 
			// Column2
			// 
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.Column2.HeaderText = "Column2";
			this.Column2.Image = global::Sim.AdminForms.Properties.Resources.User;
			this.Column2.MinimumWidth = 25;
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Column2.Width = 25;
			// 
			// Column1
			// 
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column1.DataPropertyName = "FullName";
			this.Column1.HeaderText = "Column1";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			// 
			// finistToolStrip1
			// 
			finistToolStripColorTable1.UseSystemColors = false;
			this.finistToolStrip1.ColorTable = finistToolStripColorTable1;
			this.finistToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.finistToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnDel});
			this.finistToolStrip1.Location = new System.Drawing.Point(0, 0);
			this.finistToolStrip1.Name = "finistToolStrip1";
			this.finistToolStrip1.Size = new System.Drawing.Size(302, 25);
			this.finistToolStrip1.TabIndex = 1;
			this.finistToolStrip1.Text = "finistToolStrip1";
			// 
			// btnNew
			// 
			this.btnNew.Image = global::Sim.AdminForms.Properties.Resources.UserAdd;
			this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(65, 22);
			this.btnNew.Text = "Новый";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
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
			// propertyGrid
			// 
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Left;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(421, 587);
			this.propertyGrid.TabIndex = 1;
			this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
			// 
			// finistPanel1
			// 
			this.finistPanel1.Controls.Add(this.finButtonSave);
			this.finistPanel1.Controls.Add(this.finButtonUndo);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanel1.Location = new System.Drawing.Point(0, 587);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(601, 41);
			this.finistPanel1.TabIndex = 2;
			// 
			// finButtonSave
			// 
			this.finButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.finButtonSave.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.finButtonSave.Enabled = false;
			this.finButtonSave.Image = global::Sim.AdminForms.Properties.Resources.Save;
			this.finButtonSave.Location = new System.Drawing.Point(410, 8);
			this.finButtonSave.Name = "finButtonSave";
			this.finButtonSave.Size = new System.Drawing.Size(179, 25);
			this.finButtonSave.TabIndex = 1;
			this.finButtonSave.Text = "Сохранить изменения";
			this.finButtonSave.Click += new System.EventHandler(this.finButtonSave_Click);
			// 
			// finButtonUndo
			// 
			this.finButtonUndo.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.finButtonUndo.Enabled = false;
			this.finButtonUndo.Image = global::Sim.AdminForms.Properties.Resources.Stop;
			this.finButtonUndo.Location = new System.Drawing.Point(10, 8);
			this.finButtonUndo.Name = "finButtonUndo";
			this.finButtonUndo.Size = new System.Drawing.Size(170, 25);
			this.finButtonUndo.TabIndex = 0;
			this.finButtonUndo.Text = "Отменить изменения";
			this.finButtonUndo.Click += new System.EventHandler(this.finButtonUndo_Click);
			// 
			// FormPersons
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(907, 628);
			this.Name = "FormPersons";
			this.Text = "FormPersons";
			this.PanelBack.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fdgvList)).EndInit();
			this.finistToolStrip1.ResumeLayout(false);
			this.finistToolStrip1.PerformLayout();
			this.finistPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private Controls.SimDataGridView fdgvList;
		private Controls.SimToolStrip finistToolStrip1;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private Controls.SimPanel finistPanel1;
		private Controls.SimButton finButtonSave;
		private Controls.SimButton finButtonUndo;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnDel;
		private System.Windows.Forms.DataGridViewImageColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
	}
}