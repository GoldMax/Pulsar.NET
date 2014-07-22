namespace Sim
{
 partial class NetProgressControl
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
   if(timer != null)
    lock(imgs)
    {
     timer.Dispose();
     timer = null;
     return;
    }
   if(disposing && (components != null))
   {
    components.Dispose();
   }
   base.Dispose(disposing);
  }

  #region Component Designer generated code

  /// <summary> 
  /// Required method for Designer support - do not modify 
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
   this.components = new System.ComponentModel.Container();
   this.navigatorPanel1 = new Sim.NetProgressControl.NavigatorPanel();
   this.buttonCancel = new Sim.Controls.SimButton();
   this.labelTime = new Controls.SimLabel();
   this.label1 = new System.Windows.Forms.Label();
   this.labelText = new System.Windows.Forms.Label();
   this.finistLabel1 = new Sim.Controls.SimLabel();
   this.fpImage = new Sim.Controls.SimPanel();
   this.navigatorPanel1.SuspendLayout();
   this.SuspendLayout();
   // 
   // navigatorPanel1
   // 
   this.navigatorPanel1.Controls.Add(this.fpImage);
   this.navigatorPanel1.Controls.Add(this.buttonCancel);
   this.navigatorPanel1.Controls.Add(this.labelTime);
   this.navigatorPanel1.Controls.Add(this.label1);
   this.navigatorPanel1.Controls.Add(this.labelText);
   this.navigatorPanel1.Controls.Add(this.finistLabel1);
   this.navigatorPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.navigatorPanel1.Location = new System.Drawing.Point(2, 2);
   this.navigatorPanel1.Name = "navigatorPanel1";
   this.navigatorPanel1.Padding = new System.Windows.Forms.Padding(3);
   this.navigatorPanel1.Size = new System.Drawing.Size(331, 97);
   this.navigatorPanel1.TabIndex = 4;
   // 
   // buttonCancel
   // 
   this.buttonCancel.Image = global::Sim.Shell.Properties.Resources.Cancel;
   this.buttonCancel.Location = new System.Drawing.Point(210, 64);
   this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
   this.buttonCancel.Name = "buttonCancel";
   this.buttonCancel.Size = new System.Drawing.Size(93, 23);
   this.buttonCancel.TabIndex = 1;
   this.buttonCancel.Text = "Отменить";
   this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
   // 
   // labelTime
   // 
   this.labelTime.AutoEllipsis = true;
   this.labelTime.BackColor = System.Drawing.Color.Transparent;
   this.labelTime.Location = new System.Drawing.Point(116, 70);
   this.labelTime.Name = "labelTime";
   this.labelTime.Size = new System.Drawing.Size(56, 16);
   this.labelTime.TabIndex = 3;
   this.labelTime.Text = "00:00:00";
   // 
   // label1
   // 
   this.label1.AutoSize = true;
   this.label1.BackColor = System.Drawing.Color.Transparent;
   this.label1.Location = new System.Drawing.Point(8, 70);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(106, 13);
   this.label1.TabIndex = 2;
   this.label1.Text = "Время выполнения:";
   // 
   // labelText
   // 
   this.labelText.AutoEllipsis = true;
   this.labelText.BackColor = System.Drawing.Color.Transparent;
   this.labelText.Location = new System.Drawing.Point(116, 24);
   this.labelText.Name = "labelText";
   this.labelText.Size = new System.Drawing.Size(209, 36);
   this.labelText.TabIndex = 4;
   this.labelText.Text = "Идет обмен данными с сервером. Пожалуйста, подождите ...";
   this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
   // 
   // finistLabel1
   // 
   this.finistLabel1.AutoEllipsis = true;
   this.finistLabel1.BackColor = System.Drawing.Color.Transparent;
   this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Top;
   this.finistLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.finistLabel1.Location = new System.Drawing.Point(3, 3);
   this.finistLabel1.Name = "finistLabel1";
   this.finistLabel1.Size = new System.Drawing.Size(325, 16);
   this.finistLabel1.TabIndex = 4;
   this.finistLabel1.Text = "Обмен данными с сервером";
   this.finistLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
   // 
   // fpImage
   // 
   this.fpImage.BackColor = System.Drawing.Color.Transparent;
   this.fpImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
   this.fpImage.Location = new System.Drawing.Point(6, 24);
   this.fpImage.Name = "fpImage";
   this.fpImage.Size = new System.Drawing.Size(104, 34);
   this.fpImage.TabIndex = 5;
   // 
   // NetProgressControl
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
   this.CausesValidation = false;
   this.Controls.Add(this.navigatorPanel1);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.Name = "NetProgressControl";
   this.Padding = new System.Windows.Forms.Padding(2);
   this.Size = new System.Drawing.Size(335, 101);
   this.Load += new System.EventHandler(this.NetProgressControl_Load);
   this.ParentChanged += new System.EventHandler(this.NetProgressControl_ParentChanged);
   this.navigatorPanel1.ResumeLayout(false);
   this.navigatorPanel1.PerformLayout();
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.Label labelText;
  private Controls.SimLabel labelTime;
  private System.Windows.Forms.Label label1;
  internal Sim.Controls.SimButton buttonCancel;
  private NavigatorPanel navigatorPanel1;
  private Controls.SimLabel finistLabel1;
  private Controls.SimPanel fpImage;

 }
}
