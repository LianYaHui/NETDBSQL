using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public interface IDbParameterConvert
    {
        /// <summary>
        /// 从实例对象反射成为dbParameters参数集
        /// </summary>
        /// <param name="objPara"></param>
        /// <returns></returns>
        IEnumerable<IDataParameter> FromObjectToParameters(object objPara, int index, params string[] ignoreFields);

        /// <summary>
        /// 从字典对象反射成为dbParameters参数集
        /// </summary>
        /// <param name="objPara"></param>
        /// <returns></returns>
        IEnumerable<IDataParameter> FromDictionaryToParameters(IDictionary<string, object> objPara, int index);
    }
}
