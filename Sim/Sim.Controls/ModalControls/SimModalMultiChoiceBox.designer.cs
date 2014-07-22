namespace Sim.Controls
{
 partial class SimModalMultiChoiceBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimModalMultiChoiceBox));
			this.finistPanelHeader = new Sim.Controls.SimPanel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.finistPanel2 = new Sim.Controls.SimPanel();
			this.btnCancel = new Sim.Controls.SimPopupButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBoxVariants = new System.Windows.Forms.GroupBox();
			this.fdgvVariants = new Sim.Controls.SimDataGridView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.buttonExclude = new Sim.Controls.SimButton();
			this.buttonInclude = new Sim.Controls.SimButton();
			this.groupBoxChoices = new System.Windows.Forms.GroupBox();
			this.fdgvChoises = new Sim.Controls.SimDataGridView();
			this.panel3 = new System.Windows.Forms.Panel();
			this.buttonOk = new Sim.Controls.SimButton();
			this.buttonCancel = new Sim.Controls.SimButton();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBoxVariants.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fdgvVariants)).BeginInit();
			this.panel2.SuspendLayout();
			this.groupBoxChoices.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fdgvChoises)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.finistPanelHeader);
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
			this.finistPanelHeader.Name = "finistPanelHeader";
			this.finistPanelHeader.Size = new System.Drawing.Size(388, 21);
			this.finistPanelHeader.TabIndex = 9;
			// 
			// finistLabelCaption
			// 
			this.finistLabelCaption.AutoSize = false;
			this.finistLabelCaption.BackColor = System.Drawing.Color.Transparent;
			this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelCaption.EventsTransparent = true;
			this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.finistLabelCaption.Image = global::Sim.Controls.Properties.Resources.Rename;
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(364, 21);
			this.finistLabelCaption.TabIndex = 1;
			this.finistLabelCaption.Text = "Выбор элемента";
			// 
			// finistPanel2
			// 
			this.finistPanel2.BackColor = System.Drawing.Color.Transparent;
			this.finistPanel2.Controls.Add(this.btnCancel);
			this.finistPanel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.finistPanel2.Location = new System.Drawing.Point(364, 0);
			this.finistPanel2.Name = "finistPanel2";
			this.finistPanel2.Size = new System.Drawing.Size(24, 21);
			this.finistPanel2.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Image = global::Sim.Controls.Properties.Resources.NormalXbtn2_c;
			this.btnCancel.ImagePushed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePushed")));
			this.btnCancel.ImageRaised = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageRaised")));
			this.btnCancel.Location = new System.Drawing.Point(4, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.PushedBackColorStart = System.Drawing.Color.Transparent;
			this.btnCancel.ShowBorder = false;
			this.btnCancel.Size = new System.Drawing.Size(16, 16);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.TabStop = false;
			this.btnCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 21);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.groupBoxVariants);
			this.splitContainer1.Panel1.Controls.Add(this.panel2);
			this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.splitContainer1.Panel1MinSize = 75;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.groupBoxChoices);
			this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.splitContainer1.Panel2MinSize = 50;
			this.splitContainer1.Size = new System.Drawing.Size(388, 322);
			this.splitContainer1.SplitterDistance = 221;
			this.splitContainer1.TabIndex = 11;
			// 
			// groupBoxVariants
			// 
			this.groupBoxVariants.Controls.Add(this.fdgvVariants);
			this.groupBoxVariants.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxVariants.Location = new System.Drawing.Point(3, 0);
			this.groupBoxVariants.Name = "groupBoxVariants";
			this.groupBoxVariants.Size = new System.Drawing.Size(382, 188);
			this.groupBoxVariants.TabIndex = 2;
			this.groupBoxVariants.TabStop = false;
			this.groupBoxVariants.Text = " Варианты ";
			// 
			// fdgvVariants
			// 
			this.fdgvVariants.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.fdgvVariants.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fdgvVariants.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.fdgvVariants.ColumnHeadersHeight = 20;
			this.fdgvVariants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.fdgvVariants.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fdgvVariants.Location = new System.Drawing.Point(3, 17);
			this.fdgvVariants.Name = "fdgvVariants";
			this.fdgvVariants.RowHeadersVisible = false;
			this.fdgvVariants.RowTemplate.Height = 17;
			this.fdgvVariants.Size = new System.Drawing.Size(376, 168);
			this.fdgvVariants.TabIndex = 1;
			this.fdgvVariants.VirtualMode = true;
			this.fdgvVariants.SelectionChanged += new System.EventHandler(this.fdgvVariants_SelectionChanged);
			this.fdgvVariants.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.buttonExclude);
			this.panel2.Controls.Add(this.buttonInclude);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(3, 188);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(382, 33);
			this.panel2.TabIndex = 1;
			// 
			// buttonExclude
			// 
			this.buttonExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonExclude.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonExclude.Enabled = false;
			this.buttonExclude.Image = global::Sim.Controls.Properties.Resources.Up;
			this.buttonExclude.Location = new System.Drawing.Point(249, 6);
			this.buttonExclude.Name = "buttonExclude";
			this.buttonExclude.Size = new System.Drawing.Size(113, 25);
			this.buttonExclude.TabIndex = 1;
			this.buttonExclude.Text = "Исключить";
			this.buttonExclude.Click += new System.EventHandler(this.buttonExclude_Click);
			// 
			// buttonInclude
			// 
			this.buttonInclude.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonInclude.Enabled = false;
			this.buttonInclude.Image = global::Sim.Controls.Properties.Resources.Down;
			this.buttonInclude.Location = new System.Drawing.Point(22, 6);
			this.buttonInclude.Name = "buttonInclude";
			this.buttonInclude.Size = new System.Drawing.Size(113, 25);
			this.buttonInclude.TabIndex = 0;
			this.buttonInclude.Text = "Добавить";
			this.buttonInclude.Click += new System.EventHandler(this.buttonInclude_Click);
			// 
			// groupBoxChoices
			// 
			this.groupBoxChoices.Controls.Add(this.fdgvChoises);
			this.groupBoxChoices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxChoices.Location = new System.Drawing.Point(3, 0);
			this.groupBoxChoices.Name = "groupBoxChoices";
			this.groupBoxChoices.Size = new System.Drawing.Size(382, 97);
			this.groupBoxChoices.TabIndex = 0;
			this.groupBoxChoices.TabStop = false;
			this.groupBoxChoices.Text = " Выбор ";
			// 
			// fdgvChoises
			// 
			this.fdgvChoises.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.fdgvChoises.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.fdgvChoises.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.fdgvChoises.ColumnHeadersHeight = 20;
			this.fdgvChoises.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.fdgvChoises.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fdgvChoises.Location = new System.Drawing.Point(3, 17);
			this.fdgvChoises.Name = "fdgvChoises";
			this.fdgvChoises.RowHeadersVisible = false;
			this.fdgvChoises.RowTemplate.Height = 17;
			this.fdgvChoises.Size = new System.Drawing.Size(376, 77);
			this.fdgvChoises.TabIndex = 2;
			this.fdgvChoises.VirtualMode = true;
			this.fdgvChoises.SelectionChanged += new System.EventHandler(this.fdgvChoises_SelectionChanged);
			this.fdgvChoises.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseDoubleClick);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.buttonOk);
			this.panel3.Controls.Add(this.buttonCancel);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 343);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(388, 33);
			this.panel3.TabIndex = 10;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Enabled = false;
			this.buttonOk.Image = global::Sim.Controls.Properties.Resources.OK;
			this.buttonOk.Location = new System.Drawing.Point(237, 3);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(71, 26);
			this.buttonOk.TabIndex = 0;
			this.buttonOk.Text = "ОК";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = global::Sim.Controls.Properties.Resources.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(314, 3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(71, 26);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// SimModalMultiChoiceBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MoveControl = this.finistPanelHeader;
			this.Name = "SimModalMultiChoiceBox";
			this.panel1.ResumeLayout(false);
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanel2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBoxVariants.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fdgvVariants)).EndInit();
			this.panel2.ResumeLayout(false);
			this.groupBoxChoices.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fdgvChoises)).EndInit();
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

  }

  #endregion

  private Sim.Controls.SimPanel finistPanelHeader;
  internal Sim.Controls.SimLabel finistLabelCaption;
  private Sim.Controls.SimPanel finistPanel2;
  private Sim.Controls.SimPopupButton btnCancel;
  private System.Windows.Forms.SplitContainer splitContainer1;
  private System.Windows.Forms.GroupBox groupBoxVariants;
  private System.Windows.Forms.Panel panel2;
  private Sim.Controls.SimButton buttonExclude;
  private Sim.Controls.SimButton buttonInclude;
  private System.Windows.Forms.GroupBox groupBoxChoices;
  private System.Windows.Forms.Panel panel3;
  private Sim.Controls.SimButton buttonOk;
  private Sim.Controls.SimButton buttonCancel;
  private Sim.Controls.SimDataGridView fdgvVariants;
  private Sim.Controls.SimDataGridView fdgvChoises;
 }
}
