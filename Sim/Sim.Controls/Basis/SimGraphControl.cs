using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;

using Pulsar.Reflection;

namespace Sim.Controls
{
	//**************************************************************************************
	#region public enum SimScaleValuesStyle
	/// <summary>
	/// Перечисление стилей отображения значений градуировки шкалы.
	/// </summary>
	[TypeConverter("GoldClasses.GoldTypeConverters.EnumTypeConverter")]
	public enum SimScaleValuesStyle
	{
		/// <summary>
		/// Не отображать значения.
		/// </summary>
		[EnumItemDisplayNameAttribute("Нет")]
		None,
		/// <summary>
		/// Отображать только крайние значения.
		/// </summary>
		[EnumItemDisplayNameAttribute("Крайние")]
		Outers,
		/// <summary>
		/// Отображать значения через одного.
		/// </summary>
		[EnumItemDisplayNameAttribute("Через одного")]
		EverySecond,
		/// <summary>
		/// Зарезервированно.
		/// </summary>
		[EnumItemDisplayNameAttribute("Опримально")]
		Optimal,
		/// <summary>
		/// Отображать все значения.
		/// </summary>
		[EnumItemDisplayNameAttribute("Все")]
		All
	}
	#endregion public enum ScaleValuesStyle
	//**************************************************************************************
	#region public enum SimGraphType
	/// <summary>
	/// Перечисление типов графиков.
	/// </summary>
	[TypeConverter("GoldClasses.GoldTypeConverters.EnumTypeConverter")]
	public enum SimGraphType
	{
		/// <summary>
		/// Простой график.
		/// </summary>
		[EnumItemDisplayNameAttribute("График")]
		Graph,
		/// <summary>
		/// Точечная гистограмма.
		/// </summary>
		[EnumItemDisplayNameAttribute("Точечная гистограмма")]
		PointHistogram,
		/// <summary>
		/// Столбчатая гистограмма.
		/// </summary>
		[EnumItemDisplayNameAttribute("Столбчатая гистограмма")]
		BarHistogram
	}
	#endregion public enum SimGraphType
	//**************************************************************************************
	/// <summary>
	/// Класс контрола отображения графиков.
	/// </summary>
	[DefaultEvent("CrossClick")]
	public partial class SimGraphControl : UserControl
	{
		private Color axesColor = Color.Black;
		private string axisXName;
		private string axisYName;
		private string axisXScaleName;
		private string axisYScaleName;
		private Color axesNamesColor = Color.Black;
		private Font axesNamesFont;
		private Font valuesFont;
		private Color valuesColor = Color.Black;
		private bool mergeNames = false;
		private Point graphOffset = new Point();
		private bool showGrid = true;
		private Color gridColor = Color.LightGray;
		private DashStyle gridDashStyle = DashStyle.Solid;
		private SimScaleValuesStyle scaleStyle = SimScaleValuesStyle.All;
		private bool showGraphHistory = true;
		private bool showValuesPoint = true;
		private decimal fixedScaleXUnit = 0m;
		private bool cross = false;
		private Color crossColor = Color.DarkGray;
		
		private Bitmap bmp = null;
		private ExRectangle workView = new ExRectangle(); 
		private decimal xScale = 0, yScale = 0;
		private decimal dnx, dny;
		decimal minX = decimal.MaxValue;
		decimal maxX = decimal.MinValue;
		decimal minY = decimal.MaxValue;
		decimal maxY = decimal.MinValue;
		StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);
		private DateTime xStartDate = DateTime.MinValue;
		private int xStartDateStep = 1;
		
