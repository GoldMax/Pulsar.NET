using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс формы поика по SimDataGridView.
 /// </summary>
 public partial class SimDataGridViewFindForm :Form
 {
  private SimDataGridView view = null;
  private Timer timer = new Timer();
  private int preferedColumnIndex = -1;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Таблица, по которой осуществляется поиск.
  /// </summary>
  public SimDataGridView View
  {
   get { return view; }
   set { view = value; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimDataGridViewFindForm()
  {
   InitializeComponent();
   this.BackColor = ControlPaint.Light(SystemColors.Control, 0.5f);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Инициализирующий конструктор.
  /// </summary>
  public SimDataGridViewFindForm(SimDataGridView finistDataGridView, string prevFind) : this()
  {
   view = finistDataGridView;
   timer.Tick += new EventHandler(timer_Tick);
   textBoxText.Text = prevFind;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Form Handlers >>
  private void SimDataGridViewFindForm_Shown(object sender, EventArgs e)
  {
   try
   {
    ComboBoxItem<int> selCol = null;
    int cur = -1;
    if(preferedColumnIndex != -1)
     cur = preferedColumnIndex;
    else if(view.CurrentCell != null)
     cur = view.CurrentCell.ColumnIndex;
    
    
    foreach(DataGridViewColumn col in view.Columns)
    {
     if(col.Visible == false)
      continue;
     
     ComboBoxItem<int> i = new ComboBoxItem<int>(col.Index, col.HeaderText); 
     comboBoxColumns.Items.Add(i);
     if(col.Index == cur)
      selCol = i; 
    }
    
    if(selCol != null)
     comboBoxColumns.SelectedItem = selCol;
    else
     comboBoxColumns.SelectFirstDropDownItem(); 
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void SimDataGridViewFindForm_Deactivate(object sender, EventArgs e)
  {
   if(comboBoxColumns.IsDropDownOpened)
    return;
   view.prevFind = textBoxText.Text;
   timer.Interval = 100;
   timer.Start();
  }
  //-------------------------------------------------------------------------------------
  void timer_Tick(object sender, EventArgs e)
  {
   timer.Stop();
   this.Close();
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Show
  /// </summary>
  /// <param name="owner"></param>
  /// <param name="preferedColumnIndex"></param>
  public void Show(IWin32Window owner, int preferedColumnIndex)
  {
   this.preferedColumnIndex = preferedColumnIndex;
   this.Show(owner);
  }
  #endregion << Form Handlers >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers >>
  private void textBoxText_TextChanged(object sender, EventArgs e)
  {
   if(textBoxText.Text.Length > 0)
   {
    if(!buttonFind.Enabled)
     buttonFind.Enabled = true;
   } 
   else
    buttonFind.Enabled = false;
  }
  //-------------------------------------------------------------------------------------
  private void buttonFind_Click(object sender, EventArgs e)
  {
   pictureBox1.Visible = true;
   Application.DoEvents();
   
   string s = textBoxText.Text.ToLower();
   s = Regex.Escape(s);
   s = "^" + s + "$";
   s = s.Replace("\\*",".*");
   s = s.Replace("\\?",".{1}");
   Regex rx = new Regex(s, RegexOptions.IgnoreCase | RegexOptions.Singleline);

   int colIndex = ((ComboBoxItem<int>)comboBoxColumns.SelectedItem).Key;
   int end = (view.CurrentCell == null ? 0 : view.CurrentCell.RowIndex);
   int a = end;
   do 
   {
    if(a == view.Rows.Count-1)
     a = 0;
    else
     a++; 
    if(rx.IsMatch(view.Rows[a].Cells[colIndex].FormattedValue.ToString()))
    {
     view.CurrentCell = view.Rows[a].Cells[colIndex];
     break;
    }
   Application.DoEvents();
   } while( a != end);

   pictureBox1.Visible = false;
  }
  //-------------------------------------------------------------------------------------
  private void textBoxText_KeyPress(object sender, KeyPressEventArgs e)
  {
   if(e.KeyChar == '\r' && buttonFind.Enabled)
    buttonFind_Click(buttonFind, null);
  }
  //-------------------------------------------------------------------------------------
  #endregion << Controls Handlers >>
 }
}