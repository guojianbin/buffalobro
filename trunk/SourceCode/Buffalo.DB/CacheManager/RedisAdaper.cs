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

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ����MemCached������
    /// </summary>
    public class RedisAdaper : ICacheAdaper
    {

        SockPool _pool = null;

        /// <summary>
        /// ����ʱ��(����)
        /// </summary>
        TimeSpan _expiration;
        /// <summary>
        /// ����ʱ��(����)
        /// </summary>
        public TimeSpan Expiration
        {
            get { return _expiration; }
        }

        private DBInfo _info;
        /// <summary>
        /// ���ݿ���Ϣ
        /// </summary>
        public DBInfo Info
        {
            get { return _info; }
        }
        /// <summary>
        /// memcached��������
        /// </summary>
        /// <param name="connStr">�����ַ���</param>
        public RedisAdaper(string connStr, DBInfo info) 
        {
            _info = info;
            _pool = CreatePool(connStr);
            
        }


        /// <summary>
        /// �������ӳ�
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private SockPool CreatePool(string connStr) 
        {
            string ip = "127.0.0.1";
            uint port = 6379;
            int maxSize = 10;
            string[] conStrs = connStr.Split(';');
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
                            throw new ArgumentException(parts[1].Trim() + "������ȷ�Ķ˿ں�");
                        }
                    }
                }
                else if (part.IndexOf(sizeString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string maxsizeStr = part.Substring(sizeString.Length);
                    if (!int.TryParse(maxsizeStr, out maxSize))
                    {
                        throw new ArgumentException("���������������1-" + MaxVersion + "��ֵ");
                    }
                    if (maxSize <= 0 || maxSize >= int.MaxValue)
                    {
                        throw new ArgumentException("���������������1-" + MaxVersion + "��ֵ");
                    }
                }
                else if (part.IndexOf(expirString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string expirStr = part.Substring(expirString.Length);
                    int mins = 30;
                    if (!int.TryParse(expirStr, out mins))
                    {
                        throw new ArgumentException("���ݱ��������������1-999999999��ֵ");
                    }
                    if (mins <= 0 || mins >= 999999999)
                    {
                        throw new ArgumentException("���ݱ��������������1-999999999��ֵ");
                    }
                    _expiration = TimeSpan.FromMinutes((double)mins);
                }
            }
            if (ip == "localhost") 
            {
                ip = "127.0.0.1";
            }
            string[] serviers ={ip+":"+port };

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

        

        #region ICacheAdaper ��Ա

        /// <summary>
        /// �ѱ������ϻ���������ļ���
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
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandGetDataSet);
                string key = GetKey(tableNames, sql, client, true, out sourceKey);
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }
                dsRet = client.GetDataSet(key);
            }
            OutPutMessage(QueryCache.CommandGetDataSet, sourceKey);
            return dsRet;


        }

        /// <summary>
        /// ��ȡ����
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
        /// ����SQL�ͱ��ȡ��
        /// </summary>
        /// <param name="tableNames">����</param>
        /// <param name="sql">SQL</param>
        /// <param name="client">������Ϣ</param>
        /// <param name="needCreateTableVer">�Ƿ���Ҫ������ļ�</param>
        /// <param name="sourceKey">Դ��</param>
        /// <returns></returns>
        private string GetKey(IDictionary<string, bool> tableNames, string sql,
            Redis client, bool needCreateTableVer,out string sourceKey)
        {
            List<string> tables = GetSortTables(tableNames);
            StringBuilder sbSql = new StringBuilder(tables.Count * 10 + 200);
            sourceKey = "";
            foreach (string tableName in tables)
            {
                string key = GetTableName(tableName);
                string objVer = client.GetString(key);
                if (objVer == null)
                {
                    if (!needCreateTableVer)
                    {
                        return null;
                    }
                    else 
                    {
                        client.SetValue(key, 1);
                        client.Expire(key, (int)_expiration.TotalSeconds);
                        objVer = "1";
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
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandDeleteSQL);
                string sourceKey = null;
                string key = GetKey(tableNames, sql, client, false, out sourceKey);
                if (!string.IsNullOrEmpty(key))
                {
                    client.Remove(key);
                }
            }
            OutPutMessage(QueryCache.CommandDeleteSQL, sql);

        }
        /// <summary>
        /// ���汾��
        /// </summary>
        private const int MaxVersion = (int.MaxValue-1000) ;
        /// <summary>
        /// ���ݱ���ɾ������
        /// </summary>
        /// <param name="tableName"></param>
        public void RemoveByTableName(string tableName)
        {
            //if (client.GetValue(tableName) == null) 
            //{
            //    //_client.Set(tableName,1,
            //}
            string key = GetTableName(tableName);
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandDeleteSQL);

                object oval =  client.GetString(key);
                int val = 0;
                try
                {
                    val = Convert.ToInt32(oval);
                }
                catch { }
                if (val <= 0 || val >= MaxVersion)
                {
                    client.SetValue(key, 1);
                    client.Expire(key, (int)_expiration.TotalSeconds);
                }
                else
                {
                    client.Increment(key);
                }
            }
            OutPutMessage(QueryCache.CommandDeleteTable, tableName);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public  bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds)
        {
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandSetDataSet);

                string sourceKey = null;
                string key = GetKey(tableNames, sql, client, true, out sourceKey);
                OutPutMessage(QueryCache.CommandSetDataSet, sourceKey);
                client.SetDataSet(key, ds);
                client.Expire(key, (int)_expiration.TotalSeconds);
            }
            return true;
        }

        private void OutPutMessage(string type,string message) 
        {

                _info.OutMessage(MessageType.QueryCache, "Redis", type, message);
            
        }

        #endregion

    }
}