		//private SimGraphCollection list = new SimGraphCollection();
		private List<SimGraph> list = new List<SimGraph>();
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		#region Overrided
		/// <summary>
		/// Определяет цвет фона.
		/// </summary>
		[Description("Определяет цвет фона.")]
		[Category("Appearance")]
		[DefaultValue(typeof(Color),"White")]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет минимальный размер контрола
		/// </summary>
		[DefaultValue(typeof(Size),"115, 115")]
		public new Size MinimumSize
		{
			get { return base.MinimumSize; }
			set 
			{
				if(value.Height < 115 || value.Width < 115)
					return;
				base.MinimumSize = value;
			}
		}
		#endregion Overrided
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region Hided
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override RightToLeft RightToLeft
		{
			get { return base.RightToLeft; }
			set { base.RightToLeft = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override bool AllowDrop
		{
			get { return base.AllowDrop; }
			set { base.AllowDrop = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override AutoValidate AutoValidate
		{
			get { return base.AutoValidate; }
			set { base.AutoValidate = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override ContextMenuStrip ContextMenuStrip
		{
			get { return base.ContextMenuStrip; }
			set { base.ContextMenuStrip = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public new ImeMode ImeMode
		{
			get { return base.ImeMode; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public new bool TabStop
		{
			get { return base.TabStop;}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public new int TabIndex
		{
			get { return base.TabIndex; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public new bool CausesValidation 
		{
			get { return base.CausesValidation; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get { return base.AutoSize; }
			set { base.AutoSize = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Disabled.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable( EditorBrowsableState.Never)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public new AutoSizeMode AutoSizeMode
		{
			get {return base.AutoSizeMode; }
		}
		#endregion Hided
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region Axes
		/// <summary>
		/// Определяет цвет осей графика.
		/// </summary>
		[Description("Определяет цвет осей графика.")]
		[Category("Axes")]
		[DefaultValue(typeof(Color), "Black")]
		public Color AxesColor
		{      
			get { return axesColor; }
			set { axesColor = value; Invalidate();}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет наименование оси Х.
		/// </summary>
		[Description("Определяет наименование оси Х.")]
		[Category("Axes")]
		public string AxisXName
		{
			get { return axisXName; }
			set { axisXName = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет наименование оси Y.
		/// </summary>
		[Description("Определяет наименование оси Y.")]
		[Category("Axes")]
		public string AxisYName
		{
			get { return axisYName; }
			set { axisYName = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет наименование деления оси Х.
		/// </summary>
		[Description("Определяет наименование деления оси Х.")]
		[Category("Axes")]
		public string AxisXScaleName
		{
			get { return axisXScaleName; }
			set { axisXScaleName = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет наименование деления оси Y.
		/// </summary>
		[Description("Определяет наименование деления оси Y.")]
		[Category("Axes")]
		public string AxisYScaleName
		{
			get { return axisYScaleName; }
			set { axisYScaleName = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет наименований осей и наименований делений осей.
		/// </summary>
		[Description("Определяет цвет наименований осей и наименований делений осей.")]
		[Category("Axes")]
		[DefaultValue(typeof(Color), "Black")]
		public Color AxesNamesColor
		{
			get { return axesNamesColor; }
			set { axesNamesColor = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет шрифт наименований осей и наименований делений осей.
		/// </summary>
		[Description("Определяет шрифт наименований осей и наименований делений осей.")]
		[Category("Axes")]
		public Font AxesNamesFont
		{
			get { return axesNamesFont; }
			set { axesNamesFont = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет шрифт градации осей и значений.
		/// </summary>
		[Description("Определяет шрифт градации осей и значений.")]
		[Category("Axes")]
		public Font ValuesFont
		{
			get { return valuesFont; }
			set { valuesFont = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет градации осей и значений.
		/// </summary>
		[Description("Определяет цвет градации осей и значений.")]
		[Category("Axes")]
		[DefaultValue(typeof(Color), "Black")]
		public Color ValuesColor
		{
			get { return valuesColor; }
			set { valuesColor = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет смещение начала графика относительно центра координат..
		/// </summary>
		[Description("Определяет смещение начала графика относительно центра координат.")]
		[Category("Axes")]
		public Point GraphOffset
		{
			get { return graphOffset; }
			set { graphOffset = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет отображение координатной сетки.
		/// </summary>
		[Description("Определяет отображение координатной сетки.")]
		[Category("Axes")]
		[DefaultValue(true)]
		public bool ShowGrid
		{
			get { return showGrid; }
			set { showGrid = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет координатной сетки.
		/// </summary>
		[Description("Определяет цвет координатной сетки.")]
		[Category("Axes")]
		[DefaultValue(typeof(Color), "LightGray")]
		public Color GridColor
		{
			get { return gridColor; }
			set { gridColor = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет стиль линий координатной сетки.
		/// </summary>
		[Description("Определяет стиль линий координатной сетки.")]
		[Category("Axes")]
		[DefaultValue(typeof(DashStyle), "Solid")]
		public DashStyle GridDashStyle
		{
			get { return gridDashStyle; }
			set { gridDashStyle = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет стиль отображения значений градуировки шкалы.
		/// </summary>
		[Description("Определяет стиль отображения значений градуировки шкалы.")]
		[Category("Axes")]
		[DefaultValue(typeof(SimScaleValuesStyle), "All")]
		public SimScaleValuesStyle ScaleValuesStyle
		{
			get { return scaleStyle; }
			set { scaleStyle = value;Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет фиксированный размер шага масштаба по оси X.
		/// </summary>
		[Description("Определяет фиксированный размер шага масштаба по оси X.")]
		[Category("Axes")]
		[DefaultValue(0)]
		public decimal FixedScaleXUnit
		{
			get { return fixedScaleXUnit; }
			set { fixedScaleXUnit = value; }
		}

		#endregion Axes
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region Behavior
		/// <summary>
		/// Определяет необходимость объединения наименований осей и делений.
		/// </summary>
		[Description("Определяет необходимость объединения наименований осей и делений.")]
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool MergeNames
		{
			get { return mergeNames; }
			set { mergeNames = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет необходимость отображения истории графиков.
		/// </summary>
		[Description("Определяет необходимость отображения истории графиков.")]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool ShowGraphHistory
		{
			get { return showGraphHistory; }
			set { showGraphHistory = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет необходимость отображения точек значений.
		/// </summary>
		[Description("Определяет необходимость отображения точек значений.")]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool ShowValuesPoint
		{
			get { return showValuesPoint; }
			set { showValuesPoint = value; Invalidate(); }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет необходимость отображения курсорного перекрестия.
		/// </summary>
		[Description("Определяет необходимость отображения курсорного перекрестия.")]
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool ShowCross
		{
			get { return cross; }
			set { cross = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет цвет курсорного перекрестия.
		/// </summary>
		[Description("Определяет цвет курсорного перекрестия.")]
		[Category("Behavior")]
		[DefaultValue(typeof(Color), "DarkGray")]
		public Color CrossColor
		{
			get { return crossColor; }
			set { crossColor = value; }
		}
		#endregion Behavior
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region Data
		/// <summary>
		/// Список графиков, отображаемых контролом.
		/// </summary>
		[Category("Data")]
		[Description("Список графиков, отображаемых контролом.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<SimGraph> Graphs
		{
			get { return list; }
			//set { list = value; }
		}
		#endregion Data
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Начальная дата оси х.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
		public DateTime XStartDate
		{
			get { return xStartDate; }
			set { xStartDate = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Шаг приращения даты (1 шаг = XStartDateStep дней).
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int XStartDateStep
		{
			get { return xStartDateStep; }
			set { xStartDateStep = value; }
		}
		#endregion << Properties >>
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		#region << Events >>
		#region << public event SimGraphControlCrossClick CrossClick >>
		/// <summary>
		/// Делегат события CrossClick.
		/// </summary>
		/// <param name="sender">Объект, генерирующий событие.</param>
		/// <param name="x">Координата по оси X.</param>
		/// <param name="y">Координата по оси Y.</param>
		public delegate void SimGraphControlCrossClick(object sender, decimal x, decimal y);
		/// <summary>
		/// Событие, возникающее при включеном прицеле и нажатии левой клавиши мыши.
		/// </summary>
		[Description("Событие, возникающее при включеном прицеле и нажатии левой клавиши мыши.")]
		[Category("Own events")]
		public event SimGraphControlCrossClick CrossClick;
		/// <summary>
		/// Вызывает событие CrossClick.
		/// </summary>
		/// <param name="x">Координата по оси X.</param>
		/// <param name="y">Координата по оси Y.</param>
		protected void OnCrossClick(decimal x, decimal y)
		{
			if(CrossClick != null)
				CrossClick(this, x, y);
		}
		#endregion << public event SimGraphControlCrossClick CrossClick >>
		
		#endregion << Events >>
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimGraphControl()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ContainerControl, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			axesNamesFont = base.Font;
			valuesFont = base.Font;
			base.TabStop = false;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Прорисовочка.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				Graphics g;
				if(cross)
				{
					bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
					g = Graphics.FromImage(bmp);
				}
				else
					g = e.Graphics;
				g.InterpolationMode = InterpolationMode.HighQualityBilinear;
				g.SmoothingMode =  SmoothingMode.HighQuality;
				//if(!cross)
				// base.OnPaint(e);
				g.Clear(BackColor);

				int xOffset = 0, yOffset = 0;
				minX = decimal.MaxValue;
				maxX = decimal.MinValue;
				minY = decimal.MaxValue;
				maxY = decimal.MinValue;

				//int topY = 15; // !!!!!!!!!! История здесь должна увеличить !!!!!!!!!!!!!!!!!

				#region *** Определение сдвигов осей ***
				xOffset += (int)axesNamesFont.GetHeight(g)+1;
				xOffset += (int)valuesFont.GetHeight(g)+1;
				yOffset = xOffset;
				xOffset += 1;
				#endregion *** Определение сдвигов осей ***

				ExRectangle r = new ExRectangle(this.ClientRectangle);
				r.Left += xOffset;
				if(mergeNames)
					r.Y += 15;
				else
					r.Y += ((int)this.ValuesFont.GetHeight(g) + 4);
				r.Right -= 15;  // 15 -> отступ справа
				r.Bottom -= yOffset; //  -> отступ снизу

				#region *** Оси ***
				g.SmoothingMode =  SmoothingMode.Default;
				using(Pen p = new Pen(this.AxesColor, 2f))
				{
					g.DrawLine(p, r.Left, r.Top + 3, r.Left, r.Bottom); // +3 для стрелки
					g.DrawLine(p, r.Left, r.Bottom, r.Right - 3, r.Bottom); // -3 для стрелки
				}
				g.SmoothingMode =  SmoothingMode.HighQuality;
				using(SolidBrush b = new SolidBrush(this.AxesColor))
				{
					Point[] arr = new Point[] { new Point(r.X, r.Y),
																																new Point(r.X-5, r.Y + 15),
																																new Point(r.X+5, r.Y + 15)
																														};
					g.FillPolygon(b, arr);
					arr = new Point[] { new Point(r.Right, r.Bottom),
																								new Point(r.Right - 15, r.Bottom - 5),
																								new Point(r.Right - 15, r.Bottom + 5)
																						};
					g.FillPolygon(b, arr);
				}
				#endregion *** Оси ***

				r.Top += 18;
				r.Right -= 18;
				r.Left += graphOffset.X;
				r.Bottom -= graphOffset.Y;

				#region *** Имена осей ***
				using(SolidBrush b = new SolidBrush(this.axesNamesColor))
				{
					PointF pos = new PointF();
					SizeF sName;
					if(mergeNames)
					{
						sName = g.MeasureString(axisXName + ", " + axisXScaleName, this.axesNamesFont);
						pos.X = (this.ClientRectangle.Width - sName.Width)/2f;
						pos.Y = this.ClientRectangle.Height - sName.Height;
						g.DrawString(axisXName + ", " + axisXScaleName, this.axesNamesFont, b, pos);

						sName = g.MeasureString(axisYName + ", " + axisYScaleName, this.axesNamesFont, 0, sf);
						pos.X = 0f;
						pos.Y = (this.ClientRectangle.Height - sName.Height)/2f;
						g.DrawString(axisYName + ", " + axisYScaleName, this.axesNamesFont, b, pos, sf);
					}
					else
					{
						sName = g.MeasureString(axisXName, this.axesNamesFont);
						pos.X = (this.ClientRectangle.Width - sName.Width)/2f;
						pos.Y = this.ClientRectangle.Height - sName.Height;
						g.DrawString(axisXName, this.axesNamesFont, b, pos);

						sName = g.MeasureString(axisYName, this.axesNamesFont, 0, sf);
						pos.X = 0f;
						pos.Y = (this.ClientRectangle.Height - sName.Height)/2f;
						g.DrawString(axisYName, this.AxesNamesFont, b, pos, sf);

						//using(SolidBrush b2 = new SolidBrush(this.valuesColor))
						{
							sName = g.MeasureString(axisXScaleName, this.valuesFont);
							if(sName.Width/2f > 15)
								pos.X = this.ClientRectangle.Width -3 - sName.Width;
							else
								pos.X = this.ClientRectangle.Width - 15 - sName.Width/2f;
							pos.Y = this.ClientRectangle.Height - yOffset - sName.Height - 3;
							g.DrawString(axisXScaleName, this.valuesFont, b, pos);

							sName = g.MeasureString(axisYScaleName, this.valuesFont);
							if(sName.Width/2f > xOffset)
								pos.X = 3;
							else
								pos.X = xOffset - sName.Width/2f;
							pos.Y = 2;
							g.DrawString(axisYScaleName, this.valuesFont, b, pos);

						}
					}
				}
				#endregion *** Имена осей ***

				#region *** Определение масштаба ***
				foreach(SimGraph graph in Graphs)
					foreach(SimGraphPoint gp in graph.PointList.list)
					{
						if((decimal)gp.X < minX)
							minX = (decimal)gp.X;
						if((decimal)gp.X > maxX)
							maxX = (decimal)gp.X;

						if((decimal)gp.Y < minY)
							minY = (decimal)gp.Y;
						if((decimal)gp.Y > maxY)
							maxY = (decimal)gp.Y;
					}
				if(minX == decimal.MaxValue || maxX == decimal.MinValue || minY == decimal.MaxValue || maxY == decimal.MinValue)
					return;
				if(minX == maxX)
				{
					maxX = maxX *2;
					minX = minX /2;
				} 
				if(minY == maxY)
				{
					maxY = maxY *2;
					minY = minY / 2;
				} 
					
				#region *** Масштаб по X ***
				dnx = (maxX - minX)/10; // 10 - начальное количество делений
				decimal v = 1;
				if(fixedScaleXUnit == 0)
				{
					if(dnx == 0)
						dnx = 1;
					
					if(Math.Abs(dnx) >= 1)
						while(Math.Abs(dnx) / v > 10)
							v*= 10m;
					else
						while(Math.Abs(dnx) / v < 1)
							v/= 10m;
					dnx /= v;

					if(dnx >=  1 && dnx < 2)
						dnx = dnx >= 1.5m ? 2 : 1;
					else if(dnx >=  2 && dnx < 5)
						dnx = dnx >= 2.5m ? 5 : 2;
					else if(dnx >=  5 && dnx < 10)
						dnx = dnx >= 7.5m ? 10 : 5;
					dnx *= v;
				}
				else
					dnx = fixedScaleXUnit;
				decimal miX = minX, maX = maxX;

				if(Math.Abs(miX) < dnx)
					miX = (Math.Sign(miX) < 0 ? -1 : 0);
				else if(miX - (int)(miX/dnx)*dnx != 0)
					miX = (int)(miX/dnx)*dnx - (Math.Sign(miX) < 0 ? dnx : 0);
				if(maX - (int)(maX/dnx)*dnx != 0)
					maX = (int)(maX/dnx)*dnx + (Math.Sign(maX) > 0 ? dnx : 0);

				for(xScale = r.Width/(maX - miX)*dnx;xScale < 15;)
				{
					if(dnx/v == 1) dnx = 2*v;
					else if(dnx/v == 2) dnx = 5*v;
					else if(dnx/v == 5) dnx = 10*v;
					else if(dnx/v == 10) dnx = 20*v;
					else if(dnx/v == 20) dnx = 50*v;
					else dnx = (dnx/v *2) * v;

					if(Math.Abs(miX) < dnx)
						miX = (Math.Sign(miX) < 0 ? -1 : 0);
					else if(miX - (int)(miX/dnx)*dnx != 0)
						miX = (int)(miX/dnx)*dnx - (Math.Sign(miX) < 0 ? dnx : 0);
					if(maX - (int)(maX/dnx)*dnx != 0)
						maX = (int)(maX/dnx)*dnx + (Math.Sign(maX) > 0 ? dnx : 0);

					xScale = r.Width/(maX - miX)*dnx;
				}
				minX = miX;
				maxX = maX;
				#endregion *** Масштаб по X ***
				#region *** Масштаб по Y ***
				dny = (maxY - minY)/10; // 10 - начальное количество делений
				if(dny == 0)
					dny = 1;
				v = 1;

				if(Math.Abs(dny) >= 1)
					while(Math.Abs(dny) / v > 10)
						v*= 10;
				else
					while(Math.Abs(dny) / v < 1)
						v/= 10;
				dny /= v;

				if(dny >=  1 && dny < 2)
					dny = dny >= 1.5m ? 2 : 1;
				else if(dny >=  2 && dny < 5)
					dny = dny >= 2.5m ? 5 : 2;
				else if(dny >=  5 && dny < 10)
					dny = dny >= 7.5m ? 10 : 5;
				dny *= v;

				decimal miY = minY, maY = maxY;

				if(Math.Abs(miY) < dny)
					miY = (Math.Sign(miY) < 0 ? -1 : 0);
				else if(miY - (int)(miY/dny)*dny != 0)
					miY = (int)(miY/dny)*dny - (Math.Sign(miY) < 0 ? dny : 0);
				if(maY - (int)(maY/dny)*dny != 0)
					maY = (int)(maY/dny)*dny + (Math.Sign(maY) > 0 ? dny : 0);

				if((maY - miY)*dny != 0)
					for(yScale = r.Height/(maY - miY)*dny;yScale < 15;)
					{
						if(dny/v == 1) dny = 2*v;
						else if(dny/v == 2) dny = 5*v;
						else if(dny/v == 5) dny = 10*v;
						else if(dny/v == 10) dny = 20*v;
						else if(dny/v == 20) dny = 50*v;
						else dny = (dny/v *2) * v;

						if(Math.Abs(miY) < dny)
							miY = (Math.Sign(miY) < 0 ? -1 : 0);
						else if(miY - (int)(miY/dny)*dny != 0)
							miY = (int)(miY/dny)*dny - (Math.Sign(miY) < 0 ? dny : 0);
						if(maY - (int)(maY/dny)*dny != 0)
							maY = (int)(maY/dny)*dny + (Math.Sign(maY) > 0 ? dny : 0);

						yScale = r.Height/(maY - miY)*dny;
					}

				minY = miY;
				maxY = maY;
				#endregion *** Масштаб по Y ***
				#endregion *** Определение масштаба ***

				#region *** Сетка и градуировка ***
				using(Pen pgrid = new Pen(this.gridColor))
				using(Pen pscale = new Pen(this.axesColor))
				using(Brush p = new SolidBrush(this.valuesColor))
				{
					pgrid.DashStyle = this.gridDashStyle;
					g.SmoothingMode =  SmoothingMode.Default;
					workView = r;
					string s = "";
					for(int a = 0;a <= (maxX - minX)/dnx;a++)
					{
						int px = (int)(r.Left + xScale * a); //(r.Width - graphOffset.X - 15)/(int)(maxX - minX)
						if(showGrid && !(a == 0 && graphOffset.X == 0))
							g.DrawLine(pgrid, px, r.Top, px, r.Bottom + graphOffset.X);
						g.DrawLine(pscale, px, r.Bottom + graphOffset.X - 3, px, r.Bottom + graphOffset.X + 2);
						if(scaleStyle == SimScaleValuesStyle.None)
							continue;
						if(scaleStyle == SimScaleValuesStyle.EverySecond && a%2 != 0)
							continue;
						if(scaleStyle == SimScaleValuesStyle.Outers && !(a == 0 || a == (maxX - minX)/dnx))
							continue;
						s = Decimal.Floor(minX + a*dnx) == (minX + a*dnx) ?  Decimal.Floor(minX + a*dnx).ToString() :
																																																																	(minX + a*dnx).ToString();
						SizeF ss = g.MeasureString(s, this.valuesFont);
						g.DrawString(s, this.valuesFont, p, px - ss.Width/2, r.Bottom + graphOffset.X + 3);
					}
					//workMinX = minX;
					//Decimal.TryParse(s, out workMaxX); 
					for(int a = 0;a <= (maxY - minY)/dny;a++)
					{
						int py = (int)(r.Bottom - yScale * a);
						if(showGrid && !(a == 0 && graphOffset.Y == 0))
							g.DrawLine(pgrid, r.Left - graphOffset.Y, py, r.Right, py);
						g.DrawLine(pscale, r.Left - graphOffset.Y - 3, py, r.Left - graphOffset.Y  + 2, py);
						if(scaleStyle == SimScaleValuesStyle.None)
							continue;
						if(scaleStyle == SimScaleValuesStyle.EverySecond && a%2 != 0)
							continue;
						if(scaleStyle == SimScaleValuesStyle.Outers && !(a == 0 || a == (maxY - minY)/dny))
							continue;
						s = Decimal.Floor(minY + a*dny) == (minY + a*dny) ?  Decimal.Floor(minY + a*dny).ToString() :
																																																																	(minY + a*dny).ToString();
						SizeF ss = g.MeasureString(s, this.valuesFont, 0, sf);
						g.DrawString(s, this.valuesFont, p, r.Left - graphOffset.X - ss.Width- 5, py - ss.Height/2, sf);
					}
					//workMinY = minY;
					//Decimal.TryParse(s, out workMaxY); 
				}
				#endregion *** Сетка и градуировка ***

				#region *** Рисование графиков ***
				g.SmoothingMode =  SmoothingMode.HighQuality;
				float hist = this.ClientRectangle.Width - 3;
				foreach(SimGraph graph in Graphs)
				{
					Point pos1 = Point.Empty;
					Point pos2 = Point.Empty;
					using(Pen p = new Pen(graph.GraphColor))
						foreach(SimGraphPoint gp in graph.PointList.list)
						{
							pos2.X = (int)(r.Left + Math.Round(((decimal)gp.X-minX)/dnx * xScale));
							pos2.Y = (int)(r.Bottom - Math.Round(((decimal)gp.Y-minY)/dny * yScale));
							if(!pos1.IsEmpty)
								g.DrawLine(p, pos1, pos2);
							if(showValuesPoint && gp.Show)
								using(Brush b = new SolidBrush(graph.GraphColor))
									g.FillEllipse(b, pos2.X-2, pos2.Y-2, 4, 4);
							pos1 = pos2;
						}
					if(showGraphHistory)
					{
						SizeF graphName = g.MeasureString(graph.Name, valuesFont);
						hist -= (26f + graphName.Width);
						using(Pen p = new Pen(graph.GraphColor))
						{
							p.Width = 2f;
							g.DrawLine(p, hist, graphName.Height/2f, hist + 20, graphName.Height/2f);
						}
						using(Brush b = new SolidBrush(axesNamesColor))
							g.DrawString(graph.Name, valuesFont, b, hist + 23f, 0);
					}
				}
				#endregion *** Рисование графиков ***  /**/
				
				if(cross)
					e.Graphics.DrawImageUnscaledAndClipped(bmp, this.ClientRectangle);
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Обработчик контекстного меню
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuStrip1_ItemClicked(object sender, EventArgs e)
		{
			ToolStripMenuItem i = (ToolStripMenuItem)sender;
			switch(i.Name)
			{
				case "toolStripMenuItemShowGrid" : this.ShowGrid = i.Checked;
																																							break;
				case "toolStripMenuItemLabAll"   : this.ScaleValuesStyle = SimScaleValuesStyle.All;
																																							toolStripMenuItemLabAll.Checked = true;
																																							toolStripMenuItemLabEverySecond.Checked = false;
																																							toolStripMenuItemLabOuters.Checked = false;
																																							toolStripMenuItemLabNone.Checked = false;
																																							break;
				case "toolStripMenuItemLabEverySecond" : this.ScaleValuesStyle = SimScaleValuesStyle.EverySecond;
																																													toolStripMenuItemLabAll.Checked = false;
																																													toolStripMenuItemLabEverySecond.Checked = true;
																																													toolStripMenuItemLabOuters.Checked = false;
																																													toolStripMenuItemLabNone.Checked = false;
																																													break;
				case "toolStripMenuItemLabOuters" : this.ScaleValuesStyle = SimScaleValuesStyle.Outers;
																																								toolStripMenuItemLabAll.Checked = false;
																																								toolStripMenuItemLabEverySecond.Checked = false;
																																								toolStripMenuItemLabOuters.Checked = true;
																																								toolStripMenuItemLabNone.Checked = false;
																																								break;
				case "toolStripMenuItemLabNone" : this.ScaleValuesStyle = SimScaleValuesStyle.None;
																																						toolStripMenuItemLabAll.Checked = false;
																																						toolStripMenuItemLabEverySecond.Checked = false;
																																						toolStripMenuItemLabOuters.Checked = false;
																																						toolStripMenuItemLabNone.Checked = true;
																																						break;
				case "toolStripMenuItemMergeNames" : this.MergeNames = i.Checked;
																																									break;
				case "toolStripMenuItemShowPoint" : this.ShowValuesPoint = i.Checked;
																																									break;
			}
		}
		//-------------------------------------------------------------------------------------
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			toolStripMenuItemShowGrid.Checked = this.ShowGrid;
			toolStripMenuItemLabAll.Checked = this.ScaleValuesStyle == SimScaleValuesStyle.All;
			toolStripMenuItemLabEverySecond.Checked = this.ScaleValuesStyle == SimScaleValuesStyle.EverySecond;
			toolStripMenuItemLabOuters.Checked = this.ScaleValuesStyle == SimScaleValuesStyle.Outers;
			toolStripMenuItemLabNone.Checked = this.ScaleValuesStyle == SimScaleValuesStyle.None;
			toolStripMenuItemMergeNames.Checked = this.MergeNames;
			toolStripMenuItemShowPoint.Checked = this.ShowValuesPoint;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			try
			{
				if(cross)
				{
					if(Graphs.Count == 0 || xScale == 0)
						return;
					if(bmp == null)
						return;
					Graphics g = this.CreateGraphics();
					g.DrawImageUnscaledAndClipped(bmp, this.ClientRectangle);
					Color c = Color.FromArgb(128, CrossColor);
					Color c2 = Color.FromArgb(128, BackColor);

					Point p = e.Location;
					if(e.X < workView.Left)
						p.X = workView.Left;
					else if(e.X > workView.Right)
						p.X = workView.Right;
					if(e.Y < workView.Top)
						p.Y = workView.Top;
					else if(e.Y > workView.Bottom)
						p.Y = workView.Bottom;

					using(Pen pen = new Pen(new HatchBrush(HatchStyle.DashedHorizontal, c2, c)))
						g.DrawLine(pen, p.X, workView.Top, p.X, workView.Bottom);
					using(Pen pen = new Pen(new HatchBrush(HatchStyle.DashedVertical, c2, c)))
						g.DrawLine(pen, workView.Left, p.Y, workView.Right, p.Y);
					using(SolidBrush b = new SolidBrush(crossColor))
					{
						string s = "";
						if(minX < Decimal.MaxValue)
						{
							if(XStartDate != DateTime.MinValue)
								s = XStartDate.AddDays((double)Decimal.Round(minX+((p.X - workView.Left)*dnx / xScale),1)*xStartDateStep).ToShortDateString();
							else
								s = Decimal.Round(minX+((p.X - workView.Left)*dnx / xScale), Decimals(dnx/100)).ToString();
						}  
						SizeF ss = g.MeasureString(s, this.valuesFont);
						if(p.X - ss.Width/2 < workView.Left)
							g.DrawString(s, valuesFont, b, workView.Left, workView.Top - ss.Height - 2);
						else if(p.X + ss.Width/2 > workView.Right)
							g.DrawString(s, valuesFont, b, workView.Right - ss.Width, workView.Top - ss.Height - 2);
						else
							g.DrawString(s, valuesFont, b, p.X - ss.Width/2, workView.Top - ss.Height - 2);
						
						if(minY < Decimal.MaxValue && yScale != 0)
							s = Decimal.Round(minY+((workView.Bottom - p.Y)*dny / yScale), Decimals(dny/100)).ToString();
						else
							s = ""; 
						ss = g.MeasureString(s, this.valuesFont, 0, sf);
						if(p.Y - ss.Height/2 < workView.Top)
							g.DrawString(s, valuesFont, b, workView.Right + 2, workView.Top, sf);
						else if(p.Y + ss.Height/2 > workView.Bottom)
							g.DrawString(s, valuesFont, b, workView.Right + 2, workView.Bottom - ss.Height, sf);
						else
							g.DrawString(s, valuesFont, b, workView.Right + 2, p.Y - ss.Height/2, sf);

					}
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
			base.OnMouseMove(e);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseClick(MouseEventArgs e)
		{
			try
			{
				if(Graphs.Count > 0 && cross == true && e.Clicks == 1 && e.Button == MouseButtons.Left)
				{
					Point p = e.Location;
					if(e.X < workView.Left)
						p.X = workView.Left;
					else if(e.X > workView.Right)
						p.X = workView.Right;
					if(e.Y < workView.Top)
						p.Y = workView.Top;
					else if(e.Y > workView.Bottom)
						p.Y = workView.Bottom;

					decimal x = minX+((p.X - workView.Left)*dnx / xScale);
					decimal y = minY+((workView.Bottom - p.Y)*dny / yScale);
					OnCrossClick(x,y);
				} 
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
			base.OnMouseClick(e);
		}
		//-------------------------------------------------------------------------------------
		private int Decimals(decimal d)
		{
			int a = 0;
			decimal r = d;
			while(Decimal.Truncate(r) != r)
			{
				a++;
				r = r*10;
			}
			return a;
		}
	}
	//**************************************************************************************
	#region public class SimGraph
	/// <summary>
	/// Класс представления графика.
	/// </summary>
	public class SimGraph 
	{
		private string name;
		//private List<SimGraphPoint> list = new List<SimGraphPoint>();
		private SimGraphPointCollection list = null;
		private Color color = Color.Black;
		private SimGraphType type = SimGraphType.Graph;
		private int histogramArm = 0;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Имя графика.
		/// </summary>
		[Description("Имя графика.")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает список точек графика.
		/// </summary>
		[Description("Возвращает список точек графика.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[EditorAttribute(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public SimGraphPointCollection PointList   //List<SimGraphPoint>
		{
			get { return list; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Цвет графика.
		/// </summary>
		[Description("Цвет графика.")]
		[DefaultValue(typeof(Color),"Black")]
		public Color GraphColor
		{
			get { return color; }
			set { color = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет тип графика.
		/// </summary>
		[Description("Определяет тип графика.")]
		[DefaultValue(typeof(SimGraphType), "График")]
		public SimGraphType GraphType
		{
			get { return type; }
			set 
			{ 
				type = value; 
				if(type == SimGraphType.Graph)
					this.list.RemoveSupportPoints();
				else if(type == SimGraphType.PointHistogram || type == SimGraphType.BarHistogram)
					this.list.AddHistogramPoints();
					
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет размер плеча(ширины) столбца гистограммы по умолчанию.
		/// </summary>
		[Description("Определяет размер плеча(ширины) столбца гистограммы по умолчанию.")]
		[DefaultValue(0)]
		public int HistogramArm
		{
			get { return histogramArm; }
			set 
			{
				if(value < 0)
					throw new Exception("Плечо столбчатой гистограммы должно быть больше нуля!"); 
				histogramArm = value;
				this.list.RemoveSupportPoints();
				if(type == SimGraphType.PointHistogram || type == SimGraphType.BarHistogram)
					this.list.AddHistogramPoints();
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimGraph() 
		{
			list = new SimGraphPointCollection(this);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="name">Имя графика.</param>
		public SimGraph(string name)
		{
			this.name = name;
			list = new SimGraphPointCollection(this);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="name">Имя графика.</param>
		/// <param name="color">Цвет графика.</param>
		public SimGraph(string name, Color color)
		{
			this.name = name;
			this.color = color;
			list = new SimGraphPointCollection(this);
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает строковое представление значения объекта.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "{" + name + ", " + list.ToString() + "}";
		}
	}
	#endregion public class SimGraph
	//**************************************************************************************
	#region public class SimGraphPoint
	/// <summary>
	/// Класс точки графика SimGraph.
	/// </summary>
	public class SimGraphPoint
	{
		private decimal x =0;
		private decimal y = 0;
		private bool show = true;
		private bool isSupportPoint = false;
		private int arm = -1;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Значение по оси X.
		/// </summary>
		public decimal X
		{
			get { return x; }
			set { x = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Значение по оси Y.
		/// </summary>
		public decimal Y
		{
			get { return y; }
			set { y = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет размер плеча(ширины) для столбца гистограммы.
		/// </summary>
		[DefaultValue(-1)]
		public int Arm
		{
			get { return arm; }
			set 
			{
				if(value < 0)
					throw new Exception("Плечо столбчатой гистограммы должно быть больше нуля!"); 
				arm = value; 
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, будет ли точка выделяться кружком.
		/// </summary>
		public bool Show
		{
			get { return show; }
			set { show = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Определяет, является ли точка вспомогательной (служебной).
		/// </summary>
		internal bool IsSupportPoint
		{
			get { return isSupportPoint; }
			set 
			{ 
				isSupportPoint = value; 
				if(value)
					Show = false;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimGraphPoint()
		{

		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="x">Значение по оси X.</param>
		/// <param name="y">Значение по оси Y.</param>
		public SimGraphPoint(decimal x, decimal y)
		{
			X = x;
			Y = y;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="x">Значение по оси X.</param>
		/// <param name="y">Значение по оси Y.</param>
		/// <param name="show">Определяет, будет ли точка выделяться кружком.</param>
		public SimGraphPoint(decimal x, decimal y, bool show)
		{
			X = x;
			Y = y;
			Show = show;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="show">Определяет, будет ли точка выделяться кружком.</param>
		public SimGraphPoint(bool show)
		{
			Show = show;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает строковое представление значения объекта.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			//return base.ToString();
			return "{x='" + X.ToString() + "',y='" + Y.ToString() + "'}";
		}
	}
	#endregion public struct SimGraphPoint
	//**************************************************************************************
	/// <summary>
	/// Класс коллекции точек графика.
	/// </summary>
	public class SimGraphPointCollection : ICollection<SimGraphPoint>, IList<SimGraphPoint>, IList, ICollection
	{
		internal List<SimGraphPoint> list = null;
		private SimGraph graph; 
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Возвращает или устанавливает точку графика по ее индексу.
		/// </summary>
		/// <param name="index">Индекс.</param>
		/// <returns></returns>
		public SimGraphPoint this[int index]
		{
			get
			{
				int pos = -1;
				for(int a = 0; a < list.Count; a++)
				{
					if(!list[a].IsSupportPoint)
						pos++;
					if(pos == index)
						return list[a];  
				}
				throw new Exception("Элемент с индексом " + index.ToString() + " не найден в коллекции!");
			}
			set
			{
				int pos = -1;
				for(int a = 0; a < list.Count; a++)
				{
					if(!list[a].IsSupportPoint)
						pos++;
					if(pos == index)
					{
						list[a] = value;  
						return;
					} 
				}
				throw new Exception("Элемент с индексом " + index.ToString() + " не найден в коллекции!");
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает число точек в коллекции.
		/// </summary>
		public int Count
		{
			get 
			{ 
				int points = 0;
				foreach(SimGraphPoint p in list)
					if(!p.IsSupportPoint)
						points ++;
				return points;  
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// 
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public bool IsReadOnly
		{
			get { return false; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		///// <summary>
		///// Конструктор по умолчанию.
		///// </summary>
		//public SimGraphPointCollection() 
		//{
		// list = new List<SimGraphPoint>();
		//}
		////-------------------------------------------------------------------------------------
		///// <summary>
		///// Инициализирующий конструктор.
		///// </summary>
		///// <param name="capacity">Емкость коллекции.</param>
		//public SimGraphPointCollection(int capacity)
		//{
		// list = new List<SimGraphPoint>(capacity);
		//}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Инициализирующий конструктор.
		/// </summary>
		/// <param name="graph">График, которому принадлежит коллекция.</param>
		internal SimGraphPointCollection(SimGraph graph)
		{
			list = new List<SimGraphPoint>();
			this.graph = graph;
		}
		//-------------------------------------------------------------------------------------
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		internal void AddHistogramPoints()
		{
			try
			{
				RemoveSupportPoints();

				SimGraphPoint p = null;

				if(graph.HistogramArm == 0)
				{
					for(int a = 0;a < list.Count; a+=3)
					{
						decimal val = list[a].X;
						p = new SimGraphPoint();
						p.IsSupportPoint = true;
						p.X = val;
						p.Y = 0;
						list.Insert(a, p);
						list.Insert(a+2, p);
					}
				}
				else
				{
					for(int a = 0;a < list.Count; a++)
					{
						decimal x1 = list[a].X;
						decimal x2 = (a == list.Count-1) ? Decimal.MaxValue : list[a+1].X;
						int arm = list[a].Arm;

						if(a == 0) 
						{
							p = new SimGraphPoint();
							p.IsSupportPoint = true;
							p.X = x1;
							p.Y = 0;
							list.Insert(0, p);
							a++;
						}
						p = new SimGraphPoint();
						p.IsSupportPoint = true;
						p.X = x1 + (arm == -1 ? graph.HistogramArm : arm);
						p.Y = list[a].Y;
						list.Insert(++a, p);
						if(arm > -1 ? (Math.Abs(x1-x2) != arm) : (Math.Abs(x1-x2) != graph.HistogramArm)) 
						{
							p = new SimGraphPoint();
							p.IsSupportPoint = true;
							p.X = x1 + (arm == -1 ? graph.HistogramArm : arm);
							p.Y = 0;
							list.Insert(++a, p);
							if(x2 != Decimal.MaxValue)
							{
								p = new SimGraphPoint();
								p.IsSupportPoint = true;
								p.X = x2;
								p.Y = 0;
								list.Insert(++a, p);
							}
						}
					}
				}
			}
			catch(Exception Err)
			{
				ErrorBox.Show(Err);
			}
		}
		//-------------------------------------------------------------------------------------
		internal void RemoveSupportPoints()
		{
			for(int a = 0;a < list.Count;a++)
				if(list[a].IsSupportPoint)
				{
					list.RemoveAt(a);
					a--;
				}
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет точку графика.
		/// </summary>
		/// <param name="item">Добавляемая точка.</param>
		public void Add(SimGraphPoint item)
		{
			SimGraphPoint p = null;
			if(graph.GraphType == SimGraphType.PointHistogram)
			{
				p = new SimGraphPoint();
				p.IsSupportPoint = true;
				p.X = item.X;
				p.Y = 0;
				list.Add(p);
			}
			else if(graph.GraphType == SimGraphType.BarHistogram && list.Count == 0)
			{
				p = new SimGraphPoint();
				p.IsSupportPoint = true;
				p.X = item.X;
				p.Y = 0;
				list.Add(p);
			}
				
			list.Add(item);
			
			if(graph.GraphType == SimGraphType.PointHistogram)
			{
				p = new SimGraphPoint();
				p.IsSupportPoint = true;
				p.X = item.X;
				p.Y = 0;
				list.Add(p);
			}
			else if(graph.GraphType == SimGraphType.BarHistogram)
			{
				p = new SimGraphPoint();
				p.IsSupportPoint = true;
				p.X = item.X + (item.Arm == -1 ? graph.HistogramArm : item.Arm);
				p.Y = item.Y;
				list.Add(p);
			}
			
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Очищает коллекцию.
		/// </summary>
		public void Clear()
		{
			list.Clear();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет, содержится ли точка в коллекции точек графика.
		/// </summary>
		/// <param name="item">Определяемая точка.</param>
		/// <returns></returns>
		public bool Contains(SimGraphPoint item)
		{
			foreach(SimGraphPoint p in list)
				if(p == item && p.IsSupportPoint == false)
					return true;
			return false;  
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает коллекцию как массив точек.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(SimGraphPoint[] array, int arrayIndex)
		{
			throw new Exception("Метод CopyTo не реализован!");
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет точку из коллекции.
		/// </summary>
		/// <param name="item">Удаляемая точка.</param>
		/// <returns></returns>
		public bool Remove(SimGraphPoint item)
		{
			return list.Remove(item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает перечислитель коллекции.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<SimGraphPoint> GetEnumerator()
		{
			for(int i = 0; i < list.Count; i++)
			{
				if(!list[i].IsSupportPoint)
					yield return list[i];
			}
		}
		/// <summary>
		/// Возвращает перечислитель коллекции.
		/// </summary>
		/// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает индекс точки в коллекции.
		/// </summary>
		/// <param name="item">Искомая точка.</param>
		/// <returns></returns>
		public int IndexOf(SimGraphPoint item)
		{
			return list.IndexOf(item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Вставляет точку в указанную позицию.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, SimGraphPoint item)
		{
			list.Insert(index, item);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет точку из указанной позиции. 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
		#region ICollection, IList Members
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((SimGraphPoint[])array, index);
		}
		//-------------------------------------------------------------------------------------
		int ICollection.Count
		{
			get { return this.Count; }
		}
		//-------------------------------------------------------------------------------------
		bool ICollection.IsSynchronized
		{
			get { throw new Exception("The method or operation is not implemented.11"); }
		}
		//-------------------------------------------------------------------------------------
		object ICollection.SyncRoot
		{
			get { throw new Exception("The method or operation is not implemented.10"); }
		}
		//-------------------------------------------------------------------------------------
		int IList.Add(object value)
		{
			this.Add((SimGraphPoint)value);
			return list.Count-1;
		}
		//-------------------------------------------------------------------------------------
		void IList.Clear()
		{
			this.Clear();
		}
		//-------------------------------------------------------------------------------------
		bool IList.Contains(object value)
		{
			return this.Contains((SimGraphPoint)value);
		}
		//-------------------------------------------------------------------------------------
		int IList.IndexOf(object value)
		{
			return this.IndexOf((SimGraphPoint)value);
		}
		//-------------------------------------------------------------------------------------
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (SimGraphPoint)value);
		}
		//-------------------------------------------------------------------------------------
		bool IList.IsFixedSize
		{
			get { return false; }
		}
		//-------------------------------------------------------------------------------------
		bool IList.IsReadOnly
		{
			get { return false; }
		}
		//-------------------------------------------------------------------------------------
		void IList.Remove(object value)
		{
			this.Remove((SimGraphPoint)value);
		}
		//-------------------------------------------------------------------------------------
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}
		//-------------------------------------------------------------------------------------
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (SimGraphPoint)value;
			}
		}

		#endregion    
	}
	//**************************************************************************************
	#region internal struct ExRectangle
	internal struct ExRectangle
	{
		private int left;
		private int top;
		private int right;
		private int bottom;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		public int Left
		{
			get { return left; }
			set { left = value; }
		}
		public int Top
		{
			get { return top; }
			set { top = value; }
		}
		public int Right
		{
			get { return right; }
			set { right = value; }
		}
		public int Bottom
		{
			get { return bottom; }
			set { bottom = value; }
		}
		public int Width
		{
			get { return right - left; }
			set { right = left + value; }
		}
		public int Height
		{
			get { return bottom - top; }
			set { bottom = top + value; }
		}
		public int X
		{
			get { return left; }
			set { left = value; }
		}
		public int Y
		{
			get { return top; }
			set { top = value; }
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		public ExRectangle(Rectangle r)
		{
			left = r.X;
			top = r.Y;
			right = left + r.Width;
			bottom = top + r.Height;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
						
		public static implicit operator ExRectangle(Rectangle r)
		{
			return new ExRectangle(r);
		}
	}
	#endregion internal struct ExRectangle
}
