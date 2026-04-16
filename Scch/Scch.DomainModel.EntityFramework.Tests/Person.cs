using System;
using System.Collections.Generic;

namespace Scch.DomainModel.EntityFramework.Tests
{
    public class Person : EntityFrameworkEntity<long>
    {
        public string Name { get; set; }
    }

    public class PersonNameComparer:IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            return String.CompareOrdinal(x.Name, y.Name);
        }
    }
}
