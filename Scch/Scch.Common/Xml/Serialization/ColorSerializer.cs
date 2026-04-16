using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scch.Common.Xml.Serialization
{
    /// <summary>
    /// <see cref="IXmlSerializable"/> implementation for <see cref="Color"/>.
    /// </summary>
    public class ColorSerializer : IXmlSerializable
    {
        #region Members
        Color _color;
        private const string Empty = "Empty";
        #endregion Members

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorSerializer"/>.
        /// </summary>
        public ColorSerializer()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ColorSerializer"/>.
        /// </summary>
        /// <param name="Color">The <see cref="Color"/>.</param>
        ColorSerializer(Color Color)
            : this()
        {
            _color = Color;
        }
        #endregion Constructors

        #region Properties
        Color Color
        {
            get
            {
                return _color;
            }
        }
        #endregion Properties

        #region Operators
        /// <summary>
        /// Assigns the <see cref="Color"/> to a <see cref="ColorSerializer"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/>.</param>
        /// <returns>The <see cref="ColorSerializer"/>.</returns>
        public static implicit operator ColorSerializer(Color color)
        {
            return new ColorSerializer(color);
        }

        /// <summary>
        /// Assigns the <see cref="ColorSerializer"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="ColorSerializer"/>.</param>
        /// <returns>The <see cref="Color"/>.</returns>
        public static implicit operator Color(ColorSerializer serializer)
        {
            return serializer == null ? Color.Empty : serializer.Color;
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
            string name = reader.GetAttribute("name");

            if (name!=null)
            {
                _color = name==Empty ? Color.Empty : Color.FromName(name);
            }
            else
            {
                string a = reader.GetAttribute("a");
                string r = reader.GetAttribute("r");
                string g = reader.GetAttribute("g");
                string b = reader.GetAttribute("b");
                _color = Color.FromArgb(int.Parse(a), int.Parse(r), int.Parse(g), int.Parse(b));
            }
        }

        /// <summary>
        /// <see cref="IXmlSerializable.WriteXml"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            if (_color.IsNamedColor)
                writer.WriteAttributeString("name", _color.Name);
            else
            {
                if (_color.IsEmpty)
                    writer.WriteAttributeString("name", Empty);
                else
                {
                    writer.WriteAttributeString("a", ((int) _color.A).ToString());
                    writer.WriteAttributeString("r", ((int) _color.R).ToString());
                    writer.WriteAttributeString("g", ((int) _color.G).ToString());
                    writer.WriteAttributeString("b", ((int) _color.B).ToString());
                }
            }
        }
        #endregion IXmlSerializable Member
    }
}