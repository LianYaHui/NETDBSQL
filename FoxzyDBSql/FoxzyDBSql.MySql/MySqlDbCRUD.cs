using FoxzyDBSql.Common;
using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FoxzyDBSql.MySql
{
    internal class MySqlDbCRUD : IDbCRUD
    {
        private DBSqlKeyObject _keyObject = null;
        private MySqlExpression _dbDbExpression = null;

        public MySqlDbCRUD(MySqlExpression expression)
        {
            _dbDbExpression = expression;
            this._keyObject = _dbDbExpression._keyObject;
        }

        public string BuildSql()
        {
            string strSql = "";

            switch (_keyObject.SqlType)
            {
                case SqlExceType.Delete:
                    strSql = BuildDelete();
                    break;
                case SqlExceType.Insert:
                    strSql = BuildInsert();
                    break;
                case SqlExceType.Update:
                    strSql = BuildUpdate();
                    break;
                default:
                    strSql = BuildSelect();
                    break;
            }

            return strSql;
        }


        public DataTable Pagination(int PageIndex, int PageSize, out int RowsCount)
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

            InitSelect(sb_sql);
            InitFrom(sb_sql);
            InitJoin(sb_sql);
            InitWhere(sb_sql);
            InitGroup(sb_sql);
            InitHaving(sb_sql);

            String baseSql = sb_sql.ToString();

            var _Pagination = new MySqlPaginationSelect(_dbDbExpression.db);

            _Pagination.Set(baseSql, this._keyObject.DataParameters);

            return _Pagination.Pagination(PageIndex,
                                            PageSize,
                                            out RowsCount,
                                            String.Join(",", orderSql)
                                            );
        }


        public string BuildDelete()
        {
            return _delete();
        }

        public string BuildInsert()
        {
            return _insert();
        }

        public string BuildSelect()
        {
            return _select();
        }

        public string BuildUpdate()
        {
            return _update();
        }



        //privete method
        private string _select()
        {
            StringBuilder sb_sql = new StringBuilder();

            if (String.IsNullOrEmpty(_keyObject.FromTable))
                throw new Exception("没有指定要查询的表");

            InitSelect(sb_sql);
            InitInto(sb_sql);
            InitFrom(sb_sql);
            InitJoin(sb_sql);
            InitWhere(sb_sql);
            InitGroup(sb_sql);
            InitHaving(sb_sql);
            InitSort(sb_sql);
            InitSkip(sb_sql);
            InitTake(sb_sql);

            return sb_sql.ToString();
        }
        private string _update()
        {
            StringBuilder sb_sql = new StringBuilder();

            InitUpdate(sb_sql);
            InitSet(sb_sql);
            InitWhere(sb_sql);
            return sb_sql.ToString();
        }
        private string _delete()
        {
            StringBuilder sb_sql = new StringBuilder();
            InitDelete(sb_sql);
            InitWhere(sb_sql);
            return sb_sql.ToString();
        }
        private string _insert()
        {
            StringBuilder sb_sql = new StringBuilder();
            InitInsert(sb_sql);
            InitInsertColunmVal(sb_sql);
            return sb_sql.ToString();
        }

        private void InitTake(StringBuilder sb_sql)
        {
            if (_keyObject.TakeRows < 1)
                return;

            sb_sql.AppendFormat(" FETCH  next {0} rows only", _keyObject.TakeRows);
        }

        private void InitSkip(StringBuilder sb_sql)
        {
            if (_keyObject.SkipRows < 1)
                return;

            sb_sql.AppendFormat(" OFFSet {0} rows", _keyObject.SkipRows);
        }

        #region 私有方法
        protected void InitSelect(StringBuilder sb_sql)
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

        protected void InitInto(StringBuilder sb_sql)
        {
            if (!String.IsNullOrEmpty(_keyObject.IntoTable))
            {
                sb_sql.AppendFormat(" into {0}", _keyObject.IntoTable);
            }
        }


        protected void InitFrom(StringBuilder sb)
        {
            sb.AppendFormat(" from {0}", this._keyObject.FromTable);
        }

        protected void InitWhere(StringBuilder sb)
        {
            if (!String.IsNullOrEmpty(this._keyObject.WhereSql))
            {
                sb.Append(" where ");
                sb.Append(this._keyObject.WhereSql);
            }
        }

        protected void InitSort(StringBuilder sb)
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

        protected void InitJoin(StringBuilder sb)
        {
            if (this._keyObject.Join.Count == 0)
                return;

            foreach (String key in _keyObject.Join.Keys)
            {
                var onExp = _keyObject.Join[key] as DBOnExpression;

                sb.Append(onExp.ToString());
            }

        }

        protected void InitGroup(StringBuilder sb)
        {
            if (this._keyObject.GroupByField.Count == 0)
                return;

            sb.AppendFormat(" group by {0}", String.Join(",", this._keyObject.GroupByField));
        }

        protected void InitHaving(StringBuilder sb)
        {
            if (!String.IsNullOrEmpty(this._keyObject.HavingSql))
            {
                sb.Append(" having ");
                sb.Append(this._keyObject.HavingSql);
            }
        }

        protected void InitUpdate(StringBuilder sb)
        {
            sb.AppendFormat("update {0} ", this._keyObject.UpdateTable);
        }

        protected void InitSet(StringBuilder sb)
        {
            if (_keyObject.OperateObjectParameters.Count == 0)
                throw new Exception("至少制定一个Set可供更新");

            _dbDbExpression.SetParameter(_keyObject.OperateObjectParameters);
            var vals = _keyObject.OperateObjectParameters.Select(p =>
            string.Format("{0} = {1}", SqlStringUtils.GetFieldName(p.ParameterName, _dbDbExpression.ParametersPlaceholder), p.ParameterName));

            sb.AppendFormat("set {0}", String.Join(",", vals));
        }

        protected void InitDelete(StringBuilder sb)
        {
            sb.AppendFormat("delete from {0} ", this._keyObject.DeleteTable);
        }

        protected void InitInsert(StringBuilder sb_sql)
        {
            if (String.IsNullOrEmpty(this._keyObject.InsertTable))
                throw new Exception("insert 表为空");

            sb_sql.AppendFormat("insert {0} ", this._keyObject.InsertTable);
        }

        protected void InitInsertColunmVal(StringBuilder sb_sql)
        {
            if (_keyObject.InsertColoums.Count > 0)
            {
                sb_sql.AppendFormat("({0}) ", String.Join(",", _keyObject.InsertColoums));

                InitSelect(sb_sql);
                InitFrom(sb_sql);
                InitJoin(sb_sql);
                InitWhere(sb_sql);
                InitGroup(sb_sql);
                InitHaving(sb_sql);
                InitSort(sb_sql);
            }
            else if (_keyObject.OperateObjectParameters.Count > 0)
            {
                List<String> clo = new List<string>();
                List<String> vals = new List<string>();

                foreach (var pars in this._keyObject.OperateObjectParameters)
                {
                    clo.Add(SqlStringUtils.GetFieldName(pars.ParameterName, _dbDbExpression.ParametersPlaceholder));
                    vals.Add(pars.ParameterName);
                }
                _dbDbExpression.SetParameter(_keyObject.OperateObjectParameters);
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
