using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buffalo.Kernel;
using System.Data;
using System.Net;
using Memcached.ClientLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using ServiceStack.Redis;
using MemcacheClient;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CacheManager
{
    public class RedisAdaperByServiceStack : NetCacheBase<IRedisClient>
    {
        PooledRedisClientManager _pool =null;
        
        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public RedisAdaperByServiceStack(string connStr, DBInfo info) 
        {
            _info = info;
            _pool = CreateManager(connStr);
            
        }

        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        private  PooledRedisClientManager CreateManager(string connectionString)
        {
            string localserver = "127.0.0.1:6379";
            //uint port = 6379;
            int maxSize = 10;
            string[] conStrs = connectionString.Split(';');
            string serverString = "server=";
            string readonlyserverString = "roserver=";
            string sizeString = "maxsize=";
            string expirString = "expir=";
            string throwString = "throw=";
            string part = null;
            List<string> lstServers = new List<string>();
            List<string> lstRoServers = new List<string>();
            foreach (string lpart in conStrs)
            {
                part = lpart.Trim();
                if (part.IndexOf(serverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string serverStr = part.Substring(serverString.Length);
                    //string[] parts = serverStr.Split(':');
                    //if (parts.Length > 0)
                    //{
                    //    ip = parts[0].Trim();

                    //}
                    //if (parts.Length > 1)
                    //{
                    //    if (!uint.TryParse(parts[1].Trim(), out port))
                    //    {
                    //        throw new ArgumentException(parts[1].Trim() + "不是正确的端口号");
                    //    }
                    //}
                    string[] parts = serverStr.Split(',');
                    foreach (string sser in parts)
                    {
                        if (!string.IsNullOrEmpty(sser))
                        {
                            lstServers.Add(sser);
                        }
                    }
                }
                if (part.IndexOf(readonlyserverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string serverStr = part.Substring(serverString.Length);
                    //string[] parts = serverStr.Split(':');
                    //if (parts.Length > 0)
                    //{
                    //    ip = parts[0].Trim();

                    //}
                    //if (parts.Length > 1)
                    //{
                    //    if (!uint.TryParse(parts[1].Trim(), out port))
                    //    {
                    //        throw new ArgumentException(parts[1].Trim() + "不是正确的端口号");
                    //    }
                    //}
                    string[] parts = serverStr.Split(',');
                    foreach (string sser in parts)
                    {
                        if (!string.IsNullOrEmpty(sser))
                        {
                            lstRoServers.Add(sser);
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

            string[] roserviers = null;
            if (lstRoServers.Count > 0)
            {
                roserviers = lstRoServers.ToArray();
            }
            else 
            {
                roserviers = serviers;
            }
            //string[] serviers ={ip+":"+port };

            //支持读写分离，均衡负载
            return new PooledRedisClientManager(serviers, roserviers, new RedisClientManagerConfig
            {
                MaxWritePoolSize = maxSize,//“写”链接池链接数
                MaxReadPoolSize = maxSize,//“写”链接池链接数
                AutoStart = true,
            });
        }


        #region ICacheAdaper 成员


        protected override IRedisClient CreateClient(bool realOnly,string cmd)
        {
            IRedisClient client =null;
            if (realOnly)
            {
                client = _pool.GetReadOnlyClient();
            }
            else 
            {
                client = _pool.GetClient();
            }
            return client;
        }

        protected override E GetValue<E>(string key,  IRedisClient client)
        {
            return client.Get<E>(key);
        }

        protected override void SetValue<E>(string key, E value, IRedisClient client)
        {
            client.Set(key, value, _expiration);
        }

        protected override DataSet DoGetDataSet(string key, IRedisClient client)
        {
            byte[] content = client.Get<byte[]>(key);
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadDataSet(stm);
            }
        }

        protected override bool DoSetDataSet(string key, DataSet value, IRedisClient client)
        {
            byte[] bval = MemDataSerialize.DataSetToBytes(value);
            client.Set<byte[]>(key, bval, _expiration);
            return true;
        }

        protected override void DeleteValue(string key, IRedisClient client)
        {
            client.Remove(key);
        }

        protected override void DoIncrement(string key, IRedisClient client)
        {
            client.IncrementValue(key);
        }

        protected override string GetCacheName()
        {
            return "Redis";
        }

        #endregion


    }
}
