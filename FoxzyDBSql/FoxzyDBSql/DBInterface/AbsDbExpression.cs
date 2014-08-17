using FoxzyDBSql.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public abstract class AbsDbExpression
    {
        protected DBSqlKeyObject _keyObject = DBSqlKeyObject.Create();

        public abstract AbsDbExpression From(String tableName, String AsTableName = null);

        public abstract AbsDbExpression From(String tablesql);

        public abstract AbsDbExpression Select(IEnumerable<DBSelectComponent> Component);

        public abstract AbsDbExpression Select(String selectStr = null);

        public abstract AbsDbExpression Where(String where);

        public abstract AbsDbExpression OrderBy(String field);

        public abstract AbsDbExpression OrderBy(String field, String tableName);

        public abstract AbsDbExpression OrderByDesc(String field);
        public abstract AbsDbExpression OrderByDesc(String field, String tableName);
        public abstract AbsDbExpression GropuBy();

        public abstract AbsDbExpression Having();

        public abstract String ToSql();

        public abstract DataSet ToDataSet();
    }
}
