using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FoxzyDBSql
{
    public class DataParameterEventArgs : EventArgs
    {
        public String CommandSql { set; get; }
        public CommandType CommandType { set; get; }

        public object InputDataParameter { set; get; }

        public DataParameterEventArgs(string command, object inputParamter, CommandType type)
        {
            InputDataParameter = inputParamter;

            CommandSql = command;
            CommandType = type;
        }
    }
}
