﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public abstract class DbManage : IDisposable
    {
        public string ConncetionString { private set; get; }
        protected abstract IDbParameterConvert ParameterConvert
        {
            get;
        }


        public DbManage(String conntionString)
        {
            if (String.IsNullOrEmpty(conntionString))
                throw new ArgumentNullException("conntionString");

            this.ConncetionString = conntionString;
        }

        public abstract void OpenConncetion();

        public abstract int ExecuteNonQuery(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text,
            bool isDispose = true);

        public abstract int ExecuteNonQuery(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text,
            bool isDispose = true);

        public abstract int ExecuteNonQuery(string command, object pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);


        public abstract IDataReader ExecuteDataReader(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);

        public abstract IDataReader ExecuteDataReader(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text);

        public abstract IDataReader ExecuteDataReader(string command, object pars = null, CommandType type = CommandType.Text);

        public abstract object ExecuteScalar(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text,
            bool isDispose = true);

        public abstract object ExecuteScalar(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);
        public abstract object ExecuteScalar(string command, object pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);

        public abstract DataSet FillDataSet(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text,
            bool isDispose = true);
        public abstract DataSet FillDataSet(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);
        public abstract DataSet FillDataSet(string command, object pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);

        public abstract AbsDbExpression CreateSelect();

        public abstract AbsDbExpression CreateUpdate(String table);

        public abstract AbsDbExpression CreateDelete(String table);

        public abstract AbsDbExpression CreateInsert(String table);

        public abstract PaginationSelect CreatePagination();

        protected abstract void InitCommand(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);
        protected abstract void InitCommand(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text);
        protected abstract void InitCommand(string command, object pars = null, CommandType type = CommandType.Text);

        public abstract void Dispose();
    }
}
