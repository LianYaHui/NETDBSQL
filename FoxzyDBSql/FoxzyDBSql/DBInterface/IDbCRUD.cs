using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FoxzyDBSql.DBInterface
{
    internal interface IDbCRUD
    {
        string BuildSelect();

        string BuildUpdate();

        string BuildDelete();

        string BuildInsert();

        string BuildSql();

        DataTable Pagination(int PageIndex, int PageSize, out int RowsCount);
    }
}
