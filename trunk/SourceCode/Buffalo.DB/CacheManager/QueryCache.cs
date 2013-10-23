using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 内存缓存类
    /// </summary>
    public class QueryCache
    {
        

        /// <summary>
        /// 生成表名对应的Key
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        internal static string GetTableKeyName(string tableName) 
        {
            StringBuilder sbRet = new StringBuilder(tableName.Length + 20);
            sbRet.Append("___Table:");
            sbRet.Append(tableName);
            return sbRet.ToString();
        }



        /// <summary>
        /// 根据表名清除对应的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public void ClearCache(string tableName)
        {
            //string fullName = entityType.FullName;
            //_cache.Remove(GetKeyByEntity(entityName));
            //List<string> lstViewName = QueryViewConfig.GetViewList(fullName);
            //if (lstViewName != null)//清空对应视图的缓存 
            //{
            //    foreach (string viewName in lstViewName)
            //    {
            //        _cache.Remove(GetKeyByEntity(viewName));
            //    }
            //}
        }
    }
}
