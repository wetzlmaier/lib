using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Scch.DomainModel.NoSql.MongoDB;

namespace Scch.DataAccess.NoSql.Tests
{
    [Serializable]
    [XmlInclude(typeof(Bitmap))]
    public class Person : MongoDBNoSqlEntity
    {
        public Person()
        {
            Address = new Address();
            Children = new List<Person>();
        }

        public string Name { get; set; }
        public int Age { get; set; }

        public Address Address { get; set; }

        public Image Image { get; set; }

        public List<Person> Children { get; set; }
    }
}
