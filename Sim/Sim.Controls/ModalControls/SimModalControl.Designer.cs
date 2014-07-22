namespace Sim.Controls
{
 partial class SimModalControl
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

  #region Component Designer generated code

  /// <summary> 
  /// Required method for Designer support - do not modify 
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
   this.panel1 = new System.Windows.Forms.Panel();
   this.SuspendLayout();
   // 
   // panel1
   // 
   this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.panel1.Location = new System.Drawing.Point(2, 2);
   this.panel1.Name = "panel1";
   this.panel1.Size = new System.Drawing.Size(388, 376);
   this.panel1.TabIndex = 0;
   // 
   // SimModalControl
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.Controls.Add(this.panel1);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.Name = "SimModalControl";
   this.Padding = new System.Windows.Forms.Padding(2);
   this.Size = new System.Drawing.Size(392, 380);
   this.ResumeLayout(false);

  }

  #endregion
  /// <summary>
  /// 
  /// </summary>
  protected System.Windows.Forms.Panel panel1;


 }
}
