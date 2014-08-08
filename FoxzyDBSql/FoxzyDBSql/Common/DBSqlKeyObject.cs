using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class DBSqlKeyObject
    {
        public IEnumerable<DBSelectComponent> Selects { set; get; }
        public String SelectStr { set; get; }

        public String FromTable { set; get; }

        public Dictionary<String, String> Tables { set; get; }

        public DBSqlKeyObject()
        {
            Tables = new Dictionary<string, string>();
        }

        public static DBSqlKeyObject Create()
        {
            return new DBSqlKeyObject();
        }
    }
}
