using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.SqlServer
{
    public class SqlExpression : AbsDbExpression
    {
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

        public override AbsDbExpression Where(String sql)
        {
            this._keyObject.WhereSql = sql;
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

        public override AbsDbExpression 
            
            GropuBy()
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Having()
        {
            throw new NotImplementedException();
        }

        public override string ToSql()
        {
            if (String.IsNullOrEmpty(_keyObject.FromTable))
                throw new Exception("没有制定表");

            StringBuilder sb_sql = new StringBuilder();

            _initSelect(sb_sql);

            initFrom(sb_sql);

            initWhere(sb_sql);

            initSort(sb_sql);

            return sb_sql.ToString();
        }


        #region 私有方法
        private void _initSelect(StringBuilder sb_sql)
        {
            List<String> select_sql = new List<string>();
            sb_sql.AppendLine("select ");
            //Select
            if (!String.IsNullOrEmpty(_keyObject.SelectStr) && _keyObject.Selects != null)
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
                    sb_sql.AppendFormat(String.Join(",", select_sql));
                    return;
                }

                if (!String.IsNullOrEmpty(_keyObject.SelectStr))
                {
                    sb_sql.AppendLine(_keyObject.SelectStr);
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

                sb_sql.AppendLine(String.Join(",", select_sql));
            }
        }

        void initFrom(StringBuilder sb)
        {
            sb.Append(" from ");

            List<String> fromSql = new List<string>();

            foreach (var tb in this._keyObject.Tables)
            {
                if (String.IsNullOrEmpty(tb.Value))
                {
                    fromSql.Add(tb.Key);
                }
                else fromSql.Add(String.Format("{0} as {1}", tb.Key, tb.Value));
            }

            sb.Append(String.Join(",", fromSql));

        }

        void initWhere(StringBuilder sb)
        {
            sb.Append(" where 1=1");

            if (!String.IsNullOrEmpty(this._keyObject.WhereSql))
            {
                sb.Append(" and ");
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

            sb.Append(String.Join(",", orderSql));
        }

        #endregion


        public override System.Data.DataSet ToDataSet()
        {
            throw new NotImplementedException();
        }




    }
}
