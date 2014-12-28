using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.SqlServer
{
    class SqlPaginationSelect : PaginationSelect
    {

        public SqlPaginationSelect(DbManage db)
            : base(db)
        {
        }

        /// <summary>
        /// Ms数据库的分页,查询的语句中比如含有 $$RowNumber,
        /// 如 select *,$$RowNumber from table
        /// 并且原表中不能含有Num列
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">要显示的记录数</param>
        /// <param name="RowsCount">总行数</param>
        /// <param name="order">排序,对于MsSql来说这是必须的</param>
        /// <returns>DataSet</returns>
        public override System.Data.DataSet Pagination(int PageIndex, int PageSize, out int RowsCount, string order)
        {
            if (PageSize < 1)
                throw new Exception("每页显示的数量 PageSize 不能小于1");

            PageIndex = PageIndex < 1 ? 1 : PageIndex;

            if (String.IsNullOrEmpty(order))
                throw new Exception("必须指定排序的列,对于MS的数据库，这是必须的 ");

            String getCountSql =
                String.Format("select count(*) from ({0}) as count_table", this.BaseSql);

            //TODO
            //正则提取 from 子句
            String _sql = this.BaseSql.ToUpper();
            int fromIndex = _sql.IndexOf("FROM ");

            this.BaseSql = this.BaseSql.Insert(fromIndex, String.Format(",row_number() over(order by {0}) as Num ", order));

            RowsCount = Convert.ToInt32(db.ExecuteScalar(getCountSql, this.DataParameters, CommandType.Text));

            String ReturnDataSql = String.Format("select * from ({0}) as t where t.Num>{1} and t.Num<= {2} ", this.BaseSql, (PageIndex - 1) * PageSize, PageIndex * PageSize);

            IEnumerable<SqlParameter> newPars = SqlManageUtil.CloneParameter(this.DataParameters);

            IEnumerable<IDataParameter> DataPars = null;
            if (newPars != null)
                DataPars = newPars.Select(p => p as IDataParameter);

            return db.FillDataSet(ReturnDataSql, DataPars, CommandType.Text);
        }
    }
}
