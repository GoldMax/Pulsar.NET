namespace Sim.Controls
{
	partial class SimModalDialogBase
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimModalDialogBase));
			this.finistPanelHeader = new Sim.Controls.SimPanel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.btnCancel = new Sim.Controls.SimPopupButton();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.buttonOK = new Sim.Controls.SimButton();
			this.buttonCancel = new Sim.Controls.SimButton();
			this.panelBody = new Sim.Controls.SimPanel();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panelBody);
			this.panel1.Controls.Add(this.finistPanel1);
			this.panel1.Controls.Add(this.finistPanelHeader);
			// 
			// finistPanelHeader
			// 
			this.finistPanelHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.finistPanelHeader.BackColor2 = System.Drawing.SystemColors.ActiveCaption;
			this.finistPanelHeader.Controls.Add(this.finistLabelCaption);
			this.finistPanelHeader.Controls.Add(this.btnCancel);
			this.finistPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanelHeader.GradientMode = Sim.Controls.GradientMode.Vertical;
			this.finistPanelHeader.Location = new System.Drawing.Point(0, 0);
			this.finistPanelHeader.MinimumSize = new System.Drawing.Size(0, 21);
			this.finistPanelHeader.Name = "finistPanelHeader";
			this.finistPanelHeader.Size = new System.Drawing.Size(388, 21);
			this.finistPanelHeader.TabIndex = 10;
			// 
			// finistLabelCaption
			// 
			this.finistLabelCaption.AutoSize = false;
			this.finistLabelCaption.BackColor = System.Drawing.Color.Transparent;
			this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelCaption.EventsTransparent = true;
			this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.finistLabelCaption.Image = global::Sim.Controls.Properties.Resources.Rename;
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(367, 21);
			this.finistLabelCaption.TabIndex = 1;
			this.finistLabelCaption.Text = "Заголовок";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColorEnd = System.Drawing.Color.Transparent;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Image = global::Sim.Controls.Properties.Resources.NormalXbtn2_c;
			this.btnCancel.ImagePushed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePushed")));
			this.btnCancel.ImageRaised = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageRaised")));
			this.btnCancel.Location = new System.Drawing.Point(367, 0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.PushedBackColorStart = System.Drawing.Color.Transparent;
			this.btnCancel.ShowBorder = false;
			this.btnCancel.Size = new System.Drawing.Size(21, 21);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.TabStop = false;
			this.btnCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// finistPanel1
			// 
			this.finistPanel1.BackColor = System.Drawing.SystemColors.Control;
			this.finistPanel1.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanel1.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.finistPanel1.Controls.Add(this.buttonOK);
			this.finistPanel1.Controls.Add(this.buttonCancel);
			this.finistPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanel1.GradientMode = Sim.Controls.GradientMode.BackwardDiagonal;
			this.finistPanel1.Location = new System.Drawing.Point(0, 341);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(388, 35);
			this.finistPanel1.TabIndex = 11;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonOK.Enabled = false;
			this.buttonOK.Image = global::Sim.Controls.Properties.Resources.OK;
			this.buttonOK.Location = new System.Drawing.Point(227, 4);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 26);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "ОК";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonCancel.Image = global::Sim.Controls.Properties.Resources.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(308, 4);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 26);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// panelBody
			// 
			this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBody.Location = new System.Drawing.Point(0, 21);
			this.panelBody.Name = "panelBody";
			this.panelBody.Padding = new System.Windows.Forms.Padding(3);
			this.panelBody.Size = new System.Drawing.Size(388, 320);
			this.panelBody.TabIndex = 12;
			// 
			// SimModalDialogBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MoveControl = this.finistPanelHeader;
			this.Name = "SimModalDialogBase";
			this.panel1.ResumeLayout(false);
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SimPanel finistPanelHeader;
		private SimLabel finistLabelCaption;
		private SimPopupButton btnCancel;
		/// <summary>
		/// Панель тела диалога.
		/// </summary>
		protected SimPanel panelBody;
		protected SimButton buttonOK;
		protected SimButton buttonCancel;
		protected SimPanel finistPanel1;

	}
}
