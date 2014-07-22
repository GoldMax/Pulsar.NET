namespace Sim.Controls
{
 partial class SimModalTreeItemChoiceBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimModalTreeItemChoiceBox));
			Sim.Controls.SimToolStripColorTable simToolStripColorTable1 = new Sim.Controls.SimToolStripColorTable();
			this.treeView = new Sim.Controls.SimTreeView();
			this.panelBody.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.treeView.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelBody
			// 
			this.panelBody.Controls.Add(this.treeView);
			this.panelBody.Size = new System.Drawing.Size(360, 347);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(199, 4);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(280, 4);
			// 
			// finistPanel1
			// 
			this.finistPanel1.Location = new System.Drawing.Point(0, 368);
			this.finistPanel1.Size = new System.Drawing.Size(360, 35);
			// 
			// panel1
			// 
			this.panel1.Size = new System.Drawing.Size(360, 403);
			// 
			// treeView
			// 
			this.treeView.BackColor = System.Drawing.SystemColors.Control;
			this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			// 
			// 
			// 
			this.treeView.ContextMenuStrip.Name = "contextMenuStrip1";
			this.treeView.ContextMenuStrip.Size = new System.Drawing.Size(255, 98);
			this.treeView.CurrentView = Sim.Controls.TreeViewKind.FirstView;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.FirstViewButtonImage = ((System.Drawing.Image)(resources.GetObject("treeView.FirstViewButtonImage")));
			this.treeView.FirstViewButtonToolTipText = "Основной вид дерева";
			this.treeView.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.treeView.HideSelection = false;
			this.treeView.Location = new System.Drawing.Point(3, 3);
			// 
			// treeView.MainToolStrip
			// 
			simToolStripColorTable1.UseSystemColors = false;
			this.treeView.MainToolStrip.ColorTable = simToolStripColorTable1;
			this.treeView.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.treeView.MainToolStrip.Location = new System.Drawing.Point(0, 0);
			this.treeView.MainToolStrip.Name = "MainToolStrip";
			this.treeView.MainToolStrip.Size = new System.Drawing.Size(415, 25);
			this.treeView.MainToolStrip.TabIndex = 0;
			this.treeView.MainToolStrip.Text = "toolStrip1";
			this.treeView.MainToolStrip.Visible = false;
			this.treeView.Name = "treeView";
			this.treeView.NodeItemsImageIndex = 2;
			this.treeView.OpenedNodeImageIndex = 1;
			this.treeView.SecondViewButtonImage = ((System.Drawing.Image)(resources.GetObject("treeView.SecondViewButtonImage")));
			this.treeView.SecondViewButtonToolTipText = "Дополнительный вид дерева";
			this.treeView.Size = new System.Drawing.Size(354, 341);
			this.treeView.Sorted = true;
			this.treeView.TabIndex = 0;
			// 
			// SimModalTreeItemChoiceBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ButtonOkEnabled = true;
			this.Caption = "Выбор элемента дерева";
			this.CaptionImage = global::Sim.Controls.Properties.Resources.TreeView;
			this.Name = "SimModalTreeItemChoiceBox";
			this.Size = new System.Drawing.Size(364, 407);
			this.panelBody.ResumeLayout(false);
			this.finistPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.treeView.ResumeLayout(false);
			this.treeView.PerformLayout();
			this.ResumeLayout(false);

  }

  #endregion

  private SimTreeView treeView;

 }
}
