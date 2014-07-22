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
	/// Делегат обработчика события закрытия диалога.
	/// </summary>
	/// <param name="sender">Объект, посылающий события.</param>
	/// <param name="result">Результат закрытия диалога.</param>
	public delegate void DialogClosedEventHandler(object sender, DialogResult result);
	//**************************************************************************************
	/// <summary>
	/// Класс контрола, выступающего в качестве модального окна.
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
		/// Событие, возникающее после закрытия диалога.
		/// </summary>
		public event DialogClosedEventHandler DialogClosed
		{
			add { _DialogClosed += value; }
			remove { _DialogClosed -= value; }
		}
		/// <summary>
		/// Вызывает событие DialogClosed.
		/// </summary>
		protected virtual void OnDialogClosed(DialogResult arg)
		{
			if (_DialogClosed != null)
				_DialogClosed.Raise(this, arg);
		}
		#endregion << public event DialogClosedEventHandler DialogClosed >>

		#region << public event EventHandler<DialogClosingEventArgs> DialogClosing >>
		/// <summary>
		/// Класс данных события DialogClosing 
		/// </summary>
		public class DialogClosingEventArgs : CancelEventArgs
		{
			/// <summary>
			/// Результат закрытия диалога.
			/// </summary>
			public DialogResult Result { get; set; }
			/// <summary>
			/// Инициализирующий конструктор.
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
		/// Событие, вызываемое до закрытия диалога.
		/// </summary>
		public event EventHandler<DialogClosingEventArgs> DialogClosing
		{
			add { _DialogClosing += value; }
			remove { _DialogClosing -= value; }
		}
		/// <summary>
		/// Вызывает событие DialogClosing.
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
		/// Родительский контрол, относительно которого эмулируется модальность.
		/// </summary>
		public new Control Parent
		{
			get { return parent; }
			set { parent = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает коллекцию дочерних контролов.
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
		/// Контрол, за который можно перемещать окно.
		/// </summary>
		[Description("Контрол, за который можно перемещать окно.")]
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
		/// Определяет ширину рамки в пикселях.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет ширину рамки в пикселях.")]
		[DefaultValue((byte)2)]
		public byte BorderWidth
		{
			get { return (byte)this.Padding.All; }
			set { this.Padding = new Padding(value); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет рамки.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет цвет рамки.")]
		[DefaultValue(typeof(Color), "ControlDarkDark")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { base.BackColor = borderColor; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет фона.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет цвет фона.")]
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
		/// Конструктор по умолчанию.
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
		/// Отображает контрол.
		/// </summary>
		public virtual new void Show()
		{
			try
			{
				if (parent == null)
					throw new Exception("Не указан родительский контрол!");
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
		/// Отображает контрол.
		/// </summary>
		/// <param name="parent">Родительский контрол, относительно которого эмулируется модальность.</param>
		public virtual void Show(Control parent)
		{
			this.Parent = parent;
			this.Show();
		}
		/// <summary>
		/// Отображает контрол в указанной позиции
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual void Show(int x, int y)
		{
			try
			{
				if (parent == null)
					throw new Exception("Не указан родительский контрол!");
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
		/// Отображает контрол модально.
		/// </summary>
		/// <param name="parent">Родительский контрол, относительно которого эмулируется модальность.</param>
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
		/// Скрывает контрол. Не вызывает событий закрытия.
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
		/// Скрывает контрол. Вызывает события закрытия.
		/// </summary>
		/// <param name="result">Результат закрытия.</param>
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
