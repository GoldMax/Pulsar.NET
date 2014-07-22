namespace Sim.Common
{
	partial class CtrlClipboardImport
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
			this.buttonRefresh = new Sim.Controls.SimButton();
			this.finistLabel1 = new Sim.Controls.SimLabel();
			this.fdgvRes = new Sim.Controls.SimDataGridView();
			this.panelBody.SuspendLayout();
			this.finistPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fdgvRes)).BeginInit();
			this.SuspendLayout();
			// 
			// panelBody
			// 
			this.panelBody.Controls.Add(this.fdgvRes);
			this.panelBody.Controls.Add(this.finistLabel1);
			this.panelBody.Size = new System.Drawing.Size(485, 339);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(324, 4);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(405, 4);
			// 
			// finistPanel1
			// 
			this.finistPanel1.Controls.Add(this.buttonRefresh);
			this.finistPanel1.Location = new System.Drawing.Point(0, 360);
			this.finistPanel1.Size = new System.Drawing.Size(485, 35);
			this.finistPanel1.Controls.SetChildIndex(this.buttonRefresh, 0);
			this.finistPanel1.Controls.SetChildIndex(this.buttonCancel, 0);
			this.finistPanel1.Controls.SetChildIndex(this.buttonOK, 0);
			// 
			// panel1
			// 
			this.panel1.Size = new System.Drawing.Size(485, 395);
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.buttonRefresh.Image = global::Sim.Common.Properties.Resources.Refresh;
			this.buttonRefresh.Location = new System.Drawing.Point(12, 4);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(94, 26);
			this.buttonRefresh.TabIndex = 1;
			this.buttonRefresh.Text = "Обновить";
			this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
			// 
			// finistLabel1
			// 
			this.finistLabel1.AutoEllipsis = true;
			this.finistLabel1.AutoSize = true;
			this.finistLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.finistLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.finistLabel1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.finistLabel1.Location = new System.Drawing.Point(3, 3);
			this.finistLabel1.Name = "finistLabel1";
			this.finistLabel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.finistLabel1.Size = new System.Drawing.Size(133, 19);
			this.finistLabel1.TabIndex = 0;
			this.finistLabel1.Text = "Данные для импорта";
			// 
			// fdgvRes
			// 
			this.fdgvRes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fdgvRes.Location = new System.Drawing.Point(3, 22);
			this.fdgvRes.Name = "fdgvRes";
			this.fdgvRes.RowHeadersVisible = false;
			this.fdgvRes.RowTemplate.Height = 18;
			this.fdgvRes.Size = new System.Drawing.Size(479, 314);
			this.fdgvRes.TabIndex = 1;
			this.fdgvRes.VirtualMode = true;
			// 
			// CtrlClipboardImport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ButtonOkEnabled = true;
			this.Caption = "Импорт из буфера обмена";
			this.CaptionImage = global::Sim.Common.Properties.Resources.Paste;
			this.Name = "CtrlClipboardImport";
			this.Size = new System.Drawing.Size(489, 399);
			this.panelBody.ResumeLayout(false);
			this.panelBody.PerformLayout();
			this.finistPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fdgvRes)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Controls.SimButton buttonRefresh;
		private Controls.SimDataGridView fdgvRes;
		private Controls.SimLabel finistLabel1;
	}
}
