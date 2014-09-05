using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class DBWhereComponent
    {
        public String TableName { set; get; }

        public DBWhereComponent(IEnumerable<DBExpression> expressions, string table)
        {
            Expressions = expressions.ToList();
            TableName = table;
        }

        public List<DBExpression> Expressions22 { set; get; }
    }
}
