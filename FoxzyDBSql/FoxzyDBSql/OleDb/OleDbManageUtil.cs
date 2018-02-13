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



        public override int ExecuteNonQuery(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }


        public override IDataReader ExecuteDataReader(string command, object pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }


        public override object ExecuteScalar(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        public override DataSet FillDataSet(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            throw new NotImplementedException();
        }

        protected override void InitCommand(string command, IDictionary<string, object> pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }

        protected override void InitCommand(string command, object pars = null, CommandType type = CommandType.Text)
        {
            throw new NotImplementedException();
        }
    }
}
