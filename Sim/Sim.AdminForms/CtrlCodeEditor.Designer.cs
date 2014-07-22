namespace Sim.AdminForms
{
 partial class CtrlCodeEditor
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
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCodeEditor));
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
   this.finistPanelHeader = new Sim.Controls.SimPanel();
   this.finistLabelCaption = new Sim.Controls.SimLabel();
   this.finistPanel2 = new Sim.Controls.SimPanel();
   this.btnCancel = new Sim.Controls.SimPopupButton();
   this.finistToolStrip1 = new Sim.Controls.SimToolStrip();
   this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
   this.cbObject = new Sim.Controls.SimToolStripComboBox();
   this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
   this.btnWordWrap = new System.Windows.Forms.ToolStripButton();
   this.finistPanel1 = new Sim.Controls.SimPanel();
   this.buttonOK = new Sim.Controls.SimButton();
   this.buttonCancel = new Sim.Controls.SimButton();
   this.splitContainer1 = new System.Windows.Forms.SplitContainer();
   this.richTextBox1 = new System.Windows.Forms.RichTextBox();
   this.groupBox1 = new System.Windows.Forms.GroupBox();
   this.fdgvErrs = new Sim.Controls.SimDataGridView();
   this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.panel1.SuspendLayout();
   this.finistPanelHeader.SuspendLayout();
   this.finistPanel2.SuspendLayout();
   this.finistToolStrip1.SuspendLayout();
   this.finistPanel1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
   this.splitContainer1.Panel1.SuspendLayout();
   this.splitContainer1.Panel2.SuspendLayout();
   this.splitContainer1.SuspendLayout();
   this.groupBox1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvErrs)).BeginInit();
   this.SuspendLayout();
   // 
   // panel1
   // 
   this.panel1.Controls.Add(this.splitContainer1);
   this.panel1.Controls.Add(this.finistPanel1);
   this.panel1.Controls.Add(this.finistToolStrip1);
   this.panel1.Controls.Add(this.finistPanelHeader);
   this.panel1.Size = new System.Drawing.Size(801, 566);
   // 
   // finistPanelHeader
   // 
   this.finistPanelHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
   this.finistPanelHeader.BackColor2 = System.Drawing.SystemColors.ActiveCaption;
   this.finistPanelHeader.Controls.Add(this.finistLabelCaption);
   this.finistPanelHeader.Controls.Add(this.finistPanel2);
   this.finistPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
   this.finistPanelHeader.GradientMode = Sim.Controls.GradientMode.Vertical;
   this.finistPanelHeader.Location = new System.Drawing.Point(0, 0);
   this.finistPanelHeader.MinimumSize = new System.Drawing.Size(0, 25);
   this.finistPanelHeader.Name = "finistPanelHeader";
   this.finistPanelHeader.Size = new System.Drawing.Size(801, 25);
   this.finistPanelHeader.TabIndex = 11;
   // 
   // finistLabelCaption
   // 
   this.finistLabelCaption.AutoEllipsis = true;
   this.finistLabelCaption.BackColor = System.Drawing.Color.Transparent;
   this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
   this.finistLabelCaption.EventsTransparent = true;
   this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.finistLabelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.finistLabelCaption.Image = global::Sim.AdminForms.Properties.Resources.Code;
   this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
   this.finistLabelCaption.Name = "finistLabelCaption";
   this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
   this.finistLabelCaption.Size = new System.Drawing.Size(777, 25);
   this.finistLabelCaption.TabIndex = 1;
   this.finistLabelCaption.Text = "  Редактор кода";
   // 
   // finistPanel2
   // 
   this.finistPanel2.BackColor = System.Drawing.Color.Transparent;
   this.finistPanel2.Controls.Add(this.btnCancel);
   this.finistPanel2.Dock = System.Windows.Forms.DockStyle.Right;
   this.finistPanel2.Location = new System.Drawing.Point(777, 0);
   this.finistPanel2.Name = "finistPanel2";
   this.finistPanel2.Size = new System.Drawing.Size(24, 25);
   this.finistPanel2.TabIndex = 0;
   // 
   // btnCancel
   // 
   this.btnCancel.Image = global::Sim.AdminForms.Properties.Resources.Xbutton_Normal;
   this.btnCancel.ImagePushed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePushed")));
   this.btnCancel.ImageRaised = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageRaised")));
   this.btnCancel.Location = new System.Drawing.Point(5, 5);
   this.btnCancel.Name = "btnCancel";
   this.btnCancel.Size = new System.Drawing.Size(14, 14);
   this.btnCancel.TabIndex = 0;
   this.btnCancel.TabStop = false;
   this.btnCancel.ToolTip = null;
   this.btnCancel.Click += new System.EventHandler(this.buttonCancel_Click);
   // 
   // finistToolStrip1
   // 
   finistToolStripColorTable1.UseSystemColors = false;
   this.finistToolStrip1.ColorTable = finistToolStripColorTable1;
   this.finistToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.finistToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cbObject,
            this.toolStripSeparator1,
            this.btnWordWrap});
   this.finistToolStrip1.Location = new System.Drawing.Point(0, 25);
   this.finistToolStrip1.Name = "finistToolStrip1";
   this.finistToolStrip1.Size = new System.Drawing.Size(801, 25);
   this.finistToolStrip1.TabIndex = 12;
   this.finistToolStrip1.Text = "finistToolStrip1";
   // 
   // toolStripLabel1
   // 
   this.toolStripLabel1.Name = "toolStripLabel1";
   this.toolStripLabel1.Size = new System.Drawing.Size(97, 22);
   this.toolStripLabel1.Text = "Объект Пульсара";
   // 
   // cbObject
   // 
   this.cbObject.AutoSize = false;
   this.cbObject.BackColor = System.Drawing.SystemColors.Window;
   this.cbObject.Name = "cbObject";
   this.cbObject.SelectedItem = null;
   this.cbObject.Size = new System.Drawing.Size(198, 21);
   this.cbObject.Sorted = true;
   this.cbObject.UISelectedItemChanged += new System.EventHandler(this.cbObject_SelectionChangeCommitted);
   // 
   // toolStripSeparator1
   // 
   this.toolStripSeparator1.Name = "toolStripSeparator1";
   this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
   // 
   // btnWordWrap
   // 
   this.btnWordWrap.CheckOnClick = true;
   this.btnWordWrap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
   this.btnWordWrap.Image = global::Sim.AdminForms.Properties.Resources.WordWrap;
   this.btnWordWrap.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.btnWordWrap.Name = "btnWordWrap";
   this.btnWordWrap.Size = new System.Drawing.Size(23, 22);
   this.btnWordWrap.Text = "toolStripButton1";
   this.btnWordWrap.ToolTipText = "Перенос но словам";
   this.btnWordWrap.Click += new System.EventHandler(this.btnWordWrap_Click);
   // 
   // finistPanel1
   // 
   this.finistPanel1.BackColor = System.Drawing.SystemColors.Control;
   this.finistPanel1.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
   this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.finistPanel1.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 0);
   this.finistPanel1.Controls.Add(this.buttonOK);
   this.finistPanel1.Controls.Add(this.buttonCancel);
   this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
   this.finistPanel1.GradientMode = Sim.Controls.GradientMode.BackwardDiagonal;
   this.finistPanel1.Location = new System.Drawing.Point(0, 531);
   this.finistPanel1.MinimumSize = new System.Drawing.Size(386, 35);
   this.finistPanel1.Name = "finistPanel1";
   this.finistPanel1.Size = new System.Drawing.Size(801, 35);
   this.finistPanel1.TabIndex = 13;
   // 
   // buttonOK
   // 
   this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonOK.Enabled = false;
   this.buttonOK.Image = global::Sim.AdminForms.Properties.Resources.Compile;
   this.buttonOK.Location = new System.Drawing.Point(615, 4);
   this.buttonOK.Name = "buttonOK";
   this.buttonOK.Size = new System.Drawing.Size(93, 26);
   this.buttonOK.TabIndex = 0;
   this.buttonOK.Text = "Компиляция";
   this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
   // 
   // buttonCancel
   // 
   this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonCancel.Image = global::Sim.AdminForms.Properties.Resources.Cancel;
   this.buttonCancel.Location = new System.Drawing.Point(714, 4);
   this.buttonCancel.Name = "buttonCancel";
   this.buttonCancel.Size = new System.Drawing.Size(75, 26);
   this.buttonCancel.TabIndex = 0;
   this.buttonCancel.Text = "Отмена";
   this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
   // 
   // splitContainer1
   // 
   this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
   this.splitContainer1.Location = new System.Drawing.Point(0, 50);
   this.splitContainer1.Name = "splitContainer1";
   this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
   // 
   // splitContainer1.Panel1
   // 
   this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
   // 
   // splitContainer1.Panel2
   // 
   this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
   this.splitContainer1.Size = new System.Drawing.Size(801, 481);
   this.splitContainer1.SplitterDistance = 364;
   this.splitContainer1.TabIndex = 14;
   // 
   // richTextBox1
   // 
   this.richTextBox1.AcceptsTab = true;
   this.richTextBox1.DetectUrls = false;
   this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.richTextBox1.EnableAutoDragDrop = true;
   this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.richTextBox1.HideSelection = false;
   this.richTextBox1.Location = new System.Drawing.Point(0, 0);
   this.richTextBox1.Name = "richTextBox1";
   this.richTextBox1.Size = new System.Drawing.Size(801, 364);
   this.richTextBox1.TabIndex = 0;
   this.richTextBox1.Text = "";
   this.richTextBox1.WordWrap = false;
   this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
   // 
   // groupBox1
   // 
   this.groupBox1.Controls.Add(this.fdgvErrs);
   this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.groupBox1.Location = new System.Drawing.Point(0, 0);
   this.groupBox1.Name = "groupBox1";
   this.groupBox1.Size = new System.Drawing.Size(801, 113);
   this.groupBox1.TabIndex = 0;
   this.groupBox1.TabStop = false;
   this.groupBox1.Text = " Список ошибок ";
   // 
   // fdgvErrs
   // 
   this.fdgvErrs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
   this.fdgvErrs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
   this.fdgvErrs.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
   this.fdgvErrs.ColumnHeadersVisible = false;
   this.fdgvErrs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
   this.fdgvErrs.Dock = System.Windows.Forms.DockStyle.Fill;
   this.fdgvErrs.Location = new System.Drawing.Point(3, 17);
   this.fdgvErrs.MultiSelect = false;
   this.fdgvErrs.Name = "fdgvErrs";
   this.fdgvErrs.RowHeadersVisible = false;
   this.fdgvErrs.RowTemplate.Height = 18;
   this.fdgvErrs.Size = new System.Drawing.Size(795, 93);
   this.fdgvErrs.TabIndex = 0;
   this.fdgvErrs.VirtualMode = true;
   this.fdgvErrs.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.fdgvErrs_CellMouseDoubleClick);
   // 
   // Column1
   // 
   this.Column1.DataPropertyName = "Value";
   this.Column1.HeaderText = "Ошибка";
   this.Column1.Name = "Column1";
   this.Column1.ReadOnly = true;
   // 
   // CtrlCodeEditor
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.BackColor = System.Drawing.SystemColors.Control;
   this.MoveControl = this.finistPanelHeader;
   this.Name = "CtrlCodeEditor";
   this.Size = new System.Drawing.Size(805, 570);
   this.panel1.ResumeLayout(false);
   this.panel1.PerformLayout();
   this.finistPanelHeader.ResumeLayout(false);
   this.finistPanel2.ResumeLayout(false);
   this.finistToolStrip1.ResumeLayout(false);
   this.finistToolStrip1.PerformLayout();
   this.finistPanel1.ResumeLayout(false);
   this.splitContainer1.Panel1.ResumeLayout(false);
   this.splitContainer1.Panel2.ResumeLayout(false);
   ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
   this.splitContainer1.ResumeLayout(false);
   this.groupBox1.ResumeLayout(false);
   ((System.ComponentModel.ISupportInitialize)(this.fdgvErrs)).EndInit();
   this.ResumeLayout(false);

  }

  #endregion

  private Controls.SimPanel finistPanelHeader;
  internal Controls.SimLabel finistLabelCaption;
  private Controls.SimPanel finistPanel2;
  private Controls.SimPopupButton btnCancel;
  private Controls.SimToolStrip finistToolStrip1;
  private System.Windows.Forms.ToolStripLabel toolStripLabel1;
  private System.Windows.Forms.SplitContainer splitContainer1;
  private Controls.SimPanel finistPanel1;
  private Sim.Controls.SimButton buttonOK;
  private Sim.Controls.SimButton buttonCancel;
  private System.Windows.Forms.RichTextBox richTextBox1;
  private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
  private System.Windows.Forms.ToolStripButton btnWordWrap;
  private System.Windows.Forms.GroupBox groupBox1;
  private Controls.SimDataGridView fdgvErrs;
  private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
  private Controls.SimToolStripComboBox cbObject;
 }
}
