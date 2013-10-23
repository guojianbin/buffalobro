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
        public MemCachedAdaper(string config) 
        {
            string[] strs=config.Split(':');
            string ip=null;
            uint port=0;
            if (strs.Length > 0)
            {
                ip = strs[0];
            }
            if (strs.Length > 1) 
            {
                string strPort = strs[1];
                uint.TryParse(strPort,out port);
            }
            if(port<=0)
            {
                port=11211;
            }

            _client = new MemCachedClient(ip, port);
        }


        #region ICacheAdaper 成员

        public System.Data.DataSet GetData(string tableName, string sql)
        {
            string key=QueryCache.GetTableKeyName(tableName);
            int value = Convert.ToInt32(_client.GetValue(key));
            StringBuilder skey=new StringBuilder();
            skey.Append("v");
            skey.Append(value.ToString());
            skey.Append(":");
            skey.Append(sql);
            string xml=_client
        }

        public void RemoveBySQL(string tableName, string sql)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveByTableName(string tableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool SetData(string tableName, string sql, System.Data.DataSet ds)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
