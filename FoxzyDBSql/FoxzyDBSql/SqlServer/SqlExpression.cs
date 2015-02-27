using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace FoxzyDBSql.SqlServer
{
    public class SqlExpression : AbsDbExpression
    {
        private DbManage db;

        public SqlExpression(DbManage db, Common.SqlExceType type)
        {
            if (db == null)
                throw new Exception("DbManage is not null");

            this.db = db;
            this._keyObject.SqlType = type;
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

        public override AbsDbExpression Select(IEnumerable<Common.DBSelectComponent> Component)
        {
            this._keyObject.Selects = Component;
            return this;
        }

        public override AbsDbExpression Select(string selectStr = null)
        {
            this._keyObject.SelectStr = selectStr;
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

        public override AbsDbExpression OrderBy(String faild)
        {
            this._keyObject.Sort.Add(faild.ToLower(), true);
            return this;
        }

        public override AbsDbExpression OrderBy(string field, string tableName)
        {
            String col = String.Format("{0}.{1}", tableName, field);

            this._keyObject.Sort.Add(col.ToLower(), true);
            return this;
        }

        public override AbsDbExpression OrderByDesc(string field)
        {
            this._keyObject.Sort.Add(field.ToLower(), false);
            return this;
        }

        public override AbsDbExpression OrderByDesc(string field, string tableName)
        {
            String col = String.Format("{0}.{1}", tableName, field);
            this._keyObject.Sort.Add(col.ToLower(), false);
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
            StringBuilder sb_sql = new StringBuilder();

            if (_keyObject.SqlType == Common.SqlExceType.Select)
            {
                if (String.IsNullOrEmpty(_keyObject.FromTable))
                    throw new Exception("没有制定表");

                initSelect(sb_sql);
                initInto(sb_sql);
                initFrom(sb_sql);
                initJoin(sb_sql);
                initWhere(sb_sql);
                initGroup(sb_sql);
                initHaving(sb_sql);
                initSort(sb_sql);

                return sb_sql.ToString();
            }
            if (_keyObject.SqlType == Common.SqlExceType.Update)
            {
                initUpdate(sb_sql);
                initset(sb_sql);
                initWhere(sb_sql);


                return sb_sql.ToString();
            }
            if (_keyObject.SqlType == Common.SqlExceType.Delete)
            {
                initDelete(sb_sql);
                initWhere(sb_sql);

                return sb_sql.ToString();
            }

            if (_keyObject.SqlType == Common.SqlExceType.Insert)
            {
                initInsert(sb_sql);
                initInsertColunmVal(sb_sql);
                return sb_sql.ToString();
            }
            throw new NotImplementedException();
        }

        public override System.Data.DataSet ToDataSet(bool isDispose = false)
        {
            try
            {
                return db.FillDataSet(this.ToSql(), this._keyObject.DataParameters,
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
                return db.ExecuteScalar(this.ToSql(), this._keyObject.DataParameters,
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
            joinTable = joinTable.Trim();
            DBOnExpression ex = new SqlDBOnExpression();

            ex.JoinType = SqlJoinType.LeftJoin;

            if (joinTable.IndexOf(" ") > 1)
            {
                ex.TableName = joinTable.Substring(0, joinTable.IndexOf(" "));
                ex.AsName = joinTable.Substring(joinTable.LastIndexOf(" "));

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

        public override DBOnExpression RightJoin(String joinTable)
        {
            joinTable = joinTable.Trim();
            DBOnExpression ex = new SqlDBOnExpression();

            ex.JoinType = SqlJoinType.RightJoin;

            if (joinTable.IndexOf(" ") > 1)
            {
                ex.TableName = joinTable.Substring(0, joinTable.IndexOf(" "));
                ex.AsName = joinTable.Substring(joinTable.LastIndexOf(" "));

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

        public override DBOnExpression InnerJoin(String joinTable)
        {
            joinTable = joinTable.Trim();
            DBOnExpression ex = new SqlDBOnExpression();

            ex.JoinType = SqlJoinType.InnerJoin;

            if (joinTable.IndexOf(" ") > 1)
            {
                ex.TableName = joinTable.Substring(0, joinTable.IndexOf(" "));
                ex.AsName = joinTable.Substring(joinTable.LastIndexOf(" "));

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
                _keyObject.DataParameters.Add(new SqlParameter("&" + p.Name, val));
            }

            return this;
        }

        public override AbsDbExpression SetParameter(Dictionary<string, object> dict)
        {
            foreach (var d in dict)
            {
                _keyObject.DataParameters.Add(new SqlParameter("&" + d.Key, d.Value));
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
            {
                throw new Exception("表名不能为空");
            }

            table = table.Trim();

            this._keyObject.DeleteTable = table;
            return this;
        }

        public override AbsDbExpression Insert(string table)
        {
            if (String.IsNullOrEmpty(table))
            {
                throw new Exception("表名不能为空");
            }

            table = table.Trim();

            this._keyObject.InsertTable = table;
            return this;
        }

        public override AbsDbExpression InsertColoums(params string[] coloums)
        {
            _keyObject.InsertColoums.Clear();

            foreach (string coloum in coloums)
            {
                _keyObject.InsertColoums.Add(coloum);
            }

            return this;
        }

        public override AbsDbExpression InsertColoums(IEnumerable<string> coloums)
        {
            _keyObject.InsertColoums.Clear();
            _keyObject.InsertColoums.AddRange(coloums);

            return this;
        }

        public override AbsDbExpression SetObject(object obj)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var p in properties)
            {
                object val = p.GetValue(obj, null);

                if (_keyObject.OperateObject.ContainsKey(p.Name))
                    throw new Exception(String.Format("已经指定列 {0} ", p.Name));

                _keyObject.OperateObject.Add(p.Name, val);
            }

            return this;
        }

        public override AbsDbExpression SetDictionary(Dictionary<string, object> dictionary)
        {
            foreach (var d in dictionary)
            {
                if (_keyObject.OperateObject.ContainsKey(d.Key))
                    throw new Exception(String.Format("已经指定列 {0} ", d.Key));

                _keyObject.OperateObject.Add(d.Key, d.Value);
            }

            return this;
        }

        public override DataSet Pagination(int PageIndex, int PageSize, out int RowsCount)
        {
            StringBuilder sb_sql = new StringBuilder();

            List<String> orderSql = new List<string>();
            //sb.Append(" order by ");
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

            initSelect(sb_sql);
            initFrom(sb_sql);
            initJoin(sb_sql);
            initWhere(sb_sql);
            initGroup(sb_sql);
            initHaving(sb_sql);

            String baseSql = sb_sql.ToString();

            var _Pagination = new SqlPaginationSelect(db);

            _Pagination.Set(baseSql, this._keyObject.DataParameters);

            return _Pagination.Pagination(PageIndex, PageSize, out RowsCount, String.Join(",", orderSql));
        }
    }
}
