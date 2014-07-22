using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Sim.Controls;
using Pulsar;

namespace Sim.AdminForms
{
 public partial class CtrlSelectTypes : UserControl
 {
  private PList<Type> types = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Выбранные типы.
  /// </summary>
  public PList<Type> Types
  {
   get { return types; }
   set
   {
    types = value; 
    fdgvList.DataSource = new ListBinder(types);
   }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public CtrlSelectTypes()
  {
   InitializeComponent();
   BackColor = Color.Transparent;
   fdgvList.AllowAutoGenerateColumns = false;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers>>
  private void cbAssembly_DropDown(object sender, EventArgs e)
  {
   if(cbAssembly.Items.Count == 0)
    foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
     cbAssembly.Items.Add(new ComboBoxItem<Assembly>(a));
  }
  //-------------------------------------------------------------------------------------
  private void cbAssembly_SelectionChangeCommitted(object sender, EventArgs e)
  {
   cbType.Items.Clear();
   cbType.SelectedItem = null;
  }
  //-------------------------------------------------------------------------------------
  private void cbType_DropDown(object sender, EventArgs e)
  {
   try
   {
    Assembly asm = null;
    if(cbAssembly.SelectedItem != null)
     asm = ((ComboBoxItem<Assembly>)cbAssembly.SelectedItem).Key;
    else if(cbAssembly.Text.Length != 0)
     asm = Assembly.Load(cbAssembly.Text);
    if(asm == null)
     return;
    if(asm.Equals(cbType.Tag) == false)
    {
     cbType.Items.Clear();
     foreach(Type t in asm.GetExportedTypes())
      cbType.Items.Add(new ComboBoxItem<Type>(t));
     cbType.Tag = asm;
    }
    btnAddType.Enabled = cbAssembly.SelectedItem != null;
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void btnAddType_Click(object sender, EventArgs e)
  {
   try
   {
    Type t = null;
    Assembly asm = null;
    if(cbAssembly.SelectedItem != null)
     asm = ((ComboBoxItem<Assembly>)cbAssembly.SelectedItem).Key;
    else
     asm = Assembly.Load(cbAssembly.Text);
    if(asm == null)
     return;
    string type = null;
    if(cbType.SelectedItem == null)
     type = cbType.Text;
    else
     type = ((ComboBoxItem<Type>)cbType.SelectedItem).Key.FullName;

    if(String.IsNullOrWhiteSpace(tbGeneric.Text))
     t = asm.GetType(type,false,true);
    else
    {
     t = asm.GetType(type, false, true);
     if(t.ContainsGenericParameters == false)
      return;
     t = t.MakeGenericType(ParseGenerics());
    }
    if(t == null)
     return;
    if(types.Contains(t) == false)
     types.Add(t);
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void fdgvList_SelectionChanged(object sender, EventArgs e)
  {
   btnDel.Enabled = fdgvList.SelectedRows.Count > 0;
  }
  //-------------------------------------------------------------------------------------
  private void btnDel_Click(object sender, EventArgs e)
  {
   foreach(DataGridViewRow r in fdgvList.SelectedRows)
    types.Remove((Type)r.GetData());
  }
  //-------------------------------------------------------------------------------------
  private void btnClear_Click(object sender, EventArgs e)
  {
   types.Clear();
  }
  #endregion << Controls Handlers>>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  private Type[] ParseGenerics()
  {
   string[] ss = tbGeneric.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
   if(ss.Length == 0)
    return null;
   Type[] res = new Type[ss.Length];
   for(int a = 0; a < ss.Length; a++)
   {
    string s = ss[a].Trim().ToLower();
    if(s == "bool")
     res[a] = typeof(System.Boolean);
    else if(s == "byte")
     res[a] = typeof(System.Byte);
    else if(s == "sbyte")
     res[a] = typeof(System.SByte);
    else if(s == "char")
     res[a] = typeof(System.Char);
    else if(s == "decimal")
     res[a] = typeof(System.Decimal);
    else if(s == "double")
     res[a] = typeof(System.Double);
    else if(s == "float")
     res[a] = typeof(System.Single);
    else if(s == "int")
     res[a] = typeof(System.Int32);
    else if(s == "uint")
     res[a] = typeof(System.UInt32);
    else if(s == "long")
     res[a] = typeof(System.Int64);
    else if(s == "ulong")
     res[a] = typeof(System.UInt64);
    else if(s == "object")
     res[a] = typeof(System.Object);
    else if(s == "short")
     res[a] = typeof(System.Int16);
    else if(s == "ushort")
     res[a] = typeof(System.UInt16);
    else if(s == "string")
     res[a] = typeof(System.String);
    else
    {
     foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
      if((res[a] = asm.GetType(s, false, true)) != null)
       break;
     if(res[a] == null)
      return Type.EmptyTypes;
    }
   }
   return res;
  }
  #endregion << Methods >>
 }
}
