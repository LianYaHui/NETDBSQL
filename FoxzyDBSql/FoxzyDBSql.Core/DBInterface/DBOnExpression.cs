using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public enum SqlJoinType
    {
        InnerJoin = 1,
        LeftJoin = 2,
        RightJoin = 4,
        FullJoin = 8,
        CrossJoin = 16
    }

    public class DBOnExpression
    {
        protected AbsDbExpression currentExpression;

        public AbsDbExpression On(string joinOnString)
        {
            this.Expression = joinOnString;
            currentExpression._keyObject.Join.Add(JoinTypeToString(this.JoinType) + this.AsName.ToLower(), this);
            return currentExpression;
        }

        public String TableName { set; get; }

        public String AsName { set; get; }

        public SqlJoinType JoinType { set; get; }

        public String Expression { set; get; }

        public DBOnExpression Fill(AbsDbExpression exp)
        {
            this.currentExpression = exp;
            return this;
        }

        protected String JoinTypeToString(SqlJoinType JoinType)
        {
            int JoinLength = "join".Length;

            String typeString = JoinType.ToString().ToLower();

            int emtryIndex = typeString.Length - JoinLength;

            return typeString.Insert(emtryIndex, " ");
        }
    }
}
