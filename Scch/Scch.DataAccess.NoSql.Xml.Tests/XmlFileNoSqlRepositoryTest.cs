using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Common.IO;

namespace Scch.DataAccess.NoSql.Xml.Tests
{
    /// <summary>
    /// Summary description for XmlFileNoSqlRepositoryTest
    /// </summary>
    [TestClass]
    public class XmlFileNoSqlRepositoryTest
    {
        public XmlFileNoSqlRepositoryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
            _factory=new XmlNoSqlRepositoryFactory();
            _factory.Drop();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private IXmlNoSqlRepositoryFactory _factory;

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
        public void TestDeleteAll()
        {
            
        }
    }
}
