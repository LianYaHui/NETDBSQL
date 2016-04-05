using System;
using System.Collections.Generic;
using System.Linq;
using FoxzyDBSql.DBInterface;
using System.Text;
using System.Data;
using System.Data.SqlClient;

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

        public IEnumerable<IDataParameter> FromObjectToParameters(object objPara)
        {
            List<SqlParameter> ListParas = new List<SqlParameter>();
            if (objPara == null)
                return ListParas;

            var properties = objPara.GetType().GetProperties();
            foreach (var p in properties)
            {
                object val = p.GetValue(objPara, null);
                ListParas.Add(new SqlParameter("@" + p.Name, val));
            }

            return ListParas;
        }
    }
}
