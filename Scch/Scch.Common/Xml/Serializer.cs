using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Scch.Common.Conversion;
using Scch.Common.IO;
using Scch.Common.Reflecton;

namespace Scch.Common.Xml
{
    /// <summary>
    /// Functions for serializing and deserializing objects.
    /// </summary>
    public static class Serializer
    {
        static readonly IDictionary<Type, XmlSerializer> SerializerCache;
        static readonly IDictionary<Assembly, Assembly> LoadedAssemblies;
        static readonly Encoding DefaultEncoding;

        static Serializer()
        {
            SerializerCache = new Dictionary<Type, XmlSerializer>();
            LoadedAssemblies = new Dictionary<Assembly, Assembly>();
            DefaultEncoding = new UnicodeEncoding();
        }

        #region CreateSerializer
        static XmlSerializer CreateSerializer(Type type, Type[] extraTypes=null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (extraTypes == null)
                extraTypes = new Type[0];

            if (!SerializerCache.ContainsKey(type))
            {
                lock (typeof(Serializer))
                {
                    if (!SerializerCache.ContainsKey(type))
                    {
                        if (!LoadedAssemblies.ContainsKey(type.Assembly))
                        {
                            string assemblyFileName = AssemblyHelper.GetAssemblyFileName(type.Assembly);
                            assemblyFileName =
                                assemblyFileName.Replace(AssemblyHelper.GetAssemblyFileExtension(type.Assembly),
                                    string.Empty) +
                                ".XmlSerializers.dll";

                            if (File.Exists(assemblyFileName))
                            {
                                Assembly serialzerAssembly = Assembly.LoadFile(assemblyFileName);
                                LoadedAssemblies.Add(type.Assembly, serialzerAssembly);
                            }
                        }

                        Type serializerType = null;
                        if (LoadedAssemblies.ContainsKey(type.Assembly))
                        {
                            serializerType = LoadedAssemblies[type.Assembly].GetType(
                                "Microsoft.Xml.Serialization.GeneratedAssembly." + type.Name + "Serializer", false);
                        }

                        if (serializerType != null)
                            SerializerCache.Add(type, (XmlSerializer)Activator.CreateInstance(serializerType));
                        else
                            SerializerCache.Add(type, new XmlSerializer(type, extraTypes));
                    }
                }
            }

            return SerializerCache[type];
        }

        private static XmlSerializer CreateSerializer(object obj, Type[] extraTypes = null)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return CreateSerializer(obj.GetType(), extraTypes);
        }
        #endregion CreateSerializer

        #region Serialize

        /// <summary>
        /// Serializes the <see cref="object"/> to the <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/>.</param>
        /// <param name="obj">The object.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        public static void SerializeToFile(FileInfo file, object obj, Encoding encoding = null)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (encoding == null)
                encoding = DefaultEncoding;

