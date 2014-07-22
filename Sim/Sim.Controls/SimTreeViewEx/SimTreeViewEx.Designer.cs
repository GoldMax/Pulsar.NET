namespace Sim.Controls
{
	#pragma warning disable 1591
	partial class SimTreeViewEx
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimTreeViewEx));
			Sim.Controls.SimToolStripColorTable finistToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
			this.chImageList = new System.Windows.Forms.ImageList(this.components);
			this.mainToolStrip = new Sim.Controls.SimToolStrip();
			this.btnFirstView = new System.Windows.Forms.ToolStripButton();
			this.btnSecondView = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnFind = new System.Windows.Forms.ToolStripDropDownButton();
			this.contextMenuStripFind = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTextBoxID = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTextBoxName = new System.Windows.Forms.ToolStripTextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pictureBoxFilter = new System.Windows.Forms.PictureBox();
			this.fpMain = new Sim.Controls.SimPanel();
			this.listBoxFindResult = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelFindQuery = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.buttonFindClose = new Sim.Controls.SimPopupButton();
			this.buttonFindStop = new Sim.Controls.SimButton();
			this.pictureBoxFind = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.treeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemCount = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemColAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemExAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mainToolStrip.SuspendLayout();
			this.contextMenuStripFind.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilter)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFind)).BeginInit();
			this.treeContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// chImageList
			// 
			this.chImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("chImageList.ImageStream")));
			this.chImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.chImageList.Images.SetKeyName(0, "CheckEmpty.ico");
			this.chImageList.Images.SetKeyName(1, "CheckFill.ico");
			this.chImageList.Images.SetKeyName(2, "CheckMiddle.ico");
			// 
			// mainToolStrip
			// 
			finistToolStripColorTable1.UseSystemColors = false;
			this.mainToolStrip.ColorTable = finistToolStripColorTable1;
			this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.btnFirstView,
												this.btnSecondView,
												this.toolStripSeparator1,
												this.btnFind});
			this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(280, 25);
			this.mainToolStrip.TabIndex = 0;
			this.mainToolStrip.Text = "toolStrip1";
			// 
			// btnFirstView
			// 
			this.btnFirstView.Checked = true;
			this.btnFirstView.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnFirstView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnFirstView.Image = global::Sim.Controls.Properties.Resources.TreeView;
			this.btnFirstView.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFirstView.Name = "btnFirstView";
			this.btnFirstView.Size = new System.Drawing.Size(23, 22);
			this.btnFirstView.Text = "toolStripButton1";
			this.btnFirstView.ToolTipText = "Основной вид дерева";
			this.btnFirstView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// btnSecondView
			// 
			this.btnSecondView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSecondView.Enabled = false;
			this.btnSecondView.Image = global::Sim.Controls.Properties.Resources.TreeView1;
			this.btnSecondView.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSecondView.Name = "btnSecondView";
			this.btnSecondView.Size = new System.Drawing.Size(23, 22);
			this.btnSecondView.Text = "toolStripButton2";
			this.btnSecondView.ToolTipText = "Дополнительный вид дерева";
			this.btnSecondView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnFind
			// 
			this.btnFind.AutoToolTip = false;
			this.btnFind.DropDown = this.contextMenuStripFind;
			this.btnFind.Enabled = false;
			this.btnFind.Image = global::Sim.Controls.Properties.Resources.Find;
			this.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(71, 22);
			this.btnFind.Text = "Поиск";
			this.btnFind.DropDownOpened += new System.EventHandler(this.toolStripDropDownButtonFind_DropDownOpened);
			// 
			// contextMenuStripFind
			// 
			this.contextMenuStripFind.AutoSize = false;
			this.contextMenuStripFind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.toolStripMenuItem1,
												this.toolStripTextBoxID,
												this.toolStripSeparator2,
												this.toolStripMenuItem2,
												this.toolStripTextBoxName});
			this.contextMenuStripFind.Name = "contextMenuStripFind";
			this.contextMenuStripFind.OwnerItem = this.btnFind;
			this.contextMenuStripFind.ShowImageMargin = false;
			this.contextMenuStripFind.Size = new System.Drawing.Size(170, 105);
			this.contextMenuStripFind.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuStripFind_Closed);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Enabled = false;
			this.toolStripMenuItem1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
			this.toolStripMenuItem1.Text = "Поиск по коду (ID)";
			// 
			// toolStripTextBoxID
			// 
			this.toolStripTextBoxID.AutoSize = false;
			this.toolStripTextBoxID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toolStripTextBoxID.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.toolStripTextBoxID.Name = "toolStripTextBoxID";
			this.toolStripTextBoxID.Size = new System.Drawing.Size(150, 23);
			this.toolStripTextBoxID.Tag = "id";
			this.toolStripTextBoxID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox_KeyPress);
			this.toolStripTextBoxID.TextChanged += new System.EventHandler(this.toolStripTextBox_TextChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Enabled = false;
			this.toolStripMenuItem2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 22);
			this.toolStripMenuItem2.Text = "Поиск по наименованию";
			// 
			// toolStripTextBoxName
			// 
			this.toolStripTextBoxName.AutoSize = false;
			this.toolStripTextBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toolStripTextBoxName.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.toolStripTextBoxName.Name = "toolStripTextBoxName";
			this.toolStripTextBoxName.Size = new System.Drawing.Size(150, 23);
			this.toolStripTextBoxName.Tag = "name";
			this.toolStripTextBoxName.ToolTipText = "Допускаются символы подстановки:\r\n* - ни одного, один или несколько символов\r\n? -" +
				" один любой символ";
			this.toolStripTextBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox_KeyPress);
			this.toolStripTextBoxName.TextChanged += new System.EventHandler(this.toolStripTextBox_TextChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.pictureBoxFilter);
			this.splitContainer1.Panel1.Controls.Add(this.fpMain);
			this.splitContainer1.Panel1.MouseLeave += new System.EventHandler(this.tree_MouseLeave);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.splitContainer1.Panel2.Controls.Add(this.listBoxFindResult);
			this.splitContainer1.Panel2.Controls.Add(this.panel3);
			this.splitContainer1.Panel2.Controls.Add(this.panel1);
			this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(1);
			this.splitContainer1.Size = new System.Drawing.Size(280, 401);
			this.splitContainer1.SplitterDistance = 262;
			this.splitContainer1.SplitterIncrement = 13;
			this.splitContainer1.TabIndex = 3;
			this.splitContainer1.MouseLeave += new System.EventHandler(this.tree_MouseLeave);
			// 
			// pictureBoxFilter
			// 
			this.pictureBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxFilter.BackColor = System.Drawing.SystemColors.Info;
			this.pictureBoxFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.pictureBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxFilter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFilter.Image")));
			this.pictureBoxFilter.Location = new System.Drawing.Point(236, 3);
			this.pictureBoxFilter.Name = "pictureBoxFilter";
			this.pictureBoxFilter.Padding = new System.Windows.Forms.Padding(2);
			this.pictureBoxFilter.Size = new System.Drawing.Size(22, 22);
			this.pictureBoxFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBoxFilter.TabIndex = 2;
			this.pictureBoxFilter.TabStop = false;
			this.pictureBoxFilter.Visible = false;
			this.pictureBoxFilter.MouseHover += new System.EventHandler(this.pictureBoxFilter_MouseHover);
			// 
			// fpMain
			// 
			this.fpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpMain.Location = new System.Drawing.Point(0, 0);
			this.fpMain.Name = "fpMain";
			this.fpMain.Size = new System.Drawing.Size(280, 262);
			this.fpMain.TabIndex = 3;
			// 
			// listBoxFindResult
			// 
			this.listBoxFindResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBoxFindResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxFindResult.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listBoxFindResult.Location = new System.Drawing.Point(1, 25);
			this.listBoxFindResult.Name = "listBoxFindResult";
			this.listBoxFindResult.Size = new System.Drawing.Size(278, 109);
			this.listBoxFindResult.TabIndex = 3;
			this.listBoxFindResult.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxFindResult_DrawItem);
			this.listBoxFindResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxFindResult_MouseDoubleClick);
			this.listBoxFindResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBoxFindResult_MouseMove);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(1, 24);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(278, 1);
			this.panel3.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.labelFindQuery);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.buttonFindStop);
			this.panel1.Controls.Add(this.pictureBoxFind);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(1, 1);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(278, 23);
			this.panel1.TabIndex = 0;
			// 
			// labelFindQuery
			// 
			this.labelFindQuery.AutoEllipsis = true;
			this.labelFindQuery.BackColor = System.Drawing.SystemColors.Info;
			this.labelFindQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelFindQuery.Location = new System.Drawing.Point(74, 0);
			this.labelFindQuery.Name = "labelFindQuery";
			this.labelFindQuery.Size = new System.Drawing.Size(181, 23);
			this.labelFindQuery.TabIndex = 3;
			this.labelFindQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Info;
			this.panel2.Controls.Add(this.buttonFindClose);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(255, 0);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(4);
			this.panel2.Size = new System.Drawing.Size(23, 23);
			this.panel2.TabIndex = 2;
			// 
			// buttonFindClose
			// 
			this.buttonFindClose.Image = ((System.Drawing.Image)(resources.GetObject("buttonFindClose.Image")));
			this.buttonFindClose.ImagePushed = ((System.Drawing.Image)(resources.GetObject("buttonFindClose.ImagePushed")));
			this.buttonFindClose.ImageRaised = ((System.Drawing.Image)(resources.GetObject("buttonFindClose.ImageRaised")));
			this.buttonFindClose.Location = new System.Drawing.Point(5, 4);
			this.buttonFindClose.MaximumSize = new System.Drawing.Size(14, 14);
			this.buttonFindClose.MinimumSize = new System.Drawing.Size(14, 14);
			this.buttonFindClose.Name = "buttonFindClose";
			this.buttonFindClose.Size = new System.Drawing.Size(14, 14);
			this.buttonFindClose.TabIndex = 0;
			this.buttonFindClose.ToolTip = null;
			this.buttonFindClose.Click += new System.EventHandler(this.buttonFindClose_Click);
			// 
			// buttonFindStop
			// 
			this.buttonFindStop.Dock = System.Windows.Forms.DockStyle.Left;
			this.buttonFindStop.Enabled = false;
			this.buttonFindStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonFindStop.Image")));
			this.buttonFindStop.Location = new System.Drawing.Point(16, 0);
			this.buttonFindStop.Name = "buttonFindStop";
			this.buttonFindStop.Size = new System.Drawing.Size(58, 23);
			this.buttonFindStop.TabIndex = 0;
			this.buttonFindStop.Text = "Стоп";
			this.buttonFindStop.Click += new System.EventHandler(this.buttonFindStop_Click);
			// 
			// pictureBoxFind
			// 
			this.pictureBoxFind.BackColor = System.Drawing.SystemColors.Info;
			this.pictureBoxFind.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBoxFind.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFind.Image")));
			this.pictureBoxFind.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxFind.Name = "pictureBoxFind";
			this.pictureBoxFind.Size = new System.Drawing.Size(16, 23);
			this.pictureBoxFind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxFind.TabIndex = 1;
			this.pictureBoxFind.TabStop = false;
			this.pictureBoxFind.Visible = false;
			// 
			// treeContextMenuStrip
			// 
			this.treeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.toolStripMenuItemCount,
												this.toolStripSeparator3,
												this.toolStripMenuItemColAll,
												this.toolStripMenuItemExAll});
			this.treeContextMenuStrip.Name = "contextMenuStrip1";
			this.treeContextMenuStrip.Size = new System.Drawing.Size(255, 98);
			this.treeContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.treeContextMenuStrip_ItemClicked);
			// 
			// toolStripMenuItemCount
			// 
			this.toolStripMenuItemCount.Image = global::Sim.Controls.Properties.Resources.TreeViewCount;
			this.toolStripMenuItemCount.Name = "toolStripMenuItemCount";
			this.toolStripMenuItemCount.Size = new System.Drawing.Size(254, 22);
			this.toolStripMenuItemCount.Text = "Количество конечных элементов";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(251, 6);
			// 
			// toolStripMenuItemColAll
			// 
			this.toolStripMenuItemColAll.Image = global::Sim.Controls.Properties.Resources.TreeView1;
			this.toolStripMenuItemColAll.Name = "toolStripMenuItemColAll";
			this.toolStripMenuItemColAll.ShortcutKeyDisplayString = "Shift - Minus";
			this.toolStripMenuItemColAll.Size = new System.Drawing.Size(254, 22);
			this.toolStripMenuItemColAll.Text = "Свернуть ветвь";
			// 
			// toolStripMenuItemExAll
			// 
			this.toolStripMenuItemExAll.Image = global::Sim.Controls.Properties.Resources.TreeView3;
			this.toolStripMenuItemExAll.Name = "toolStripMenuItemExAll";
			this.toolStripMenuItemExAll.ShortcutKeyDisplayString = "Shift - Plus";
			this.toolStripMenuItemExAll.Size = new System.Drawing.Size(254, 22);
			this.toolStripMenuItemExAll.Text = "Развернуть ветвь";
			// 
			// SimTreeViewEx
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ContextMenuStrip = this.treeContextMenuStrip;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.mainToolStrip);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "SimTreeViewEx";
			this.Size = new System.Drawing.Size(280, 426);
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			this.contextMenuStripFind.ResumeLayout(false);
			this.contextMenuStripFind.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilter)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFind)).EndInit();
			this.treeContextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ImageList chImageList;
		private SimToolStrip mainToolStrip;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.PictureBox pictureBoxFilter;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripFind;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBoxID;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBoxName;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBoxFindResult;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labelFindQuery;
		private Sim.Controls.SimButton buttonFindStop;
		private System.Windows.Forms.PictureBox pictureBoxFind;
		private System.Windows.Forms.Panel panel2;
		private SimPopupButton buttonFindClose;
		private System.Windows.Forms.ToolTip toolTip1;
		public System.Windows.Forms.ToolStripButton btnFirstView;
		public System.Windows.Forms.ToolStripButton btnSecondView;
		public System.Windows.Forms.ToolStripDropDownButton btnFind;
		private SimPanel fpMain;
		private System.Windows.Forms.ContextMenuStrip treeContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCount;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemColAll;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExAll;
	}
	#pragma warning restore 1591
}
