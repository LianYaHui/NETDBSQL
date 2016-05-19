using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.SqlServer
{
    public class SqlManageUtil : DbManage
    {
        SqlConnection Connection = null;
        SqlCommand Command = null;
        SqlDataAdapter DataAdapter = null;


        SqlParameterConvert _ParameterConvert = new SqlParameterConvert();
        protected override IDbParameterConvert ParameterConvert
        {
            get
            {
                return _ParameterConvert;
            }
        }

        /// <summary>
        /// 用连接字符串初始化新的实例
        /// </summary>
        /// <param name="ConnetionString">ADO.NET连接字符串</param>
        public SqlManageUtil(String ConnetionString)
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
                Connection = new SqlConnection(ConncetionString);
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        protected override void InitCommand(string command, IEnumerable<IDataParameter> pars, CommandType type)
        {
            OpenConncetion();

            if (Command == null)
                Command = new SqlCommand();

            Command.CommandText = command;
            Command.Connection = (Connection);
            Command.CommandType = type;

            if (pars == null) return;
            Command.Parameters.AddRange(pars.ToArray());
        }

        protected override void InitCommand(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text)
        {
            OpenConncetion();

            if (Command == null)
                Command = new SqlCommand();

            Command.CommandText = command;
            Command.Connection = (Connection);
            Command.CommandType = type;

            var paraList = ParameterConvert.FromDictionaryToParameters(pars);
            Command.Parameters.AddRange(paraList.ToArray());
        }

        protected override void InitCommand(string command, object pars = null, CommandType type = CommandType.Text)
        {
            OpenConncetion();

            if (Command == null)
                Command = new SqlCommand();

            Command.CommandText = command;
            Command.Connection = (Connection);
            Command.CommandType = type;

            var paraList = ParameterConvert.FromObjectToParameters(pars);
            Command.Parameters.AddRange(paraList.ToArray());
        }


        /// <summary>
        /// 执行SQL并返回DataReader,必须手动调用Dispose
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override IDataReader ExecuteDataReader(string command,
            IEnumerable<IDataParameter> pars = null,
            CommandType type = CommandType.Text)
        {
            InitCommand(command, pars, type);

            return Command.ExecuteReader();
        }
        public override IDataReader ExecuteDataReader(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text)
        {
            InitCommand(command, pars, type);
            return Command.ExecuteReader();
        }

        public override IDataReader ExecuteDataReader(string command, object pars = null, CommandType type = CommandType.Text)
        {
            InitCommand(command, pars, type);

            return Command.ExecuteReader();
        }



        /// <summary>
        /// 执行一段Sql,返回受影响的行数
        /// </summary>
        /// <param name="command">sql 语句</param>
        /// <param name="pars">参数集</param>
        /// <param name="type">CommandType 指定执行的是sql,还是存储过程</param>
        /// <returns>受影响的行数。</returns>
        public override int ExecuteNonQuery(string command,
            IEnumerable<IDataParameter> pars = null,
            CommandType type = CommandType.Text,
            bool isDispose = true)
        {
            InitCommand(command, pars, type);
            int _result = Command.ExecuteNonQuery();
            if (isDispose) Dispose();
            return _result;
        }

        public override int ExecuteNonQuery(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            InitCommand(command, pars, type);
            int _result = Command.ExecuteNonQuery();
            if (isDispose) Dispose();
            return _result;
        }

        public override int ExecuteNonQuery(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            InitCommand(command, pars, type);
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
        public override object ExecuteScalar(string command,
            IEnumerable<IDataParameter> pars = null,
            CommandType type = CommandType.Text,
            bool isDispose = true)
        {
            InitCommand(command, pars, type);
            object _result = Command.ExecuteScalar();
            if (isDispose) Dispose();
            return _result;
        }

        public override object ExecuteScalar(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            InitCommand(command, pars, type);
            object _result = Command.ExecuteScalar();
            if (isDispose) Dispose();
            return _result;
        }

        public override object ExecuteScalar(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            InitCommand(command, pars, type);
            object _result = Command.ExecuteScalar();
            if (isDispose) Dispose();
            return _result;
        }




        /// <summary>
        /// 释放所有打开的资源
        /// </summary>
        public override void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
            if (Command != null) { Command.Dispose(); Command = null; }
            if (Command != null) { Command.Dispose(); Command = null; }
            if (DataAdapter != null) { DataAdapter.Dispose(); DataAdapter = null; }
        }

        /// <summary>
        /// 执行一个查询
        /// </summary>
        /// <param name="command">sql 语句</param>
        /// <param name="pars">参数集</param>
        /// <param name="type">CommandType 指定执行的是sql,还是存储过程</param>
        /// <returns>执行sql生成的数据集。</returns>
        public override DataSet FillDataSet(string command,
            IEnumerable<IDataParameter> pars = null,
            CommandType type = CommandType.Text,
            bool isDispose = true)
        {
            DataAdapter = new SqlDataAdapter();
            DataSet DBDataSet = new DataSet();

            InitCommand(command, pars, type);
            DataAdapter.SelectCommand = Command;

            try
            {
                DataAdapter.Fill(DBDataSet);

                if (isDispose) Dispose();
                return DBDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataSet FillDataSet(string command, Dictionary<string, object> pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            DataAdapter = new SqlDataAdapter();
            DataSet DBDataSet = new DataSet();

            InitCommand(command, pars, type);
            DataAdapter.SelectCommand = Command;

            try
            {
                DataAdapter.Fill(DBDataSet);

                if (isDispose) Dispose();
                return DBDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataSet FillDataSet(string command, object pars = null, CommandType type = CommandType.Text, bool isDispose = true)
        {
            DataAdapter = new SqlDataAdapter();
            DataSet DBDataSet = new DataSet();

            InitCommand(command, pars, type);
            DataAdapter.SelectCommand = Command;

            try
            {
                DataAdapter.Fill(DBDataSet);

                if (isDispose) Dispose();
                return DBDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 开始生成一个SELECT语句
        /// </summary>
        /// <returns></returns>
        public override AbsDbExpression CreateSelect()
        {
            var _sql = new SqlExpression(this, Common.SqlExceType.Select);
            return _sql;
        }

        /// <summary>
        /// 开始生成一个Update语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public override AbsDbExpression CreateUpdate(String table)
        {
            return new SqlExpression(this, Common.SqlExceType.Update).Update(table);
        }

        /// <summary>
        /// 开始生成一个Delete语句
        /// </summary>
        /// <param name="table">要Delete的表名</param>
        /// <returns></returns>
        public override AbsDbExpression CreateDelete(String table)
        {
            return new SqlExpression(this, Common.SqlExceType.Delete).Delete(table);
        }

        /// <summary>
        /// 开始生成一个Insert语句
        /// </summary>
        /// <param name="table">要Insert的表名</param>
        /// <returns></returns>
        public override AbsDbExpression CreateInsert(String table)
        {
            return new SqlExpression(this, Common.SqlExceType.Insert).Insert(table);
        }

        /// <summary>
        /// 创建一个要分页查询的执行器
        /// </summary>
        /// <returns></returns>
        public override PaginationSelect CreatePagination()
        {
            return new SqlPaginationSelect(this);
        }

        /// <summary>
        /// 开启一个事物
        /// </summary>
        /// <param name="action">事物中包含的动作</param>
        /// <returns>是否执行成功</returns>
        public override bool ExecTranstion(Action<DbManage> action, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (action == null)
                throw new ArgumentNullException("action");


            OpenConncetion();

            SqlConnection conn = Connection as SqlConnection;
            var sqlTran = conn.BeginTransaction();

            if (Command == null)
                Command = new SqlCommand();

            Command.Connection = conn;
            Command.Transaction = sqlTran;

            try
            {
                action.Invoke(this);
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                throw ex;
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
