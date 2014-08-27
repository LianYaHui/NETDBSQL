using FoxzyDBSql.Common;
using FoxzyDBSql.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SqlManageUtil();


            string sql = db.CreateExpression()
                 .From("h_emp_mstr as tb_emp")
                 .Select()
                 .OrderBy("tb_emp.emp_id")
                 .LeftJoin("P_Plan p")
                 .On("p.EmpID=tb_emp.emp_id")
                 .ToSql();

            Console.WriteLine(sql);

            Console.ReadKey();
        }
    }
}
