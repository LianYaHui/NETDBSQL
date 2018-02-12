using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public abstract class ValueConvertAttribute : Attribute
    {
        public int ExecuteIndex { set; get; }

        public abstract object ConvertToModel(DataRow row, object rowValue);

        public abstract object ConverToParameter(object entry, object value);
    }
}
