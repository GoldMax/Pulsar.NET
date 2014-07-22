namespace Sim.Controls
{
 partial class SimModalExecLog
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
			this.finistPanelHeader = new Sim.Controls.SimPanel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.labelTime = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.buttonStop = new Sim.Controls.SimButton();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.finistPanel3 = new Sim.Controls.SimPanel();
			this.rtbLog = new System.Windows.Forms.RichTextBox();
			this.finistLabel1 = new Sim.Controls.SimLabel();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel2.SuspendLayout();
			this.finistPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.finistLabel1);
			this.panel1.Controls.Add(this.finistPanel1);
			this.panel1.Controls.Add(this.finistPanelHeader);
			// 
			// finistPanelHeader
			// 
			this.finistPanelHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.finistPanelHeader.BackColor2 = System.Drawing.SystemColors.ActiveCaption;
			this.finistPanelHeader.Controls.Add(this.finistLabelCaption);
			this.finistPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanelHeader.GradientMode = Sim.Controls.GradientMode.Vertical;
			this.finistPanelHeader.Location = new System.Drawing.Point(0, 0);
			this.finistPanelHeader.Name = "finistPanelHeader";
			this.finistPanelHeader.Size = new System.Drawing.Size(388, 25);
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
			this.finistLabelCaption.Image = global::Sim.Controls.Properties.Resources.Flush;
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(388, 25);
			this.finistLabelCaption.TabIndex = 1;
			this.finistLabelCaption.Text = "Выполнение операции";
			// 
			// finistPanel1
			// 
			this.finistPanel1.BackColor = System.Drawing.SystemColors.Control;
			this.finistPanel1.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel1.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.finistPanel1.Controls.Add(this.labelTime);
			this.finistPanel1.Controls.Add(this.pictureBox1);
			this.finistPanel1.Controls.Add(this.buttonStop);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanel1.GradientMode = Sim.Controls.GradientMode.BackwardDiagonal;
			this.finistPanel1.Location = new System.Drawing.Point(0, 345);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.finistPanel1.Size = new System.Drawing.Size(388, 31);
			this.finistPanel1.TabIndex = 10;
			// 
			// labelTime
			// 
			this.labelTime.AutoSize = true;
			this.labelTime.BackColor = System.Drawing.Color.Transparent;
			this.labelTime.Location = new System.Drawing.Point(9, 8);
			this.labelTime.Name = "labelTime";
			this.labelTime.Size = new System.Drawing.Size(51, 13);
			this.labelTime.TabIndex = 2;
			this.labelTime.Text = "00:00:00";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = global::Sim.Controls.Properties.Resources.CompServerExchange;
			this.pictureBox1.Location = new System.Drawing.Point(67, 7);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(65, 16);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// buttonStop
			// 
			this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonStop.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonStop.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.buttonStop.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonStop.Image = global::Sim.Controls.Properties.Resources.Stop;
			this.buttonStop.Location = new System.Drawing.Point(285, 2);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(98, 25);
			this.buttonStop.TabIndex = 0;
			this.buttonStop.Text = "Прервать";
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.finistPanel3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 44);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.panel2.Size = new System.Drawing.Size(388, 301);
			this.panel2.TabIndex = 11;
			// 
			// finistPanel3
			// 
			this.finistPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel3.Controls.Add(this.rtbLog);
			this.finistPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistPanel3.Location = new System.Drawing.Point(3, 0);
			this.finistPanel3.Name = "finistPanel3";
			this.finistPanel3.Size = new System.Drawing.Size(382, 298);
			this.finistPanel3.TabIndex = 2;
			// 
			// rtbLog
			// 
			this.rtbLog.BackColor = System.Drawing.Color.Ivory;
			this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbLog.Cursor = System.Windows.Forms.Cursors.Default;
			this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbLog.Location = new System.Drawing.Point(0, 0);
			this.rtbLog.Name = "rtbLog";
			this.rtbLog.ReadOnly = true;
			this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtbLog.Size = new System.Drawing.Size(380, 296);
			this.rtbLog.TabIndex = 13;
			this.rtbLog.Text = "";
			// 
			// finistLabel1
			// 
			this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistLabel1.Location = new System.Drawing.Point(0, 25);
			this.finistLabel1.Name = "finistLabel1";
			this.finistLabel1.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
			this.finistLabel1.Size = new System.Drawing.Size(120, 19);
			this.finistLabel1.TabIndex = 12;
			this.finistLabel1.Text = "Журнал выполнения";
			// 
			// SimModalExecLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MoveControl = this.finistPanelHeader;
			this.Name = "SimModalExecLog";
			this.VisibleChanged += new System.EventHandler(this.SimModalBatchExec_VisibleChanged);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanel1.ResumeLayout(false);
			this.finistPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.finistPanel3.ResumeLayout(false);
			this.ResumeLayout(false);

  }

  #endregion

  private SimPanel finistPanelHeader;
  internal SimLabel finistLabelCaption;
  private SimPanel finistPanel1;
  private System.Windows.Forms.Label labelTime;
  private System.Windows.Forms.PictureBox pictureBox1;
  internal Sim.Controls.SimButton buttonStop;
  private System.Windows.Forms.Timer timer1;
  private System.Windows.Forms.Panel panel2;
  private SimPanel finistPanel3;
  internal System.Windows.Forms.RichTextBox rtbLog;
  private SimLabel finistLabel1;
 }
}
