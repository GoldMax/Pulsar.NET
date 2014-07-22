using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace Pulsar.Serialization
{
#pragma warning disable 1591
	internal class NoThrowDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>,
																																																		IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection,
																																																		IEnumerable, ISerializable, IDeserializationCallback
		// where TKey : class
	{
		#region Classes
		private struct Entry
		{
			public int hashCode;
			public int next;
			public TKey key;
			public TValue value;
		}
		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
		{
			internal const int DictEntry = 1;
			internal const int KeyValuePair = 2;
			private NoThrowDictionary<TKey, TValue> dictionary;
			private int version;
			private int index;
			private KeyValuePair<TKey, TValue> current;
			private int getEnumeratorRetType;
			public KeyValuePair<TKey, TValue> Current
			{
				[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
				get
				{
					return this.current;
				}
			}
			object IEnumerator.Current
			{
				get
				{
					if(this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if(this.getEnumeratorRetType == 1)
					{
						return new DictionaryEntry(this.current.Key, this.current.Value);
					}
					return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
				}
			}
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if(this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return new DictionaryEntry(this.current.Key, this.current.Value);
				}
			}
			object IDictionaryEnumerator.Key
			{
				get
				{
					if(this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Key;
				}
			}
			object IDictionaryEnumerator.Value
			{
				get
				{
					if(this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Value;
				}
			}
			internal Enumerator(NoThrowDictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this.dictionary = dictionary;
				this.version = dictionary.version;
				this.index = 0;
				this.getEnumeratorRetType = getEnumeratorRetType;
				this.current = default(KeyValuePair<TKey, TValue>);
			}
			public bool MoveNext()
			{
				if(this.version != this.dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				while(this.index < this.dictionary.count)
				{
					if(this.dictionary.entries[this.index].hashCode >= 0)
					{
						this.current = new KeyValuePair<TKey, TValue>(this.dictionary.entries[this.index].key, this.dictionary.entries[this.index].value);
						this.index++;
						return true;
					}
					this.index++;
				}
				this.index = this.dictionary.count + 1;
				this.current = default(KeyValuePair<TKey, TValue>);
				return false;
			}
			public void Dispose()
			{
			}
			void IEnumerator.Reset()
			{
				if(this.version != this.dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.current = default(KeyValuePair<TKey, TValue>);
			}
		}
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
		{
			[Serializable]
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
			{
				private NoThrowDictionary<TKey, TValue> dictionary;
				private int index;
				private int version;
				private TKey currentKey;
				public TKey Current
				{
					[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
					get
					{
						return this.currentKey;
					}
				}
				object IEnumerator.Current
				{
					get
					{
						if(this.index == 0 || this.index == this.dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.currentKey;
					}
				}
				internal Enumerator(NoThrowDictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					this.version = dictionary.version;
					this.index = 0;
					this.currentKey = default(TKey);
				}
				public void Dispose()
				{
				}
				public bool MoveNext()
				{
					if(this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while(this.index < this.dictionary.count)
					{
						if(this.dictionary.entries[this.index].hashCode >= 0)
						{
							this.currentKey = this.dictionary.entries[this.index].key;
							this.index++;
							return true;
						}
						this.index++;
					}
					this.index = this.dictionary.count + 1;
					this.currentKey = default(TKey);
					return false;
				}
				void IEnumerator.Reset()
				{
					if(this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					this.index = 0;
					this.currentKey = default(TKey);
				}
			}
			private NoThrowDictionary<TKey, TValue> dictionary;
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}
			bool ICollection<TKey>.IsReadOnly
			{
				get
				{
					return true;
				}
			}
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}
			public KeyCollection(NoThrowDictionary<TKey, TValue> dictionary)
			{
				if(dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}
			public NoThrowDictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new NoThrowDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}
			public void CopyTo(TKey[] array, int index)
			{
				if(array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if(index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if(array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this.dictionary.count;
				NoThrowDictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				for(int i = 0; i < count; i++)
				{
					if(entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}
			void ICollection<TKey>.Add(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}
			void ICollection<TKey>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}
			bool ICollection<TKey>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}
			bool ICollection<TKey>.Remove(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}
			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new NoThrowDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new NoThrowDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}
			void ICollection.CopyTo(Array array, int index)
			{
				if(array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if(array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				if(array.GetLowerBound(0) != 0)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
				}
				if(index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if(array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if(array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if(array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = this.dictionary.count;
				NoThrowDictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				try
				{
					for(int i = 0; i < count; i++)
					{
						if(entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch(ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}
		}
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
		{
			[Serializable]
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
			{
				private NoThrowDictionary<TKey, TValue> dictionary;
				private int index;
				private int version;
				private TValue currentValue;
				public TValue Current
				{
					[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
					get
					{
						return this.currentValue;
					}
				}
				object IEnumerator.Current
				{
					get
					{
						if(this.index == 0 || this.index == this.dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.currentValue;
					}
				}
				internal Enumerator(NoThrowDictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					this.version = dictionary.version;
					this.index = 0;
					this.currentValue = default(TValue);
				}
				public void Dispose()
				{
				}
				public bool MoveNext()
				{
					if(this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while(this.index < this.dictionary.count)
					{
						if(this.dictionary.entries[this.index].hashCode >= 0)
						{
							this.currentValue = this.dictionary.entries[this.index].value;
							this.index++;
							return true;
						}
						this.index++;
					}
					this.index = this.dictionary.count + 1;
					this.currentValue = default(TValue);
					return false;
				}
				void IEnumerator.Reset()
				{
					if(this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					this.index = 0;
					this.currentValue = default(TValue);
				}
			}
			private NoThrowDictionary<TKey, TValue> dictionary;
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}
			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}
			public ValueCollection(NoThrowDictionary<TKey, TValue> dictionary)
			{
				if(dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}
			public NoThrowDictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new NoThrowDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}
			public void CopyTo(TValue[] array, int index)
			{
				if(array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if(index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if(array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this.dictionary.count;
				NoThrowDictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				for(int i = 0; i < count; i++)
				{
					if(entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}
			void ICollection<TValue>.Add(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}
			bool ICollection<TValue>.Remove(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}
			void ICollection<TValue>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}
			bool ICollection<TValue>.Contains(TValue item)
			{
				return this.dictionary.ContainsValue(item);
			}
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new NoThrowDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new NoThrowDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}
			void ICollection.CopyTo(Array array, int index)
			{
				if(array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if(array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				if(array.GetLowerBound(0) != 0)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
				}
				if(index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if(array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if(array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if(array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = this.dictionary.count;
				NoThrowDictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				try
				{
					for(int i = 0; i < count; i++)
					{
						if(entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch(ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}
		}
		#endregion Classes
		private int[] buckets;
		private NoThrowDictionary<TKey, TValue>.Entry[] entries;
		private int count;
		private int version;
		private int freeList;
		private int freeCount;
		private IEqualityComparer<TKey> comparer;
		private NoThrowDictionary<TKey, TValue>.KeyCollection keys;
		private NoThrowDictionary<TKey, TValue>.ValueCollection values;
		private object _syncRoot;
		private SerializationInfo m_siInfo;
		private const string VersionName = "Version";
		private const string HashSizeName = "HashSize";
		private const string KeyValuePairsName = "KeyValuePairs";
		private const string ComparerName = "Comparer";
		public IEqualityComparer<TKey> Comparer
		{
			[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
			get
			{
				return this.comparer;
			}
		}
		public int Count
		{
			get
			{
				return this.count - this.freeCount;
			}
		}
		public NoThrowDictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if(this.keys == null)
				{
					this.keys = new NoThrowDictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				if(this.keys == null)
				{
					this.keys = new NoThrowDictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}
		public NoThrowDictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if(this.values == null)
				{
					this.values = new NoThrowDictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				if(this.values == null)
				{
					this.values = new NoThrowDictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}
		public TValue this[TKey key]
		{
			get
			{
				int num = this.FindEntry(key);
				if(num >= 0)
					return this.entries[num].value;
				return default(TValue);
			}
			[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
			set
			{
				this.Insert(key, value, false);
			}
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}
		object ICollection.SyncRoot
		{
			get
			{
				if(this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}
		ICollection IDictionary.Keys
		{
			[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
			get
			{
				return this.Keys;
			}
		}
		ICollection IDictionary.Values
		{
			[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
			get
			{
				return this.Values;
			}
		}
		object IDictionary.this[object key]
		{
			get { return this[(TKey)key]; }
			set { this[(TKey)key] = (TValue)value; }
		}


		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public NoThrowDictionary()
			: this(0, null)
		{
		}
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public NoThrowDictionary(int capacity)
			: this(capacity, null)
		{
		}
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public NoThrowDictionary(IEqualityComparer<TKey> comparer)
			: this(0, comparer)
		{
		}
		public NoThrowDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			if(capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			if(capacity > 0)
			{
				this.Initialize(capacity);
			}
			this.comparer = (comparer ?? EqualityComparer<TKey>.Default);
		}
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public NoThrowDictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}
		public NoThrowDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: this((dictionary != null) ? dictionary.Count : 0, comparer)
		{
			if(dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			foreach(KeyValuePair<TKey, TValue> current in dictionary)
			{
				this.Add(current.Key, current.Value);
			}
		}
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		protected NoThrowDictionary(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			this.Insert(key, value, true);
		}
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public TValue Add(TKey key, TValue value)
		{
			return this.Insert(key, value, true);
		}
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Insert(keyValuePair.Key, keyValuePair.Value, true);
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			return num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value);
		}
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			return this.Remove(keyValuePair.Key);
		}
		public void Clear()
		{
			if(this.count > 0)
			{
				for(int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				Array.Clear(this.entries, 0, this.count);
				this.freeList = -1;
				this.count = 0;
				this.freeCount = 0;
				this.version++;
			}
		}
		public bool ContainsKey(TKey key)
		{
			return this.FindEntry(key) >= 0;
		}
		public bool ContainsValue(TValue value)
		{
			if(value == null)
			{
				for(int i = 0; i < this.count; i++)
				{
					if(this.entries[i].hashCode >= 0 && this.entries[i].value == null)
					{
						return true;
					}
				}
			}
			else
			{
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				for(int j = 0; j < this.count; j++)
				{
					if(this.entries[j].hashCode >= 0 && @default.Equals(this.entries[j].value, value))
					{
						return true;
					}
				}
			}
			return false;
		}
		[SecuritySafeCritical]
		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if(array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if(index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if(array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			int num = this.count;
			NoThrowDictionary<TKey, TValue>.Entry[] array2 = this.entries;
			for(int i = 0; i < num; i++)
			{
				if(array2[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(array2[i].key, array2[i].value);
				}
			}
		}
		public NoThrowDictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new NoThrowDictionary<TKey, TValue>.Enumerator(this, 2);
		}
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new NoThrowDictionary<TKey, TValue>.Enumerator(this, 2);
		}
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if(info == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
			}
			info.AddValue("Version", this.version);
			info.AddValue("Comparer", this.comparer, typeof(IEqualityComparer<TKey>));
			info.AddValue("HashSize", (this.buckets == null) ? 0 : this.buckets.Length);
			if(this.buckets != null)
			{
				KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("KeyValuePairs", array, typeof(KeyValuePair<TKey, TValue>[]));
			}
		}
		private int FindEntry(TKey key)
		{
			if(key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if(this.buckets != null)
			{
				int num = this.comparer.GetHashCode(key) & 2147483647;
				for(int i = this.buckets[num % this.buckets.Length]; i >= 0; i = this.entries[i].next)
				{
					if(this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
					{
						return i;
					}
				}
			}
			return -1;
		}
		private void Initialize(int capacity)
		{
			int prime = HashHelpers.GetPrime(capacity);
			this.buckets = new int[prime];
			for(int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i] = -1;
			}
			this.entries = new NoThrowDictionary<TKey, TValue>.Entry[prime];
			this.freeList = -1;
		}
		private TValue Insert(TKey key, TValue value, bool add)
		{
			if(key == null)
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			if(this.buckets == null)
			{
				this.Initialize(238);
			}
			int num = this.comparer.GetHashCode(key) & 2147483647;
			int num2 = num % this.buckets.Length;
			for(int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
			{
				if(this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
				{
					if(add)
						return this.entries[i].value;
					this.entries[i].value = value;
					this.version++;
					return value;
				}
			}
			int num3;
			if(this.freeCount > 0)
			{
				num3 = this.freeList;
				this.freeList = this.entries[num3].next;
				this.freeCount--;
			}
			else
			{
				if(this.count == this.entries.Length)
				{
					this.Resize();
					num2 = num % this.buckets.Length;
				}
				num3 = this.count;
				this.count++;
			}
			this.entries[num3].hashCode = num;
			this.entries[num3].next = this.buckets[num2];
			this.entries[num3].key = key;
			this.entries[num3].value = value;
			this.buckets[num2] = num3;
			this.version++;
			return value;
		}
		public virtual void OnDeserialization(object sender)
		{
			if(this.m_siInfo == null)
			{
				return;
			}
			int @int = this.m_siInfo.GetInt32("Version");
			int int2 = this.m_siInfo.GetInt32("HashSize");
			this.comparer = (IEqualityComparer<TKey>)this.m_siInfo.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
			if(int2 != 0)
			{
				this.buckets = new int[int2];
				for(int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				this.entries = new NoThrowDictionary<TKey, TValue>.Entry[int2];
				this.freeList = -1;
				KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])this.m_siInfo.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
				if(array == null)
				{
					ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingKeys);
				}
				for(int j = 0; j < array.Length; j++)
				{
					if(array[j].Key == null)
					{
						ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_NullKey);
					}
					this.Insert(array[j].Key, array[j].Value, true);
				}
			}
			else
			{
				this.buckets = null;
			}
			this.version = @int;
			this.m_siInfo = null;
		}
		private void Resize()
		{
			int prime = HashHelpers.GetPrime(this.count * 2);
			int[] array = new int[prime];
			for(int i = 0; i < array.Length; i++)
			{
				array[i] = -1;
			}
			NoThrowDictionary<TKey, TValue>.Entry[] array2 = new NoThrowDictionary<TKey, TValue>.Entry[prime];
			Array.Copy(this.entries, 0, array2, 0, this.count);
			for(int j = 0; j < this.count; j++)
			{
				int num = array2[j].hashCode % prime;
				array2[j].next = array[num];
				array[num] = j;
			}
			this.buckets = array;
			this.entries = array2;
		}
		public bool Remove(TKey key)
		{
			if(key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if(this.buckets != null)
			{
				int num = this.comparer.GetHashCode(key) & 2147483647;
				int num2 = num % this.buckets.Length;
				int num3 = -1;
				for(int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
				{
					if(this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
					{
						if(num3 < 0)
						{
							this.buckets[num2] = this.entries[i].next;
						}
						else
						{
							this.entries[num3].next = this.entries[i].next;
						}
						this.entries[i].hashCode = -1;
						this.entries[i].next = this.freeList;
						this.entries[i].key = default(TKey);
						this.entries[i].value = default(TValue);
						this.freeList = i;
						this.freeCount++;
						this.version++;
						return true;
					}
					num3 = i;
				}
			}
			return false;
		}
		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = this.FindEntry(key);
			if(num >= 0)
			{
				value = this.entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}
		internal TValue GetValueOrDefault(TKey key)
		{
			int num = this.FindEntry(key);
			if(num >= 0)
			{
				return this.entries[num].value;
			}
			return default(TValue);
		}
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this.CopyTo(array, index);
		}
		void ICollection.CopyTo(Array array, int index)
		{
			if(array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if(array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if(array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if(index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if(array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if(array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			if(array is DictionaryEntry[])
			{
				DictionaryEntry[] array3 = array as DictionaryEntry[];
				NoThrowDictionary<TKey, TValue>.Entry[] array4 = this.entries;
				for(int i = 0; i < this.count; i++)
				{
					if(array4[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(array4[i].key, array4[i].value);
					}
				}
				return;
			}
			object[] array5 = array as object[];
			if(array5 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				int num = this.count;
				NoThrowDictionary<TKey, TValue>.Entry[] array6 = this.entries;
				for(int j = 0; j < num; j++)
				{
					if(array6[j].hashCode >= 0)
					{
						array5[index++] = new KeyValuePair<TKey, TValue>(array6[j].key, array6[j].value);
					}
				}
			}
			catch(ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new NoThrowDictionary<TKey, TValue>.Enumerator(this, 2);
		}
		private static bool IsCompatibleKey(object key)
		{
			if(key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}
		void IDictionary.Add(object key, object value)
		{
			if(key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
			try
			{
				TKey key2 = (TKey)key;
				try
				{
					this.Add(key2, (TValue)value);
				}
				catch(InvalidCastException)
				{
					ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
				}
			}
			catch(InvalidCastException)
			{
				ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
			}
		}
		bool IDictionary.Contains(object key)
		{
			return NoThrowDictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)key);
		}
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new NoThrowDictionary<TKey, TValue>.Enumerator(this, 1);
		}
		void IDictionary.Remove(object key)
		{
			if(NoThrowDictionary<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)key);
			}
		}
	}
	#region Helpers
	internal static class ThrowHelper
	{
		[SecuritySafeCritical]
		internal static void ThrowArgumentOutOfRangeException()
		{
			throw new ArgumentOutOfRangeException();
		}
		[SecuritySafeCritical]
		internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
		{
			throw new ArgumentException();
		}
		[SecuritySafeCritical]
		internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
		{
			throw new ArgumentException();
		}
		[SecuritySafeCritical]
		internal static void ThrowKeyNotFoundException()
		{
			throw new KeyNotFoundException();
		}
		[SecuritySafeCritical]
		internal static void ThrowArgumentException(ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
		{
			throw new ArgumentException();
		}
		[SecuritySafeCritical]
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
		}
		[SecuritySafeCritical]
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
		}
		[SecuritySafeCritical]
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		[SecuritySafeCritical]
		internal static void ThrowInvalidOperationException(ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		internal static void ThrowSerializationException(ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		internal static void ThrowSecurityException(ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		[SecuritySafeCritical]
		internal static void ThrowNotSupportedException(ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		internal static void ThrowUnauthorizedAccessException(ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource)
		{
			throw new ArgumentException();
		}
		internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value, ExceptionArgument argName)
		{
			if(value == null && default(T) != null)
			{
				ThrowHelper.ThrowArgumentNullException(argName);
			}
		}
		[SecuritySafeCritical]
		internal static string GetArgumentName(ExceptionArgument argument)
		{
			string result = null;
			switch(argument)
			{
				case ExceptionArgument.obj:
					{
						result = "obj";
						break;
					}
				case ExceptionArgument.dictionary:
					{
						result = "dictionary";
						break;
					}
				case ExceptionArgument.dictionaryCreationThreshold:
					{
						result = "dictionaryCreationThreshold";
						break;
					}
				case ExceptionArgument.array:
					{
						result = "array";
						break;
					}
				case ExceptionArgument.info:
					{
						result = "info";
						break;
					}
				case ExceptionArgument.key:
					{
						result = "key";
						break;
					}
				case ExceptionArgument.collection:
					{
						result = "collection";
						break;
					}
				case ExceptionArgument.list:
					{
						result = "list";
						break;
					}
				case ExceptionArgument.match:
					{
						result = "match";
						break;
					}
				case ExceptionArgument.converter:
					{
						result = "converter";
						break;
					}
				case ExceptionArgument.queue:
					{
						result = "queue";
						break;
					}
				case ExceptionArgument.stack:
					{
						result = "stack";
						break;
					}
				case ExceptionArgument.capacity:
					{
						result = "capacity";
						break;
					}
				case ExceptionArgument.index:
					{
						result = "index";
						break;
					}
				case ExceptionArgument.startIndex:
					{
						result = "startIndex";
						break;
					}
				case ExceptionArgument.value:
					{
						result = "value";
						break;
					}
				case ExceptionArgument.count:
					{
						result = "count";
						break;
					}
				case ExceptionArgument.arrayIndex:
					{
						result = "arrayIndex";
						break;
					}
				case ExceptionArgument.name:
					{
						result = "name";
						break;
					}
				case ExceptionArgument.mode:
					{
						result = "mode";
						break;
					}
				case ExceptionArgument.item:
					{
						result = "item";
						break;
					}
				case ExceptionArgument.options:
					{
						result = "options";
						break;
					}
				case ExceptionArgument.view:
					{
						result = "view";
						break;
					}
				default:
					{
						return string.Empty;
					}
			}
			return result;
		}
		[SecuritySafeCritical]
		internal static string GetResourceName(ExceptionResource resource)
		{
			string result = null;
			switch(resource)
			{
				case ExceptionResource.Argument_ImplementIComparable:
					{
						result = "Argument_ImplementIComparable";
						break;
					}
				case ExceptionResource.Argument_InvalidType:
					{
						result = "Argument_InvalidType";
						break;
					}
				case ExceptionResource.Argument_InvalidArgumentForComparison:
					{
						result = "Argument_InvalidArgumentForComparison";
						break;
					}
				case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
					{
						result = "Argument_InvalidRegistryKeyPermissionCheck";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
					{
						result = "ArgumentOutOfRange_NeedNonNegNum";
						break;
					}
				case ExceptionResource.Arg_ArrayPlusOffTooSmall:
					{
						result = "Arg_ArrayPlusOffTooSmall";
						break;
					}
				case ExceptionResource.Arg_NonZeroLowerBound:
					{
						result = "Arg_NonZeroLowerBound";
						break;
					}
				case ExceptionResource.Arg_RankMultiDimNotSupported:
					{
						result = "Arg_RankMultiDimNotSupported";
						break;
					}
				case ExceptionResource.Arg_RegKeyDelHive:
					{
						result = "Arg_RegKeyDelHive";
						break;
					}
				case ExceptionResource.Arg_RegKeyStrLenBug:
					{
						result = "Arg_RegKeyStrLenBug";
						break;
					}
				case ExceptionResource.Arg_RegSetStrArrNull:
					{
						result = "Arg_RegSetStrArrNull";
						break;
					}
				case ExceptionResource.Arg_RegSetMismatchedKind:
					{
						result = "Arg_RegSetMismatchedKind";
						break;
					}
				case ExceptionResource.Arg_RegSubKeyAbsent:
					{
						result = "Arg_RegSubKeyAbsent";
						break;
					}
				case ExceptionResource.Arg_RegSubKeyValueAbsent:
					{
						result = "Arg_RegSubKeyValueAbsent";
						break;
					}
				case ExceptionResource.Argument_AddingDuplicate:
					{
						result = "Argument_AddingDuplicate";
						break;
					}
				case ExceptionResource.Serialization_InvalidOnDeser:
					{
						result = "Serialization_InvalidOnDeser";
						break;
					}
				case ExceptionResource.Serialization_MissingKeys:
					{
						result = "Serialization_MissingKeys";
						break;
					}
				case ExceptionResource.Serialization_NullKey:
					{
						result = "Serialization_NullKey";
						break;
					}
				case ExceptionResource.Argument_InvalidArrayType:
					{
						result = "Argument_InvalidArrayType";
						break;
					}
				case ExceptionResource.NotSupported_KeyCollectionSet:
					{
						result = "NotSupported_KeyCollectionSet";
						break;
					}
				case ExceptionResource.NotSupported_ValueCollectionSet:
					{
						result = "NotSupported_ValueCollectionSet";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
					{
						result = "ArgumentOutOfRange_SmallCapacity";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_Index:
					{
						result = "ArgumentOutOfRange_Index";
						break;
					}
				case ExceptionResource.Argument_InvalidOffLen:
					{
						result = "Argument_InvalidOffLen";
						break;
					}
				case ExceptionResource.Argument_ItemNotExist:
					{
						result = "Argument_ItemNotExist";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_Count:
					{
						result = "ArgumentOutOfRange_Count";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
					{
						result = "ArgumentOutOfRange_InvalidThreshold";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_ListInsert:
					{
						result = "ArgumentOutOfRange_ListInsert";
						break;
					}
				case ExceptionResource.NotSupported_ReadOnlyCollection:
					{
						result = "NotSupported_ReadOnlyCollection";
						break;
					}
				case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
					{
						result = "InvalidOperation_CannotRemoveFromStackOrQueue";
						break;
					}
				case ExceptionResource.InvalidOperation_EmptyQueue:
					{
						result = "InvalidOperation_EmptyQueue";
						break;
					}
				case ExceptionResource.InvalidOperation_EnumOpCantHappen:
					{
						result = "InvalidOperation_EnumOpCantHappen";
						break;
					}
				case ExceptionResource.InvalidOperation_EnumFailedVersion:
					{
						result = "InvalidOperation_EnumFailedVersion";
						break;
					}
				case ExceptionResource.InvalidOperation_EmptyStack:
					{
						result = "InvalidOperation_EmptyStack";
						break;
					}
				case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
					{
						result = "ArgumentOutOfRange_BiggerThanCollection";
						break;
					}
				case ExceptionResource.InvalidOperation_EnumNotStarted:
					{
						result = "InvalidOperation_EnumNotStarted";
						break;
					}
				case ExceptionResource.InvalidOperation_EnumEnded:
					{
						result = "InvalidOperation_EnumEnded";
						break;
					}
				case ExceptionResource.NotSupported_SortedListNestedWrite:
					{
						result = "NotSupported_SortedListNestedWrite";
						break;
					}
				case ExceptionResource.InvalidOperation_NoValue:
					{
						result = "InvalidOperation_NoValue";
						break;
					}
				case ExceptionResource.InvalidOperation_RegRemoveSubKey:
					{
						result = "InvalidOperation_RegRemoveSubKey";
						break;
					}
				case ExceptionResource.Security_RegistryPermission:
					{
						result = "Security_RegistryPermission";
						break;
					}
				case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
					{
						result = "UnauthorizedAccess_RegistryNoWrite";
						break;
					}
				case ExceptionResource.ObjectDisposed_RegKeyClosed:
					{
						result = "ObjectDisposed_RegKeyClosed";
						break;
					}
				case ExceptionResource.NotSupported_InComparableType:
					{
						result = "NotSupported_InComparableType";
						break;
					}
				case ExceptionResource.Argument_InvalidRegistryOptionsCheck:
					{
						result = "Argument_InvalidRegistryOptionsCheck";
						break;
					}
				case ExceptionResource.Argument_InvalidRegistryViewCheck:
					{
						result = "Argument_InvalidRegistryViewCheck";
						break;
					}
				default:
					{
						return string.Empty;
					}
			}
			return result;
		}
	}
	internal enum ExceptionResource
	{
		Argument_ImplementIComparable,
		Argument_InvalidType,
		Argument_InvalidArgumentForComparison,
		Argument_InvalidRegistryKeyPermissionCheck,
		ArgumentOutOfRange_NeedNonNegNum,
		Arg_ArrayPlusOffTooSmall,
		Arg_NonZeroLowerBound,
		Arg_RankMultiDimNotSupported,
		Arg_RegKeyDelHive,
		Arg_RegKeyStrLenBug,
		Arg_RegSetStrArrNull,
		Arg_RegSetMismatchedKind,
		Arg_RegSubKeyAbsent,
		Arg_RegSubKeyValueAbsent,
		Argument_AddingDuplicate,
		Serialization_InvalidOnDeser,
		Serialization_MissingKeys,
		Serialization_NullKey,
		Argument_InvalidArrayType,
		NotSupported_KeyCollectionSet,
		NotSupported_ValueCollectionSet,
		ArgumentOutOfRange_SmallCapacity,
		ArgumentOutOfRange_Index,
		Argument_InvalidOffLen,
		Argument_ItemNotExist,
		ArgumentOutOfRange_Count,
		ArgumentOutOfRange_InvalidThreshold,
		ArgumentOutOfRange_ListInsert,
		NotSupported_ReadOnlyCollection,
		InvalidOperation_CannotRemoveFromStackOrQueue,
		InvalidOperation_EmptyQueue,
		InvalidOperation_EnumOpCantHappen,
		InvalidOperation_EnumFailedVersion,
		InvalidOperation_EmptyStack,
		ArgumentOutOfRange_BiggerThanCollection,
		InvalidOperation_EnumNotStarted,
		InvalidOperation_EnumEnded,
		NotSupported_SortedListNestedWrite,
		InvalidOperation_NoValue,
		InvalidOperation_RegRemoveSubKey,
		Security_RegistryPermission,
		UnauthorizedAccess_RegistryNoWrite,
		ObjectDisposed_RegKeyClosed,
		NotSupported_InComparableType,
		Argument_InvalidRegistryOptionsCheck,
		Argument_InvalidRegistryViewCheck
	}
	internal enum ExceptionArgument
	{
		obj,
		dictionary,
		dictionaryCreationThreshold,
		array,
		info,
		key,
		collection,
		list,
		match,
		converter,
		queue,
		stack,
		capacity,
		index,
		startIndex,
		value,
		count,
		arrayIndex,
		name,
		mode,
		item,
		options,
		view
	}
	internal static class HashHelpers
	{
		internal static readonly int[] primes = new int[]
		{
			3, 
			7, 
			11, 
			17, 
			23, 
			29, 
			37, 
			47, 
			59, 
			71, 
			89, 
			107, 
			131, 
			163, 
			197, 
			239, 
			293, 
			353, 
			431, 
			521, 
			631, 
			761, 
			919, 
			1103, 
			1327, 
			1597, 
			1931, 
			2333, 
			2801, 
			3371, 
			4049, 
			4861, 
			5839, 
			7013, 
			8419, 
			10103, 
			12143, 
			14591, 
			17519, 
			21023, 
			25229, 
			30293, 
			36353, 
			43627, 
			52361, 
			62851, 
			75431, 
			90523, 
			108631, 
			130363, 
			156437, 
			187751, 
			225307, 
			270371, 
			324449, 
			389357, 
			467237, 
			560689, 
			672827, 
			807403, 
			968897, 
			1162687, 
			1395263, 
			1674319, 
			2009191, 
			2411033, 
			2893249, 
			3471899, 
			4166287, 
			4999559, 
			5999471, 
			7199369
		};
		internal static bool IsPrime(int candidate)
		{
			if((candidate & 1) != 0)
			{
				int num = (int)Math.Sqrt((double)candidate);
				for(int i = 3; i <= num; i += 2)
				{
					if(candidate % i == 0)
					{
						return false;
					}
				}
				return true;
			}
			return candidate == 2;
		}
		internal static int GetPrime(int min)
		{
			if(min < 0)
			{
				throw new ArgumentException();
			}
			for(int i = 0; i < HashHelpers.primes.Length; i++)
			{
				int num = HashHelpers.primes[i];
				if(num >= min)
				{
					return num;
				}
			}
			for(int j = min | 1; j < 2147483647; j += 2)
			{
				if(HashHelpers.IsPrime(j))
				{
					return j;
				}
			}
			return min;
		}
	} 
	#endregion Helpers
}
