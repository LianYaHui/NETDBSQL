using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxzyDBSql.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
namespace FoxzyDBSql.SqlServer.Tests
{
    [TestClass()]
    public class SqlManageUtilTests
    {
        static SqlManageUtil db = new SqlManageUtil("Data Source=.;Initial Catalog=HRM_XX;Integrated Security=True");


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
            var sql = db.CreateSelect()
                 .Select("ID", "EmpName")
                 .LeftJoin("xx_EndowmentInsurance i")
                 .On("i.EmpID = e.ID")
                 .From("xx_Employee     e")
                 .ToSql();


            Assert.Fail();
        }

        [TestMethod()]
        public void CreateUpdateTest()
        {
            var sql = db.CreateUpdate("xx_Employee")
                .SetObject(new
                {
                    EmpName = "EmpTest"
                }).ExecuteNonQuery();

            var dt = db.CreateSelect()
                .Select()
                .From("xx_Employee")
                .ToDataSet();


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

            List<IDataParameter>
                pars = new List<IDataParameter>()
                {
                    new SqlParameter("@v","汉族")
                };

            //db.CreatePagination()
            //    .Set("select * from XX_employee where Volk =@v", pars)
            //    .Pagination(2, 5, out c, "ID");

            int count = 0;

            var dt = db.CreateSelect()
                .From("XX_employee")
                .OrderBy("ID")
                .OrderByDesc("Name")
                .Select()
                .Where("Volk =@v")
                .SetParameter(pars)
                .Pagination(1, 10, out  count);

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
