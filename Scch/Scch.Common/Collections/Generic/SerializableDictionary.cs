using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Scch.Common.Collections.Generic
{
    /// <summary>
    /// <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <remarks>Workaround for InvalidOperationException, NotSupportedException with classes which implement IDictionary.</remarks>
    [Serializable]
    public class SerializableDictionary<TKey, TValue>: Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region Constructors
        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}(IDictionary{TKey,TValue})"/>
        /// </summary>
        /// <param name="dictionary"></param>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary):base(dictionary)
        {
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}(IDictionary{TKey,TValue}, IEqualityComparer{TKey})"/>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}(int, IEqualityComparer{TKey})"/>
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}(IEqualityComparer{TKey})"/>
        /// </summary>
        /// <param name="comparer"></param>
        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}(int)"/>
        /// </summary>
        /// <param name="capacity"></param>
        public SerializableDictionary(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}()"/>
        /// </summary>
        public SerializableDictionary()
        {
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}(SerializationInfo, StreamingContext)"/>
        /// </summary>
        /// <param name="si"></param>
        /// <param name="sc"></param>
        protected SerializableDictionary(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
        #endregion Constructors

        #region IXmlSerializable Members
        /// <summary>
        /// <see cref="IXmlSerializable.GetSchema"/>
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotSupportedException();
        }
 
        /// <summary>
        /// <see cref="IXmlSerializable.ReadXml"/>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// <see cref="IXmlSerializable.WriteXml"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}