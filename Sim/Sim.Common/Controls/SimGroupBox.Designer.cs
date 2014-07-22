namespace Sim.Controls
{
	partial class SimGroupBox
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
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.fpBody = new Sim.Controls.SimPanel();
			this.fpHeader = new Sim.Controls.SimPanel();
			this.finistLabel1 = new Sim.Controls.SimLabel();
			this.btnCollapse = new Sim.Controls.SimPopupButton();
			this.finistPanel1.SuspendLayout();
			this.fpHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// finistPanel1
			// 
			this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel1.Controls.Add(this.fpBody);
			this.finistPanel1.Controls.Add(this.fpHeader);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistPanel1.Location = new System.Drawing.Point(0, 0);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(237, 232);
			this.finistPanel1.TabIndex = 0;
			// 
			// fpBody
			// 
			this.fpBody.AutoSize = true;
			this.fpBody.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.fpBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpBody.Location = new System.Drawing.Point(0, 22);
			this.fpBody.Name = "fpBody";
			this.fpBody.Padding = new System.Windows.Forms.Padding(3);
			this.fpBody.Size = new System.Drawing.Size(235, 208);
			this.fpBody.TabIndex = 1;
			// 
			// fpHeader
			// 
			this.fpHeader.BackColor = System.Drawing.SystemColors.ControlLight;
			this.fpHeader.BackColor2 = System.Drawing.SystemColors.Control;
			this.fpHeader.BackColorMiddle = System.Drawing.SystemColors.ControlLightLight;
			this.fpHeader.Controls.Add(this.finistLabel1);
			this.fpHeader.Controls.Add(this.btnCollapse);
			this.fpHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.fpHeader.GradientMode = Sim.Controls.GradientMode.TrioVertical;
			this.fpHeader.Location = new System.Drawing.Point(0, 0);
			this.fpHeader.Name = "fpHeader";
			this.fpHeader.Size = new System.Drawing.Size(235, 22);
			this.fpHeader.TabIndex = 2;
			// 
			// finistLabel1
			// 
			this.finistLabel1.AutoSize = false;
			this.finistLabel1.BackColor = System.Drawing.Color.Transparent;
			this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabel1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.finistLabel1.Location = new System.Drawing.Point(0, 0);
			this.finistLabel1.Name = "finistLabel1";
			this.finistLabel1.Size = new System.Drawing.Size(215, 22);
			this.finistLabel1.TabIndex = 1;
			this.finistLabel1.Text = "Заголовок";
			// 
			// btnCollapse
			// 
			this.btnCollapse.BackColor = System.Drawing.Color.Transparent;
			this.btnCollapse.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCollapse.Image = global::Sim.Common.Properties.Resources.Collapce_Normal;
			this.btnCollapse.ImagePushed = global::Sim.Common.Properties.Resources.Collapce_Pushed;
			this.btnCollapse.ImageRaised = global::Sim.Common.Properties.Resources.Collapce_Raised;
			this.btnCollapse.Location = new System.Drawing.Point(215, 0);
			this.btnCollapse.MinimumSize = new System.Drawing.Size(0, 20);
			this.btnCollapse.Name = "btnCollapse";
			this.btnCollapse.PushedBackColorStart = System.Drawing.Color.Transparent;
			this.btnCollapse.ShowBorder = false;
			this.btnCollapse.Size = new System.Drawing.Size(20, 22);
			this.btnCollapse.TabIndex = 0;
			this.btnCollapse.TabStop = false;
			this.btnCollapse.Visible = false;
			this.btnCollapse.Click += new System.EventHandler(this.btnCollapse_Click);
			// 
			// SimGroupBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.finistPanel1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "SimGroupBox";
			this.Size = new System.Drawing.Size(237, 232);
			this.finistPanel1.ResumeLayout(false);
			this.finistPanel1.PerformLayout();
			this.fpHeader.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SimPanel finistPanel1;
		/// <summary>
		/// 
		/// </summary>
		public SimPanel fpBody;
		private SimPopupButton btnCollapse;
		private SimPanel fpHeader;
		private SimLabel finistLabel1;

	}
}
