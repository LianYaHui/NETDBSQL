using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public abstract class PaginationSelect
    {
        public static String DefaultRowNumber = "RowNum";
        public readonly static int selectLength = "select".Length;

        protected string BaseSql = String.Empty;
        protected DbManage db = null;
        protected IEnumerable<IDataParameter> DataParameters;

        public PaginationSelect(DbManage db)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            this.db = db;
        }

        public PaginationSelect Set(String baseSql, IEnumerable<IDataParameter> pars)
        {
            if (String.IsNullOrEmpty(baseSql))
                throw new ArgumentNullException("baseSql");

            this.BaseSql = baseSql;
            this.DataParameters = pars;

            return this;
        }

        public abstract DataSet Pagination(int PageIndex, int PageSize, out int RowsCount, String order);
    }
}
