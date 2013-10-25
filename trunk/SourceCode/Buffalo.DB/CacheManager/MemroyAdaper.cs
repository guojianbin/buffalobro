using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Data;
using System.Collections;
using Buffalo.Kernel;

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
        private Dictionary<string, List<string>> _dicTableToKey = new Dictionary<string, List<string>>();
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sql">SQL名</param>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        public bool SetData(ICollection<string> tableNames, string sql, DataSet ds) 
        {
            string key = GetKey(sql);
            List<string> sqlItems = null;
            //添加表对应的SQL语句值
            foreach (string tableName in tableNames) 
            {
                if (!_dicTableToKey.TryGetValue(tableName, out sqlItems)) 
                {
                    using (Lock objLock = new Lock(_dicTableToKey))
                    {
                        sqlItems = new List<string>();
                        _dicTableToKey[tableName] = sqlItems;
                    }
                }
                using (Lock objLock = new Lock(sqlItems))
                {
                    sqlItems.Add(key);
                }
            }
            _cache[key] = ds;
            return true;
        }

        /// <summary>
        /// 通过SQL获取键
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string GetKey(string sql) 
        {
            StringBuilder sbKey = new StringBuilder(100);
            sbKey.Append("BuffaloCache:");
            sbKey.Append(sql.Length );
            sbKey.Append(":");
            sbKey.Append(PasswordHash.ToSHA1String(sql));
            return sbKey.ToString();
        }

        /// <summary>
        /// 根据SQL语句从缓存中找出数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">表名集合</param>
        /// <returns></returns>
        public DataSet GetData( string sql)
        {
            string key = GetKey(sql);
            return hs[key] as DataSet;
        }

        /// <summary>
        /// 通过SQL删除某项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL( string sql) 
        {
            string key = GetKey(sql);
            hs.Remove(key);
        }
        /// <summary>
        /// 通过表名删除关联项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveByTableName(string tableName)
        {
            List<string> sqlItems = null;
            if (_dicTableToKey.TryGetValue(tableName, out sqlItems))
            {
                if (sqlItems == null)
                {
                    return;
                }
                using (Lock objLock = new Lock(sqlItems))
                {
                    foreach (string key in sqlItems)
                    {
                        _cache.Remove(key);
                    }
                    sqlItems.Clear();
                }
            }
        }
    }

    
}
