using FoxzyDBSql.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

namespace FoxzyDBSql.DBInterface
{
    public abstract class AbsDbExpression
    {
        public DBSqlKeyObject _keyObject = DBSqlKeyObject.Create();

        public void ReSet()
        {
            _keyObject = new DBSqlKeyObject();
        }

        public AbsDbExpression()
        {
            this._keyObject = new DBSqlKeyObject();
        }

        public abstract AbsDbExpression Update(String tb);

        public abstract AbsDbExpression Delete(string table);

        public abstract AbsDbExpression Insert(string table);

        public abstract AbsDbExpression InsertColoums(params String[] coloums);

        public abstract AbsDbExpression InsertColoums(IEnumerable<String> coloums);

        public abstract AbsDbExpression SetObject(Object obj);

        public abstract AbsDbExpression SetDictionary(Dictionary<String, Object> dictionary);

        public abstract AbsDbExpression Set(String sql);

        public abstract AbsDbExpression From(String tableName, String AsTableName = null);

        public abstract AbsDbExpression From(String tablesql);

        public abstract AbsDbExpression Select(IEnumerable<DBSelectComponent> Component);

        public abstract AbsDbExpression Select(String selectStr = null);

        public abstract AbsDbExpression Into(String intoTable);

        public abstract AbsDbExpression Where(String where);

        public abstract AbsDbExpression Where(Func<String> Fun);

        public abstract AbsDbExpression OrderBy(String field);
        public abstract AbsDbExpression OrderBy(String field, String tableName);

        public abstract AbsDbExpression OrderByDesc(String field);
        public abstract AbsDbExpression OrderByDesc(String field, String tableName);

        public abstract AbsDbExpression GropuBy(params String[] field);

        public abstract AbsDbExpression SetParameter(params IDataParameter[] pars);

        public abstract AbsDbExpression SetParameter(IEnumerable<IDataParameter> pars);

        public abstract AbsDbExpression SetParameter(String replaceText, object value);

        public abstract AbsDbExpression SetParameter(object parsObj);

        public abstract AbsDbExpression SetParameter(Dictionary<String, Object> parsObj);

        public abstract AbsDbExpression Having(String havingsql);

        public abstract IDBOnExpression LeftJoin(String joinTable);

        public abstract IDBOnExpression RightJoin(String joinTable);

        public abstract IDBOnExpression InnerJoin(String joinTable);

        /// <summary>
        /// 分页
        /// </summary>
        public abstract DataSet Pagination(int PageIndex, int PageSize, out int RowsCount);

        public abstract String ToSql();

        public abstract DataSet ToDataSet();

        public abstract Object ExecuteScalar();

        public abstract int ExecuteNonQuery();

        public class DBSqlKeyObject
        {
            public SqlExceType SqlType { set; get; }

            public IEnumerable<DBSelectComponent> Selects { set; get; }
            public String SelectStr { set; get; }

            public String FromTable { set; get; }

            public Dictionary<String, String> Tables { set; get; }

            public String WhereSql { set; get; }

            public Hashtable Sort { set; get; }

            public Hashtable Join { set; get; }

            public List<System.Data.IDataParameter> DataParameters { set; get; }

            public Hashtable OperateObject { set; get; }

            public List<String> InsertColoums { set; get; }


            public HashSet<String> GroupByField { set; get; }

            public String UpdateTable { set; get; }

            public Hashtable Set { set; get; }

            public String Limit { set; get; }

            public DBSqlKeyObject()
            {
                Tables = new Dictionary<string, string>();
                Sort = new Hashtable();
                Join = new Hashtable();

                Set = new Hashtable();

                OperateObject = new Hashtable();
                InsertColoums = new List<string>();

                DataParameters = new List<System.Data.IDataParameter>();
                GroupByField = new HashSet<string>();
            }

            public static DBSqlKeyObject Create()
            {
                return new DBSqlKeyObject();
            }

            public string HavingSql { get; set; }

            public string DeleteTable { get; set; }

            public string IntoTable { get; set; }

            public string InsertTable { get; set; }
        }

    }
}
