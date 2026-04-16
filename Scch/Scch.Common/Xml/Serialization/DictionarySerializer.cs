using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scch.Common.Collections.Generic;

namespace Scch.Common.Xml.Serialization
{
    /// <summary>
    /// <see cref="IXmlSerializable"/> implementation for <see cref="Dictionary"/>.
    /// </summary>
    public class DictionarySerializer<TKey, TValue> : IXmlSerializable
    {
        #region Members
        SerializableDictionary<TKey, TValue> _dictionary;
        #endregion Members

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="DictionarySerializer{TKey, TValue}"/>.
        /// </summary>
        public DictionarySerializer()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="DictionarySerializer{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="Dictionary"/>.</param>
        DictionarySerializer(SerializableDictionary<TKey, TValue> dictionary)
            : this()
        {
            _dictionary = dictionary;
        }
        #endregion Constructors

        #region Properties
        SerializableDictionary<TKey, TValue> Dictionary
        {
            get
            {
                return _dictionary;
            }
        }
        #endregion Properties

        #region Operators
        /// <summary>
        /// Assigns the <see cref="SerializableDictionary{TKey, TValue}"/> to a <see cref="DictionarySerializer{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="SerializableDictionary{TKey, TValue}"/>.</param>
        /// <returns>The <see cref="DictionarySerializer{TKey, TValue}"/>.</returns>
        public static implicit operator DictionarySerializer<TKey, TValue>(SerializableDictionary<TKey, TValue> dictionary)
        {
            return new DictionarySerializer<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Assigns the <see cref="DictionarySerializer{TKey, TValue}"/> to a <see cref="SerializableDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="DictionarySerializer{TKey, TValue}"/>.</param>
        /// <returns>The <see cref="SerializableDictionary{TKey, TValue}"/>.</returns>
        public static implicit operator
            SerializableDictionary<TKey, TValue>(DictionarySerializer<TKey, TValue> serializer)
        {
            return serializer == null ? new SerializableDictionary<TKey, TValue>() : serializer.Dictionary;
        }
        #endregion Operators

        #region IXmlSerializable Member
        /// <summary>
        /// <see cref="IXmlSerializable.GetSchema"/>
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// <see cref="IXmlSerializable.ReadXml"/>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            _dictionary = new SerializableDictionary<TKey, TValue>();

            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("KeyValuePair");

                reader.ReadStartElement("Key");
                var key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("Value");
                var value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                _dictionary.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// <see cref="IXmlSerializable.WriteXml"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in _dictionary.Keys)
            {
                writer.WriteStartElement("KeyValuePair");

                writer.WriteStartElement("Key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("Value");
                TValue value = _dictionary[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion IXmlSerializable Member
    }
}