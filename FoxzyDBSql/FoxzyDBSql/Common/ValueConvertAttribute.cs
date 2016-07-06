using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class ValueConvertAttribute : Attribute
    {
        public string ConvertToModel { get; set; }

        public string ConverToParameter { get; set; }
    }
}
