using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Pulsar
{
	//*************************************************************************************
	#region << public interface IVersionedObject >>
	/// <summary>
	/// Класс интерфейса объета, содержащего версию.
	/// </summary>
	public interface IVersionedObject
	{
		/// <summary>
		/// Версия объекта.
		/// </summary>
		uint Version { get; }
		/// <summary>
		/// Проверяет версию объекта. 
		/// Если версия одинаковая, возвращает сам объект, иначе выбрасывает исключение защиты.
		/// </summary>
		/// <param name="version">Проверяемая версия</param>
		/// <returns></returns>
		object CheckVersion(uint version);
	} 
	#endregion << public interface IVersionedObject >>
	//**************************************************************************************
	#region << public interface ICoList >>
	/// <summary>
	/// Ковариантный интерфейс списка
	/// </summary>
	/// <typeparam name="Tret">Тип возвращаемых значений (тип списка)</typeparam>
	/// <typeparam name="Targ">Тип аргументов</typeparam>
	public interface ICoList<out Tret, in Targ> : IEnumerable<Tret>
	{
		#pragma warning disable
		Tret this[int index] { get; }
		int Count { get; }

		int IndexOf(Targ item);
		bool Contains(Targ item);
		IEnumerator<Tret> GetEnumerator();

		int Add(Targ item);
		void Insert(int index, Targ item);
		void Remove(Targ item);
		void RemoveAt(int index);
		void Clear();
	#pragma warning restore
	}
	/// <summary>
	/// Ковариантный интерфейс списка
	/// </summary>
	/// <typeparam name="T">Тип возвращаемых значений (тип списка)</typeparam>
	public interface ICoList<out T> : ICoList<T, object> { }
	#endregion << public interface ICoList >>
	//**************************************************************************************
	#region << public interface ICoDictionary >>
	/// <summary>
	/// Ковариантный интерфейс коллекции.
	/// </summary>
	public interface ICoDictionary<out TKeyRet, in TKeyArg, out TValRet, in TValArg> : IEnumerable
	{
		/// <summary>
		/// Возвращает коллекцию ключей.
		/// </summary>
		IEnumerable<TKeyRet> Keys { get; }
		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		/// <summary>
		/// Возвращает коллекцию значений.
		/// </summary>
		IEnumerable<TValRet> Values { get; }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Добавляет элемент в коллекцию.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <param name="value">Значение элемента.</param>
		void Add(TKeyArg key, TValArg value);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяет принадлежность коллекции элемента с заданным ключем.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		bool ContainsKey(TKeyArg key);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Удаляет элемент с указанным ключем из коллекции.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		void Remove(TKeyArg key);
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Определяе значение элемента коллекции по ключу.
		/// </summary>
		/// <param name="key">Ключ элемента.</param>
		/// <returns></returns>
		TValRet this[TKeyArg key] { get; }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Количество элементов.
		/// </summary>
		int Count { get; }
		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Очищает коллекцию
		/// </summary>
		void Clear();
	}
	/// <summary>
	/// Ковариантный интерфейс коллекции.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TVal"></typeparam>
	public interface ICoDictionary<out TKey, out TVal> : ICoDictionary<TKey, object, TVal, object>
	{

	}
	#endregion << public interface ICoDictionary >>
}
