using System.Text;

namespace Scch.DataAccess.NoSql.Xml
{
    public interface IXmlContext
    {
        string RootDirectory { get; }

        Encoding Encoding { get; }
    }
}