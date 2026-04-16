using System;
using Scch.DomainModel.NoSql.Xml;

namespace Scch.DataAccess.NoSql.Xml.Tests
{
    [Serializable]
    public class Address : XmlNoSqlEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public int Postal { get; set; }
    }
}
