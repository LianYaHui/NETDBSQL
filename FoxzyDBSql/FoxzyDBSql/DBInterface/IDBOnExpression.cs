using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public interface IDBOnExpression
    {
        AbsDbExpression On(string joinOnString);

        String TableName { set; get; }

        String AsName { set; get; }

        String Type { set; get; }

        String Expression { set; get; }

        IDBOnExpression Fill(AbsDbExpression exp);
    }
}
