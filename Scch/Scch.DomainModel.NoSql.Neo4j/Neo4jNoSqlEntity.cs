using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Scch.Neo4j;

namespace Scch.DomainModel.NoSql.Neo4j
{
    [Serializable]
    [XmlRoot]
    public abstract class Neo4jNoSqlEntity : NoSqlEntity<Neo4jObjectId>, INeo4jNoSqlEntity
    {
        [JsonIgnore]
        [XmlIgnore]
        public override bool IsTransient
        {
            get { return Equals(Id, default(Neo4jObjectId)); }
        }

        [JsonIgnore]
        [XmlIgnore]
        public override bool IsValid
        {
            get { return base.IsValid; }
            protected set { base.IsValid = value; }
        }
    }
}
