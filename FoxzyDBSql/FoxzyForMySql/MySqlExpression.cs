using MySql.Data.MySqlClient;
﻿using FoxzyDBSql.Common;
using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyForMySql
{
    public class MySqlExpression : AbsDbExpression
    {
        private DbManage db;

        public MySqlExpression(DbManage db, SqlExceType type)
        {
            if (db == null)
                throw new Exception("DbManage is not null");

            this.db = db;
            this._keyObject.SqlType = type;
        }


        public override AbsDbExpression Update(string tb)
        {
            if (String.IsNullOrEmpty(tb))
            {
                throw new Exception("表名不能为空");
            }

            tb = tb.Trim();

            this._keyObject.UpdateTable = tb;

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

        public override AbsDbExpression Select(IEnumerable<FoxzyDBSql.Common.DBSelectComponent> Component)
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

        public override AbsDbExpression Where(string where)
        {
            this._keyObject.WhereSql = where;
            return this;
        }

        public override AbsDbExpression Where(Func<string> Fun)
        {
            if (Fun == null)
                throw new ArgumentNullException("Fun");

            this._keyObject.WhereSql = Fun();
            return this;
        }

        public override AbsDbExpression OrderBy(string field)
        {
            this._keyObject.Sort.Add(field.ToLower(), true);
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

        public override AbsDbExpression GropuBy(params string[] field)
        {
            foreach (String f in field)
                this._keyObject.GroupByField.Add(f);

            return this;
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
            this._keyObject.DataParameters.Add(new MySqlParameter(replaceText, value));
            return this;
        }


        public override AbsDbExpression SetParameter(object parsObj)
        {
            var properties = parsObj.GetType().GetProperties();

            foreach (var p in properties)
            {
                object val = p.GetValue(parsObj, null);
                _keyObject.DataParameters.Add(new MySqlParameter("?" + p.Name, val));
            }

            return this;
        }

        public override AbsDbExpression SetParameter(Dictionary<string, object> parsObj)
        {
            foreach (var d in parsObj)
            {
                _keyObject.DataParameters.Add(new MySqlParameter("?" + d.Key, d.Value));
            }

            return this;
        }

        public override AbsDbExpression Having(string havingsql)
        {
            this._keyObject.HavingSql = havingsql;
            return this;
        }

        public override IDBOnExpression LeftJoin(string joinTable)
        {
            joinTable = joinTable.Trim();
            IDBOnExpression ex = new MySqlDBOnExpression();

            ex.Type = "left join";

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

        public override IDBOnExpression RightJoin(string joinTable)
        {
            joinTable = joinTable.Trim();
            IDBOnExpression ex = new MySqlDBOnExpression();

            ex.Type = "right join";

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

        public override IDBOnExpression InnerJoin(string joinTable)
        {
            joinTable = joinTable.Trim();
            IDBOnExpression ex = new MySqlDBOnExpression();

            ex.Type = "inner join";

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

        public override string ToSql()
        {
            StringBuilder sb_sql = new StringBuilder();

            if (_keyObject.SqlType == SqlExceType.Select)
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
            if (_keyObject.SqlType == SqlExceType.Update)
            {
                initUpdate(sb_sql);
                initset(sb_sql);
                initWhere(sb_sql);


                return sb_sql.ToString();
            }
            if (_keyObject.SqlType == SqlExceType.Delete)
            {
                initDelete(sb_sql);
                initWhere(sb_sql);

                return sb_sql.ToString();
            }

            if (_keyObject.SqlType == SqlExceType.Insert)
            {
                initInsert(sb_sql);
                initInsertColunmVal(sb_sql);
                return sb_sql.ToString();
            }


            throw new NotImplementedException();
        }

        public override System.Data.DataSet ToDataSet()
        {
            try
            {
                return db.FillDataSet(this.ToSql(), this._keyObject.DataParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override object ExecuteScalar()
        {
            try
            {
                return db.ExecuteScalar(this.ToSql(), this._keyObject.DataParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override int ExecuteNonQuery()
        {
            try
            {
                return db.ExecuteNonQuery(this.ToSql(), this._keyObject.DataParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 私有方法

        new void initset(StringBuilder sb)
        {
            List<String> vals = this._keyObject.Set.Values.OfType<String>().ToList();

            foreach (System.Collections.DictionaryEntry d in this._keyObject.OperateObject)
            {
                vals.Add(d.Key + "= ?" + Convert.ToString(d.Key));
                this._keyObject.DataParameters.Add(new MySqlParameter("?" + d.Key, d.Value));
            }

            if (vals.Count == 0)
                throw new Exception("至少制定一个Set可供更新");

            sb.AppendFormat("set {0}", String.Join(",", vals.ToArray()));
        }

        new void initInsertColunmVal(StringBuilder sb_sql)
        {
            if (_keyObject.InsertColoums.Count > 0)
            {
                sb_sql.AppendFormat("({0}) ", String.Join(",", _keyObject.InsertColoums.ToArray()));

                initSelect(sb_sql);
                initFrom(sb_sql);
                initJoin(sb_sql);
                initWhere(sb_sql);
                initGroup(sb_sql);
                initHaving(sb_sql);
                initSort(sb_sql);
            }
            else if (_keyObject.OperateObject.Count > 0)
            {
                List<String> clo = new List<string>();
                List<String> vals = new List<string>();

                foreach (System.Collections.DictionaryEntry d in this._keyObject.OperateObject)
                {
                    clo.Add(Convert.ToString(d.Key));
                    vals.Add("?" + Convert.ToString(d.Key));

                    this._keyObject.DataParameters.Add(new MySqlParameter("?" + d.Key, d.Value));
                }

                sb_sql.AppendFormat("({0}) values ({1})",
                    String.Join(",", clo.ToArray()),
                    String.Join(",", vals.ToArray()));
            }
            else
                throw new Exception("这个你还是看下ToSql就知道了");
        }
        #endregion

        public override DataSet Pagination(int PageIndex, int PageSize, out int RowsCount)
        {
            if (PageIndex < 1)
                throw new Exception("页码PageIndex 必须从1开始");

            if (PageSize < 1)
                throw new Exception("每页显示的数量 PageSize 不能小于1");

            StringBuilder sb_sql = new StringBuilder();

            initSelect(sb_sql);
            initFrom(sb_sql);
            initJoin(sb_sql);
            initWhere(sb_sql);
            initGroup(sb_sql);
            initHaving(sb_sql);
            initSort(sb_sql);

            String baseSql = sb_sql.ToString();

            String getCountSql =
                String.Format("select count(*) from ({0}) as count_table", baseSql);

            RowsCount = Convert.ToInt32(db.ExecuteScalar(getCountSql, this._keyObject.DataParameters, CommandType.Text));

            String ReturnDataSql = String.Format("{0} limit {1},{2}", baseSql, (PageIndex - 1) * PageSize, PageSize);

            List<MySqlParameter> newPars = MySqlManageUtil.CloneParameter(this._keyObject.DataParameters);
            return db.FillDataSet(ReturnDataSql, newPars as IEnumerable<IDataParameter>, CommandType.Text);
        }
    }
}
