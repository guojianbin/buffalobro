using System;
using System.Collections.Generic;
using System.Text;
using Enyim.Caching;
using MySql.Data.MySqlClient.Memcached;
using Buffalo.DB.CacheManager.Memcached;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 利用MemCached做缓存
    /// </summary>
    public class MemCachedAdaper : ICacheAdaper
    {
        private MemCachedClient _client = null;

        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public MemCachedAdaper(string connStr) 
        {
            string conStrs=connStr.Split(':');

            string ip = "127.0.0.1";
            uint port = 11211;

            if (connStr.Length > 0) 
            {
                ip = connStr[0];
            }
            if (connStr.Length > 1) 
            {
                port = Convert.ToUInt32(connStr[1]);
            }
            _client = new MemCachedClient(ip, port);
        }

        #region ICacheAdaper 成员

        public System.Data.DataSet GetData(string sql)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveBySQL(string sql)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveByTableName(string tableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool SetData(ICollection<string> tableNames, string sql, System.Data.DataSet ds)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
