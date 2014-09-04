using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public class DBColunmMappingAttribute : Attribute
    {
        public String ColounmName { set; get; }

        public bool IsDBKey { set; get; }

        public DBColunmMappingAttribute(string name)
        {
            this.ColounmName = name;
        }
    }
}
