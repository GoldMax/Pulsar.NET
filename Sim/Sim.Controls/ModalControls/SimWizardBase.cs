using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс базового контрола мастеров.
 /// </summary>
 public partial class SimWizardBase : Sim.Controls.SimModalControl
 {
  private int step = -1;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Browsable Properties >>
  /// <summary>
  /// Заголовок мастера.
  /// </summary>
  [Category("Appearance")]
  [Description("Заголовок мастера.")]
  public string Caption
  {
   get { return this.finistLabelCaption.Text; }
   set { this.finistLabelCaption.Text = value; }
  }
  #endregion << Browsable Properties >>
  #region << Nonbrowsable Properties >>
  /// <summary>
  /// Последний, полностью пройденный, шаг.
  /// </summary>
  [Browsable(false)]
  public int LastPassedStep
  {
   get { return step; }
   set 
   { 
    step = value; 
    //if(step >= tabControlMain.SelectedIndex)
    // buttonNext.Enabled = true;
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimWizardBase()
  {
   InitializeComponent();
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers >>
  private void buttonCancel_Click(object sender, EventArgs e)
  {
   this.Hide();
   OnDialogClosed(DialogResult.Cancel);
  }
  //-------------------------------------------------------------------------------------
  private void buttonNext_Click(object sender, EventArgs e)
  {
   try
   {
    if(buttonNext.Image != null)
    {
     OnDoneClick();
     return;
    }
    tabControlMain.SelectedIndex++;
    finistLabelStep.Text = String.Format("Шаг {0} из {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
    if(buttonPrev.Visible == false)
     buttonPrev.Visible = true;
    if(tabControlMain.SelectedIndex == tabControlMain.TabPages.Count - 1)
    {
     buttonNext.Image = global::Sim.Controls.Properties.Resources.OK;
     buttonNext.Text = "Готово";
    }
    if(tabControlMain.SelectedIndex <= step)
     buttonNext.Enabled = true;  
    else
     buttonNext.Enabled = false; 
    OnSelectedTabChanged(tabControlMain.SelectedTab);
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }   
  }
  //-------------------------------------------------------------------------------------
  private void buttonPrev_Click(object sender, EventArgs e)
  {
   try
   {
    if(buttonNext.Image != null)
    {
     buttonNext.Image = null;
     buttonNext.Text = "Далее >>";
    } 
    tabControlMain.SelectedIndex--; 
    if(tabControlMain.SelectedIndex == 0)
     buttonPrev.Visible = false;
    finistLabelStep.Text = String.Format("Шаг {0} из {1}  {2}",
     tabControlMain.SelectedIndex + 1,
     tabControlMain.TabPages.Count,
     tabControlMain.SelectedTab.Text);
    if(tabControlMain.SelectedIndex <= step)
     buttonNext.Enabled = true;
    else
     buttonNext.Enabled = false;
    OnSelectedTabChanged(tabControlMain.SelectedTab);  
   }
   catch(Exception Err)
   {
    ErrorBox.Show(Err);
   }
  }
  /// <summary>
  /// Вызывается при смене вкладки
  /// </summary>
  /// <param name="tab"></param>
  protected virtual void OnSelectedTabChanged(TabPage tab)
  {

  }
  /// <summary>
  /// Вызывается при нажатии кнопки Готово.
  /// </summary>
  protected virtual void OnDoneClick()
  {
   this.Hide();
   OnDialogClosed(DialogResult.OK);
  }
  #endregion << Controls Handlers >>
  //-------------------------------------------------------------------------------------
  #region << Overrides Methods >>
  /// <summary>
  /// OnVisibleChanged
  /// </summary>
  /// <param name="e"></param>
  protected override void OnVisibleChanged(EventArgs e)
  {
   if(this.Visible)
   {
    buttonPrev.Visible = false;
    if(tabControlMain.SelectedIndex <= step)
     buttonNext.Enabled = true;
    if(tabControlMain.TabPages.Count == 0)
     finistLabelStep.Text = "Шаг 0 из 0";
    else
     finistLabelStep.Text = String.Format("Шаг {0} из {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
    if(tabControlMain.SelectedIndex == tabControlMain.TabPages.Count - 1)
    {
     buttonNext.Image = global::Sim.Controls.Properties.Resources.OK;
     buttonNext.Text = "Готово";
    }
     
    base.OnVisibleChanged(e);
   } 
  }
  #endregion << Overrides Methods >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Объявляет текущий шаг пройденным. 
  /// </summary>
  public void CurrentStepPassed()
  {
   if(LastPassedStep < tabControlMain.SelectedIndex)
    LastPassedStep = tabControlMain.SelectedIndex;
   if(buttonNext.Enabled == false)
    buttonNext.Enabled = true; 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Объявляет текущий шаг непройденным.
  /// </summary>
  public void ResetToCurrentStep()
  {
   LastPassedStep = tabControlMain.SelectedIndex-1;
   if(buttonNext.Enabled)
    buttonNext.Enabled = false; 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Добавляет страницу шага.
  /// </summary>
  /// <param name="tab">Добавляемая страница.</param>
  public void AddTab(TabPage tab)
  {
   tabControlMain.TabPages.Add(tab);
   finistLabelStep.Text = String.Format("Шаг {0} из {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
   if(buttonNext.Image != null)
   {
    buttonNext.Image = null;
    buttonNext.Text = "Далее >>";
   }   
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Вставляет страницу шага в указанную позицию.
  /// </summary>
  /// <param name="index">Индекс вставляемой страницы.</param>
  /// <param name="tab">Добавляемая страница.</param>
  public void InsertTab(int index, TabPage tab)
  {
   tabControlMain.TabPages.Insert(index, tab);
   finistLabelStep.Text = String.Format("Шаг {0} из {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
   if(tabControlMain.SelectedIndex != tabControlMain.TabPages.Count -1 && buttonNext.Image != null)
   {
    buttonNext.Image = null;
    buttonNext.Text = "Далее >>";
   }   
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Удаляет страницу шага.
  /// </summary>
  /// <param name="tab">Удаляемая страница.</param>
  public void RemoveTab(TabPage tab)
  {
   tabControlMain.TabPages.Remove(tab);
   finistLabelStep.Text = String.Format("Шаг {0} из {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
   if(tabControlMain.SelectedIndex == tabControlMain.TabPages.Count - 1)
   {
    buttonNext.Image = global::Sim.Controls.Properties.Resources.OK;
    buttonNext.Text = "Готово";
   }  
  }
  #endregion << Methods >>
 }
}

