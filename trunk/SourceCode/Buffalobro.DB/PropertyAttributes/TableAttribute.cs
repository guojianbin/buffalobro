using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Buffalobro.DB.PropertyAttributes
{
    public class TableAttribute : System.Attribute
    {
        private string tableName;
        private bool isParamNameUpper;
        /// <summary>
        /// 类标示
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionKey">连接字符串的键</param>
        /// <param name="isParamNameUpper">字段名是否转成大写</param>
        public TableAttribute(string tableName, bool isParamNameUpper)
        {
            this.tableName = tableName;
            this.isParamNameUpper = isParamNameUpper;
        }
        /// <summary>
        /// 类标示
        /// </summary>
        /// <param name="tableName">表名</param>
        public TableAttribute(string tableName):this(tableName,false)
        {

        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName 
        {
            get 
            {
                return tableName;
            }
        }
        /// <summary>
        /// 字段名是否转成大写
        /// </summary>
        public bool IsParamNameUpper
        {
            get
            {
                return isParamNameUpper;
            }
        }

    }
}
