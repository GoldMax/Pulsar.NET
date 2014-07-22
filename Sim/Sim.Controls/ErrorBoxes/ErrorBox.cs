using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace Sim.Controls
{
	//*************************************************************************************
	#region ErrorBoxForm class
	internal class ErrorBoxForm : System.Windows.Forms.Form
	{
		private Sim.Controls.SimButton button1;
		internal Sim.Controls.SimTextBox textBoxTrace;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		internal Sim.Controls.SimLabel textBoxSource;
		internal Sim.Controls.SimLabel textBoxClass;
		internal Sim.Controls.SimLabel textBoxMethod;
		internal Sim.Controls.SimLabel textBoxFile;
		internal Sim.Controls.SimLabel textBoxLine;
		internal Sim.Controls.SimLabel textBoxMessage;
		private Sim.Controls.SimButton button2;
		private Label label7;

		private System.ComponentModel.Container components = null;

		public ErrorBoxForm()
		{
			InitializeComponent();
			this.Height -= 100;
			button2.Top += 100;
			//label7.Top +=100;
			//textBoxTrace.Visible = false;
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
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
   System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorBoxForm));
   this.pictureBox1 = new System.Windows.Forms.PictureBox();
   this.label1 = new System.Windows.Forms.Label();
   this.label2 = new System.Windows.Forms.Label();
   this.label3 = new System.Windows.Forms.Label();
   this.label4 = new System.Windows.Forms.Label();
   this.label5 = new System.Windows.Forms.Label();
   this.label6 = new System.Windows.Forms.Label();
   this.label7 = new System.Windows.Forms.Label();
   this.button2 = new Sim.Controls.SimButton();
   this.textBoxMessage = new Sim.Controls.SimLabel();
   this.textBoxLine = new Sim.Controls.SimLabel();
   this.textBoxFile = new Sim.Controls.SimLabel();
   this.textBoxMethod = new Sim.Controls.SimLabel();
   this.textBoxClass = new Sim.Controls.SimLabel();
   this.textBoxSource = new Sim.Controls.SimLabel();
   this.textBoxTrace = new Sim.Controls.SimTextBox();
   this.button1 = new Sim.Controls.SimButton();
   ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
   this.SuspendLayout();
   // 
   // pictureBox1
   // 
   this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
   this.pictureBox1.InitialImage = null;
   this.pictureBox1.Location = new System.Drawing.Point(18, 18);
   this.pictureBox1.Name = "pictureBox1";
   this.pictureBox1.Size = new System.Drawing.Size(32, 32);
   this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
   this.pictureBox1.TabIndex = 2;
   this.pictureBox1.TabStop = false;
   // 
   // label1
   // 
   this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.label1.Location = new System.Drawing.Point(76, 6);
   this.label1.Name = "label1";
   this.label1.Size = new System.Drawing.Size(50, 13);
   this.label1.TabIndex = 9;
   this.label1.Text = "Модуль";
   // 
   // label2
   // 
   this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.label2.Location = new System.Drawing.Point(76, 22);
   this.label2.Name = "label2";
   this.label2.Size = new System.Drawing.Size(50, 13);
   this.label2.TabIndex = 10;
   this.label2.Text = "Класс";
   // 
   // label3
   // 
   this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.label3.Location = new System.Drawing.Point(76, 38);
   this.label3.Name = "label3";
   this.label3.Size = new System.Drawing.Size(50, 13);
   this.label3.TabIndex = 11;
   this.label3.Text = "Метод";
   // 
   // label4
   // 
   this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.label4.Location = new System.Drawing.Point(76, 54);
   this.label4.Name = "label4";
   this.label4.Size = new System.Drawing.Size(50, 13);
   this.label4.TabIndex = 12;
   this.label4.Text = "Файл";
   // 
   // label5
   // 
   this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.label5.Location = new System.Drawing.Point(76, 70);
   this.label5.Name = "label5";
   this.label5.Size = new System.Drawing.Size(50, 13);
   this.label5.TabIndex = 13;
   this.label5.Text = "Строка";
   // 
   // label6
   // 
   this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
   this.label6.Location = new System.Drawing.Point(76, 86);
   this.label6.Name = "label6";
   this.label6.Size = new System.Drawing.Size(50, 13);
   this.label6.TabIndex = 14;
   this.label6.Text = "Текст";
   // 
   // label7
   // 
   this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
   this.label7.Location = new System.Drawing.Point(22, 105);
   this.label7.Name = "label7";
   this.label7.Size = new System.Drawing.Size(407, 2);
   this.label7.TabIndex = 19;
   // 
   // button2
   // 
   this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
   this.button2.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
   this.button2.CausesValidation = false;
   this.button2.Font = new System.Drawing.Font("Arial Black", 9.75F);
   this.button2.Location = new System.Drawing.Point(3, 99);
   this.button2.Margin = new System.Windows.Forms.Padding(0);
   this.button2.Name = "button2";
   this.button2.Size = new System.Drawing.Size(14, 14);
   this.button2.TabIndex = 18;
   this.button2.Tag = "0";
   this.button2.Text = "+";
   this.button2.Click += new System.EventHandler(this.button2_Click);
   // 
   // textBoxMessage
   // 
   this.textBoxMessage.AutoSize = false;
   this.textBoxMessage.Location = new System.Drawing.Point(128, 86);
   this.textBoxMessage.Name = "textBoxMessage";
   this.textBoxMessage.Size = new System.Drawing.Size(300, 13);
   this.textBoxMessage.TabIndex = 8;
   this.textBoxMessage.WordWrap = true;
   this.textBoxMessage.TextChanged += new System.EventHandler(this.textBoxMessage_TextChanged);
   // 
   // textBoxLine
   // 
   this.textBoxLine.AutoSize = false;
   this.textBoxLine.Location = new System.Drawing.Point(128, 70);
   this.textBoxLine.Name = "textBoxLine";
   this.textBoxLine.Size = new System.Drawing.Size(300, 13);
   this.textBoxLine.TabIndex = 7;
   // 
   // textBoxFile
   // 
   this.textBoxFile.AutoSize = false;
   this.textBoxFile.Location = new System.Drawing.Point(128, 54);
   this.textBoxFile.Name = "textBoxFile";
   this.textBoxFile.Size = new System.Drawing.Size(300, 13);
   this.textBoxFile.TabIndex = 6;
   // 
   // textBoxMethod
   // 
   this.textBoxMethod.AutoSize = false;
   this.textBoxMethod.Location = new System.Drawing.Point(128, 38);
   this.textBoxMethod.Name = "textBoxMethod";
   this.textBoxMethod.Size = new System.Drawing.Size(300, 13);
   this.textBoxMethod.TabIndex = 5;
   // 
   // textBoxClass
   // 
   this.textBoxClass.AutoSize = false;
   this.textBoxClass.Location = new System.Drawing.Point(128, 22);
   this.textBoxClass.Name = "textBoxClass";
   this.textBoxClass.Size = new System.Drawing.Size(300, 13);
   this.textBoxClass.TabIndex = 4;
   // 
   // textBoxSource
   // 
   this.textBoxSource.AutoSize = false;
   this.textBoxSource.BorderColor = System.Drawing.SystemColors.Control;
   this.textBoxSource.Location = new System.Drawing.Point(128, 6);
   this.textBoxSource.Name = "textBoxSource";
   this.textBoxSource.Size = new System.Drawing.Size(300, 13);
   this.textBoxSource.TabIndex = 3;
   // 
   // textBoxTrace
   // 
   this.textBoxTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.textBoxTrace.Cursor = System.Windows.Forms.Cursors.Arrow;
   this.textBoxTrace.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.textBoxTrace.Location = new System.Drawing.Point(3, 116);
   this.textBoxTrace.Multiline = true;
   this.textBoxTrace.Name = "textBoxTrace";
   this.textBoxTrace.ReadOnly = true;
   this.textBoxTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
   this.textBoxTrace.Size = new System.Drawing.Size(426, 100);
   this.textBoxTrace.TabIndex = 1;
   this.textBoxTrace.Visible = false;
   this.textBoxTrace.WordWrap = false;
   // 
   // button1
   // 
   this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
   this.button1.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
   this.button1.Location = new System.Drawing.Point(179, 225);
   this.button1.Name = "button1";
   this.button1.Size = new System.Drawing.Size(75, 23);
   this.button1.TabIndex = 0;
   this.button1.Text = "OK";
   this.button1.Click += new System.EventHandler(this.button1_Click);
   // 
   // ErrorBoxForm
   // 
   this.ClientSize = new System.Drawing.Size(432, 254);
   this.Controls.Add(this.label7);
   this.Controls.Add(this.button2);
   this.Controls.Add(this.label6);
   this.Controls.Add(this.label5);
   this.Controls.Add(this.label4);
   this.Controls.Add(this.label3);
   this.Controls.Add(this.label2);
   this.Controls.Add(this.label1);
   this.Controls.Add(this.textBoxMessage);
   this.Controls.Add(this.textBoxLine);
   this.Controls.Add(this.textBoxFile);
   this.Controls.Add(this.textBoxMethod);
   this.Controls.Add(this.textBoxClass);
   this.Controls.Add(this.textBoxSource);
   this.Controls.Add(this.textBoxTrace);
   this.Controls.Add(this.pictureBox1);
   this.Controls.Add(this.button1);
   this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
   this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
   this.MaximizeBox = false;
   this.MinimizeBox = false;
   this.Name = "ErrorBoxForm";
   this.ShowInTaskbar = false;
   this.Text = "Ошибка!";
   this.TopMost = true;
   ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
   this.ResumeLayout(false);
   this.PerformLayout();

		}
		#endregion

		private void textBoxMessage_TextChanged(object sender, EventArgs e)
		{
			using(Graphics g = textBoxMessage.CreateGraphics())
			{
				StringFormat sf = new StringFormat();
				var s = g.MeasureString(textBoxMessage.Text, textBoxMessage.Font,textBoxMessage.Width); 
				textBoxMessage.Height = (int)s.Height;
			}
			this.Height += textBoxMessage.Height - textBoxMessage.PreferredSize.Height;
			this.button2.Anchor = AnchorStyles.Top;
			this.label7.Top += textBoxMessage.Height - textBoxMessage.PreferredSize.Height;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if(button2.Text == "+")
			{
				this.Height += 100;
				textBoxTrace.Visible = true;
				button2.Text = "-";
				button2.Tag = 1;
				button1.Focus();
			}
			else
			{
				textBoxTrace.Visible = false;
				this.Height -= 100;
				button2.Text = "+";
				button2.Tag = 0;
				button1.Focus();
			}
		}

		private void groupBox1_Enter(object sender, EventArgs e)
		{

		}
	}
	#endregion ErrorBoxForm class
	//*************************************************************************************
	/// <summary>
	/// Класс сообщения об ощибке
	/// </summary>
	public class ErrorBox
	{
		private delegate void ErrorBoxShowDelegate(Exception Exc, Form mainForm);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Отображает сообщение об ошибке.
		/// </summary>
		/// <param name="Exc">Исключение, перехваченное обработчиком catch.</param>
		public static void Show(Exception Exc)
		{
			try
			{
				if(Application.OpenForms.Count > 0)
				{
					if(Application.OpenForms[0].InvokeRequired)
						Application.OpenForms[0].Invoke(new ErrorBoxShowDelegate(Show), Exc, Application.OpenForms[0]);
					else
						Show(Exc, Application.OpenForms[0]);
				}
				else
					Show(Exc, null);
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.Message, "Ошибка ErrorBox.Show", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Отображает сообщение об ошибке.
		/// </summary>
		/// <param name="Exc">Исключение, перехваченное обработчиком catch.</param>
		/// <param name="mainForm">Форма, относительно которой диалог сообщения об ошибке должен быть модальным.</param>
		public static void Show(Exception Exc, Form mainForm)
		{
			ErrorBoxForm form = null;
			try
			{
				form = new ErrorBoxForm();
				form.textBoxSource.Text = Exc.Source;
				form.textBoxClass.Text = Exc.TargetSite.DeclaringType.FullName;
				form.textBoxMethod.Text = Exc.TargetSite.ToString();
				form.textBoxMessage.Text = Exc.Message;

				StackTrace trace = new System.Diagnostics.StackTrace(Exc, true);
				for(int a = 0; a < trace.FrameCount; a++)
					if(trace.GetFrame(a).GetFileName() != null)
					{
						form.textBoxFile.Text = System.IO.Path.GetFileName(trace.GetFrame(a).GetFileName());
						form.textBoxLine.Text = trace.GetFrame(a).GetFileLineNumber().ToString();
						break;
					}

				string s = "Стек исключения:\r\n";
				s += trace.ToString();
				s += "\r\nСтек программы:\r\n";
				s += (new StackTrace(1)).ToString();
				s = s.Replace("\t", "").Trim();
				s = s.Replace("at ", "\u2514\u25ba ");
				s = s.Replace("в ", "\u2514\u25ba ");
				form.textBoxTrace.Text = s; //\u2500

				SystemSounds.Exclamation.Play();
				if(mainForm == null)
					form.ShowDialog();
				else
					form.ShowDialog(mainForm);
			}
			catch
			{
				MessageBox.Show(Exc.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if(form != null)
					form.Dispose();
			}
		}
	}
}
