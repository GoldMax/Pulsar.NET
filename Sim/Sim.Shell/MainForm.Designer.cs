namespace Sim
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelText = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelServerName = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.tabPage8 = new System.Windows.Forms.TabPage();
			this.tabPage9 = new System.Windows.Forms.TabPage();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.tabViewer1 = new Sim.TabViewer();
			this.navigator1 = new Sim.Shell.Navigator();
			this.statusStrip1.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.AutoSize = false;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelText,
            this.toolStripStatusLabelServerName,
            this.toolStripStatusLabelProgress});
			this.statusStrip1.Location = new System.Drawing.Point(0, 625);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.statusStrip1.ShowItemToolTips = true;
			this.statusStrip1.Size = new System.Drawing.Size(843, 23);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabelText
			// 
			this.toolStripStatusLabelText.ActiveLinkColor = System.Drawing.SystemColors.ControlLight;
			this.toolStripStatusLabelText.Name = "toolStripStatusLabelText";
			this.toolStripStatusLabelText.Size = new System.Drawing.Size(755, 18);
			this.toolStripStatusLabelText.Spring = true;
			this.toolStripStatusLabelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabelServerName
			// 
			this.toolStripStatusLabelServerName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLabelServerName.DoubleClickEnabled = true;
			this.toolStripStatusLabelServerName.Name = "toolStripStatusLabelServerName";
			this.toolStripStatusLabelServerName.Size = new System.Drawing.Size(4, 18);
			this.toolStripStatusLabelServerName.DoubleClick += new System.EventHandler(this.toolStripStatusLabelServerName_DoubleClick);
			// 
			// toolStripStatusLabelProgress
			// 
			this.toolStripStatusLabelProgress.AutoSize = false;
			this.toolStripStatusLabelProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripStatusLabelProgress.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripStatusLabelProgress.Name = "toolStripStatusLabelProgress";
			this.toolStripStatusLabelProgress.Size = new System.Drawing.Size(69, 18);
			this.toolStripStatusLabelProgress.Text = "toolStripStatusLabel1";
			this.toolStripStatusLabelProgress.ToolTipText = "Идет обмен данными с SQL сервером ...";
			// 
			// tabPage7
			// 
			this.tabPage7.Location = new System.Drawing.Point(4, 25);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage7.Size = new System.Drawing.Size(840, 547);
			this.tabPage7.TabIndex = 6;
			this.tabPage7.Text = "tabPage7";
			this.tabPage7.UseVisualStyleBackColor = true;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Transparent;
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(840, 547);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.Transparent;
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(840, 547);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.Color.Transparent;
			this.tabPage3.Location = new System.Drawing.Point(4, 25);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(840, 547);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "tabPage3";
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(4, 25);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(840, 547);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tabPage5
			// 
			this.tabPage5.Location = new System.Drawing.Point(4, 25);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(840, 547);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "tabPage5";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// tabPage6
			// 
			this.tabPage6.Location = new System.Drawing.Point(4, 25);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage6.Size = new System.Drawing.Size(840, 547);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "tabPage6";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// tabPage8
			// 
			this.tabPage8.Location = new System.Drawing.Point(4, 25);
			this.tabPage8.Name = "tabPage8";
			this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage8.Size = new System.Drawing.Size(840, 547);
			this.tabPage8.TabIndex = 7;
			this.tabPage8.Text = "tabPage8";
			this.tabPage8.UseVisualStyleBackColor = true;
			// 
			// tabPage9
			// 
			this.tabPage9.Location = new System.Drawing.Point(4, 25);
			this.tabPage9.Name = "tabPage9";
			this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage9.Size = new System.Drawing.Size(840, 547);
			this.tabPage9.TabIndex = 8;
			this.tabPage9.Text = "tabPage9";
			this.tabPage9.UseVisualStyleBackColor = true;
			// 
			// finistPanel1
			// 
			this.finistPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.finistPanel1.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanel1.Controls.Add(this.tabViewer1);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistPanel1.GradientMode = Sim.Controls.GradientMode.Horizontal;
			this.finistPanel1.Location = new System.Drawing.Point(0, 57);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Padding = new System.Windows.Forms.Padding(2, 0, 1, 1);
			this.finistPanel1.Size = new System.Drawing.Size(843, 568);
			this.finistPanel1.TabIndex = 13;
			// 
			// tabViewer1
			// 
			this.tabViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabViewer1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tabViewer1.ItemSize = new System.Drawing.Size(42, 21);
			this.tabViewer1.Location = new System.Drawing.Point(2, 0);
			this.tabViewer1.Name = "tabViewer1";
			this.tabViewer1.Padding = new System.Drawing.Point(9, 3);
			this.tabViewer1.SelectedIndex = 0;
			this.tabViewer1.Size = new System.Drawing.Size(840, 567);
			this.tabViewer1.TabIndex = 12;
			this.tabViewer1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabViewer1_Selected);
			// 
			// navigator1
			// 
			this.navigator1.AssistSex = ((Pulsar.Sex)(Pulsar.Sex.Female));
			this.navigator1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.navigator1.Dock = System.Windows.Forms.DockStyle.Top;
			this.navigator1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.navigator1.Location = new System.Drawing.Point(0, 0);
			this.navigator1.MaximumSize = new System.Drawing.Size(0, 57);
			this.navigator1.MinimumSize = new System.Drawing.Size(190, 57);
			this.navigator1.Name = "navigator1";
			this.navigator1.Size = new System.Drawing.Size(843, 57);
			this.navigator1.TabIndex = 10;
			this.navigator1.FormSelected += new Sim.Shell.Navigator.FormSelectedEventHandler(this.navigator1_FormSelected);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(843, 648);
			this.Controls.Add(this.finistPanel1);
			this.Controls.Add(this.navigator1);
			this.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(400, 200);
			this.Name = "MainForm";
			this.Text = " SIM";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.finistPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelText;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelServerName;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProgress;
		private Sim.Shell.Navigator navigator1;
		private TabViewer tabViewer1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.TabPage tabPage8;
		private System.Windows.Forms.TabPage tabPage9;
		private Controls.SimPanel finistPanel1;
	}
}

