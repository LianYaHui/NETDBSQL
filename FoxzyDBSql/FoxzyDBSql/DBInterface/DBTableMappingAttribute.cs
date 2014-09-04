using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public class DBTableMappingAttribute : Attribute
    {
        public String DBTableName { set; get; }

        public DBTableMappingAttribute(String name)
        {
            this.DBTableName = name;
        }
    }
}
