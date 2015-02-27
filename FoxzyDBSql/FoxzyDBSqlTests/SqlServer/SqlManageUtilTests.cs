using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxzyDBSql.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
namespace FoxzyDBSql.SqlServer.Tests
{
    [TestClass()]
    public class SqlManageUtilTests
    {
        SqlManageUtil db = new SqlManageUtil("Data Source=.;Initial Catalog=HRM_XX;Integrated Security=True");


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
            int result = db.CreateInsert("xx_Log").SetObject(new
            {
                ID = -100
                ,
                Contont = "test1",
                CreateTime = DateTime.Now
            }).ExecuteNonQuery();

            Assert.AreEqual(result, 1);
        }

        [TestMethod()]
        public void CreatePaginationTest()
        {
            int c = 0;

            db.CreatePagination()
                .Set("SELect * from xx_employee", null)
                .Pagination(2, 5, out c, "ID");

            Assert.AreEqual(c, 24);

        }

        [TestMethod()]
        public void ExecTranstionTest()
        {
            Action<FoxzyDBSql.DBInterface.DbManage> action = (dbUtil) =>
            {
                dbUtil.CreateInsert("xx_Log").SetObject(new
                {
                    Contont = "test12",
                    CreateTime = DateTime.Now,
                    ID = -126
                }).ExecuteNonQuery();

                dbUtil.CreateInsert("xx_Log").SetObject(new
                {
                    Contont = "test22",
                    CreateTime = DateTime.Now,
                    ID = -127
                }).ExecuteNonQuery();

                dbUtil.CreateInsert("xx_Log").SetObject(new
                {
                    Contont = "test333333",
                    CreateTime = DateTime.Now,
                    ID = -128
                }).ExecuteNonQuery();

            };

            var f = db.ExecTranstion(action);

            Assert.AreEqual<bool>(f, true);
        }
    }
}
