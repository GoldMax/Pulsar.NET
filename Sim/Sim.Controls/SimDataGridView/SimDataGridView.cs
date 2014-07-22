using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Sim.Controls.WinAPI;
using Pulsar;

namespace Sim.Controls
{
	//**************************************************************************************
	/// <summary>
	/// Класс перегруженного DataGridView
	/// </summary>
	//[TypeConverter(typeof(ExpandableObjectConverter))]
	public class SimDataGridView : DataGridView
	{
		#region protected ToolStripMenuItem
#pragma warning disable
		protected ToolStripMenuItem toolStripMenuItemCopy;
		protected ToolStripMenuItem toolStripMenuItemCopyCell;
		protected ToolStripMenuItem toolStripMenuItemSelectAll;
		protected ToolStripMenuItem toolStripMenuItemExport;
		protected ToolStripMenuItem toolStripMenuItemExportExcel;
		protected ToolStripMenuItem toolStripMenuItemExportCsv;
		protected ToolStripMenuItem toolStripMenuItemFind;
		protected ToolStripMenuItem toolStripMenuItemCount;
#pragma warning restore
		#endregion protected ToolStripMenuItem

		private static DataGridViewCellStyle disabledStyle = new DataGridViewCellStyle()
		{
			BackColor = SystemColors.Control,
			ForeColor = SystemColors.GrayText,
			SelectionBackColor = SystemColors.ControlDark,
			SelectionForeColor = SystemColors.Control
		};

		private System.ComponentModel.IContainer components;

		private Color borderColor = SystemColors.ControlDark;
		private BorderStyle borderStyle = BorderStyle.FixedSingle;
		private Padding borderWidth = new Padding(1);
		private bool useVisualStyleBorderColor = true;
		//private Color[] disableColors = new Color[6];
		private DataGridViewCellStyle[] currentStyles = new DataGridViewCellStyle[4];
		private Color backColor2 = SystemColors.Window;
		private GradientMode gradMode = GradientMode.None;
		private Image backImage = null;

		private SimContextMenu contextMenuStrip1;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;

		private SaveFileDialog saveFileDialog1;
		private SimDataGridViewFindForm find = null;
		internal string prevFind = "";

		private bool enterMovesDown = true;
		private bool allowMultiSort = true;
		private bool menuEnable = true;
		private bool autoGenCols = true;
		private bool hideSel = false;
		private int _autoColWidth = 0;
		private Dictionary<Type, DataGridViewColumn> typedCols = new Dictionary<Type, DataGridViewColumn>();

		private bool raiseSelectionChanged = true;

		private IList source = null;
		private Dictionary<string, PropertyDescriptor> props = new Dictionary<string, PropertyDescriptor>();
		private PDictionary<string, ListSortDirection> sorts = new PDictionary<string, ListSortDirection>();
		private HashSet<DataGridViewColumn> autoGenColumns = new HashSet<DataGridViewColumn>();
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		#region public event EventHandler<object,UnboundColumnCellValueNeedEventArgs> UnboundColumnCellValueNeed
		#region << public class UnboundColumnCellValueNeedEventArgs : EventArgs >>
		/// <summary>
		/// Класс аргумента события UnboundColumnCellValueNeed
		/// </summary>
		public class UnboundColumnCellValueNeedEventArgs : EventArgs
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			#region << Properties >>
			/// <summary>
			/// Столбец
			/// </summary>
			public DataGridViewColumn Column { get; private set; }
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Объект сроки.
			/// </summary>
			public object BoundedObject { get; private set; }
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			/// <summary>
			/// Значение столбца.
			/// </summary>
			public object Value { get; set; }
			#endregion << Properties >>
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//-------------------------------------------------------------------------------------
			#region << Constructors >>
			/// <summary>
			/// Конструктор по умолчанию.
			/// </summary>
			public UnboundColumnCellValueNeedEventArgs(DataGridViewColumn column, object boundedObject)
			{
				Column = column;
				BoundedObject = boundedObject;
			}
			//-------------------------------------------------------------------------------------

			#endregion << Constructors >>
			//-------------------------------------------------------------------------------------

		}
		#endregion << public class UnboundColumnCellValueNeedEventArgs : EventArgs >>

		/// <summary>
		/// Событие, возникающее при необходимости получения значения объекта строки для открепленного столбца.
		/// </summary>
		[Description("Событие, возникающее при необходимости получения значения объекта строки для открепленного столбца.")]
		public event EventHandler<object, UnboundColumnCellValueNeedEventArgs> UnboundColumnCellValueNeed;
		/// <summary>
		/// Вызывает событие UnboundColumnCellValueNeed
		/// </summary>
		/// <param name="column">Столбец</param>
		/// <param name="boundedObject">Объект сроки.</param>
		/// <returns></returns>
		protected internal virtual object OnUnboundColumnCellValueNeed(DataGridViewColumn column, object boundedObject)
		{
			if(UnboundColumnCellValueNeed == null)
				return null;
			UnboundColumnCellValueNeedEventArgs args = new UnboundColumnCellValueNeedEventArgs(column, boundedObject);
			UnboundColumnCellValueNeed(this, args);
			return args.Value;
		}
		#endregion public event EventHandler<object,UnboundColumnCellValueNeedEventArgs> UnboundColumnCellValueNeed

		#region public event DataGridViewCellMouseEventHandler CellImageClick
		/// <summary>
		/// Событие, возникающее при клике на картинке ячейки.
		/// </summary>
		public event DataGridViewCellMouseEventHandler CellImageClick;
		/// <summary>
		/// Вызывает событие CellImageClick
		/// </summary>
		protected internal virtual void OnCellImageClick(DataGridViewCellMouseEventArgs e)
		{
			if(CellImageClick != null)
				CellImageClick(this, e);
		}
		#endregion public event DataGridViewCellMouseEventHandler CellImageClick

