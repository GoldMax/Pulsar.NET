using System;
using System.Collections.Generic;
using System.Text;

namespace Sim.Controls
{
 //**************************************************************************************
 #region << public enum GradientMode : int >>
 /// <summary>
 /// ������������ ����� ����������� �������.
 /// </summary>
 public enum GradientMode : int
 {
  /// <summary>
  /// ��� ���������
  /// </summary>
  None = -1,
  /// <summary>
  /// �������� � �������� ������� ���� �� ������� ������.
  /// </summary>
  BackwardDiagonal = 3,
  /// <summary>
  /// �������� � ������ �������� ���� �� ������� �������
  /// </summary>
  ForwardDiagonal = 2,
  /// <summary>
  /// �������� ����� �������.
  /// </summary>
  Horizontal = 0,
  /// <summary>
  /// �������� ������ ����.
  /// </summary>
  Vertical = 1,
    /// <summary>
  /// �������� � �������� ������� ���� �� ������� ������.
  /// </summary>
  TrioBackwardDiagonal = 13,
  /// <summary>
  /// �������� � ������ �������� ���� �� ������� �������
  /// </summary>
  TrioForwardDiagonal = 12,
  /// <summary>
  /// �������� ����� �������.
  /// </summary>
  TrioHorizontal = 10,
  /// <summary>
  /// �������� ������ ����.
  /// </summary>
  TrioVertical = 11
 }
 #endregion << public enum GradientMode : int >>
}
