using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class EntityConvert
    {
        DataTable parsTable = null;

        public EntityConvert(DataTable table)
        {
            if (table == null)
                throw new NullReferenceException(nameof(table));
            this.parsTable = table;
        }

        public List<TEntity> ToEntity<TEntity>(Dictionary<string, Func<DataRow, object>> farmat = null) where TEntity : new()
        {
            List<TEntity> resultList = new List<TEntity>();
            Type entityType = typeof(TEntity);

            farmat = farmat ?? new Dictionary<string, Func<DataRow, object>>();

            foreach (DataRow row in parsTable.Rows)
            {
                TEntity entity = new TEntity();

                foreach (var enProperty in entityType.GetProperties())
                {
                    string FieldName = enProperty.Name;
                    object FiledValue = null;

                    try
                    {
                        if (farmat.ContainsKey(FieldName))
                        {
                            var farmatAction = farmat[FieldName];
                            if (farmatAction != null)
                                FiledValue = farmatAction.Invoke(row);
                        }
                        else
                        {
                            if (parsTable.Columns.Contains(FieldName))
                                FiledValue = row[enProperty.Name];
                        }

                        if (FiledValue != null)
                            enProperty.SetValue(entity, FiledValue, null);
                    }
                    catch
                    {
                        //跳过为此属性赋值
                        continue;
                    }
                }

                resultList.Add(entity);
            }

            return resultList;
        }
    }
}
