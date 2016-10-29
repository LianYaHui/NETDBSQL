﻿using FoxzyDBSql.Common;
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
        public DBSqlKeyObject _keyObject = DBSqlKeyObject.Create();

        public void ReSet()
        {
            _keyObject = DBSqlKeyObject.Create();
        }

        public AbsDbExpression()
        {
            this._keyObject = DBSqlKeyObject.Create();
        }

        public abstract string ParametersPlaceholder
        {
            get;
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

        public class DBSqlKeyObject
        {
            public SqlExceType SqlType { set; get; }

            public String SelectStr { set; get; }

            public String FromTable { set; get; }

            public Dictionary<String, String> Tables { set; get; }

            public String WhereSql { set; get; }

            public Hashtable Sort { set; get; }

            public Hashtable Join { set; get; }

            public List<System.Data.IDataParameter> DataParameters { set; get; }

            public List<System.Data.IDataParameter> OperateObjectParameters { set; get; }

            public List<String> InsertColoums { set; get; }

            public HashSet<String> GroupByField { set; get; }

            public String UpdateTable { set; get; }

            public Hashtable Set { set; get; }

            public String Limit { set; get; }

            public int SkipRows { set; get; }

            public int TakeRows { set; get; }

            public DBSqlKeyObject()
            {
                Tables = new Dictionary<string, string>();
                Sort = new Hashtable();
                Join = new Hashtable();

                Set = new Hashtable();

                OperateObjectParameters = new List<IDataParameter>();
                InsertColoums = new List<string>();

                DataParameters = new List<IDataParameter>();
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

        #region 私有方法
        protected void initSelect(StringBuilder sb_sql)
        {
            List<String> select_sql = new List<string>();
            sb_sql.Append("select ");
            //Select
            if (!String.IsNullOrEmpty(_keyObject.SelectStr))
            {
                sb_sql.Append(_keyObject.SelectStr);
            }
            else
            {
                //没有定义字段，则查询出表的所有字段　[table]．＊
                //多表
                //连接
                foreach (var tb in _keyObject.Tables)
                {
                    if (String.IsNullOrEmpty(tb.Value))
                        select_sql.Add(tb.Key + ".*");
                    else
                        select_sql.Add(tb.Value + ".*");
                }

                sb_sql.Append(String.Join(",", select_sql));
            }
        }

        protected void initInto(StringBuilder sb_sql)
        {
            if (!String.IsNullOrEmpty(_keyObject.IntoTable))
            {
                sb_sql.AppendFormat(" into {0}", _keyObject.IntoTable);
            }
        }


        protected void initFrom(StringBuilder sb)
        {
            sb.AppendFormat(" from {0}", this._keyObject.FromTable);
        }

        protected void initWhere(StringBuilder sb)
        {
            if (!String.IsNullOrEmpty(this._keyObject.WhereSql))
            {
                sb.Append(" where ");
                sb.Append(this._keyObject.WhereSql);
            }
        }

        protected void initSort(StringBuilder sb)
        {
            if (this._keyObject.Sort.Count == 0)
                return;

            List<String> orderSql = new List<string>();
            sb.Append(" order by ");

            foreach (String key in _keyObject.Sort.Keys)
            {
                if ((bool)(_keyObject.Sort[key]))
                {
                    orderSql.Add(String.Format("{0} asc", key));
                }
                else
                {
                    orderSql.Add(String.Format("{0} desc", key));
                }
            }

            sb.Append(String.Join(",", orderSql));
        }

        protected void initJoin(StringBuilder sb)
        {
            if (this._keyObject.Join.Count == 0)
                return;

            foreach (String key in _keyObject.Join.Keys)
            {
                var onExp = _keyObject.Join[key] as DBOnExpression;

                sb.Append(onExp.ToString());
            }

        }

        protected void initGroup(StringBuilder sb)
        {
            if (this._keyObject.GroupByField.Count == 0)
                return;

            sb.AppendFormat(" group by {0}", String.Join(",", this._keyObject.GroupByField));
        }

        protected void initHaving(StringBuilder sb)
        {
            if (!String.IsNullOrEmpty(this._keyObject.HavingSql))
            {
                sb.Append(" having ");
                sb.Append(this._keyObject.HavingSql);
            }
        }

        protected void initUpdate(StringBuilder sb)
        {
            sb.AppendFormat("update {0} ", this._keyObject.UpdateTable);
        }

        protected void initSet(StringBuilder sb)
        {
            if (_keyObject.OperateObjectParameters.Count == 0)
                throw new Exception("至少制定一个Set可供更新");

            this._keyObject.DataParameters.AddRange(_keyObject.OperateObjectParameters);
            var vals = _keyObject.OperateObjectParameters.Select(p => string.Format("{0} = {1}", p.ParameterName.Replace("@", ""), p.ParameterName));

            sb.AppendFormat("set {0}", String.Join(",", vals));
        }

        protected void initDelete(StringBuilder sb)
        {
            sb.AppendFormat("delete {0} ", this._keyObject.DeleteTable);
        }

        protected void initInsert(StringBuilder sb_sql)
        {
            if (String.IsNullOrEmpty(this._keyObject.InsertTable))
                throw new Exception("insert 表为空");

            sb_sql.AppendFormat("insert {0} ", this._keyObject.InsertTable);
        }

        protected void initInsertColunmVal(StringBuilder sb_sql)
        {
            if (_keyObject.InsertColoums.Count > 0)
            {
                sb_sql.AppendFormat("({0}) ", String.Join(",", _keyObject.InsertColoums));

                initSelect(sb_sql);
                initFrom(sb_sql);
                initJoin(sb_sql);
                initWhere(sb_sql);
                initGroup(sb_sql);
                initHaving(sb_sql);
                initSort(sb_sql);
            }
            else if (_keyObject.OperateObjectParameters.Count > 0)
            {
                List<String> clo = new List<string>();
                List<String> vals = new List<string>();

                foreach (var pars in this._keyObject.OperateObjectParameters)
                {
                    clo.Add(pars.ParameterName.TrimStart(ParametersPlaceholder.ToCharArray()));
                    vals.Add(pars.ParameterName);
                }
                _keyObject.DataParameters.AddRange(_keyObject.OperateObjectParameters);
                sb_sql.AppendFormat("({0}) values ({1})",
                    String.Join(",", clo),
                    String.Join(",", vals));
            }
            else
                throw new Exception("这个你还是看下ToSql就知道了");
        }

        #endregion
    }
}
