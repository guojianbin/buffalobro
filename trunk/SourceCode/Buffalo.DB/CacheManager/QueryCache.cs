using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataFillers;
using System.Data.Common;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// �ڴ滺����
    /// </summary>
    public class QueryCache
    {
        internal const string CommandDeleteSQL = "DeleteSQL";
        internal const string CommandDeleteTable = "DeleteTable";
        internal const string CommandSetDataSet = "SetDataSet";
        internal const string CommandGetDataSet = "GetDataSet";

        private DBInfo _db;

        private Dictionary<string, bool> _dicAllowCache = new Dictionary<string, bool>();

        private bool _isAllTableCache;
        /// <summary>
        /// �Ƿ����б�ʹ�û���
        /// </summary>
        public bool IsAllTableCache
        {
            get { return _isAllTableCache; }
        }
        /// <summary>
        /// ���ݿ���Ϣ
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _db; }
            internal set { _db = value; }
        }
        private ICacheAdaper _cache;

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="db">���ݿ�</param>
        public QueryCache(DBInfo db) 
        {
            _db = db;
           
        }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="cache">������</param>
        /// <param name="isAllTableCache">�Ƿ����б����л���</param>
        internal void InitCache(ICacheAdaper cache, bool isAllTableCache) 
        {
            _cache = cache;
            _isAllTableCache = isAllTableCache;
        }

        /// <summary>
        /// �Ƿ�ʹ���˻���
        /// </summary>
        public bool HasCache 
        {
            get 
            {
                return _cache != null;
            }
        }

        /// <summary>
        /// �������ʹ�������������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="connectionString">�����ַ���</param>
        /// <returns></returns>
        public static ICacheAdaper GetCache(DBInfo info, string type, string connectionString)
        {
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }
            string dtype = type.Trim();
            if (dtype.Equals("system", StringComparison.CurrentCultureIgnoreCase))//�ڴ�
            {
                return new MemoryAdaper(info);
            }
            else if (dtype.Equals("memcached", StringComparison.CurrentCultureIgnoreCase))//memcached
            {
                return new MemCachedAdaper(connectionString, info);
            }
            else if (dtype.Equals("redis", StringComparison.CurrentCultureIgnoreCase))//redis
            {
                return new RedisAdaper(connectionString, info);
            }

            throw new NotSupportedException("��֧��:" + type + " �Ļ������ͣ���ǰֻ֧��system��memcached��redis���͵Ļ���");
            return null;
        }

        /// <summary>
        /// ��ȡ�����е�DataSet
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <returns></returns>
        public DataSet GetDataSet(IDictionary<string, bool> tables,
            string sql, ParamList lstParam) 
        {
            if (_cache == null) 
            {
                return null;
            }
            CheckTable(tables);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db));
            DataSet ds = _cache.GetData(tables, sbSql.ToString());
            return ds;
        }
        /// <summary>
        /// ��ȡ�����е�DataSet
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <returns></returns>
        public bool SetDataSet(DataSet ds, IDictionary<string, bool> tables,
            string sql, ParamList lstParam)
        {
            if (_cache == null)
            {
                return false;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db));
            return _cache.SetData(tables, sbSql.ToString(), ds);
            
        }

        /// <summary>
        /// ���������������Ϣ
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        internal Dictionary<string, bool> CreateMap(params string[] tables) 
        {
            if (_cache == null)
            {
                return null;
            }
            Dictionary<string, bool> dic = new Dictionary<string, bool>(tables.Length+1);
            foreach (string tableName in tables) 
            {
                dic[tableName] = true;
            }
            return dic;
        }
        /// <summary>
        /// ��ȡ�����е�Reader
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <returns></returns>
        public MemCacheReader GetReader( IDictionary<string, bool> tables,
            string sql, ParamList lstParam)
        {
            if (_cache == null)
            {
                return null;
            }
            CheckTable(tables);
            DataSet ds = GetDataSet(tables,sql,lstParam);
            if (ds == null) 
            {
                return null;
            }
            MemCacheReader reader = new MemCacheReader(ds);
            return reader;
        }
        /// <summary>
        /// ��ȡ�����е�Reader
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <returns></returns>
        public IDataReader SetReader(IDataReader reader, IDictionary<string, bool> tables,
            string sql, ParamList lstParam)
        {
            if (_cache == null)
            {
                return null;
            }
            
            DataSet ds = CacheReader.GenerateDataSet(reader, false);
            MemCacheReader mreader = new MemCacheReader(ds);
            SetDataSet(ds, tables, sql, lstParam);
            return mreader;
        }
        /// <summary>
        /// ɾ����Ļ���
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public bool ClearTableCache(IDictionary<string, bool> tables) 
        {
            if (_cache == null)
            {
                return false; 
            }

            foreach (KeyValuePair<string, bool> kvp in tables)
            {
                if (IsCacheTable(kvp.Key) || _isAllTableCache)
                {
                    _cache.RemoveByTableName(kvp.Key);
                }
            }
            return true;
        }

        /// <summary>
        /// �����Ƿ���û���
        /// </summary>
        /// <param name="tables"></param>
        private void CheckTable(IDictionary<string, bool> tables) 
        {
            if (_isAllTableCache) 
            {
                return;
            }
            StringBuilder sbBuffer=new StringBuilder();
            foreach (KeyValuePair<string, bool> kvp in tables) 
            {
                if (!IsCacheTable(kvp.Key)) 
                {
                    sbBuffer.Append(kvp.Key);
                    sbBuffer.Append(",");
                }
            }
            if (sbBuffer.Length > 0) 
            {
                sbBuffer.Remove(sbBuffer.Length - 1, 1);
                throw new Exception("��:" + sbBuffer.ToString() + "û����Ϊʹ�û��棬���������ļ���ָ��");
            }
        }

        /// <summary>
        /// �жϱ����Ƿ�����ʹ�û���
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private bool IsCacheTable(string table) 
        {
            bool hascache = false;

            if (_dicAllowCache.TryGetValue(table, out hascache)) 
            {
                return hascache;
            }
            
            return false;
        }

        /// <summary>
        /// ������Ҫ����ı�
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool SetCacheTable(string tableName) 
        {
            _dicAllowCache[tableName] = true;
            return true;
        }
    }
}
