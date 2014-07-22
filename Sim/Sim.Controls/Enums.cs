using System;
using System.Collections.Generic;
using System.Text;

namespace Sim.Controls
{
 //**************************************************************************************
 #region << public enum GradientMode : int >>
 /// <summary>
 /// Перечисление видов градиентной заливки.
 /// </summary>
 public enum GradientMode : int
 {
  /// <summary>
  /// Нет градиента
  /// </summary>
  None = -1,
  /// <summary>
  /// Градиент с верхнего правого угла до нижнего левого.
  /// </summary>
  BackwardDiagonal = 3,
  /// <summary>
  /// Градиент с левого верхнего угла до нижнего правого
  /// </summary>
  ForwardDiagonal = 2,
  /// <summary>
  /// Градиент слева направо.
  /// </summary>
  Horizontal = 0,
  /// <summary>
  /// Градиент сверху вниз.
  /// </summary>
  Vertical = 1,
    /// <summary>
  /// Градиент с верхнего правого угла до нижнего левого.
  /// </summary>
  TrioBackwardDiagonal = 13,
  /// <summary>
  /// Градиент с левого верхнего угла до нижнего правого
  /// </summary>
  TrioForwardDiagonal = 12,
  /// <summary>
  /// Градиент слева направо.
  /// </summary>
  TrioHorizontal = 10,
  /// <summary>
  /// Градиент сверху вниз.
  /// </summary>
  TrioVertical = 11
 }
 #endregion << public enum GradientMode : int >>
}
