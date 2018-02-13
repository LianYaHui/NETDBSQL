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

        protected string BaseSql { get; set; }
        protected DbManage db = null;
        protected IEnumerable<IDataParameter> DataParameters { get; set; }
        protected abstract IDbParameterConvert ParameterConvert { get; }

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

        public PaginationSelect Set(String baseSql, Dictionary<string, object> pars)
        {
            if (String.IsNullOrEmpty(baseSql))
                throw new ArgumentNullException("baseSql");

            this.BaseSql = baseSql;
            this.DataParameters = ParameterConvert.FromDictionaryToParameters(pars, db.ParameterIndex);

            return this;
        }

        public PaginationSelect Set(String baseSql, object pars)
        {
            if (String.IsNullOrEmpty(baseSql))
                throw new ArgumentNullException("baseSql");

            this.BaseSql = baseSql;
            this.DataParameters = ParameterConvert.FromObjectToParameters(pars, db.ParameterIndex);

            return this;
        }

        public abstract DataTable Pagination(int PageIndex, int PageSize, out int RowsCount, String order);
    }
}
