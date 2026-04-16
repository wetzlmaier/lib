using System;
using System.Collections.Generic;
using Scch.DomainModel.NoSql.Neo4j;

namespace Scch.DataAccess.NoSql.Neo4j.Tests
{
    [Serializable]
    public class Person : Neo4jNoSqlEntity
    {
        public Person()
        {
            Address = new Address();
            Children = new List<Person>();
        }

        public string Name { get; set; }
        public int Age { get; set; }

        public Address Address { get; set; }

        public List<Person> Children { get; set; }
    }
}
