using System;
using System.Collections.Generic;
using System.Linq;
using FoxzyDBSql.DBInterface;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FoxzyDBSql.Common;

namespace FoxzyDBSql.SqlServer
{
    /// <summary>
    /// Sql数据库的参数转化器
    /// </summary>
    public class SqlParameterConvert : IDbParameterConvert
    {
        public IEnumerable<IDataParameter> FromDictionaryToParameters(Dictionary<string, object> objPara)
        {
            List<SqlParameter> ListParas = new List<SqlParameter>();
            if (objPara == null)
                return ListParas;

            foreach (var d in objPara)
                ListParas.Add(new SqlParameter("@" + d.Key, d.Value));

            return ListParas;
        }

        public IEnumerable<IDataParameter> FromObjectToParameters(object objPara, params string[] ignoreFields)
        {
            List<SqlParameter> ListParas = new List<SqlParameter>();
            if (objPara == null)
                return ListParas;

            Type enEntityType = objPara.GetType();
            var properties = enEntityType.GetProperties();
            foreach (var p in properties)
            {
                if (ignoreFields.Contains(p.Name))
                    continue;


                var ValueConvert = p.GetCustomAttributes(true).FirstOrDefault(attr => attr is ValueConvertAttribute);

                object val = p.GetValue(objPara, null);

                if (ValueConvert != null)
                {
                    val = (ValueConvert as ValueConvertAttribute).ConverToParameter(objPara, val);
                }

                if (val == null)
                    continue;

                ListParas.Add(new SqlParameter("@" + p.Name, val));
            }

            return ListParas;
        }
    }
}
