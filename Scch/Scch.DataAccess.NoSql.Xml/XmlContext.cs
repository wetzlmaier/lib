using System.IO;
using System.Text;
using Scch.Common.Configuration;
using Scch.Common.Reflecton;

namespace Scch.DataAccess.NoSql.Xml
{
    public class XmlContext : IXmlContext
    {
        public XmlContext()
            : this(ConfigurationHelper.Current.ReadString("RootDirectory", Path.Combine(AssemblyHelper.GetApplicationDirectory(), "Xml")),
                  Encoding.GetEncoding(ConfigurationHelper.Current.ReadString("XmlEncoding", "UTF-8")))
        {
        }

        public XmlContext(string rootDirectory, Encoding encoding = null)
        {
            RootDirectory = rootDirectory;
            Encoding = encoding;
        }

        public string RootDirectory { get; protected set; }

        public Encoding Encoding { get; private set; }
    }
}
