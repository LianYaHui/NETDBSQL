using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public class DBFunction
    {
        public string Max(String field)
        {
            return String.Format("MAX({0})", field);
        }

        public string Min(String field)
        {
            return String.Format("Min({0})", field);
        }

        public string Count(String field)
        {
            return String.Format("Count({0})", field);
        }

    }
}
