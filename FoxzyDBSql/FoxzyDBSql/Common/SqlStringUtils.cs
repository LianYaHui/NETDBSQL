using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FoxzyDBSql.Common
{
    public static class SqlStringUtils
    {
        public static String ReplaceSpace(string sourceTest)
        {
            return Regex.Replace(sourceTest, " {2,}", " ");
        }



        public static string BuilderParameterName(string placeholder, string name, int index)
        {
            string result = string.Empty;

            if (index > 0)
            {
                result = $"{placeholder}{name}_{index}";
            }
            else
            {
                result = $"{placeholder}{name}";
            }

            return result;
        }
    }
}
