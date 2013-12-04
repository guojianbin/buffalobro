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
    /// 内存缓存类
    /// </summary>
    public class QueryCache
    {
        internal const string CommandDeleteSQL = "DeleteSQL";
        internal const string CommandDeleteTable = "DeleteTable";
        internal const string CommandSetDataSet = "SetDataSet";
        internal const string CommandGetDataSet = "GetDataSet";

        private DBInfo _db;

        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _db; }
            internal set { _db = value; }
        }
        private ICacheAdaper _cache;

        /// <summary>
        /// 缓存操作类
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="cache">缓存</param>
        public QueryCache(DBInfo db,ICacheAdaper cache) 
        {
            _db = db;
            _cache = cache;
        }

        /// <summary>
        /// 是否使用了缓存
        /// </summary>
        public bool HasCache 
        {
            get 
            {
                return _cache != null;
            }
        }

        /// <summary>
        /// 根据类型创建缓存适配器
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static ICacheAdaper GetCache(DBInfo info, string type, string connectionString)
        {
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }
            string dtype = type.Trim();
            if (dtype.Equals("system", StringComparison.CurrentCultureIgnoreCase))//内存
            {
                return new MemroyAdaper(info);
            }
            else if (dtype.Equals("memcached", StringComparison.CurrentCultureIgnoreCase))//memcached
            {
                return new MemCachedAdaper(connectionString, info);
            }
            else if (dtype.Equals("redis", StringComparison.CurrentCultureIgnoreCase))//memcached
            {
                return new RedisAdaper(connectionString, info);
            }
            throw new NotSupportedException("不支持:" + type + " 的缓存类型，当前只支持system、memcached类型");
            return null;
        }

        /// <summary>
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public DataSet GetDataSet(IDictionary<string, bool> tables,
            string sql, ParamList lstParam) 
        {
            if (_cache == null) 
            {
                return null;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db));
            DataSet ds = _cache.GetData(tables, sbSql.ToString());
            return ds;
        }
        /// <summary>
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
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
        /// 创建缓存关联表信息
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public Dictionary<string, bool> CreateMap(params string[] tables) 
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
        /// 获取缓存中的Reader
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public MemCacheReader GetReader( IDictionary<string, bool> tables,
            string sql, ParamList lstParam)
        {
            if (_cache == null)
            {
                return null;
            }
            DataSet ds = GetDataSet(tables,sql,lstParam);
            if (ds == null) 
            {
                return null;
            }
            MemCacheReader reader = new MemCacheReader(ds);
            return reader;
        }
        /// <summary>
        /// 获取缓存中的Reader
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
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
        /// 删除表的缓存
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public bool ClearTableCache(IDictionary<string, bool> tables) 
        {
            if (_cache == null)
            {
                return false; ;
            }
            foreach (KeyValuePair<string, bool> kvp in tables)
            {
                _cache.RemoveByTableName(kvp.Key);
            }
            return true;
        }
    }
}
