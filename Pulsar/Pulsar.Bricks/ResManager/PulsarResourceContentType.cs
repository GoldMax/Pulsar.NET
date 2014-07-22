using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pulsar.Reflection;
using System.ComponentModel;

namespace Sim.Refs
{
 /// <summary>
 /// Перечисление типов контента ресурсов.
 /// </summary>
 [TypeConverter(typeof(EnumTypeConverter))]
 public enum PulsarResourceContentType : byte
 {
  /// <summary>
  /// Тип не известен
  /// </summary>
  [EnumItemDisplayName("(тип не известен)")]
  Unknown = 0,
  /// <summary>
  /// Docx
  /// </summary>
  [EnumItemDisplayName("Docx")]
  Docx = 1,
  /// <summary>
  /// Xlsx
  /// </summary>
  [EnumItemDisplayName("Xlsx")]
  Xslx = 2,
  /// <summary>
  /// Html
  /// </summary>
  [EnumItemDisplayName("Html")]
  Html = 3,
  /// <summary>
  /// Xml
  /// </summary>
  [EnumItemDisplayName("Xml")]
  Xml = 4,
  /// <summary>
  /// Png
  /// </summary>
  [EnumItemDisplayName("Png")]
  Png = 5,
  /// <summary>
  /// Jpeg
  /// </summary>
  [EnumItemDisplayName("Jpeg")]
  Jpeg = 6,
  /// <summary>
  /// Gif
  /// </summary>
  [EnumItemDisplayName("Gif")]
  Gif = 7,
  /// <summary>
  /// URL
  /// </summary>
  [EnumItemDisplayName("URL")]
  URL = 8,
  /// <summary>
  /// URL
  /// </summary>
  [EnumItemDisplayName("Коллекция изображений")]
  ImageCluster = 9
 }
}
