namespace Sim
{
 partial class ConParamsForm
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
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConParamsForm));
   this.groupBox1 = new System.Windows.Forms.GroupBox();
   this.radioButtonOther = new System.Windows.Forms.RadioButton();
   this.radioButtonCentral = new System.Windows.Forms.RadioButton();
   this.radioButtonAuto = new System.Windows.Forms.RadioButton();
   this.groupBox2 = new System.Windows.Forms.GroupBox();
   this.nudReceiveBuf = new System.Windows.Forms.NumericUpDown();
   this.nudReceiveTimeout = new System.Windows.Forms.NumericUpDown();
   this.nudSendBuf = new System.Windows.Forms.NumericUpDown();
   this.nudSendTimeout = new System.Windows.Forms.NumericUpDown();
   this.label6 = new System.Windows.Forms.Label();
   this.nudPort = new System.Windows.Forms.NumericUpDown();
   this.label4 = new System.Windows.Forms.Label();
   this.label5 = new System.Windows.Forms.Label();
   this.textBoxServer = new Sim.Controls.SimTextBox();
   this.label3 = new System.Windows.Forms.Label();
   this.label2 = new System.Windows.Forms.Label();
   this.label1 = new System.Windows.Forms.Label();
   this.checkBoxAlways = new Sim.Controls.SimCheckBox();
   this.buttonTest = new Sim.Controls.SimButton();
   this.buttonOk = new Sim.Controls.SimButton();
   this.buttonCancel = new Sim.Controls.SimButton();
   this.groupBox1.SuspendLayout();
   this.groupBox2.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.nudReceiveBuf)).BeginInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudReceiveTimeout)).BeginInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudSendBuf)).BeginInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudSendTimeout)).BeginInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
   this.SuspendLayout();
   // 
   // groupBox1
   // 
   this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.groupBox1.Controls.Add(this.radioButtonOther);
   this.groupBox1.Controls.Add(this.radioButtonCentral);
   this.groupBox1.Controls.Add(this.radioButtonAuto);
   this.groupBox1.Location = new System.Drawing.Point(7, 4);
   this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
   this.groupBox1.Name = "groupBox1";
   this.groupBox1.Size = new System.Drawing.Size(390, 89);
   this.groupBox1.TabIndex = 0;
   this.groupBox1.TabStop = false;
   this.groupBox1.Text = " Сервер ";
   // 
   // radioButtonOther
   // 
   this.radioButtonOther.AutoSize = true;
   this.radioButtonOther.Location = new System.Drawing.Point(6, 66);
   this.radioButtonOther.Name = "radioButtonOther";
   this.radioButtonOther.Size = new System.Drawing.Size(62, 17);
   this.radioButtonOther.TabIndex = 4;
   this.radioButtonOther.Text = "Другой";
   this.radioButtonOther.UseVisualStyleBackColor = true;
   this.radioButtonOther.CheckedChanged += new System.EventHandler(this.radioButtonType_CheckedChanged);
   // 
   // radioButtonCentral
   // 
   this.radioButtonCentral.AutoSize = true;
   this.radioButtonCentral.Location = new System.Drawing.Point(6, 43);
   this.radioButtonCentral.Name = "radioButtonCentral";
   this.radioButtonCentral.Size = new System.Drawing.Size(133, 17);
   this.radioButtonCentral.TabIndex = 1;
   this.radioButtonCentral.Text = "Центральный сервер";
   this.radioButtonCentral.UseVisualStyleBackColor = true;
   this.radioButtonCentral.CheckedChanged += new System.EventHandler(this.radioButtonType_CheckedChanged);
   // 
   // radioButtonAuto
   // 
   this.radioButtonAuto.AutoSize = true;
   this.radioButtonAuto.Checked = true;
   this.radioButtonAuto.Location = new System.Drawing.Point(6, 20);
   this.radioButtonAuto.Name = "radioButtonAuto";
   this.radioButtonAuto.Size = new System.Drawing.Size(223, 17);
   this.radioButtonAuto.TabIndex = 0;
   this.radioButtonAuto.TabStop = true;
   this.radioButtonAuto.Text = "Автоматическое определение сервера";
   this.radioButtonAuto.UseVisualStyleBackColor = true;
   this.radioButtonAuto.CheckedChanged += new System.EventHandler(this.radioButtonType_CheckedChanged);
   // 
   // groupBox2
   // 
   this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.groupBox2.Controls.Add(this.nudReceiveBuf);
   this.groupBox2.Controls.Add(this.nudReceiveTimeout);
   this.groupBox2.Controls.Add(this.nudSendBuf);
   this.groupBox2.Controls.Add(this.nudSendTimeout);
   this.groupBox2.Controls.Add(this.label6);
   this.groupBox2.Controls.Add(this.nudPort);
   this.groupBox2.Controls.Add(this.label4);
   this.groupBox2.Controls.Add(this.label5);
   this.groupBox2.Controls.Add(this.textBoxServer);
   this.groupBox2.Controls.Add(this.label3);
   this.groupBox2.Controls.Add(this.label2);
   this.groupBox2.Controls.Add(this.label1);
   this.groupBox2.Location = new System.Drawing.Point(7, 96);
   this.groupBox2.Name = "groupBox2";
   this.groupBox2.Size = new System.Drawing.Size(390, 115);
   this.groupBox2.TabIndex = 5;
   this.groupBox2.TabStop = false;
   this.groupBox2.Text = " Параметры соединения ";
   // 
   // nudReceiveBuf
   // 
   this.nudReceiveBuf.Location = new System.Drawing.Point(320, 87);
   this.nudReceiveBuf.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
   this.nudReceiveBuf.Name = "nudReceiveBuf";
   this.nudReceiveBuf.Size = new System.Drawing.Size(64, 21);
   this.nudReceiveBuf.TabIndex = 3;
   this.nudReceiveBuf.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
   // 
   // nudReceiveTimeout
   // 
   this.nudReceiveTimeout.Location = new System.Drawing.Point(144, 86);
   this.nudReceiveTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
   this.nudReceiveTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
   this.nudReceiveTimeout.Name = "nudReceiveTimeout";
   this.nudReceiveTimeout.Size = new System.Drawing.Size(49, 21);
   this.nudReceiveTimeout.TabIndex = 3;
   this.nudReceiveTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
   // 
   // nudSendBuf
   // 
   this.nudSendBuf.Location = new System.Drawing.Point(320, 60);
   this.nudSendBuf.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
   this.nudSendBuf.Name = "nudSendBuf";
   this.nudSendBuf.Size = new System.Drawing.Size(64, 21);
   this.nudSendBuf.TabIndex = 3;
   this.nudSendBuf.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
   // 
   // nudSendTimeout
   // 
   this.nudSendTimeout.Location = new System.Drawing.Point(144, 59);
   this.nudSendTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
   this.nudSendTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
   this.nudSendTimeout.Name = "nudSendTimeout";
   this.nudSendTimeout.Size = new System.Drawing.Size(49, 21);
   this.nudSendTimeout.TabIndex = 3;
   this.nudSendTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
   // 
   // label6
   // 
   this.label6.AutoSize = true;
   this.label6.Location = new System.Drawing.Point(199, 90);
   this.label6.Name = "label6";
   this.label6.Size = new System.Drawing.Size(109, 13);
   this.label6.TabIndex = 0;
   this.label6.Text = "Буфер приема, байт";
   // 
   // nudPort
   // 
   this.nudPort.Enabled = false;
   this.nudPort.Location = new System.Drawing.Point(320, 34);
   this.nudPort.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
   this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
   this.nudPort.Name = "nudPort";
   this.nudPort.Size = new System.Drawing.Size(64, 21);
   this.nudPort.TabIndex = 2;
   this.nudPort.Value = new decimal(new int[] {
            5021,
            0,
            0,
            0});
   // 
   // label4
   // 
   this.label4.AutoSize = true;
   this.label4.Location = new System.Drawing.Point(199, 63);
   this.label4.Name = "label4";
   this.label4.Size = new System.Drawing.Size(121, 13);
   this.label4.TabIndex = 0;
   this.label4.Text = "Буфер отправки, байт";
   // 
   // label5
   // 
   this.label5.AutoSize = true;
   this.label5.Location = new System.Drawing.Point(6, 89);
   this.label5.Name = "label5";
   this.label5.Size = new System.Drawing.Size(120, 13);
   this.label5.TabIndex = 0;
   this.label5.Text = "Тайм-аут приема, сек.";
   // 
   // textBoxServer
   // 
   this.textBoxServer.Enabled = false;
   this.textBoxServer.Location = new System.Drawing.Point(6, 33);
   this.textBoxServer.Name = "textBoxServer";
   this.textBoxServer.Size = new System.Drawing.Size(308, 21);
   this.textBoxServer.TabIndex = 1;
   // 
   // label3
   // 
   this.label3.AutoSize = true;
   this.label3.Location = new System.Drawing.Point(6, 62);
   this.label3.Name = "label3";
   this.label3.Size = new System.Drawing.Size(132, 13);
   this.label3.TabIndex = 0;
   this.label3.Text = "Тайм-аут отправки, сек.";
   // 
   // label2
   // 
   this.label2.AutoSize = true;
   this.label2.Location = new System.Drawing.Point(317, 17);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(32, 13);
   this.label2.TabIndex = 0;
   this.label2.Text = "Порт";
   // 
   // label1
   // 
   this.label1.AutoSize = true;
   this.label1.Location = new System.Drawing.Point(6, 17);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(124, 13);
   this.label1.TabIndex = 0;
   this.label1.Text = "Имя (IP адрес) сервера";
   // 
   // checkBoxAlways
   // 
   this.checkBoxAlways.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
   this.checkBoxAlways.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   this.checkBoxAlways.Location = new System.Drawing.Point(13, 216);
   this.checkBoxAlways.Name = "checkBoxAlways";
   this.checkBoxAlways.Size = new System.Drawing.Size(262, 17);
   this.checkBoxAlways.TabIndex = 2;
   this.checkBoxAlways.Text = "Всегда использовать выбранное соединение";
   // 
   // buttonTest
   // 
   this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
   this.buttonTest.Image = null;
   this.buttonTest.Location = new System.Drawing.Point(7, 239);
   this.buttonTest.Name = "buttonTest";
   this.buttonTest.Size = new System.Drawing.Size(139, 25);
   this.buttonTest.TabIndex = 3;
   this.buttonTest.Text = "Тест соединения";
   this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
   // 
   // buttonOk
   // 
   this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
   this.buttonOk.Image = global::Sim.Shell.Properties.Resources.OK;
   this.buttonOk.Location = new System.Drawing.Point(237, 239);
   this.buttonOk.Name = "buttonOk";
   this.buttonOk.Size = new System.Drawing.Size(77, 25);
   this.buttonOk.TabIndex = 4;
   this.buttonOk.Text = "OK";
   this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
   // 
   // buttonCancel
   // 
   this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
   this.buttonCancel.Image = global::Sim.Shell.Properties.Resources.Cancel;
   this.buttonCancel.Location = new System.Drawing.Point(320, 239);
   this.buttonCancel.Name = "buttonCancel";
   this.buttonCancel.Size = new System.Drawing.Size(77, 25);
   this.buttonCancel.TabIndex = 5;
   this.buttonCancel.Text = "Отмена";
   this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
   // 
   // ConParamsForm
   // 
   this.AcceptButton = this.buttonOk;
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.CancelButton = this.buttonCancel;
   this.ClientSize = new System.Drawing.Size(405, 272);
   this.Controls.Add(this.checkBoxAlways);
   this.Controls.Add(this.buttonTest);
   this.Controls.Add(this.buttonOk);
   this.Controls.Add(this.buttonCancel);
   this.Controls.Add(this.groupBox1);
   this.Controls.Add(this.groupBox2);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
   this.MaximizeBox = false;
   this.MinimizeBox = false;
   this.Name = "ConParamsForm";
   this.ShowInTaskbar = false;
   this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
   this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
   this.Text = "Параметры соединения с сервером Пульсара";
   this.Load += new System.EventHandler(this.FormConnections_Load);
   this.groupBox1.ResumeLayout(false);
   this.groupBox1.PerformLayout();
   this.groupBox2.ResumeLayout(false);
   this.groupBox2.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.nudReceiveBuf)).EndInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudReceiveTimeout)).EndInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudSendBuf)).EndInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudSendTimeout)).EndInit();
   ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
   this.ResumeLayout(false);
   this.PerformLayout();

  }

  #endregion

  private System.Windows.Forms.GroupBox groupBox1;
  private Sim.Controls.SimButton buttonCancel;
  private Sim.Controls.SimButton buttonOk;
  private System.Windows.Forms.RadioButton radioButtonOther;
  private System.Windows.Forms.RadioButton radioButtonCentral;
  private System.Windows.Forms.RadioButton radioButtonAuto;
  private Sim.Controls.SimButton buttonTest;
  private Sim.Controls.SimCheckBox checkBoxAlways;
  private System.Windows.Forms.GroupBox groupBox2;
  private Sim.Controls.SimTextBox textBoxServer;
  private System.Windows.Forms.Label label1;
  private System.Windows.Forms.NumericUpDown nudSendBuf;
  private System.Windows.Forms.NumericUpDown nudSendTimeout;
  private System.Windows.Forms.NumericUpDown nudPort;
  private System.Windows.Forms.Label label4;
  private System.Windows.Forms.Label label3;
  private System.Windows.Forms.Label label2;
  private System.Windows.Forms.NumericUpDown nudReceiveBuf;
  private System.Windows.Forms.NumericUpDown nudReceiveTimeout;
  private System.Windows.Forms.Label label6;
  private System.Windows.Forms.Label label5;
 }
}