using System;
using System.Collections.Generic;
using System.Text;

namespace FoxzyDBSql.MySql
{

    /// <summary>
    /// mysql环境参数
    /// </summary>
    public static class MySqlEnvParameter
    {
        private static string __ParametersPlaceholder = null;
        private static MySqlParameterConvert __ParameterConvert = null;

        static MySqlEnvParameter()
        {
            __ParametersPlaceholder = "?";
            __ParameterConvert = new MySqlParameterConvert();
        }

        /// <summary>
        /// 参数化前导符号
        /// </summary>
        public static string ParametersPlaceholder
        {
            get => __ParametersPlaceholder;
        }


        public static MySqlParameterConvert ParameterConvert
        {
            get => __ParameterConvert;
        }

    }
}
