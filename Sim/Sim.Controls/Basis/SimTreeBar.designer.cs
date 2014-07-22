namespace Sim.Controls
{
 partial class SimTreeBar
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
   this.components = new System.ComponentModel.Container();
   Sim.Controls.SimToolStripColorTable finistToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
   this.contextMenuStripYear = new System.Windows.Forms.ContextMenuStrip(this.components);
   this.finistPanel1 = new Sim.Controls.SimPanel();
   this.toolStrip1 = new Sim.Controls.SimToolStrip();
   this.rootQueueButton = new System.Windows.Forms.ToolStripSplitButton();
   this.finistPanel1.SuspendLayout();
   this.toolStrip1.SuspendLayout();
   this.SuspendLayout();
   // 
   // contextMenuStripYear
   // 
   this.contextMenuStripYear.Name = "contextMenuStripYear";
   this.contextMenuStripYear.ShowImageMargin = false;
   this.contextMenuStripYear.Size = new System.Drawing.Size(36, 4);
   // 
   // finistPanel1
   // 
   this.finistPanel1.AutoSize = true;
   this.finistPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   this.finistPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
   this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.finistPanel1.Controls.Add(this.toolStrip1);
   this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Top;
   this.finistPanel1.Location = new System.Drawing.Point(0, 0);
   this.finistPanel1.Name = "finistPanel1";
   this.finistPanel1.Size = new System.Drawing.Size(655, 25);
   this.finistPanel1.TabIndex = 1;
   // 
   // toolStrip1
   // 
   finistToolStripColorTable1.UseSystemColors = false;
   this.toolStrip1.ColorTable = finistToolStripColorTable1;
   this.toolStrip1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
   this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rootQueueButton});
   this.toolStrip1.Location = new System.Drawing.Point(0, 0);
   this.toolStrip1.Name = "toolStrip1";
   this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
   this.toolStrip1.Size = new System.Drawing.Size(653, 25);
   this.toolStrip1.TabIndex = 0;
   // 
   // rootQueueButton
   // 
   this.rootQueueButton.AutoSize = false;
   this.rootQueueButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
   this.rootQueueButton.DropDownButtonWidth = 15;
   this.rootQueueButton.ImageTransparentColor = System.Drawing.Color.Magenta;
   this.rootQueueButton.Name = "rootQueueButton";
   this.rootQueueButton.Size = new System.Drawing.Size(15, 22);
   this.rootQueueButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
   this.rootQueueButton.DropDownOpening += new System.EventHandler(this.btn_DropDownOpening);
   this.rootQueueButton.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripSplitButton_Paint);
   // 
   // SimDropDownBar
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.AutoSize = true;
   this.Controls.Add(this.finistPanel1);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.Margin = new System.Windows.Forms.Padding(0);
   this.Name = "SimDropDownBar";
   this.Size = new System.Drawing.Size(655, 33);
   this.finistPanel1.ResumeLayout(false);
   this.finistPanel1.PerformLayout();
   this.toolStrip1.ResumeLayout(false);
   this.toolStrip1.PerformLayout();
   this.ResumeLayout(false);
   this.PerformLayout();

  }

  #endregion

  private Sim.Controls.SimToolStrip toolStrip1;
  private System.Windows.Forms.ContextMenuStrip contextMenuStripYear;
  private System.Windows.Forms.ToolStripSplitButton rootQueueButton;
  private SimPanel finistPanel1;
 }
}
