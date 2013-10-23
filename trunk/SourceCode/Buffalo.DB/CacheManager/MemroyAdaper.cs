using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Data;
using System.Collections;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 系统内存的缓存适配器
    /// </summary>
    public class MemroyAdaper : ICacheAdaper
    {
        public MemroyAdaper() 
        {
            
        }

        private Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sql">SQL名</param>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        public bool SetData(ICollection<string> tableNames, string sql, DataSet ds) 
        {
            string key = QueryCache.GetTableKeyName(tableName);
            Hashtable hs = _cache[key] as Hashtable;
            if (hs == null)
            {
                hs = new Hashtable();
                _cache[key] = hs;
            }
            hs[sql] = ds;
            return true;
        }
        /// <summary>
        /// 根据SQL语句从缓存中找出数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="entityType">表对应的实体的类型</param>
        /// <returns></returns>
        public DataSet GetData(string tableName, string sql)
        {
            Hashtable hs = _cache[QueryCache.GetTableKeyName(tableName)] as Hashtable;
            if (hs == null)
            {
                return null;
            }
            return hs[sql] as DataSet;
        }

        /// <summary>
        /// 通过SQL删除某项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL(string tableName,string sql) 
        {
            Hashtable hs = _cache[QueryCache.GetTableKeyName(tableName)] as Hashtable;
            if (hs == null)
            {
                return;
            }
            hs.Remove(sql);
        }
        /// <summary>
        /// 通过表名删除关联项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveByTableName(string tableName)
        {
            _cache.Remove(tableName);
        }
    }
}
