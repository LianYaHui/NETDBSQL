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
        public MySqlManageUtil(String ConnetionString)
            : base(ConnetionString)
        { }

        public static IEnumerable<IDataParameter> CloneParameter(IEnumerable<IDataParameter> pars)
        {
            if (pars == null)
                return null;

            List<IDataParameter> list = new List<IDataParameter>();
            foreach (var p in pars)
            {
                list.Add(new MySqlParameter(p.ParameterName, p.Value));
            }
            return list;
        }



        public override bool OpenConncetion()
        {
            bool _opneResult = false;
            try
            {
                Connection = new MySqlConnection(ConncetionString);
                Connection.Open();
                _opneResult = true;
            }
            catch { throw; }
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

        /// <summary>
        ///  执行语句或者存储过程,返回收影响的行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  执行语句或者存储过程,放回IDataReader对象
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override System.Data.IDataReader ExecuteDataReader(string command, IEnumerable<System.Data.IDataParameter> pars = null, System.Data.CommandType type = CommandType.Text)
        {
            InitCommand(command, pars, type);
            try
            {
                return Command.ExecuteReader();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 执行语句或者存储过程,返回第一行第一列的值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行语句或者存储过程,返回第一行第一列的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 用语句或者存储过程填充DataSet
        /// </summary>
        /// <param name="command">Sql命令</param>
        /// <param name="pars">参数化集合</param>
        /// <param name="type">指明CommandType</param>
        /// <returns>填充的DataSet</returns>
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

        /// <summary>
        /// 释放资源和链接
        /// </summary>
        public override void Dispose()
        {
            if (Connection != null) Connection.Dispose();
            if (Command != null) Command.Dispose();
            if (DataAdapter != null) DataAdapter.Dispose();
            if (DBDataSet != null) DBDataSet.Dispose();
        }

        /// <summary>
        /// 创建一个Select语句
        /// </summary>
        /// <returns></returns>
        public override AbsDbExpression CreateSelect()
        {
            var _sql = new MySqlExpression(this, SqlExceType.Select);
            return _sql;
        }

        /// <summary>
        /// 创建一个Update 语句，支持多表连接更新
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>表达式对象。</returns>
        public override AbsDbExpression CreateUpdate(string table)
        {
            return new MySqlExpression(this, SqlExceType.Update).Update(table);
        }

        /// <summary>
        /// 创建一个Delete语句
        /// </summary>
        /// <param name="table">要删除的表</param>
        /// <returns>表达式对象。注意请使用Where 子句构建条件</returns>
        public override AbsDbExpression CreateDelete(string table)
        {
            return new MySqlExpression(this, SqlExceType.Delete).Delete(table);
        }

        /// <summary>
        /// 创建一个Insert语句
        /// </summary>
        /// <param name="table">要insert的表名</param>
        /// <returns>表达式对象</returns>
        public override AbsDbExpression CreateInsert(string table)
        {
            return new MySqlExpression(this, SqlExceType.Insert).Insert(table);
        }

        public override PaginationSelect CreatePagination()
        {
            return new MySqlPaginationSelect(this);
        }
    }
}
