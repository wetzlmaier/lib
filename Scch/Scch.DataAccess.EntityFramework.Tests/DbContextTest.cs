using System.ComponentModel.DataAnnotations;
using System.IO;
using Scch.Common.ComponentModel.DataAnnotations;
using Scch.DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.DomainModel.EntityFramework;
using Scch.DomainModel.EntityFramework.Mapping;

namespace Scch.DataAccess.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DbContextTest
    {
        public DbContextTest()
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
        public void TestCreateDatabaseScript()
        {
            DbContextHelper.Init();

            using (var context = DbContextManager<long>.CreateContext())
            {
                string script = context.CreateDatabaseScript();
                File.WriteAllText("script.sql", script);

                // Default
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy ADD CONSTRAINT DF_IndexDummy_Column2 DEFAULT 'DEF' FOR Column2"));
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy ADD CONSTRAINT DF_IndexDummy_Column3 DEFAULT 0 FOR Column3"));
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy ADD CONSTRAINT DF_IndexDummy_Column4 DEFAULT 0.1 FOR Column4"));

                // Index
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy ADD CONSTRAINT IDX1 UNIQUE NONCLUSTERED (Column1 ASC , Column2 ASC )"));
                Assert.IsTrue(script.Contains("CREATE NONCLUSTERED INDEX IDX2 ON IndexDummy (Column4 ASC , Column3 ASC )"));
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy ADD CONSTRAINT IDX3 UNIQUE NONCLUSTERED (Column6 ASC , Column5 ASC )"));
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy ADD CONSTRAINT IDX4 UNIQUE NONCLUSTERED (Column7 ASC , Column5 ASC )"));

                // Range
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy WITH CHECK ADD CONSTRAINT CK_IndexDummy_Column3 CHECK ((Column3 >= 0))"));
                Assert.IsTrue(script.Contains("ALTER TABLE IndexDummy CHECK CONSTRAINT CK_IndexDummy_Column3"));
            }
        }
    }

    public class IndexDummy : EntityFrameworkEntity<long>
    {
        [Index(IsUnique = true, IndexName = "IDX1")]
        [StringLength(50)]
        public string Column1 { get; set; }

        [Index(IsUnique = true, IndexName = "IDX1", OrdinalPoistion = 1)]
        [Default("DEF")]
        [StringLength(50)]
        public string Column2 { get; set; }

        [Index(IndexName = "IDX2", OrdinalPoistion = 1)]
        [Default(0)]
        [Range(0, int.MaxValue)]
        public int Column3 { get; set; }
        [Index(IndexName = "IDX2")]
        [Default(0.1)]
        public double Column4 { get; set; }

        [Index(IsUnique = true, IndexName = "IDX3", OrdinalPoistion = 1)]
        [Index(IsUnique = true, IndexName = "IDX4", OrdinalPoistion = 1)]
        public int Column5 { get; set; }

        [Index(IsUnique = true, IndexName = "IDX3")]
        [StringLength(50)]
        public string Column6 { get; set; }

        [Index(IsUnique = true, IndexName = "IDX4")]
        [StringLength(50)]
        public string Column7 { get; set; }
    }

    public class IndexDummyFrameworkEntityMapping : EntityFrameworkEntityMappingBase<long, IndexDummy>
    {
        public IndexDummyFrameworkEntityMapping()
        {
            Property(x => x.Column1);
            Property(x => x.Column2);
            Property(x => x.Column3);
            Property(x => x.Column4);
            Property(x => x.Column5);
            Property(x => x.Column6);
            Property(x => x.Column7);
        }
    }
}
