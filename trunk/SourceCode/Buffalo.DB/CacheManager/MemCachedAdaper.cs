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

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 利用MemCached做缓存
    /// </summary>
    public class MemCachedAdaper : ICacheAdaper
    {

        SockIOPool _pool = null;

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
            string ip = "127.0.0.1";
            uint port = 11211;
            int maxSize = 10;
            string[] conStrs = connStr.Split(';');
            string serverString = "server=";
            string sizeString = "maxsize=";
            string timeoutString = "timeout=";
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
                else if (part.IndexOf(timeoutString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string timeoutStr = part.Substring(timeoutString.Length);
                    int mins = 30;
                    if (!int.TryParse(timeoutStr, out mins))
                    {
                        throw new ArgumentException("超时分钟数必须是1-9999的值");
                    }
                    if (mins <= 0 || mins >= 9999)
                    {
                        throw new ArgumentException("超时分钟数必须是1-9999的值");
                    }
                    _expiration = TimeSpan.FromMinutes((double)mins);
                }
            }

            string[] serviers ={ip+":"+port };

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


            MemcachedClient client = new MemcachedClient(_pool);
            client.PrimitiveAsString = true;
            string sourceKey = null;
            string key = GetKey(tableNames, sql, client, true,out sourceKey);
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }



            DataSet dsRet = client.GetDataSet(key);
            OutPutMessage(QueryCache.CommandGetDataSet, sourceKey);
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
            MemcachedClient client, bool needCreateTableVer,out string sourceKey)
        {
            List<string> tables = GetSortTables(tableNames);
            StringBuilder sbSql = new StringBuilder(tables.Count * 10 + 200);
            sourceKey = "";
            foreach (string tableName in tables)
            {
                string key = GetTableName(tableName);
                object objVer = client.Get(key);
                if (objVer == null)
                {
                    if (!needCreateTableVer)
                    {
                        return null;
                    }
                    else 
                    {
                        client.SetValue(key, 1, _expiration);
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
            sbSql.Append(";");
            sbSql.Append(sbSql.Length.ToString());
            StringBuilder sbRet = new StringBuilder();

            sourceKey = sbSql.ToString();
            sbRet.Append(PasswordHash.ToMD5String(sourceKey));
            return sbRet.ToString();
        }

        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql)
        {
            MemcachedClient client = new MemcachedClient(_pool);
            
            client.PrimitiveAsString = true;
            string sourceKey = null;
            string key = GetKey(tableNames, sql, client, false, out sourceKey);
            if (!string.IsNullOrEmpty(key))
            {
                client.Delete(key);
            }
            OutPutMessage(QueryCache.CommandDeleteSQL, sql);

        }
        /// <summary>
        /// 最大版本号
        /// </summary>
        private const int MaxVersion = (int.MaxValue-1000) ;
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
            MemcachedClient client = new MemcachedClient(_pool);
            client.PrimitiveAsString = true;
            object oval = client.Get(key);
            int val = 0;
            try
            {
                val = Convert.ToInt32(oval);
            }
            catch { }
            if (val <= 0 || val >= MaxVersion)
            {
                client.Set(key, 1, _expiration);
            }
            else
            {
                client.Increment(key, 1);
            }
            OutPutMessage(QueryCache.CommandDeleteTable, tableName);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public  bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds)
        {
            MemcachedClient client = new MemcachedClient(_pool);
            client.PrimitiveAsString = true;
            string sourceKey = null;
            string key = GetKey(tableNames, sql, client, true,out sourceKey);
            OutPutMessage(QueryCache.CommandSetDataSet, sourceKey);
            return client.SetDataSet(key, ds, _expiration);
        }

        private void OutPutMessage(string type,string message) 
        {
            if (_info.SqlOutputer.HasOutput)
            {
                _info.SqlOutputer.DefaultOutputer.Output("Memcached", type, new string[] { message });
            }
        }

        #endregion

    }
}
