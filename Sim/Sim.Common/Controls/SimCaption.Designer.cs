namespace Sim.Controls
{      
 partial class SimCaption
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
   this.finistLabel1 = new Sim.Controls.SimLabel();
   this.SuspendLayout();
   // 
   // finistLabel1
   // 
   this.finistLabel1.AutoSize = false;
   this.finistLabel1.BackColor = System.Drawing.Color.Transparent;
   this.finistLabel1.BackColor2 = System.Drawing.Color.Transparent;
   this.finistLabel1.BackColorMiddle = System.Drawing.SystemColors.ControlLightLight;
   this.finistLabel1.BackColorMiddlePosition = 0F;
   this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.finistLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.finistLabel1.ForceDockSize = true;
   this.finistLabel1.GradientMode = Sim.Controls.GradientMode.TrioHorizontal;
   this.finistLabel1.Location = new System.Drawing.Point(3, 3);
   this.finistLabel1.Name = "finistLabel1";
   this.finistLabel1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
   this.finistLabel1.Size = new System.Drawing.Size(94, 19);
   this.finistLabel1.TabIndex = 0;
   this.finistLabel1.Text = "Заголовок";
   // 
   // SimCaption
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   this.Controls.Add(this.finistLabel1);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.MinimumSize = new System.Drawing.Size(100, 0);
   this.Name = "SimCaption";
   this.Padding = new System.Windows.Forms.Padding(3);
   this.Size = new System.Drawing.Size(100, 25);
   this.ResumeLayout(false);

  }

  #endregion

  private SimLabel finistLabel1;


 }
}
