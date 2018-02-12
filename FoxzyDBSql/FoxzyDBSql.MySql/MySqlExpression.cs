using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FoxzyDBSql.Common;

namespace FoxzyDBSql.MySql
{
    public class MySqlExpression : AbsDbExpression
    {
        private const string __ParametersPlaceholder = "?";

        private MySqlParameterConvert __ParameterConvert = null;

        private MySqlDbCRUD sqlDbCRUD = null;

        internal DbManage db = null;

        private IDbParameterConvert ParameterConvert
        {
            get
            {
                return __ParameterConvert;
            }
        }

        /// <summary>
        /// 参数化的前导字符
        /// </summary>
        public override string ParametersPlaceholder
        {
            get
            {
                return __ParametersPlaceholder;
            }
        }

        public MySqlExpression(DbManage db, Common.SqlExceType type)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            this.db = db;
            this._keyObject.SqlType = type;
            __ParameterConvert = new MySqlParameterConvert();
        }

        static MySqlExpression()
        {
            MySqlParameterConvert.ParametersPlaceholder = __ParametersPlaceholder;
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
            tablesql = SqlStringUtils.ReplaceSpace(tablesql);

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

        public override AbsDbExpression WhereFromObject(object whereEntity)
        {
            if (whereEntity == null)
                throw new ArgumentNullException("whereEntity");

            var pars = ParameterConvert.FromObjectToParameters(whereEntity, null);

            var vals = pars.Select(p =>
            string.Format("{0} = {1}", SqlStringUtils.GetFieldName(p.ParameterName, ParametersPlaceholder), p.ParameterName));

            this._keyObject.WhereSql = string.Join(" and ", vals);
            SetParameter(pars);

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
            sqlDbCRUD = new MySqlDbCRUD(this);
            return sqlDbCRUD.BuildSql();
        }

        public override System.Data.DataSet ToDataSet(bool isDispose = false)
        {
            return db.FillDataSet(this.ToSql(),
                this._keyObject.DataParameters,
                CommandType.Text,
                isDispose);
        }

        public override DataTable QueryToTable(bool isDispose = false)
        {
            return ToDataSet(isDispose)
                    .Tables[0];
        }

        public override List<T> QueryToEntityList<T>(Dictionary<string, Func<DataRow, object>> farmat = null, bool isDispose = false)
        {
            return QueryToTable(isDispose).ToEntity<T>(farmat);
        }

        public override object ExecuteScalar(bool isDispose = false)
        {
            return db.ExecuteScalar(this.ToSql(),
                this._keyObject.DataParameters,
                CommandType.Text,
                isDispose);
        }

        public override int ExecuteNonQuery(bool isDispose = false)
        {
            return db.ExecuteNonQuery(this.ToSql(),
                this._keyObject.DataParameters,
                CommandType.Text,
                isDispose);
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
            joinTable = SqlStringUtils.ReplaceSpace(joinTable);
            DBOnExpression ex = new MySqlDBOnExpression();

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

        /// <summary>
        /// 用于设置参数
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
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
            if (value == null)
                throw new Exception("参数值不能为NULL");

            this._keyObject.DataParameters.Add(new SqlParameter($"{ParametersPlaceholder}{replaceText}", value));
            return this;
        }

        public override AbsDbExpression SetParameter(object parsObj)
        {
            var pars = ParameterConvert.FromObjectToParameters(parsObj, null);

            _keyObject.DataParameters.AddRange(pars);
            return this;
        }

        public override AbsDbExpression SetParameter(Dictionary<string, object> dict)
        {
            foreach (var d in dict)
            {
                _keyObject.DataParameters.Add(new SqlParameter(ParametersPlaceholder + d.Key, d.Value));
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
            var pars = ParameterConvert.FromObjectToParameters(obj, ignoreFields);

            _keyObject.OperateObjectParameters.AddRange(pars);
            return this;
        }

        public override AbsDbExpression SetDictionary(Dictionary<string, object> dictionary)
        {
            var pars = ParameterConvert.FromDictionaryToParameters(dictionary);

            _keyObject.OperateObjectParameters.AddRange(pars);
            return this;
        }

        public override DataTable Pagination(int PageIndex, int PageSize, out int RowsCount)
        {
            sqlDbCRUD = new MySqlDbCRUD(this);
            return sqlDbCRUD.Pagination(PageIndex, PageSize, out RowsCount);
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
