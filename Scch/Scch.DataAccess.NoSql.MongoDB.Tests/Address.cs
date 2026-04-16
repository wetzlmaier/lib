using System;
using Scch.DomainModel.NoSql.MongoDB;

namespace Scch.DataAccess.NoSql.MongoDB.Tests
{
    [Serializable]
    public class Address : MongoDBNoSqlEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public int Postal { get; set; }
    }
}
