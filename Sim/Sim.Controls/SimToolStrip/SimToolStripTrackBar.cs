using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Sim.Controls
{
 /// <summary>
 /// Класс контрола панели инструментов ToolStrip - TrackBar.
 /// </summary>
 //[DesignerCategory("code")]
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
 [ToolboxBitmap(typeof(TrackBar))]
 [DefaultProperty("Checked")]
 [DefaultEvent("CheckedChanged")]
 public class SimToolStripTrackBar : ToolStripControlHost
 {
  private TrackBar trackBar = null;
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  #region << Properties >>
  /// <summary>
  /// Объект TrackBar.
  /// </summary>
  [Description("Объект TrackBar.")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  public TrackBar TrackBar
  {
   get { return trackBar; }
  }
  #endregion << Properties >>
  //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimToolStripTrackBar() : base (new TrackBar())
  {
   trackBar = (TrackBar)Control;
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Overrides Methods >>

  #endregion << Overrides Methods >>
  //-------------------------------------------------------------------------------------
          
 }
}
