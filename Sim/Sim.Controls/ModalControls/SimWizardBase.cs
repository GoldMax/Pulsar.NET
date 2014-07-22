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
 /// ����� �������� �������� ��������.
 /// </summary>
 public partial class SimWizardBase : Sim.Controls.SimModalControl
 {
  private int step = -1;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Browsable Properties >>
  /// <summary>
  /// ��������� �������.
  /// </summary>
  [Category("Appearance")]
  [Description("��������� �������.")]
  public string Caption
  {
   get { return this.finistLabelCaption.Text; }
   set { this.finistLabelCaption.Text = value; }
  }
  #endregion << Browsable Properties >>
  #region << Nonbrowsable Properties >>
  /// <summary>
  /// ���������, ��������� ����������, ���.
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
  /// ����������� �� ���������.
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
    finistLabelStep.Text = String.Format("��� {0} �� {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
    if(buttonPrev.Visible == false)
     buttonPrev.Visible = true;
    if(tabControlMain.SelectedIndex == tabControlMain.TabPages.Count - 1)
    {
     buttonNext.Image = global::Sim.Controls.Properties.Resources.OK;
     buttonNext.Text = "������";
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
     buttonNext.Text = "����� >>";
    } 
    tabControlMain.SelectedIndex--; 
    if(tabControlMain.SelectedIndex == 0)
     buttonPrev.Visible = false;
    finistLabelStep.Text = String.Format("��� {0} �� {1}  {2}",
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
  /// ���������� ��� ����� �������
  /// </summary>
  /// <param name="tab"></param>
  protected virtual void OnSelectedTabChanged(TabPage tab)
  {

  }
  /// <summary>
  /// ���������� ��� ������� ������ ������.
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
     finistLabelStep.Text = "��� 0 �� 0";
    else
     finistLabelStep.Text = String.Format("��� {0} �� {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
    if(tabControlMain.SelectedIndex == tabControlMain.TabPages.Count - 1)
    {
     buttonNext.Image = global::Sim.Controls.Properties.Resources.OK;
     buttonNext.Text = "������";
    }
     
    base.OnVisibleChanged(e);
   } 
  }
  #endregion << Overrides Methods >>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// ��������� ������� ��� ����������. 
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
  /// ��������� ������� ��� ������������.
  /// </summary>
  public void ResetToCurrentStep()
  {
   LastPassedStep = tabControlMain.SelectedIndex-1;
   if(buttonNext.Enabled)
    buttonNext.Enabled = false; 
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ��������� �������� ����.
  /// </summary>
  /// <param name="tab">����������� ��������.</param>
  public void AddTab(TabPage tab)
  {
   tabControlMain.TabPages.Add(tab);
   finistLabelStep.Text = String.Format("��� {0} �� {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
   if(buttonNext.Image != null)
   {
    buttonNext.Image = null;
    buttonNext.Text = "����� >>";
   }   
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ��������� �������� ���� � ��������� �������.
  /// </summary>
  /// <param name="index">������ ����������� ��������.</param>
  /// <param name="tab">����������� ��������.</param>
  public void InsertTab(int index, TabPage tab)
  {
   tabControlMain.TabPages.Insert(index, tab);
   finistLabelStep.Text = String.Format("��� {0} �� {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
   if(tabControlMain.SelectedIndex != tabControlMain.TabPages.Count -1 && buttonNext.Image != null)
   {
    buttonNext.Image = null;
    buttonNext.Text = "����� >>";
   }   
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ������� �������� ����.
  /// </summary>
  /// <param name="tab">��������� ��������.</param>
  public void RemoveTab(TabPage tab)
  {
   tabControlMain.TabPages.Remove(tab);
   finistLabelStep.Text = String.Format("��� {0} �� {1}  {2}",
      tabControlMain.SelectedIndex + 1,
      tabControlMain.TabPages.Count,
      tabControlMain.SelectedTab.Text);
   if(tabControlMain.SelectedIndex == tabControlMain.TabPages.Count - 1)
   {
    buttonNext.Image = global::Sim.Controls.Properties.Resources.OK;
    buttonNext.Text = "������";
   }  
  }
  #endregion << Methods >>
 }
}

