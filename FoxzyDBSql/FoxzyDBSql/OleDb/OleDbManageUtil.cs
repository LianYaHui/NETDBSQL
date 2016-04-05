using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace FoxzyDBSql.OleDb
{
    public class OleDbManageUtil : DbManage
    {
        OleDbConnection Connection = null;
        OleDbCommand Command = null;
        OleDbDataAdapter DataAdapter = null;

        protected override IDbParameterConvert ParameterConvert
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void OpenConncetion()
        {
            Connection = new OleDbConnection(ConncetionString);
            Connection.Open();
        }

        public OleDbManageUtil(String ConnetionString)
            : base(ConnetionString)
        {

        }

        protected override void InitCommand(string command, IEnumerable<IDataParameter> pars, CommandType type)
        {
            OpenConncetion();

            Command = new OleDbCommand(command, (Connection as OleDbConnection));
            Command.CommandType = type;

            if (pars == null) return;
            Command.Parameters.AddRange(pars.ToArray());
        }

        public override int ExecuteNonQuery(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text,
            bool isDispose = true)
        {
            try
            {
                InitCommand(command, pars, type);
                int _result = Command.ExecuteNonQuery();
                Dispose();
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override System.Data.IDataReader ExecuteDataReader(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text)
        {
            InitCommand(command, pars, type);
            try
            {
                return Command.ExecuteReader();
            }
            catch (Exception ex) { throw ex; }
        }

        public override object ExecuteScalar(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text,
            bool isDispose = true)
        {
            try
            {
                InitCommand(command, pars, type);
                object _result = Command.ExecuteScalar();
                Dispose();
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override System.Data.DataSet FillDataSet(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text,
            bool isDispose = true)
        {
            DataAdapter = new OleDbDataAdapter();
            DataSet DBDataSet = new DataSet();

            InitCommand(command, pars, type);
            DataAdapter.SelectCommand = Command;

            try
            {
                DataAdapter.Fill(DBDataSet);
                Dispose();
                return DBDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override AbsDbExpression CreateSelect()
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression CreateUpdate(string table)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression CreateDelete(string table)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression CreateInsert(string table)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            if (Connection != null) Connection.Dispose();
            if (Command != null) Command.Dispose();
            if (DataAdapter != null) DataAdapter.Dispose();
        }

        public override PaginationSelect CreatePagination()
        {
            throw new NotImplementedException();
        }

        public override bool ExecTranstion(Action<DbManage> action, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        public override IDataReader ExecuteDataReader(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public override IDataReader ExecuteDataReader(string command, object pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        public override DataSet FillDataSet(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        public override DataSet FillDataSet(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        protected override void InitCommand(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        protected override void InitCommand(string command, object pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }
    }
}
