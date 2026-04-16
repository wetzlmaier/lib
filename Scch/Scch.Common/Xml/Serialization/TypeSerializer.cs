using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scch.Common.Xml.Serialization
{
    /// <summary>
    /// <see cref="IXmlSerializable"/> implementation for <see cref="Type"/>.
    /// </summary>
    public class TypeSerializer: IXmlSerializable
    {
        #region Members
        Type _type;
        #endregion Members

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="TypeSerializer"/>.
        /// </summary>
        public TypeSerializer()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="TypeSerializer"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        TypeSerializer(Type type)
            : this()
        {
            _type = type;
        }
        #endregion Constructors

        #region Properties
        Type Type
        {
            get
            {
                return _type;
            }
        }
        #endregion Properties

        #region Operators
        /// <summary>
        /// Assigns the <see cref="Type"/> to a <see cref="TypeSerializer"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The <see cref="TypeSerializer"/>.</returns>
        public static implicit operator TypeSerializer(Type type)
        {
            return new TypeSerializer(type);
        }

        /// <summary>
        /// Assigns the <see cref="TypeSerializer"/> to an <see cref="Type"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="TypeSerializer"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static implicit operator Type(TypeSerializer serializer)
        {
            return serializer == null ? null : serializer.Type;
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
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;
            
            _type = Type.GetType(reader.ReadContentAsString(), true, false);

            reader.ReadEndElement();
        }

        /// <summary>
        /// <see cref="IXmlSerializable.WriteXml"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            if (_type.AssemblyQualifiedName != null)
                writer.WriteValue(_type.AssemblyQualifiedName);
        }
        #endregion IXmlSerializable Member
    }
}