using System;
using System.Collections.Generic;
using System.Linq;
using FoxzyDBSql.DBInterface;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FoxzyDBSql.Common;
using MySql.Data.MySqlClient;

namespace FoxzyDBSql.MySql
{
    /// <summary>
    /// Sql数据库的参数转化器
    /// </summary>
    public class MySqlParameterConvert : IDbParameterConvert
    {
        private List<string> fieldsList = new List<string>();

        public static string ParametersPlaceholder { get; set; }

        public IEnumerable<string> GetFields
        {
            get
            {
                return fieldsList;
            }
        }

        public IEnumerable<IDataParameter> FromDictionaryToParameters(Dictionary<string, object> objPara)
        {
            fieldsList.Clear();

            List<MySqlParameter> ListParas = new List<MySqlParameter>();
            if (objPara == null)
                return ListParas;

            foreach (var d in objPara)
            {
                fieldsList.Add(d.Key);
                ListParas.Add(new MySqlParameter(ParametersPlaceholder + d.Key, d.Value));
            }
            return ListParas;
        }

        public IEnumerable<IDataParameter> FromObjectToParameters(object objPara, params string[] ignoreFields)
        {
            fieldsList.Clear();

            List<MySqlParameter> ListParas = new List<MySqlParameter>();
            if (objPara == null)
                return ListParas;

            Type enEntityType = objPara.GetType();
            var properties = enEntityType.GetProperties();
            foreach (var p in properties)
            {
                if (ignoreFields != null && ignoreFields.Contains(p.Name))
                    continue;


                var ValueConvert = p.GetCustomAttributes(true).FirstOrDefault(attr => attr is ValueConvertAttribute);

                object val = p.GetValue(objPara, null);

                if (ValueConvert != null)
                {
                    val = (ValueConvert as ValueConvertAttribute).ConverToParameter(objPara, val);
                }

                if (val == null)
                    continue;

                fieldsList.Add(p.Name);

                ListParas.Add(new MySqlParameter(ParametersPlaceholder + p.Name, val));
            }

            return ListParas;
        }
    }
}
