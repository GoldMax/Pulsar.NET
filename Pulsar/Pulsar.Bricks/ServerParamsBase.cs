using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Pulsar.Server
{
 /// <summary>
 /// Базовый класс параметров сервера
 /// </summary>
	public abstract class ServerParamsBase
	{
		private static string defReg = @"Software\EKS\SIM\";
		private int _port = 5021;
		private int _receiveTimeout = 5000;
		private int _sendTimeout = 5000;
		private int _receiveBufferSize = 8192;
		private int _sendBufferSize = 8192;

		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Опредяеляет серверный или клиентский режим выполнения.
		/// </summary>
		public static bool IsServer { get; protected set; }
		/// <summary>
		/// Корневой ключ реестра настроек
		/// </summary>
		public static string DefaultRegistryKey
		{
			get { return defReg; }
			set { defReg = value; }
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Порт
		/// </summary>
		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}
		/// <summary>
		/// Тайм-аут получения, мс.
		/// </summary>
		public int ReceiveTimeOut
		{
			get { return _receiveTimeout; }
			set { _receiveTimeout = value; }
		}
		/// <summary>
		/// Тайм-аут отправки, мс.
		/// </summary>
		public int SendTimeOut
		{
			get { return _sendTimeout; }
			set { _sendTimeout = value; }
		}
		/// <summary>
		/// Размер буфера получения.
		/// </summary>
		public int ReceiveBufferSize
		{
			get { return _receiveBufferSize; }
			set { _receiveBufferSize = value; }
		}
		/// <summary>
		/// Размер буфера отправки.
		/// </summary>
		public int SendBufferSize
		{
			get { return _sendBufferSize; }
			set { _sendBufferSize = value; }
		}
		#endregion << Properties >>
		//-------------------------------------------------------------------------------------
		#region << Registry Methods >>
		/// <summary>
		/// Открывает ключ реестра группы параметров.
		/// </summary>
		/// <param name="groupName">Имя группы параметров.</param>
		/// <param name="forWrite">Открывает ключ для записи, при необходимости создает его.</param>
		/// <returns>Открытый или созданный ключ.</returns>
		private static RegistryKey OpenGroup(string groupName, bool forWrite)
		{
			RegistryKey reg;
			if(IsServer)
				reg = Registry.LocalMachine;
			else
			 reg = Registry.CurrentUser;
			string s = defReg + (groupName ?? "");
			string[] keys = s.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
			for(int a = 0; a < keys.Length; a++)
			{
				if(reg.OpenSubKey(keys[a]) == null)
				{
					if(forWrite)
						reg = reg.CreateSubKey(keys[a]);
				}
				else
					reg = reg.OpenSubKey(keys[a], forWrite);
			}
			return reg;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Устанавливает значение параметра.
		/// </summary>
		/// <param name="groupName">Имя группы параметров.</param>
		/// <param name="param">Имя параметра.</param>
		/// <param name="value">Значение параметра.</param>
		public static void SetParam(string groupName, string param, object value)
		{
			if(value == null)
				return;
			RegistryKey reg = OpenGroup(groupName, true);
			RegistryValueKind type = RegistryValueKind.Unknown;
			switch(value.GetType().Name)
			{
				case "Byte": type = RegistryValueKind.DWord; break;
				case "SByte": type = RegistryValueKind.DWord; break;
				case "Int16": type = RegistryValueKind.DWord; break;
				case "UInt16": type = RegistryValueKind.DWord; break;
				case "Int32": type = RegistryValueKind.DWord; break;
				case "UInt32": type = RegistryValueKind.DWord; break;
				case "Int64": type = RegistryValueKind.QWord; break;
				case "UInt64": type = RegistryValueKind.QWord; break;
				case "Single": type = RegistryValueKind.DWord; break;
				case "Double": type = RegistryValueKind.QWord; break;
				case "Boolean": type = RegistryValueKind.DWord; break;
				case "Char": type = RegistryValueKind.String; break;
				case "IntPtr": type = RegistryValueKind.DWord; break;
				case "UIntPtr": type = RegistryValueKind.DWord; break;
			}
			if(type == RegistryValueKind.Unknown)
				reg.SetValue(param, value);
			else
				reg.SetValue(param, value, type);
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Возвращает значение параметра.
		/// </summary>
		/// <param name="groupName">Имя группы параметров.</param>
		/// <param name="param">Имя параметра.</param>
		/// <param name="defaultValue">Значение параметра по умолчанию.</param>
		/// <returns>Сохраненное значение параметра.</returns>
		public static object GetParam(string groupName, string param, object defaultValue)
		{
			RegistryKey reg = OpenGroup(groupName, false);
			if(reg == null)
				return defaultValue;
			return reg.GetValue(param, defaultValue);
		}
		#endregion << Registry Methods >>
		//-------------------------------------------------------------------------------------

	}
}
