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
using System.Reflection;

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
        public void InitCache(ICacheAdaper cache, bool isAllTableCache) 
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
            ICacheAdaper cache = GetAssemblyCache(info, dtype, connectionString);
            if (cache != null) 
            {
                return cache;
            }

            throw new NotSupportedException("��֧��:" + type + " �Ļ������ͣ���ǰֻ֧��system��memcached��redis���͵Ļ���");
        }

        private static Assembly _cacheAssembly = null;
        /// <summary>
        /// ��ȡ�ⲿ���򼯵Ļ���
        /// </summary>
        /// <param name="info"></param>
        /// <param name="type"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ICacheAdaper GetAssemblyCache(DBInfo info, string type, string connectionString) 
        {
            Assembly _cacheAssembly = null;
            try
            {
                _cacheAssembly = Assembly.Load("Buffalo.QueryCache");
            }
            catch (Exception ex)
            {
                throw new MissingMemberException("�Ҳ�����Buffalo.QueryCache,�뱣֤��Ŀ�Ѿ�������Buffalo.QueryCache.dll");
            }
            if (_cacheAssembly == null) 
            {
                throw new MissingMemberException("�Ҳ�����Buffalo.QueryCache,�뱣֤��Ŀ�Ѿ�������Buffalo.QueryCache.dll");
            }
            Type loaderType = _cacheAssembly.GetType("Buffalo.QueryCache.CacheLoader", false, false);
            if (loaderType == null) 
            {
                throw new MissingMemberException("�Ҳ�����Buffalo.QueryCache.CacheLoader,�뱣֤Buffalo.QueryCache.dll��������");
            }
            MethodInfo mInfo = loaderType.GetMethod("GetCache");
            if (loaderType == null)
            {
                throw new MissingMethodException("�Ҳ�������GetCache,�뱣֤Buffalo.QueryCache.dll��������");
            }
            ICacheAdaper cache = mInfo.Invoke(null, new object[] {info,type,connectionString }) as ICacheAdaper;
            return cache;
        }

        /// <summary>
        /// ��ȡ�����е�DataSet
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <returns></returns>
        public DataSet GetDataSet(IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }
            CheckTable(tables);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db, oper));

            DataSet ds = _cache.GetData(tables, sbSql.ToString(), oper);

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
            string sql, ParamList lstParam,DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return false;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db,oper));
            return _cache.SetData(tables, sbSql.ToString(), ds,oper);
            
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
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }
            CheckTable(tables);
            DataSet ds = GetDataSet(tables,sql,lstParam,oper);
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
            string sql, ParamList lstParam,DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }
            
            DataSet ds = CacheReader.GenerateDataSet(reader, false);
            MemCacheReader mreader = new MemCacheReader(ds);
            SetDataSet(ds, tables, sql, lstParam,oper);
            return mreader;
        }
        /// <summary>
        /// ɾ����Ļ���
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public bool ClearTableCache(IDictionary<string, bool> tables, DataBaseOperate oper) 
        {
            if (_cache == null)
            {
                return false; 
            }
            
            foreach (KeyValuePair<string, bool> kvp in tables)
            {
                if (IsCacheTable(kvp.Key) || _isAllTableCache)
                {
                    _cache.RemoveByTableName(kvp.Key,oper);
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
