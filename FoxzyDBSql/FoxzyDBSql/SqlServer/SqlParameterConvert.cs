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

                object val = null;
                if (ValueConvert == null)
                {
                    val = p.GetValue(objPara, null);
                }
                else
                {
                    string convertFunctionName = (ValueConvert as ValueConvertAttribute).ConverToParameter;

                    var methodInfo = enEntityType.GetMethod(convertFunctionName);

                    if (methodInfo == null)
                        throw new Exception($"反射{enEntityType.FullName}.{p.Name}找不到指定特性 ValueConvertAttribute 所指向的ConverFrom 属性的方法{convertFunctionName},请确保该类中具有公开的方法{convertFunctionName}");

                    val = methodInfo.Invoke(objPara, null);
                }

                if (val == null)
                    continue;

                ListParas.Add(new SqlParameter("@" + p.Name, val));
            }

            return ListParas;
        }
    }
}
