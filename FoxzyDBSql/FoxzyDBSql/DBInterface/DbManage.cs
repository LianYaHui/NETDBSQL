using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public abstract class DbManage : IDisposable
    {
        protected DbConnection Connection;

        protected DbCommand Command;

        protected DbDataAdapter DataAdapter;

        protected DataSet DBDataSet;

        public static String DefaultConncetionString { set; get; }

        public static string ConncetionString { set; get; }

        public abstract bool OpenConncetion();

        public abstract int ExecuteNonQuery(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);

        public abstract IDataReader ExecuteDataReader(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);

        public abstract object ExecuteScalar(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);

        public abstract T ExecuteScalar<T>(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);

        public abstract DataSet FillDataSet(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);

        public abstract AbsDbExpression CreateSelect();

        public abstract AbsDbExpression CreateUpdate(String table);

        public abstract AbsDbExpression CreateDelete(String table);

        public abstract AbsDbExpression CreateInsert(String table);

        public abstract PaginationSelect CreatePagination();

        public virtual void Dispose()
        {

        }

        protected virtual void InitCommand(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text)
        {
            OpenConncetion();
        }
    }
}
