namespace Sim
{
	partial class FormUpdateProgress
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateProgress));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.nudReceiveBuf = new System.Windows.Forms.NumericUpDown();
			this.radioButtonOther = new System.Windows.Forms.RadioButton();
			this.checkBoxAlways = new System.Windows.Forms.CheckBox();
			this.nudReceiveTimeout = new System.Windows.Forms.NumericUpDown();
			this.radioButtonCentral = new System.Windows.Forms.RadioButton();
			this.nudSendBuf = new System.Windows.Forms.NumericUpDown();
			this.radioButtonAuto = new System.Windows.Forms.RadioButton();
			this.nudSendTimeout = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.nudPort = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxServer = new System.Windows.Forms.TextBox();
			this.buttonTest = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel10 = new System.Windows.Forms.Panel();
			this.labelStepText = new System.Windows.Forms.Label();
			this.panelParams = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.panelError = new System.Windows.Forms.Panel();
			this.labelError = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBoxLoad = new System.Windows.Forms.PictureBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudReceiveBuf)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudReceiveTimeout)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSendBuf)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSendTimeout)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panelParams.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panelError.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoad)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.nudReceiveBuf);
			this.groupBox1.Controls.Add(this.radioButtonOther);
			this.groupBox1.Controls.Add(this.checkBoxAlways);
			this.groupBox1.Controls.Add(this.nudReceiveTimeout);
			this.groupBox1.Controls.Add(this.radioButtonCentral);
			this.groupBox1.Controls.Add(this.nudSendBuf);
			this.groupBox1.Controls.Add(this.radioButtonAuto);
			this.groupBox1.Controls.Add(this.nudSendTimeout);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.nudPort);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textBoxServer);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 222);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Параметры соединения ";
			// 
			// nudReceiveBuf
			// 
			this.nudReceiveBuf.Location = new System.Drawing.Point(329, 164);
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
			// checkBoxAlways
			// 
			this.checkBoxAlways.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxAlways.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxAlways.Location = new System.Drawing.Point(15, 190);
			this.checkBoxAlways.Name = "checkBoxAlways";
			this.checkBoxAlways.Size = new System.Drawing.Size(262, 17);
			this.checkBoxAlways.TabIndex = 2;
			this.checkBoxAlways.Text = "Всегда использовать выбранное соединение";
			// 
			// nudReceiveTimeout
			// 
			this.nudReceiveTimeout.Location = new System.Drawing.Point(153, 163);
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
												50,
												0,
												0,
												0});
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
			// nudSendBuf
			// 
			this.nudSendBuf.Location = new System.Drawing.Point(329, 137);
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
			// nudSendTimeout
			// 
			this.nudSendTimeout.Location = new System.Drawing.Point(153, 136);
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
												50,
												0,
												0,
												0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(208, 140);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(121, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Буфер отправки, байт";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(208, 167);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(109, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Буфер приема, байт";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 94);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Имя (IP адрес) сервера";
			// 
			// nudPort
			// 
			this.nudPort.Enabled = false;
			this.nudPort.Location = new System.Drawing.Point(329, 111);
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
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(326, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Порт";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 139);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(132, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Тайм-аут отправки, сек.";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(15, 166);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Тайм-аут приема, сек.";
			// 
			// textBoxServer
			// 
			this.textBoxServer.Enabled = false;
			this.textBoxServer.Location = new System.Drawing.Point(15, 110);
			this.textBoxServer.Name = "textBoxServer";
			this.textBoxServer.Size = new System.Drawing.Size(308, 21);
			this.textBoxServer.TabIndex = 1;
			// 
			// buttonTest
			// 
			this.buttonTest.Location = new System.Drawing.Point(9, 6);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new System.Drawing.Size(139, 25);
			this.buttonTest.TabIndex = 3;
			this.buttonTest.Text = "Тест соединения";
			this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.panel10);
			this.panel1.Controls.Add(this.labelStepText);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 20);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(400, 85);
			this.panel1.TabIndex = 6;
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.pictureBoxLoad);
			this.panel10.Controls.Add(this.panelError);
			this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel10.Location = new System.Drawing.Point(0, 0);
			this.panel10.Name = "panel10";
			this.panel10.Padding = new System.Windows.Forms.Padding(5);
			this.panel10.Size = new System.Drawing.Size(400, 60);
			this.panel10.TabIndex = 7;
			// 
			// labelStepText
			// 
			this.labelStepText.AutoEllipsis = true;
			this.labelStepText.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.labelStepText.Location = new System.Drawing.Point(0, 60);
			this.labelStepText.Name = "labelStepText";
			this.labelStepText.Size = new System.Drawing.Size(400, 25);
			this.labelStepText.TabIndex = 0;
			this.labelStepText.Text = "Проверка соединения с сервером ...";
			this.labelStepText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panelParams
			// 
			this.panelParams.AutoSize = true;
			this.panelParams.Controls.Add(this.panel2);
			this.panelParams.Controls.Add(this.groupBox1);
			this.panelParams.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelParams.Location = new System.Drawing.Point(3, 105);
			this.panelParams.Name = "panelParams";
			this.panelParams.Size = new System.Drawing.Size(400, 258);
			this.panelParams.TabIndex = 7;
			this.panelParams.Visible = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.buttonCancel);
			this.panel2.Controls.Add(this.buttonOk);
			this.panel2.Controls.Add(this.buttonTest);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 222);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(400, 36);
			this.panel2.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.AutoSize = true;
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel3.Controls.Add(this.panel4);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Padding = new System.Windows.Forms.Padding(1);
			this.panel3.Size = new System.Drawing.Size(408, 365);
			this.panel3.TabIndex = 8;
			// 
			// panel4
			// 
			this.panel4.AutoSize = true;
			this.panel4.BackColor = System.Drawing.SystemColors.Control;
			this.panel4.Controls.Add(this.panelParams);
			this.panel4.Controls.Add(this.panel1);
			this.panel4.Controls.Add(this.label7);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(1, 1);
			this.panel4.Name = "panel4";
			this.panel4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.panel4.Size = new System.Drawing.Size(406, 363);
			this.panel4.TabIndex = 0;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.label7.Dock = System.Windows.Forms.DockStyle.Top;
			this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label7.Location = new System.Drawing.Point(3, 3);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(400, 17);
			this.label7.TabIndex = 9;
			this.label7.Text = "Обновление СИМ";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Image = global::Sim.Properties.Resources.OK;
			this.buttonOk.Location = new System.Drawing.Point(236, 6);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(77, 25);
			this.buttonOk.TabIndex = 4;
			this.buttonOk.Text = "OK";
			this.buttonOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Image = global::Sim.Properties.Resources.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(319, 6);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(77, 25);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// panelError
			// 
			this.panelError.Controls.Add(this.labelError);
			this.panelError.Controls.Add(this.pictureBox2);
			this.panelError.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelError.Location = new System.Drawing.Point(5, 5);
			this.panelError.Name = "panelError";
			this.panelError.Size = new System.Drawing.Size(390, 50);
			this.panelError.TabIndex = 0;
			// 
			// labelError
			// 
			this.labelError.AutoEllipsis = true;
			this.labelError.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelError.Location = new System.Drawing.Point(48, 0);
			this.labelError.Name = "labelError";
			this.labelError.Size = new System.Drawing.Size(342, 50);
			this.labelError.TabIndex = 3;
			this.labelError.Text = "Ошибка!\r\nСервер недоступен!";
			this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBox2.Image = global::Sim.Properties.Resources.Error48;
			this.pictureBox2.Location = new System.Drawing.Point(0, 0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(48, 50);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox2.TabIndex = 2;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBoxLoad
			// 
			this.pictureBoxLoad.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxLoad.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLoad.Image")));
			this.pictureBoxLoad.Location = new System.Drawing.Point(5, 5);
			this.pictureBoxLoad.Name = "pictureBoxLoad";
			this.pictureBoxLoad.Size = new System.Drawing.Size(390, 50);
			this.pictureBoxLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxLoad.TabIndex = 10;
			this.pictureBoxLoad.TabStop = false;
			// 
			// FormUpdateProgress
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(414, 375);
			this.Controls.Add(this.panel3);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(414, 0);
			this.Name = "FormUpdateProgress";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "СИМ - Обновление";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudReceiveBuf)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudReceiveTimeout)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSendBuf)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSendTimeout)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel10.ResumeLayout(false);
			this.panelParams.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panelError.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoad)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.RadioButton radioButtonOther;
		private System.Windows.Forms.RadioButton radioButtonCentral;
		private System.Windows.Forms.RadioButton radioButtonAuto;
		private System.Windows.Forms.Button buttonTest;
		private System.Windows.Forms.CheckBox checkBoxAlways;
		private System.Windows.Forms.TextBox textBoxServer;
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
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Label labelStepText;
		private System.Windows.Forms.Panel panelParams;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel panelError;
		private System.Windows.Forms.Label labelError;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBoxLoad;
	}
}