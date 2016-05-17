using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FoxzyDBSql.Common;

namespace FoxzyDBSql.SqlServer
{
    public class SqlExpression : AbsDbExpression, ISqlSkipTake
    {
        private DbManage db;

        private string _select()
        {
            StringBuilder sb_sql = new StringBuilder();

            if (String.IsNullOrEmpty(_keyObject.FromTable))
                throw new Exception("没有指定要查询的表");

            initSelect(sb_sql);
            initInto(sb_sql);
            initFrom(sb_sql);
            initJoin(sb_sql);
            initWhere(sb_sql);
            initGroup(sb_sql);
            initHaving(sb_sql);
            initSort(sb_sql);
            initSkip(sb_sql);
            initTake(sb_sql);

            return sb_sql.ToString();
        }

        private void initTake(StringBuilder sb_sql)
        {
            if (_keyObject.TakeRows < 1)
                return;

            sb_sql.AppendFormat(" FETCH  next {0} rows only", _keyObject.TakeRows);
        }

        private void initSkip(StringBuilder sb_sql)
        {
            if (_keyObject.SkipRows < 1)
                return;

            sb_sql.AppendFormat(" OFFSet {0} rows", _keyObject.SkipRows);
        }

        private string _update()
        {
            StringBuilder sb_sql = new StringBuilder();

            initUpdate(sb_sql);
            initset(sb_sql);
            initWhere(sb_sql);
            return sb_sql.ToString();
        }
        private string _delete()
        {
            StringBuilder sb_sql = new StringBuilder();
            initDelete(sb_sql);
            initWhere(sb_sql);
            return sb_sql.ToString();
        }
        private string _insert()
        {
            StringBuilder sb_sql = new StringBuilder();
            initInsert(sb_sql);
            initInsertColunmVal(sb_sql);
            return sb_sql.ToString();
        }

        protected Dictionary<Common.SqlExceType, Func<string>> _ToSqlDict = null;


        public SqlExpression(DbManage db, Common.SqlExceType type)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            this.db = db;
            this._keyObject.SqlType = type;

            _ToSqlDict = new Dictionary<SqlExceType, Func<string>>()
            {
                {
                    SqlExceType.Select,
                    new Func<string>(_select)
                },
                {
                    SqlExceType.Delete,
                    new Func<string>(_delete)
                },
                {
                    SqlExceType.Update,
                    new Func<string>(_update)
                },
                {
                    SqlExceType.Insert,
                    new Func<string>(_insert)
                }
            };

        }

        public override AbsDbExpression From(string tableName, string AsTableName = null)
        {
            if (AsTableName == null)
            {
                this._keyObject.FromTable = tableName;
            }
            else
            {
                this._keyObject.FromTable = String.Format("{0} as {1}", tableName, AsTableName);
            }

            this._keyObject.Tables.Add(tableName, AsTableName);
            return this;
        }

        public override AbsDbExpression From(string tablesql)
        {
            tablesql = tablesql.ReplaceSpace();

            foreach (String table in tablesql.Split(','))
            {
                if (table.IndexOf(" ") >= 0)
                {
                    String _tbName = table.Substring(0, table.IndexOf(" "));
                    String _asName = table.Substring(table.LastIndexOf(" "));

                    this._keyObject.Tables.Add(_tbName, _asName);
                }
                else
                    this._keyObject.Tables.Add(table, null);
            }

            this._keyObject.FromTable = tablesql;
            return this;
        }

        public override AbsDbExpression Select(string selectStr = null)
        {
            this._keyObject.SelectStr = selectStr;
            return this;
        }
        public override AbsDbExpression Select(params string[] selectStr)
        {
            this._keyObject.SelectStr = String.Join(",", selectStr);
            return this;
        }

        public override AbsDbExpression Into(string intoTable)
        {
            this._keyObject.IntoTable = intoTable;
            return this;
        }

        public override AbsDbExpression Where(String sql)
        {
            this._keyObject.WhereSql = sql;
            return this;
        }

        public override AbsDbExpression Where(Func<String> Fun)
        {
            if (Fun == null)
                throw new ArgumentNullException("Fun");

            this._keyObject.WhereSql = Fun();
            return this;
        }

