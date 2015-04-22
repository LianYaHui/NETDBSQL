using FoxzyDBSql.DBInterface;
using System;

namespace FoxzyDBSql.SqlServer
{
    public class SqlDBOnExpression : DBOnExpression
    {
        public override string ToString()
        {
            String table = (this.TableName == this.AsName ? this.TableName : string.Format("{0} as {1}", this.TableName, this.AsName));
            String tmpStr = String.Format(" {0} {1}", JoinTypeToString(this.JoinType), table);

            if (this.JoinType == SqlJoinType.CrossJoin)
                return tmpStr;

            return String.Format("{0} on {1}", tmpStr, Expression);
        }
    }
}
