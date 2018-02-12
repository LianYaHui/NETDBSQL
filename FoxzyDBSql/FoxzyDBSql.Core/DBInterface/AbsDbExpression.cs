using FoxzyDBSql.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace FoxzyDBSql.DBInterface
{
    public abstract class AbsDbExpression
    {
        public DBSqlKeyObject _keyObject = null;

        public void ReSet()
        {
            _keyObject = DBSqlKeyObject.Create();
        }

        public AbsDbExpression()
        {
            ReSet();
        }

        public abstract AbsDbExpression Update(string tb);

        public abstract AbsDbExpression Delete(string table);

        public abstract AbsDbExpression Insert(string table);

        public abstract AbsDbExpression InsertColoums(params string[] coloums);

        public abstract AbsDbExpression InsertColoums(IEnumerable<String> coloums);

        public abstract AbsDbExpression SetObject(Object obj, params string[] ignoreFields);

        public abstract AbsDbExpression SetDictionary(Dictionary<String, Object> dictionary);

        public abstract AbsDbExpression From(String tableName, String AsTableName = null);

        public abstract AbsDbExpression From(String tablesql);

        public abstract AbsDbExpression Select(String selectStr = null);

        public abstract AbsDbExpression Select(params String[] selectStr);

        public abstract AbsDbExpression Into(String intoTable);

        public abstract AbsDbExpression Where(String where);

        public abstract AbsDbExpression Where(Func<String> Fun);

        public abstract AbsDbExpression WhereFromObject(object whereEntity);

        public abstract AbsDbExpression OrderBy(String field, String tableName = null);

        public abstract AbsDbExpression Order(string ordersql);

        public abstract AbsDbExpression OrderByDesc(String field, String tableName = null);

        public abstract AbsDbExpression GropuBy(params String[] field);

        public abstract AbsDbExpression SetParameter(params IDataParameter[] pars);

        public abstract AbsDbExpression SetParameter(IEnumerable<IDataParameter> pars);

        public abstract AbsDbExpression SetParameter(String replaceText, object value);

        public abstract AbsDbExpression SetParameter(object parsObj);

        public abstract AbsDbExpression SetParameter(Dictionary<String, Object> parsObj);

        public abstract AbsDbExpression Having(String havingsql);

        public abstract DBOnExpression LeftJoin(String joinTable);

        public abstract DBOnExpression RightJoin(String joinTable);

        public abstract DBOnExpression InnerJoin(String joinTable);

        public abstract DBOnExpression CrossJoin(String joinTable);

        public abstract DBOnExpression FullJoin(String joinTable);



        /// <summary>
        /// 分页
        /// </summary>
        public abstract DataTable Pagination(int PageIndex, int PageSize, out int RowsCount);

        public abstract String ToSql();

        public abstract DataSet ToDataSet(bool isDispose = false);

        public abstract DataTable QueryToTable(bool isDispose = false);

        public abstract List<T> QueryToEntityList<T>(Dictionary<string, Func<DataRow, object>> farmat = null, bool isDispose = false) where T : new();

        public abstract Object ExecuteScalar(bool isDispose = false);

        public abstract int ExecuteNonQuery(bool isDispose = false);
    }
}
