using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
	/// <summary>
	/// ������� ����������� ������� �������� �������.
	/// </summary>
	/// <param name="sender">������, ���������� �������.</param>
	/// <param name="result">��������� �������� �������.</param>
	public delegate void DialogClosedEventHandler(object sender, DialogResult result);
	//**************************************************************************************
	/// <summary>
	/// ����� ��������, ������������ � �������� ���������� ����.
	/// </summary>
	[DefaultEvent("DialogClosed")]
	[DefaultProperty("MoveControl")]
	public partial class SimModalControl : UserControl
	{
		protected DialogResult result = DialogResult.None;


		private Control parent = null;
		private Control moveControl = null;
		private Point mp = Point.Empty;
		private bool init = true;
		//private byte borderWidth = 2;
		private Color borderColor = SystemColors.ControlDarkDark;
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		#region << public event DialogClosedEventHandler DialogClosed >>
		[NonSerialized]
		private Pulsar.WeakEvent<DialogResult> _DialogClosed;
		/// <summary>
		/// �������, ����������� ����� �������� �������.
		/// </summary>
		public event DialogClosedEventHandler DialogClosed
		{
			add { _DialogClosed += value; }
			remove { _DialogClosed -= value; }
		}
		/// <summary>
		/// �������� ������� DialogClosed.
		/// </summary>
		protected virtual void OnDialogClosed(DialogResult arg)
		{
			if (_DialogClosed != null)
				_DialogClosed.Raise(this, arg);
		}
		#endregion << public event DialogClosedEventHandler DialogClosed >>

		#region << public event EventHandler<DialogClosingEventArgs> DialogClosing >>
		/// <summary>
		/// ����� ������ ������� DialogClosing 
		/// </summary>
		public class DialogClosingEventArgs : CancelEventArgs
		{
			/// <summary>
			/// ��������� �������� �������.
			/// </summary>
			public DialogResult Result { get; set; }
			/// <summary>
			/// ���������������� �����������.
			/// </summary>
			public DialogClosingEventArgs(DialogResult result)
				: base(false)
			{
				Result = result;
			}
		}

		[NonSerialized]
		private Pulsar.WeakEvent<DialogClosingEventArgs> _DialogClosing;
		/// <summary>
		/// �������, ���������� �� �������� �������.
		/// </summary>
		public event EventHandler<DialogClosingEventArgs> DialogClosing
		{
			add { _DialogClosing += value; }
			remove { _DialogClosing -= value; }
		}
		/// <summary>
		/// �������� ������� DialogClosing.
		/// </summary>
		protected virtual bool OnDialogClosing(DialogResult result)
		{
			if (_DialogClosing != null)
			{
				DialogClosingEventArgs args = new DialogClosingEventArgs(result);
				_DialogClosing.Raise(this, args);
				return args.Cancel;
			}
			return false;
		}

		#endregion << public event EventHandler<DialogClosingEventArgs> DialogClosing >>
		#endregion << Events >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// ������������ �������, ������������ �������� ����������� �����������.
		/// </summary>
		public new Control Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ��������� �������� ���������.
		/// </summary>
		public new ControlCollection Controls
		{
			get
			{
				if (init)
					return base.Controls;
				return panel1.Controls;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// �������, �� ������� ����� ���������� ����.
		/// </summary>
		[Description("�������, �� ������� ����� ���������� ����.")]
		[DefaultValue(null)]
		public Control MoveControl
		{
			get { return moveControl; }
			set
			{
				if (moveControl != null)
				{
					moveControl.MouseMove -= new MouseEventHandler(moveControl_MouseMove);
					moveControl.MouseDown -= new MouseEventHandler(moveControl_MouseDown);
				}
				moveControl = value;
				if (value != null)
				{
					moveControl.MouseMove += new MouseEventHandler(moveControl_MouseMove);
					moveControl.MouseDown += new MouseEventHandler(moveControl_MouseDown);
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ������ ����� � ��������.
		/// </summary>
		[Category("Appearance")]
		[Description("���������� ������ ����� � ��������.")]
		[DefaultValue((byte)2)]
		public byte BorderWidth
		{
			get { return (byte)this.Padding.All; }
			set { this.Padding = new Padding(value); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ���� �����.
		/// </summary>
		[Category("Appearance")]
		[Description("���������� ���� �����.")]
		[DefaultValue(typeof(Color), "ControlDarkDark")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { base.BackColor = borderColor; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ���������� ���� ����.
		/// </summary>
		[Category("Appearance")]
		[Description("���������� ���� ����.")]
		[DefaultValue(typeof(Color), "Control")]
		public new Color BackColor
		{
			get { return panel1.BackColor; }
			set { panel1.BackColor = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// ����������� �� ���������.
		/// </summary>
		public SimModalControl()
		{
			base.BackColor = SystemColors.ControlDarkDark;
			InitializeComponent();
			init = false;
			panel1.BackColor = SystemColors.Control;
			this.Visible = false;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Overrides Methods & Handlers >>
		/// <summary>
		/// ���������� �������.
		/// </summary>
		public virtual new void Show()
		{
			try
			{
				if (parent == null)
					throw new Exception("�� ������ ������������ �������!");
				foreach (Control c in parent.Controls)
					if (c.Equals(this) == false)
						c.Enabled = false;
				parent.Controls.Add(this);
				this.BringToFront();
				int x = (parent.Width - this.Width) / 2;
				int y = (parent.Height - this.Height) / 2;
				this.Location = new Point(x < 0 ? 0 : x, y < 0 ? 0 : y);
				base.Show();
			}
			catch (Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		/// <summary>
		/// ���������� �������.
		/// </summary>
		/// <param name="parent">������������ �������, ������������ �������� ����������� �����������.</param>
		public virtual void Show(Control parent)
		{
			this.Parent = parent;
			this.Show();
		}
		/// <summary>
		/// ���������� ������� � ��������� �������
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual void Show(int x, int y)
		{
			try
			{
				if (parent == null)
					throw new Exception("�� ������ ������������ �������!");
				foreach (Control c in parent.Controls)
					if (c.Equals(this) == false)
						c.Enabled = false;
				parent.Controls.Add(this);
				this.BringToFront();
				this.Location = new Point(x < 0 ? 0 : x, y < 0 ? 0 : y);
				this.Show();
			}
			catch (Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		/// <summary>
		/// ���������� ������� ��������.
		/// </summary>
		/// <param name="parent">������������ �������, ������������ �������� ����������� �����������.</param>
		/// <returns></returns>
		public DialogResult ShowModal(Control parent)
		{
			this.Parent = parent;
			this.Show();
   while(result == DialogResult.None && this.Visible)
			{
				if (this.IsDisposed || parent.IsDisposed || this.FindForm() == null)
					break;
				Application.DoEvents();
				System.Threading.Thread.Sleep(10);
			}
			return result == DialogResult.None ? DialogResult.Cancel : result;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// �������� �������. �� �������� ������� ��������.
		/// </summary>
		public new void Hide()
		{
			try
			{
				base.Hide();
				foreach (Control c in parent.Controls)
					if (c.Equals(this) == false)
						c.Enabled = true;
				//base.Parent.Controls.Remove(this);  
			}
			catch (Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		/// <summary>
		/// �������� �������. �������� ������� ��������.
		/// </summary>
		/// <param name="result">��������� ��������.</param>
		public void Hide(DialogResult result)
		{
			this.result = result;
			if (OnDialogClosing(result))
				return;
			Hide();
			OnDialogClosed(result);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnParentChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentChanged(EventArgs e)
		{
			parent = base.Parent;
		}
		//-------------------------------------------------------------------------------------
		void moveControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			mp = moveControl.PointToClient(Control.MousePosition);
		}
		//-------------------------------------------------------------------------------------
		void moveControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			if (parent.ClientRectangle.Contains(parent.PointToClient(Control.MousePosition)))
			{
				Size s = new Size(moveControl.PointToClient(Control.MousePosition));
				this.Location = Point.Subtract(this.Location, new Size(Point.Subtract(mp, s)));
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			//if(keyData == Keys.Escape)
			//{
			// this.Hide();
			// OnDialogClosed(DialogResult.Cancel);
			// return true;
			//}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		#endregion << Overrides Methods & Handlers >>

	}
}
