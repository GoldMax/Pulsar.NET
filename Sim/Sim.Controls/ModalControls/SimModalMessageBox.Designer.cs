namespace Sim.Controls
{
	partial class SimModalMessageBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimModalMessageBox));
			this.finistPanelHeader = new Sim.Controls.SimPanel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.finistPanel2 = new Sim.Controls.SimPanel();
			this.btnCancel = new Sim.Controls.SimPopupButton();
			this.finistPanelButtons = new Sim.Controls.SimPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonNo = new Sim.Controls.SimButton();
			this.buttonYes = new Sim.Controls.SimButton();
			this.buttonOK = new Sim.Controls.SimButton();
			this.buttonCancel = new Sim.Controls.SimButton();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.finistLabelText = new Sim.Controls.SimLabel();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanel2.SuspendLayout();
			this.finistPanelButtons.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.finistLabelText);
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.finistPanelButtons);
			this.panel1.Controls.Add(this.finistPanelHeader);
			this.panel1.Size = new System.Drawing.Size(366, 111);
			// 
			// finistPanelHeader
			// 
			this.finistPanelHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.finistPanelHeader.BackColor2 = System.Drawing.SystemColors.ActiveCaption;
			this.finistPanelHeader.Controls.Add(this.finistLabelCaption);
			this.finistPanelHeader.Controls.Add(this.finistPanel2);
			this.finistPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistPanelHeader.GradientMode = Sim.Controls.GradientMode.Vertical;
			this.finistPanelHeader.Location = new System.Drawing.Point(0, 0);
			this.finistPanelHeader.MinimumSize = new System.Drawing.Size(0, 21);
			this.finistPanelHeader.Name = "finistPanelHeader";
			this.finistPanelHeader.Size = new System.Drawing.Size(366, 21);
			this.finistPanelHeader.TabIndex = 9;
			// 
			// finistLabelCaption
			// 
			this.finistLabelCaption.AutoSize = false;
			this.finistLabelCaption.BackColor = System.Drawing.Color.Transparent;
			this.finistLabelCaption.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelCaption.EventsTransparent = true;
			this.finistLabelCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabelCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(342, 21);
			this.finistLabelCaption.TabIndex = 1;
			this.finistLabelCaption.Text = "  Сообщение";
			// 
			// finistPanel2
			// 
			this.finistPanel2.BackColor = System.Drawing.Color.Transparent;
			this.finistPanel2.Controls.Add(this.btnCancel);
			this.finistPanel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.finistPanel2.Location = new System.Drawing.Point(342, 0);
			this.finistPanel2.Name = "finistPanel2";
			this.finistPanel2.Size = new System.Drawing.Size(24, 21);
			this.finistPanel2.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Image = global::Sim.Controls.Properties.Resources.NormalXbtn2_c;
			this.btnCancel.ImagePushed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePushed")));
			this.btnCancel.ImageRaised = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageRaised")));
			this.btnCancel.Location = new System.Drawing.Point(4, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.PushedBackColorStart = System.Drawing.Color.Transparent;
			this.btnCancel.ShowBorder = false;
			this.btnCancel.Size = new System.Drawing.Size(16, 16);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.TabStop = false;
			this.btnCancel.Click += new System.EventHandler(this.Buttons_Click);
			// 
			// finistPanelButtons
			// 
			this.finistPanelButtons.BackColor = System.Drawing.SystemColors.Control;
			this.finistPanelButtons.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
			this.finistPanelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.finistPanelButtons.BorderWidth = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.finistPanelButtons.Controls.Add(this.flowLayoutPanel1);
			this.finistPanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanelButtons.GradientMode = Sim.Controls.GradientMode.BackwardDiagonal;
			this.finistPanelButtons.Location = new System.Drawing.Point(0, 76);
			this.finistPanelButtons.MinimumSize = new System.Drawing.Size(368, 35);
			this.finistPanelButtons.Name = "finistPanelButtons";
			this.finistPanelButtons.Size = new System.Drawing.Size(368, 35);
			this.finistPanelButtons.TabIndex = 10;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.flowLayoutPanel1.Controls.Add(this.buttonNo);
			this.flowLayoutPanel1.Controls.Add(this.buttonYes);
			this.flowLayoutPanel1.Controls.Add(this.buttonOK);
			this.flowLayoutPanel1.Controls.Add(this.buttonCancel);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(23, 4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(326, 26);
			this.flowLayoutPanel1.TabIndex = 0;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// buttonNo
			// 
			this.buttonNo.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonNo.Image = global::Sim.Controls.Properties.Resources.Stop;
			this.buttonNo.Location = new System.Drawing.Point(5, 0);
			this.buttonNo.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.Size = new System.Drawing.Size(75, 26);
			this.buttonNo.TabIndex = 3;
			this.buttonNo.Text = "Нет";
			this.buttonNo.Click += new System.EventHandler(this.Buttons_Click);
			// 
			// buttonYes
			// 
			this.buttonYes.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonYes.Image = global::Sim.Controls.Properties.Resources.OK;
			this.buttonYes.Location = new System.Drawing.Point(85, 0);
			this.buttonYes.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.Size = new System.Drawing.Size(75, 26);
			this.buttonYes.TabIndex = 4;
			this.buttonYes.Text = "Да";
			this.buttonYes.Click += new System.EventHandler(this.Buttons_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonOK.Image = global::Sim.Controls.Properties.Resources.OK;
			this.buttonOK.Location = new System.Drawing.Point(165, 0);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 26);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "ОК";
			this.buttonOK.Click += new System.EventHandler(this.Buttons_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonCancel.Image = global::Sim.Controls.Properties.Resources.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(245, 0);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(81, 26);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.Click += new System.EventHandler(this.Buttons_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBox1.Image = global::Sim.Controls.Properties.Resources.Info48;
			this.pictureBox1.Location = new System.Drawing.Point(0, 21);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(58, 55);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 15;
			this.pictureBox1.TabStop = false;
			// 
			// finistLabelText
			// 
			this.finistLabelText.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistLabelText.Location = new System.Drawing.Point(58, 21);
			this.finistLabelText.MaximumSize = new System.Drawing.Size(315, 400);
			this.finistLabelText.MinimumSize = new System.Drawing.Size(315, 55);
			this.finistLabelText.Name = "finistLabelText";
			this.finistLabelText.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
			this.finistLabelText.Size = new System.Drawing.Size(315, 55);
			this.finistLabelText.TabIndex = 17;
			this.finistLabelText.WordWrap = true;
			// 
			// SimModalMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MinimumSize = new System.Drawing.Size(370, 107);
			this.MoveControl = this.finistPanelHeader;
			this.Name = "SimModalMessageBox";
			this.Size = new System.Drawing.Size(370, 115);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanel2.ResumeLayout(false);
			this.finistPanelButtons.ResumeLayout(false);
			this.finistPanelButtons.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SimPanel finistPanelHeader;
		internal SimLabel finistLabelCaption;
		private SimPanel finistPanel2;
		private SimPopupButton btnCancel;
		private SimPanel finistPanelButtons;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private Sim.Controls.SimButton buttonNo;
		private Sim.Controls.SimButton buttonYes;
		private Sim.Controls.SimButton buttonOK;
		private Sim.Controls.SimButton buttonCancel;
		private SimLabel finistLabelText;
	}
}