        public override AbsDbExpression Order(string ordersql)
        {
            string descOrderStr = " desc";
            string OrderStr = " asc";

            if (String.IsNullOrEmpty(ordersql))
                throw new NullReferenceException("ordersql");

            var orders = from orderstr in ordersql.Split(',')
                         select orderstr.Trim().ToLower();


            foreach (string strOrder in orders)
            {
                if (strOrder.LastIndexOf(descOrderStr) == 0)
                    OrderByDesc(strOrder.Replace(descOrderStr, ""));
                else
                    OrderBy(strOrder.Replace(OrderStr, ""));
            }



            return this;
        }

        public override AbsDbExpression OrderBy(string field, string tableName = null)
        {
            if (String.IsNullOrEmpty(field))
                throw new NullReferenceException("field");

            if (String.IsNullOrEmpty(tableName))
                this._keyObject.Sort.Add(field.ToLower(), true);
            else
            {
                String col = String.Format("{0}.{1}", tableName, field);
                this._keyObject.Sort.Add(col.ToLower(), true);
            }

            return this;
        }

        public override AbsDbExpression OrderByDesc(string field, string tableName = null)
        {
            if (String.IsNullOrEmpty(field))
                throw new NullReferenceException("field");

            if (String.IsNullOrEmpty(tableName))
                this._keyObject.Sort.Add(field.ToLower(), false);
            else
            {
                String col = String.Format("{0}.{1}", tableName, field);
                this._keyObject.Sort.Add(col.ToLower(), false);
            }

            return this;
        }

        public override AbsDbExpression GropuBy(params String[] field)
        {
            foreach (String f in field)
                this._keyObject.GroupByField.Add(f);

            return this;
        }

        public override AbsDbExpression Having(String havingStr)
        {
            this._keyObject.HavingSql = havingStr;
            return this;
        }

        public override string ToSql()
        {
            Func<string> func = _ToSqlDict[_keyObject.SqlType];
            return func.Invoke();
        }

