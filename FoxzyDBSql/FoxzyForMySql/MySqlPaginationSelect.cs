using FoxzyDBSql.DBInterface;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyForMySql
{
    class MySqlPaginationSelect : PaginationSelect
    {
        public MySqlPaginationSelect(DbManage db)
            : base(db)
        {
        }

        public override System.Data.DataSet Pagination(int PageIndex, int PageSize, out int RowsCount, string order = null)
        {
            if (PageSize < 1)
                throw new Exception("每页显示的数量 PageSize 不能小于1");

            PageIndex = PageIndex < 1 ? 1 : PageIndex;

            String baseSql = this.BaseSql;

            String getCountSql =
                String.Format("select count(*) from ({0}) as count_table", baseSql);

            RowsCount = Convert.ToInt32(db.ExecuteScalar(getCountSql, this.DataParameters, CommandType.Text));

            String ReturnDataSql = String.Format("{0} limit {1},{2}", baseSql, (PageIndex - 1) * PageSize, PageSize);

            IEnumerable<IDataParameter> newPars = MySqlManageUtil.CloneParameter(this.DataParameters);
            return db.FillDataSet(ReturnDataSql, newPars, CommandType.Text);
        }
    }
}
