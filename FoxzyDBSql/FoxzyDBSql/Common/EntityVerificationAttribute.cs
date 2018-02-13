using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FoxzyDBSql.Common
{
    public abstract class EntityVerificationAttribute : Attribute
    {
        public int ExecuteIndex { set; get; }

        public abstract void Verification(DataRow sourceRow, object TModel);
    }
}
