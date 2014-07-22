namespace Sim.Shell
{
	partial class Navigator
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new Sim.Shell.NavigatorTabControl();
			this.navigatorPanel1 = new Sim.Shell.NavigatorPanel();
			this.finistLabelUser = new Sim.Controls.SimLabel();
			this.finistPanel1.SuspendLayout();
			this.navigatorPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// finistPanel1
			// 
			this.finistPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.finistPanel1.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanel1.Controls.Add(this.tabControl1);
			this.finistPanel1.Controls.Add(this.panel1);
			this.finistPanel1.Controls.Add(this.navigatorPanel1);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistPanel1.GradientMode = Sim.Controls.GradientMode.Horizontal;
			this.finistPanel1.Location = new System.Drawing.Point(0, 0);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Padding = new System.Windows.Forms.Padding(2);
			this.finistPanel1.Size = new System.Drawing.Size(190, 58);
			this.finistPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(75, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(3, 54);
			this.panel1.TabIndex = 6;
			// 
			// tabControl1
			// 
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tabControl1.Location = new System.Drawing.Point(78, 2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(110, 54);
			this.tabControl1.TabIndex = 5;
			// 
			// navigatorPanel1
			// 
			this.navigatorPanel1.BackColor = System.Drawing.Color.Transparent;
			this.navigatorPanel1.Controls.Add(this.finistLabelUser);
			this.navigatorPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.navigatorPanel1.Location = new System.Drawing.Point(2, 2);
			this.navigatorPanel1.Name = "navigatorPanel1";
			this.navigatorPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this.navigatorPanel1.Size = new System.Drawing.Size(73, 54);
			this.navigatorPanel1.TabIndex = 0;
			// 
			// finistLabelUser
			// 
			this.finistLabelUser.AutoEllipsis = true;
			this.finistLabelUser.BackColor = System.Drawing.Color.Transparent;
			this.finistLabelUser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelUser.ForceDockSize = true;
			this.finistLabelUser.Image = global::Sim.Shell.Properties.Resources.Man48;
			this.finistLabelUser.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.finistLabelUser.Location = new System.Drawing.Point(0, 0);
			this.finistLabelUser.Name = "finistLabelUser";
			this.finistLabelUser.Size = new System.Drawing.Size(73, 51);
			this.finistLabelUser.TabIndex = 0;
			this.finistLabelUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.finistLabelUser.Click += new System.EventHandler(this.finistLabelUser_Click);
			this.finistLabelUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.finistLabelDiv_MouseDown);
			this.finistLabelUser.MouseEnter += new System.EventHandler(this.finistLabelDiv_MouseEnter);
			this.finistLabelUser.MouseLeave += new System.EventHandler(this.finistLabelUser_MouseLeave);
			this.finistLabelUser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.finistLabelDiv_MouseUp);
			// 
			// Navigator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.Controls.Add(this.finistPanel1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MaximumSize = new System.Drawing.Size(0, 56);
			this.MinimumSize = new System.Drawing.Size(190, 58);
			this.Name = "Navigator";
			this.Size = new System.Drawing.Size(190, 58);
			this.finistPanel1.ResumeLayout(false);
			this.navigatorPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.SimPanel finistPanel1;
		private NavigatorTabControl tabControl1;
		private Sim.Shell.NavigatorPanel navigatorPanel1;
		private Controls.SimLabel finistLabelUser;
		private System.Windows.Forms.Panel panel1;
	}
}
