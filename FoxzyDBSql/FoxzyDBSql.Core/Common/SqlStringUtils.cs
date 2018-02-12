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

        public static string GetFieldName(string ParameterName, string placeHolder)
        {
            if (string.IsNullOrEmpty(placeHolder))
                throw new NullReferenceException("placeHolder");
            if (string.IsNullOrEmpty(ParameterName))
                throw new NullReferenceException("ParameterName");

            return ParameterName.TrimStart(placeHolder.ToCharArray());
        }
    }
}
