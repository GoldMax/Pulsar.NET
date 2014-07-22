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
 /// <summary>
 /// Класс контрола создания объекта.
 /// </summary>
 public partial class CtrlCreateObject : SimModalControl
 {

  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Созданный объект
  /// </summary>
  public object Object { get; private set; }
  /// <summary>
  /// Хранилище объектов
  /// </summary>
  internal PDictionary<string, FormConsole.StoreItem> ObjectStore { get; set; }
  /// <summary>
  /// Имя объекта.
  /// </summary>
  public string ObjectName
  {
   get { return tbName.Text; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public CtrlCreateObject()
  {
   InitializeComponent();
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Controls Handlers>>
  public override void Show()
  {
   try
   {
    tbName.Text = "";
    cbAssembly.SelectedItem = null;
    cbAssembly.Items.Clear();
    cbType.SelectedItem = null;
    cbType.Items.Clear();
    tbArgs.Text = "";

    foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
     cbAssembly.Items.Add(new ComboBoxItem<Assembly>(a));
     //if(a.GetName().Name == "Sim.Pulsar")


    base.Show();
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void tbName_TextChanged(object sender, EventArgs e)
  {
   buttonOK.Enabled = !(tbName.Text.Length == 0 || cbAssembly.SelectedItem == null || cbType.SelectedItem == null);
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
    //if(asm.Equals(cbType.Tag) == false)
    {
     cbType.Items.Clear();
     foreach(Type t in asm.GetExportedTypes())
      cbType.Items.Add(new ComboBoxItem<Type>(t));
     cbType.Tag = asm;
    }
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void cbAssembly_SelectionChangeCommitted(object sender, EventArgs e)
  {
   cbType.Items.Clear();
   cbType.SelectedItem = null;
   buttonOK.Enabled  = false;
  }
  //-------------------------------------------------------------------------------------
  private void cbType_SelectedItemChanged(object sender, EventArgs e)
  {
   buttonOK.Enabled  = cbType.SelectedItem != null && tbName.Text.Length > 0;
  }
  //-------------------------------------------------------------------------------------
  private void buttonOK_Click(object sender, EventArgs e)
  {
   try
   {
    Assembly asm = null;
    if(cbAssembly.SelectedItem != null)
     asm = ((ComboBoxItem<Assembly>)cbAssembly.SelectedItem).Key;
    else
     asm = Assembly.Load(cbAssembly.Text);
    if(asm == null)
     throw new Exception("Не удалось загрузить сборку!");
    string type = null;
    if(cbType.SelectedItem == null)
     type = cbType.Text;
    else
     type = ((ComboBoxItem<Type>)cbType.SelectedItem).Key.FullName;
    
    if(String.IsNullOrWhiteSpace(tbGeneric.Text))
    { 
     try
     {
      Object = asm.CreateInstance(type, true, BindingFlags.Instance|BindingFlags.NonPublic | BindingFlags.Public,
                                    null, ParseArgs(),null, null);
     }
     catch
     {
      DialogResult res = SimModalMessageBox.Show(this.Parent,
       "Необходимый конструктор не найден!\r\nСоздать объект не используя конструкторы?",
       "Вопрос",MessageBoxIcon.Question, MessageBoxButtons.YesNoCancel);
      if(res == DialogResult.Yes)
       Object = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(asm.GetType(type));
      else if(res == DialogResult.Cancel)
       return;
      else
      {
       this.Hide();
       OnDialogClosed(DialogResult.Cancel);
       return;
      }
     }
    }
    else
    {
     Type t = asm.GetType(type, true, true);
     if(t.ContainsGenericParameters == false)
      throw new PulsarException("Тип [{0}] не содержит generic параметров!", t.FullName);
     t = t.MakeGenericType(ParseGenerics());
     object[] args = ParseArgs();
     Type[] ts = null;
     if(args != null && args.Length > 0)
     {
      ts = new Type[args.Length];
      for(int a = 0; a < args.Length; a++)
       ts[a] = args[a].GetType();
     }
     else
      ts = Type.EmptyTypes; 
     ConstructorInfo ci = t.GetConstructor(ts);
     if(ci == null)
      throw new Exception("Не найден соответствующий конструктор!");
     Object = ci.Invoke(args);
    }
    if(Object == null)
     throw new Exception("Не удалось создать объект!");
    this.Hide();
    OnDialogClosed(DialogResult.OK);
   }
   catch(Exception Err)
   {
    Sim.Controls.ErrorBox.Show(Err);
   }
  }
  //-------------------------------------------------------------------------------------
  private void buttonCancel_Click(object sender, EventArgs e)
  {
   this.Hide();
   OnDialogClosed(DialogResult.Cancel);
  }
  //-------------------------------------------------------------------------------------
  private void btnCancel_Click(object sender, EventArgs e)
  {
   buttonCancel_Click(sender, e);
  }
  //-------------------------------------------------------------------------------------
  #endregion << Controls Handlers>>
  //-------------------------------------------------------------------------------------
  #region << Methods >>
  private object[] ParseArgs()
  {
   string s = tbArgs.Text;
   string[] sss = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
   if(sss.Length == 0)
    return new object[0];
   object[] res = new object[sss.Length];
   for(int a = 0; a < sss.Length; a++)
   {
    s = sss[a];
    if(s.StartsWith("i_"))
     res[a] = Int32.Parse(s.Replace("i_", ""));
    else if(s.StartsWith("ui_"))
     res[a] = UInt32.Parse(s.Replace("ui_", ""));
    else if(s.StartsWith("b_"))
     res[a] = Byte.Parse(s.Replace("b_", ""));
    else if(s.StartsWith("s_"))
     res[a] = Int16.Parse(s.Replace("s_", ""));
    else if(s.StartsWith("us_"))
     res[a] = UInt16.Parse(s.Replace("us_", ""));
    else if(s.StartsWith("l_"))
     res[a] = Int64.Parse(s.Replace("l_", ""));
    else if(s.StartsWith("ul_"))
     res[a] = UInt64.Parse(s.Replace("ul_", ""));
    else if(s.StartsWith("f_"))
     res[a] = float.Parse(s.Replace("f_", ""));
    else if(s.StartsWith("d_"))
     res[a] = Decimal.Parse(s.Replace("d_", ""));
    else if(s.StartsWith("$"))
     res[a] = ObjectStore[s.Replace("$", "")].Object;
    else
    {
     bool bVal;
     int iVal;
     DateTime dtVal;
     Guid gVal;

     if(Boolean.TryParse(s, out bVal))
      res[a] = bVal;
     else if(Int32.TryParse(s, out iVal))
      res[a] = iVal;
     else if(Guid.TryParse(s, out gVal))
      res[a] = gVal;
     else if(DateTime.TryParse(s, out dtVal))
      res[a] = dtVal;
     else
      res[a] = s.Replace("\"", "");
    }
   }
   return res;
  }

  //-------------------------------------------------------------------------------------
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
     res[a] = typeof( System.Byte);
    else if(s == "sbyte")
     res[a] = typeof( System.SByte);
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
      throw new PulsarException("Тип [{0}] не является встроеным!", s);
    }
   }
   return res;
  }
  #endregion << Methods >>

  //-------------------------------------------------------------------------------------
      
      
 }
}
