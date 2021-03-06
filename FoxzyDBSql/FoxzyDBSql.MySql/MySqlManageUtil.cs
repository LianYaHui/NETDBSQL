﻿using FoxzyDBSql.Common;
using FoxzyDBSql.DBInterface;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.MySql
{
    public class MySqlManageUtil : DbManage, IDbTranstion
    {
        MySqlConnection Connection = null;
        MySqlCommand Command = null;
        MySqlDataAdapter DataAdapter = null;

        private bool disposed = false;



        /// <summary>
        /// 参数化的前导字符
        /// </summary>
        private readonly string ParametersPlaceholder = MySqlEnvParameter.ParametersPlaceholder;
        private IDbParameterConvert ParameterConvert = MySqlEnvParameter.ParameterConvert;

        /// <summary>
        /// 用连接字符串初始化新的实例
        /// </summary>
        /// <param name="ConnetionString">ADO.NET连接字符串</param>
        public MySqlManageUtil(String ConnetionString)
            : base(ConnetionString)
        {

        }


        /// <summary>
        /// 重载基类方法,打开数据库连接
        /// </summary>
        /// <returns></returns>
        public override void OpenConncetion()
        {
            if (Connection == null)
                Connection = new MySqlConnection(ConncetionString);
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        protected override void InitCommand(string command, IEnumerable<IDataParameter> pars, CommandType type)
        {
            OpenConncetion();

            if (Command == null)
                Command = new MySqlCommand();

            Command.CommandText = command;
            Command.Connection = (Connection);
            Command.CommandType = type;

            if (pars == null) return;
            Command.Parameters.AddRange(pars.ToArray());
        }

        protected override void InitCommand(string command, IDictionary<string, object> pars = null, CommandType type = CommandType.Text)
        {
            OpenConncetion();

            if (Command == null)
                Command = new MySqlCommand();

            Command.CommandText = command;
            Command.Connection = (Connection);
            Command.CommandType = type;

            var paraList = ParameterConvert.FromDictionaryToParameters(pars, ParameterIndex);
            Command.Parameters.AddRange(paraList.ToArray());
        }

        protected override void InitCommand(string command, object pars = null, CommandType type = CommandType.Text)
        {
            OpenConncetion();

            if (Command == null)
                Command = new MySqlCommand();

            Command.CommandText = command;
            Command.Connection = (Connection);
            Command.CommandType = type;

            var paraList = ParameterConvert.FromObjectToParameters(pars, ParameterIndex);
            Command.Parameters.AddRange(paraList.ToArray());
        }


        /// <summary>
        /// 执行SQL并返回DataReader,必须手动调用Dispose
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override IDataReader ExecuteDataReader(string command, object pars = null, CommandType type = CommandType.Text)
        {
            BuilderCommand(command, pars, type);
            return Command.ExecuteReader();
        }

        public override int ExecuteNonQuery(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            BuilderCommand(command, pars, type);

            int _result = Command.ExecuteNonQuery();
            if (isDispose) Dispose();
            return _result;
        }



        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="command">sql 语句</param>
        /// <param name="pars">参数集</param>
        /// <param name="type">CommandType 指定执行的是sql,还是存储过程</param>
        /// <returns>结果集中第一行的第一列。</returns>
        public override object ExecuteScalar(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            BuilderCommand(command, pars, type);
            object _result = Command.ExecuteScalar();
            if (isDispose) Dispose();
            return _result;
        }


        public override void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        ~MySqlManageUtil()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }
                if (Command != null) { Command.Dispose(); Command = null; }
                if (Command != null) { Command.Dispose(); Command = null; }
                if (DataAdapter != null) { DataAdapter.Dispose(); DataAdapter = null; }
            }
            // 清理非托管资源

            //让类型知道自己已经被释放
            disposed = true;
        }




        /// <summary>
        /// 执行一个查询
        /// </summary>
        /// <param name="command">sql 语句</param>
        /// <param name="pars">参数集</param>
        /// <param name="type">CommandType 指定执行的是sql,还是存储过程</param>
        /// <returns>执行sql生成的数据集。</returns>
        public override DataSet FillDataSet(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            DataAdapter = new MySqlDataAdapter();
            DataSet DBDataSet = new DataSet();

            BuilderCommand(command, pars, type);
            DataAdapter.SelectCommand = Command;

            DataAdapter.Fill(DBDataSet);

            if (isDispose) Dispose();
            return DBDataSet;
        }


        /// <summary>
        /// 开始生成一个SELECT语句
        /// </summary>
        /// <returns></returns>
        public override AbsDbExpression CreateSelect()
        {
            ParameterIndex++;
            var _sql = new MySqlExpression(this, Common.SqlExceType.Select);
            return _sql;
        }

        /// <summary>
        /// 开始生成一个Update语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public override AbsDbExpression CreateUpdate(String table)
        {
            ParameterIndex++;

            return new MySqlExpression(this, Common.SqlExceType.Update).Update(table);
        }

        /// <summary>
        /// 开始生成一个Delete语句
        /// </summary>
        /// <param name="table">要Delete的表名</param>
        /// <returns></returns>
        public override AbsDbExpression CreateDelete(String table)
        {
            ParameterIndex++;

            return new MySqlExpression(this, Common.SqlExceType.Delete).Delete(table);
        }

        /// <summary>
        /// 开始生成一个Insert语句
        /// </summary>
        /// <param name="table">要Insert的表名</param>
        /// <returns></returns>
        public override AbsDbExpression CreateInsert(String table)
        {
            ParameterIndex++;
            return new MySqlExpression(this, Common.SqlExceType.Insert).Insert(table);
        }

        /// <summary>
        /// 创建一个要分页查询的执行器
        /// </summary>
        /// <returns></returns>
        public override PaginationSelect CreatePagination()
        {
            ParameterIndex++;
            return new MySqlPaginationSelect(this);
        }

        /// <summary>
        /// 执行事务的事件，所有的操作都不允许Dispose
        /// </summary>
        public event OnTranstionEvent OnTranstion;

        public bool StartTranstion(dynamic TranstionData = null, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (OnTranstion == null)
                return false;

            OpenConncetion();

            var sqlTran = Connection.BeginTransaction();

            if (Command == null)
                Command = new MySqlCommand();

            Command.Connection = Connection;
            Command.Transaction = sqlTran;

            try
            {
                OnTranstion.Invoke(this, new TranstionEventArgs(TranstionData));
                sqlTran.Commit();
            }
            catch
            {
                sqlTran.Rollback();
                throw;
            }
            finally
            {
                sqlTran.Dispose();
                Dispose();
            }


            return true;
        }
    }
}
