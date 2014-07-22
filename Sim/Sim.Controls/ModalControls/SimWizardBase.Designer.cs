namespace Sim.Controls
{
 partial class SimWizardBase
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
			this.finistPanelHeader = new Sim.Controls.SimPanel();
			this.finistLabelStep = new Sim.Controls.SimLabel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.buttonPrev = new Sim.Controls.SimButton();
			this.buttonNext = new Sim.Controls.SimButton();
			this.buttonCancel = new Sim.Controls.SimButton();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tabControlMain);
			this.panel1.Controls.Add(this.finistPanel1);
			this.panel1.Controls.Add(this.finistPanelHeader);
			this.panel1.Size = new System.Drawing.Size(386, 401);
			// 
			// finistPanelHeader
			// 
			this.finistPanelHeader.BackColor = System.Drawing.SystemColors.Window;
			this.finistPanelHeader.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanelHeader.BorderWidth = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.finistPanelHeader.Controls.Add(this.finistLabelStep);
			this.finistPanelHeader.Controls.Add(this.finistLabelCaption);
			this.finistPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanelHeader.GradientMode = Sim.Controls.GradientMode.ForwardDiagonal;
			this.finistPanelHeader.Location = new System.Drawing.Point(0, 0);
			this.finistPanelHeader.Name = "finistPanelHeader";
			this.finistPanelHeader.Size = new System.Drawing.Size(386, 37);
			this.finistPanelHeader.TabIndex = 2;
			// 
			// finistLabelStep
			// 
			this.finistLabelStep.AutoSize = false;
			this.finistLabelStep.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelStep.EventsTransparent = true;
			this.finistLabelStep.Location = new System.Drawing.Point(0, 15);
			this.finistLabelStep.Name = "finistLabelStep";
			this.finistLabelStep.Padding = new System.Windows.Forms.Padding(7, 2, 5, 0);
			this.finistLabelStep.Size = new System.Drawing.Size(386, 21);
			this.finistLabelStep.TabIndex = 1;
			this.finistLabelStep.Text = "finistLabel1";
			this.finistLabelStep.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			// 
			// finistLabelCaption
			// 
			this.finistLabelCaption.AutoSize = false;
			this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistLabelCaption.EventsTransparent = true;
			this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(5, 2, 5, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(386, 15);
			this.finistLabelCaption.TabIndex = 0;
			this.finistLabelCaption.Text = "finistLabelCaption";
			// 
			// finistPanel1
			// 
			this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel1.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.finistPanel1.Controls.Add(this.buttonPrev);
			this.finistPanel1.Controls.Add(this.buttonNext);
			this.finistPanel1.Controls.Add(this.buttonCancel);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanel1.Location = new System.Drawing.Point(0, 365);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(386, 36);
			this.finistPanel1.TabIndex = 3;
			// 
			// buttonPrev
			// 
			this.buttonPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPrev.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonPrev.Location = new System.Drawing.Point(136, 5);
			this.buttonPrev.Margin = new System.Windows.Forms.Padding(5);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Size = new System.Drawing.Size(75, 25);
			this.buttonPrev.TabIndex = 5;
			this.buttonPrev.Text = "<< Назад";
			this.buttonPrev.Visible = false;
			this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonNext.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonNext.Enabled = false;
			this.buttonNext.Location = new System.Drawing.Point(221, 5);
			this.buttonNext.Margin = new System.Windows.Forms.Padding(5);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(75, 25);
			this.buttonNext.TabIndex = 4;
			this.buttonNext.Text = "Далее >>";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = global::Sim.Controls.Properties.Resources.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(306, 5);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(5);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 25);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.ItemSize = new System.Drawing.Size(0, 1);
			this.tabControlMain.Location = new System.Drawing.Point(0, 37);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(386, 328);
			this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControlMain.TabIndex = 4;
			this.tabControlMain.TabStop = false;
			// 
			// SimWizardBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MoveControl = this.finistPanelHeader;
			this.Name = "SimWizardBase";
			this.Size = new System.Drawing.Size(390, 405);
			this.panel1.ResumeLayout(false);
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

  }

  #endregion

  private SimPanel finistPanel1;
  private Sim.Controls.SimButton buttonPrev;
  private Sim.Controls.SimButton buttonNext;
  private Sim.Controls.SimButton buttonCancel;
  private SimPanel finistPanelHeader;
  private SimLabel finistLabelStep;
  private SimLabel finistLabelCaption;
  /// <summary>
  /// 
  /// </summary>
  protected System.Windows.Forms.TabControl tabControlMain;

 }
}
