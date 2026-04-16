using System;
using System.Collections;
using System.Collections.Generic;

namespace Scch.Common.Collections.Generic
{
    /// <summary>
    /// Read only implementation of <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private const string ErrorMessage = "This dictionary is read-only";
        private readonly IDictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary"></param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.Add(TKey,TValue)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.ContainsKey"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.Keys"/>
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.TryGetValue"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.Values"/>
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.this"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                throw new NotSupportedException(ErrorMessage);
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}.Add"/>
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// <see cref="ICollection{T}.Clear"/>
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// <see cref="ICollection{T}.Contains"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// <see cref="ICollection{T}.CopyTo"/>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// <see cref="ICollection{T}.Count"/>
        /// </summary>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// <see cref="ICollection{T}.IsReadOnly"/>
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// <see cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException(ErrorMessage);
        }

        /// <summary>
        /// <see cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// <see cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_dictionary as IEnumerable).GetEnumerator();
        }
    }
}
