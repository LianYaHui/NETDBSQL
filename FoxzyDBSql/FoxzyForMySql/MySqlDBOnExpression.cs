using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyForMySql
{
    public class MySqlDBOnExpression : DBOnExpression
    {
        public override string ToString()
        {
            String table = (this.TableName == this.AsName ? this.TableName : string.Format("{0} as {1}", this.TableName, this.AsName));
            return String.Format(" {0} {1} on {2}", JoinTypeToString(this.JoinType), table, this.Expression);
        }
    }
}
