using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.DataAccess.NoSql.MongoDB;

namespace Scch.DataAccess.NoSql.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        private IMongoDBNoSqlRepositoryFactory _factory;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            _factory = new MongoDBNoSqlRepositoryFactory();

            using (var repository = _factory.Create())
            {
                repository.DeleteAll<Person>();
                repository.DeleteAll<Address>();
            }
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup]
        // public void TestCleanup()
        // {
        // }
        //
        #endregion

        [TestMethod]
        public void TestCrud()
        {
            var person = new Person
            {
                Name = "Rudi Ratlos",
                Age = 22,
                Image = new Bitmap(10, 10),
                Address = new Address { City = "Entenhausen", Postal = 1234, Street = "Schotterstrasse" }
            };

            using (var repository = _factory.Create())
            {
                repository.Save(person.Address);
                repository.Save(person);
                Assert.AreEqual(1, repository.Count<Person>());

                var personen = repository.Find<Person>(p => p.Age == 22);
                Assert.AreEqual(1, personen.Count());
                person = personen.Single();
                Assert.AreEqual("Schotterstrasse", person.Address.Street);

                person.Age = 23;
                repository.Save(person);
                personen = repository.Find<Person>(p => p.Age == 23);
                Assert.AreEqual(1, personen.Count());

                repository.Delete<Person>(p => p.Age == 23);
                personen = repository.Find<Person>(p => p.Age == 23);
                Assert.AreEqual(0, personen.Count());
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            var person = new Person
            {
                Name = "Rudi Ratlos"
            };

            using (var repository = _factory.Create())
            {
                repository.Save(person);

                repository.UpdatePush(person, p => p.Children, new Person { Name = "Child1" }, new Person { Name = "Child2" });
                var c1 = person.Children.SingleOrDefault(c => c.Name == "Child1");
                Assert.IsNotNull(c1);
                var c2 = person.Children.SingleOrDefault(c => c.Name == "Child2");
                Assert.IsNotNull(c2);

                var id = person.Id;
                person = repository.FindOne<Person>(e => e.Id == id);
                c1 = person.Children.SingleOrDefault(c => c.Name == "Child1");
                Assert.IsNotNull(c1);
                c2 = person.Children.SingleOrDefault(c => c.Name == "Child2");
                Assert.IsNotNull(c2);

                repository.UpdatePull(person, p => p.Children, person.Children.ToArray());
                c1 = person.Children.SingleOrDefault(c => c.Name == "Child1");
                Assert.IsNull(c1);
                c2 = person.Children.SingleOrDefault(c => c.Name == "Child2");
                Assert.IsNull(c2);

                person = repository.FindOne<Person>(e => e.Id == id);
                c1 = person.Children.SingleOrDefault(c => c.Name == "Child1");
                Assert.IsNull(c1);
                c2 = person.Children.SingleOrDefault(c => c.Name == "Child2");
                Assert.IsNull(c2);
            }
        }

        [TestMethod]
        public void TestUpdateMany()
        {
            for (int i = 0; i < 1000; i++)
            {
                TestUpdate();
            }
        }

        [TestMethod]
        public void TestUpdateManyAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(TestUpdate));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
