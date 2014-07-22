namespace Sim.AdminForms
{
 partial class CtrlCreateObject
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
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCreateObject));
   this.finistPanelHeader = new Sim.Controls.SimPanel();
   this.finistLabelCaption = new Sim.Controls.SimLabel();
   this.finistPanel2 = new Sim.Controls.SimPanel();
   this.btnCancel = new Sim.Controls.SimPopupButton();
   this.finistPanel1 = new Sim.Controls.SimPanel();
   this.buttonOK = new Sim.Controls.SimButton();
   this.buttonCancel = new Sim.Controls.SimButton();
   this.label1 = new System.Windows.Forms.Label();
   this.tbName = new Sim.Controls.SimTextBox();
   this.label2 = new System.Windows.Forms.Label();
   this.cbAssembly = new Sim.Controls.SimComboBox();
   this.label3 = new System.Windows.Forms.Label();
   this.cbType = new Sim.Controls.SimComboBox();
   this.label4 = new System.Windows.Forms.Label();
   this.tbArgs = new Sim.Controls.SimTextBox();
   this.label5 = new System.Windows.Forms.Label();
   this.tbGeneric = new Sim.Controls.SimTextBox();
   this.panel1.SuspendLayout();
   this.finistPanelHeader.SuspendLayout();
   this.finistPanel2.SuspendLayout();
   this.finistPanel1.SuspendLayout();
   this.SuspendLayout();
   // 
   // panel1
   // 
   this.panel1.Controls.Add(this.label5);
   this.panel1.Controls.Add(this.tbGeneric);
   this.panel1.Controls.Add(this.finistPanel1);
   this.panel1.Controls.Add(this.cbType);
   this.panel1.Controls.Add(this.cbAssembly);
   this.panel1.Controls.Add(this.label4);
   this.panel1.Controls.Add(this.label3);
   this.panel1.Controls.Add(this.label2);
   this.panel1.Controls.Add(this.tbArgs);
   this.panel1.Controls.Add(this.tbName);
   this.panel1.Controls.Add(this.label1);
   this.panel1.Controls.Add(this.finistPanelHeader);
   this.panel1.Size = new System.Drawing.Size(379, 291);
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
   this.finistPanelHeader.Size = new System.Drawing.Size(379, 25);
   this.finistPanelHeader.TabIndex = 10;
   // 
   // finistLabelCaption
   // 
   this.finistLabelCaption.AutoEllipsis = true;
   this.finistLabelCaption.BackColor = System.Drawing.Color.Transparent;
   this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
   this.finistLabelCaption.EventsTransparent = true;
   this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.finistLabelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.finistLabelCaption.Image = global::Sim.AdminForms.Properties.Resources.Object_New;
   this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
   this.finistLabelCaption.Name = "finistLabelCaption";
   this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
   this.finistLabelCaption.Size = new System.Drawing.Size(355, 25);
   this.finistLabelCaption.TabIndex = 1;
   this.finistLabelCaption.Text = "  Создание объекта";
   // 
   // finistPanel2
   // 
   this.finistPanel2.BackColor = System.Drawing.Color.Transparent;
   this.finistPanel2.Controls.Add(this.btnCancel);
   this.finistPanel2.Dock = System.Windows.Forms.DockStyle.Right;
   this.finistPanel2.Location = new System.Drawing.Point(355, 0);
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
   this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
   this.finistPanel1.Location = new System.Drawing.Point(0, 256);
   this.finistPanel1.MinimumSize = new System.Drawing.Size(386, 35);
   this.finistPanel1.Name = "finistPanel1";
   this.finistPanel1.Size = new System.Drawing.Size(386, 35);
   this.finistPanel1.TabIndex = 11;
   // 
   // buttonOK
   // 
   this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonOK.Enabled = false;
   this.buttonOK.Image = global::Sim.AdminForms.Properties.Resources.OK;
   this.buttonOK.Location = new System.Drawing.Point(218, 4);
   this.buttonOK.Name = "buttonOK";
   this.buttonOK.Size = new System.Drawing.Size(75, 26);
   this.buttonOK.TabIndex = 0;
   this.buttonOK.Text = "ОК";
   this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
   // 
   // buttonCancel
   // 
   this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonCancel.Image = global::Sim.AdminForms.Properties.Resources.Cancel;
   this.buttonCancel.Location = new System.Drawing.Point(299, 4);
   this.buttonCancel.Name = "buttonCancel";
   this.buttonCancel.Size = new System.Drawing.Size(75, 26);
   this.buttonCancel.TabIndex = 0;
   this.buttonCancel.Text = "Отмена";
   this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
   // 
   // label1
   // 
   this.label1.AutoSize = true;
   this.label1.Location = new System.Drawing.Point(6, 33);
   this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(72, 13);
   this.label1.TabIndex = 12;
   this.label1.Text = "Имя объекта";
   // 
   // tbName
   // 
   this.tbName.Location = new System.Drawing.Point(9, 49);
   this.tbName.Name = "tbName";
   this.tbName.Size = new System.Drawing.Size(362, 21);
   this.tbName.TabIndex = 13;
   this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
   // 
   // label2
   // 
   this.label2.AutoSize = true;
   this.label2.Location = new System.Drawing.Point(6, 78);
   this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(90, 13);
   this.label2.TabIndex = 14;
   this.label2.Text = "Сборка объекта";
   // 
   // cbAssembly
   // 
   this.cbAssembly.Items = ((System.Collections.IList)(resources.GetObject("cbAssembly.Items")));
   this.cbAssembly.Location = new System.Drawing.Point(9, 94);
   this.cbAssembly.Name = "cbAssembly";
   this.cbAssembly.SelectedItem = null;
   this.cbAssembly.Size = new System.Drawing.Size(362, 21);
   this.cbAssembly.TabIndex = 15;
   this.cbAssembly.UISelectedItemChanged += new System.EventHandler(this.cbAssembly_SelectionChangeCommitted);
   this.cbAssembly.TextChanged += new System.EventHandler(this.tbName_TextChanged);
   // 
   // label3
   // 
   this.label3.AutoSize = true;
   this.label3.Location = new System.Drawing.Point(6, 123);
   this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label3.Name = "label3";
   this.label3.Size = new System.Drawing.Size(71, 13);
   this.label3.TabIndex = 14;
   this.label3.Text = "Тип объекта";
   // 
   // cbType
   // 
   this.cbType.Items = ((System.Collections.IList)(resources.GetObject("cbType.Items")));
   this.cbType.Location = new System.Drawing.Point(9, 139);
   this.cbType.Name = "cbType";
   this.cbType.SelectedItem = null;
   this.cbType.Size = new System.Drawing.Size(362, 21);
   this.cbType.TabIndex = 15;
   this.cbType.DropDownOpening += new System.ComponentModel.CancelEventHandler(this.cbType_DropDown);
   this.cbType.SelectedItemChanged += new System.EventHandler(this.cbType_SelectedItemChanged);
   this.cbType.TextChanged += new System.EventHandler(this.tbName_TextChanged);
   // 
   // label4
   // 
   this.label4.AutoSize = true;
   this.label4.Location = new System.Drawing.Point(6, 213);
   this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label4.Name = "label4";
   this.label4.Size = new System.Drawing.Size(275, 13);
   this.label4.TabIndex = 14;
   this.label4.Text = "Параметры конструктора (значения через запятую)";
   // 
   // tbArgs
   // 
   this.tbArgs.Location = new System.Drawing.Point(9, 229);
   this.tbArgs.Name = "tbArgs";
   this.tbArgs.Size = new System.Drawing.Size(362, 21);
   this.tbArgs.TabIndex = 13;
   // 
   // label5
   // 
   this.label5.AutoSize = true;
   this.label5.Location = new System.Drawing.Point(6, 168);
   this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label5.Name = "label5";
   this.label5.Size = new System.Drawing.Size(102, 13);
   this.label5.TabIndex = 17;
   this.label5.Text = "Generic параметры";
   // 
   // tbGeneric
   // 
   this.tbGeneric.Location = new System.Drawing.Point(9, 184);
   this.tbGeneric.Name = "tbGeneric";
   this.tbGeneric.Size = new System.Drawing.Size(362, 21);
   this.tbGeneric.TabIndex = 16;
   // 
   // CtrlCreateObject
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.BackColor = System.Drawing.SystemColors.Control;
   this.MoveControl = this.finistPanelHeader;
   this.Name = "CtrlCreateObject";
   this.Size = new System.Drawing.Size(383, 295);
   this.panel1.ResumeLayout(false);
   this.panel1.PerformLayout();
   this.finistPanelHeader.ResumeLayout(false);
   this.finistPanel2.ResumeLayout(false);
   this.finistPanel1.ResumeLayout(false);
   this.ResumeLayout(false);

  }

  #endregion

  private Controls.SimPanel finistPanelHeader;
  internal Controls.SimLabel finistLabelCaption;
  private Controls.SimPanel finistPanel2;
  private Controls.SimPopupButton btnCancel;
  private Controls.SimPanel finistPanel1;
  private Sim.Controls.SimButton buttonOK;
  private Sim.Controls.SimButton buttonCancel;
  private Sim.Controls.SimComboBox cbType;
  private Sim.Controls.SimComboBox cbAssembly;
  private System.Windows.Forms.Label label4;
  private System.Windows.Forms.Label label3;
  private System.Windows.Forms.Label label2;
  private Sim.Controls.SimTextBox tbArgs;
  private Sim.Controls.SimTextBox tbName;
  private System.Windows.Forms.Label label1;
  private System.Windows.Forms.Label label5;
  private Sim.Controls.SimTextBox tbGeneric;
 }
}
