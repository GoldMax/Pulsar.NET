namespace Sim.Controls
{
 partial class SimGraphControl
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
   this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
   this.toolStripMenuItemScaleLables = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemLabAll = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemLabEverySecond = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemLabOuters = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemLabNone = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemMergeNames = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemShowGrid = new System.Windows.Forms.ToolStripMenuItem();
   this.toolStripMenuItemShowPoint = new System.Windows.Forms.ToolStripMenuItem();
   this.contextMenuStrip1.SuspendLayout();
   this.SuspendLayout();
   // 
   // contextMenuStrip1
   // 
   this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemScaleLables,
            this.toolStripMenuItemShowGrid,
            this.toolStripMenuItemShowPoint,
            this.toolStripMenuItemMergeNames});
   this.contextMenuStrip1.Name = "contextMenuStrip1";
   this.contextMenuStrip1.Size = new System.Drawing.Size(220, 114);
   this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
   // 
   // toolStripMenuItemScaleLables
   // 
   this.toolStripMenuItemScaleLables.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemLabAll,
            this.toolStripMenuItemLabEverySecond,
            this.toolStripMenuItemLabOuters,
            this.toolStripMenuItemLabNone});
   this.toolStripMenuItemScaleLables.Name = "toolStripMenuItemScaleLables";
   this.toolStripMenuItemScaleLables.Size = new System.Drawing.Size(219, 22);
   this.toolStripMenuItemScaleLables.Text = "Метки шкалы";
   // 
   // toolStripMenuItemLabAll
   // 
   this.toolStripMenuItemLabAll.Checked = true;
   this.toolStripMenuItemLabAll.CheckOnClick = true;
   this.toolStripMenuItemLabAll.CheckState = System.Windows.Forms.CheckState.Checked;
   this.toolStripMenuItemLabAll.Name = "toolStripMenuItemLabAll";
   this.toolStripMenuItemLabAll.Size = new System.Drawing.Size(166, 22);
   this.toolStripMenuItemLabAll.Text = "Все";
   this.toolStripMenuItemLabAll.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemLabEverySecond
   // 
   this.toolStripMenuItemLabEverySecond.Name = "toolStripMenuItemLabEverySecond";
   this.toolStripMenuItemLabEverySecond.Size = new System.Drawing.Size(166, 22);
   this.toolStripMenuItemLabEverySecond.Text = "Через одну";
   this.toolStripMenuItemLabEverySecond.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemLabOuters
   // 
   this.toolStripMenuItemLabOuters.Name = "toolStripMenuItemLabOuters";
   this.toolStripMenuItemLabOuters.Size = new System.Drawing.Size(166, 22);
   this.toolStripMenuItemLabOuters.Text = "Только крайние";
   this.toolStripMenuItemLabOuters.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemLabNone
   // 
   this.toolStripMenuItemLabNone.Name = "toolStripMenuItemLabNone";
   this.toolStripMenuItemLabNone.Size = new System.Drawing.Size(166, 22);
   this.toolStripMenuItemLabNone.Text = "Нет";
   this.toolStripMenuItemLabNone.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemMergeNames
   // 
   this.toolStripMenuItemMergeNames.CheckOnClick = true;
   this.toolStripMenuItemMergeNames.Name = "toolStripMenuItemMergeNames";
   this.toolStripMenuItemMergeNames.Size = new System.Drawing.Size(219, 22);
   this.toolStripMenuItemMergeNames.Text = "Объединение имен ";
   this.toolStripMenuItemMergeNames.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemShowGrid
   // 
   this.toolStripMenuItemShowGrid.Checked = true;
   this.toolStripMenuItemShowGrid.CheckOnClick = true;
   this.toolStripMenuItemShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
   this.toolStripMenuItemShowGrid.Image = global::Sim.Controls.Properties.Resources.Grid;
   this.toolStripMenuItemShowGrid.Name = "toolStripMenuItemShowGrid";
   this.toolStripMenuItemShowGrid.Size = new System.Drawing.Size(219, 22);
   this.toolStripMenuItemShowGrid.Text = "Показывать сетку";
   this.toolStripMenuItemShowGrid.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // toolStripMenuItemShowPoint
   // 
   this.toolStripMenuItemShowPoint.Checked = true;
   this.toolStripMenuItemShowPoint.CheckOnClick = true;
   this.toolStripMenuItemShowPoint.CheckState = System.Windows.Forms.CheckState.Checked;
   this.toolStripMenuItemShowPoint.Image = global::Sim.Controls.Properties.Resources.ShowPoint;
   this.toolStripMenuItemShowPoint.Name = "toolStripMenuItemShowPoint";
   this.toolStripMenuItemShowPoint.Size = new System.Drawing.Size(219, 22);
   this.toolStripMenuItemShowPoint.Text = "Выделять точки значений";
   this.toolStripMenuItemShowPoint.Click += new System.EventHandler(this.contextMenuStrip1_ItemClicked);
   // 
   // SimGraphControl
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.BackColor = System.Drawing.Color.White;
   this.ContextMenuStrip = this.contextMenuStrip1;
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.MinimumSize = new System.Drawing.Size(115, 115);
   this.Name = "SimGraphControl";
   this.Size = new System.Drawing.Size(310, 278);
   this.contextMenuStrip1.ResumeLayout(false);
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemScaleLables;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowGrid;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLabAll;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLabOuters;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLabEverySecond;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLabNone;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMergeNames;
  private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPoint;
 }
}
