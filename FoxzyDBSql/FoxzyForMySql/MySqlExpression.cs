using FoxzyDBSql.DBInterface;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyForMySql
{
    public class MySqlExpression : AbsDbExpression
    {
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
            throw new NotImplementedException();
        }

        public override AbsDbExpression From(string tablesql)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Select(IEnumerable<FoxzyDBSql.Common.DBSelectComponent> Component)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Select(string selectStr = null)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Into(string intoTable)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Where(string where)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression OrderBy(string field)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression OrderBy(string field, string tableName)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression OrderByDesc(string field)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression OrderByDesc(string field, string tableName)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression GropuBy(params string[] field)
        {
            throw new NotImplementedException();
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

        public override AbsDbExpression Having(string havingsql)
        {
            throw new NotImplementedException();
        }

        public override IDBOnExpression LeftJoin(string joinTable)
        {
            throw new NotImplementedException();
        }

        public override IDBOnExpression RightJoin(string joinTable)
        {
            throw new NotImplementedException();
        }

        public override IDBOnExpression InnerJoin(string joinTable)
        {
            throw new NotImplementedException();
        }

        public override string ToSql()
        {
            throw new NotImplementedException();
        }

        public override System.Data.DataSet ToDataSet()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
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

        public override AbsDbExpression Limit(int skipNum, int returnNum)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Top(int count)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression RowPagination(int beginRowNumber, int endRowNumber)
        {
            throw new NotImplementedException();
        }
    }
}