		#region public event EventHandler<object,DataGridViewNeedCellImageEventArgs> NeedCellImage
		/// <summary>
		/// Класс аргумента события NeedCellImage
		/// </summary>
		public class DataGridViewNeedCellImageEventArgs : DataGridViewCellEventArgs
		{
			/// <summary>
			/// Картинка ячейки
			/// </summary>
			public Image Image { get; set; }
			/// <summary>
			/// Инициализирующий конструктор.
			/// </summary>
			public DataGridViewNeedCellImageEventArgs(int rowIndex, int columnIndex, Image img)
				: base(columnIndex, rowIndex)
			{
				Image = img;
			}

		}
		/// <summary>
		/// Событие, возникающее при необходимости определить катринку ячейки.
		/// </summary>
		public event EventHandler<object, DataGridViewNeedCellImageEventArgs> NeedCellImage;
		/// <summary>
		/// Вызывает событие CellImageClick
		/// </summary>
		/// <param name="rowIndex">Индекс строки ячейки.</param>
		/// <param name="columnIndex">Индекс столбца ячейки.</param>
		/// <param name="img">Значение картинки по умолчанию.</param>
		protected internal virtual Image OnNeedCellImage(int rowIndex, int columnIndex, Image img)
		{
			if(NeedCellImage == null)
				return img;
			var arg = new DataGridViewNeedCellImageEventArgs(rowIndex, columnIndex, img);
			NeedCellImage(this, arg);
			return arg.Image;
		}
		#endregion public event EventHandler<object,DataGridViewNeedCellImageEventArgs> CellImageClick
		#endregion << Events >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Border & Background Properties >>
		/// <summary>
		/// Определяет цвет рамки при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет цвет рамки при BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color BorderColor
		{
			get { return borderColor; }
			set
			{
				borderColor = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет вид рамки.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет вид рамки.")]
		[DefaultValue(typeof(BorderStyle), "FixedSingle")]
		public new BorderStyle BorderStyle
		{
			get { return borderStyle; }
			set
			{
				if(borderStyle != value)
				{
					borderStyle = value;
					this.RefreshBorder();
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет ширину рамки при BorderStyle = FixedSingle.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет ширину рамки при BorderStyle = FixedSingle.")]
		[DefaultValue(typeof(Padding), "1, 1, 1, 1")]
		public Padding BorderWidth
		{
			get { return borderWidth; }
			set
			{
				borderWidth = value;
				this.RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будут ли использованы цвета схемы.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет, будут ли использованы цвета схемы.")]
		[DefaultValue(true)]
		public bool UseVisualStyleBorderColor
		{
			get { return useVisualStyleBorderColor; }
			set
			{
				useVisualStyleBorderColor = value;
				RefreshBorder();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет фона.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Window")]
		[Description("Определяет цвет фона.")]
		public new Color BackgroundColor
		{
			get { return base.BackgroundColor; }
			set
			{
				base.BackgroundColor = value;
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет второй цвет градиентной заливки.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Window")]
		[Description("Определяет второй цвет градиентной заливки.")]
		public Color BackgroundColor2
		{
			get { return backColor2; }
			set
			{
				backColor2 = value;
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет вид градиентной заливки фона.
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(typeof(GradientMode), "None")]
		[Description("Определяет вид градиентной заливки фона.")]
		public GradientMode BackGradientMode
		{
			get { return gradMode; }
			set
			{
				gradMode = value;
				Invalidate();
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет изображение фона.
		/// </summary>
		[Category("Appearance")]
		[Description("Определяет изображение фона.")]
		[Browsable(true)]
		public new Image BackgroundImage
		{
			get { return backImage; }
			set
			{
				backImage = value;
				Invalidate();
			}
		}
		#endregion << Border & Background Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Own Properties >>
		/// <summary>
		/// Определяет, будет ли курсор опускаться при нажатии клавиши Enter.(Блокирует KeyDown для Enter).
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет, будет ли курсор опускаться при нажатии клавиши Enter.")]
		[DefaultValue(true)]
		public bool EnterMovesDown
		{
			get { return enterMovesDown; }
			set { enterMovesDown = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет режим скрытия выбора при потере фокуса.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет режим скрытия выбора при потере фокуса.")]
		[DefaultValue(false)]
		public bool HideSelection
		{
			get { return hideSel; }
			set { hideSel = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли отображаться контекстное меню.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет, будет ли отображаться контекстное меню.")]
		[DefaultValue(true)]
		[RefreshProperties(RefreshProperties.All)]
		public bool ContextMenuEnable
		{
			get { return menuEnable; }
			set
			{
				menuEnable = value;
				if(menuEnable)
					this.ContextMenuStrip = this.contextMenuStrip1;
				else
					this.ContextMenuStrip = null;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли меняться текущаяя ячейка по правому клику.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет, будет ли меняться текущаяя ячейка по правому клику.")]
		[DefaultValue(false)]
		public bool RightClickChangeCurrent { get; set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли возможно сортировать таблицу по нескольким столбцам.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет, будет ли возможно сортировать таблицу по нескольким столбцам.")]
		[DefaultValue(true)]
		public bool AllowMultipleSort
		{
			get
			{
				if(DesignMode == true || source == null)
					return allowMultiSort;
				else if((source as IBindingListView) != null && (source as ITypedList) != null &&
																								((IBindingListView)source).SupportsAdvancedSorting)
					return allowMultiSort;
				else
					return false;
			}
			set
			{
				allowMultiSort = value;
				if(allowMultiSort)
				{
					if((source as IBindingListView) != null && (source as ITypedList) != null &&
																							((IBindingListView)source).SupportsAdvancedSorting)
						DrawMultiSort();
				}
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будут ли столбцы создаваться автоматически.
		/// </summary>
		[Category("Behavior")]
		[Description("Определяет, будут ли столбцы создаваться автоматически.")]
		[DefaultValue(true)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool AllowAutoGenerateColumns
		{
			get { return autoGenCols; }
			set
			{
				autoGenCols = value;
				base.AutoGenerateColumns = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// AutoGenerateColumn
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoGenerateColumns
		{
			get { return autoGenCols; }
			set { AllowAutoGenerateColumns = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Коллекция столбцов для указанных типов, используемых при автоматическом создании столбцов.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Dictionary<Type, DataGridViewColumn> TypedColumns
		{
			get { return typedCols; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Ширина по умолчанию для автоматически создаваемых столбцов.
		/// 0 - ширина по умолчанию.
		/// </summary>
		[Category("Behavior")]
		[Description("Ширина по умолчанию для автоматически создаваемых столбцов.")]
		[DefaultValue(0)]
		public int AutoGenerateColumnsWidth
		{
			get { return _autoColWidth; }
			set { _autoColWidth = value; }
		}
		#endregion << Own Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Data Properties >>
		/// <summary>
		/// Определяет источник данных.
		/// </summary>
		[Category("Data")]
		[Description("Определяет источник данных.")]
		[DefaultValue(null)]
		public new object DataSource
		{
			get { return source; }
			set
			{
				Rows.Clear();
				if(source != null && source is IBindingList)
					((IBindingList)source).ListChanged -= new ListChangedEventHandler(DataSource_ListChanged);
				if(value == null)
				{
					if(source != null && source is IBindingList)
						((IBindingList)source).ListChanged -= new ListChangedEventHandler(DataSource_ListChanged);
					foreach(DataGridViewColumn col in autoGenColumns)
						if(Columns.Contains(col))
							Columns.Remove(col);
					autoGenColumns.Clear();
					props.Clear();
					source = null;
					return;
				}


				if(value is IList == false)
					if(value is IListSource)
						source = ((IListSource)value).GetList();
					else
						throw new Exception("Источник данных должен реализовать интерфейс IList!");

				source = (IList)value;

				if(value is ITypedList)
					foreach(PropertyDescriptor pd in ((ITypedList)value).GetItemProperties(null))
					{
						if(pd.IsBrowsable == false)
							continue;
						if(props.ContainsKey(pd.Name) == false)
							props.Add(pd.Name, pd);

						if(autoGenCols == true)
						{
							bool has = false;
							foreach(DataGridViewColumn c in Columns)
								if(c.DataPropertyName == pd.Name)
								{
									has = true;
									break;
								}
							if(has)
								continue;

							DataGridViewColumn col = null;
							if(pd.PropertyType == typeof(Boolean))
								col = new DataGridViewCheckBoxColumn();
							else if(pd.PropertyType == typeof(Image) || pd.PropertyType.IsSubclassOf(typeof(Image)))
								col = new DataGridViewImageColumn();
							else if(typedCols.ContainsKey(pd.PropertyType))
								col = (DataGridViewColumn)typedCols[pd.PropertyType].Clone();
							else
							{
								col = new DataGridViewTextBoxColumn();
								if(pd.PropertyType.IsInterface)
								{
									FieldInfo fi = typeof(DataGridViewColumn).GetField("boundColumnConverter", BindingFlags.NonPublic | BindingFlags.Instance);
									fi.SetValue(col, new TypeConverter());
								}
							}

							if(_autoColWidth != 0)
								col.Width = _autoColWidth;
							col.Name = pd.Name;
							col.DataPropertyName = pd.Name;
							col.HeaderText = pd.DisplayName;
							col.HeaderCell.ToolTipText = pd.Description;
							col.ReadOnly = pd.IsReadOnly;
							col.ValueType = pd.PropertyType;
							this.Columns.Add(col);
							autoGenColumns.Add(col);
						}
					}
				RowCount = source.Count;
				if(source is IBindingList)
					((IBindingList)source).ListChanged += new ListChangedEventHandler(DataSource_ListChanged);
			}
		}
		/// <summary>
		/// Закрыто.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string DataMember
		{
			get { return null; }
		}
		[Browsable(false)]
		public Dictionary<string, PropertyDescriptor> Props
		{
			get { return props; }
		}

		#endregion << Data Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Reset defaults >>
		/// <summary>
		/// пределяет возможность редактирования.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("Определяет возможность редактирования.")]
		[Browsable(true)]
		public new bool ReadOnly
		{
			get { return base.ReadOnly; }
			set { base.ReadOnly = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ShowCellErrors
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(false)]
		public new bool ShowCellErrors
		{
			get { return base.ShowCellErrors; }
			set { base.ShowCellErrors = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ShowRowErrors
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(false)]
		public new bool ShowRowErrors
		{
			get { return base.ShowRowErrors; }
			set { base.ShowRowErrors = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ShowEditingIcon
		/// </summary>
		[Category("Appearance")]
		[DefaultValue(false)]
		public new bool ShowEditingIcon
		{
			get { return base.ShowEditingIcon; }
			set { base.ShowEditingIcon = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// AllowUserToAddRows
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public new bool AllowUserToAddRows
		{
			get { return base.AllowUserToAddRows; }
			set { base.AllowUserToAddRows = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// AllowUserToDeleteRows
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public new bool AllowUserToDeleteRows
		{
			get { return base.AllowUserToDeleteRows; }
			set { base.AllowUserToDeleteRows = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// AllowUserToOrderColumns
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		public new bool AllowUserToOrderColumns
		{
			get { return base.AllowUserToOrderColumns; }
			set { base.AllowUserToOrderColumns = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// AllowUserToResizeRows
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public new bool AllowUserToResizeRows
		{
			get { return base.AllowUserToResizeRows; }
			set { base.AllowUserToResizeRows = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// ColumnHeadersHeightSizeMode
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(typeof(DataGridViewColumnHeadersHeightSizeMode), "AutoSize")]
		public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
		{
			get { return base.ColumnHeadersHeightSizeMode; }
			set { base.ColumnHeadersHeightSizeMode = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// SelectionMode
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(typeof(DataGridViewSelectionMode), "FullRowSelect")]
		public new DataGridViewSelectionMode SelectionMode
		{
			get { return base.SelectionMode; }
			set { base.SelectionMode = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Столбец сортировки (при AllowMultipleSort = false).
		/// </summary>
		public new DataGridViewColumn SortedColumn { get; private set; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Направление сортировки (при AllowMultipleSort = false).
		/// </summary>
		public new SortOrder SortOrder { get; private set; }
		#endregion << Reset defaults >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Events >>
		/// <summary>
		/// Делегат события PreviewMouseDown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void SimPreviewMouseDown(object sender, SimMouseEventArgs args);
		/// <summary>
		/// Событие, возникающее до обработки события нажатия кнопки мыши.
		/// </summary>
		[Description("Событие, возникающее до обработки события нажатия кнопки мыши.")]
		public event SimPreviewMouseDown PreviewMouseDown;
		/// <summary>
		/// Вызывает событие PreviewMouseDown.
		/// </summary>
		/// <param name="args"></param>
		protected void OnPreviewMouseDown(SimMouseEventArgs args)
		{
			if(PreviewMouseDown != null)
				PreviewMouseDown(this, args);
		}
		#endregion << Events >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridView()
			: base()
		{
			currentStyles[0] = new DataGridViewCellStyle();
			currentStyles[3] = new DataGridViewCellStyle();

			InitializeComponent();

			if(LicenseManager.UsageMode != LicenseUsageMode.Designtime)
			{
				DataGridViewCellStyle st = new DataGridViewCellStyle(base.DefaultCellStyle);
				st.Alignment = DataGridViewContentAlignment.MiddleLeft;
				st.BackColor = System.Drawing.Color.Transparent;
				st.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
				st.ForeColor = SystemColors.ControlText;
				st.SelectionBackColor = ProfessionalColors.ButtonSelectedHighlight;
				st.SelectionForeColor = ForeColor;
				st.WrapMode = DataGridViewTriState.False;
				base.DefaultCellStyle = st;

				base.RowHeadersDefaultCellStyle.ApplyStyle(st);
			}

			base.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			base.VirtualMode = true;

			if(menuEnable)
				this.ContextMenuStrip = this.contextMenuStrip1;

			DoubleBuffered = true;
			this.VerticalScrollBar.ValueChanged += new EventHandler(VerticalScrollBar_ValueChanged);
			this.HorizontalScrollBar.ValueChanged += new EventHandler(HorizontalScrollBar_ValueChanged);

			DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
			col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			typedCols.Add(typeof(decimal), col);
			typedCols.Add(typeof(int), col);
			typedCols.Add(typeof(byte), col);
			Type t = Type.GetType("Pulsar.Amount, Pulsar.Atoms", false);
			if(t != null)
				typedCols.Add(t, col);
			if((t = Type.GetType("Pulsar.Money, Pulsar.Atoms", false)) != null)
				typedCols.Add(t, col);
			if((t = Type.GetType("Pulsar.MoneyPrecise, Pulsar.Atoms", false)) != null)
				typedCols.Add(t, col);
		}
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.contextMenuStrip1 = new Sim.Controls.SimContextMenu(this.components);
			this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemCopyCell = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemFind = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemCount = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemExportExcel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemExportCsv = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.RestoreDirectory = true;
			this.saveFileDialog1.Title = "Выберите файл для экспорта";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.toolStripMenuItemCopy,
												this.toolStripMenuItemCopyCell,
												this.toolStripMenuItemSelectAll,
												this.toolStripSeparator1,
												this.toolStripMenuItemFind,
												this.toolStripMenuItemCount,
												this.toolStripSeparator2,
												this.toolStripMenuItemExport});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(198, 148);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// toolStripMenuItemCopy
			// 
			this.toolStripMenuItemCopy.Image = global::Sim.Controls.Properties.Resources.Copy;
			this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
			this.toolStripMenuItemCopy.ShortcutKeyDisplayString = "Ctrl - C";
			this.toolStripMenuItemCopy.Size = new System.Drawing.Size(197, 22);
			this.toolStripMenuItemCopy.Text = "Копировать";
			this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// toolStripMenuItemCopyCell
			// 
			this.toolStripMenuItemCopyCell.Name = "toolStripMenuItemCopyCell";
			this.toolStripMenuItemCopyCell.Size = new System.Drawing.Size(197, 22);
			this.toolStripMenuItemCopyCell.Text = "Копировать ячейку";
			this.toolStripMenuItemCopyCell.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// toolStripMenuItemSelectAll
			// 
			this.toolStripMenuItemSelectAll.Name = "toolStripMenuItemSelectAll";
			this.toolStripMenuItemSelectAll.ShortcutKeyDisplayString = "Ctrl - A";
			this.toolStripMenuItemSelectAll.Size = new System.Drawing.Size(197, 22);
			this.toolStripMenuItemSelectAll.Text = "Выделить все";
			this.toolStripMenuItemSelectAll.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
			// 
			// toolStripMenuItemFind
			// 
			this.toolStripMenuItemFind.Image = global::Sim.Controls.Properties.Resources.Find;
			this.toolStripMenuItemFind.Name = "toolStripMenuItemFind";
			this.toolStripMenuItemFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.toolStripMenuItemFind.Size = new System.Drawing.Size(197, 22);
			this.toolStripMenuItemFind.Text = "Поиск";
			this.toolStripMenuItemFind.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// toolStripMenuItemCount
			// 
			this.toolStripMenuItemCount.Name = "toolStripMenuItemCount";
			this.toolStripMenuItemCount.Size = new System.Drawing.Size(197, 22);
			this.toolStripMenuItemCount.Text = "Число строк";
			this.toolStripMenuItemCount.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(194, 6);
			// 
			// toolStripMenuItemExport
			// 
			this.toolStripMenuItemExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
												this.toolStripMenuItemExportExcel,
												this.toolStripMenuItemExportCsv});
			this.toolStripMenuItemExport.Image = global::Sim.Controls.Properties.Resources.ExportToFile;
			this.toolStripMenuItemExport.Name = "toolStripMenuItemExport";
			this.toolStripMenuItemExport.Size = new System.Drawing.Size(197, 22);
			this.toolStripMenuItemExport.Text = "Экспорт данных";
			// 
			// toolStripMenuItemExportExcel
			// 
			this.toolStripMenuItemExportExcel.Image = global::Sim.Controls.Properties.Resources.XLS;
			this.toolStripMenuItemExportExcel.Name = "toolStripMenuItemExportExcel";
			this.toolStripMenuItemExportExcel.Size = new System.Drawing.Size(196, 22);
			this.toolStripMenuItemExportExcel.Text = "Экспорт в файл Excel ";
			this.toolStripMenuItemExportExcel.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// toolStripMenuItemExportCsv
			// 
			this.toolStripMenuItemExportCsv.Image = global::Sim.Controls.Properties.Resources.CSV;
			this.toolStripMenuItemExportCsv.Name = "toolStripMenuItemExportCsv";
			this.toolStripMenuItemExportCsv.Size = new System.Drawing.Size(196, 22);
			this.toolStripMenuItemExportCsv.Text = "Экспорт в CSV файл";
			this.toolStripMenuItemExportCsv.Click += new System.EventHandler(this.toolStripMenuItems_Click);
			// 
			// SimDataGridView
			// 
			this.AllowUserToOrderColumns = true;
			this.AllowUserToResizeRows = false;
			this.AllowUserToAddRows = false;
			this.AllowUserToDeleteRows = false;
			this.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ReadOnly = true;
			this.RowTemplate.Height = 18;
			this.ShowCellErrors = false;
			this.ShowEditingIcon = false;
			this.ShowRowErrors = false;
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << DataGridView Methods >>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if(e.KeyCode == Keys.F && e.Modifiers == Keys.Control && this.Rows.Count > 0)
			{
				Find(-1);
				e.Handled = true;
			}
			base.OnKeyUp(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Осуществляет рисование в неклиенской области.
		/// </summary>
		/// <param name="g"></param>
		protected void OnNonClientPaint(Graphics g)
		{
			bool useVS = VisualStyleInformation.IsEnabledByUser &&
																										VisualStyleInformation.IsSupportedByOS &&
																										Application.RenderWithVisualStyles &&
																										useVisualStyleBorderColor;


			Rectangle r = Rectangle.Round(g.VisibleClipBounds);
			if(borderStyle == BorderStyle.FixedSingle)
			{
				SolidBrush b;
				if(useVS)
					b = new SolidBrush(VisualStyleInformation.TextControlBorder);
				else
					b = new SolidBrush(this.BorderColor);
				using(b)  //
				{
					g.FillRectangle(b, 0, 0, r.Width, borderWidth.Top);
					g.FillRectangle(b, r.Width - borderWidth.Right, 0, r.Width, r.Height);
					g.FillRectangle(b, 0, r.Height - borderWidth.Bottom, r.Width, r.Height);
					g.FillRectangle(b, 0, 0, borderWidth.Left, r.Height);
				}
			}
			else if(borderStyle == BorderStyle.Fixed3D)
			{
				g.DrawLine(SystemPens.ControlDarkDark, 1, 1, r.Width - 3, 1);  // Top
				g.DrawLine(SystemPens.ControlDarkDark, 1, 1, 1, r.Height - 3); // Left
				g.DrawLine(SystemPens.ControlLight, 1, r.Height - 2, r.Width - 2, r.Height - 2); // Bottom
				g.DrawLine(SystemPens.ControlLight, r.Width - 2, 1, r.Width - 2, r.Height - 2); // Right
				g.DrawLine(SystemPens.ControlDark, 0, 0, r.Width - 2, 0);  // Top
				g.DrawLine(SystemPens.ControlDark, 0, 0, 0, r.Height - 2); // Left
				g.DrawLine(SystemPens.ControlLightLight, 0, r.Height - 1, r.Width - 1, r.Height - 1); // Bottom
				g.DrawLine(SystemPens.ControlLightLight, r.Width - 1, 0, r.Width - 1, r.Height - 1); // Right
			}
			g.Flush();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обновляет рисование рамки.
		/// </summary>
		public void RefreshBorder()
		{
			WinAPI.APIWrappers.SetWindowPos(new HandleRef(this, this.Handle), new HandleRef(), 0, 0, 0, 0,
				0x0001 | 0x0002 | 0x0004 | 0x0010 | 0x0020 | 0x0200);
			Application.DoEvents();
		}
		#endregion << DataGridView Methods >>
		//-------------------------------------------------------------------------------------
		#region << Context Menu Handlers >>
		private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			OnDefaultContextMenuOpening(sender, e);
		}
		/// <summary>
		/// Метод, вызываемый при открытии контекстного меню по умолчанию.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e"></param>
		protected virtual void OnDefaultContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(this.RowCount == 0)
			{
				toolStripMenuItemCopy.Enabled = false;
				toolStripMenuItemCopyCell.Enabled = false;
				toolStripMenuItemSelectAll.Enabled = false;
				toolStripMenuItemExport.Enabled = false;
				toolStripMenuItemCount.Enabled = false;
				toolStripMenuItemFind.Enabled = false;
			}
			else
			{
				toolStripMenuItemCopy.Enabled = this.CurrentCell != null;
				toolStripMenuItemCopyCell.Enabled = this.CurrentCell != null;
				toolStripMenuItemSelectAll.Enabled = true;
				toolStripMenuItemExport.Enabled = true;
				toolStripMenuItemCount.Enabled = true;
				toolStripMenuItemFind.Enabled = true;
			}
		}
		//-------------------------------------------------------------------------------------
		private void toolStripMenuItems_Click(object sender, EventArgs e)
		{
			OnDefaultContextMenuItemsClick(sender, e);
		}
		/// <summary>
		/// Метод, вызываемый при выборе элемента контекстного меню по умолчанию.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">e</param>
		protected virtual void OnDefaultContextMenuItemsClick(object sender, EventArgs e)
		{
			try
			{
				if(sender as ToolStripSeparator != null)
					return;
				Point p = contextMenuStrip1.Location;
				contextMenuStrip1.Hide();
				switch(((ToolStripMenuItem)sender).Name)
				{
					case "toolStripMenuItemSelectAll":
						this.SelectAll();
						break;
					case "toolStripMenuItemCopy":
						{
							DataObject obj = GetClipboardContent();
							Clipboard.SetDataObject(obj, true);
						} break;
					case "toolStripMenuItemCopyCell":
						{
							DataObject obj = GetCellClipboardContent();
							Clipboard.SetDataObject(obj, true);
							break;
						}
					case "toolStripMenuItemFind":
						p = this.PointToClient(p);
						HitTestInfo hti = this.HitTest(p.X, p.Y);
						Find(hti.ColumnIndex);
						break;
					case "toolStripMenuItemCount":
						{
							SimLabel l = new SimLabel();
							l.Image = Properties.Resources.Info;
							l.Text = "Число строк : " + this.RowCount.ToString();
							l.BackColor = Color.Transparent;
							l.Width = l.GetPreferredSize(Size.Empty).Width + 10;
							SimPopupControl.Show(l, p, false, true);
							//MessageBox.Show("Число строк : " + this.RowCount.ToString(),
							//               "SimDataGridView Info",
							//               MessageBoxButtons.OK,
							//               MessageBoxIcon.Information);
						}
						break;
					case "toolStripMenuItemExportExcel":
						DoExport(true);
						break;
					case "toolStripMenuItemExportCsv":
						DoExport(false);
						break;
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		#endregion << Context Menu Handlers >>
		//-------------------------------------------------------------------------------------
		#region << Others Methods >>
		private void Find(int columnIndex)
		{
			if(this.Columns.Count == 0)
				return;
			if(find == null || find.IsDisposed)
				find = new SimDataGridViewFindForm(this, prevFind);
			Point xy = new Point(this.Right, this.Top);
			xy = this.PointToScreen(xy);
			xy.X -= (find.Width + 30);
			xy.Y += 20;
			find.Location = xy;
			find.Show(this, columnIndex);
		}
		//-------------------------------------------------------------------------------------
		private void DoExport(bool toExcel)
		{
			try
			{
				if(toExcel)
				{
					saveFileDialog1.DefaultExt = "xls";
					saveFileDialog1.Filter = "Файлы Excel(*.xls)|*.xls";
				}
				else
				{
					saveFileDialog1.DefaultExt = "csv";
					saveFileDialog1.Filter = "Файлы *.csv|*.csv";
				}
				saveFileDialog1.FileName = "";
				if(saveFileDialog1.ShowDialog(this) != DialogResult.OK)
					return;

				//Sim.Trace.TraceTime.BeginTrace(); 
				if(toExcel)
					DataExport.ExportToExcel(this, saveFileDialog1.FileName);
				else
					DataExport.ExportToCsv(this, saveFileDialog1.FileName);
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Копирует в clipboard выделенные ячейки. Удаляет формат html представления.
		/// </summary>
		/// <returns></returns>
		public override DataObject GetClipboardContent()
		{
			DataObject obj = base.GetClipboardContent();
			DataObject newObj = new DataObject();

			//this.Sele

			string[] types = obj.GetFormats();
			foreach(String type in types)
			{
				if(type != DataFormats.Html)
					newObj.SetData(type, obj.GetData(type));
			}
			return newObj;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Копирует в clipboard текущую ячейку. Удаляет формат html представления.
		/// </summary>
		/// <returns></returns>
		public DataObject GetCellClipboardContent()
		{
			DataObject obj = new DataObject(CurrentCell.FormattedValue);
			DataObject newObj = new DataObject();

			//this.Sele

			string[] types = obj.GetFormats();
			foreach(String type in types)
			{
				if(type != DataFormats.Html)
					newObj.SetData(type, obj.GetData(type));
			}
			return newObj;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Генерирует событие MouseClick.
		/// </summary>
		/// <param name="e">Аргументы события.</param>
		public void RaiseMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
		}
		//-------------------------------------------------------------------------------------
		private void AddSortColumn(DataGridViewColumn col, ListSortDirection direction = ListSortDirection.Ascending)
		{
			if(this.DataSource == null)// || props.ContainsKey(col.DataPropertyName) == false)
				return;
			try
			{
				if(AllowMultipleSort == false)
				{
					if((this.DataSource as IBindingList) == null)
						throw new Exception(String.Format("Источник данных [{0}] не поддерживает интерфейс IBindingList!",
																																								DataSource.GetType().FullName));
					IBindingList ibl = (IBindingList)this.DataSource;
					if(ibl.SupportsSorting == false)
						return;
					PropertyDescriptor pd = null;
					if(props.ContainsKey(col.DataPropertyName))
						pd = props[col.DataPropertyName];
					else
						pd = new UnboundColumnPropertyDescriptor(col);
					ListSortDirection lsd = direction;
					if(ibl.SortProperty != null && Object.Equals(ibl.SortProperty, pd))
						if(ibl.SortDirection == ListSortDirection.Ascending)
							lsd = ListSortDirection.Descending;
						else
							lsd = ListSortDirection.Ascending;
					ibl.ApplySort(pd, lsd);
					foreach(DataGridViewColumn c in Columns)
						c.HeaderCell.SortGlyphDirection = SortOrder.None;
					if(lsd == ListSortDirection.Ascending)
						col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
					else
						col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
					SortedColumn = col;
					SortOrder = lsd == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
				}
				else
				{
					if((this.DataSource as IBindingListView) == null)
						throw new Exception(String.Format("Источник данных [{0}] не поддерживает интерфейс IBindingListView!",
																																								DataSource.GetType().FullName));
					if((this.DataSource as ITypedList) == null)
						throw new Exception(String.Format("Источник данных [{0}] не поддерживает интерфейс ITypedList !",
																																								DataSource.GetType().FullName));
					if(((IBindingListView)DataSource).SupportsAdvancedSorting == false)
						throw new Exception(String.Format("Источник данных [{0}] не поддерживает сортировку по нескольким столбцам!",
																																								DataSource.GetType().FullName));

					PDictionary<string, ListSortDescription> sl = new PDictionary<string, ListSortDescription>();

					if(((IBindingListView)DataSource).SortDescriptions != null)
						foreach(ListSortDescription lsd in ((IBindingListView)DataSource).SortDescriptions)
							sl.Add(lsd.PropertyDescriptor.Name, lsd);

					List<string> toDel = new List<string>();
					if(ModifierKeys != Keys.Control)
						foreach(string s in sl.Keys)
							if(s != (String.IsNullOrEmpty(col.DataPropertyName) ? col.Name : col.DataPropertyName))
								toDel.Add(s);
					foreach(string s in toDel)
						sl.Remove(s);

					PropertyDescriptor pd = null;
					if(props.ContainsKey(col.DataPropertyName))
						pd = props[col.DataPropertyName];
					else
						pd = new UnboundColumnPropertyDescriptor(col);
					if(sl.ContainsKey(pd.Name))
					{
						if(sl[pd.Name].SortDirection == ListSortDirection.Ascending)
							sl[pd.Name].SortDirection = ListSortDirection.Descending;
						else
							sl[pd.Name].SortDirection = ListSortDirection.Ascending;
					}
					else
						sl.Add(pd.Name, new ListSortDescription(pd, direction));
					SortedColumn = col;
					SortOrder = sl[pd.Name].SortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
					ListSortDescription[] sd = new ListSortDescription[sl.Count];
					int c = 0;
					foreach(var i in sl.Values)
						sd[c++] = i;
					((IBindingListView)DataSource).ApplySort(new ListSortDescriptionCollection(sd));

					DrawMultiSort();

				}
				this.Refresh();
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		private void DrawMultiSort()
		{
			try
			{
				if(this.DataSource == null)
					return;
				if((this.DataSource as IBindingListView) == null)
					return;
				if(((IBindingListView)DataSource).SupportsAdvancedSorting == false)
					return;

				PDictionary<string, ListSortDescription> sl = new PDictionary<string, ListSortDescription>();

				if(((IBindingListView)DataSource).SortDescriptions != null)
					foreach(ListSortDescription lsd in ((IBindingListView)DataSource).SortDescriptions)
						sl.Add(lsd.PropertyDescriptor.Name, lsd);

				foreach(DataGridViewColumn col in this.Columns)
					if(sl.ContainsKey(col.Name))
					{
						if(sl[col.Name].SortDirection == ListSortDirection.Ascending)
							col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						else
							col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
					}
					else if(sl.ContainsKey(col.DataPropertyName))
					{
						if(sl[col.DataPropertyName].SortDirection == ListSortDirection.Ascending)
							col.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
						else
							col.HeaderCell.SortGlyphDirection = SortOrder.Descending;
					}
					else if(col.HeaderCell.SortGlyphDirection != SortOrder.None)
						col.HeaderCell.SortGlyphDirection = SortOrder.None;
			}
			catch
			{
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает изображением фона картинку фильтра.
		/// </summary>
		/// <param name="set">True - установить картинку, false - снять картинку.</param>
		public void SetFilteredBackImage(bool set)
		{
			if(set)
				backImage = global::Sim.Controls.Properties.Resources.Filtered;
			else
				backImage = null;
			this.Refresh();
		}
		//-------------------------------------------------------------------------------------
		private void AllColumnCellsSelect(int columnIndex)
		{
			raiseSelectionChanged = false;
			if(this.CurrentCell != null)
				this.CurrentCell = this[columnIndex, this.CurrentCell.RowIndex];
			for(int a = 0; a < this.Rows.Count; a++)
				if(a != this.CurrentCell.RowIndex)
					this[columnIndex, a].Selected = true;
			raiseSelectionChanged = true;
			OnSelectionChanged(EventArgs.Empty);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает PropertyDescriptor 
		/// <param name="propName">Имя свойства.</param>
		/// </summary>
		public PropertyDescriptor GetDescriptor(string propName)
		{
			return props.ContainsKey(propName) ? props[propName] : null;
		}
		#endregion << Others Methods >>
		//-------------------------------------------------------------------------------------
		#region << Overrides & handlers Methods >>
		void DataSource_ListChanged(object sender, ListChangedEventArgs e)
		{
			try
			{
				switch(e.ListChangedType)
				{
					case ListChangedType.Reset:
						{
							raiseSelectionChanged = false;
							//if(RowCount != source.Count)
							// if(source.Count == 0)
							//  SetCurrentCellAddressCore(-1, -1,false,false,false);
							// else if(CurrentCellAddress.Y >= source.Count)
							//  SetCurrentCellAddressCore(ColumnCount == 0? -1 : 0, source.Count-1, false, false, false);
							RowCount = source.Count;
							this.Invalidate();
							raiseSelectionChanged = true;
							OnSelectionChanged(EventArgs.Empty);
						} break;
					case ListChangedType.ItemAdded: this.Rows.Insert(e.NewIndex, 1); break;
					case ListChangedType.ItemDeleted: this.Rows.RemoveAt(e.NewIndex); break;
					case ListChangedType.ItemChanged: if(e.NewIndex >= 0) this.InvalidateRow(e.NewIndex); break;
					case ListChangedType.ItemMoved:
						int lo = Math.Min(e.NewIndex, e.OldIndex);
						int hi = Math.Max(e.NewIndex, e.OldIndex);
						for(int a = lo; a <= hi; a++)
							this.InvalidateRow(a);
						break;
				}
			}
			catch
			{
				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return && enterMovesDown == false)
				e.Handled = true;
			else
				base.OnKeyDown(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left && e.Clicks == 1 && ModifierKeys == Keys.Shift)
			{
				if(this.SelectionMode != DataGridViewSelectionMode.FullRowSelect)
				{
					AllColumnCellsSelect(e.ColumnIndex);
					return;
				}
			}
			if(this.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Automatic)
			{
				//if(e.Button == MouseButtons.Left && e.Clicks == 1 && ModifierKeys == Keys.Control)
				AddSortColumn(this.Columns[e.ColumnIndex]);
				//else
				// base.OnColumnHeaderMouseClick(e);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if(m.Msg == WM.LBUTTONDOWN || m.Msg == WM.LBUTTONDBLCLK)
			{
				#region
				SimMouseEventArgs arg =
																	new SimMouseEventArgs(MouseButtons.Left, m.Msg == WM.LBUTTONDOWN ? 1 : 2,
																																																						(short)((int)((long)m.LParam) & 0xffff),
																																																						(short)((int)((long)m.LParam) >> 0x10) & 0xffff, 0);
				OnPreviewMouseDown(arg);
				if(arg.Handled)
					return;
				base.WndProc(ref m);
				if((Form.MouseButtons & MouseButtons.Left) != MouseButtons.Left)
					base.OnMouseUp(arg);
				#endregion
			}
			else if(m.Msg == WM.RBUTTONDOWN || m.Msg == WM.RBUTTONDBLCLK)
			{
				#region
				SimMouseEventArgs arg =
																	new SimMouseEventArgs(MouseButtons.Right, m.Msg == WM.RBUTTONDOWN ? 1 : 2,
																																																						(short)((int)((long)m.LParam) & 0xffff),
																																																						(short)((int)((long)m.LParam) >> 0x10) & 0xffff, 0);
				OnPreviewMouseDown(arg);
				if(arg.Handled)
					return;
				if(m.Msg == WM.RBUTTONDOWN && RightClickChangeCurrent)
				{
					m.Msg = (int)WM.LBUTTONDOWN;
					m.WParam = (IntPtr)((int)m.WParam ^ 2);
					m.WParam = (IntPtr)((int)m.WParam | 1);
				}
				base.WndProc(ref m);
				#endregion
			}
			else if(m.Msg == WM.NCCALCSIZE)
			{
				#region
				//if(m.WParam == IntPtr.Zero)
				// return;
				m.Result = new IntPtr(0x0400);
				if(borderStyle == BorderStyle.FixedSingle)
				{
					NCCALCSIZE_PARAMS p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));
					p.rgc[0].left += borderWidth.Left;
					p.rgc[0].right -= borderWidth.Right;
					p.rgc[0].top += borderWidth.Top;
					p.rgc[0].bottom -= borderWidth.Bottom;
					Marshal.StructureToPtr(p, m.LParam, true);
				}
				else if(borderStyle == BorderStyle.Fixed3D)
				{
					int w = 2;
					NCCALCSIZE_PARAMS p = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));
					p.rgc[0].left += w;
					p.rgc[0].right -= w;
					p.rgc[0].top += w;
					p.rgc[0].bottom -= w;
					Marshal.StructureToPtr(p, m.LParam, true);
				}
				base.WndProc(ref m);
				#endregion
			}
			else if(m.Msg == WM.NCPAINT)
			{
				#region
				if(borderStyle != BorderStyle.None)
				{
					base.WndProc(ref m);
					IntPtr hdc = IntPtr.Zero;
					try
					{
						//hdc = APIWrappers.GetDCEx(this.Handle, (IntPtr)m.WParam,
						//            (int)(GetDCExFlags.DCX_WINDOW | GetDCExFlags.DCX_PARENTCLIP ));
						hdc = APIWrappers.GetWindowDC(this.Handle);
						if(hdc != IntPtr.Zero)
							using(Graphics g = Graphics.FromHdc(hdc))
								OnNonClientPaint(g);
					}
					catch(Exception ex)
					{
						throw ex;
					}
					finally
					{
						if(hdc != IntPtr.Zero)
							APIWrappers.ReleaseDC(this.Handle, hdc);
					}
					return;
				}
				base.WndProc(ref m);
				#endregion
			}
			else
				base.WndProc(ref m);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// PaintBackground
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="clipBounds"></param>
		/// <param name="gridBounds"></param>
		protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
		{
			if(Enabled == false)
				base.PaintBackground(graphics, clipBounds, gridBounds);
			else if(gradMode == GradientMode.None)
			{
				using(SolidBrush b = new SolidBrush(BackgroundColor))
					graphics.FillRectangle(b, gridBounds);
			}
			else
			{
				using(LinearGradientBrush b = new LinearGradientBrush(gridBounds, BackgroundColor, backColor2,
																																																										(LinearGradientMode)(int)gradMode))
					graphics.FillRectangle(b, gridBounds);
			}
			if(backImage != null)
			{
				graphics.DrawImageUnscaled(backImage,
																															this.DisplayRectangle.Width - backImage.Width - 1,
																															this.DisplayRectangle.Height - backImage.Height - 1);
			}
		}
		//-------------------------------------------------------------------------------------
		void HorizontalScrollBar_ValueChanged(object sender, EventArgs e)
		{
			Invalidate();
		}
		//-------------------------------------------------------------------------------------
		void VerticalScrollBar_ValueChanged(object sender, EventArgs e)
		{
			Invalidate();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnResize
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentEnabledChanged(EventArgs e)
		{
			if(Parent.Enabled == false && this.DefaultCellStyle.ForeColor != SystemColors.GrayText)
			{
				currentStyles[0].BackColor = this.BackgroundColor;
				currentStyles[1] = this.DefaultCellStyle;
				currentStyles[2] = this.RowHeadersDefaultCellStyle;
				currentStyles[3].BackColor = this.ColumnHeadersDefaultCellStyle.BackColor;
				currentStyles[3].ForeColor = this.ColumnHeadersDefaultCellStyle.ForeColor;
			}
			base.OnParentEnabledChanged(e);
		}
		/// <summary>
		/// OnEnabledChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			if(base.Enabled == false)
			{
				if(this.DefaultCellStyle.ForeColor != SystemColors.GrayText)
				{
					currentStyles[0].BackColor = this.BackgroundColor;
					currentStyles[1] = this.DefaultCellStyle;
					currentStyles[2] = this.RowHeadersDefaultCellStyle;
					currentStyles[3].BackColor = this.ColumnHeadersDefaultCellStyle.BackColor;
					currentStyles[3].ForeColor = this.ColumnHeadersDefaultCellStyle.ForeColor;
				}
				this.BackgroundColor = SystemColors.Control;
				this.DefaultCellStyle = disabledStyle;
				this.RowHeadersDefaultCellStyle = disabledStyle;
				this.ColumnHeadersDefaultCellStyle.BackColor = disabledStyle.BackColor;
				this.ColumnHeadersDefaultCellStyle.ForeColor = disabledStyle.ForeColor;
			}
			else
			{
				this.BackgroundColor = currentStyles[0].BackColor;
				this.DefaultCellStyle = currentStyles[1];
				this.RowHeadersDefaultCellStyle = currentStyles[2];
				this.ColumnHeadersDefaultCellStyle.BackColor = currentStyles[3].BackColor;
				this.ColumnHeadersDefaultCellStyle.ForeColor = currentStyles[3].ForeColor;
			}
			base.OnEnabledChanged(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Invalidate()
		/// </summary>
		public new void Invalidate()
		{
			base.Invalidate();
			DrawMultiSort();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Invalidate()
		/// </summary>
		/// <param name="invalidateChildren">Перерисовка дочерних элементов.</param>
		public new void Invalidate(bool invalidateChildren)
		{
			base.Invalidate(invalidateChildren);
			DrawMultiSort();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnSelectionChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			if(raiseSelectionChanged == false)// || (hideSel && Focused == false))
				return;
			base.OnSelectionChanged(e);
		}
		//protected override void OnCurrentCellChanged(EventArgs e)
		//{
		// if(raiseSelectionChanged == false || (hideSel && Focused == false))
		//  return;
		// base.OnCurrentCellChanged(e);
		//}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
		{
			base.OnColumnAdded(e);
			//if(ApplyNumericFormatForNewColumns == false || e.Column is DataGridViewTextBoxColumn == false) 
			// return;
			//Type t = e.Column.ValueType;
			//if(t == null)
			// return;
			//if(t.Equals(typeof(Byte)) || t.Equals(typeof(SByte)) || t.Equals(typeof(Int16)) || 
			//   t.Equals(typeof(UInt16)) || t.Equals(typeof(Int32)) || t.Equals(typeof(UInt32)) ||
			//   t.Equals(typeof(Int64)) || t.Equals(typeof(UInt64)) || t.Equals(typeof(Double)) || 
			//   t.Equals(typeof(Single)) || t.Equals(typeof(Decimal)))
			//{
			// e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			// e.Column.DefaultCellStyle.Format = "#,0.##";
			//}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// OnCellValueNeeded
		/// </summary>
		/// <param name="e"></param>
		protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
		{
			try
			{
				DataGridViewColumn col = Columns[e.ColumnIndex];
				if(props.ContainsKey(col.DataPropertyName))
				{
					if(RowCount > 0 && e.RowIndex < source.Count)
						e.Value = props[col.DataPropertyName].GetValue(source[e.RowIndex]);
				}
				else
				{
					e.Value = OnUnboundColumnCellValueNeed(col, this.Rows[e.RowIndex].GetData());
					if(e.Value == null)
						base.OnCellValueNeeded(e);
				}
			}
			catch
			{
				throw;
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает сортировку
		/// </summary>
		/// <param name="dataGridViewColumn">Столбец сортировки.</param>
		/// <param name="direction">Направление сортировки.</param>
		public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
		{
			AddSortColumn(dataGridViewColumn, direction);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// GetPreferredSize
		/// </summary>
		/// <param name="proposedSize"></param>
		/// <returns></returns>
		public override Size GetPreferredSize(Size proposedSize)
		{
			int h = 0;
			if(borderStyle != System.Windows.Forms.BorderStyle.None)
				h += borderWidth.Vertical;
			if(ColumnHeadersVisible)
				h += this.ColumnHeadersHeight;
			if(this.HorizontalScrollBar.Visible)
				h += this.HorizontalScrollBar.Height;
			h += this.Rows.GetRowsHeight(DataGridViewElementStates.None);

			int w = this.Columns.GetColumnsWidth(DataGridViewElementStates.None);
			return new Size(w, h);    // proposedSize.Width
		}
		//-------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			if(hideSel)
				base.OnSelectionChanged(e);
			base.OnEnter(e);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if(hideSel)
				CurrentCell = null;
		}
		#endregion << Overrides Methods >>
	}
	//**************************************************************************************
	#region << public class SimMouseEventArgs : MouseEventArgs >>
	/// <summary>
	/// Класс аргумента собития мыши.
	/// </summary>
	public class SimMouseEventArgs : MouseEventArgs
	{
		private bool handled = false;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Определяет, обработано ли событие.
		/// </summary>
		public bool Handled
		{
			get { return handled; }
			set { handled = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="arg"></param>
		public SimMouseEventArgs(MouseEventArgs arg)
			: base(arg.Button, arg.Clicks, arg.X, arg.Y, arg.Delta)
		{

		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="button"></param>
		/// <param name="clicks"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="delta"></param>
		public SimMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta) :
			base(button, clicks, x, y, delta)
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class SimMouseEventArgs : MouseEventArgs >>
	//**************************************************************************************
	#region << public static class DataExport >>
	/// <summary>
	/// Класс методов экспорта данных.
	/// </summary>
	public static class DataExport
	{
		//**************************************************************************************
		#region Common Definition
#pragma warning disable 1591
		#region COM Interfaces
		[ComImport, Guid("00000000-0000-0000-C000-000000000046"), ComConversionLoss]
		public interface IUnknown3
		{
			uint QueryInterface([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] IntPtr ppvObject);
			uint AddRef();
			uint Release();
		}

		[ComImport, Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D"), InterfaceType((short)1), ComConversionLoss]
		public interface ISequentialStream
		{
			uint Read([Out, MarshalAs(UnmanagedType.LPArray)] IntPtr pv, [In] uint cb, [Out] out uint pcbRead);
			//void RemoteRead(out byte pv, [In] uint cb, out uint pcbRead);
			uint Write([In, MarshalAs(UnmanagedType.BStr)] string pv, [In] uint cb, [Out] out uint pcbWritten);
			//void RemoteWrite([In] ref byte pv, [In] uint cb, out uint pcbWritten);
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4), ComConversionLoss]
		public struct tagRemSNB
		{
			public uint ulCntStr;
			public uint ulCntChar;
			[ComConversionLoss]
			public IntPtr rgString;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct _FILETIME
		{
			public uint dwLowDateTime;
			public uint dwHighDateTime;
		}

		[ComImport, Guid("0000000B-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComConversionLoss]
		public interface IStorage
		{
			uint CreateStream([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName, [In] uint grfMode, [In] uint reserved1, [In] uint reserved2, [Out, MarshalAs(UnmanagedType.Interface)] out IStream3 ppstm);
			uint OpenStream([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName, [In] uint reserved1, [In] uint grfMode, [In] uint reserved2, [Out, MarshalAs(UnmanagedType.Interface)] out IStream3 ppstm);
			//void RemoteOpenStream([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] uint cbReserved1, [In] ref byte reserved1, [In] uint grfMode, [In] uint reserved2, [MarshalAs(UnmanagedType.Interface)] out IStream ppstm);
			uint CreateStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] uint grfMode, [In] uint reserved1, [In] uint reserved2, [Out, MarshalAs(UnmanagedType.Interface)] out IStorage ppstg);
			uint OpenStorage([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage pstgPriority, [In] uint grfMode, [In, MarshalAs(UnmanagedType.Struct)] ref tagRemSNB snbExclude, [In] uint reserved, [Out, MarshalAs(UnmanagedType.Interface)] out IStorage ppstg);
			uint CopyTo([In] uint ciidExclude, [In] ref Guid rgiidExclude, [In, MarshalAs(UnmanagedType.Struct)] ref tagRemSNB snbExclude, [In, MarshalAs(UnmanagedType.Interface)] IStorage pstgDest);
			uint MoveElementTo([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In, MarshalAs(UnmanagedType.Interface)] IStorage pstgDest, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName, [In] uint grfFlags);
			uint Commit([In] uint grfCommitFlags);
			uint Revert();
			uint EnumElements([In] uint reserved1, [In] IntPtr reserved2, [In] uint reserved3, [Out, MarshalAs(UnmanagedType.IUnknown)] out IntPtr ppenum);
			uint DestroyElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsName);
			uint RenameElement([In, MarshalAs(UnmanagedType.BStr)] string pwcsOldName, [In, MarshalAs(UnmanagedType.BStr)] string pwcsNewName);
			uint SetElementTimes([In, MarshalAs(UnmanagedType.BStr)] string pwcsName, [In] ref _FILETIME pctime, [In] ref _FILETIME patime, [In] ref _FILETIME pmtime);
			uint SetClass([In] ref Guid clsid);
			uint SetStateBits([In] uint grfStateBits, [In] uint grfMask);
			uint Stat([Out, MarshalAs(UnmanagedType.Struct)] System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, [In] uint grfStatFlag);
		}

		[ComImport, Guid("0000000c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IStream3
		{
			uint Read([Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, [Out] out uint pcbRead);
			uint Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, out uint pcbWritten);
			void Seek([In] ulong dlibMove, int dwOrigin, [Out] out ulong plibNewPosition);
			uint SetSize([In] ulong dlibMove);
			void CopyTo(IStream3 pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);
			void Commit(int grfCommitFlags);
			void Revert();
			void LockRegion(long libOffset, long cb, int dwLockType);
			void UnlockRegion(long libOffset, long cb, int dwLockType);
			void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);
			void Clone(out IStream3 ppstm);
		}

		#endregion COM Interfaces
		#region Static Fields
		[Flags]
		public enum StorageModes : uint
		{
			//Access
			STGM_READ = 0x00000000,
			STGM_WRITE = 0x00000001,
			STGM_READWRITE = 0x00000002,
			//Sharing
			STGM_SHARE_DENY_NONE = 0x00000040,
			STGM_SHARE_DENY_READ = 0x00000030,
			STGM_SHARE_DENY_WRITE = 0x00000020,
			STGM_SHARE_EXCLUSIVE = 0x00000010,
			STGM_PRIORITY = 0x00040000,
			//Creation 
			STGM_CREATE = 0x00001000,
			STGM_CONVERT = 0x00020000,
			STGM_FAILIFTHERE = 0x00000000,
			//Transactioning 
			STGM_DIRECT = 0x00000000,
			STGM_TRANSACTED = 0x00010000,
			//Transactioning Performance
			STGM_NOSCRATCH = 0x00100000,
			STGM_NOSNAPSHOT = 0x00200000,
			//Direct SWMR and Simple
			STGM_SIMPLE = 0x08000000,
			STGM_DIRECT_SWMR = 0x00400000,
			//Delete On Release 
			STGM_DELETEONRELEASE = 0x04000000
		}

		#endregion Static Fields
		enum STREAM_SEEK
		{
			STREAM_SEEK_SET = 0,
			STREAM_SEEK_CUR = 1,
			STREAM_SEEK_END = 2
		}
#pragma warning restore 1591
		#endregion Common Definition
		//**************************************************************************************
		/// <summary>
		/// Guid for Structured Storage
		/// </summary>
		public static Guid StructuredStorageGuid = new Guid("0000000b-0000-0000-C000-000000000046");
		#region DllImport Definition
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="grfMode"></param>
		/// <param name="stgfmt"></param>
		/// <param name="grfAttrs"></param>
		/// <param name="pStgOptions"></param>
		/// <param name="reserved"></param>
		/// <param name="riid"></param>
		/// <param name="ppObjectOpen"></param>
		/// <returns></returns>
		[DllImport("OLE32.DLL", CharSet = CharSet.Unicode)]
		private static extern uint StgOpenStorageEx([MarshalAs(UnmanagedType.BStr)] string pwcsName,
										uint grfMode,
										uint stgfmt,
										uint grfAttrs,
										IntPtr pStgOptions,
										IntPtr reserved,
										ref Guid riid,
										[MarshalAs(UnmanagedType.Interface)] ref IStorage ppObjectOpen);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pwcsName"></param>
		/// <param name="grfMode"></param>
		/// <param name="stgfmt"></param>
		/// <param name="grfAttrs"></param>
		/// <param name="pStgOptions"></param>
		/// <param name="reserved"></param>
		/// <param name="riid"></param>
		/// <param name="ppObjectOpen"></param>
		/// <returns></returns>
		[DllImport("OLE32.DLL", CharSet = CharSet.Unicode)]
		private static extern uint StgCreateStorageEx([MarshalAs(UnmanagedType.BStr)] string pwcsName,
										uint grfMode,
										uint stgfmt,
										uint grfAttrs,
										IntPtr pStgOptions,
										IntPtr reserved,
										ref Guid riid,
										[MarshalAs(UnmanagedType.Interface)] ref IStorage ppObjectOpen);
		#endregion DllImport Definition
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод экспорта данных в файл Excel.
		/// </summary>
		/// <param name="table">Таблица, содержащая экспортируемые данные.</param>
		/// <param name="xlsFile">Файл xls, в который экспортируются данные.</param>
		public static void ExportToExcel(SimDataGridView table, string xlsFile)
		{
			FileStream fss = null;
			IStorage stg = null;
			IStream3 stream = null;
			uint len = 0;
			byte[] arr = null;
			LittleEndianWord w = new LittleEndianWord();

			//Dictionary<string, uint> strTable = new Dictionary<string,uint>();
			//uint strCount = 0;
			try
			{
				if(table.Rows.Count > ushort.MaxValue - 1)
					throw new Exception(String.Format("Количество строк должно быть меньше {0}!", ushort.MaxValue - 1));

				#region Создаем пустой xls файл.
				FileStream file = File.Create(xlsFile);
				byte[] tes = (byte[])global::Sim.Controls.Properties.Resources.Excel;
				file.Write(tes, 0, tes.Length);
				file.Flush();
				file.Close();
				#endregion Создаем пустой xls файл.

				#region Открываем хранилище для чтения и временный файл для записи
				uint hr = StgOpenStorageEx(xlsFile, (uint)(
																														StorageModes.STGM_DIRECT | StorageModes.STGM_READWRITE | StorageModes.STGM_SHARE_EXCLUSIVE),
																														0, 0, (IntPtr)0, (IntPtr)0, ref StructuredStorageGuid, ref stg);
				if(hr > 0)
					throw new Exception("Ошибка при открытии xls файла для чтения!");

				hr = stg.OpenStream("Workbook", 0, (uint)(
																													StorageModes.STGM_READ | StorageModes.STGM_DIRECT | StorageModes.STGM_SHARE_EXCLUSIVE),
																												0, out stream);
				if(hr > 0)
					throw new Exception("Ошибка при открытии Workbook потока временного xls файла!");

				fss = new FileStream(Environment.GetEnvironmentVariable("TEMP") + "\\excel_export.fe", FileMode.Create);
				#endregion Открываем хранилище для чтения и временный файл для записи

				#region Читаем и записываем до данных
				while(true)
				{
					if((hr = stream.Read(w, 2, out len)) != 0 || len == 0)
						break;
					if(w == 0x0200)
					{
						if((hr = stream.Read(w, 2, out len)) != 0 || len == 0)
							throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
						arr = new byte[w];
						if((hr = stream.Read(arr, w, out len)) != 0 || len == 0)
							throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
						break;
					}
					fss.Write(w, 0, 2);
					if((hr = stream.Read(w, 2, out len)) != 0 || len == 0)
						throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
					fss.Write(w, 0, 2);
					if(w > 0)
					{
						arr = new byte[w];
						if((hr = stream.Read(arr, w, out len)) != 0 || len == 0)
							throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
						fss.Write(arr, 0, w);
					}
				}
				#endregion Читаем и записываем до данных

				#region Записываем данные
				arr = Build_DIMENSIONS_Record(table.Rows.Count, table.Columns.Count);
				fss.Write(arr, 0, arr.Length);
				uint offsetFirstRow = 0;
				ushort rowset = 0;
				ushort row = 0;
				ushort[] cellBlocksOffsets = null;

				while(row < table.Rows.Count + 1)
				{
					if((table.Rows.Count + 1 - row) >= 32)
						rowset = 32;
					else
						rowset = (ushort)(table.Rows.Count + 1 - row);

					cellBlocksOffsets = new ushort[rowset];

					for(ushort a = 0; a < rowset; a++)
					{
						arr = Build_ROW_Record((ushort)(row + a), (ushort)table.Columns.Count);
						fss.Write(arr, 0, arr.Length);
						offsetFirstRow += (uint)arr.Length;
						if(a != 0)
							cellBlocksOffsets[0] += (ushort)arr.Length;
					}

					for(int a = 0; a < rowset; row++, a++)
					{
						for(ushort col = 0; col < table.Columns.Count; col++)
						{
							if(row == 0)
								arr = Build_LABEL_Record(row, col, table.Columns[col].HeaderText);
							else
							{
								object obj = table[col, row - 1].Value;
								if(obj == null)
									obj = table[col, row - 1].FormattedValue;
								if(Convert.IsDBNull(obj))
									continue;
								switch((table.Columns[col].ValueType ?? typeof(string)).Name)
								{
									case "Byte": arr = Build_RK_Record(row, col, (Int32)(Byte)obj);
										break;
									case "SByte": arr = Build_RK_Record(row, col, (Int32)(SByte)obj);
										break;
									case "Int16": arr = Build_RK_Record(row, col, (Int32)(Int16)obj);
										break;
									case "UInt16": arr = Build_RK_Record(row, col, (Int32)(UInt16)obj);
										break;
									case "Int32": if((arr = Build_RK_Record(row, col, (Int32)obj)) == null)
											arr = Build_NUMBER_Record(row, col, (Double)(Int32)obj);
										break;
									case "UInt32": if((UInt32)obj <= 0x1FFFFFFF)
											arr = Build_RK_Record(row, col, (Int32)(UInt32)obj);
										else
											arr = Build_NUMBER_Record(row, col, (Double)(UInt32)obj);
										break;
									case "Int64": arr = Build_NUMBER_Record(row, col, (Double)(Int64)obj);
										break;
									case "UInt64": arr = Build_NUMBER_Record(row, col, (Double)(UInt64)obj);
										break;
									case "Decimal": //double val = (Double)(Decimal)obj;//Double.Parse(((Decimal)obj).ToString());
										if((arr = Build_RK_Record(row, col, (Double)(Decimal)obj)) == null)
											arr = Build_NUMBER_Record(row, col, (Double)(Decimal)obj);
										break;
									case "Single": double val = Double.Parse(((Single)obj).ToString());
										if((arr = Build_RK_Record(row, col, val)) == null)
											arr = Build_NUMBER_Record(row, col, val);
										break;
									case "Double": if((arr = Build_RK_Record(row, col, (Double)obj)) == null)
											arr = Build_NUMBER_Record(row, col, (Double)obj);
										break;
									//case "Boolean": arr = Build_BOOLERR_Record(row, col, (Boolean)obj);
									case "Boolean": arr = Build_LABEL_Record(row, col, obj == null ? "" : ((bool)obj ? "+" : "-"));
										break;
									case "String": arr = Build_LABEL_Record(row, col, obj == null ? "" : obj.ToString());
										break;
									case "DateTime": arr = Build_NUMBER_Record(row, col, (DateTime)obj);
										break;
									//default: throw new Exception(String.Format("Тип данных {0} не поддерживается для экспорта!",
									//                             table.Columns[col].ValueType.FullName));
									default: arr = Build_LABEL_Record(row, col, obj == null ? "" : obj.ToString()); break;
								}
							}
							fss.Write(arr, 0, arr.Length);
							offsetFirstRow += (uint)arr.Length;
							if(a != 0)
							{
								if((cellBlocksOffsets[a] + arr.Length) <= ushort.MaxValue)
									cellBlocksOffsets[a] += (ushort)arr.Length;
								else
									cellBlocksOffsets[a] = 0;
							}
						}
					}
					arr = Build_DBCELL_Record(offsetFirstRow, cellBlocksOffsets);
					fss.Write(arr, 0, arr.Length);
					offsetFirstRow = 0;
				}
				#endregion Записываем данные

				#region Дописываем оставшиеся данные из потока
				while(true)
				{
					if((hr = stream.Read(w, 2, out len)) != 0 || len == 0)
						break;
					if(w == 0x000A)
					{
						fss.Write(w, 0, 2);

						if((hr = stream.Read(w, 2, out len)) != 0 || len == 0)
							throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
						fss.Write(w, 0, 2);
						break;
					}
					fss.Write(w, 0, 2);
					if((hr = stream.Read(w, 2, out len)) != 0 || len == 0)
						throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
					fss.Write(w, 0, 2);
					if(w > 0)
					{
						arr = new byte[w];
						if((hr = stream.Read(arr, w, out len)) != 0 || len == 0)
							throw new Exception(String.Format("Ошибка в записи, hr = {0}, len = {1}!", hr.ToString("X"), len));
						fss.Write(arr, 0, w);
					}
				}
				fss.Close();
				Marshal.ReleaseComObject(stream);
				stream = null;
				#endregion Дописываем оставшиеся данные из потока

				#region Записываем данные из временного файла
				fss = new FileStream(Environment.GetEnvironmentVariable("TEMP") + "\\excel_export.fe", FileMode.Open, FileAccess.Read);
				stg.CreateStream("Workbook", (uint)(
																						StorageModes.STGM_WRITE | StorageModes.STGM_DIRECT | StorageModes.STGM_SHARE_EXCLUSIVE |
																																		StorageModes.STGM_CREATE), 0, 0, out stream);
				hr = stream.SetSize((ulong)fss.Length);

				arr = new byte[32768];
				while(fss.Read(arr, 0, 32768) != 0)
					if((hr = stream.Write(arr, 32768, out len)) != 0 || len == 0)
						throw new Exception(String.Format("Ошибка при записи в выходной поток, hr = {0}, len = {1}!", hr.ToString("X"), len));
				#endregion Записываем данные из временного файла
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
			finally
			{
				if(fss != null)
					fss.Close();
				if(stream != null)
					Marshal.ReleaseComObject(stream);
				if(stg != null)
					Marshal.ReleaseComObject(stg);
				if(File.Exists(Environment.GetEnvironmentVariable("TEMP") + "\\excel_export.fe"))
					File.Delete(Environment.GetEnvironmentVariable("TEMP") + "\\excel_export.fe");
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод экспорта данных в файл CSV.
		/// </summary>
		/// <param name="view">Таблица, содержащая экспортируемые данные.</param>
		/// <param name="csvFile">Файл csv, в который экспортируются данные.</param>
		public static void ExportToCsv(SimDataGridView view, string csvFile)
		{
			StreamWriter file = null;
			try
			{
				file = new StreamWriter(csvFile, false, Encoding.Default);

				StringBuilder sb = new StringBuilder(1000);
				foreach(DataGridViewColumn col in view.Columns)
					sb.AppendFormat("\"{0}\";", col.HeaderText.Replace("\"", "\"\""));
				file.WriteLine(sb.ToString());

				foreach(DataGridViewRow row in view.Rows)
				{
					StringBuilder cmdtext = new StringBuilder(1000);
					for(int col = 0; col < view.ColumnCount; col++) //DataGridViewColumn col in view.Columns) 
					{
						if(row.Cells[col].ValueType == typeof(string))
							cmdtext.AppendFormat("\"{0}\";", row.Cells[col].Value.ToString().Replace("\"", "\"\""));
						else
							cmdtext.AppendFormat("{0};", row.Cells[col].Value ?? row.Cells[col].FormattedValue);
					}
					file.WriteLine(cmdtext.ToString());
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
			finally
			{
				if(file != null)
					file.Close();
			}
		}
		//-------------------------------------------------------------------------------------
		#region Build Record Methods
		private static byte[] Build_DIMENSIONS_Record(int rowsCount, int colsCount)
		{
			byte[] arr = new byte[18];
			arr[0] = 0x00; arr[1] = 0x02;                    // 0x0208
			arr[2] = 0x0E; arr[3] = 0x00;                    // length
			arr[4] = 0x00; arr[5] = 0x00;                    // 4 -> 
			arr[6] = 0x00; arr[7] = 0x00;                    // -> First Row Index

			uint t = (uint)rowsCount;
			arr[8] = (byte)t;                                                        //
			t = t >> 8;                                                              //
			arr[9] = (byte)t;                                                        //  4 ->
			t = t >> 8;                                                              //  -> Last Row Index +1
			arr[10] = (byte)t;                                                       //
			t = t >> 8;                                                              //
			arr[11] = (byte)t;                                                       //
			arr[12] = 0x00; arr[13] = 0x00;                    // First Column Index
			ushort r = (ushort)colsCount;
			arr[14] = GetLowByte(r); arr[15] = GetHiByte(r);           // Last Column Index +1 
			arr[16] = 0x00; arr[17] = 0x00;                    // Not Used

			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_ROW_Record(ushort rowIndex, ushort cellsCount)
		{
			byte[] arr = new byte[20];
			arr[0] = 0x08; arr[1] = 0x02;                    // 0x0208
			arr[2] = 0x10; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = 0x00; arr[7] = 0x00;                    // Index of first cell column
			arr[8] = GetLowByte(cellsCount); arr[9] = GetHiByte(cellsCount);   // Index of last cell column +1
			arr[10] = 0xFF; arr[11] = 0x00;                    // Row Height
			arr[12] = 0x00; arr[13] = 0x00;                    // Not used
			arr[14] = 0x00; arr[15] = 0x00;                    // Not used
			arr[16] = 0x00; arr[17] = 0x01;                    // 4 ->
			arr[18] = 0x0F; arr[19] = 0x00;                    // Flag
			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_DBCELL_Record(uint firstRowOffset, ushort[] cellBlocksOffsets)
		{
			byte[] arr = new byte[8 + 2 * cellBlocksOffsets.Length];
			arr[0] = 0xD7; arr[1] = 0x00;                    // 0x00D7
			ushort t = (ushort)(4 + 2 * cellBlocksOffsets.Length);
			arr[2] = GetLowByte(t); arr[3] = GetHiByte(t);             // length
			arr[4] = (byte)firstRowOffset;                                           // 4 -> 
			firstRowOffset = firstRowOffset >> 8;                                    //
			arr[5] = (byte)firstRowOffset;                                           //
			firstRowOffset = firstRowOffset >> 8;                                    //
			arr[6] = (byte)firstRowOffset;                                           //
			firstRowOffset = firstRowOffset >> 8;                                    //
			arr[7] = (byte)firstRowOffset;                                           // -> Offset to first row in Row Block

			for(int a = 0; a < cellBlocksOffsets.Length; a++)
			{
				arr[8 + a * 2] = GetLowByte(cellBlocksOffsets[a]);
				arr[9 + a * 2] = GetHiByte(cellBlocksOffsets[a]);
			}
			return arr;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Создает запись RK для целочисленных значений.
		/// </summary>
		/// <param name="rowIndex">Индекс строки.</param>
		/// <param name="colIndex">Индекс столбца.</param>
		/// <param name="value">Значение.</param>
		/// <returns>Возвращает null, если число не может быть записано в RK.</returns>
		private static byte[] Build_RK_Record(ushort rowIndex, ushort colIndex, int value)
		{
			if(value >= 0 && value > 0x1FFFFFFF)
				return null;
			if(value < 0 && (~value + 1) > 0x20000000)
				return null;

			byte[] arr = new byte[14];
			arr[0] = 0x7E; arr[1] = 0x02;                    // 0x027E
			arr[2] = 0x0A; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x0F; arr[9] = 0x00;                    // Index of XF record

			uint t = ((uint)value << 2) | 0x00000002;
			if(value >= 0)
				t = t & 0x7FFFFFFF;
			else
				t = t | 0x80000000;

			arr[10] = (byte)t;
			t = t >> 8;
			arr[11] = (byte)t;
			t = t >> 8;
			arr[12] = (byte)t;
			t = t >> 8;
			arr[13] = (byte)t;
			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_RK_Record(ushort rowIndex, ushort colIndex, double value)
		{
			ulong num = (ulong)BitConverter.DoubleToInt64Bits(value);
			if(num != (num & 0xFFFFFFFC00000000))
				return null;

			byte[] arr = new byte[14];
			arr[0] = 0x7E; arr[1] = 0x02;                    // 0x027E
			arr[2] = 0x0A; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x0F; arr[9] = 0x00;                    // Index of XF record

			uint b30 = (uint)(num >> 32);
			b30 = b30 & 0xFFFFFFFC;
			byte[] dbits = BitConverter.GetBytes(b30);

			for(int a = 0; a < 4; a++)
				arr[10 + a] = dbits[a];

			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_NUMBER_Record(ushort rowIndex, ushort colIndex, double value)
		{
			byte[] arr = new byte[18];
			arr[0] = 0x03; arr[1] = 0x02;                    // 0x0203
			arr[2] = 0x0E; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x0F; arr[9] = 0x00;                    // Index of XF record

			byte[] dbits = BitConverter.GetBytes(value);

			for(int a = 0; a < dbits.Length; a++)
				arr[10 + a] = dbits[a];

			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_NUMBER_Record(ushort rowIndex, ushort colIndex, DateTime value)
		{
			byte[] arr = new byte[18];
			arr[0] = 0x03; arr[1] = 0x02;                    // 0x0203
			arr[2] = 0x0E; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x3E; arr[9] = 0x00;                    // Index of XF record

			TimeSpan x = value.Subtract(new DateTime(1899, 12, 30, 0, 0, 0, 0));
			byte[] dbits = BitConverter.GetBytes(x.TotalDays);//(value.Ticks)/100000.0);

			for(int a = 0; a < dbits.Length; a++)
				arr[10 + a] = dbits[a];

			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_BOOLERR_Record(ushort rowIndex, ushort colIndex, bool value)
		{
			byte[] arr = new byte[12];
			arr[0] = 0x05; arr[1] = 0x02;                    // 0x0205
			arr[2] = 0x08; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x0F; arr[9] = 0x00;                    // Index of XF record
			arr[10] = (byte)(value ? 1 : 0); arr[11] = 0x00;

			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_LABELSST_Record(ushort rowIndex, ushort colIndex, uint strIndex)
		{
			byte[] arr = new byte[14];
			arr[0] = 0xFD; arr[1] = 0x00;                    // 0x00FD
			arr[2] = 0x0A; arr[3] = 0x00;                    // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x0F; arr[9] = 0x00;                    // Index of XF record
			arr[10] = (byte)strIndex;
			strIndex = strIndex >> 8;
			arr[11] = (byte)strIndex;
			strIndex = strIndex >> 8;
			arr[12] = (byte)strIndex;
			strIndex = strIndex >> 8;
			arr[13] = (byte)strIndex;
			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_SST_Record(uint strCount, Dictionary<string, uint> strTable)
		{
			ushort buflen = 0;
			foreach(string key in strTable.Keys)
				buflen += (ushort)key.Length;

			buflen = (ushort)(buflen * 2 + (ushort)strTable.Count * 3 + 8);

			byte[] arr = new byte[buflen + 4];
			arr[0] = 0xFC; arr[1] = 0x00;                    // 0x00FC
			arr[2] = GetLowByte(buflen); arr[3] = GetHiByte(buflen);       // length
			arr[4] = (byte)strCount;                                         // 4 ->
			strCount = strCount >> 8;                                        //
			arr[5] = (byte)strCount;                                         //
			strCount = strCount >> 8;                                        //
			arr[6] = (byte)strCount;                                         //
			strCount = strCount >> 8;                                        //
			arr[7] = (byte)strCount;                                         // -> Total number of strings in workbook
			strCount = (uint)strTable.Count;
			arr[8] = (byte)strCount;                                         // 4 ->
			strCount = strCount >> 8;                                        //
			arr[9] = (byte)strCount;                                         //
			strCount = strCount >> 8;                                        //
			arr[10] = (byte)strCount;                                        //
			strCount = strCount >> 8;                                        //
			arr[11] = (byte)strCount;                                        // -> Count of string

			int a = 0;
			foreach(string key in strTable.Keys)
			{
				buflen = (ushort)key.Length;
				arr[12 + a] = GetLowByte(buflen);
				arr[13 + a] = GetHiByte(buflen);
				arr[14 + a] = 0x01;
				byte[] buf = UnicodeEncoding.Unicode.GetBytes(key);
				for(int b = 0; b < buf.Length; b++, a++)
					arr[15 + a] = buf[b];
				a += 3;
			}
			return arr;
		}
		//-------------------------------------------------------------------------------------
		private static byte[] Build_LABEL_Record(ushort rowIndex, ushort colIndex, string str)
		{
			ushort buflen = (ushort)(str.Length * 2 + 9);

			byte[] arr = new byte[buflen + 4];
			arr[0] = 0x04; arr[1] = 0x02;                    // 0x00FC
			arr[2] = GetLowByte(buflen); arr[3] = GetHiByte(buflen);       // length
			arr[4] = GetLowByte(rowIndex); arr[5] = GetHiByte(rowIndex);     // Row Index
			arr[6] = GetLowByte(colIndex); arr[7] = GetHiByte(colIndex);     // Column Index
			arr[8] = 0x0F; arr[9] = 0x00;                    // Index of XF record

			buflen = (ushort)str.Length;
			arr[10] = GetLowByte(buflen);
			arr[11] = GetHiByte(buflen);
			arr[12] = 0x01;
			byte[] buf = UnicodeEncoding.Unicode.GetBytes(str);
			for(int a = 0; a < buf.Length; a++)
				arr[13 + a] = buf[a];
			return arr;
		}
		#endregion Build Record Methods
		//-------------------------------------------------------------------------------------
		#region Helpers Methods
		/// <summary>
		/// Возвращает младший (правый) байт.
		/// </summary>
		/// <param name="value">Значение, для которого определяется значение байта.</param>
		/// <returns></returns>
		private static byte GetLowByte(ushort value)
		{
			return (byte)value;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает старший (левый) байт.
		/// </summary>
		/// <param name="value">Значение, для которого определяется значение байта.</param>
		/// <returns></returns>
		private static byte GetHiByte(ushort value)
		{
			return (byte)(value >> 8);
		}
		//-------------------------------------------------------------------------------------
		#endregion Helpers Methods
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает наименование типа данных для OLE DB и ODBC провайдеров.
		/// </summary>
		/// <param name="t">Тип .NET Framework.</param>
		/// <returns>Строковое значение типа провайдера.</returns>
		public static string GetProviderDataTypeString(Type t)
		{
			if(t == typeof(Boolean)) return "BIT";
			if(t == typeof(Byte)) return "BYTE";
			if(t == typeof(Int16)) return "SMALLINT";
			if(t == typeof(UInt16)) return "SMALLINT";
			if(t == typeof(Int32)) return "INT";
			if(t == typeof(UInt32)) return "INT";
			if(t == typeof(Int64)) return "LONG";
			if(t == typeof(UInt64)) return "LONG";
			if(t == typeof(Decimal)) return "REAL";
			if(t == typeof(Double)) return "DOUBLE";
			if(t == typeof(String)) return "VARCHAR";
			if(t == typeof(Guid)) return "GUID";
			if(t == typeof(Byte[])) return "VARBINARY";
			if(t == typeof(Single)) return "SINGLE";
			if(t == typeof(DateTime)) return "TIMESTAMP";

			throw new ArgumentException("Отсутствует соответствие для типа {" + t.FullName + "}");
		}
	}
	#endregion << public static class DataExport >>
	//**************************************************************************************
	#region public class LittleEndianWord
	/// <summary>
	/// Класс слова, записанного по принципу LittleEndian.
	/// </summary>
	public class LittleEndianWord
	{
		private byte[] vals;
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public LittleEndianWord()
		{
			vals = new byte[2];
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region Operators
		/// <summary>
		/// Оператор явного преобразования к типу UInt32
		/// </summary>
		/// <param name="w"></param>
		/// <returns></returns>
		public static implicit operator ushort(LittleEndianWord w)
		{
			return (ushort)(((ushort)w.vals[1] << 8) + w.vals[0]);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Оператор явного преобразования к типу byte[]
		/// </summary>
		/// <param name="w"></param>
		/// <returns></returns>
		public static implicit operator byte[](LittleEndianWord w)
		{
			return w.vals;
		}
		#endregion Operators
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает строку представления объекта.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "0x" + (vals[1] > 0xF ? vals[1].ToString("X") : "0" + vals[1].ToString("X")) +
																			(vals[0] > 0xF ? vals[0].ToString("X") : "0" + vals[0].ToString("X"));
		}
	}
	#endregion public class LittleEndianWord
	//**************************************************************************************
	#region << public class SimDataGridViewCellStyle : DataGridViewCellStyle >>
	/// <summary>
	/// 
	/// </summary>
	public class SimDataGridViewCellStyle : DataGridViewCellStyle
	{
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimDataGridViewCellStyle()
		{
			base.BackColor = Color.Transparent;
		}
		//-------------------------------------------------------------------------------------

	}
	#endregion << public class SimDataGridViewCellStyle : DataGridViewCellStyle >>
	//*************************************************************************************
	#region << public class UnboundColumnPropertyDescriptor : PropertyDescriptor >>
	/// <summary>
	/// 
	/// </summary>
	public class UnboundColumnPropertyDescriptor : PropertyDescriptor
	{
		private DataGridViewColumn col = null;
#pragma warning disable 1591
		public override bool CanResetValue(object component)
		{
			return false;
		}
		//-------------------------------------------------------------------------------------
		public override Type ComponentType
		{
			get { throw new NotImplementedException(); }
		}
		//-------------------------------------------------------------------------------------
		public override object GetValue(object component)
		{
			if(col is SimDataGridViewUnboundColumn)
				return ((SimDataGridViewUnboundColumn)col).GetValue(col, component);
			return ((SimDataGridView)col.DataGridView).OnUnboundColumnCellValueNeed(col, component);
		}
		//-------------------------------------------------------------------------------------
		public override bool IsReadOnly
		{
			get { return true; }
		}
		//-------------------------------------------------------------------------------------
		public override Type PropertyType
		{
			get { throw new NotImplementedException(); }
		}
		//-------------------------------------------------------------------------------------
		public override void ResetValue(object component)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		public override void SetValue(object component, object value)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		public override bool ShouldSerializeValue(object component)
		{
			throw new NotImplementedException();
		}
		//-------------------------------------------------------------------------------------
		public override bool Equals(object obj)
		{
			if(obj == null || obj is UnboundColumnPropertyDescriptor == false)
				return false;
			return Object.Equals(col, ((UnboundColumnPropertyDescriptor)obj).col);
		}
		//-------------------------------------------------------------------------------------
		public override int GetHashCode()
		{
			return col == null ? 0 : col.GetHashCode();
		}
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор.
		/// </summary>
		public UnboundColumnPropertyDescriptor(DataGridViewColumn column)
			: base(String.IsNullOrEmpty(column.DataPropertyName) ? column.Name : column.DataPropertyName, null)
		{
			col = column;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
#pragma warning restore 1591
	}
	#endregion << public class UnboundColumnPropertyDescriptor : PropertyDescriptor >>
}
