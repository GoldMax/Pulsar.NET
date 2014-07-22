using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pulsar.Security
{
	#region << public enum SecurityAccess : byte >>
	/// <summary>
	/// Перечисление результатов проверок доступа
	/// </summary>
	[Flags]
	public enum SecurityAccess : byte
	{
		/// <summary>
		/// Доступ не установлен.
		/// </summary>
		NotSet = 0,
		/// <summary>
		/// Доступ имеют дочерние элементы
		/// </summary>
		Browse = 1,
		/// <summary>
		/// Доступ установлен
		/// </summary>
		Set = 2,
		/// <summary>
		/// Доступ запрещен
		/// </summary>
		Denied = 128
	}
	#endregion << public enum SecurityAccess : byte >>
	//**************************************************************************************
	#region << public class SecurityItemAccess >>
	/// <summary>
	/// Класс набора доступов элемента.
	/// </summary>
	public class SecurityItemAccess
	{
		private SecurityAccess browse = SecurityAccess.NotSet;
		private SecurityAccess L1 = SecurityAccess.NotSet;
		private SecurityAccess L2 = SecurityAccess.NotSet;
		private SecurityAccess L3 = SecurityAccess.NotSet;
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		#region << Properties >>
		/// <summary>
		/// Доступ на просмотр
		/// </summary>
		public SecurityAccess Browse
		{
			get { return browse; }
			set
			{
				if((byte)value > (byte)browse)
					browse = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Доступ Уровня 1
		/// </summary>
		public SecurityAccess Level1
		{
			get { return L1; }
			set
			{
				if((byte)value > (byte)L1)
					L1 = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Доступ Уровня 2
		/// </summary>
		public SecurityAccess Level2
		{
			get { return L2; }
			set
			{
				//if(!(add == PulsarAccess.Denied || value == PulsarAccess.NotSet))
				if((byte)value > (byte)L2)
					L2 = value;
			}
		}
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Доступ Уровня 3
		/// </summary>
		public SecurityAccess Level3
		{
			get { return L3; }
			set
			{
				if((byte)value > (byte)L3)
					L3 = value;
			}
		}
		#endregion << Properties >>
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SecurityItemAccess()
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("Browse:[{0}] L1:[{1}] L2:[{2}] L3:[{3}]", Browse.ToString(), Level1.ToString(),
																											Level2.ToString(), Level3.ToString());
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод объединения (сложения) наборов доступов.
		/// </summary>
		/// <param name="ia">Добавляемый набор доступов.</param>
		public void Join(SecurityItemAccess ia)
		{
			if(ia.Browse != SecurityAccess.Browse)
				this.Browse = ia.Browse;
			if(ia.Level1 != SecurityAccess.Browse)
				this.Level1 = ia.Level1;
			if(ia.Level2 != SecurityAccess.Browse)
				this.Level2 = ia.Level2;
			if(ia.Level3 != SecurityAccess.Browse)
				this.Level3 = ia.Level3;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Метод инициализации набора доступов из указанного набора.
		/// </summary>
		/// <param name="ia">Набор, из которого проводится инициализация.</param>
		public void InitFrom(SecurityItemAccess ia)
		{
			if(Browse == SecurityAccess.NotSet && (ia.Browse == SecurityAccess.Set || ia.Browse == SecurityAccess.Browse))
				this.Browse = SecurityAccess.Browse;
			if(Level1 == SecurityAccess.NotSet && (ia.Level1 == SecurityAccess.Set || ia.Level1 == SecurityAccess.Browse))
				this.Level1 = SecurityAccess.Browse;
			if(Level2 == SecurityAccess.NotSet && (ia.Level2 == SecurityAccess.Set || ia.Level2 == SecurityAccess.Browse))
				this.Level2 = SecurityAccess.Browse;
			if(Level3 == SecurityAccess.NotSet && (ia.Level3 == SecurityAccess.Set || ia.Level3 == SecurityAccess.Browse))
				this.Level3 = SecurityAccess.Browse;
		}
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Сбрасывает набор поступов в неопределенное состояние.
		/// </summary>
		public void Reset()
		{
			browse = SecurityAccess.NotSet;
			L1 = SecurityAccess.NotSet;
			L2 = SecurityAccess.NotSet;
			L3 = SecurityAccess.NotSet;
		}
		#endregion << Methods >>
		//-------------------------------------------------------------------------------------
	}
	#endregion << public class SecurityItemAccess >>

}
