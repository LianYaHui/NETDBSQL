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
                 .From("h_emp_mstr")
                 .Select()
                 .ToSql();

            Console.WriteLine(sql);

        }
    }
}
