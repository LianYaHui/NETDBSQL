using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public abstract class PaginationSelect
    {
        protected string BaseSql = String.Empty;
        protected DbManage db = null;
        protected IEnumerable<IDataParameter> DataParameters;

        public PaginationSelect(DbManage db)
        {
            this.db = db;
        }

        public void Set(String baseSql, IEnumerable<IDataParameter> pars)
        {
            this.BaseSql = baseSql;
            this.DataParameters = pars;
        }

        public abstract DataSet Pagination(int PageIndex, int PageSize, out int RowsCount, String order);
    }
}
