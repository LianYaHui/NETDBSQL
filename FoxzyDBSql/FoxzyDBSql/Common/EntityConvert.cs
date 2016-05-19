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

        public List<TEntity> ToList<TEntity>(Func<DataRow, TEntity> render)
        {
            if (render == null)
                throw new NullReferenceException(nameof(render));

            List<TEntity> data = new List<TEntity>();

            foreach (DataRow row in parsTable.Rows)
            {
                data.Add(render.Invoke(row));
            }

            return data;
        }

        public List<TEntity> ToEntity<TEntity>() where TEntity : new()
        {
            List<TEntity> resultList = new List<TEntity>();
            Type entityType = typeof(TEntity);

            foreach (DataRow row in parsTable.Rows)
            {
                TEntity entity = default(TEntity);

                foreach (var enProperty in entityType.GetProperties())
                {
                    try
                    {
                        var rowVal = row[enProperty.Name];
                        enProperty.SetValue(entity, rowVal, null);
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
