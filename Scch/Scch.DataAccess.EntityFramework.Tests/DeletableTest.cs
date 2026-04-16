using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Scch.DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.DomainModel.EntityFramework;
using Scch.DomainModel.EntityFramework.Tests;
using Scch.DomainModel.EntityFramework.Tests.Mapping;

namespace Scch.DataAccess.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for DeletableTest
    /// </summary>
    [TestClass]
    public class DeletableTest
    {
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetAuditRecordsForChangeDeleted()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                string deletedId = Guid.NewGuid().ToString();

                var entity = RepositoryTest.Create<DeletableDummy>();
                entity.Bezeichnung = deletedId;
                repository.ApplyChanges(entity);
                repository.UnitOfWork.CommitTransaction();
                entity.MarkAsDeleted();
                repository.ApplyChanges(entity);

                var entries = from entry in context.ChangeTracker.Entries()
                              where
                                  (entry.State == EntityState.Added || entry.State == EntityState.Deleted ||
                                   entry.State == EntityState.Modified)
                              select entry;

                foreach (var entry in entries)
                {
                    var logEntries = new List<AuditLog>(EntityFrameworkHelper.GetAuditRecordsForChange(entry, string.Empty));

                    Assert.AreEqual(1, logEntries.Count());
                    Assert.AreEqual("IsDeleted", logEntries[0].ColumnName);
                    Assert.AreEqual("M", logEntries[0].EventType);
                    Assert.AreEqual("True", logEntries[0].NewValue);
                    Assert.AreEqual("False", logEntries[0].OriginalValue);
                    Assert.AreEqual("DeletableDummy", logEntries[0].TableName);
                }
            }
        }
    }

    public class DeletableDummy : EntityWithBezeichnung, IDeletable
    {
        public bool IsDeleted { get; set; }
    }

    public class DeletableDummyMapping : EntityWithBezeichnungMapping<DeletableDummy>
    {
        public DeletableDummyMapping()
        {
            Property(x => x.IsDeleted);
        }
    }
}
