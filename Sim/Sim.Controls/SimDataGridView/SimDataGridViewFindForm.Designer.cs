namespace Sim.Controls
{
 partial class SimDataGridViewFindForm
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
   this.finistPanel1 = new Sim.Controls.SimPanel();
   this.label1 = new System.Windows.Forms.Label();
   this.pictureBox1 = new System.Windows.Forms.PictureBox();
   this.textBoxText = new Sim.Controls.SimTextBox();
   this.buttonFind = new Sim.Controls.SimButton();
   this.label2 = new System.Windows.Forms.Label();
   this.comboBoxColumns = new Sim.Controls.SimComboBox();
   this.finistPanel1.SuspendLayout();
   ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
   this.SuspendLayout();
   // 
   // finistPanel1
   // 
   this.finistPanel1.BackColor = System.Drawing.Color.Transparent;
   this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.finistPanel1.Controls.Add(this.label1);
   this.finistPanel1.Controls.Add(this.pictureBox1);
   this.finistPanel1.Controls.Add(this.textBoxText);
   this.finistPanel1.Controls.Add(this.buttonFind);
   this.finistPanel1.Controls.Add(this.label2);
   this.finistPanel1.Controls.Add(this.comboBoxColumns);
   this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
   this.finistPanel1.Location = new System.Drawing.Point(0, 0);
   this.finistPanel1.Name = "finistPanel1";
   this.finistPanel1.Size = new System.Drawing.Size(212, 120);
   this.finistPanel1.TabIndex = 11;
   // 
   // label1
   // 
   this.label1.AutoSize = true;
   this.label1.Location = new System.Drawing.Point(3, 4);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(96, 13);
   this.label1.TabIndex = 5;
   this.label1.Text = "Текст для поиска";
   // 
   // pictureBox1
   // 
   this.pictureBox1.Location = new System.Drawing.Point(7, 89);
   this.pictureBox1.Name = "pictureBox1";
   this.pictureBox1.Size = new System.Drawing.Size(16, 16);
   this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
   this.pictureBox1.TabIndex = 10;
   this.pictureBox1.TabStop = false;
   this.pictureBox1.Visible = false;
   // 
   // textBoxText
   // 
   this.textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.textBoxText.Location = new System.Drawing.Point(3, 20);
   this.textBoxText.Name = "textBoxText";
   this.textBoxText.Size = new System.Drawing.Size(204, 21);
   this.textBoxText.TabIndex = 6;
   this.textBoxText.TextChanged += new System.EventHandler(this.textBoxText_TextChanged);
   this.textBoxText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxText_KeyPress);
   // 
   // buttonFind
   // 
   this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
   this.buttonFind.Enabled = false;
   this.buttonFind.Image = global::Sim.Controls.Properties.Resources.Find;
   this.buttonFind.Location = new System.Drawing.Point(113, 87);
   this.buttonFind.Name = "buttonFind";
   this.buttonFind.Size = new System.Drawing.Size(94, 25);
   this.buttonFind.TabIndex = 9;
   this.buttonFind.Text = "Найти";
   this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
   // 
   // label2
   // 
   this.label2.AutoSize = true;
   this.label2.Location = new System.Drawing.Point(3, 44);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(88, 13);
   this.label2.TabIndex = 7;
   this.label2.Text = "Столбец поиска";
   // 
   // comboBoxColumns
   // 
   this.comboBoxColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.comboBoxColumns.Location = new System.Drawing.Point(3, 60);
   this.comboBoxColumns.Name = "comboBoxColumns";
   this.comboBoxColumns.SelectedIndex = -1;
   this.comboBoxColumns.SelectedItem = null;
   this.comboBoxColumns.Size = new System.Drawing.Size(204, 21);
   this.comboBoxColumns.TabIndex = 8;
   // 
   // SimDataGridViewFindForm
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.ClientSize = new System.Drawing.Size(212, 120);
   this.Controls.Add(this.finistPanel1);
   this.DoubleBuffered = true;
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
   this.Name = "SimDataGridViewFindForm";
   this.ShowInTaskbar = false;
   this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
   this.Text = "Поиск по таблице";
   this.Deactivate += new System.EventHandler(this.SimDataGridViewFindForm_Deactivate);
   this.Shown += new System.EventHandler(this.SimDataGridViewFindForm_Shown);
   this.finistPanel1.ResumeLayout(false);
   this.finistPanel1.PerformLayout();
   ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
   this.ResumeLayout(false);

  }

  #endregion

  private Sim.Controls.SimButton buttonFind;
  private Sim.Controls.SimComboBox comboBoxColumns;
  private System.Windows.Forms.Label label2;
  private Sim.Controls.SimTextBox textBoxText;
  private System.Windows.Forms.Label label1;
  private System.Windows.Forms.PictureBox pictureBox1;
  private SimPanel finistPanel1;

 }
}