using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Scch.Common.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.DomainModel.EntityFramework;
using Scch.DomainModel.EntityFramework.Tests;

namespace Scch.DataAccess.EntityFramework.Tests
{
    [TestClass]
    public class EntityFrameworkHelperTest
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
        public void TestCollectRelationalMembers()
        {
            Expression<Func<AuditLog, object>> expression = i => i.TableName;
            Assert.AreEqual("TableName", ExpressionHelper.GetPropertyPath(expression));
        }

        [TestMethod]
        public void TestGetAuditRecordsForChangeModified()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                string modifiedId = Guid.NewGuid().ToString();
                string bezeichnung = Guid.NewGuid().ToString();
                var entity = RepositoryTest.Create<Anweisungstyp>();
                entity.Bezeichnung = bezeichnung;
                repository.ApplyChanges(entity);
                repository.UnitOfWork.CommitTransaction();
                entity.Bezeichnung = modifiedId;

                var entries = from entry in context.ChangeTracker.Entries()
                              where
                                  (entry.State == EntityState.Added || entry.State == EntityState.Deleted ||
                                   entry.State == EntityState.Modified)
                              select entry;

                foreach (var entry in entries)
                {
                    var logEntries = new List<AuditLog>(EntityFrameworkHelper.GetAuditRecordsForChange(entry, string.Empty));

                    Assert.AreEqual(1, logEntries.Count());
                    Assert.AreEqual("Bezeichnung", logEntries[0].ColumnName);
                    Assert.AreEqual("M", logEntries[0].EventType);
                    Assert.AreEqual(modifiedId, logEntries[0].NewValue);
                    Assert.AreEqual(bezeichnung, logEntries[0].OriginalValue);
                    Assert.AreEqual("Anweisungstyp", logEntries[0].TableName);
                }
            }
        }

        [TestMethod]
        public void TestGetAuditRecordsForChangeAdded()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                string addedId = Guid.NewGuid().ToString();
                var entity = RepositoryTest.Create<Anweisungstyp>();
                entity.Bezeichnung = addedId;
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
                    Assert.AreEqual(null, logEntries[0].ColumnName);
                    Assert.AreEqual("A", logEntries[0].EventType);
                    Assert.AreEqual(addedId, logEntries[0].NewValue);
                    Assert.AreEqual(null, logEntries[0].OriginalValue);
                    Assert.AreEqual("Anweisungstyp", logEntries[0].TableName);
                }
            }
        }

        [TestMethod]
        public void TestGetAuditRecordsForChangeDeleted()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                string bezeichnung = Guid.NewGuid().ToString();
                var entity = RepositoryTest.Create<Produktmerkmal>();
                entity.Laenge = 1;
                entity.Bezeichnung = bezeichnung;
                entity.Merkmalsgruppe = RepositoryTest.Create<Merkmalsgruppe>();
                entity.Merkmalsgruppe.Bezeichnung = Guid.NewGuid().ToString();
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
                    Assert.AreEqual("MerkmalsgruppeId", logEntries[0].ColumnName);
                    Assert.AreEqual("M", logEntries[0].EventType);
                    Assert.AreEqual("0", logEntries[0].NewValue);
                    Assert.AreEqual("Produktmerkmal", logEntries[0].TableName);
                    break;
                }
            }
        }
    }
}
