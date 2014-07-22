namespace Sim.AdminForms
{
 partial class CtrlSelectTypes
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
   this.groupBox1 = new System.Windows.Forms.GroupBox();
   this.tbGeneric = new Sim.Controls.SimTextBox();
   this.label5 = new System.Windows.Forms.Label();
   this.cbType = new Sim.Controls.SimComboBox();
   this.label3 = new System.Windows.Forms.Label();
   this.cbAssembly = new Sim.Controls.SimComboBox();
   this.label2 = new System.Windows.Forms.Label();
   this.fdgvList = new Sim.Controls.SimDataGridView();
   this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
   this.finistPanel1 = new Sim.Controls.SimPanel();
   this.btnDel = new Sim.Controls.SimPopupButton();
   this.btnClear = new Sim.Controls.SimPopupButton();
   this.btnAddType = new Sim.Controls.SimButton();
   this.groupBox1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvList)).BeginInit();
   this.finistPanel1.SuspendLayout();
   this.SuspendLayout();
   // 
   // groupBox1
   // 
   this.groupBox1.Controls.Add(this.tbGeneric);
   this.groupBox1.Controls.Add(this.label5);
   this.groupBox1.Controls.Add(this.cbType);
   this.groupBox1.Controls.Add(this.label3);
   this.groupBox1.Controls.Add(this.cbAssembly);
   this.groupBox1.Controls.Add(this.label2);
   this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
   this.groupBox1.Location = new System.Drawing.Point(0, 0);
   this.groupBox1.Name = "groupBox1";
   this.groupBox1.Size = new System.Drawing.Size(320, 128);
   this.groupBox1.TabIndex = 0;
   this.groupBox1.TabStop = false;
   this.groupBox1.Text = " Добавляемый тип ";
   // 
   // tbGeneric
   // 
   this.tbGeneric.Dock = System.Windows.Forms.DockStyle.Top;
   this.tbGeneric.Location = new System.Drawing.Point(3, 101);
   this.tbGeneric.Name = "tbGeneric";
   this.tbGeneric.Size = new System.Drawing.Size(314, 21);
   this.tbGeneric.TabIndex = 22;
   // 
   // label5
   // 
   this.label5.AutoSize = true;
   this.label5.Dock = System.Windows.Forms.DockStyle.Top;
   this.label5.Location = new System.Drawing.Point(3, 86);
   this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label5.Name = "label5";
   this.label5.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
   this.label5.Size = new System.Drawing.Size(102, 15);
   this.label5.TabIndex = 23;
   this.label5.Text = "Generic параметры";
   // 
   // cbType
   // 
   this.cbType.Dock = System.Windows.Forms.DockStyle.Top;
   this.cbType.Location = new System.Drawing.Point(3, 65);
   this.cbType.Name = "cbType";
   this.cbType.Size = new System.Drawing.Size(314, 21);
   this.cbType.Sorted = true;
   this.cbType.TabIndex = 21;
   this.cbType.DropDownOpening += new System.ComponentModel.CancelEventHandler(this.cbType_DropDown);
   // 
   // label3
   // 
   this.label3.AutoSize = true;
   this.label3.Dock = System.Windows.Forms.DockStyle.Top;
   this.label3.Location = new System.Drawing.Point(3, 50);
   this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label3.Name = "label3";
   this.label3.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
   this.label3.Size = new System.Drawing.Size(25, 15);
   this.label3.TabIndex = 18;
   this.label3.Text = "Тип";
   // 
   // cbAssembly
   // 
   this.cbAssembly.Dock = System.Windows.Forms.DockStyle.Top;
   this.cbAssembly.Location = new System.Drawing.Point(3, 29);
   this.cbAssembly.Name = "cbAssembly";
   this.cbAssembly.Size = new System.Drawing.Size(314, 21);
   this.cbAssembly.Sorted = true;
   this.cbAssembly.TabIndex = 20;
   this.cbAssembly.DropDownOpening += new System.ComponentModel.CancelEventHandler(this.cbAssembly_DropDown);
   this.cbAssembly.UISelectedItemChanged += new System.EventHandler(this.cbAssembly_SelectionChangeCommitted);
   // 
   // label2
   // 
   this.label2.AutoSize = true;
   this.label2.Dock = System.Windows.Forms.DockStyle.Top;
   this.label2.Location = new System.Drawing.Point(3, 16);
   this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(44, 13);
   this.label2.TabIndex = 19;
   this.label2.Text = "Сборка";
   // 
   // fdgvList
   // 
   this.fdgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
   this.fdgvList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
   this.fdgvList.ColumnHeadersVisible = false;
   this.fdgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
   this.fdgvList.Dock = System.Windows.Forms.DockStyle.Fill;
   this.fdgvList.Location = new System.Drawing.Point(0, 156);
   this.fdgvList.Name = "fdgvList";
   this.fdgvList.RowHeadersVisible = false;
   this.fdgvList.RowTemplate.Height = 16;
   this.fdgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
   this.fdgvList.Size = new System.Drawing.Size(320, 164);
   this.fdgvList.TabIndex = 2;
   this.fdgvList.SelectionChanged += new System.EventHandler(this.fdgvList_SelectionChanged);
   // 
   // Column1
   // 
   this.Column1.DataPropertyName = "FullName";
   this.Column1.HeaderText = "Column1";
   this.Column1.Name = "Column1";
   this.Column1.ReadOnly = true;
   // 
   // finistPanel1
   // 
   this.finistPanel1.Controls.Add(this.btnDel);
   this.finistPanel1.Controls.Add(this.btnClear);
   this.finistPanel1.Controls.Add(this.btnAddType);
   this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Top;
   this.finistPanel1.Location = new System.Drawing.Point(0, 128);
   this.finistPanel1.Name = "finistPanel1";
   this.finistPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
   this.finistPanel1.Size = new System.Drawing.Size(320, 28);
   this.finistPanel1.TabIndex = 3;
   // 
   // btnDel
   // 
   this.btnDel.Dock = System.Windows.Forms.DockStyle.Right;
   this.btnDel.Enabled = false;
   this.btnDel.Image = global::Sim.AdminForms.Properties.Resources.Delete_big;
   this.btnDel.Location = new System.Drawing.Point(272, 2);
   this.btnDel.Name = "btnDel";
   this.btnDel.Size = new System.Drawing.Size(24, 24);
   this.btnDel.TabIndex = 3;
   this.btnDel.Text = "finistPopupButton2";
   this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
   // 
   // btnClear
   // 
   this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
   this.btnClear.Image = global::Sim.AdminForms.Properties.Resources.Clear;
   this.btnClear.Location = new System.Drawing.Point(296, 2);
   this.btnClear.Name = "btnClear";
   this.btnClear.Size = new System.Drawing.Size(24, 24);
   this.btnClear.TabIndex = 2;
   this.btnClear.Text = "finistPopupButton1";
   this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
   // 
   // btnAddType
   // 
   this.btnAddType.Dock = System.Windows.Forms.DockStyle.Left;
   this.btnAddType.Enabled = false;
   this.btnAddType.Image = global::Sim.AdminForms.Properties.Resources.Down;
   this.btnAddType.Location = new System.Drawing.Point(0, 2);
   this.btnAddType.Name = "btnAddType";
   this.btnAddType.Size = new System.Drawing.Size(86, 24);
   this.btnAddType.TabIndex = 1;
   this.btnAddType.Text = "Добавить";
   this.btnAddType.Click += new System.EventHandler(this.btnAddType_Click);
   // 
   // CtrlSelectTypes
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.Controls.Add(this.fdgvList);
   this.Controls.Add(this.finistPanel1);
   this.Controls.Add(this.groupBox1);
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.Name = "CtrlSelectTypes";
   this.Size = new System.Drawing.Size(320, 320);
   this.groupBox1.ResumeLayout(false);
   this.groupBox1.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.fdgvList)).EndInit();
   this.finistPanel1.ResumeLayout(false);
   this.ResumeLayout(false);

  }

  #endregion

  private System.Windows.Forms.GroupBox groupBox1;
  private Controls.SimTextBox tbGeneric;
  private System.Windows.Forms.Label label5;
  private Controls.SimComboBox cbType;
  private System.Windows.Forms.Label label3;
  private Controls.SimComboBox cbAssembly;
  private System.Windows.Forms.Label label2;
  private Controls.SimDataGridView fdgvList;
  private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
  private Controls.SimPanel finistPanel1;
  private Controls.SimButton btnAddType;
  private Controls.SimPopupButton btnDel;
  private Controls.SimPopupButton btnClear;
 }
}
