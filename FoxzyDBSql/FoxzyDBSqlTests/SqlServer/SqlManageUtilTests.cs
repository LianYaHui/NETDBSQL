using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxzyDBSql.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FoxzyDBSql.SqlServer.Tests
{
    [TestClass()]
    public class SqlManageUtilTests
    {
        [TestMethod()]
        public void SqlManageUtilTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CloneParameterTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OpenConncetionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteDataReaderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteScalarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteScalarTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisposeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FillDataSetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateSelectTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateUpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateDeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateInsertTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreatePaginationTest()
        {
            SqlManageUtil db = new SqlManageUtil("Data Source=.;Initial Catalog=HRM_XX;Integrated Security=True");
            int c = 0;

            db.CreatePagination()
                .Set("SELect * from xx_employee", null)
                .Pagination(2, 5, out c, "ID");

            Assert.AreEqual(c, 24);

        }
    }
}
