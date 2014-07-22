using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Pulsar;

namespace Sim.Refs
{
 /// <summary>
 /// Класс сгруппированных изображений, одно из которых является главным
 /// </summary>
 public class ImageCluster: PulsarCluster<Image>
 {
  #region << Constructors >>

  /// <summary>
  /// Конструктор по умолчанию
  /// </summary>
  public ImageCluster()
   : base()
  { }

  /// <summary>
  /// Инициализирующий конструктор
  /// </summary>
  /// <param name="item"></param>
  public ImageCluster(Image item)
   : base(item)
  {
   if (item == null)
    throw new ArgumentNullException("Элемент кластера изображений не может быть не определенным!");
  }

  /// <summary>
  /// Инициализирующий конструктор
  /// </summary>
  /// <param name="items"></param>
  public ImageCluster(IEnumerable<Image> items)
   : base(items)
  {
   foreach (Image img in items)
    if (img == null)
     throw new ArgumentNullException("Элемент кластера изображений не может быть не определенным!");

  }

  #endregion << Constructors >>
 }
}
