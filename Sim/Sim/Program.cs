using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Sim
{
	static class Program
	{
		internal static Dictionary<string, object> ConParams = new Dictionary<string, object>();
		private static string _defReg = @"Software\EKS\Sim\Connection";
		private static uint _version = 0;
		private static FormUpdateProgress formUpdate = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				Thread.CurrentThread.Name = "Main";
				//if(Environment.UserName != "goldaev")
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				formUpdate = new FormUpdateProgress();
				formUpdate.Show();
				formUpdate.StepText = "Определение параметров сервера ...";

				ConParams.Add("Address", null);
				ConParams.Add("Port", 5021);
				ConParams.Add("ConnectionType", 1);
				ConParams.Add("ReceiveTimeOut", 50000);
				ConParams.Add("ReceiveBufferSize", 8192);
				ConParams.Add("SendTimeOut", 50000); ;
				ConParams.Add("SendBufferSize", 8192);
				InitConParams();

				while(true)
					try
					{
						if(ConParams.ContainsKey("Address") == false || ConParams["Address"] == null)
							throw new Exception("Не удалось определить параметры подключения!");

						formUpdate.StepText = "Проверка наличия обновлений ...";
						if(Application.ExecutablePath.Contains("Debug") == false)
						{
							// попытка найти версию
							if(Application.StartupPath != GetFullPathProgram() || (_version = GetLastVersion()) == 0)
							{
								// запуск не из основной директории
								formUpdate.StepText = "Загрузка обновления ...";
								GetUpdate();
								formUpdate.StepText = "Создание ярлыков ...";
								CreateShortcut();
								formUpdate.StepText = "Перезагрузка ...";
								RestartAfterUpdate();
								return;
							}
							else
							{
								formUpdate.StepText = "Загрузка обновления ...";
								if(GetUpdate())
								{
									RestartAfterUpdate();
									return;
								}
								formUpdate.StepText = "Получение версии ...";
								_version = GetLastVersion();
							}
						}
						break;
					}
					catch(Exception err)
					{
						formUpdate.SetError(err.Message);
						if(formUpdate.DialogResult == DialogResult.Cancel)
							return;
						formUpdate.SetProgress();
					}

				formUpdate.Hide();
				formUpdate.Dispose();
				formUpdate = null;

				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
				Type t = Type.GetType("Sim.AuthForm, Sim.Shell");
				var authForm = (Form)Activator.CreateInstance(t, ConParams); //Activator.CreateInstance("Sim.Shell", "Sim.AuthForm").Unwrap();
				if(authForm.ShowDialog() != DialogResult.OK)
					return;
				t = Type.GetType("Sim.MainForm, Sim.Shell");
				var mainForm = (Form)Activator.CreateInstance("Sim.Shell", "Sim.MainForm").Unwrap();
				Application.Run(mainForm);
			}
			catch(Exception Err)
			{
				MessageBox.Show(Err.Message + Err.StackTrace, "Фатальная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		//-------------------------------------------------------------------------------------
		internal static void InitConParams(int type = 0)
		{
			if(type < 1)
			{
				type = (int)GetParam("ConnectionType", 1);
				ConParams["ConnectionType"]	= type;
				ConParams["ReceiveTimeOut"] = (int)GetParam("ReceiveTimeOut", 50000);
				ConParams["ReceiveBufferSize"] = (int)GetParam("ReceiveBufferSize", 8192);
				ConParams["SendTimeOut"] = (int)GetParam("SendTimeOut", 50000); ;
				ConParams["SendBufferSize"] = (int)GetParam("SendBufferSize", 8192);
			}
			switch(type)
			{
				case 1:
					{
						ConParams["Port"] = 5022;
						IPAddress[] ips = Dns.GetHostAddresses("");
						if(ips.Length != 0)
							foreach(IPAddress ip in ips)
							{
								if(ip.AddressFamily != AddressFamily.InterNetwork)
									continue;
								byte[] parts = ip.GetAddressBytes();
								if(parts[0] == 10 && parts[1] < 120)   //(parts[0] == 192 && parts[1] == 168) || >= 100
									ConParams["Address"] = "10.0.0.123";
								else if((parts[0] == 10 && parts[1] == 120) || (parts[0] == 192 && parts[1] == 168))
									ConParams["Address"] = "10.120.0.244";
							}
					} break;
				case 2:
					ConParams["Address"] = "10.0.0.123";
					ConParams["Port"] = 5022;
					break;
				case 3:
					ConParams["Address"] = (string)GetParam("Address", "127.0.0.1");
					ConParams["Port"]    =  (int)GetParam("Port", 5021);
					break;
				default:
					throw new Exception("Тип соединение не известен!");
			}
		}
		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if(args.Name.Contains("resources"))
				return null;
			else
			{
				string name = new AssemblyName(args.Name).Name;
				// TODO: проверить, возможно потребуется искать во вложенных директориях
				string path = _version == 0 ? string.Format("{0}\\{1}.dll", AppDomain.CurrentDomain.BaseDirectory,
					name) : string.Format("{0}\\{1}\\{2}.dll", AppDomain.CurrentDomain.BaseDirectory, _version,
					name);
			 System.Diagnostics.Debug.WriteLine(path);
				return Assembly.LoadFrom(path);
			}
		}
		//-------------------------------------------------------------------------------------
		#region << Registry >>
		/// <summary>
		/// Возвращает значение параметра.
		/// </summary>
		/// <param name="param">Имя параметра.</param>
		/// <param name="defaultValue">Значение параметра по умолчанию.</param>
		/// <returns>Сохраненное значение параметра.</returns>
		internal static object GetParam(string param, object defaultValue)
		{
			RegistryKey reg = Registry.CurrentUser;
			using(reg = reg.OpenSubKey(_defReg, false))
			{
				if(reg == null)
					return defaultValue;
				return reg.GetValue(param, defaultValue);
			}
		}
		/// <summary>
		/// Возвращает значение параметра.
		/// </summary>
		internal static void SaveParams()
		{
			RegistryKey reg = Registry.CurrentUser;
			//string[] ss = _defReg.Split(new [] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
			//foreach(string s in ss)
			// reg = reg.Cr
			using(reg = reg.CreateSubKey(_defReg))
			{
				foreach(var i in ConParams)
					reg.SetValue(i.Key, i.Value, i.Value is string ? RegistryValueKind.String : RegistryValueKind.DWord);
			}
		}
		#endregion << Registry >>
		//-------------------------------------------------------------------------------------
		#region << Update Methods >>
		/// Получает полный путь к программе в локальном каталоге пользователя
		/// </summary>
		/// <returns>Полный путь</returns>
		private static string GetFullPathProgram()
		{
			return string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(
				Environment.SpecialFolder.LocalApplicationData), "EKS", Application.ProductName);
		}
		/// <summary>
		/// Получает обновление с сервера
		/// </summary>
		/// <returns>Признак, было ли выполнено обновление</returns>
		private static bool GetUpdate()
		{
			TcpClient cl = null;
			NetworkStream ns = null;

			try
			{
				cl = new TcpClient();
				cl.ReceiveTimeout = (int)ConParams["ReceiveTimeOut"];
				cl.ReceiveBufferSize = (int)ConParams["ReceiveBufferSize"];
				cl.SendTimeout = (int)ConParams["SendTimeOut"];
				cl.SendBufferSize = (int)ConParams["SendBufferSize"];
				Application.DoEvents();
				cl.Connect((string)ConParams["Address"], (int)ConParams["Port"]);
				ns = cl.GetStream();

				// Посылаем признак запроса на обновление
				ns.WriteByte(1);
				ns.Write(BitConverter.GetBytes(_version), 0, sizeof(uint));

				uint version = BitConverter.ToUInt32(ns.ReadBytes(sizeof(uint)), 0);
				if(version == 0)
					return false;

				// формат информации о файле 
				// путь
				// имя файла
				// файл

				string path, fileName;
				byte[] buf = new byte[cl.ReceiveBufferSize];
				uint readBytes = 0;
				int count;
				uint size;
				ulong fileSize;

				string[] versions = GetDirectoriesVesions();
				foreach(string v in versions)
					Directory.Delete(GetFullPathProgram() + "\\" + v, true);

				string newPath = GetFullPathProgram();
				string verPath = newPath + "\\" + version;
				string[] topFiles = { "sim.exe", "sim.pdb", "sim.exe.config" };
				while(true)
				{
					Application.DoEvents();
					count = BitConverter.ToInt32(ns.ReadBytes(sizeof(int)), 0);
					if(count == -1)
						break;
					if(count > 0)
						path = System.Text.UTF8Encoding.UTF8.GetString(ns.ReadBytes((uint)count));
					else
						path = null;
					size = BitConverter.ToUInt32(ns.ReadBytes(sizeof(uint)), 0);
					fileName = System.Text.UTF8Encoding.UTF8.GetString(ns.ReadBytes(size));
					fileSize = BitConverter.ToUInt64(ns.ReadBytes(sizeof(ulong)), 0);

					if(topFiles.Contains(fileName.ToLower()))
					{
						path = newPath;
					 fileName =  "_" + fileName;
					}
					else
					{
						if(String.IsNullOrEmpty(path))
						 path = verPath;
						else
						 path = verPath + "\\" + path;
					}

					if(!Directory.Exists(path))
						Directory.CreateDirectory(path);
					using(FileStream file = File.Open(path+"\\"+fileName, FileMode.Create, FileAccess.Write))
					{
						readBytes = 0;
						while((count = ns.Read(buf, 0, (ulong)(readBytes + cl.ReceiveBufferSize) <= fileSize
								? cl.ReceiveBufferSize : (int)(fileSize - readBytes))) != 0)
						{
							file.Write(buf, 0, count);
							readBytes += (uint)count;
							Application.DoEvents();
						}
					}
				}
			}
			finally
			{
				if(ns != null)
					ns.Close();
				if(cl != null)
					cl.Close();
			}

			return true;
		}
		/// <summary>
		/// Получает директории версий
		/// </summary>
		/// <returns>Возвращает список директорий с версиями программы</returns>
		private static string[] GetDirectoriesVesions()
		{
			if(!Directory.Exists(GetFullPathProgram()))
				return new string[0];
			string[] dirs = Directory.GetDirectories(GetFullPathProgram());
			List<string> dirsVersions = new List<string>();
			int ver;
			for(int i = 0; i < dirs.Length; i++)
				if(int.TryParse(new DirectoryInfo(dirs[i]).Name, out ver))
					dirsVersions.Add(ver.ToString());

			return dirsVersions.ToArray();
		}
		/// <summary>
		/// Получает номер последней версии
		/// </summary>
		/// <returns>Версия</returns>
		private static uint GetLastVersion()
		{
			string[] dirVersions = GetDirectoriesVesions();
			uint ver = 0, currVer;

			for(int i = 0; i < dirVersions.Length; i++)
				if(uint.TryParse(dirVersions[i], out currVer))
					if(currVer > ver)
						ver = currVer;
			return ver;
		}
		#endregion << Update Methods >>

		#region << Other >>
		/// <summary>
		/// Создает ярлык для программы
		/// </summary>
		private static void CreateShortcut()
		{
			if(!File.Exists(Environment.SpecialFolder.DesktopDirectory + "\\SIM.lnk"))
				ShortCut.Create(GetFullPathProgram() + "\\" + Application.ProductName + ".exe",
					Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\"
					+ "SIM.lnk",
					"", "", GetFullPathProgram() + "\\" + Application.ProductName + ".exe");
		}
		/// <summary>
		/// Выполняет перезапуск программы после обновления
		/// </summary>
		private static void RestartAfterUpdate()
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = "cmd.exe",
				WorkingDirectory = GetFullPathProgram(),
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = string.Format("/C ping -n 2 127.0.0.1 > nul && del /f /q {0}.*"
					+ " && ren _{0}.* {0}.* && start {0}.exe", Application.ProductName)
			});
			Application.Exit();
		}
		/// <summary>
		/// Читает и возвращает заданное количество байт. В случае ошибки выбрасывает исключение.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="count">Количество байт для чтения.</param>
		/// <returns></returns>
		public static byte[] ReadBytes(this Stream s, uint count)
		{
			if(count <= 0)
				return new byte[0];
			byte[] res = new byte[count];
			int shift = 0;
			do
			{
				int step = s.Read(res, shift, (int)count - shift);
				if(step == 0)
					throw new Exception("Не удалось прочитать из потока заданое количество байт!");
				shift += step;
				//if (shift != count)
				// System.Threading.Thread.Sleep(50);
			} while(count != shift);
			return res;
		}

		#endregion << Other >>
	}

	#region << ShellLink >>

	static class ShellLink
	{
		[ComImport, Guid("000214F9-0000-0000-C000-000000000046"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		internal interface IShellLinkW
		{
			[PreserveSig]
			int GetPath([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszFile,
				int cch, ref IntPtr pfd, uint fFlags);

			[PreserveSig]
			int GetIDList(out IntPtr ppidl);

			[PreserveSig]
			int SetIDList(IntPtr pidl);

			[PreserveSig]
			int GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszName, int cch);

			[PreserveSig]
			int SetDescription([MarshalAs(UnmanagedType.LPWStr)]string pszName);

			[PreserveSig]
			int GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszDir, int cch);

			[PreserveSig]
			int SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)]string pszDir);

			[PreserveSig]
			int GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszArgs, int cch);

			[PreserveSig]
			int SetArguments([MarshalAs(UnmanagedType.LPWStr)]string pszArgs);

			[PreserveSig]
			int GetHotkey(out ushort pwHotkey);

			[PreserveSig]
			int SetHotkey(ushort wHotkey);

			[PreserveSig]
			int GetShowCmd(out int piShowCmd);

			[PreserveSig]
			int SetShowCmd(int iShowCmd);

			[PreserveSig]
			int GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)]StringBuilder pszIconPath, int cch, out int piIcon);

			[PreserveSig]
			int SetIconLocation([MarshalAs(UnmanagedType.LPWStr)]string pszIconPath, int iIcon);

			[PreserveSig]
			int SetRelativePath([MarshalAs(UnmanagedType.LPWStr)]string pszPathRel, uint dwReserved);

			[PreserveSig]
			int Resolve(IntPtr hwnd, uint fFlags);

			[PreserveSig]
			int SetPath([MarshalAs(UnmanagedType.LPWStr)]string pszFile);
		}

		[ComImport, Guid("00021401-0000-0000-C000-000000000046"), ClassInterface(ClassInterfaceType.None)]
		private class shl_link { }

		internal static IShellLinkW CreateShellLink()
		{
			return (IShellLinkW)(new shl_link());
		}
	}

	/// <summary>
	/// Класс с методами по созданию ярлыка
	/// </summary>
	public static class ShortCut
	{
		/// <summary>
		/// Создает ярлык для программы
		/// </summary>
		/// <param name="PathToFile">Путь к файлу программы</param>
		/// <param name="PathToLink">Путь ярлыка</param>
		/// <param name="Arguments">Аргументы запуска</param>
		/// <param name="Description">Описание</param>
		/// <param name="PathToIcon">Путь к иконке для ярлыка</param>
		public static void Create(string PathToFile, string PathToLink,
			string Arguments, string Description, string PathToIcon = null)
		{
			ShellLink.IShellLinkW shlLink = ShellLink.CreateShellLink();

			Marshal.ThrowExceptionForHR(shlLink.SetDescription(Description));
			Marshal.ThrowExceptionForHR(shlLink.SetPath(PathToFile));
			Marshal.ThrowExceptionForHR(shlLink.SetArguments(Arguments));
			Marshal.ThrowExceptionForHR(shlLink.SetIconLocation(PathToIcon, 0));

			((System.Runtime.InteropServices.ComTypes.IPersistFile)shlLink).Save(PathToLink, false);
		}
	}

	#endregion << ShellLing >>
}
