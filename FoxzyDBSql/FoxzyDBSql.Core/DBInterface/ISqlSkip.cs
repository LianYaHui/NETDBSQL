using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    public interface ISqlSkipTake
    {
        /// <summary>
        /// 跳过指定的行数的语句
        /// </summary>
        /// <param name="skipRowCount">跳过的行数</param>
        /// <returns></returns>
        AbsDbExpression Skip(int skipRowCount);

        /// <summary>
        /// 返回指定行数的语句
        /// </summary>
        /// <param name="takeRowCount">返回指定的行数</param>
        /// <returns></returns>
        AbsDbExpression Take(int takeRowCount);
    }
}
