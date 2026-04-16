using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Common.ComponentModel;
using Scch.DataAccess.EntityFramework;

namespace Scch.DomainModel.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for MappingTest
    /// </summary>
    [TestClass]
    public class MappingTest
    {
        public MappingTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        private readonly List<Type> _testedTypes = new List<Type>();

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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMapping()
        {
            var failedAssertions = new List<string>();

            var builder = new DbContextBuilder<ExtendedDbContext<long>, long>("SCCH",
                                                                  new[]
                                                                      {
                                                                          "Scch.DomainModel",
                                                                          "Scch.DomainModel.EntityFramework"
                                                                      },
                                                                  false, false);

            TestIfAllPropertiesAreMapped(builder.Entity<AuditLog>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Anweisungstyp>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Produktversion>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Produktmerkmal>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Auspraegung>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Person>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Produktgruppe>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Produkt>(), failedAssertions);
            TestIfAllPropertiesAreMapped(builder.Entity<Merkmalsgruppe>(), failedAssertions);

            foreach (var entityType in typeof(Anweisungstyp).Assembly.GetExportedTypes())
            {
                if (typeof(EntityFrameworkEntity<long>).IsAssignableFrom(entityType) && !entityType.IsAbstract)
                    if (!_testedTypes.Contains(entityType))
                        failedAssertions.Add(entityType + " not tested.");
            }

            foreach (string failedAssertion in failedAssertions)
                Console.WriteLine(failedAssertion);

            Assert.AreEqual(0, failedAssertions.Count);
        }

        private void TestIfAllPropertiesAreMapped<T>(EntityTypeConfiguration<T> typeConfiguration, List<string> failedAssertions) where T : class
        {
            _testedTypes.Add(typeof(T));

            PropertyInfo configurationProperty = typeConfiguration.GetType().GetProperty("Configuration", BindingFlags.Instance | BindingFlags.NonPublic);
            object configuration = configurationProperty.GetValue(typeConfiguration, null);
            PropertyInfo clrTypeProperty = configuration.GetType().GetProperty("ClrType", BindingFlags.Instance | BindingFlags.NonPublic);
            PropertyInfo configuredProperties = configuration.GetType().GetProperty("ConfiguredProperties", BindingFlags.Instance | BindingFlags.NonPublic);

            Type clrType = (Type)clrTypeProperty.GetValue(configuration, null);
            Assert.AreEqual(typeof(T), clrType);

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType))
                    continue;

                var attributes = (BrowsableAttribute[])propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute), true);
                if (attributes.Length == 0 || !attributes[0].Browsable)
                    continue;

                if (propertyInfo.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0)
                    continue;

                if (propertyInfo.GetCustomAttributes(typeof(ConcurrencyCheckAttribute), true).Length > 0)
                    continue;

                if (propertyInfo.DeclaringType==typeof(DataErrorInfo))
                    continue;
                
                if (!((IEnumerable<PropertyInfo>)configuredProperties.GetValue(configuration, null)).Any(configuredProperty => propertyInfo.Name == configuredProperty.Name))
                    failedAssertions.Add(propertyInfo.DeclaringType.Name + "." + propertyInfo.Name + " is not mapped.");
            }
        }
    }
}
