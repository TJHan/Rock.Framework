using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Data.DataAccess
{
    public static class DBHelper
    {
        /// <summary>
        /// 获取属性对应的表字段名称
        /// </summary>
        /// <param name="property">对象属性</param>
        /// <returns></returns>
        public static string GetTableColumnName(PropertyInfo property)
        {
            if (property == null)
                return string.Empty;

            string columnName = property.Name;
            Attributes.EntityMappingAttribute attrList = property.GetCustomAttribute<Attributes.EntityMappingAttribute>(true);
            if (attrList != null)
            {
                columnName = attrList.ColumnName;
            }
            return columnName;
        }

        /// <summary>
        /// 获取对象对应的表名
        /// </summary>
        /// <param name="tableType">对象的类型对象</param>
        /// <returns></returns>
        public static string GetTableName(Type tableType)
        {
            string tableName = tableType.Name;
            Attributes.EntityMappingAttribute attr = tableType.GetCustomAttribute<Attributes.EntityMappingAttribute>(true);
            if (attr != null && !string.IsNullOrEmpty(attr.TableName))
                tableName = attr.TableName;
            return tableName;
        }
    }
}
