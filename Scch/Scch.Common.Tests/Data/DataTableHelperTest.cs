using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Common.Data;

namespace Scch.Common.Tests.Data
{
    [TestClass]
    public class DataTableHelperTest
    {
        DataTable _source;
        DataTable _destination;

        [TestInitialize]
        public void SetUp()
        {
            _source = CreateAndSetUpDataTable();
            _destination = CreateAndSetUpDataTable();

            _source.Rows.Add("SourceRow1", 1);
            _source.Rows.Add("SourceRow2", 2);
            _source.Rows.Add("SourceRow3", 3);

            _destination.Rows.Add("DestinationRow1", 1);
            _destination.Rows.Add("DestinationRow2", 2);
            _destination.Rows.Add("DestinationRow3", 3);
        }

        static DataTable CreateAndSetUpDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("String", typeof(string));
            dt.Columns.Add("Number", typeof(int));

            dt.PrimaryKey = new[] { dt.Columns["String"] };

            return dt;
        }

        [TestMethod]
        public void TestMoveRow()
        {
            DataRow row = _source.Rows.Find("SourceRow1");
            DataTableHelper.MoveRow(_source, row, _destination);

            Assert.IsNull(_source.Rows.Find("SourceRow1"));
            Assert.IsNotNull(_destination.Rows.Find("SourceRow1"));
            Assert.AreEqual(_destination.Rows.Count, _source.Rows.Count + 2);
        }

        [TestMethod]
        public void TestMoveRows()
        {
            DataTableHelper.MoveRows(_source, _source.Rows, _destination);

            Assert.AreEqual(0, _source.Rows.Count);
            Assert.AreEqual(6, _destination.Rows.Count);

            Assert.IsNotNull(_destination.Rows.Find("SourceRow1"));
            Assert.IsNotNull(_destination.Rows.Find("SourceRow2"));
            Assert.IsNotNull(_destination.Rows.Find("SourceRow3"));
        }

        [TestMethod]
        public void TestGenerateHtmlTableContent()
        {
            var result = DataTableHelper.GenerateHtmlTableContent(_source).Replace(Environment.NewLine, "");
            Assert.AreEqual("<tr><td>SourceRow1</td><td>1</td></tr><tr><td>SourceRow2</td><td>2</td></tr><tr><td>SourceRow3</td><td>3</td></tr>", result);
        }

        [TestMethod]
        public void TestGenerateHtmlTableHeader()
        {
            var result = DataTableHelper.GenerateHtmlTableHeader(_source).Replace(Environment.NewLine, "");
            Assert.AreEqual("<tr><th>String</th><th>Number</th></tr>", result);
        }

        [TestMethod]
        public void TestGenerateHtmlTable()
        {
            var result = DataTableHelper.GenerateHtmlTable(_source).Replace(Environment.NewLine, "");
            Assert.AreEqual("<table><tr><th>String</th><th>Number</th></tr><tr><td>SourceRow1</td><td>1</td></tr><tr><td>SourceRow2</td><td>2</td></tr><tr><td>SourceRow3</td><td>3</td></tr></table>", result);
        }

        class Person
        {
            [DisplayName("Name of Person")]
            public string Name { get; set; }
            [DisplayName("Age of Person")]
            public int Age { get; set; }
        }

        [TestMethod]
        public void TestCreateDataTable()
        {
            var dt = DataTableHelper.CreateDataTable(typeof(Person), "Name of Person");
            Assert.AreEqual(2, dt.Columns.Count);
            Assert.AreEqual("Name", dt.Columns[0].ColumnName);
            Assert.AreEqual("Name of Person", dt.Columns[0].Caption);
            Assert.AreEqual(typeof(string), dt.Columns[0].DataType);

            dt = DataTableHelper.CreateDataTable(typeof(Person));
            Assert.AreEqual(3, dt.Columns.Count);
            Assert.AreEqual("Age", dt.Columns[0].ColumnName);
            Assert.AreEqual("Age of Person", dt.Columns[0].Caption);
            Assert.AreEqual(typeof(int), dt.Columns[0].DataType);
            Assert.AreEqual("Name", dt.Columns[1].ColumnName);
            Assert.AreEqual("Name of Person", dt.Columns[1].Caption);
            Assert.AreEqual(typeof(string), dt.Columns[1].DataType);
        }

        [TestMethod]
        public void TestFillDataTable()
        {
            var persons = new List<Person>
            {
                new Person {Name = "Person1", Age = 1},
                new Person {Name = "Person2", Age = 2},
                new Person {Name = "Person3", Age = 3}
            };

            var dt = DataTableHelper.CreateDataTable(typeof(Person));
            DataTableHelper.FillDataTable(dt, persons);
            Assert.AreEqual(2, dt.Rows[1].ItemArray[0]);
            Assert.AreEqual("Person2", dt.Rows[1].ItemArray[1]);
        }

        [TestMethod]
        public void TestGenerateCsvHeader()
        {
            var result = DataTableHelper.GenerateCsvHeader(_source);
            Assert.AreEqual("\"String\";\"Number\"" + Environment.NewLine, result);
        }

        [TestMethod]
        public void TestGenerateCsvContent()
        {
            var result = DataTableHelper.GenerateCsvContent(_source);
            Assert.AreEqual("\"SourceRow1\";1" + Environment.NewLine + "\"SourceRow2\";2" + Environment.NewLine + "\"SourceRow3\";3" + Environment.NewLine, result);
        }

        [TestMethod]
        public void TestLoadSaveCsv()
        {
            DataTableHelper.SaveAsCsvFile(_source, "test.csv");
            var dt = DataTableHelper.LoadFromCsvFile(new FileInfo("test.csv"));

            Assert.AreEqual(_source.Columns.Count, dt.Columns.Count);

            for (int i = 0; i < _source.Columns.Count; i++)
            {
                Assert.AreEqual(_source.Columns[i].Caption, dt.Columns[i].Caption);
            }

            Assert.AreEqual(_source.Rows.Count, dt.Rows.Count);

            for (int row = 0; row < _source.Rows.Count; row++)
            {
                for (int column = 0; column < _source.Columns.Count; column++)
                {
                    Assert.AreEqual(_source.Rows[row].ItemArray[column].ToString(), dt.Rows[row].ItemArray[column]);
                }
            }
        }

        [TestMethod]
        public void TestGenerateTextHeader()
        {
            var result = DataTableHelper.GenerateTextHeader(_source, new[] { 10, 10 });
            Assert.AreEqual("| String     | Number     |" + Environment.NewLine, result);
        }

        [TestMethod]
        public void TestGenerateTextContent()
        {
            var result = DataTableHelper.GenerateTextContent(_source, new[] { 10, 10 });
            Assert.AreEqual("| SourceRow1 | 1          |" + Environment.NewLine + "| SourceRow2 | 2          |" + Environment.NewLine + "| SourceRow3 | 3          |" + Environment.NewLine, result);

            result = DataTableHelper.GenerateTextContent(_source, new[] { 9, 9 });
            Assert.AreEqual("| Source... | 1         |" + Environment.NewLine + "| Source... | 2         |" + Environment.NewLine + "| Source... | 3         |" + Environment.NewLine, result);
        }

        [TestMethod]
        public void TestGenerateText()
        {
            var result = DataTableHelper.GenerateText(_source);
            Assert.AreEqual("| String     | Number |" + Environment.NewLine + "| SourceRow1 | 1      |" + Environment.NewLine + "| SourceRow2 | 2      |" + Environment.NewLine + "| SourceRow3 | 3      |" + Environment.NewLine, result);
        }
    }
}
