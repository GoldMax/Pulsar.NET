namespace Sim.AdminForms
{
 partial class FormSetAccesses
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
   if (disposing && (components != null))
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
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetAccesses));
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable2 = new Sim.Controls.SimToolStripColorTable();
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable3 = new Sim.Controls.SimToolStripColorTable();
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable4 = new Sim.Controls.SimToolStripColorTable();
   this.splitContainer1 = new System.Windows.Forms.SplitContainer();
   this.splitContainer2 = new System.Windows.Forms.SplitContainer();
   this.groupBox2 = new System.Windows.Forms.GroupBox();
   this.treeMainMenu = new Sim.Controls.SimTreeView();
   this.comboBoxMainMenu = new System.Windows.Forms.ToolStripComboBox();
   this.groupBox3 = new System.Windows.Forms.GroupBox();
   this.treeDivisions = new Sim.Controls.SimTreeView();
   this.comboBoxDivTree = new System.Windows.Forms.ToolStripComboBox();
   this.groupBox1 = new System.Windows.Forms.GroupBox();
   this.panelAccess = new System.Windows.Forms.Panel();
   this.fdgvACL = new Sim.Controls.SimDataGridView();
   this.toolStripAccess = new Sim.Controls.SimToolStrip();
   this.btnAddGroups = new System.Windows.Forms.ToolStripButton();
   this.btnAddUsers = new System.Windows.Forms.ToolStripButton();
   this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
   this.btnDel = new System.Windows.Forms.ToolStripButton();
   this.btnCancel = new System.Windows.Forms.ToolStripButton();
   this.btnSave = new System.Windows.Forms.ToolStripButton();
   this.toolStrip2 = new Sim.Controls.SimToolStrip();
   this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
   this.comboBoxUsers = new System.Windows.Forms.ToolStripComboBox();
   this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
   this.btnAddCurrent = new System.Windows.Forms.ToolStripButton();
   this.btnSetPass = new System.Windows.Forms.ToolStripButton();
   this.finistPanel1 = new Sim.Controls.SimPanel();
   this.PanelBack.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
   this.splitContainer1.Panel1.SuspendLayout();
   this.splitContainer1.Panel2.SuspendLayout();
   this.splitContainer1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
   this.splitContainer2.Panel1.SuspendLayout();
   this.splitContainer2.Panel2.SuspendLayout();
   this.splitContainer2.SuspendLayout();
   this.groupBox2.SuspendLayout();
   this.treeMainMenu.SuspendLayout();
   this.groupBox3.SuspendLayout();
   this.treeDivisions.SuspendLayout();
   this.groupBox1.SuspendLayout();
   this.panelAccess.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvACL)).BeginInit();
   this.toolStripAccess.SuspendLayout();
   this.toolStrip2.SuspendLayout();
   this.finistPanel1.SuspendLayout();
   this.SuspendLayout();
   // 
   // PanelBack
   // 
   this.PanelBack.Controls.Add(this.splitContainer1);
   this.PanelBack.Controls.Add(this.finistPanel1);
   this.PanelBack.Size = new System.Drawing.Size(871, 653);
   // 
   // splitContainer1
   // 
   this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
   this.splitContainer1.Location = new System.Drawing.Point(0, 27);
   this.splitContainer1.Name = "splitContainer1";
   this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
   // 
   // splitContainer1.Panel1
   // 
   this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
   // 
   // splitContainer1.Panel2
   // 
   this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
   this.splitContainer1.Panel2MinSize = 150;
   this.splitContainer1.Size = new System.Drawing.Size(871, 626);
   this.splitContainer1.SplitterDistance = 405;
   this.splitContainer1.TabIndex = 0;
   // 
   // splitContainer2
   // 
   this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
   this.splitContainer2.Location = new System.Drawing.Point(0, 0);
   this.splitContainer2.Name = "splitContainer2";
   // 
   // splitContainer2.Panel1
   // 
   this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
   // 
   // splitContainer2.Panel2
   // 
   this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
   this.splitContainer2.Size = new System.Drawing.Size(871, 405);
   this.splitContainer2.SplitterDistance = 408;
   this.splitContainer2.TabIndex = 1;
   // 
   // groupBox2
   // 
   this.groupBox2.Controls.Add(this.treeMainMenu);
   this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox2.Location = new System.Drawing.Point(0, 0);
   this.groupBox2.Name = "groupBox2";
   this.groupBox2.Size = new System.Drawing.Size(408, 405);
   this.groupBox2.TabIndex = 0;
   this.groupBox2.TabStop = false;
   this.groupBox2.Text = " Главное меню ";
   // 
   // treeMainMenu
   // 
   this.treeMainMenu.BackColor = System.Drawing.SystemColors.Control;
   this.treeMainMenu.CurrentView = Sim.Controls.TreeViewKind.FirstView;
   this.treeMainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
   this.treeMainMenu.FirstViewButtonEnabled = false;
   this.treeMainMenu.FirstViewButtonImage = ((System.Drawing.Image)(resources.GetObject("treeMainMenu.FirstViewButtonImage")));
   this.treeMainMenu.FirstViewButtonToolTipText = "Основной вид дерева";
   this.treeMainMenu.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.treeMainMenu.Location = new System.Drawing.Point(3, 17);
   // 
   // treeMainMenu.MainToolStrip
   // 
   this.treeMainMenu.MainToolStrip.BackColor = System.Drawing.SystemColors.Control;
   finistToolStripColorTable1.UseSystemColors = false;
   this.treeMainMenu.MainToolStrip.ColorTable = finistToolStripColorTable1;
   this.treeMainMenu.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.treeMainMenu.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comboBoxMainMenu});
   this.treeMainMenu.MainToolStrip.Location = new System.Drawing.Point(0, 0);
   this.treeMainMenu.MainToolStrip.Name = "MainToolStrip";
   this.treeMainMenu.MainToolStrip.Size = new System.Drawing.Size(402, 25);
   this.treeMainMenu.MainToolStrip.TabIndex = 0;
   this.treeMainMenu.MainToolStrip.Text = "toolStrip1";
   this.treeMainMenu.Name = "treeMainMenu";
   this.treeMainMenu.NodeTextPropName = "Caption";
   this.treeMainMenu.SecondViewButtonImage = ((System.Drawing.Image)(resources.GetObject("treeMainMenu.SecondViewButtonImage")));
   this.treeMainMenu.SecondViewButtonToolTipText = "Дополнительный вид дерева";
   this.treeMainMenu.Size = new System.Drawing.Size(402, 385);
   this.treeMainMenu.TabIndex = 0;
   this.treeMainMenu.ViewButtonsVisible = false;
   this.treeMainMenu.SelectedNodeChanged += new Sim.Controls.SimTreeView.SelectedNodeChangedHandler(this.treeMainMenu_SelectedNodeChanged);
   this.treeMainMenu.Enter += new System.EventHandler(this.treeMainMenu_Enter);
   // 
   // comboBoxMainMenu
   // 
   this.comboBoxMainMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
   this.comboBoxMainMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
   this.comboBoxMainMenu.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.comboBoxMainMenu.Name = "comboBoxMainMenu";
   this.comboBoxMainMenu.Size = new System.Drawing.Size(121, 25);
   this.comboBoxMainMenu.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsers_SelectedIndexChanged);
   // 
   // groupBox3
   // 
   this.groupBox3.Controls.Add(this.treeDivisions);
   this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox3.Location = new System.Drawing.Point(0, 0);
   this.groupBox3.Name = "groupBox3";
   this.groupBox3.Size = new System.Drawing.Size(459, 405);
   this.groupBox3.TabIndex = 0;
   this.groupBox3.TabStop = false;
   this.groupBox3.Text = " Организационное дерево ";
   // 
   // treeDivisions
   // 
   this.treeDivisions.BackColor = System.Drawing.SystemColors.Control;
   this.treeDivisions.CurrentView = Sim.Controls.TreeViewKind.FirstView;
   this.treeDivisions.Dock = System.Windows.Forms.DockStyle.Fill;
   this.treeDivisions.FirstViewButtonEnabled = false;
   this.treeDivisions.FirstViewButtonImage = ((System.Drawing.Image)(resources.GetObject("treeDivisions.FirstViewButtonImage")));
   this.treeDivisions.FirstViewButtonToolTipText = "Основной вид дерева";
   this.treeDivisions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.treeDivisions.Location = new System.Drawing.Point(3, 17);
   // 
   // treeDivisions.MainToolStrip
   // 
   this.treeDivisions.MainToolStrip.BackColor = System.Drawing.Color.Transparent;
   finistToolStripColorTable2.UseSystemColors = false;
   this.treeDivisions.MainToolStrip.ColorTable = finistToolStripColorTable2;
   this.treeDivisions.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.treeDivisions.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comboBoxDivTree});
   this.treeDivisions.MainToolStrip.Location = new System.Drawing.Point(0, 0);
   this.treeDivisions.MainToolStrip.Name = "MainToolStrip";
   this.treeDivisions.MainToolStrip.Size = new System.Drawing.Size(453, 25);
   this.treeDivisions.MainToolStrip.TabIndex = 0;
   this.treeDivisions.MainToolStrip.Text = "toolStrip1";
   this.treeDivisions.Name = "treeDivisions";
   this.treeDivisions.SecondViewButtonImage = ((System.Drawing.Image)(resources.GetObject("treeDivisions.SecondViewButtonImage")));
   this.treeDivisions.SecondViewButtonToolTipText = "Дополнительный вид дерева";
   this.treeDivisions.ShowRootLines = false;
   this.treeDivisions.Size = new System.Drawing.Size(453, 385);
   this.treeDivisions.TabIndex = 0;
   this.treeDivisions.ViewButtonsVisible = false;
   this.treeDivisions.SelectedNodeChanged += new Sim.Controls.SimTreeView.SelectedNodeChangedHandler(this.treeDivisions_SelectedNodeChanged);
   this.treeDivisions.Enter += new System.EventHandler(this.treeDivisions_Enter);
   // 
   // comboBoxDivTree
   // 
   this.comboBoxDivTree.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
   this.comboBoxDivTree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
   this.comboBoxDivTree.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.comboBoxDivTree.Name = "comboBoxDivTree";
   this.comboBoxDivTree.Size = new System.Drawing.Size(121, 25);
   this.comboBoxDivTree.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsers_SelectedIndexChanged);
   // 
   // groupBox1
   // 
   this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
   this.groupBox1.Controls.Add(this.panelAccess);
   this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox1.Location = new System.Drawing.Point(0, 0);
   this.groupBox1.Name = "groupBox1";
   this.groupBox1.Size = new System.Drawing.Size(871, 217);
   this.groupBox1.TabIndex = 0;
   this.groupBox1.TabStop = false;
   this.groupBox1.Text = " Действующие разрешения ";
   // 
   // panelAccess
   // 
   this.panelAccess.Controls.Add(this.fdgvACL);
   this.panelAccess.Controls.Add(this.toolStripAccess);
   this.panelAccess.Dock = System.Windows.Forms.DockStyle.Fill;
   this.panelAccess.Location = new System.Drawing.Point(3, 17);
   this.panelAccess.Name = "panelAccess";
   this.panelAccess.Size = new System.Drawing.Size(865, 197);
   this.panelAccess.TabIndex = 0;
   // 
   // fdgvACL
   // 
   this.fdgvACL.AllowUserToOrderColumns = false;
   this.fdgvACL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
   this.fdgvACL.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
   this.fdgvACL.Dock = System.Windows.Forms.DockStyle.Fill;
   this.fdgvACL.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
   this.fdgvACL.Location = new System.Drawing.Point(0, 25);
   this.fdgvACL.Name = "fdgvACL";
   this.fdgvACL.RowHeadersVisible = false;
   this.fdgvACL.RowTemplate.Height = 18;
   this.fdgvACL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
   this.fdgvACL.Size = new System.Drawing.Size(865, 172);
   this.fdgvACL.TabIndex = 3;
   this.fdgvACL.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.fdgvACL_CellContentClick);
   this.fdgvACL.SelectionChanged += new System.EventHandler(this.fdgvACL_SelectionChanged);
   // 
   // toolStripAccess
   // 
   finistToolStripColorTable3.UseSystemColors = false;
   this.toolStripAccess.ColorTable = finistToolStripColorTable3;
   this.toolStripAccess.Enabled = false;
   this.toolStripAccess.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.toolStripAccess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddGroups,
            this.btnAddUsers,
            this.toolStripSeparator1,
            this.btnDel,
            this.btnCancel,
            this.btnSave});
   this.toolStripAccess.Location = new System.Drawing.Point(0, 0);
   this.toolStripAccess.Name = "toolStripAccess";
   this.toolStripAccess.Size = new System.Drawing.Size(865, 25);
   this.toolStripAccess.TabIndex = 2;
   this.toolStripAccess.Text = "toolStrip1";
   // 
   // btnAddGroups
   // 
   this.btnAddGroups.Image = global::Sim.AdminForms.Properties.Resources.addChildGroup;
   this.btnAddGroups.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnAddGroups.Name = "btnAddGroups";
   this.btnAddGroups.Size = new System.Drawing.Size(117, 22);
   this.btnAddGroups.Text = "Добавить группы";
   this.btnAddGroups.ToolTipText = "Добавить группы пользователей к действующим разрешениям";
   this.btnAddGroups.Click += new System.EventHandler(this.btnAddGroups_Click);
   // 
   // btnAddUsers
   // 
   this.btnAddUsers.Image = global::Sim.AdminForms.Properties.Resources.addUserToGroup;
   this.btnAddUsers.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnAddUsers.Name = "btnAddUsers";
   this.btnAddUsers.Size = new System.Drawing.Size(157, 22);
   this.btnAddUsers.Text = "Добавить пользователей";
   this.btnAddUsers.ToolTipText = "Добавить пользователей к действующим разрешениям";
   this.btnAddUsers.Click += new System.EventHandler(this.btnAddUsers_Click);
   // 
   // toolStripSeparator1
   // 
   this.toolStripSeparator1.Name = "toolStripSeparator1";
   this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
   // 
   // btnDel
   // 
   this.btnDel.Enabled = false;
   this.btnDel.Image = global::Sim.AdminForms.Properties.Resources.DelGroup;
   this.btnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnDel.Name = "btnDel";
   this.btnDel.Size = new System.Drawing.Size(71, 22);
   this.btnDel.Text = "Удалить";
   this.btnDel.ToolTipText = "Удалить группы или пользователей из разрешений";
   this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
   // 
   // btnCancel
   // 
   this.btnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
   this.btnCancel.Enabled = false;
   this.btnCancel.Image = global::Sim.AdminForms.Properties.Resources.Stop;
   this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnCancel.Name = "btnCancel";
   this.btnCancel.Size = new System.Drawing.Size(77, 22);
   this.btnCancel.Text = "Отменить";
   this.btnCancel.ToolTipText = "Отменяет изменения";
   this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
   // 
   // btnSave
   // 
   this.btnSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
   this.btnSave.Enabled = false;
   this.btnSave.Image = global::Sim.AdminForms.Properties.Resources.Save;
   this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnSave.Name = "btnSave";
   this.btnSave.Size = new System.Drawing.Size(82, 22);
   this.btnSave.Text = "Сохранить";
   this.btnSave.ToolTipText = "Сохраняет изменения";
   this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
   // 
   // toolStrip2
   // 
   this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
   finistToolStripColorTable4.ToolStrip_Border = System.Drawing.Color.Transparent;
   finistToolStripColorTable4.UseSystemColors = false;
   this.toolStrip2.ColorTable = finistToolStripColorTable4;
   this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.comboBoxUsers,
            this.toolStripSeparator2,
            this.btnAddCurrent,
            this.btnSetPass});
   this.toolStrip2.Location = new System.Drawing.Point(0, 0);
   this.toolStrip2.Name = "toolStrip2";
   this.toolStrip2.ShowItemToolTips = false;
   this.toolStrip2.Size = new System.Drawing.Size(869, 25);
   this.toolStrip2.TabIndex = 0;
   this.toolStrip2.Text = "toolStrip2";
   // 
   // toolStripLabel1
   // 
   this.toolStripLabel1.Image = global::Sim.AdminForms.Properties.Resources.User;
   this.toolStripLabel1.Name = "toolStripLabel1";
   this.toolStripLabel1.Size = new System.Drawing.Size(95, 22);
   this.toolStripLabel1.Text = "Пользователь";
   // 
   // comboBoxUsers
   // 
   this.comboBoxUsers.AutoToolTip = true;
   this.comboBoxUsers.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.comboBoxUsers.MaxDropDownItems = 16;
   this.comboBoxUsers.Name = "comboBoxUsers";
   this.comboBoxUsers.Size = new System.Drawing.Size(250, 25);
   this.comboBoxUsers.Sorted = true;
   this.comboBoxUsers.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsers_SelectedIndexChanged);
   // 
   // toolStripSeparator2
   // 
   this.toolStripSeparator2.Name = "toolStripSeparator2";
   this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
   // 
   // btnAddCurrent
   // 
   this.btnAddCurrent.Enabled = false;
   this.btnAddCurrent.Image = global::Sim.AdminForms.Properties.Resources.Down;
   this.btnAddCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnAddCurrent.Name = "btnAddCurrent";
   this.btnAddCurrent.Size = new System.Drawing.Size(141, 22);
   this.btnAddCurrent.Text = "Добавить выбранного";
   this.btnAddCurrent.Click += new System.EventHandler(this.buttonAddCurrent_Click);
   // 
   // btnSetPass
   // 
   this.btnSetPass.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
   this.btnSetPass.Enabled = false;
   this.btnSetPass.Image = global::Sim.AdminForms.Properties.Resources.Keys;
   this.btnSetPass.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnSetPass.Name = "btnSetPass";
   this.btnSetPass.Size = new System.Drawing.Size(126, 22);
   this.btnSetPass.Text = "Установить пароль";
   this.btnSetPass.Click += new System.EventHandler(this.btnSetPass_Click);
   // 
   // finistPanel1
   // 
   this.finistPanel1.BackColor = System.Drawing.SystemColors.Info;
   this.finistPanel1.BackColor2 = System.Drawing.SystemColors.Control;
   this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.finistPanel1.Controls.Add(this.toolStrip2);
   this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Top;
   this.finistPanel1.Location = new System.Drawing.Point(0, 0);
   this.finistPanel1.Name = "finistPanel1";
   this.finistPanel1.Size = new System.Drawing.Size(871, 27);
   this.finistPanel1.TabIndex = 1;
   // 
   // frmSetAccesses
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.ClientSize = new System.Drawing.Size(871, 653);
   this.Name = "frmSetAccesses";
   this.Text = "Назначение доступов";
   this.PanelBack.ResumeLayout(false);
   this.splitContainer1.Panel1.ResumeLayout(false);
   this.splitContainer1.Panel2.ResumeLayout(false);
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
   this.splitContainer1.ResumeLayout(false);
   this.splitContainer2.Panel1.ResumeLayout(false);
   this.splitContainer2.Panel2.ResumeLayout(false);
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
   this.splitContainer2.ResumeLayout(false);
   this.groupBox2.ResumeLayout(false);
   this.treeMainMenu.ResumeLayout(false);
   this.treeMainMenu.PerformLayout();
   this.groupBox3.ResumeLayout(false);
   this.treeDivisions.ResumeLayout(false);
   this.treeDivisions.PerformLayout();
   this.groupBox1.ResumeLayout(false);
   this.panelAccess.ResumeLayout(false);
   this.panelAccess.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvACL)).EndInit();
   this.toolStripAccess.ResumeLayout(false);
   this.toolStripAccess.PerformLayout();
   this.toolStrip2.ResumeLayout(false);
   this.toolStrip2.PerformLayout();
   this.finistPanel1.ResumeLayout(false);
   this.finistPanel1.PerformLayout();
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.SplitContainer splitContainer1;
  private System.Windows.Forms.GroupBox groupBox1;
  private System.Windows.Forms.SplitContainer splitContainer2;
  private System.Windows.Forms.GroupBox groupBox2;
  private Sim.Controls.SimTreeView treeMainMenu;
  private System.Windows.Forms.GroupBox groupBox3;
  private Sim.Controls.SimTreeView treeDivisions;
  private Sim.Controls.SimToolStrip toolStrip2;
  private System.Windows.Forms.ToolStripLabel toolStripLabel1;
  private System.Windows.Forms.ToolStripComboBox comboBoxUsers;
  private System.Windows.Forms.Panel panelAccess;
  private Sim.Controls.SimDataGridView fdgvACL;
  private Sim.Controls.SimToolStrip toolStripAccess;
  private System.Windows.Forms.ToolStripButton btnAddGroups;
  private System.Windows.Forms.ToolStripButton btnAddUsers;
  private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
  private System.Windows.Forms.ToolStripButton btnDel;
  private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
  private System.Windows.Forms.ToolStripButton btnAddCurrent;
  private System.Windows.Forms.ToolStripComboBox comboBoxMainMenu;
  private System.Windows.Forms.ToolStripComboBox comboBoxDivTree;
  private Controls.SimPanel finistPanel1;
  private System.Windows.Forms.ToolStripButton btnSetPass;
  private System.Windows.Forms.ToolStripButton btnCancel;
  private System.Windows.Forms.ToolStripButton btnSave;

 }
}
