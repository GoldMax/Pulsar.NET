using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
 /// <summary>
	/// Класс контрола RadioButton СИМ.
 /// </summary>
 public class SimRadioButton : RadioButton
 {

  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  public SimRadioButton() : base()
  {

  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  #region << Override Methods >>
  /// <summary>
  /// OnClick(EventArgs e)
  /// </summary>
  /// <param name="e"></param>
  protected override void OnClick(EventArgs e)
  {
   if(this.Enabled)
    base.OnClick(e);
  }
  #endregion << Override Methods >>
  //-------------------------------------------------------------------------------------
          
 }
}
