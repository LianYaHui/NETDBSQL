using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class DBSelectComponent
    {
        public String TableName { set; get; }

        public List<Coloum> Colunms { set; get; }

        public DBSelectComponent(IEnumerable<Coloum> colunms, String tableName = null)
        {
            this.TableName = tableName;
        }
    }
}
