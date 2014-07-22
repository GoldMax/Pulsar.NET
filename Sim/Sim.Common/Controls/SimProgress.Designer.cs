namespace Sim.Controls
{
 partial class SimProgress
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.flCircle = new Sim.Controls.SimLabel();
			this.finistLabel1 = new Sim.Controls.SimLabel();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.flCircle);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.finistLabel1);
			this.splitContainer1.Size = new System.Drawing.Size(365, 317);
			this.splitContainer1.SplitterDistance = 147;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 2;
			// 
			// flCircle
			// 
			this.flCircle.AutoSize = false;
			this.flCircle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flCircle.EventsTransparent = true;
			this.flCircle.Image = global::Sim.Common.Properties.Resource1._1;
			this.flCircle.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.flCircle.Location = new System.Drawing.Point(0, 0);
			this.flCircle.Name = "flCircle";
			this.flCircle.Size = new System.Drawing.Size(365, 147);
			this.flCircle.TabIndex = 0;
			// 
			// finistLabel1
			// 
			this.finistLabel1.AutoSize = false;
			this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabel1.EventsTransparent = true;
			this.finistLabel1.ForceDockSize = true;
			this.finistLabel1.Image = global::Sim.Common.Properties.Resources.Text;
			this.finistLabel1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.finistLabel1.Location = new System.Drawing.Point(0, 0);
			this.finistLabel1.Name = "finistLabel1";
			this.finistLabel1.Size = new System.Drawing.Size(365, 169);
			this.finistLabel1.TabIndex = 0;
			// 
			// SimProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "SimProgress";
			this.Size = new System.Drawing.Size(365, 317);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.SplitContainer splitContainer1;
  private SimLabel flCircle;
  private SimLabel finistLabel1;

 }
}
