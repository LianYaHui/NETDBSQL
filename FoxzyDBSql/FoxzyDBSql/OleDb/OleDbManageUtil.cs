﻿using FoxzyDBSql.DBInterface;
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
        public override bool OpenConncetion()
        {
            bool _opneResult = false;
            String _conStr = ConncetionString;

            if (String.IsNullOrEmpty(_conStr))
                _conStr = DefaultConncetionString;


            if (_conStr == null)
            {
                ConnectionStringIsNull();

                //当连接字符串为空的时候进行默认操作
                return _opneResult;
            }
            try
            {
                Connection = new OleDbConnection(_conStr);
                Connection.Open();
                _opneResult = true;
            }
            catch (Exception ex) { throw; }
            return _opneResult;
        }

        protected override void InitCommand(string command, IEnumerable<IDataParameter> pars, CommandType type)
        {
            OpenConncetion();

            Command = new OleDbCommand(command, (Connection as OleDbConnection));
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
            DataAdapter = new OleDbDataAdapter();
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
            if (DBDataSet != null) DBDataSet.Dispose();
        }

        public override PaginationSelect CreatePagination()
        {
            throw new NotImplementedException();
        }
    }
}
