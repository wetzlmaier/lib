using System;
using System.Data.Entity.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.DomainModel.EntityFramework.Tests;

namespace Scch.DataAccess.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for UnitOfWorkTest
    /// </summary>
    [TestClass]
    public class UnitOfWorkTest
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
        public void TestCommit()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                
                string bezeichnung = Guid.NewGuid().ToString();
                var entity = RepositoryTest.Create<Anweisungstyp>(); 
                entity.Bezeichnung = bezeichnung;
                repository.ApplyChanges(entity);
                repository.UnitOfWork.CommitTransaction();

                Assert.AreEqual(bezeichnung, repository.Single<Anweisungstyp>(at => at.Bezeichnung == bezeichnung).Bezeichnung);
            }
        }

        [TestMethod]
        public void TestRollback()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                string bezeichnung = Guid.NewGuid().ToString();
                var entity = RepositoryTest.Create<Anweisungstyp>();
                entity.Bezeichnung = bezeichnung;
                repository.ApplyChanges(entity);
                repository.UnitOfWork.RollBackTransaction();

                Assert.IsNull(repository.SingleOrDefault<Anweisungstyp>(at => at.Bezeichnung == bezeichnung));
            }
        }

        [TestMethod]
        public void TestOptimisticLocking()
        {
            DbContextHelper.Init();

            using (var context1 = DbContextManager<long>.CreateContext())
            {
                var repository1 = new EntityFrameworkRepository<long>(context1);

                string bezeichnung = Guid.NewGuid().ToString();
                var entity1 = RepositoryTest.Create<Anweisungstyp>();
                entity1.Bezeichnung = bezeichnung;
                repository1.ApplyChanges(entity1);
                repository1.UnitOfWork.SaveChanges();

                using (var context2 = DbContextManager<long>.CreateContext())
                {
                    var repository2 = new EntityFrameworkRepository<long>(context2);

                    var entity2 = repository2.Single<Anweisungstyp>(at => at.Bezeichnung == bezeichnung);
                    entity2.Bezeichnung = Guid.NewGuid().ToString();
                    repository2.UnitOfWork.SaveChanges();
                }

                entity1.Bezeichnung = Guid.NewGuid().ToString();
                Assert.ThrowsExactly<DbUpdateConcurrencyException>(()=>repository1.UnitOfWork.SaveChanges());
            }
        }
    }
}
