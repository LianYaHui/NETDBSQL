using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.SqlServer
{
    public class SqlDBOnExpression : IDBOnExpression
    {
        public AbsDbExpression On(string joinOnString)
        {
            this.Expression = joinOnString;
            currentExpression._keyObject.Join.Add(this.Type + this.AsName.ToLower(), this);
            return currentExpression;
        }

        public string TableName
        {
            get;
            set;
        }

        public string AsName
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        AbsDbExpression currentExpression;


        public IDBOnExpression Fill(AbsDbExpression exp)
        {
            this.currentExpression = exp;
            return this;
        }


        public string Expression
        {
            get;
            set;
        }

        public override string ToString()
        {
            String table = (this.TableName == this.AsName ? this.TableName : string.Format("{0} as {1}", this.TableName, this.AsName));
            return String.Format(" {0} {1} on {2}", this.Type, table, this.Expression);
        }
    }
}
