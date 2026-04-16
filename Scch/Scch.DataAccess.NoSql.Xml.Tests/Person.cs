using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Scch.Common.Xml.Serialization;
using Scch.DomainModel.NoSql.Xml;

namespace Scch.DataAccess.NoSql.Xml.Tests
{
    [Serializable]
    //[XmlInclude(typeof(Bitmap))]
    public class Person : XmlNoSqlEntity
    {
        public Person()
        {
            Address = new Address();
            Children = new List<Person>();
        }

        public string Name { get; set; }
        public int Age { get; set; }

        public Address Address { get; set; }

        [XmlElement(typeof(ImageSerializer))]
        public Image Image { get; set; }

        public List<Person> Children { get; set; }
    }
}
