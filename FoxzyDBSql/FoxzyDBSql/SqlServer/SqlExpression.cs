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

        #region 私有方法
        private void initSelect(StringBuilder sb_sql)
        {
            List<String> select_sql = new List<string>();
            sb_sql.Append("select ");

            if (_keyObject.Top != 0)
                sb_sql.AppendFormat("top {0} ", _keyObject.Top);

            //Select
            if (!String.IsNullOrEmpty(_keyObject.SelectStr) || _keyObject.Selects != null)
            {
                if (_keyObject.Selects != null)
                {
                    foreach (var c in _keyObject.Selects)
                    {
                        String tableName = c.TableName;
                        foreach (var col in c.Colunms)
                        {
                            String _select = String.IsNullOrEmpty(col.AsName) ?
                                String.Format("$${0}", col.ColunmName) :
                                String.Format("$${0} as {1}", col.ColunmName, col.AsName);

                            if (String.IsNullOrEmpty(tableName)) _select = _select.Replace("$$", "");
                            else _select = _select.Replace("$$", tableName + ".");

                            select_sql.Add(_select);
                        }
                    }
                    sb_sql.AppendFormat(String.Join(",", select_sql.ToArray()));
                    return;
                }

                if (!String.IsNullOrEmpty(_keyObject.SelectStr))
                {
                    sb_sql.Append(_keyObject.SelectStr);
                }
            }
            else
            {
                //没有定义字段，则查询出表的所有字段　［ｔａｂｌｅ］．＊
                //多表
                //连接
                foreach (var tb in _keyObject.Tables)
                {
                    if (String.IsNullOrEmpty(tb.Value))
                        select_sql.Add(tb.Key + ".*");
                    else
                        select_sql.Add(tb.Value + ".*");
                }

                sb_sql.AppendLine(String.Join(",", select_sql.ToArray()));
            }
        }

        void initInto(StringBuilder sb_sql)
        {
            if (!String.IsNullOrEmpty(_keyObject.IntoTable))
            {
                sb_sql.AppendFormat(" into {0}", _keyObject.IntoTable);
            }
        }


        void initFrom(StringBuilder sb)
        {
            sb.AppendFormat(" from {0}", this._keyObject.FromTable);
        }

        void initWhere(StringBuilder sb)
        {
            if (!String.IsNullOrEmpty(this._keyObject.WhereSql))
            {
                sb.Append(" where ");
                sb.Append(this._keyObject.WhereSql);
            }
        }

        void initSort(StringBuilder sb)
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

            sb.Append(String.Join(",", orderSql.ToArray()));
        }

        void initJoin(StringBuilder sb)
        {
            if (this._keyObject.Join.Count == 0)
                return;

            foreach (String key in _keyObject.Join.Keys)
            {
                var onExp = _keyObject.Join[key] as IDBOnExpression;

                sb.Append(onExp.ToString());
            }

        }

        void initGroup(StringBuilder sb)
        {
            if (this._keyObject.GroupByField.Count == 0)
                return;

            sb.AppendFormat(" group by {0}", String.Join(",", this._keyObject.GroupByField.ToArray()));
        }

        void initHaving(StringBuilder sb)
        {
            if (!String.IsNullOrEmpty(this._keyObject.HavingSql))
            {
                sb.Append(" having ");
                sb.Append(this._keyObject.HavingSql);
            }
        }

        void initUpdate(StringBuilder sb)
        {
            sb.AppendFormat("update {0} ", this._keyObject.UpdateTable);
        }

        void initset(StringBuilder sb)
        {
            List<String> vals = this._keyObject.Set.Values.OfType<String>().ToList();

            foreach (System.Collections.DictionaryEntry d in this._keyObject.OperateObject)
            {
                vals.Add(d.Key + "= @" + Convert.ToString(d.Key));
                this._keyObject.DataParameters.Add(new SqlParameter("@" + d.Key, d.Value));
            }

            if (vals.Count == 0)
                throw new Exception("至少制定一个Set可供更新");

            sb.AppendFormat("set {0}", String.Join(",", vals.ToArray()));
        }

        private void initDelete(StringBuilder sb)
        {
            sb.AppendFormat("delete {0} ", this._keyObject.DeleteTable);
        }

        private void initInsert(StringBuilder sb_sql)
        {
            if (String.IsNullOrEmpty(this._keyObject.InsertTable))
                throw new Exception("insert 表为空");

            sb_sql.AppendFormat("insert {0} ", this._keyObject.InsertTable);
        }

        void initInsertColunmVal(StringBuilder sb_sql)
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
                    vals.Add("@" + Convert.ToString(d.Key));

                    this._keyObject.DataParameters.Add(new SqlParameter("@" + d.Key, d.Value));
                }

                sb_sql.AppendFormat("({0}) values ({1})",
                    String.Join(",", clo.ToArray()),
                    String.Join(",", vals.ToArray()));
            }
            else
                throw new Exception("这个你还是看下ToSql就知道了");
        }

        #endregion


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



        public override IDBOnExpression LeftJoin(String joinTable)
        {
            joinTable = joinTable.Trim();
            IDBOnExpression ex = new SqlDBOnExpression();

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

        public override IDBOnExpression RightJoin(String joinTable)
        {
            joinTable = joinTable.Trim();
            IDBOnExpression ex = new SqlDBOnExpression();

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

        public override IDBOnExpression InnerJoin(String joinTable)
        {
            joinTable = joinTable.Trim();
            IDBOnExpression ex = new SqlDBOnExpression();

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

        public override AbsDbExpression Limit(int skipNum, int returnNum = 0)
        {
            throw new Exception("SqlServer 不支持 limit 语句");
        }

        public override AbsDbExpression Top(int count)
        {
            if (count <= 0)
                throw new Exception("top参数不能为负数");

            _keyObject.Top = count;

            return this;
        }

        public override AbsDbExpression RowPagination(int beginRowNumber, int endRowNumber)
        {
            throw new NotImplementedException();
        }
    }
}
