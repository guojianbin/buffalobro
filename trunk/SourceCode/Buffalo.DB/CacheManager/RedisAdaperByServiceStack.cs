﻿using System;
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
            string localserver = "127.0.0.1:6379";
            //uint port = 6379;
            int maxSize = 10;
            string[] conStrs = connectionString.Split(';');
            string serverString = "server=";
            string readonlyserverString = "roserver=";
            string sizeString = "maxsize=";
            string expirString = "expir=";
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


        public System.Data.DataSet GetData(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            DataSet dsRet = null;
            using (IRedisClient client = _pool.GetReadOnlyClient())
            {
                string sqlMD5 = GetSQLMD5(sql);
                bool isVersion = ComparVersion(tableNames, sqlMD5, client);//判断版本号
                if (!isVersion)
                {
                    return null;
                }
                byte[] content = client.Get<byte[]>(sqlMD5);
                using (MemoryStream stm = new MemoryStream(content))
                {
                    dsRet = MemDataSerialize.LoadDataSet(stm);
                }
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandGetDataSet, sql, oper);
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
        /// 获取SQL语句的键
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="client">创建器</param>
        /// <returns></returns>
        private string GetSQLMD5(string sql)
        {
            StringBuilder sbSql = new StringBuilder(256);
            StringBuilder sbSqlInfo = new StringBuilder();
            sbSqlInfo.Append(_info.Name);
            sbSqlInfo.Append(":");
            sbSqlInfo.Append(sql);
            sbSql.Append(PasswordHash.ToMD5String(sbSqlInfo.ToString()));
            return sbSql.ToString();
        }
        /// <summary>
        /// 获取版本号的键
        /// </summary>
        /// <param name="md5">哈希值</param>
        /// <returns></returns>
        private string FormatVersionKey(string md5)
        {
            return "v." + md5;
        }
        /// <summary>
        /// 对比版本
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="md5">sql语句的MD5</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        private bool ComparVersion(IDictionary<string, bool> tableNames, string md5, IRedisClient client)
        {
            Dictionary<string, string> dicTableVers = GetTablesVersion(tableNames, client, false);
            if (dicTableVers == null)
            {
                return false;
            }
            Dictionary<string, string> dicDataVers = GetDataVersion(md5, client);
            if (dicDataVers == null)
            {
                return false;
            }
            string tmp = null;
            foreach (KeyValuePair<string, string> kvp in dicTableVers)
            {
                if (!dicDataVers.TryGetValue(kvp.Key, out tmp))
                {
                    return false;
                }
                if (tmp != kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取当前库中所有表的版本号
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="client">Redis连接</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <returns></returns>
        private Dictionary<string, string> GetTablesVersion(IDictionary<string, bool> tableNames, IRedisClient client, bool needCreateTableVer)
        {
            Dictionary<string, string> dicTableVers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (KeyValuePair<string, bool> kvp in tableNames)
            {
                string key = GetTableName(kvp.Key);
                string objVer = client.GetValue(key);
                if (objVer == null)
                {
                    if (!needCreateTableVer)
                    {
                        return null;
                    }
                    else
                    {
                        client.Set<int>(key, 1, _expiration);
                        objVer = "1";
                    }
                }
                dicTableVers[kvp.Key] = objVer;

            }
            return dicTableVers;
        }
        /// <summary>
        /// 获取当前库中表的版本号字符串
        /// </summary>
        /// <param name="tableNames">表名集合</param>
        /// <param name="client">Redis连接</param>
        /// <param name="needCreateTableVer">是否需要创建表的键</param>
        /// <returns></returns>
        private string GetTablesVerString(IDictionary<string, bool> tableNames, IRedisClient client, bool needCreateTableVer)
        {
            Dictionary<string, string> dicTableVers = GetTablesVersion(tableNames, client, needCreateTableVer);
            StringBuilder sbTables = new StringBuilder(dicTableVers.Count * 10);
            foreach (KeyValuePair<string, string> kvp in dicTableVers)
            {
                sbTables.Append(kvp.Key);
                sbTables.Append("=");
                sbTables.Append(kvp.Value);
                sbTables.Append("\n");
            }
            if (sbTables.Length > 0)
            {
                sbTables.Remove(sbTables.Length - 1, 1);
            }
            return sbTables.ToString();
        }
        /// <summary>
        /// 获取当前查询的版本号
        /// </summary>
        /// <param name="md5">SQL的md5</param>
        /// <param name="client">Redis连接</param>
        /// <returns></returns>
        private Dictionary<string, string> GetDataVersion(string md5, IRedisClient client)
        {
            string key = FormatVersionKey(md5);
            string vers = client.Get<string>(key);
            Dictionary<string, string> dicDataVers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            if (CommonMethods.IsNullOrWhiteSpace(vers))
            {
                return null;
            }
            string[] verItems = vers.Split('\n');
            foreach (string verItem in verItems)
            {
                if (string.IsNullOrEmpty(verItem))
                {
                    continue;
                }
                string[] part = verItem.Split('=');
                if (part.Length < 2)
                {
                    continue;
                }

                dicDataVers[part[0]] = part[1];
            }
            return dicDataVers;
        }



        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            using (IRedisClient client = _pool.GetClient())
            {
                string md5 = GetSQLMD5(sql);
                string verKey = FormatVersionKey(md5);
                if (!string.IsNullOrEmpty(md5))
                {
                    client.Remove(md5);
                    client.Remove(verKey);
                }
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandDeleteSQL, sql,oper);
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
        public void RemoveByTableName(string tableName, DataBaseOperate oper)
        {
            string key = GetTableName(tableName);
            using (IRedisClient client = _pool.GetClient())
            {

                object oval = client.GetValue(key);
                int val = 0;
                try
                {
                    val = Convert.ToInt32(oval);
                }
                catch { }
                if (val <= 0 || val >= MaxVersion)
                {
                    client.Set<int>(key, 1, _expiration);
                }
                else
                {
                    client.IncrementValue(key);
                }
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandDeleteTable, tableName,oper);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, DataBaseOperate oper)
        {
            using (IRedisClient client = _pool.GetClient())
            {

                string md5 = GetSQLMD5(sql);
                string verKey = FormatVersionKey(md5);
                string verValue = GetTablesVerString(tableNames, client, true);

                if (_info.SqlOutputer.HasOutput)
                {
                    OutPutMessage(QueryCache.CommandSetDataSet, sql, oper);
                }
                

                byte[] bval = MemDataSerialize.DataSetToBytes(ds);
                client.Set<string>(verKey, verValue);
                client.Set<byte[]>(md5, bval, _expiration);
            }
            return true;
        }

        private void OutPutMessage(string type, string message, DataBaseOperate oper)
        {

            oper.OutMessage(MessageType.QueryCache, "Redis", type, message);

        }

        #endregion


    }
}
