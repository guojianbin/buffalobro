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
        private Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// 获取Sql的Key
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string GetKeyBySql(string sql) 
        {
            StringBuilder sbRet = new StringBuilder(sql.Length + 20);
            sbRet.Append("___SQL:");
            sbRet.Append(sql);
            return sbRet.ToString();
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private string GetKeyByEntity(string tableName) 
        {
            StringBuilder sbRet = new StringBuilder(tableName.Length + 20);
            sbRet.Append("___Entity:");
            sbRet.Append(tableName);
            return sbRet.ToString();
        }

        /// <summary>
        /// 根据SQL语句从缓存中找出数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="entityType">表对应的实体的类型</param>
        /// <returns></returns>
        public object GetData(string tableName, string sql)
        {
            Hashtable hs = _cache[GetKeyByEntity(tableName)] as Hashtable;
            if (hs == null) 
            {
                return null;
            }
            return hs[GetKeyBySql(sql)];
        }
        /// <summary>
        /// 把数据加进缓存
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="entityType">表对应的实体的类型</param>
        /// <param name="val">数据</param>
        /// <returns></returns>
        public void AppendData(string sql, string tableName, object val)
        {
            string key = GetKeyByEntity(tableName);
            Hashtable hs = _cache[key] as Hashtable;
            if (hs == null) 
            {
                hs = new Hashtable();
                _cache[key] = hs;
            }
            hs[sql] = val;


        }
        /// <summary>
        /// 根据实体清除对应的缓存
        /// </summary>
        /// <param name="entityType"></param>
        public void ClearCache(string tableName)
        {
            //string fullName = entityType.FullName;
            _cache.Remove(GetKeyByEntity(fullName));
            List<string> lstViewName = QueryViewConfig.GetViewList(fullName);
            if (lstViewName != null)//清空对应视图的缓存 
            {
                foreach (string viewName in lstViewName)
                {
                    _cache.Remove(GetKeyByEntity(viewName));
                }
            }
        }
    }
}