            using (FileStream fs = file.OpenWrite())
            {
                SerializeToStream(fs, obj, encoding);
                fs.SetLength(fs.Position);
            }
        }

        /// <summary>
        /// Serializes the <see cref="object"/> to the <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/>.</param>
        /// <param name="obj">The object.</param>
        public static void SerializeToTextWriter(TextWriter writer, object obj)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            XmlSerializer serializer = CreateSerializer(obj);
            serializer.Serialize(writer, obj);
        }

        /// <summary>
        /// Serializes the <see cref="object"/> to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/>.</param>
        /// <param name="obj">The object.</param>
        public static void SerializeToXmlWriter(XmlWriter writer, object obj)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            XmlSerializer serializer = CreateSerializer(obj);
            serializer.Serialize(writer, obj);
        }

        /// <summary>
        /// Serializes the <see cref="object"/> to the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="encoding">The <see cref="Encoding"/></param>
        public static void SerializeToStream(Stream stream, object obj, Encoding encoding = null)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (encoding == null)
                encoding = DefaultEncoding;

            var sw = new StreamWriter(stream, encoding);
            SerializeToTextWriter(sw, obj);
        }

        /// <summary>
        /// Serializes the <see cref="object"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The serialized object.</returns>
        public static string SerializeToString(object obj, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = DefaultEncoding;

            StringBuilder builder = new StringBuilder();
            using (var sw = new EncodingStringWriter(builder, encoding))
            {
                SerializeToTextWriter(sw, obj);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Serializes the <see cref="object"/> to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The serialized object.</returns>
        public static XmlDocument SerializeToXmlDocument(object obj, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = DefaultEncoding;

            var doc = new XmlDocument();
            doc.LoadXml(SerializeToString(obj, encoding));
            return doc;
        }

        /// <summary>
        /// Serializes the <see cref="object"/> to an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The serialized object.</returns>
        public static XmlElement SerializeToXmlElement(object obj, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = DefaultEncoding;

            XmlDocument doc = SerializeToXmlDocument(obj, encoding);

            return doc.ChildNodes.Cast<XmlNode>().Where(node => node.NodeType == XmlNodeType.Element).Cast<XmlElement>().FirstOrDefault();
        }
        #endregion Serialize

        #region Deserialize

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="file">The <see cref="FileInfo"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromFile(Type type, FileInfo file,
            Encoding encoding = null)
        {
            return DeserializeFromFile(type, null, file, encoding);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="file">The <see cref="FileInfo"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromFile(Type type, Type[] extraTypes, FileInfo file, Encoding encoding = null)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (encoding == null)
                encoding = DefaultEncoding;

            using (FileStream fs = file.OpenRead())
            {
                return DeserializeFromStream(type, extraTypes, fs, encoding);
            }
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="reader">The <see cref="TextReader"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromTextReader(Type type, TextReader reader)
        {
            return DeserializeFromTextReader(type, null, reader);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="reader">The <see cref="TextReader"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromTextReader(Type type, Type[] extraTypes, TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            XmlSerializer serializer = CreateSerializer(type, extraTypes);
            return Converter.ConvertTo(serializer.Deserialize(reader), type);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from an <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="reader">The <see cref="XmlReader"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXmlReader(Type type, XmlReader reader)
        {
            return DeserializeFromXmlReader(type, null, reader);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from an <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="reader">The <see cref="XmlReader"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXmlReader(Type type, Type[] extraTypes, XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            XmlSerializer serializer = CreateSerializer(type, extraTypes);
            return Converter.ConvertTo(serializer.Deserialize(reader), type);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The <see cref="Encoding"/>,</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromStream(Type type, Stream stream, Encoding encoding = null)
        {
            return DeserializeFromStream(type, null, stream, encoding);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The <see cref="Encoding"/>,</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromStream(Type type, Type[] extraTypes, Stream stream, Encoding encoding = null)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (encoding == null)
                encoding = DefaultEncoding;

            using (var sr = new StreamReader(stream, encoding))
            {
                return DeserializeFromTextReader(type, extraTypes, sr);
            }
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="string"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="xml">The xml string.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromString(Type type, string xml, Encoding encoding = null)
        {
            return DeserializeFromString(type, null, xml, encoding);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from a <see cref="string"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="xml">The xml string.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromString(Type type, Type[] extraTypes, string xml, Encoding encoding = null)
        {
            if (xml == null)
                return null;

            if (encoding == null)
                encoding = DefaultEncoding;

            using (var stream = new MemoryStream(encoding.GetBytes(xml)))
            {
                return DeserializeFromStream(type, extraTypes, stream, encoding);
            }
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="document">The <see cref="XmlDocument"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXmlDocument(Type type, XmlDocument document,
            Encoding encoding = null)
        {
            return DeserializeFromXmlDocument(type, null, document, encoding);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="document">The <see cref="XmlDocument"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXmlDocument(Type type, Type[] extraTypes, XmlDocument document, Encoding encoding = null)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            if (encoding == null)
                encoding = DefaultEncoding;

            var sb = new StringBuilder();
            document.Save(new EncodingStringWriter(sb, encoding));
            return DeserializeFromTextReader(type, extraTypes, new StringReader(sb.ToString()));
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="element">The <see cref="XmlElement"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXmlElement(Type type, XmlElement element,
            Encoding encoding = null)
        {
            return DeserializeFromXmlElement(type, null, element, encoding);
        }

        /// <summary>
        /// Deserializes an object with the specified <see cref="Type"/> from an <see cref="XmlElement"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the object.</param>
        /// <param name="extraTypes">>A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
        /// <param name="element">The <see cref="XmlElement"/>.</param>
        /// <param name="encoding">The <see cref="Encoding"/>.</param>
        /// <returns>The deserialized object.</returns>
        public static object DeserializeFromXmlElement(Type type, Type[] extraTypes, XmlElement element, Encoding encoding = null)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            if (encoding == null)
                encoding = DefaultEncoding;

            return DeserializeFromXmlDocument(type, extraTypes, element.OwnerDocument, encoding);
        }
        #endregion Deserialize
    }
}
