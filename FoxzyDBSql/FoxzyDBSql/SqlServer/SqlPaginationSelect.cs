using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.SqlServer
{
    class SqlPaginationSelect : PaginationSelect
    {
        public SqlPaginationSelect(DbManage sqlManageUtil)
            : base(sqlManageUtil)
        {
        }

        public override System.Data.DataSet Pagination(int PageIndex, int PageSize, out int RowsCount, string order)
        {
            throw new NotImplementedException();
        }
    }
}