        public override System.Data.DataSet ToDataSet(bool isDispose = false)
        {
            try
            {
                return db.FillDataSet(this.ToSql(),
                    this._keyObject.DataParameters,
                    CommandType.Text,
                    isDispose);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override object ExecuteScalar(bool isDispose = false)
        {
            try
            {
                return db.ExecuteScalar(this.ToSql(),
                    this._keyObject.DataParameters,
                    CommandType.Text,
                    isDispose);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override int ExecuteNonQuery(bool isDispose = false)
        {
            try
            {
                return db.ExecuteNonQuery(this.ToSql(),
                    this._keyObject.DataParameters,
                    CommandType.Text,
                    isDispose);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public override DBOnExpression LeftJoin(String joinTable)
        {
            return InitJoin(joinTable, SqlJoinType.LeftJoin);
        }

        public override DBOnExpression RightJoin(String joinTable)
        {
            return InitJoin(joinTable, SqlJoinType.RightJoin);
        }

        public override DBOnExpression InnerJoin(String joinTable)
        {
            return InitJoin(joinTable, SqlJoinType.InnerJoin);
        }

        public override DBOnExpression CrossJoin(String joinTable)
        {
            return InitJoin(joinTable, SqlJoinType.CrossJoin);
        }

        public override DBOnExpression FullJoin(String joinTable)
        {
            return InitJoin(joinTable, SqlJoinType.FullJoin);
        }

        private DBOnExpression InitJoin(String joinTable, SqlJoinType type)
        {
            joinTable = joinTable.Trim().ReplaceSpace();
            DBOnExpression ex = new SqlDBOnExpression();

            ex.JoinType = type;

            if (joinTable.IndexOf(" ") > 1)
            {
                ex.TableName = joinTable.Substring(0, joinTable.IndexOf(" "));
                ex.AsName = joinTable.Substring(joinTable.LastIndexOf(" ") + 1);

                this._keyObject.Tables.Add(ex.TableName, ex.AsName);
            }
            else
            {
                ex.TableName = joinTable;
                ex.AsName = joinTable;

                this._keyObject.Tables.Add(ex.TableName, null);
            }

            return ex.Fill(this);
        }

        public override AbsDbExpression SetParameter(params IDataParameter[] pars)
        {
            this._keyObject.DataParameters.AddRange(pars);
            return this;
        }

        public override AbsDbExpression SetParameter(IEnumerable<IDataParameter> pars)
        {
            this._keyObject.DataParameters.AddRange(pars);
            return this;
        }

        public override AbsDbExpression SetParameter(string replaceText, object value)
        {
            this._keyObject.DataParameters.Add(new SqlParameter(replaceText, value));
            return this;
        }

        public override AbsDbExpression SetParameter(object parsObj)
        {
            var properties = parsObj.GetType().GetProperties();

            foreach (var p in properties)
            {
                object val = p.GetValue(parsObj, null);
                if (val == null) continue;
                _keyObject.DataParameters.Add(new SqlParameter("@" + p.Name, val));
            }

            return this;
        }

        public override AbsDbExpression SetParameter(Dictionary<string, object> dict)
        {
            foreach (var d in dict)
            {
                _keyObject.DataParameters.Add(new SqlParameter("@" + d.Key, d.Value));
            }

            return this;
        }


        public override AbsDbExpression Update(String tb)
        {
            if (String.IsNullOrEmpty(tb))
            {
                throw new Exception("表名不能为空");
            }

            tb = tb.Trim();

            this._keyObject.UpdateTable = tb;

            return this;
        }

        public override AbsDbExpression Set(string sql)
        {
            if (String.IsNullOrEmpty(sql))
            {
                throw new Exception("set 参数不能为空");
            }

            foreach (String set in sql.Split(','))
            {
                String key = set.Substring(0, set.IndexOf("="));

                if (this._keyObject.Set.ContainsKey(key))
                    throw new Exception(String.Format("在 SET 子句中多次指定了列名 '{0}'", key));

                this._keyObject.Set.Add(key, set);
            }

            return this;
        }

        public override AbsDbExpression Delete(string table)
        {
            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

            table = table.Trim();

            this._keyObject.DeleteTable = table;
            return this;
        }

        public override AbsDbExpression Insert(string table)
        {
            if (String.IsNullOrEmpty(table))
                throw new ArgumentNullException("table");

            table = table.Trim();

            this._keyObject.InsertTable = table;
            return this;
        }

        public override AbsDbExpression InsertColoums(params string[] coloums)
        {
            _keyObject.InsertColoums.Clear();
            _keyObject.InsertColoums.AddRange(coloums);

            return this;
        }

        public override AbsDbExpression InsertColoums(IEnumerable<string> coloums)
        {
            _keyObject.InsertColoums.Clear();
            _keyObject.InsertColoums.AddRange(coloums);

            return this;
        }

        public override AbsDbExpression SetObject(object obj, params string[] ignoreFields)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var p in properties)
            {
                object val = p.GetValue(obj, null);

                if (val == null)
                    continue;
                if (ignoreFields.Contains(p.Name))
                    continue;

                if (_keyObject.OperateObject.ContainsKey(p.Name))
                    throw new Exception(String.Format("已经指定列 {0} ", p.Name));

                _keyObject.OperateObject.Add(p.Name, val);
            }

            return this;
        }

        public override AbsDbExpression SetDictionary(Dictionary<string, object> dictionary)
        {
            if (dictionary == null)
                throw new NullReferenceException(nameof(dictionary));

            foreach (var dict in dictionary)
            {
                if (dict.Value == null)
                    continue;

                if (_keyObject.OperateObject.ContainsKey(dict.Key))
                    throw new Exception(String.Format("已经指定列 {0} ", dict.Key));

                _keyObject.OperateObject.Add(dict.Key, dict.Value);
            }

            return this;
        }

        public override DataSet Pagination(int PageIndex, int PageSize, out int RowsCount)
        {
            StringBuilder sb_sql = new StringBuilder();
            List<String> orderSql = new List<string>();

            foreach (String key in _keyObject.Sort.Keys)
            {
                if ((bool)(_keyObject.Sort[key]))
                    orderSql.Add(String.Format("{0} asc", key));
                else
                    orderSql.Add(String.Format("{0} desc", key));
            }

            initSelect(sb_sql);
            initFrom(sb_sql);
            initJoin(sb_sql);
            initWhere(sb_sql);
            initGroup(sb_sql);
            initHaving(sb_sql);

            String baseSql = sb_sql.ToString();

            var _Pagination = new SqlPaginationSelect(db);

            _Pagination.Set(baseSql, this._keyObject.DataParameters);

            return _Pagination.Pagination(PageIndex,
                                            PageSize,
                                            out RowsCount,
                                            String.Join(",", orderSql)
                                            );
        }

        public AbsDbExpression Skip(int skipRowCount)
        {
            _keyObject.SkipRows = skipRowCount;
            return this;
        }

        public AbsDbExpression Take(int takeRowCount)
        {
            _keyObject.TakeRows = takeRowCount;
            return this;
        }
    }
}
