using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyForMySql
{
    public class MySqlExpression : AbsDbExpression
    {
        public override AbsDbExpression Update(string tb)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Delete(string table)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Insert(string table)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression InsertColoums(params string[] coloums)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression InsertColoums(IEnumerable<string> coloums)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression SetObject(object obj)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression SetDictionary(Dictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression Set(string sql)
        {
            throw new NotImplementedException();
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

        public override AbsDbExpression SetParameter(params System.Data.SqlClient.SqlParameter[] pars)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression SetParameter(IEnumerable<System.Data.SqlClient.SqlParameter> pars)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression SetParameter(string replaceText, object value)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
