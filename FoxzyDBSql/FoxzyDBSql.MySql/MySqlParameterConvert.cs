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
        public string __ParametersPlaceholder = MySqlEnvParameter.ParametersPlaceholder;



        public IEnumerable<IDataParameter> FromDictionaryToParameters(IDictionary<string, object> objPara, int index)
        {
            List<MySqlParameter> ListParas = new List<MySqlParameter>();
            if (objPara == null)
                return ListParas;

            foreach (var d in objPara)
            {
                ListParas.Add(new MySqlParameter(SqlStringUtils.BuilderParameterName(__ParametersPlaceholder, d.Key, index), d.Value)
                {
                    SourceColumn = d.Key
                });
            }
            return ListParas;
        }

        public IEnumerable<IDataParameter> FromObjectToParameters(object objPara, int index, params string[] ignoreFields)
        {
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

                ListParas.Add(new MySqlParameter(SqlStringUtils.BuilderParameterName(__ParametersPlaceholder, p.Name, index), val)
                {
                    SourceColumn = p.Name
                });
            }

            return ListParas;
        }
    }
}
