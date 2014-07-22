using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// ����� ��������������� ������� ��� ������ � ����������.
 /// </summary>
 public static class WinFormsUtils
 {
  private static MethodInfo OnPaintMeth = null;
  static WinFormsUtils()
  {
   OnPaintMeth = typeof(Control).GetMethod("OnPaint", BindingFlags.Instance | BindingFlags.Public |
                                                         BindingFlags.NonPublic);

  }

  /// <summary>
  /// ������������� ������ �� ��������� ��� ������� � ���� ��� �������� ���������.
  /// </summary>
  /// <param name="ctrl">�������, ��� �������� ��������������� ������ �� ���������.</param>
  public static void SetDefaultCursor(Control ctrl)
  {
   ctrl.Cursor = Cursors.Default;
   foreach(Control c in ctrl.Controls)
    SetDefaultCursor(c);
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ��������� ������������� �������� DoubleBuffered ���������.
  /// </summary>
  /// <param name="c">�������, ��� �������� ��������� ���������� ��������.</param>
  /// <param name="value">��������, ��������������� ��� ��������.</param>
  public static void SetDoubleBuffered(Control c, bool value)
  {
   Type t = typeof(Control);
   t.InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                   null, c, new object[] { value });
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ������������� ����� ��� ��������
  /// </summary>
  /// <param name="c">�������, ��� �������� ��������� ���������� �����.</param>
  /// <param name="styles">��������������� �����.</param>
  /// <param name="set">�������� ��������� ��� ������.</param>
  public static void SetStyle(Control c, ControlStyles styles, bool set)
  {
   Type t = typeof(Control);
   t.InvokeMember("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
                   null, c, new object[] { styles, set });
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// �������� ����� Paint ���������� ��������.
  /// </summary>
  /// <param name="c">������� ����������.</param>
  /// <param name="args">��������� ���������.</param>
  public static void CallPaint(Control c, PaintEventArgs args)
  {
   OnPaintMeth.Invoke(c, new object[] { args });
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// ���������� ������� �������� �������� � ������� Parent == null
  /// </summary>
  /// <param name="ctrl"></param>
  public static void ClearChilds(Control ctrl)
  {
   try
   {
    ctrl.SuspendLayout();
    Stack<Control> st = new Stack<Control>();
    foreach(Control c in ctrl.Controls)
     st.Push(c);
    while(st.Count > 0)
    {
     Control c =  st.Pop();
     c.Parent = null;
     if(c.Controls.Count > 0)
     {
      foreach(Control cc in c.Controls)
       st.Push(cc);
     }
     c.Controls.Clear();
    }
   }
   finally
   {
    ctrl.ResumeLayout();
   }

  }
 }
}
