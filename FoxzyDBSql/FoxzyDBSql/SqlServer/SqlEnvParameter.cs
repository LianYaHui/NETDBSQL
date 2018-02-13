using System;
using System.Collections.Generic;
using System.Text;

namespace FoxzyDBSql.SqlServer
{

    /// <summary>
    /// mysql环境参数
    /// </summary>
    public static class SqlEnvParameter
    {
        private static string __ParametersPlaceholder = null;
        private static SqlParameterConvert __ParameterConvert = null;

        static SqlEnvParameter()
        {
            __ParametersPlaceholder = "@";
            __ParameterConvert = new SqlParameterConvert();
        }

        /// <summary>
        /// 参数化前导符号
        /// </summary>
        public static string ParametersPlaceholder
        {
            get => __ParametersPlaceholder;
        }

        public static SqlParameterConvert ParameterConvert
        {
            get => __ParameterConvert;
        }

    }
}
