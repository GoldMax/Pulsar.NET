using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
 /// Класс расширений для Control
 /// </summary>
 public static class ControlExtension
 {
  /// <summary>
  /// Расширение для Control, позволяющий найти PanelBack.
  /// </summary>
  /// <param name="ctrl"></param>
  /// <returns></returns>
  public static Control FindModalContainer(this Control ctrl)
  {
   Control p = ctrl.Parent;
   while(p != null && p is Form == false)
   {
    if(p is SimPanel && p.Name == "PanelBack" && p.Parent is TabPage)
     break;
    p = p is SimPopupControl ? ((SimPopupControl)p).Parent : p.Parent;
   }
   return p;
  }
 }
}
