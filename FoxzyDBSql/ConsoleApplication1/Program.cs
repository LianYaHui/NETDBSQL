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


            string sql = db.CreateSelect()
                 .From("h_emp_mstr as tb_emp")
                 .Select("p.EmpID,tb_emp.post_id,count(*)")
                 .OrderBy("tb_emp.emp_id")
                 .InnerJoin("P_Plan p")
                 .On("p.EmpID=tb_emp.emp_id")
                 .Where("p.EmpID=@emp")
                 .GropuBy("p.EmpID", "tb_emp.post_id")
                 .Having("count(*) >= 10")
                 .ToSql();

            Console.WriteLine(sql);

            int sql2 = db.CreateUpdate("P_Plan")
                  .SetObject(new { PostID = "1234", Remark = "Foxzy" })
                  .Where("ID=4000")
                  .Set("EmpID=@lian")
                  .SetParameter("@lian", "123455555")   
                  .ExecuteNonQuery();

            Console.WriteLine(sql2);

            Console.ReadKey();
        }



        public static List<int[]> A(List<int[]> a, List<int[]> b)
        {
            int maxLen = Math.Max(a.Count, b.Count);

            List<int[]> result = new List<int[]>();

            for (int i = 0; i < maxLen; i++)
            {
                int[] _a = null, _b = null;
                if (a.Count > i) _a = a[i];
                else _a = new int[0];

                if (b.Count > i) _b = b[i];
                else _b = new int[0];

                result.Add(_a.Concat(_b).ToArray());
            }

            return result;
        }
    }
}
