namespace Sim.AdminForms
{
 partial class FormSecurityGroups
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
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable2 = new Sim.Controls.SimToolStripColorTable();
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable3 = new Sim.Controls.SimToolStripColorTable();
   this.splitContainer1 = new System.Windows.Forms.SplitContainer();
   this.groupBox1 = new System.Windows.Forms.GroupBox();
   this.fdgvGroups = new Sim.Controls.SimDataGridView();
   this.groupsImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
   this.groupsNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.groupsDescColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.panel1 = new System.Windows.Forms.Panel();
   this.flSID = new Sim.Controls.SimLabel();
   this.label2 = new System.Windows.Forms.Label();
   this.labelGroupName = new System.Windows.Forms.Label();
   this.toolStripGroups = new Sim.Controls.SimToolStrip();
   this.toolStripButtonAddGroup = new System.Windows.Forms.ToolStripButton();
   this.btnDelGroup = new System.Windows.Forms.ToolStripButton();
   this.btnRenameGroup = new System.Windows.Forms.ToolStripButton();
   this.splitContainer3 = new System.Windows.Forms.SplitContainer();
   this.groupBox2 = new System.Windows.Forms.GroupBox();
   this.fdgvParents = new Sim.Controls.SimDataGridView();
   this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
   this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.toolStripParentGroups = new Sim.Controls.SimToolStrip();
   this.btnParentAdd = new System.Windows.Forms.ToolStripButton();
   this.btnParentDel = new System.Windows.Forms.ToolStripButton();
   this.groupBox3 = new System.Windows.Forms.GroupBox();
   this.fdgvChilds = new Sim.Controls.SimDataGridView();
   this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
   this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.toolStripChildGroups = new Sim.Controls.SimToolStrip();
   this.btnChildGroupAdd = new System.Windows.Forms.ToolStripButton();
   this.btnChildUserAdd = new System.Windows.Forms.ToolStripButton();
   this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
   this.btnChildDel = new System.Windows.Forms.ToolStripButton();
   this.PanelBack.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
   this.splitContainer1.Panel1.SuspendLayout();
   this.splitContainer1.Panel2.SuspendLayout();
   this.splitContainer1.SuspendLayout();
   this.groupBox1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvGroups)).BeginInit();
   this.panel1.SuspendLayout();
   this.toolStripGroups.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
   this.splitContainer3.Panel1.SuspendLayout();
   this.splitContainer3.Panel2.SuspendLayout();
   this.splitContainer3.SuspendLayout();
   this.groupBox2.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvParents)).BeginInit();
   this.toolStripParentGroups.SuspendLayout();
   this.groupBox3.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvChilds)).BeginInit();
   this.toolStripChildGroups.SuspendLayout();
   this.SuspendLayout();
   // 
   // PanelBack
   // 
   this.PanelBack.Controls.Add(this.splitContainer1);
   this.PanelBack.Size = new System.Drawing.Size(813, 578);
   // 
   // splitContainer1
   // 
   this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
   this.splitContainer1.Location = new System.Drawing.Point(0, 0);
   this.splitContainer1.Name = "splitContainer1";
   // 
   // splitContainer1.Panel1
   // 
   this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
   this.splitContainer1.Panel1MinSize = 100;
   // 
   // splitContainer1.Panel2
   // 
   this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
   this.splitContainer1.Panel2MinSize = 100;
   this.splitContainer1.Size = new System.Drawing.Size(813, 578);
   this.splitContainer1.SplitterDistance = 354;
   this.splitContainer1.TabIndex = 0;
   // 
   // groupBox1
   // 
   this.groupBox1.Controls.Add(this.fdgvGroups);
   this.groupBox1.Controls.Add(this.panel1);
   this.groupBox1.Controls.Add(this.toolStripGroups);
   this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox1.Location = new System.Drawing.Point(0, 0);
   this.groupBox1.Name = "groupBox1";
   this.groupBox1.Size = new System.Drawing.Size(354, 578);
   this.groupBox1.TabIndex = 0;
   this.groupBox1.TabStop = false;
   this.groupBox1.Text = "Группы безопасности";
   // 
   // fdgvGroups
   // 
   this.fdgvGroups.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
   this.fdgvGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupsImageColumn,
            this.groupsNameColumn,
            this.groupsDescColumn});
   this.fdgvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
   this.fdgvGroups.Location = new System.Drawing.Point(3, 42);
   this.fdgvGroups.MultiSelect = false;
   this.fdgvGroups.Name = "fdgvGroups";
   this.fdgvGroups.RowHeadersVisible = false;
   this.fdgvGroups.RowTemplate.Height = 18;
   this.fdgvGroups.Size = new System.Drawing.Size(348, 493);
   this.fdgvGroups.TabIndex = 3;
   this.fdgvGroups.VirtualMode = true;
   this.fdgvGroups.SelectionChanged += new System.EventHandler(this.fdgvGroups_SelectionChanged);
   // 
   // groupsImageColumn
   // 
   this.groupsImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
   this.groupsImageColumn.HeaderText = "";
   this.groupsImageColumn.Image = global::Sim.AdminForms.Properties.Resources.Group;
   this.groupsImageColumn.MinimumWidth = 20;
   this.groupsImageColumn.Name = "groupsImageColumn";
   this.groupsImageColumn.ReadOnly = true;
   this.groupsImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
   this.groupsImageColumn.Width = 20;
   // 
   // groupsNameColumn
   // 
   this.groupsNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
   this.groupsNameColumn.DataPropertyName = "Name";
   this.groupsNameColumn.HeaderText = "Группа";
   this.groupsNameColumn.Name = "groupsNameColumn";
   this.groupsNameColumn.ReadOnly = true;
   // 
   // groupsDescColumn
   // 
   this.groupsDescColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
   this.groupsDescColumn.DataPropertyName = "Description";
   this.groupsDescColumn.HeaderText = "Описание";
   this.groupsDescColumn.Name = "groupsDescColumn";
   this.groupsDescColumn.ReadOnly = true;
   // 
   // panel1
   // 
   this.panel1.Controls.Add(this.flSID);
   this.panel1.Controls.Add(this.label2);
   this.panel1.Controls.Add(this.labelGroupName);
   this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
   this.panel1.Location = new System.Drawing.Point(3, 535);
   this.panel1.Name = "panel1";
   this.panel1.Size = new System.Drawing.Size(348, 40);
   this.panel1.TabIndex = 4;
   // 
   // flSID
   // 
   this.flSID.AutoEllipsis = true;
   this.flSID.Dock = System.Windows.Forms.DockStyle.Fill;
   this.flSID.Location = new System.Drawing.Point(28, 22);
   this.flSID.Name = "flSID";
   this.flSID.Size = new System.Drawing.Size(320, 18);
   this.flSID.TabIndex = 2;
   this.flSID.TextAlign = System.Drawing.ContentAlignment.TopLeft;
   // 
   // label2
   // 
   this.label2.AutoSize = true;
   this.label2.Dock = System.Windows.Forms.DockStyle.Left;
   this.label2.Location = new System.Drawing.Point(0, 22);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(28, 13);
   this.label2.TabIndex = 1;
   this.label2.Text = "SID:";
   // 
   // labelGroupName
   // 
   this.labelGroupName.Dock = System.Windows.Forms.DockStyle.Top;
   this.labelGroupName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.labelGroupName.Location = new System.Drawing.Point(0, 0);
   this.labelGroupName.Name = "labelGroupName";
   this.labelGroupName.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
   this.labelGroupName.Size = new System.Drawing.Size(348, 22);
   this.labelGroupName.TabIndex = 0;
   // 
   // toolStripGroups
   // 
   finistToolStripColorTable1.UseSystemColors = false;
   this.toolStripGroups.ColorTable = finistToolStripColorTable1;
   this.toolStripGroups.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.toolStripGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddGroup,
            this.btnDelGroup,
            this.btnRenameGroup});
   this.toolStripGroups.Location = new System.Drawing.Point(3, 17);
   this.toolStripGroups.Name = "toolStripGroups";
   this.toolStripGroups.Size = new System.Drawing.Size(348, 25);
   this.toolStripGroups.TabIndex = 2;
   this.toolStripGroups.Text = "toolStrip1";
   // 
   // toolStripButtonAddGroup
   // 
   this.toolStripButtonAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.toolStripButtonAddGroup.Image = global::Sim.AdminForms.Properties.Resources.AddGroup;
   this.toolStripButtonAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.toolStripButtonAddGroup.Name = "toolStripButtonAddGroup";
   this.toolStripButtonAddGroup.Size = new System.Drawing.Size(23, 22);
   this.toolStripButtonAddGroup.ToolTipText = "Создать новую группу безопасности";
   this.toolStripButtonAddGroup.Click += new System.EventHandler(this.toolStripButtonAddGroup_Click);
   // 
   // btnDelGroup
   // 
   this.btnDelGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.btnDelGroup.Enabled = false;
   this.btnDelGroup.Image = global::Sim.AdminForms.Properties.Resources.DelGroup;
   this.btnDelGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnDelGroup.Name = "btnDelGroup";
   this.btnDelGroup.Size = new System.Drawing.Size(23, 22);
   this.btnDelGroup.Text = "toolStripButton2";
   this.btnDelGroup.ToolTipText = "Удалить группу безопасности";
   this.btnDelGroup.Click += new System.EventHandler(this.btnDelGroup_Click);
   // 
   // btnRenameGroup
   // 
   this.btnRenameGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.btnRenameGroup.Enabled = false;
   this.btnRenameGroup.Image = global::Sim.AdminForms.Properties.Resources.Rename;
   this.btnRenameGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnRenameGroup.Name = "btnRenameGroup";
   this.btnRenameGroup.Size = new System.Drawing.Size(23, 22);
   this.btnRenameGroup.Text = "toolStripButton1";
   this.btnRenameGroup.ToolTipText = "Изменить имя или описание группы";
   this.btnRenameGroup.Click += new System.EventHandler(this.btnRenameGroup_Click);
   // 
   // splitContainer3
   // 
   this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
   this.splitContainer3.Location = new System.Drawing.Point(0, 0);
   this.splitContainer3.Name = "splitContainer3";
   this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
   // 
   // splitContainer3.Panel1
   // 
   this.splitContainer3.Panel1.Controls.Add(this.groupBox2);
   // 
   // splitContainer3.Panel2
   // 
   this.splitContainer3.Panel2.Controls.Add(this.groupBox3);
   this.splitContainer3.Size = new System.Drawing.Size(455, 578);
   this.splitContainer3.SplitterDistance = 273;
   this.splitContainer3.TabIndex = 0;
   // 
   // groupBox2
   // 
   this.groupBox2.Controls.Add(this.fdgvParents);
   this.groupBox2.Controls.Add(this.toolStripParentGroups);
   this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox2.Location = new System.Drawing.Point(0, 0);
   this.groupBox2.Name = "groupBox2";
   this.groupBox2.Size = new System.Drawing.Size(455, 273);
   this.groupBox2.TabIndex = 0;
   this.groupBox2.TabStop = false;
   this.groupBox2.Text = "Родительские группы";
   // 
   // fdgvParents
   // 
   this.fdgvParents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
   this.fdgvParents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewImageColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
   this.fdgvParents.Dock = System.Windows.Forms.DockStyle.Fill;
   this.fdgvParents.Location = new System.Drawing.Point(3, 42);
   this.fdgvParents.Name = "fdgvParents";
   this.fdgvParents.RowHeadersVisible = false;
   this.fdgvParents.RowTemplate.Height = 18;
   this.fdgvParents.Size = new System.Drawing.Size(449, 228);
   this.fdgvParents.TabIndex = 4;
   this.fdgvParents.VirtualMode = true;
   this.fdgvParents.SelectionChanged += new System.EventHandler(this.fdgvParents_SelectionChanged);
   // 
   // dataGridViewImageColumn1
   // 
   this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
   this.dataGridViewImageColumn1.HeaderText = "";
   this.dataGridViewImageColumn1.Image = global::Sim.AdminForms.Properties.Resources.Group;
   this.dataGridViewImageColumn1.MinimumWidth = 20;
   this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
   this.dataGridViewImageColumn1.ReadOnly = true;
   this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
   this.dataGridViewImageColumn1.Width = 20;
   // 
   // dataGridViewTextBoxColumn1
   // 
   this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
   this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
   this.dataGridViewTextBoxColumn1.HeaderText = "Группа";
   this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
   this.dataGridViewTextBoxColumn1.ReadOnly = true;
   // 
   // dataGridViewTextBoxColumn2
   // 
   this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
   this.dataGridViewTextBoxColumn2.DataPropertyName = "Description";
   this.dataGridViewTextBoxColumn2.HeaderText = "Описание";
   this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
   this.dataGridViewTextBoxColumn2.ReadOnly = true;
   // 
   // toolStripParentGroups
   // 
   finistToolStripColorTable2.UseSystemColors = false;
   this.toolStripParentGroups.ColorTable = finistToolStripColorTable2;
   this.toolStripParentGroups.Enabled = false;
   this.toolStripParentGroups.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.toolStripParentGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnParentAdd,
            this.btnParentDel});
   this.toolStripParentGroups.Location = new System.Drawing.Point(3, 17);
   this.toolStripParentGroups.Name = "toolStripParentGroups";
   this.toolStripParentGroups.Size = new System.Drawing.Size(449, 25);
   this.toolStripParentGroups.TabIndex = 0;
   this.toolStripParentGroups.Text = "toolStrip2";
   // 
   // btnParentAdd
   // 
   this.btnParentAdd.AutoToolTip = false;
   this.btnParentAdd.Image = global::Sim.AdminForms.Properties.Resources.InGroup;
   this.btnParentAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnParentAdd.Name = "btnParentAdd";
   this.btnParentAdd.Size = new System.Drawing.Size(77, 22);
   this.btnParentAdd.Text = "Добавить";
   this.btnParentAdd.ToolTipText = "Добавляет выбранную слева группу в другие группы";
   this.btnParentAdd.Click += new System.EventHandler(this.btnParentAdd_Click);
   // 
   // btnParentDel
   // 
   this.btnParentDel.AutoToolTip = false;
   this.btnParentDel.Enabled = false;
   this.btnParentDel.Image = global::Sim.AdminForms.Properties.Resources.FromGroup;
   this.btnParentDel.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnParentDel.Name = "btnParentDel";
   this.btnParentDel.Size = new System.Drawing.Size(84, 22);
   this.btnParentDel.Text = "Исключить";
   this.btnParentDel.ToolTipText = "Исключает выбранную слева группу из выбранных снизу групп.";
   this.btnParentDel.Click += new System.EventHandler(this.btnParentDel_Click);
   // 
   // groupBox3
   // 
   this.groupBox3.Controls.Add(this.fdgvChilds);
   this.groupBox3.Controls.Add(this.toolStripChildGroups);
   this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox3.Location = new System.Drawing.Point(0, 0);
   this.groupBox3.Name = "groupBox3";
   this.groupBox3.Size = new System.Drawing.Size(455, 301);
   this.groupBox3.TabIndex = 0;
   this.groupBox3.TabStop = false;
   this.groupBox3.Text = "Дочерние группы и пользователи";
   // 
   // fdgvChilds
   // 
   this.fdgvChilds.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
   this.fdgvChilds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewImageColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
   this.fdgvChilds.Dock = System.Windows.Forms.DockStyle.Fill;
   this.fdgvChilds.Location = new System.Drawing.Point(3, 42);
   this.fdgvChilds.Name = "fdgvChilds";
   this.fdgvChilds.RowHeadersVisible = false;
   this.fdgvChilds.RowTemplate.Height = 18;
   this.fdgvChilds.Size = new System.Drawing.Size(449, 256);
   this.fdgvChilds.TabIndex = 5;
   this.fdgvChilds.VirtualMode = true;
   this.fdgvChilds.SelectionChanged += new System.EventHandler(this.fdgvChilds_SelectionChanged);
   // 
   // dataGridViewImageColumn2
   // 
   this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
   this.dataGridViewImageColumn2.DataPropertyName = "Image";
   this.dataGridViewImageColumn2.HeaderText = "";
   this.dataGridViewImageColumn2.MinimumWidth = 20;
   this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
   this.dataGridViewImageColumn2.ReadOnly = true;
   this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
   this.dataGridViewImageColumn2.Width = 20;
   // 
   // dataGridViewTextBoxColumn3
   // 
   this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
   this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
   this.dataGridViewTextBoxColumn3.HeaderText = "Группа";
   this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
   this.dataGridViewTextBoxColumn3.ReadOnly = true;
   // 
   // dataGridViewTextBoxColumn4
   // 
   this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
   this.dataGridViewTextBoxColumn4.DataPropertyName = "Desc";
   this.dataGridViewTextBoxColumn4.HeaderText = "Описание";
   this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
   this.dataGridViewTextBoxColumn4.ReadOnly = true;
   // 
   // toolStripChildGroups
   // 
   finistToolStripColorTable3.UseSystemColors = false;
   this.toolStripChildGroups.ColorTable = finistToolStripColorTable3;
   this.toolStripChildGroups.Enabled = false;
   this.toolStripChildGroups.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.toolStripChildGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnChildGroupAdd,
            this.btnChildUserAdd,
            this.toolStripSeparator1,
            this.btnChildDel});
   this.toolStripChildGroups.Location = new System.Drawing.Point(3, 17);
   this.toolStripChildGroups.Name = "toolStripChildGroups";
   this.toolStripChildGroups.Size = new System.Drawing.Size(449, 25);
   this.toolStripChildGroups.TabIndex = 0;
   this.toolStripChildGroups.Text = "toolStrip3";
   // 
   // btnChildGroupAdd
   // 
   this.btnChildGroupAdd.AutoToolTip = false;
   this.btnChildGroupAdd.Image = global::Sim.AdminForms.Properties.Resources.addChildGroup;
   this.btnChildGroupAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnChildGroupAdd.Name = "btnChildGroupAdd";
   this.btnChildGroupAdd.Size = new System.Drawing.Size(117, 22);
   this.btnChildGroupAdd.Text = "Добавить группы";
   this.btnChildGroupAdd.ToolTipText = "Добавляет группы в выбранную слева группу";
   this.btnChildGroupAdd.Click += new System.EventHandler(this.btnChildGroupAdd_Click);
   // 
   // btnChildUserAdd
   // 
   this.btnChildUserAdd.AutoToolTip = false;
   this.btnChildUserAdd.Image = global::Sim.AdminForms.Properties.Resources.addUserToGroup;
   this.btnChildUserAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnChildUserAdd.Name = "btnChildUserAdd";
   this.btnChildUserAdd.Size = new System.Drawing.Size(157, 22);
   this.btnChildUserAdd.Text = "Добавить пользователей";
   this.btnChildUserAdd.ToolTipText = "Добавляет пользователей в выбранную слева группу.";
   this.btnChildUserAdd.Click += new System.EventHandler(this.btnChildUserAdd_Click);
   // 
   // toolStripSeparator1
   // 
   this.toolStripSeparator1.Name = "toolStripSeparator1";
   this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
   // 
   // btnChildDel
   // 
   this.btnChildDel.Enabled = false;
   this.btnChildDel.Image = global::Sim.AdminForms.Properties.Resources.RemoveChildGroup;
   this.btnChildDel.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnChildDel.Name = "btnChildDel";
   this.btnChildDel.Size = new System.Drawing.Size(84, 22);
   this.btnChildDel.Text = "Исключить";
   this.btnChildDel.ToolTipText = "Исключает выбранных ниже пользователей и групп из выбранной слева группы.";
   this.btnChildDel.Click += new System.EventHandler(this.btnChildDel_Click);
   // 
   // FormSecurityGroups
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.ClientSize = new System.Drawing.Size(813, 578);
   this.Name = "FormSecurityGroups";
   this.Text = "Группы безопасности";
   this.PanelBack.ResumeLayout(false);
   this.splitContainer1.Panel1.ResumeLayout(false);
   this.splitContainer1.Panel2.ResumeLayout(false);
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
   this.splitContainer1.ResumeLayout(false);
   this.groupBox1.ResumeLayout(false);
   this.groupBox1.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvGroups)).EndInit();
   this.panel1.ResumeLayout(false);
   this.panel1.PerformLayout();
   this.toolStripGroups.ResumeLayout(false);
   this.toolStripGroups.PerformLayout();
   this.splitContainer3.Panel1.ResumeLayout(false);
   this.splitContainer3.Panel2.ResumeLayout(false);
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
   this.splitContainer3.ResumeLayout(false);
   this.groupBox2.ResumeLayout(false);
   this.groupBox2.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvParents)).EndInit();
   this.toolStripParentGroups.ResumeLayout(false);
   this.toolStripParentGroups.PerformLayout();
   this.groupBox3.ResumeLayout(false);
   this.groupBox3.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvChilds)).EndInit();
   this.toolStripChildGroups.ResumeLayout(false);
   this.toolStripChildGroups.PerformLayout();
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.SplitContainer splitContainer1;
  private System.Windows.Forms.GroupBox groupBox1;
  private Sim.Controls.SimToolStrip toolStripGroups;
  private System.Windows.Forms.ToolStripButton toolStripButtonAddGroup;
  private System.Windows.Forms.ToolStripButton btnDelGroup;
  private System.Windows.Forms.SplitContainer splitContainer3;
  private System.Windows.Forms.GroupBox groupBox2;
  private Sim.Controls.SimToolStrip toolStripParentGroups;
  private System.Windows.Forms.GroupBox groupBox3;
  private Sim.Controls.SimToolStrip toolStripChildGroups;
  private System.Windows.Forms.ToolStripButton btnRenameGroup;
  private System.Windows.Forms.ToolStripButton btnParentAdd;
  private System.Windows.Forms.ToolStripButton btnParentDel;
  private System.Windows.Forms.ToolStripButton btnChildGroupAdd;
  private System.Windows.Forms.ToolStripButton btnChildDel;
  private System.Windows.Forms.ToolStripButton btnChildUserAdd;
  private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
  private Controls.SimDataGridView fdgvGroups;
  private System.Windows.Forms.DataGridViewImageColumn groupsImageColumn;
  private System.Windows.Forms.DataGridViewTextBoxColumn groupsNameColumn;
  private System.Windows.Forms.DataGridViewTextBoxColumn groupsDescColumn;
  private System.Windows.Forms.Panel panel1;
  private Controls.SimLabel flSID;
  private System.Windows.Forms.Label label2;
  private System.Windows.Forms.Label labelGroupName;
  private Controls.SimDataGridView fdgvParents;
  private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
  private Controls.SimDataGridView fdgvChilds;
  private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
  private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
 }
}
