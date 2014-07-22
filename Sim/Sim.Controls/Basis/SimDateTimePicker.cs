using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls.Common
{
	/// <summary>
	/// Класс контрола выбора даты.
	/// </summary>
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	public class SimDateTimePicker : SimComboBox
	{
		private MonthCalendar mc = new MonthCalendar();
		private ComboBoxItem<DateTime> _val = null;
		private string format = "dd.MM.yyyy";
		private DateTime _minDate = new DateTime(2000,1,1);
		private DateTime _maxDate = DateTime.MaxValue;

		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << public new event EventHandler ValueChanged >>
		/// <summary>
		/// Событие, возникающее при изменении значения контрола пользователем (не через свойство Value).
		/// </summary>
		[Category("Action")]
		[Description("Событие, возникающее при изменении значения контрола пользователем (не через свойство Value).")]
		public event EventHandler ValueChanged;
		/// <summary>
		/// Вызывает событие ValueChanged.
		/// </summary>
		protected void OnValueChanged()
		{
			if(ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		protected override void OnUISelectedItemChanged()
		{
			base.OnUISelectedItemChanged();
			OnValueChanged();
		}
		#endregion << public new event EventHandler ValueChanged >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Значение контрола.
		/// </summary>
		[Category("Behavior")]
		[Description("Значение контрола.")]
		public DateTime Value
		{
			get { return _val.Key; }
			set 
			{ 
				if(value > _maxDate)
					value = _maxDate;
				if(value < _minDate)
					value = _minDate;
				_val.Key = value;
				UpdateText();
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override object SelectedItem
		{
			get { return _val; }
			set 
			{
				if(value is ComboBoxItem<DateTime> == false)
					throw new Exception("Значение должно быть типа ComboBoxItem<DateTime>!");
				base.SelectedItem = value;
				_val = (ComboBoxItem<DateTime>)value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Формат отображения значения
		/// </summary>
		[Category("Behavior")]
		[Description("Формат отображения значения.")]
		[DefaultValue("dd.MM.yyyy")]
		public string Format
		{
			get { return format; }
			set 
			{ 
				format = value; 
				UpdateText();
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Минимальное значение
		/// </summary>
		[Category("Behavior")]
		[Description("Минимальное значение")]
		public DateTime MinValue
		{
			get { return _minDate; }
			set 
			{ 
				_minDate = value; 
				if(_val != null && _val.Key < _minDate)
					Value = _minDate;
				mc.MinDate = _minDate;
			}
		}
		/// <summary>
		/// Максимальное значение
		/// </summary>
		[Category("Behavior")]
		[Description("Максимальное значение")]
		public DateTime MaxValue
		{
			get { return _minDate; }
			set 
			{ 
				_maxDate = value;
				if(_val != null && _val.Key > _maxDate)
					Value = _maxDate;
				mc.MaxDate = _maxDate;
			}
		}

		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDateTimePicker() : base() 
		{
			_val = new ComboBoxItem<DateTime>(DateTime.Now.Date);
			UpdateText();
			base.SelectedItem = _val;

			SimPanel p = new SimPanel();
			//SimLabel l = new SimLabel();
			//l.Text = "Очистить";
			//l.Image = Properties.Resources.Delete_big;
			//l.Cursor = Cursors.Hand;
			//l.Dock = DockStyle.Top;
			//l.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			//l.Click += (s, ee) => { this.SelectedItem = null; this.DropDown.Hide(); };
			//p.Controls.Add(l);
			mc.MaxSelectionCount = 1;
			mc.ShowToday = false;
			mc.DateSelected += (s, ee) =>
			{
				this.DropDown.Hide();
				if(_val.Key != ((MonthCalendar)s).SelectionStart.Date)
				{
					Value = ((MonthCalendar)s).SelectionStart.Date;
					OnValueChanged();
				}
			};
			mc.Location = new Point(10, 0); //l.Height);
			p.Controls.Add(mc);
			mc.BringToFront();
			p.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			p.AutoSize = true;
			p.BackColor = Color.Transparent;
			this.DropDown = new SimPopupControl(p);
			this.DropDown.Opening += (s,e) => mc.SelectionStart = Value;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		private void UpdateText()
		{
			if(format == null || format.Length == 0)
				_val.Value = _val.Key.ToString();
			else
				_val.Value = _val.Key.ToString(format);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
						
	}
}
