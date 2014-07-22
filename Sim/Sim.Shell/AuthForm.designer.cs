namespace Sim
{
 partial class AuthForm
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
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthForm));
   this.groupBox1 = new System.Windows.Forms.GroupBox();
   this.labelServer = new System.Windows.Forms.Label();
   this.buttonConnect = new Sim.Controls.SimButton();
   this.label3 = new System.Windows.Forms.Label();
   this.textBoxPassword = new Sim.Controls.SimTextBox();
   this.label2 = new System.Windows.Forms.Label();
   this.comboBoxUsers = new Sim.Controls.SimComboBox();
   this.label1 = new System.Windows.Forms.Label();
   this.buttonCancel = new Sim.Controls.SimButton();
   this.buttonOk = new Sim.Controls.SimButton();
   this.pictureBox1 = new System.Windows.Forms.PictureBox();
   this.groupBox1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
   this.SuspendLayout();
   // 
   // groupBox1
   // 
   this.groupBox1.Controls.Add(this.labelServer);
   this.groupBox1.Controls.Add(this.buttonConnect);
   this.groupBox1.Controls.Add(this.label3);
   this.groupBox1.Controls.Add(this.textBoxPassword);
   this.groupBox1.Controls.Add(this.label2);
   this.groupBox1.Controls.Add(this.comboBoxUsers);
   this.groupBox1.Controls.Add(this.label1);
   this.groupBox1.Location = new System.Drawing.Point(86, 8);
   this.groupBox1.Name = "groupBox1";
   this.groupBox1.Size = new System.Drawing.Size(295, 129);
   this.groupBox1.TabIndex = 1;
   this.groupBox1.TabStop = false;
   this.groupBox1.Text = " Данные аутентификации ";
   // 
   // labelServer
   // 
   this.labelServer.AutoEllipsis = true;
   this.labelServer.Location = new System.Drawing.Point(60, 100);
   this.labelServer.Name = "labelServer";
   this.labelServer.Size = new System.Drawing.Size(198, 20);
   this.labelServer.TabIndex = 5;
   this.labelServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // buttonConnect
   // 
   this.buttonConnect.Image = global::Sim.Shell.Properties.Resources.ConString;
   this.buttonConnect.Location = new System.Drawing.Point(264, 99);
   this.buttonConnect.Name = "buttonConnect";
   this.buttonConnect.Size = new System.Drawing.Size(23, 23);
   this.buttonConnect.TabIndex = 4;
   this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
   // 
   // label3
   // 
   this.label3.AutoSize = true;
   this.label3.Location = new System.Drawing.Point(6, 104);
   this.label3.Name = "label3";
   this.label3.Size = new System.Drawing.Size(48, 13);
   this.label3.TabIndex = 3;
   this.label3.Text = "Сервер:";
   // 
   // textBoxPassword
   // 
   this.textBoxPassword.Location = new System.Drawing.Point(6, 72);
   this.textBoxPassword.Name = "textBoxPassword";
   this.textBoxPassword.ShortcutsEnabled = false;
   this.textBoxPassword.Size = new System.Drawing.Size(281, 21);
   this.textBoxPassword.TabIndex = 1;
   this.textBoxPassword.UseSystemPasswordChar = true;
   this.textBoxPassword.WordWrap = false;
   this.textBoxPassword.TextChanged += new System.EventHandler(this.comboBoxUsers_SelectionChangeCommitted);
   this.textBoxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPassword_KeyPress);
   // 
   // label2
   // 
   this.label2.AutoSize = true;
   this.label2.Location = new System.Drawing.Point(6, 56);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(44, 13);
   this.label2.TabIndex = 2;
   this.label2.Text = "Пароль";
   // 
   // comboBoxUsers
   // 
   this.comboBoxUsers.Location = new System.Drawing.Point(6, 32);
   this.comboBoxUsers.Name = "comboBoxUsers";
   this.comboBoxUsers.SelectedIndex = -1;
   this.comboBoxUsers.SelectedItem = null;
   this.comboBoxUsers.Size = new System.Drawing.Size(281, 21);
   this.comboBoxUsers.TabIndex = 0;
   this.comboBoxUsers.UISelectedItemChanged += new System.EventHandler(this.comboBoxUsers_SelectionChangeCommitted);
   // 
   // label1
   // 
   this.label1.AutoSize = true;
   this.label1.Location = new System.Drawing.Point(6, 16);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(79, 13);
   this.label1.TabIndex = 0;
   this.label1.Text = "Пользователь";
   // 
   // buttonCancel
   // 
   this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonCancel.Image = global::Sim.Shell.Properties.Resources.Cancel;
   this.buttonCancel.Location = new System.Drawing.Point(297, 143);
   this.buttonCancel.Name = "buttonCancel";
   this.buttonCancel.Size = new System.Drawing.Size(84, 26);
   this.buttonCancel.TabIndex = 3;
   this.buttonCancel.Text = "Отмена";
   this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
   // 
   // buttonOk
   // 
   this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonOk.Enabled = false;
   this.buttonOk.Image = global::Sim.Shell.Properties.Resources.OK;
   this.buttonOk.Location = new System.Drawing.Point(207, 143);
   this.buttonOk.Name = "buttonOk";
   this.buttonOk.Size = new System.Drawing.Size(84, 26);
   this.buttonOk.TabIndex = 2;
   this.buttonOk.Text = "Вход";
   this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
   // 
   // pictureBox1
   // 
   this.pictureBox1.Image = global::Sim.Shell.Properties.Resources.UsersAuth;
   this.pictureBox1.Location = new System.Drawing.Point(13, 33);
   this.pictureBox1.Name = "pictureBox1";
   this.pictureBox1.Size = new System.Drawing.Size(60, 60);
   this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
   this.pictureBox1.TabIndex = 0;
   this.pictureBox1.TabStop = false;
   // 
   // AuthForm
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.ClientSize = new System.Drawing.Size(388, 176);
   this.Controls.Add(this.buttonCancel);
   this.Controls.Add(this.buttonOk);
   this.Controls.Add(this.groupBox1);
   this.Controls.Add(this.pictureBox1);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
   this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
   this.MaximizeBox = false;
   this.MinimizeBox = false;
   this.Name = "AuthForm";
   this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
   this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
   this.Text = "Аутентификация пользователей";
   this.Load += new System.EventHandler(this.AuthForm_Load);
   this.groupBox1.ResumeLayout(false);
   this.groupBox1.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.PictureBox pictureBox1;
  private System.Windows.Forms.GroupBox groupBox1;
  private System.Windows.Forms.Label label2;
  private System.Windows.Forms.Label label1;
  private Sim.Controls.SimButton buttonOk;
  private Sim.Controls.SimButton buttonCancel;
  internal Sim.Controls.SimTextBox textBoxPassword;
  internal Sim.Controls.SimComboBox comboBoxUsers;
  private Sim.Controls.SimButton buttonConnect;
  private System.Windows.Forms.Label label3;
  private System.Windows.Forms.Label labelServer;
 }
}