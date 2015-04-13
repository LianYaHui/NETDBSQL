using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public static class SqlConverter
    {
        public static List<T> ToList<T>(this DataTable table, Func<DataRow, T> render)
        {
            if (render == null)
                throw new NullReferenceException("render");

            List<T> data = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                data.Add(render.Invoke(row));
            }

            return data;
        }
    }
}
