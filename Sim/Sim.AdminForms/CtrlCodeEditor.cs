using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using Sim.Controls;
using Pulsar;
using Pulsar.Serialization;

namespace Sim.AdminForms
{
 /// <summary>
 /// Класс контрола редактирования и компиляции теста кода.
 /// </summary>
 public partial class CtrlCodeEditor : Sim.Controls.SimModalControl
 {
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Результирующий кодовый запрос.
  /// </summary>
  public PulsarCodeTransfer.CodeQuery CodeQuery { get; private set; }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public CtrlCodeEditor()
  {
   InitializeComponent();
   fdgvErrs.AllowAutoGenerateColumns = false;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers>>
  private void btnWordWrap_Click(object sender, EventArgs e)
  {
   richTextBox1.WordWrap = btnWordWrap.Checked;
  }
  //-------------------------------------------------------------------------------------
  private void buttonCancel_Click(object sender, EventArgs e)
  {
   this.Hide();
   OnDialogClosed(DialogResult.Cancel);
  }
  //-------------------------------------------------------------------------------------
  private void cbObject_SelectionChangeCommitted(object sender, EventArgs e)
  {
   try
   {
    int end = richTextBox1.Text.LastIndexOf("}");
    if(end != -1)
     end = richTextBox1.Text.LastIndexOf("}", end - 1);
    int start = richTextBox1.Text.IndexOf("private void DynamicMethod()");
    if(start != -1)
     start = richTextBox1.Text.IndexOf("{", start) + 1;
    string body = "";
    if(start > -1 && end > -1)
     body = richTextBox1.Text.Substring(start, end - start);
    richTextBox1.Clear();
    richTextBox1.Text = 
     PulsarCodeTransfer.GetClassWrapperText(((ComboBoxItem<Type>)cbObject.SelectedItem).Key, body);
   }
   catch(Exception Err)
   {
    ModalErrorBox.Show(Err, this);
   }
  }
  //-------------------------------------------------------------------------------------
  private void richTextBox1_TextChanged(object sender, EventArgs e)
  {
   CodeQuery = null;
   buttonOK.Text = "Компиляция";
   buttonOK.Image = global::Sim.AdminForms.Properties.Resources.Compile;
   if(richTextBox1.Text.Length > 0)
    buttonOK.Enabled = true;
  }
  //-------------------------------------------------------------------------------------
  private void buttonOK_Click(object sender, EventArgs e)
  {
   try
   {
    if(buttonOK.Text == "OK")
    {
     this.Hide();
     OnDialogClosed(DialogResult.OK);
    }
    else
    {
     Type t = ((ComboBoxItem<Type>)cbObject.SelectedItem).Key;
     ValuesPair<PulsarCodeTransfer.CodeQuery,List<string>> query = 
     PulsarCodeTransfer.GetCodeQuery(t, richTextBox1.Text);
     fdgvErrs.DataSource = new ListBinder(query.Value2);
     if(query.Value1 != null)
     {
      buttonOK.Text = "OK";
      buttonOK.Image = global::Sim.AdminForms.Properties.Resources.OK;
      CodeQuery = query.Value1;
     }
    }
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void fdgvErrs_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
  {
   try
   {
    string s = fdgvErrs[e.ColumnIndex, e.RowIndex].Value.ToString();
    int col =  s.IndexOf(",");
    int line = Int32.Parse(s.Substring(1,col-1));
    col = Int32.Parse(s.Substring(col+1, s.IndexOf("]") - col - 1));
    line = richTextBox1.GetFirstCharIndexFromLine(line-1);
    richTextBox1.Select();
    richTextBox1.Focus();
    richTextBox1.Select(line + col, 0);
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  #endregion << Controls Handlers>>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  /// <summary>
  /// Отображает контрол.
  /// </summary>
  /// <param name="pulsarObjects">Перечисление типов объектов Пульсара.</param>
  /// <param name="current">Выбранный по умолчанию тип.</param>
  /// <param name="code">Строка кода</param>
  public void Show(IEnumerable<Type> pulsarObjects, Type current, string code = null)
  {
   try
   {
    if(Parent == null)
     throw new Exception("Не указан родительский контрол!");
    
    ComboBoxItem<Type> selItem = null;
    foreach(Type t in pulsarObjects)
     if(t == current)
      cbObject.Items.Add(selItem = new ComboBoxItem<Type>(t,t.FullName));
     else
      cbObject.Items.Add(new ComboBoxItem<Type>(t,t.FullName));
    cbObject.SelectedItem = selItem ?? (cbObject.Items.Count == 0 ? null : cbObject.Items[0]);
    if(code == null)
     cbObject_SelectionChangeCommitted(cbObject, EventArgs.Empty);
    else
     richTextBox1.Text = code;
    if(richTextBox1.Text.Length > 0)
     buttonOK.Enabled = true;
    Show();
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  #endregion << Methods >>
  //-------------------------------------------------------------------------------------
              


 }
}
