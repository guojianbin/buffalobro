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
    /// ����MemCached������
    /// </summary>
    public class MemCachedAdaper : ICacheAdaper
    {

        SockIOPool _pool = null;

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
        public MemCachedAdaper(string connStr, DBInfo info) 
        {
            _pool = CreatePool(connStr);
            _info = info;
        }



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
                else if (part.IndexOf(timeoutString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string timeoutStr = part.Substring(timeoutString.Length);
                    int mins = 30;
                    if (!int.TryParse(timeoutStr, out mins))
                    {
                        throw new ArgumentException("��ʱ������������1-9999��ֵ");
                    }
                    if (mins <= 0 || mins >= 9999)
                    {
                        throw new ArgumentException("��ʱ������������1-9999��ֵ");
                    }
                    _expiration = TimeSpan.FromMinutes((double)mins);
                }
            }

            string[] serviers ={ip+":"+port };

            SockIOPool pool = SockIOPool.GetInstance();
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

        ///// <summary>
        ///// ���������ַ��������̳߳�
        ///// </summary>
        ///// <param name="connStr"></param>
        ///// <returns></returns>
        //private MemCachedConnectPool CreatePool(string connStr) 
        //{
        //    string ip = "127.0.0.1";
        //    uint port = 11211;
        //    int maxSize=10;
        //    string[] conStrs = connStr.Split(';');
        //    string serverString = "server=";
        //    string sizeString = "maxsize=";
        //    string timeoutString = "timeout=";
        //    string part = null;
        //    foreach (string lpart in conStrs)
        //    {
        //        part = lpart.Trim();
        //        if (part.IndexOf(serverString, StringComparison.CurrentCultureIgnoreCase) == 0)
        //        {
        //            string serverStr = part.Substring(serverString.Length);
        //            string[] parts = serverStr.Split(':');
        //            if (parts.Length > 0)
        //            {
        //                ip = parts[0].Trim();
                        
        //            }
        //            if (parts.Length > 1)
        //            {
        //                if (!uint.TryParse(parts[1].Trim(), out port)) 
        //                {
        //                    throw new ArgumentException(parts[1].Trim() + "������ȷ�Ķ˿ں�");
        //                }
        //            }
        //        }
        //        else if (part.IndexOf(sizeString, StringComparison.CurrentCultureIgnoreCase) == 0)
        //        {
        //            string maxsizeStr = part.Substring(sizeString.Length);
        //            if (!int.TryParse(maxsizeStr, out maxSize)) 
        //            {
        //                throw new ArgumentException("���������������1-"+MaxVersion+"��ֵ");
        //            }
        //            if (maxSize<=0 || maxSize>=int.MaxValue)
        //            {
        //                throw new ArgumentException("���������������1-" + MaxVersion + "��ֵ");
        //            }
        //        }
        //        else if (part.IndexOf(timeoutString, StringComparison.CurrentCultureIgnoreCase) == 0)
        //        {
        //            string timeoutStr = part.Substring(timeoutString.Length);
        //            int mins = 30;
        //            if (!int.TryParse(timeoutStr, out mins)) 
        //            {
        //                throw new ArgumentException("��ʱ������������1-9999��ֵ");
        //            }
        //            if (mins <= 0 || mins >= 9999)
        //            {
        //                throw new ArgumentException("��ʱ������������1-9999��ֵ");
        //            }
        //            _expiration = TimeSpan.FromMinutes((double)mins);
        //        }
        //    }
            
        //}

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
        
        public  System.Data.DataSet GetData(IDictionary<string, bool> tableNames, string sql)
        {
            byte[] data = null;

            MemcachedClient client = new MemcachedClient();
            client.PrimitiveAsString = true;
                string key = GetKey(tableNames, sql,client,true);
                if (string.IsNullOrEmpty(key)) 
                {
                    return null;
                }
                data = client.Get(key) as byte[];

            
            if (data==null || data.Length==0) 
            {
                return null;
            }
            try
            {
                DataSet dsRet = BytesToDataSet(data);
                return dsRet;
            }
            catch 
            {
                
            }
            return null;
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
        /// <returns></returns>
        private string GetKey(IDictionary<string, bool> tableNames, string sql,
            MemcachedClient client, bool needCreateTableVer)
        {
            List<string> tables = GetSortTables(tableNames);
            StringBuilder sbSql = new StringBuilder(tables.Count * 10 + 200);

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
                sbSql.Append(key);
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

            
            sbRet.Append(PasswordHash.ToMD5String(sbSql.ToString()));
            return sbRet.ToString();
        }

        public  void RemoveBySQL(IDictionary<string, bool> tableNames,string sql)
        {
            MemcachedClient client = new MemcachedClient();
            client.PrimitiveAsString = true;
                string key = GetKey(tableNames, sql, client,false);
                if (!string.IsNullOrEmpty(key))
                {
                    client.Delete(key);
                }
            
            
        }
        /// <summary>
        /// ���汾��
        /// </summary>
        private const int MaxVersion = (int.MaxValue-1000) ;
        public  void RemoveByTableName(string tableName)
        {
            //if (client.GetValue(tableName) == null) 
            //{
            //    //_client.Set(tableName,1,
            //}
            string key = GetTableName(tableName);
            MemcachedClient client = new MemcachedClient();
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

        }

        public  bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds)
        {
            MemcachedClient client = new MemcachedClient();
            client.PrimitiveAsString = true;
            string key = GetKey(tableNames, sql, client, true);
            byte[] xml = DataSetToBytes(ds);

            client.Set(key, xml, _expiration);
            
            return true;
        }

        #endregion
        BinaryFormatter sfFormatter = new BinaryFormatter();
        /// <summary>
        /// XML�ַ���ת��DataSet
        /// </summary>
        /// <param name="xml">xml�ַ���</param>
        /// <param name="mode">ָ����ν� XML ���ݺ͹�ϵ�ܹ����� System.Data.DataSet</param>
        /// <returns></returns>
        private DataSet BytesToDataSet(byte[] data)
        {
            using (MemoryStream stm = new MemoryStream(data))
            {
                try
                {
                    //stm.Write(data, 0, data.Length);
                    DataSet ret = sfFormatter.Deserialize(stm) as DataSet;
                    return ret;
                }
                catch { }
                
            }
            return null;
        }

        /// <summary>
        /// ��DataSet���byte����
        /// </summary>
        /// <param name="ds">Ҫ�����DataSet</param>
        /// <param name="mode">ָ����δ� System.Data.DataSet д�� XML ���ݺ͹�ϵ�ܹ�</param>
        /// <returns></returns>
        private byte[] DataSetToBytes(DataSet ds)
        {
            byte[] ret = null;
            
            using (MemoryStream stm = new MemoryStream())
            {
                sfFormatter.Serialize(stm, ds);
               
                ret = stm.ToArray();
            }
            return ret;
        }

    }
}
