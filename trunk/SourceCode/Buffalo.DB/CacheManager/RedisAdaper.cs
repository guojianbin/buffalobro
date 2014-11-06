using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient.Memcached;

using Buffalo.Kernel;
using System.Data;
using System.Net;
using Memcached.ClientLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using Redissharp;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 利用MemCached做缓存
    /// </summary>
    public class RedisAdaper : NetCacheBase<Redis>
    {

        SockPool _pool = null;
        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public RedisAdaper(string connStr, DBInfo info) 
        {
            _info = info;
            _pool = CreatePool(connStr);
            
        }


        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private SockPool CreatePool(string connStr) 
        {
            string localserver = "127.0.0.1:6379";
            //uint port = 6379;
            int maxSize = 10;
            string[] conStrs = connStr.Split(';');
            string serverString = "server=";
            string sizeString = "maxsize=";
            string expirString = "expir=";
            string throwString = "throw=";
            string part = null;
            List<string> lstServers = new List<string>();
            foreach (string lpart in conStrs)
            {
                part = lpart.Trim();
                if (part.IndexOf(serverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string serverStr = part.Substring(serverString.Length);
                    string[] parts = serverStr.Split(',');
                    foreach (string sser in parts)
                    {
                        if (!string.IsNullOrEmpty(sser))
                        {
                            lstServers.Add(sser);
                        }
                    }
                }
                else if (part.IndexOf(sizeString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string maxsizeStr = part.Substring(sizeString.Length);
                    if (!int.TryParse(maxsizeStr, out maxSize))
                    {
                        throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
                    }
                    if (maxSize <= 0 || maxSize >= int.MaxValue)
                    {
                        throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
                    }
                }
                else if (part.IndexOf(throwString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string throwStr = part.Substring(throwString.Length);
                    _throwExcertion = (throwStr == "1");
                }
                else if (part.IndexOf(expirString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string expirStr = part.Substring(expirString.Length);
                    int mins = 30;
                    if (!int.TryParse(expirStr, out mins))
                    {
                        throw new ArgumentException("数据保存分钟数必须是1-999999999的值");
                    }
                    if (mins <= 0 || mins >= 999999999)
                    {
                        throw new ArgumentException("数据保存分钟数必须是1-999999999的值");
                    }
                    _expiration = TimeSpan.FromMinutes((double)mins);
                }
            }
            if (lstServers.Count == 0)
            {
                lstServers.Add(localserver);
            }
            string[] serviers = lstServers.ToArray();

            SockPool pool = SockPool.GetInstance(_info.Name);
            pool.SetServers(serviers);
            pool.InitConnections = 1;
            pool.MinConnections = 1;
            pool.MaxConnections = maxSize;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            
            pool.Initialize();
            
            return pool;
        }

        

        #region ICacheAdaper 成员

        protected override Redis CreateClient(bool realOnly, string cmd)
        {
            Redis client = new Redis(_pool);
            client.Open(cmd);
            return client;
        }

        protected override E GetValue<E>(string key, Redis client)
        {
            string value=client.GetString(key);
            if (value == null) 
            {
                return default(E);
            }
            Type curType = typeof(E);
            
            return (E)Convert.ChangeType(value, curType);
        }

        protected override void SetValue<E>(string key, E value, Redis client)
        {
            client.SetValue(key, value);
            client.Expire(key, (int)_expiration.TotalSeconds);
        }

        protected override DataSet DoGetDataSet(string key, Redis client)
        {
            return client.GetDataSet(key);
        }

        protected override bool DoSetDataSet(string key, DataSet value, Redis client)
        {
            client.SetDataSet(key, value);
            return true;
        }

        protected override void DeleteValue(string key, Redis client)
        {
            client.Remove(key);
        }

        protected override void DoIncrement(string key, Redis client)
        {
            client.Increment(key);
        }

        protected override string GetCacheName()
        {
            return "Redis";
        }

        #endregion

    }
}
