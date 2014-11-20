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
        static SqlManageUtil()
        {

        }

        public static List<SqlParameter> CloneParameter(IEnumerable<IDataParameter> pars)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (var p in pars)
            {
                list.Add(new SqlParameter(p.ParameterName, p.Value));
            }
            return list;
        }


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
                Connection = new SqlConnection(_conStr);
                Connection.Open();
                _opneResult = true;
            }
            catch (Exception ex) { throw; }
            return _opneResult;
        }

        protected override void InitCommand(string command, IEnumerable<IDataParameter> pars, CommandType type)
        {
            OpenConncetion();

            Command = new SqlCommand(command, (Connection as SqlConnection));
            Command.CommandType = type;

            Command.Parameters.Clear();


            if (pars == null) return;
            Command.Parameters.AddRange(pars.ToArray());
        }

        /// <summary>
        /// 执行SQL并返回DataReader,必须手动调用Dispose
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pars"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override IDataReader ExecuteDataReader(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text)
        {
            InitCommand(command, pars, type);
            try
            {
                return Command.ExecuteReader();
            }
            catch (Exception ex) { throw ex; }
        }

        public override int ExecuteNonQuery(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text)
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

        public override object ExecuteScalar(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text)
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

        public override T ExecuteScalar<T>(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text)
        {
            try
            {
                var obj = ExecuteScalar(command, pars, type);
                return (T)(obj);
            }
            catch
            {
                throw new Exception();
            }
        }


        public override void Dispose()
        {
            if (Connection != null) Connection.Dispose();
            if (Command != null) Command.Dispose();
            if (DataAdapter != null) DataAdapter.Dispose();
            if (DBDataSet != null) DBDataSet.Dispose();
        }

        public override DataSet FillDataSet(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text)
        {
            DataAdapter = new SqlDataAdapter();
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

        public override int BulkCopyInsert(String tabelName, DataTable data)
        {
            SqlBulkCopy _bulkcopy = init_bulkcopy(tabelName, data.Rows.Count);

            try
            {
                _bulkcopy.WriteToServer(data);
                return 1;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _bulkcopy.Close();
                Dispose();
            }
        }

        private static SqlBulkCopy init_bulkcopy(String tabelName, int count)
        {
            SqlBulkCopy _bulkcopy = new SqlBulkCopy(ConncetionString, SqlBulkCopyOptions.UseInternalTransaction);

            _bulkcopy.BatchSize = count;

            _bulkcopy.DestinationTableName = tabelName;
            return _bulkcopy;
        }

        public override AbsDbExpression CreateSelect()
        {
            var _sql = new SqlExpression(this, Common.SqlExceType.Select);
            return _sql;
        }

        public override AbsDbExpression CreateUpdate(String table)
        {
            return new SqlExpression(this, Common.SqlExceType.Update).Update(table);
        }

        public override AbsDbExpression CreateDelete(String table)
        {
            return new SqlExpression(this, Common.SqlExceType.Delete).Delete(table);
        }

        public override AbsDbExpression CreateInsert(String table)
        {
            return new SqlExpression(this, Common.SqlExceType.Insert).Insert(table);
        }
    }
}
