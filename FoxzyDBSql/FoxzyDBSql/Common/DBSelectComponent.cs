using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class DBSelectComponent
    {
        public String TableName { set; get; }

        public IEnumerable<Coloum> Colunms { set; get; }

        public DBSelectComponent(IEnumerable<Coloum> colunms, String tableName = null)
        {
            this.Colunms = colunms;
            this.TableName = tableName;
        }

        public class Coloum
        {
            public String ColunmName { set; get; }

            public String AsName { set; get; }

            public Coloum(String colunmName, String asName = null)
            {
                this.ColunmName = colunmName;
                this.AsName = asName;
            }
        }
    }
}
