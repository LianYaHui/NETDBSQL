using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FoxzyDBSql.Common
{
    public static class SqlString
    {
        public static String ReplaceSpace(this String sourceTest)
        {
            return Regex.Replace(sourceTest, " {2,}", " ");
        }
    }
}
