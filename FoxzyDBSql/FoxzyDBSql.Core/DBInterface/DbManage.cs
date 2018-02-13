using FoxzyDBSql.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace FoxzyDBSql.DBInterface
{
    public abstract class DbManage : IDisposable
    {
        public string ConncetionString { private set; get; }

        public int ParameterIndex
        {
            set; get;
        }

        public DbManage(String conntionString)
        {
            if (String.IsNullOrEmpty(conntionString))
                throw new ArgumentNullException("conntionString");

            this.ConncetionString = conntionString;
        }

        public abstract void OpenConncetion();

        public abstract int ExecuteNonQuery(string command, object pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);

        public abstract IDataReader ExecuteDataReader(string command, object pars = null, CommandType type = CommandType.Text);

        public abstract object ExecuteScalar(string command, object pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);

        public abstract DataSet FillDataSet(string command, object pars = null, CommandType type = CommandType.Text,
           bool isDispose = true);

        public abstract AbsDbExpression CreateSelect();

        public abstract AbsDbExpression CreateUpdate(String table);

        public abstract AbsDbExpression CreateDelete(String table);

        public abstract AbsDbExpression CreateInsert(String table);

        public abstract PaginationSelect CreatePagination();

        protected abstract void InitCommand(string command, IEnumerable<IDataParameter> pars = null, CommandType type = CommandType.Text);
        protected abstract void InitCommand(string command, IDictionary<string, object> pars = null, CommandType type = CommandType.Text);
        protected abstract void InitCommand(string command, object pars = null, CommandType type = CommandType.Text);


        protected void BuilderCommand(string command, object pars, CommandType commandType)
        {
            InputParamterType paramterBuilder = new InputParamterType(command, pars, commandType);
            paramterBuilder.OnInputIsDictionary += ParamterBuilder_OnInputIsDictionary;
            paramterBuilder.OnInputIsList += ParamterBuilder_OnInputIsList;
            paramterBuilder.OnInputIsObject += ParamterBuilder_OnInputIsObject;

            paramterBuilder.InitCommand();
        }

        private void ParamterBuilder_OnInputIsObject(object sender, DataParameterEventArgs e)
        {
            InitCommand(e.CommandSql, e.InputDataParameter, e.CommandType);
        }

        private void ParamterBuilder_OnInputIsList(object sender, DataParameterEventArgs e)
        {
            var dataParamter = e.InputDataParameter as IEnumerable<IDataParameter>;

            InitCommand(e.CommandSql, dataParamter, e.CommandType);
        }

        private void ParamterBuilder_OnInputIsDictionary(object sender, DataParameterEventArgs e)
        {
            var dataParamter = e.InputDataParameter as IDictionary<string, object>;

            InitCommand(e.CommandSql, dataParamter, e.CommandType);
        }

        public abstract void Dispose();
    }
}
