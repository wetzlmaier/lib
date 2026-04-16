using System;
using Scch.DomainModel.NoSql.Neo4j;

namespace Scch.DataAccess.NoSql.Neo4j.Tests
{
    [Serializable]
    public class Address : Neo4jNoSqlEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public int Postal { get; set; }
    }
}
