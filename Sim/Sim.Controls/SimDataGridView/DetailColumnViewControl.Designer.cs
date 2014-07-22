namespace Sim.Controls
{
	partial class DetailColumnViewControl
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
			this.finistPanel2 = new Sim.Controls.SimPanel();
			this.finistLabelValue = new Sim.Controls.SimLabel();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.finistPopupButton1 = new Sim.Controls.SimPopupButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.finistPanel2.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// finistPanel2
			// 
			this.finistPanel2.AutoSize = true;
			this.finistPanel2.BackColor = System.Drawing.Color.Transparent;
			this.finistPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel2.BorderWidth = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.finistPanel2.Controls.Add(this.finistLabelValue);
			this.finistPanel2.Controls.Add(this.finistPanel1);
			this.finistPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistPanel2.Location = new System.Drawing.Point(1, 1);
			this.finistPanel2.Name = "finistPanel2";
			this.finistPanel2.Size = new System.Drawing.Size(38, 37);
			this.finistPanel2.TabIndex = 2;
			// 
			// finistLabelValue
			// 
			this.finistLabelValue.AutoEllipsis = true;
			this.finistLabelValue.BackColor = System.Drawing.Color.Transparent;
			this.finistLabelValue.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistLabelValue.Location = new System.Drawing.Point(0, 18);
			this.finistLabelValue.Name = "finistLabelValue";
			this.finistLabelValue.Size = new System.Drawing.Size(38, 16);
			this.finistLabelValue.TabIndex = 3;
			this.finistLabelValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// finistPanel1
			// 
			this.finistPanel1.BackColor = System.Drawing.Color.Transparent;
			this.finistPanel1.BorderWidth = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this.finistPanel1.Controls.Add(this.finistLabelCaption);
			this.finistPanel1.Controls.Add(this.finistPopupButton1);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanel1.Location = new System.Drawing.Point(0, 0);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(38, 18);
			this.finistPanel1.TabIndex = 2;
			// 
			// finistLabelCaption
			// 
			this.finistLabelCaption.AutoEllipsis = true;
			this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabelCaption.Location = new System.Drawing.Point(18, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Size = new System.Drawing.Size(20, 18);
			this.finistLabelCaption.TabIndex = 1;
			// 
			// finistPopupButton1
			// 
			this.finistPopupButton1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.finistPopupButton1.Dock = System.Windows.Forms.DockStyle.Left;
			this.finistPopupButton1.Image = global::Sim.Controls.Properties.Resources.ColumnFromDetail;
			this.finistPopupButton1.ImagePushed = global::Sim.Controls.Properties.Resources.ColumnFromDetail;
			this.finistPopupButton1.ImageRaised = global::Sim.Controls.Properties.Resources.ColumnFromDetail;
			this.finistPopupButton1.Location = new System.Drawing.Point(0, 0);
			this.finistPopupButton1.Name = "finistPopupButton1";
			this.finistPopupButton1.PushedBackColorStart = System.Drawing.SystemColors.Control;
			this.finistPopupButton1.ShowBorder = false;
			this.finistPopupButton1.Size = new System.Drawing.Size(18, 18);
			this.finistPopupButton1.TabIndex = 0;
			this.finistPopupButton1.TabStop = false;
			this.toolTip1.SetToolTip(this.finistPopupButton1, "Отображает столбец в таблице.");
			this.finistPopupButton1.Click += new System.EventHandler(this.finistPopupButton1_Click);
			// 
			// DetailColumnViewControl
			// 
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.finistPanel2);
			this.MaximumSize = new System.Drawing.Size(0, 38);
			this.MinimumSize = new System.Drawing.Size(40, 38);
			this.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.Size = new System.Drawing.Size(40, 38);
			this.finistPanel2.ResumeLayout(false);
			this.finistPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SimPanel finistPanel2;
		private SimLabel finistLabelValue;
		private SimPanel finistPanel1;
		private SimLabel finistLabelCaption;
		private SimPopupButton finistPopupButton1;
		private System.Windows.Forms.ToolTip toolTip1;

	}
}
