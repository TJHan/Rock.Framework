using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Data.DataAccess
{
    public class SelectStatement
    {
        /// <summary>
        /// 编辑分页查询语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryColumnName"></param>
        /// <param name="fields"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string PagingSelectStatement(string tableName, string primaryColumnName, string fields, int pageIndex, int pageSize, string filter, string orderBy)
        {
            string whereStr = string.IsNullOrEmpty(filter) ? string.Empty : string.Format(@"WHERE {0}", filter);
            string sql = string.Format(@"WITH PagingTable AS(
                                SELECT TOP {4} {2}, ROW_NUMBER() OVER(ORDER BY {3}) AS RowNbr FROM {0} {1}
                                )
                                SELECT * FROM PagingTable WHERE RowNbr >{5};
                                SELECT COUNT(*) FROM {0} {1};", tableName
                                                              , whereStr
                                                              , fields
                                                              , string.IsNullOrEmpty(orderBy) ? primaryColumnName : orderBy
                                                              , pageIndex * pageSize
                                                              , pageSize * (pageIndex - 1));
            return sql;
        }
    }
}
