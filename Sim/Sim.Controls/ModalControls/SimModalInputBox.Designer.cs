namespace Sim.Controls
{
	partial class SimModalInputBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimModalInputBox));
			this.finistPanelHeader = new Sim.Controls.SimPanel();
			this.finistLabelCaption = new Sim.Controls.SimLabel();
			this.btnCancel = new Sim.Controls.SimPopupButton();
			this.finistPanel1 = new Sim.Controls.SimPanel();
			this.buttonOK = new Sim.Controls.SimButton();
			this.buttonCancel = new Sim.Controls.SimButton();
			this.finistLabelText = new System.Windows.Forms.Label();
			this.finistPanel3 = new Sim.Controls.SimPanel();
			this.finistTextBoxValue = new Sim.Controls.SimTextBox();
			this.panel1.SuspendLayout();
			this.finistPanelHeader.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.finistPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.finistLabelText);
			this.panel1.Controls.Add(this.finistPanel3);
			this.panel1.Controls.Add(this.finistPanel1);
			this.panel1.Controls.Add(this.finistPanelHeader);
			this.panel1.Size = new System.Drawing.Size(386, 115);
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
			this.finistPanelHeader.Size = new System.Drawing.Size(386, 21);
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
			this.finistLabelCaption.Image = global::Sim.Controls.Properties.Resources.Rename;
			this.finistLabelCaption.Location = new System.Drawing.Point(0, 0);
			this.finistLabelCaption.Name = "finistLabelCaption";
			this.finistLabelCaption.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.finistLabelCaption.Size = new System.Drawing.Size(365, 21);
			this.finistLabelCaption.TabIndex = 1;
			this.finistLabelCaption.Text = "Ввод значения";
			// 
			// btnCancel
			// 
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnCancel.Image = global::Sim.Controls.Properties.Resources.NormalXbtn2_c;
			this.btnCancel.ImagePushed = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImagePushed")));
			this.btnCancel.ImageRaised = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageRaised")));
			this.btnCancel.Location = new System.Drawing.Point(365, 0);
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
			this.finistPanel1.Location = new System.Drawing.Point(0, 80);
			this.finistPanel1.MinimumSize = new System.Drawing.Size(386, 35);
			this.finistPanel1.Name = "finistPanel1";
			this.finistPanel1.Size = new System.Drawing.Size(386, 35);
			this.finistPanel1.TabIndex = 10;
			// 
			// buttonOK
			// 
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
			this.buttonCancel.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonCancel.Image = global::Sim.Controls.Properties.Resources.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(308, 4);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 26);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "Отмена";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// finistLabelText
			// 
			this.finistLabelText.AutoEllipsis = true;
			this.finistLabelText.AutoSize = true;
			this.finistLabelText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.finistLabelText.Location = new System.Drawing.Point(0, 21);
			this.finistLabelText.MaximumSize = new System.Drawing.Size(386, 200);
			this.finistLabelText.MinimumSize = new System.Drawing.Size(386, 24);
			this.finistLabelText.Name = "finistLabelText";
			this.finistLabelText.Padding = new System.Windows.Forms.Padding(5);
			this.finistLabelText.Size = new System.Drawing.Size(386, 24);
			this.finistLabelText.TabIndex = 11;
			this.finistLabelText.Text = "Введите новое значение:";
			// 
			// finistPanel3
			// 
			this.finistPanel3.Controls.Add(this.finistTextBoxValue);
			this.finistPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.finistPanel3.Location = new System.Drawing.Point(0, 45);
			this.finistPanel3.MinimumSize = new System.Drawing.Size(386, 35);
			this.finistPanel3.Name = "finistPanel3";
			this.finistPanel3.Size = new System.Drawing.Size(386, 35);
			this.finistPanel3.TabIndex = 12;
			// 
			// finistTextBoxValue
			// 
			this.finistTextBoxValue.Location = new System.Drawing.Point(7, 6);
			this.finistTextBoxValue.MaxLength = 1000;
			this.finistTextBoxValue.Name = "finistTextBoxValue";
			this.finistTextBoxValue.Size = new System.Drawing.Size(373, 21);
			this.finistTextBoxValue.TabIndex = 0;
			this.finistTextBoxValue.TextChanged += new System.EventHandler(this.finistTextBoxValue_TextChanged);
			this.finistTextBoxValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.finistTextBoxValue_KeyPress);
			// 
			// SimModalInputBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.MoveControl = this.finistPanelHeader;
			this.Name = "SimModalInputBox";
			this.Size = new System.Drawing.Size(390, 119);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.finistPanelHeader.ResumeLayout(false);
			this.finistPanel1.ResumeLayout(false);
			this.finistPanel3.ResumeLayout(false);
			this.finistPanel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SimPanel finistPanelHeader;
		internal SimLabel finistLabelCaption;
		private SimPopupButton btnCancel;
		private SimPanel finistPanel1;
		private Sim.Controls.SimButton buttonOK;
		private Sim.Controls.SimButton buttonCancel;
		private SimPanel finistPanel3;
		private SimTextBox finistTextBoxValue;
		private System.Windows.Forms.Label finistLabelText;
	}
}
