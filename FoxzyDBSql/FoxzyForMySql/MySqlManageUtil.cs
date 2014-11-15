using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using FoxzyDBSql.Common;

namespace FoxzyForMySql
{
    public class MySqlManageUtil : DbManage
    {
        public override bool OpenConncetion()
        {
            bool _opneResult = false;

            if (ConncetionString == null)
            {
                ConnectionStringIsNull();

                //当连接字符串为空的时候进行默认操作
                return _opneResult;
            }
            try
            {
                Connection = new MySqlConnection(ConncetionString);
                Connection.Open();
                _opneResult = true;
            }
            catch (Exception ex) { throw ex; }
            return _opneResult;
        }


        protected override void InitCommand(string command, IEnumerable<IDataParameter> pars, CommandType type)
        {
            OpenConncetion();

            Command = new MySqlCommand(command, (Connection as MySqlConnection));
            Command.CommandType = type;

            if (pars == null) return;
            Command.Parameters.AddRange(pars.ToArray());
        }

        public override int ExecuteNonQuery(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text)
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

        public override object ExecuteScalar(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text)
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

        public override T ExecuteScalar<T>(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text)
        {
            try
            {
                var obj = ExecuteScalar(command, pars, type);
                return (T)(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override System.Data.DataSet FillDataSet(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text)
        {
            DataAdapter = new MySqlDataAdapter();
            DBDataSet = new DataSet();

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

        public override void Dispose()
        {
            if (Connection != null) Connection.Dispose();
            if (Command != null) Command.Dispose();
            if (DataAdapter != null) DataAdapter.Dispose();
            if (DBDataSet != null) DBDataSet.Dispose();
        }

        public override int BulkCopyInsert(string tabelName, System.Data.DataTable data)
        {
            throw new NotImplementedException();
        }

        public override AbsDbExpression CreateSelect()
        {
            var _sql = new MySqlExpression(this, SqlExceType.Select);
            return _sql;
        }

        public override AbsDbExpression CreateUpdate(string table)
        {
            return new MySqlExpression(this, SqlExceType.Update).Update(table);
        }

        public override AbsDbExpression CreateDelete(string table)
        {
            return new MySqlExpression(this, SqlExceType.Delete).Delete(table);
        }

        public override AbsDbExpression CreateInsert(string table)
        {
            return new MySqlExpression(this, SqlExceType.Insert).Insert(table);
        }
    }
}
