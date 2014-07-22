namespace Sim.Controls
{
	partial class SimDataGridViewEx
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimDataGridViewEx));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.fdgv = new Sim.Controls.SimDataGridView();
			this.panelDetail = new Sim.Controls.SimPanel();
			this.finistPanel2 = new Sim.Controls.SimPanel();
			this.finistLabel1 = new Sim.Controls.SimLabel();
			this.finistPopupButton1 = new Sim.Controls.SimPopupButton();
			this.finistContextMenu1 = new Sim.Controls.SimContextMenu(this.components);
			this.menuItemToDetail = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemShowDetail = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemFrozen = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fdgv)).BeginInit();
			this.finistPanel2.SuspendLayout();
			this.finistContextMenu1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.fdgv);
			this.splitContainer1.Panel1MinSize = 50;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panelDetail);
			this.splitContainer1.Panel2.Controls.Add(this.finistPanel2);
			this.splitContainer1.Panel2MinSize = 50;
			this.splitContainer1.Size = new System.Drawing.Size(484, 425);
			this.splitContainer1.SplitterDistance = 327;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 0;
			// 
			// fdgv
			// 
			this.fdgv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fdgv.Location = new System.Drawing.Point(0, 0);
			this.fdgv.Name = "fdgv";
			this.fdgv.RowTemplate.Height = 18;
			this.fdgv.Size = new System.Drawing.Size(327, 425);
			this.fdgv.TabIndex = 0;
			this.fdgv.VirtualMode = true;
			// 
			// panelDetail
			// 
			this.panelDetail.AllowDrop = true;
			this.panelDetail.AutoScroll = true;
			this.panelDetail.BackColor = System.Drawing.Color.Ivory;
			this.panelDetail.BackColor2 = System.Drawing.Color.Ivory;
			this.panelDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelDetail.Location = new System.Drawing.Point(0, 25);
			this.panelDetail.Name = "panelDetail";
			this.panelDetail.Size = new System.Drawing.Size(155, 400);
			this.panelDetail.TabIndex = 0;
			// 
			// finistPanel2
			// 
			this.finistPanel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanel2.BackColor2 = System.Drawing.SystemColors.Control;
			this.finistPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel2.BorderWidth = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.finistPanel2.Controls.Add(this.finistLabel1);
			this.finistPanel2.Controls.Add(this.finistPopupButton1);
			this.finistPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanel2.GradientMode = Sim.Controls.GradientMode.Vertical;
			this.finistPanel2.Location = new System.Drawing.Point(0, 0);
			this.finistPanel2.Name = "finistPanel2";
			this.finistPanel2.Size = new System.Drawing.Size(155, 25);
			this.finistPanel2.TabIndex = 1;
			// 
			// finistLabel1
			// 
			this.finistLabel1.AutoEllipsis = true;
			this.finistLabel1.BackColor = System.Drawing.Color.Transparent;
			this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabel1.Image = global::Sim.Controls.Properties.Resources.MsgInfo;
			this.finistLabel1.Location = new System.Drawing.Point(0, 0);
			this.finistLabel1.Name = "finistLabel1";
			this.finistLabel1.Size = new System.Drawing.Size(128, 24);
			this.finistLabel1.TabIndex = 1;
			this.finistLabel1.Text = "Детализация";
			// 
			// finistPopupButton1
			// 
			this.finistPopupButton1.BackColor = System.Drawing.Color.Transparent;
			this.finistPopupButton1.Dock = System.Windows.Forms.DockStyle.Right;
			this.finistPopupButton1.Image = ((System.Drawing.Image)(resources.GetObject("finistPopupButton1.Image")));
			this.finistPopupButton1.ImagePushed = ((System.Drawing.Image)(resources.GetObject("finistPopupButton1.ImagePushed")));
			this.finistPopupButton1.ImageRaised = ((System.Drawing.Image)(resources.GetObject("finistPopupButton1.ImageRaised")));
			this.finistPopupButton1.Location = new System.Drawing.Point(128, 0);
			this.finistPopupButton1.Name = "finistPopupButton1";
			this.finistPopupButton1.ShowBorder = false;
			this.finistPopupButton1.Size = new System.Drawing.Size(25, 24);
			this.finistPopupButton1.TabIndex = 0;
			this.finistPopupButton1.TabStop = false;
			this.finistPopupButton1.Click += new System.EventHandler(this.finistPopupButton1_Click);
			// 
			// finistContextMenu1
			// 
			this.finistContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.menuItemToDetail,
												this.menuItemShowDetail,
												this.toolStripSeparator1,
												this.menuItemFrozen});
			this.finistContextMenu1.Name = "finistContextMenu1";
			this.finistContextMenu1.Size = new System.Drawing.Size(253, 76);
			this.finistContextMenu1.Opening += new System.ComponentModel.CancelEventHandler(this.finistContextMenu1_Opening);
			// 
			// menuItemToDetail
			// 
			this.menuItemToDetail.Image = global::Sim.Controls.Properties.Resources.ColumnToDetail;
			this.menuItemToDetail.Name = "menuItemToDetail";
			this.menuItemToDetail.Size = new System.Drawing.Size(252, 22);
			this.menuItemToDetail.Text = "Переместить в детализацию";
			this.menuItemToDetail.Click += new System.EventHandler(this.HeaderMenuItems_Click);
			// 
			// menuItemShowDetail
			// 
			this.menuItemShowDetail.Image = global::Sim.Controls.Properties.Resources.ColumnDetail;
			this.menuItemShowDetail.Name = "menuItemShowDetail";
			this.menuItemShowDetail.Size = new System.Drawing.Size(252, 22);
			this.menuItemShowDetail.Text = "Отобразить панель детализации";
			this.menuItemShowDetail.Click += new System.EventHandler(this.HeaderMenuItems_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(249, 6);
			// 
			// menuItemFrozen
			// 
			this.menuItemFrozen.CheckOnClick = true;
			this.menuItemFrozen.Name = "menuItemFrozen";
			this.menuItemFrozen.Size = new System.Drawing.Size(252, 22);
			this.menuItemFrozen.Text = "Закрепить столбец";
			this.menuItemFrozen.Click += new System.EventHandler(this.menuItemFrozen_Click);
			// 
			// SimDataGridViewEx
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "SimDataGridViewEx";
			this.Size = new System.Drawing.Size(484, 425);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fdgv)).EndInit();
			this.finistPanel2.ResumeLayout(false);
			this.finistContextMenu1.ResumeLayout(false);
			this.ResumeLayout(false);

		}


		#endregion

		private SimPanel panelDetail;
		private SimDataGridView fdgv;
		private SimPanel finistPanel2;
		private SimLabel finistLabel1;
		private SimPopupButton finistPopupButton1;
		private SimContextMenu finistContextMenu1;
		private System.Windows.Forms.ToolStripMenuItem menuItemToDetail;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem menuItemShowDetail;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripMenuItem menuItemFrozen;
	}
}
