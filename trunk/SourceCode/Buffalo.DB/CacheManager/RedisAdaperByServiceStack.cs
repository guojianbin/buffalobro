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

namespace Buffalo.DB.CacheManager
{
    public class RedisAdaperByServiceStack : ICacheAdaper
    {
        PooledRedisClientManager _pool =null;
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        TimeSpan _expiration;
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        public TimeSpan Expiration
        {
            get { return _expiration; }
        }

        private DBInfo _info;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo Info
        {
            get { return _info; }
        }
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
            string ip = "127.0.0.1";
            uint port = 6379;
            int maxSize = 10;
            string[] conStrs = connectionString.Split(';');
            string serverString = "server=";
            string sizeString = "maxsize=";
            string expirString = "expir=";
            string part = null;
            foreach (string lpart in conStrs)
            {
                part = lpart.Trim();
                if (part.IndexOf(serverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string serverStr = part.Substring(serverString.Length);
                    string[] parts = serverStr.Split(':');
                    if (parts.Length > 0)
                    {
                        ip = parts[0].Trim();

                    }
                    if (parts.Length > 1)
                    {
                        if (!uint.TryParse(parts[1].Trim(), out port))
                        {
                            throw new ArgumentException(parts[1].Trim() + "不是正确的端口号");
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
            if (ip == "localhost") 
            {
                ip = "127.0.0.1";
            }
            string[] serviers ={ip+":"+port };

            //支持读写分离，均衡负载
            return new PooledRedisClientManager(serviers, serviers, new RedisClientManagerConfig
            {
                MaxWritePoolSize = maxSize,//“写”链接池链接数
                MaxReadPoolSize = maxSize,//“写”链接池链接数
                AutoStart = true,
            });
        }


        #region ICacheAdaper 成员

        /// <summary>
        /// 把表名集合换成已排序的集合
        /// </summary>
        /// <param name="dicTables"></param>
        /// <returns></returns>
        internal static List<string> GetSortTables(IDictionary<string, bool> dicTables)
        {
            List<string> ret = new List<string>(dicTables.Count);
            foreach (KeyValuePair<string, bool> kvp in dicTables)
            {
                ret.Add(kvp.Key);
            }
            ret.Sort();
            return ret;
        }

        public System.Data.DataSet GetData(IDictionary<string, bool> tableNames, string sql)
        {
            string sourceKey = null;
            DataSet dsRet = null;
            using (IRedisClient client = _pool.GetClient())
            {
                
                string key = GetKey(tableNames, sql, client, true, out sourceKey);
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }
                byte[] content = client.Get<byte[]>(key);
                if (content == null) 
                {
                    return null;
                }
                using (MemoryStream stm = new MemoryStream(content))
                {
                    dsRet = MemDataSerialize.LoadDataSet(stm);
                }
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandGetDataSet, sourceKey);
            }
            return dsRet;


        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetTableName(string tableName)
        {
            StringBuilder sbInfo = new StringBuilder(tableName.Length + 10);
            sbInfo.Append(_info.Name);
            sbInfo.Append(".");
            sbInfo.Append(tableName);
            return PasswordHash.ToMD5String(sbInfo.ToString());
        }

        /// <summary>
        /// 根据SQL和表获取键
        /// </summary>
        /// <param name="tableNames">表名</param>
        /// <param name="sql">SQL</param>
        /// <param name="client">缓存信息</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <param name="sourceKey">源键</param>
        /// <returns></returns>
        private string GetKey(IDictionary<string, bool> tableNames, string sql,
            IRedisClient client, bool needCreateTableVer, out string sourceKey)
        {
            List<string> tables = GetSortTables(tableNames);
            StringBuilder sbSql = new StringBuilder(tables.Count * 10 + 200);
            sourceKey = "";
            foreach (string tableName in tables)
            {
                string key = GetTableName(tableName);
                int objVer = client.Get<int>(key);
                if (objVer == 0)
                {
                    if (!needCreateTableVer)
                    {
                        return null;
                    }
                    else
                    {
                        client.Set<int>(key, 1, _expiration);
                        objVer = 1;
                    }
                }
                sbSql.Append(tableName);
                sbSql.Append(".");
                sbSql.Append(objVer.ToString());
                sbSql.Append(",");
            }

            if (sbSql.Length > 0)
            {
                sbSql[sbSql.Length - 1] = ':';
            }
            sbSql.Append(sql);
            StringBuilder sbRet = new StringBuilder();

            sourceKey = sbSql.ToString();
            sbRet.Append(PasswordHash.ToMD5String(sourceKey));
            return sbRet.ToString();
        }

        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql)
        {
            using (IRedisClient client = _pool.GetClient())
            {
                string sourceKey = null;
                string key = GetKey(tableNames, sql, client, false, out sourceKey);
                if (!string.IsNullOrEmpty(key))
                {
                    client.Remove(key);
                }
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandDeleteSQL, sql);
            }

        }
        /// <summary>
        /// 最大版本号
        /// </summary>
        private const int MaxVersion = (int.MaxValue - 1000);
        /// <summary>
        /// 根据表名删除缓存
        /// </summary>
        /// <param name="tableName"></param>
        public void RemoveByTableName(string tableName)
        {
            //if (client.GetValue(tableName) == null) 
            //{
            //    //_client.Set(tableName,1,
            //}
            string key = GetTableName(tableName);
            using (IRedisClient client = _pool.GetClient())
            {

                object oval = client.Get<string>(key);
                int val = 0;
                try
                {
                    val = Convert.ToInt32(oval);
                }
                catch { }
                if (val <= 0 || val >= MaxVersion)
                {
                    client.Set<int>(key, 1,_expiration);
                }
                else
                {
                    client.Increment(key,1);
                }
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandDeleteTable, tableName);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds)
        {
            using (IRedisClient client = _pool.GetClient())
            {

                string sourceKey = null;
                string key = GetKey(tableNames, sql, client, true, out sourceKey);
                if (_info.SqlOutputer.HasOutput)
                {
                    OutPutMessage(QueryCache.CommandSetDataSet, sourceKey);
                }
                byte[] bval = MemDataSerialize.DataSetToBytes(ds);

                client.Set<byte[]>(key, bval, _expiration);
            }
            return true;
        }

        private void OutPutMessage(string type, string message)
        {

            _info.OutMessage(MessageType.QueryCache, "Redis", type, message);

        }

        #endregion


    }
}
