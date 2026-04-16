using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Scch.DomainModel.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.DomainModel.EntityFramework.Tests;

namespace Scch.DataAccess.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for RepositoryTest
    /// </summary>
    [TestClass]
    public class RepositoryTest
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
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
/*
        [TestMethod]
        public void TestTemplate()
        {
            DbContextHelper.Init();

            using (ExtendedDbContext context = DbContextManager<long>.CreateContext())
            {
                IRepository repository = new GenericRepository(context);

                
            }
        }
*/

        Produkt CreateProdukt(string bezeichnung)
        {
            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();

                var produktGruppe = Create<Produktgruppe>();
                produktGruppe.Bezeichnung = bezeichnung;
                var produkt = Create<Produkt>();
                produkt.Bezeichnung = bezeichnung;
                produkt.Nummer = bezeichnung;
                produkt.Produktgruppe = produktGruppe;

                repository.ApplyChanges(produkt);
                repository.UnitOfWork.CommitTransaction();

                return produkt;
            }
        }

        public static TEntity Create<TEntity>() where TEntity:EntityFrameworkEntity<long>, new()
        {
            var entity = new TEntity();
            //entity.StartTracking();
            return entity;
        }

        [TestMethod]
        public void TestFlatObject()
        {
            DbContextHelper.Init();

            string bezeichnung = Guid.NewGuid().ToString();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                var entity = Create<Anweisungstyp>();
                entity.Bezeichnung = bezeichnung;

                repository.UnitOfWork.BeginTransaction();
                repository.ApplyChanges(entity);
                repository.UnitOfWork.CommitTransaction();

                Assert.AreEqual(1, (from g in repository.GetQuery<Anweisungstyp>() where g.Bezeichnung == bezeichnung select g).Count());
            }
        }

        [TestMethod]
        public void TestNewObjectGraph()
        {
            DbContextHelper.Init();

            string bezeichnung = Guid.NewGuid().ToString();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();

                var produktGruppe = Create<Produktgruppe>();
                produktGruppe.Bezeichnung = bezeichnung;
                var produkt = Create<Produkt>();
                produkt.Bezeichnung = bezeichnung;
                produkt.Nummer = bezeichnung;
                produkt.Produktgruppe = produktGruppe;
               
                repository.ApplyChanges(produkt);
                repository.UnitOfWork.CommitTransaction();

                Assert.AreEqual(1, (from p in repository.GetQuery<Produkt>() where p.Bezeichnung == bezeichnung select p).Count());
                Assert.AreEqual(1, (from g in repository.GetQuery<Produktgruppe>() where g.Bezeichnung == bezeichnung select g).Count());
            }
        }

        [TestMethod]
        public void TestObjectGraph()
        {
            DbContextHelper.Init();

            string bezeichnung = Guid.NewGuid().ToString();
            CreateProdukt(bezeichnung);

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                var version = Create<Produktversion>();
                version.Produkt = repository.GetQuery<Produkt>().Include(p => p.Produktgruppe).First(p => p.Bezeichnung == bezeichnung);
                version.Nummer = 1;
                version.Kommentar = bezeichnung;
                repository.ApplyChanges(version);
                repository.UnitOfWork.CommitTransaction();

                Assert.AreEqual(1, (from g in repository.GetQuery<Produktgruppe>() where g.Bezeichnung == bezeichnung select g).Count());
                Assert.AreEqual(1, (from p in repository.GetQuery<Produkt>() where p.Bezeichnung == bezeichnung select p).Count());
            }
        }

        [TestMethod]
        [Ignore]
        public void TestChangeObjectGraph()
        {
            DbContextHelper.Init();

            string bezeichnung = Guid.NewGuid().ToString();
            CreateProdukt(bezeichnung);

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                var version = Create<Produktversion>();
                var query = repository.GetQuery<Produkt>().Include(p => p.Produktgruppe);
                query.MergeOption = MergeOption.NoTracking;
                version.Produkt=query.First(p => p.Bezeichnung == bezeichnung);

                version.Nummer = 1;
                version.Kommentar = bezeichnung;
                repository.ApplyChanges(version);
                repository.UnitOfWork.CommitTransaction();

                Assert.AreEqual(1, (from g in repository.GetQuery<Produktgruppe>() where g.Bezeichnung == bezeichnung select g).Count());
                Assert.AreEqual(1, (from p in repository.GetQuery<Produkt>() where p.Bezeichnung == bezeichnung select p).Count());
            }

            string bezeichnung2 = Guid.NewGuid().ToString();

            using (var context = DbContextManager<long>.CreateContext())
            {
                var repository = new EntityFrameworkRepository<long>(context);

                repository.UnitOfWork.BeginTransaction();
                var query = repository.GetQuery<Produktversion>().Include(v => v.Produkt.Produktgruppe);
                query.MergeOption = MergeOption.NoTracking;
                var version=query.First(p => p.Kommentar == bezeichnung);
                //version.StartTracking();

                var produktGruppe = Create<Produktgruppe>();
                produktGruppe.Bezeichnung = bezeichnung2;
                var produkt = Create<Produkt>();
                produkt.Bezeichnung = bezeichnung2;
                produkt.Nummer = bezeichnung2;
                produkt.Produktgruppe = produktGruppe;
                version.Produkt = produkt;
                
                repository.ApplyChanges(version);
                repository.UnitOfWork.CommitTransaction();

                Assert.AreEqual(1, (from g in repository.GetQuery<Produktgruppe>() where g.Bezeichnung == bezeichnung select g).Count());
                Assert.AreEqual(1, (from p in repository.GetQuery<Produkt>() where p.Bezeichnung == bezeichnung select p).Count());

                Assert.AreEqual(1, (from g in repository.GetQuery<Produktgruppe>() where g.Bezeichnung == bezeichnung2 select g).Count());
                Assert.AreEqual(1, (from p in repository.GetQuery<Produkt>() where p.Bezeichnung == bezeichnung2 select p).Count());
            }
        }
    }
}
