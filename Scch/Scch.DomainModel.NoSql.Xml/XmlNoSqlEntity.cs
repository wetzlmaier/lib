using System;
using System.Xml.Serialization;

namespace Scch.DomainModel.NoSql.Xml
{
    [Serializable]
    [XmlRoot]
    public class XmlNoSqlEntity : NoSqlEntity<Guid>, IXmlNoSqlEntity
    {
        [XmlIgnore]
        public override bool IsTransient
        {
            get { return Id == Guid.Empty; }
        }

        [XmlIgnore]
        public override bool IsValid
        {
            get { return base.IsValid; }
            protected set { base.IsValid = value; }
        }
    }
}
