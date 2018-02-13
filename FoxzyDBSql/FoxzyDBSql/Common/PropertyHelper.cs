using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Data;

namespace FoxzyDBSql.Common
{
    public static class PropertyHelper
    {
        /// <summary>
        /// 从DataRow中获取自定义的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="row"></param>
        /// <param name="farmat"></param>
        /// <returns></returns>
        public static object GetValueFromDataRow(this PropertyInfo property,
            DataRow row, Dictionary<string, Func<DataRow, object>> farmat)
        {
            string FieldName = property.Name;
            object FiledValue = null;
            string farmatName = FieldName;


            var entityCustomAttributes = property.GetCustomAttributes(true);
            var ValueConverts = entityCustomAttributes
                                .Where(attr => attr is ValueConvertAttribute);


            if (row.Table.Columns.Contains(FieldName))
                FiledValue = row[FieldName];

            var _valConverts = ValueConverts.OfType<ValueConvertAttribute>().OrderBy(attr => attr.ExecuteIndex);
            foreach (ValueConvertAttribute convert in _valConverts)
            {
                FiledValue = convert.ConvertToModel(row, FiledValue);
            }

            if (farmat.ContainsKey(FieldName))
            {
                FiledValue = farmat[FieldName](row);
            }


            return FiledValue;
        }
    }
}
