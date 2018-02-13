using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public static class EntityHelper
    {
        /// <summary>
        /// 将DataTable转化为实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sourceTable"></param>
        /// <param name="farmat"></param>
        /// <returns></returns>
        public static List<TEntity> ToEntity<TEntity>(this DataTable sourceTable, Dictionary<string, Func<DataRow, object>> farmat = null) where TEntity : new()
        {
            List<TEntity> resultList = new List<TEntity>();
            Type entityType = typeof(TEntity);

            farmat = farmat ?? new Dictionary<string, Func<DataRow, object>>();

            foreach (DataRow row in sourceTable.Rows)
            {
                TEntity entity = new TEntity();

                foreach (var enProperty in entityType.GetProperties())
                {
                    string FieldName = enProperty.Name;
                    object FiledValue = null;
                    string farmatName = FieldName;

                    try
                    {
                        FiledValue = enProperty.GetValueFromDataRow(row, farmat);

                        if (FiledValue != null)
                            enProperty.SetValue(entity, FiledValue, null);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"【{DateTime.Now}】【EntityHelper.ToEntity()】- 【Entity:{entityType.FullName}】-【{FieldName}】 赋值错误，FiledValue = {FiledValue},Error:{ex.Message}");

                        //跳过为此属性赋值
                        continue;
                    }
                }

                try
                {
                    var Verifications = entityType.GetCustomAttributes(true)
                                      .Where(attr => attr is EntityVerificationAttribute);


                    var _verifications = Verifications.OfType<EntityVerificationAttribute>().OrderBy(attr => attr.ExecuteIndex);
                    foreach (EntityVerificationAttribute verifica in _verifications)
                    {
                        verifica.Verification(row, entity);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"【{DateTime.Now}】【EntityHelper.ToEntity()】- 【Entity:{entityType.FullName}】 验证错误，Error:{ex.Message}");
                    //跳过为此属性赋值
                    continue;
                }


                resultList.Add(entity);
            }

            return resultList;
        }

        /// <summary>
        /// 将DataTable转为List<Dictionary<string, object>>
        /// 一般用于序列化数据库
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToDictionaryList(this DataTable sourceTable)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            foreach (DataRow row in sourceTable.Rows)
            {
                Dictionary<string, object> entity = new Dictionary<string, object>();

                foreach (DataColumn colunm in sourceTable.Columns)
                {
                    string colName = colunm.ColumnName;
                    object FiledValue = row[colunm];

                    entity.Add(colName, FiledValue);
                }

                resultList.Add(entity);
            }

            return resultList;
        }
    }
}
