using FoxzyDBSql.Common;
using FoxzyDBSql.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SqlManageUtil();
            SqlManageUtil.ConncetionString = "Data Source=.;Initial Catalog=YODO;Integrated Security=True";


            //string sql = db.CreateSelect()
            //     .From("h_emp_mstr as tb_emp")
            //     .Select("p.EmpID,tb_emp.post_id,count(*)")
            //     .OrderBy("tb_emp.emp_id")
            //     .InnerJoin("P_Plan p")
            //     .On("p.EmpID=tb_emp.emp_id")
            //     .Where("p.EmpID=@emp")
            //     .GropuBy("p.EmpID", "tb_emp.post_id")
            //     .Having("count(*) >= 10")
            //     .ToSql();


            db.CreateUpdate("P_Plan")
                .Set("remark=@r,TheoryCreateEmp=@t")
                .SetParameter(new SqlParameter("@r", "Foxzy"))
                .Set("lock=0")
                .Where("ID=4000")
                .SetParameter("@t", "lianF")
                .ExecuteNonQuery();

            Console.WriteLine("");



            Console.ReadKey();
        }
    }
}
