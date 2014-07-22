using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Sim.Controls;
using Pulsar;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола множественного выбора.
 /// </summary>
 public partial class SimModalMultiChoiceBoxDual : Sim.Controls.SimModalControl
 {
  private IPulsarBinder vars = null;
  private IPulsarBinder choices = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Определяет текст заголовка формы.
  /// </summary>
  public string CaptionText
  {
   get { return this.finistLabelCaption.Text; }
   set { this.finistLabelCaption.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// пределяет изображение заголовка формы.
  /// </summary>
  public Image CaptionImage
  {
   get { return this.finistLabelCaption.Image; }
   set { this.finistLabelCaption.Image = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет текст заголовка вариантов.
  /// </summary>
  public string VariantsCaption
  {
   get { return groupBoxVariants.Text; }
   set { groupBoxVariants.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Определяет текст заголовка выбранных элементов.
  /// </summary>
  public string ChoicesCaption
  {
   get { return groupBoxChoices.Text; }
   set { groupBoxChoices.Text = value; }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Список вариантов.
  /// </summary>
  public IPulsarBinder Variants 
  { 
   get { return vars; }
   set
   {
    vars = value;
    if (vars != null && choices != null)
    {
      if (Variants.IsFiltered == false)
       Variants.ApplyFilter(Filter);
      else
       Variants.Refresh();
    }
   }
  }
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Список выбранных строк.
  /// </summary>
  public IPulsarBinder Choices
  {
   get { return choices; }
   set
   {
    choices = value;
    if (vars != null && choices != null)
    {
     if (Variants.IsFiltered == false)
      Variants.ApplyFilter(Filter);
     else
      Variants.Refresh();
    }
   }
  }

  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimModalMultiChoiceBoxDual()
  {
   InitializeComponent();
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Отображает диалог.
  /// </summary>
  /// <param name="columnName">Имена отображаемых столбцов.</param>
  public void Show( params string[] columnName)
  {
   fdgvVariants.DataSource = Variants;
   foreach(DataGridViewColumn c in fdgvVariants.Columns)
   {
    if(c is DataGridViewImageColumn)
    {
     c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
     c.HeaderText = "";
     c.MinimumWidth = 20;
     c.Resizable = DataGridViewTriState.False;
     c.SortMode = DataGridViewColumnSortMode.NotSortable;
     c.Width = 20;
    }
    if (columnName.InList((s) => s == c.Name) == null)
     c.Visible = false;
   }
   
   if(Variants != null && Choices == null)
   {
    object r = Variants.Collection.GetType().GetConstructor(Type.EmptyTypes).Invoke(null);
     Type listType = r.GetType();
     Choices = (IPulsarBinder)typeof(ListBinder).GetConstructor(new Type[] { listType }).Invoke(new object[] { r });
   }

   fdgvChoises.DataSource = Choices;
   foreach (DataGridViewColumn c in fdgvChoises.Columns)
   {
    if (c is DataGridViewImageColumn)
    {
     c.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
     c.HeaderText = "";
     c.MinimumWidth = 20;
     c.Resizable = DataGridViewTriState.False;
     c.SortMode = DataGridViewColumnSortMode.NotSortable;
     c.Width = 20;
    }
    if (columnName.InList((s) => s == c.Name) == null)
     c.Visible = false;
   }
   base.Show();
  }
  //-------------------------------------------------------------------------------------
  private void buttonInclude_Click(object sender, EventArgs e)
  {
   try
   {
    foreach(DataGridViewRow item in fdgvVariants.SelectedRows)
     Choices.Add(item.GetData());
    if(Variants.IsFiltered == false)
     Variants.ApplyFilter(Filter);
    else
     Variants.Refresh();
    
    buttonOk.Enabled = true;
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void buttonExclude_Click(object sender, EventArgs e)
  {
   try
   {
    foreach(DataGridViewRow item in fdgvChoises.SelectedRows)
     Choices.Remove(item.GetData());
    if(Variants.IsFiltered == false)
     Variants.ApplyFilter(Filter);
    else
     Variants.Refresh();

    buttonOk.Enabled = true;
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
  {
   buttonInclude_Click(this, null);
  }
  //-------------------------------------------------------------------------------------
  private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
  {
   buttonExclude_Click(this, null);
  }
  //-------------------------------------------------------------------------------------
  private void buttonOk_Click(object sender, EventArgs e)
  {
   this.Hide();
   OnDialogClosed(DialogResult.OK);
  }
  //-------------------------------------------------------------------------------------
  private void buttonCancel_Click(object sender, EventArgs e)
  {
   this.Hide();
   OnDialogClosed(DialogResult.Cancel);
  }
  //-------------------------------------------------------------------------------------
  private void fdgvVariants_SelectionChanged(object sender, EventArgs e)
  {
   if(fdgvVariants.SelectedRows.Count == 0)
    buttonInclude.Enabled = false;
   else
    buttonInclude.Enabled = true;
  }
  //-------------------------------------------------------------------------------------
  private void fdgvChoises_SelectionChanged(object sender, EventArgs e)
  {
   if(fdgvChoises.SelectedRows.Count == 0)
    buttonExclude.Enabled = false;
   else
    buttonExclude.Enabled = true;
  }
  //-------------------------------------------------------------------------------------
  private bool Filter(object val)
  {
   return ! Choices.Contains(val);
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
          
 }
}

