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
            string localserver = "127.0.0.1:6379";
            //uint port = 6379;
            int maxSize = 10;
            string[] conStrs = connStr.Split(';');
            string serverString = "server=";
            string sizeString = "maxsize=";
            string expirString = "expir=";
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

        public System.Data.DataSet GetData(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            DataSet dsRet = null;
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandGetDataSet);
                string sqlMD5 = GetSQLMD5(sql);
                bool isVersion = ComparVersion(tableNames, sqlMD5, client);//�жϰ汾��
                if (!isVersion)
                {
                    return null;
                }
                dsRet = client.GetDataSet(sqlMD5);
            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandGetDataSet, sql,oper);
            }
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
        /// ��ȡSQL���ļ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="client">������</param>
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
        /// ��ȡ�汾�ŵļ�
        /// </summary>
        /// <param name="md5">��ϣֵ</param>
        /// <returns></returns>
        private string FormatVersionKey(string md5) 
        {
            return "v." + md5;
        }
        /// <summary>
        /// �ԱȰ汾
        /// </summary>
        /// <param name="tableNames">��������</param>
        /// <param name="md5">sql����MD5</param>
        /// <param name="client">�ͻ���</param>
        /// <returns></returns>
        private bool ComparVersion(IDictionary<string, bool> tableNames, string md5, Redis client) 
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
            string tmp=null;
            foreach (KeyValuePair<string, string> kvp in dicTableVers) 
            {
                if (!dicDataVers.TryGetValue(kvp.Key,out tmp) )
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
        /// ��ȡ��ǰ�������б�İ汾��
        /// </summary>
        /// <param name="tableNames">��������</param>
        /// <param name="client">Redis����</param>
        /// <param name="needCreateTableVer">�Ƿ���Ҫ������ļ�</param>
        /// <returns></returns>
        private Dictionary<string, string> GetTablesVersion(IDictionary<string, bool> tableNames, Redis client, bool needCreateTableVer) 
        {
            Dictionary<string, string> dicTableVers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            
            foreach (KeyValuePair<string, bool> kvp in tableNames)
            {
                string key = GetTableName(kvp.Key);
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
                dicTableVers[kvp.Key] = objVer;

            }
            return dicTableVers;
        }
        /// <summary>
        /// ��ȡ��ǰ���б�İ汾���ַ���
        /// </summary>
        /// <param name="tableNames">��������</param>
        /// <param name="client">Redis����</param>
        /// <param name="needCreateTableVer">�Ƿ���Ҫ������ļ�</param>
        /// <returns></returns>
        private string GetTablesVerString(IDictionary<string, bool> tableNames, Redis client, bool needCreateTableVer)
        {
            Dictionary<string, string> dicTableVers = GetTablesVersion(tableNames, client, needCreateTableVer);
            StringBuilder sbTables = new StringBuilder(dicTableVers.Count*10);
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
        /// ��ȡ��ǰ��ѯ�İ汾��
        /// </summary>
        /// <param name="md5">SQL��md5</param>
        /// <param name="client">Redis����</param>
        /// <returns></returns>
        private Dictionary<string, string> GetDataVersion(string md5, Redis client)
        {
            //string md5 = GetSQLKey(sql);
            string key = FormatVersionKey(md5);
            string vers = client.GetString(key);
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
                if (part.Length <2) 
                {
                    continue;
                }
                
                dicDataVers[part[0]] = part[1];
            }
            return dicDataVers;
        }


        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandDeleteSQL);
                string sourceKey = null;
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
        /// ���汾��
        /// </summary>
        private const int MaxVersion = (int.MaxValue-1000) ;
        /// <summary>
        /// ���ݱ���ɾ������
        /// </summary>
        /// <param name="tableName"></param>
        public void RemoveByTableName(string tableName, DataBaseOperate oper)
        {
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
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandDeleteTable, tableName,oper);
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="sql"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, DataBaseOperate oper)
        {
            using (Redis client = new Redis(_pool))
            {
                client.Open(QueryCache.CommandSetDataSet);
                string md5 = GetSQLMD5(sql);
                string verKey = FormatVersionKey(md5);
                string verValue = GetTablesVerString(tableNames, client, true);
                
                if (_info.SqlOutputer.HasOutput)
                {
                    OutPutMessage(QueryCache.CommandSetDataSet, sql,oper);
                }
                client.SetValue(verKey, verValue);
                client.SetDataSet(md5, ds);
                client.Expire(verKey, (int)_expiration.TotalSeconds);
                client.Expire(md5, (int)_expiration.TotalSeconds);
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
