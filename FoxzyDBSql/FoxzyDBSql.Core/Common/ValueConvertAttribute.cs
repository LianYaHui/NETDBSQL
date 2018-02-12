using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public abstract class ValueConvertAttribute : Attribute
    {
        public abstract object ConvertToModel(DataRow row, object rowValue);

        public abstract object ConverToParameter(object entry, object value);
    }
}
