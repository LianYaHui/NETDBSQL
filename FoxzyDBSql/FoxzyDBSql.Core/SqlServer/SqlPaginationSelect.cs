using FoxzyDBSql.DBInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.SqlServer
{
    public class SqlPaginationSelect : PaginationSelect
    {
        SqlParameterConvert _ParameterConvert = new SqlParameterConvert();
        protected override IDbParameterConvert ParameterConvert
        {
            get
            {
                return _ParameterConvert;
            }
        }

        public SqlPaginationSelect(DbManage db)
            : base(db)
        {

        }

        /// <summary>
        /// Ms数据库的分页
        /// 如 select * from table
        /// 自动释放资源
        /// </summary>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">要显示的记录数</param>
        /// <param name="RowsCount">总行数</param>
        /// <param name="order">排序,对于MsSql来说这是必须的</param>
        /// <returns>DataSet</returns>
        public override DataTable Pagination(int PageIndex, int PageSize, out int RowsCount, string order)
        {
            if (PageSize < 1)
                throw new Exception("每页显示的数量 PageSize 不能小于1");

            PageIndex = PageIndex < 1 ? 1 : PageIndex;

            if (String.IsNullOrEmpty(order))
                throw new Exception("必须指定排序的列,对于MS的数据库，这是必须的");

            String RowNumberSql = String.Format("select row_number() over(order by {0}) as {1},* from ({2}) as {1}_table", order, DefaultRowNumber, this.BaseSql);

            String ReturnDataSql = String.Format("select * from ({0}) as t where t.{1} > {2} and t.{1}<= {3} ", RowNumberSql, DefaultRowNumber, (PageIndex - 1) * PageSize, PageIndex * PageSize);

            DataSet execData = db.FillDataSet(ReturnDataSql, this.DataParameters, CommandType.Text, false);

            String getCountSql =
                   String.Format("select count(*) from ({0}) as count_table", this.BaseSql);

            RowsCount = Convert.ToInt32(db.ExecuteScalar(getCountSql, new Object()));

            return execData.Tables[0];
        }

    }
}
