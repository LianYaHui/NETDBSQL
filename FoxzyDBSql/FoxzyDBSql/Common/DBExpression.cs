using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class DBExpression
    {
        public String Field { get; private set; }

        public String Operator { get; private set; }

        public DbType FieldType { get; private set; }

        public Object[] Values { get; private set; }

        public String custom { get; private set; }


        public DBExpression Custom(String wheresql)
        {
            return new DBExpression(wheresql);
        }

        public DBExpression(String custom)
        {
            this.custom = custom;
        }

        public DBExpression(String field, String Operator, DbType type, params object[] values)
        {
            this.Field = field;
            this.Operator = Operator;
            this.FieldType = type;
            this.Values = values;
        }

        public static DBExpression Eq(String field, object val, DbType type)
        {
            return new DBExpression(field, "=", type, val);
        }

        public static DBExpression NotEq(String field, object val, DbType type)
        {
            return new DBExpression(field, "!=", type, val);
        }

        public static DBExpression Gt(String field, object val, DbType type)
        {
            return new DBExpression(field, ">", type, val);
        }

        public static DBExpression Ge(String field, object val, DbType type)
        {
            return new DBExpression(field, ">=", type, val);
        }

        public static DBExpression Lt(String field, object val, DbType type)
        {
            return new DBExpression(field, "<", type, val);
        }

        public static DBExpression Le(String field, object val, DbType type)
        {
            return new DBExpression(field, "<=", type, val);
        }

        public static DBExpression Like(String field, String val)
        {
            return new DBExpression(field, "like", DbType.String, val);
        }

        public static DBExpression In(String field, object val, DbType type)
        {
            return new DBExpression(field, "in", type, val);
        }

        public static DBExpression NotIn(String field, object val, DbType type)
        {
            return new DBExpression(field, "not in", type, val);
        }
    }
}
