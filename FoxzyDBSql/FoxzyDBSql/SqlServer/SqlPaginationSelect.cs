﻿using FoxzyDBSql.DBInterface;
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

        public override PaginationSelect Set(string baseSql, object pars = null)
        {
            if (String.IsNullOrEmpty(baseSql))
                throw new ArgumentNullException("baseSql");

            this.BaseSql = baseSql;

            if (pars != null)
            {
                var properties = pars.GetType().GetProperties();
                List<SqlParameter> ListParas = new List<SqlParameter>();

                foreach (var p in properties)
                {
                    object val = p.GetValue(pars, null);
                    ListParas.Add(new SqlParameter("@" + p.Name, val));
                }

                this.DataParameters = ListParas;
            }
            return this;
        }

        public override PaginationSelect Set(string baseSql, Dictionary<string, object> pars = null)
        {
            if (String.IsNullOrEmpty(baseSql))
                throw new ArgumentNullException("baseSql");

            this.BaseSql = baseSql;
            if (pars != null)
            {
                List<SqlParameter> ListParas = new List<SqlParameter>();
                foreach (var d in pars)
                    ListParas.Add(new SqlParameter("@" + d.Key, d.Value));
                this.DataParameters = ListParas;
            }

            return this;
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
        public override System.Data.DataSet Pagination(int PageIndex, int PageSize, out int RowsCount, string order)
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

            return execData;
        }

    }
}
