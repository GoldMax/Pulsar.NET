using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sim.Common
{
	/// <summary>
	/// Класс контрола импорта данных из clipboard
	/// </summary>
	public partial class CtrlClipboardImport : Sim.Controls.SimModalDialogBase
	{
		private Type _tt;
		private IList _res = null;
		private string[] _colNames = null;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Тип кортежей результата.
		/// </summary>
		public Type TupleType
		{
			get { return _tt; }
			set { _tt = value; }
		}
		/// <summary>
		/// Результирующий список
		/// </summary>
		public IList Result
		{
			get { return _res; }
		}
		/// <summary>
		/// Наименования столбцов
		/// </summary>
		public string[] ColumnNames
		{
			get { return _colNames; }
			set { _colNames = value; }
		}

		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public CtrlClipboardImport()
		{
			InitializeComponent();
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Controls Handlers>>
		public override void Show()
		{
			base.Show();
			buttonRefresh_Click(null, null);
		}
		private void buttonRefresh_Click(object sender, EventArgs e)
		{
		 fdgvRes.DataSource = null;
			Parse();
			if(_res == null)
			 return;
			fdgvRes.DataSource = new Pulsar.ListBinder(_res);
			if(_colNames == null)
				fdgvRes.ColumnHeadersVisible = false;
			else
			 for(int a = 0; a < _colNames.Length; a++)
				 if(a < fdgvRes.ColumnCount)
					 fdgvRes.Columns[a].HeaderText = _colNames[a];
			fdgvRes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
		}
		#endregion << Controls Handlers>>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		 
		private void Parse()
		{
			try
			{
			 if(_tt == null)
				 throw new Exception("Не указан тип кортежа результата!");
				if(_tt.IsGenericType == false)
				 throw new Exception("Тип кортежа не является generic типом!");
				IDataObject iData = Clipboard.GetDataObject();
				if(iData.GetDataPresent(DataFormats.UnicodeText) == false)
					return;

				Type[] attrs  = _tt.GetGenericArguments();
				if(attrs.Length == 0)
				 return;

				Type tl = typeof(List<>).MakeGenericType(_tt);
				_res = (IList)Activator.CreateInstance(tl);

							
				string s = (String)iData.GetData(DataFormats.UnicodeText);
				
				string[] rows = s.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				foreach(var row in rows)
				{
					string[] cc = row.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if(cc.Length < attrs.Length)
					 continue;
					object[] vals = new object[attrs.Length];
					try
					{
						for(int a = 0; a < attrs.Length; a++)
						{
						 if(attrs[a] == typeof(string))
							 vals[a] = cc[a].Trim();
							else
							 vals[a] = Convert.ChangeType(cc[a].Replace(" ",""), attrs[a]);
							if(vals[a] == null)
							 goto End;
						}
					}
					catch
					{
						continue;
					}

					_res.Add(Activator.CreateInstance(_tt, vals));

					End:
					 continue;
				}
			}
			catch(Exception Err)
			{
				Sim.Controls.ErrorBox.Show(Err);
				_res = null;
			}
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
						
	}
}
