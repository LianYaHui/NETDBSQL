using FoxzyDBSql.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public class DBSqlKeyObject
    {
        public SqlExceType SqlType { set; get; }

        public String SelectStr { set; get; }

        public String FromTable { set; get; }

        public Dictionary<String, String> Tables { set; get; }

        public String WhereSql { set; get; }

        public Hashtable Sort { set; get; }

        public Hashtable Join { set; get; }

        public List<System.Data.IDataParameter> DataParameters { set; get; }

        public List<System.Data.IDataParameter> OperateObjectParameters { set; get; }

        public List<String> InsertColoums { set; get; }

        public HashSet<String> GroupByField { set; get; }

        public String UpdateTable { set; get; }

        public Hashtable Set { set; get; }

        public String Limit { set; get; }

        public int SkipRows { set; get; }

        public int TakeRows { set; get; }

        public DBSqlKeyObject()
        {
            Tables = new Dictionary<string, string>();
            Sort = new Hashtable();
            Join = new Hashtable();

            Set = new Hashtable();

            OperateObjectParameters = new List<IDataParameter>();
            InsertColoums = new List<string>();

            DataParameters = new List<IDataParameter>();
            GroupByField = new HashSet<string>();
        }

        public static DBSqlKeyObject Create()
        {
            return new DBSqlKeyObject();
        }

        public string HavingSql { get; set; }

        public string DeleteTable { get; set; }

        public string IntoTable { get; set; }

        public string InsertTable { get; set; }
    }
}
