using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scch.Common.Drawing;

namespace Scch.Common.Xml.Serialization
{
    /// <summary>
    /// <see cref="IXmlSerializable"/> implementation for <see cref="Image"/>.
    /// </summary>
    public class ImageSerializer : IXmlSerializable
    {
        #region Members
        Image _image;
        #endregion Members

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ImageSerializer"/>.
        /// </summary>
        public ImageSerializer()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ImageSerializer"/>.
        /// </summary>
        /// <param name="image">The <see cref="Image"/>.</param>
        ImageSerializer(Image image):this()
        {
            _image = image;
        }
        #endregion Constructors

        #region Properties
        Image Image
        {
            get
            {
                return _image;
            }
        }
        #endregion Properties

        #region Operators
        /// <summary>
        /// Assigns the <see cref="Image"/> to a <see cref="ImageSerializer"/>.
        /// </summary>
        /// <param name="image">The <see cref="Image"/>.</param>
        /// <returns>The <see cref="ImageSerializer"/>.</returns>
        public static implicit operator ImageSerializer(Image image)
        {
            return image == null ? null : new ImageSerializer(image);
        }

        /// <summary>
        /// Assigns the <see cref="ImageSerializer"/> to a <see cref="Image"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="ImageSerializer"/></param>
        /// <returns>The <see cref="Image"/>.</returns>
        public static implicit operator Image(ImageSerializer serializer)
        {
            return serializer == null ? null : serializer.Image;
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
            _image = ImageHelper.ImageFromBase64String(reader.GetAttribute("base64"));
        }

        /// <summary>
        /// <see cref="IXmlSerializable.WriteXml"/>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("base64", ImageHelper.ImageToBase64String(_image));
        }
        #endregion IXmlSerializable Member
    }
}