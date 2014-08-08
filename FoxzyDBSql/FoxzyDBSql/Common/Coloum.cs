using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
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
