namespace Sim.Controls
{
 partial class ModalErrorBox
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
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.finistPanelButtons = new Sim.Controls.SimPanel();
			this.buttonOK = new Sim.Controls.SimButton();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.button2 = new Sim.Controls.SimButton();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.flErrCaption = new Sim.Controls.SimLabel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.fpServMessage = new Sim.Controls.SimPanel();
			this.finistLabel7 = new Sim.Controls.SimLabel();
			this.finistLabel5 = new Sim.Controls.SimLabel();
			this.flMethod = new Sim.Controls.SimLabel();
			this.flFile = new Sim.Controls.SimLabel();
			this.flClass = new Sim.Controls.SimLabel();
			this.flLine = new Sim.Controls.SimLabel();
			this.flAsm = new Sim.Controls.SimLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.flErrText = new Sim.Controls.SimLabel();
			this.fpStack = new System.Windows.Forms.Panel();
			this.ftbStack = new Sim.Controls.SimTextBox();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanelButtons.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel3.SuspendLayout();
			this.fpServMessage.SuspendLayout();
			this.panel6.SuspendLayout();
			this.fpStack.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.fpStack);
			this.panel1.Controls.Add(this.finistPanelButtons);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.finistPanelHeader);
			this.panel1.Size = new System.Drawing.Size(456, 308);
			// 
			// finistPanelHeader
			// 
			this.finistPanelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
			this.finistPanelHeader.BackColor2 = System.Drawing.Color.DarkRed;
			this.finistPanelHeader.BackColorMiddle = System.Drawing.Color.Red;
			this.finistPanelHeader.Controls.Add(this.finistLabelCaption);
			this.finistPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanelHeader.GradientMode = Sim.Controls.GradientMode.TrioVertical;
			this.finistPanelHeader.Location = new System.Drawing.Point(0, 0);
			this.finistPanelHeader.Name = "finistPanelHeader";
			this.finistPanelHeader.Size = new System.Drawing.Size(456, 22);
			this.finistPanelHeader.TabIndex = 10;
			// 
			// finistLabelCaption
			// 
			this.finistLabelCaption.BackColor = System.Drawing.Color.Transparent;
			this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelCaption.EventsTransparent = true;
			this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.MinimumSize = new System.Drawing.Size(457, 22);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(457, 22);
			this.finistLabelCaption.TabIndex = 1;
			this.finistLabelCaption.Text = "Ошибка!";
			// 
			// finistPanelButtons
			// 
			this.finistPanelButtons.BackColor = System.Drawing.SystemColors.Control;
			this.finistPanelButtons.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanelButtons.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.finistPanelButtons.Controls.Add(this.buttonOK);
			this.finistPanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanelButtons.GradientMode = Sim.Controls.GradientMode.BackwardDiagonal;
			this.finistPanelButtons.Location = new System.Drawing.Point(0, 273);
			this.finistPanelButtons.MinimumSize = new System.Drawing.Size(456, 35);
			this.finistPanelButtons.Name = "finistPanelButtons";
			this.finistPanelButtons.Size = new System.Drawing.Size(456, 35);
			this.finistPanelButtons.TabIndex = 11;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonOK.Location = new System.Drawing.Point(189, 4);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(78, 26);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "ОК";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label7);
			this.panel2.Controls.Add(this.button2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 126);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(456, 20);
			this.panel2.TabIndex = 0;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label7.Location = new System.Drawing.Point(26, 9);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(416, 2);
			this.label7.TabIndex = 21;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.button2.CausesValidation = false;
			this.button2.Font = new System.Drawing.Font("Arial Black", 9.75F);
			this.button2.Location = new System.Drawing.Point(7, 3);
			this.button2.Margin = new System.Windows.Forms.Padding(0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(14, 14);
			this.button2.TabIndex = 20;
			this.button2.Tag = "0";
			this.button2.Text = "+";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBox1.Image = global::Sim.Controls.Properties.Resources.Error48;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(69, 126);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// flErrCaption
			// 
			this.flErrCaption.Dock = System.Windows.Forms.DockStyle.Top;
			this.flErrCaption.EventsTransparent = true;
			this.flErrCaption.Location = new System.Drawing.Point(69, 0);
			this.flErrCaption.Name = "flErrCaption";
			this.flErrCaption.Size = new System.Drawing.Size(99, 13);
			this.flErrCaption.TabIndex = 2;
			this.flErrCaption.Text = "Текст сообщения:";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.fpServMessage);
			this.panel3.Controls.Add(this.flMethod);
			this.panel3.Controls.Add(this.flFile);
			this.panel3.Controls.Add(this.flClass);
			this.panel3.Controls.Add(this.flLine);
			this.panel3.Controls.Add(this.flAsm);
			this.panel3.Controls.Add(this.label5);
			this.panel3.Controls.Add(this.label4);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Controls.Add(this.panel6);
			this.panel3.Controls.Add(this.flErrCaption);
			this.panel3.Controls.Add(this.pictureBox1);
			this.panel3.Controls.Add(this.panel2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(0, 22);
			this.panel3.MinimumSize = new System.Drawing.Size(0, 146);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(456, 146);
			this.panel3.TabIndex = 13;
			// 
			// fpServMessage
			// 
			this.fpServMessage.Controls.Add(this.finistLabel7);
			this.fpServMessage.Controls.Add(this.finistLabel5);
			this.fpServMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpServMessage.Location = new System.Drawing.Point(69, 69);
			this.fpServMessage.Name = "fpServMessage";
			this.fpServMessage.Size = new System.Drawing.Size(387, 57);
			this.fpServMessage.TabIndex = 20;
			this.fpServMessage.Visible = false;
			// 
			// finistLabel7
			// 
			this.finistLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabel7.Location = new System.Drawing.Point(0, 13);
			this.finistLabel7.Name = "finistLabel7";
			this.finistLabel7.Size = new System.Drawing.Size(353, 39);
			this.finistLabel7.TabIndex = 1;
			this.finistLabel7.Text = "Последняя операция над данными завершилась с ошибкой!\r\nЗакройте текущую форму, от" +
    "кройте вновь и повторите операцию.\r\nВ случае повторения ошибки, обратитесь к раз" +
    "работчикам.";
			this.finistLabel7.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			// 
			// finistLabel5
			// 
			this.finistLabel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistLabel5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabel5.Location = new System.Drawing.Point(0, 0);
			this.finistLabel5.Name = "finistLabel5";
			this.finistLabel5.Size = new System.Drawing.Size(65, 13);
			this.finistLabel5.TabIndex = 0;
			this.finistLabel5.Text = "Внимание!";
			// 
			// flMethod
			// 
			this.flMethod.Location = new System.Drawing.Point(118, 94);
			this.flMethod.Name = "flMethod";
			this.flMethod.Size = new System.Drawing.Size(0, 0);
			this.flMethod.TabIndex = 19;
			// 
			// flFile
			// 
			this.flFile.Location = new System.Drawing.Point(118, 112);
			this.flFile.Name = "flFile";
			this.flFile.Size = new System.Drawing.Size(0, 0);
			this.flFile.TabIndex = 19;
			// 
			// flClass
			// 
			this.flClass.Location = new System.Drawing.Point(257, 76);
			this.flClass.Name = "flClass";
			this.flClass.Size = new System.Drawing.Size(0, 0);
			this.flClass.TabIndex = 19;
			// 
			// flLine
			// 
			this.flLine.Location = new System.Drawing.Point(419, 112);
			this.flLine.Name = "flLine";
			this.flLine.Size = new System.Drawing.Size(0, 0);
			this.flLine.TabIndex = 19;
			// 
			// flAsm
			// 
			this.flAsm.Location = new System.Drawing.Point(118, 76);
			this.flAsm.Name = "flAsm";
			this.flAsm.Size = new System.Drawing.Size(12, 13);
			this.flAsm.TabIndex = 19;
			this.flAsm.Text = "s";
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(378, 113);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Строка:";
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(75, 113);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(28, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Файл:";
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(75, 95);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "Метод:";
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(218, 77);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Класс:";
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(75, 77);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Сборка:";
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.flErrText);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point(69, 13);
			this.panel6.Name = "panel6";
			this.panel6.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.panel6.Size = new System.Drawing.Size(387, 56);
			this.panel6.TabIndex = 7;
			// 
			// flErrText
			// 
			this.flErrText.AutoSize = false;
			this.flErrText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flErrText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flErrText.Location = new System.Drawing.Point(0, 0);
			this.flErrText.Name = "flErrText";
			this.flErrText.Size = new System.Drawing.Size(384, 56);
			this.flErrText.TabIndex = 6;
			this.flErrText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.flErrText.WordWrap = true;
			// 
			// fpStack
			// 
			this.fpStack.Controls.Add(this.ftbStack);
			this.fpStack.Dock = System.Windows.Forms.DockStyle.Top;
			this.fpStack.Location = new System.Drawing.Point(0, 168);
			this.fpStack.MinimumSize = new System.Drawing.Size(0, 105);
			this.fpStack.Name = "fpStack";
			this.fpStack.Padding = new System.Windows.Forms.Padding(3);
			this.fpStack.Size = new System.Drawing.Size(456, 105);
			this.fpStack.TabIndex = 14;
			this.fpStack.Visible = false;
			// 
			// ftbStack
			// 
			this.ftbStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ftbStack.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.ftbStack.Location = new System.Drawing.Point(3, 3);
			this.ftbStack.Multiline = true;
			this.ftbStack.Name = "ftbStack";
			this.ftbStack.ReadOnly = true;
			this.ftbStack.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.ftbStack.Size = new System.Drawing.Size(450, 99);
			this.ftbStack.TabIndex = 0;
			this.ftbStack.WordWrap = false;
			// 
			// ModalErrorBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MinimumSize = new System.Drawing.Size(460, 0);
			this.MoveControl = this.finistPanelHeader;
			this.Name = "ModalErrorBox";
			this.Size = new System.Drawing.Size(460, 312);
			this.panel1.ResumeLayout(false);
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanelHeader.PerformLayout();
			this.finistPanelButtons.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.fpServMessage.ResumeLayout(false);
			this.fpServMessage.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.fpStack.ResumeLayout(false);
			this.fpStack.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

  }

  #endregion

  private SimPanel finistPanelHeader;
  internal SimLabel finistLabelCaption;
  private SimPanel finistPanelButtons;
  private SimButton buttonOK;
  private System.Windows.Forms.PictureBox pictureBox1;
  private System.Windows.Forms.Panel panel2;
  private SimLabel flErrCaption;
  private System.Windows.Forms.Panel fpStack;
  private System.Windows.Forms.Panel panel3;
  private SimLabel flErrText;
  private System.Windows.Forms.Panel panel6;
  private SimLabel flMethod;
  private SimLabel flFile;
  private SimLabel flClass;
  private SimLabel flLine;
  private SimLabel flAsm;
  private System.Windows.Forms.Label label5;
  private System.Windows.Forms.Label label4;
  private System.Windows.Forms.Label label3;
  private System.Windows.Forms.Label label2;
  private System.Windows.Forms.Label label1;
  private SimPanel fpServMessage;
  private SimLabel finistLabel7;
  private SimLabel finistLabel5;
  private System.Windows.Forms.Label label7;
  private SimButton button2;
  private SimTextBox ftbStack;
 }
}
