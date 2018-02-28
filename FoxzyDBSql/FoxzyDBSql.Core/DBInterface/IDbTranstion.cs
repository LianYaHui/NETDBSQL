using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    /// <summary>
    /// 实现事务的接口
    /// </summary>
    public interface IDbTranstion
    {
        /// <summary>
        /// 执行实物
        /// </summary>
        event OnTranstionEvent OnTranstion;

        bool StartTranstion(dynamic TranstionData, IsolationLevel isolationLevel);
    }
}
