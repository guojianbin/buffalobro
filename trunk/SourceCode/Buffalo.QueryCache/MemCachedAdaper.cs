using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;
using System.Data;
using System.Net;
using Memcached.ClientLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CacheManager;

namespace Buffalo.QueryCache
{
    /// <summary>
    /// 利用MemCached做缓存
    /// </summary>
    public class MemCachedAdaper : NetCacheBase<MemcachedClient>
    {

        SockIOPool _pool = null;

        
        
        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public MemCachedAdaper(string connStr, DBInfo info) 
        {
            _info = info;
            _pool = CreatePool(connStr);
            
        }


        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private SockIOPool CreatePool(string connStr) 
        {
            string localserver = "127.0.0.1:11211";
            //uint port = 11211;
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
            string[] serviers =lstServers.ToArray();

            SockIOPool pool = SockIOPool.GetInstance(_info.Name);
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






        protected override MemcachedClient CreateClient(bool readOnly, string cmd)
        {
            MemcachedClient client = new MemcachedClient(_pool);
            client.PrimitiveAsString = true;
            return client;
        }

        protected override E GetValue<E>(string key, MemcachedClient client)
        {
            object value = client.Get(key);
            if (value == null)
            {
                return default(E);
            }
            Type curType = typeof(E);

            return (E)Convert.ChangeType(value, curType);
        }

        protected override void SetValue<E>(string key, E value, MemcachedClient client)
        {
            client.Set(key, value, _expiration);
        }

        protected override DataSet DoGetDataSet(string key, MemcachedClient client)
        {
            return client.GetDataSet(key);
        }

        protected override bool DoSetDataSet(string key, DataSet value, MemcachedClient client)
        {
            return client.SetDataSet(key, value, _expiration);
        }

        protected override void DeleteValue(string key, MemcachedClient client)
        {
            client.Delete(key);
        }

        protected override void DoIncrement(string key, MemcachedClient client)
        {
            client.Increment(key);
        }

        protected override string GetCacheName()
        {
            return "Memcached";
        }
    }
}
