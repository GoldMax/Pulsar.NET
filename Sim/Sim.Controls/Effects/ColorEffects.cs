using System;
using System.Drawing;

namespace Sim.Controls
{
 /// <summary>
 /// Эффекты над цветом.
 /// </summary>
 public static class ColorEffects
 {
  private static float Lerp(float start, float end, float amount)
  {
   float difference = end - start;
   float adjusted = difference * amount;
   return start + adjusted;
  }
  /*
   make red 50% lighter:
   Color.Red.Lerp( Color.White, 0.5f );

   make red 75% darker:
   Color.Red.Lerp( Color.Black, 0.75f );

   make white 10% bluer:
   Color.White.Lerp( Color.Blue, 0.1f );
   */
  /// <summary>
  /// Метод линейной интерполяции двух цаетов
  /// </summary>
  /// <param name="srcColor"></param>
  /// <param name="toColor"></param>
  /// <param name="amount"></param>
  /// <returns></returns>
  public static Color Lerp(Color srcColor, Color toColor, float amount)
  {
   // start colours as lerp-able floats
   float sr = srcColor.R, sg = srcColor.G, sb = srcColor.B;

   // end colours as lerp-able floats
   float er = toColor.R, eg = toColor.G, eb = toColor.B;

   // lerp the colours to get the difference
   byte r = (byte)Lerp(sr, er, amount),
        g = (byte)Lerp(sg, eg, amount),
        b = (byte)Lerp(sb, eb, amount);

   // return the new colour
   return Color.FromArgb(srcColor.A, r, g, b);
  }
  /// <summary>
  /// Метод осветления цаета
  /// </summary>
  /// <param name="color">Осветляемый цвет</param>
  /// <param name="pers">Процент осветления (0.0 - 1.0)</param>
  public static System.Drawing.Color Light(System.Drawing.Color color, float pers)
  {
   return Lerp(color, Color.White, pers);
  }
  /// <summary>
  /// Метод затемнения цаета
  /// </summary>
  /// <param name="color">Затемняемый цвет</param>
  /// <param name="pers">Процент затемнения (0.0 - 1.0)</param>
  public static System.Drawing.Color Dark(System.Drawing.Color color, float pers)
  {
   return Lerp(color, Color.Black, pers);
  }
 }
}
