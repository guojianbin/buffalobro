using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Data;
using System.Collections;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ϵͳ�ڴ�Ļ���������
    /// </summary>
    public class MemroyAdaper : ICacheAdaper
    {
        public MemroyAdaper() 
        {
            
        }

        private Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="sql">SQL��</param>
        /// <param name="dt">����</param>
        /// <returns></returns>
        public bool SetData(ICollection<string> tableNames, string sql, DataSet ds) 
        {
            string key = QueryCache.GetTableKeyName(tableName);
            Hashtable hs = _cache[key] as Hashtable;
            if (hs == null)
            {
                hs = new Hashtable();
                _cache[key] = hs;
            }
            hs[sql] = ds;
            return true;
        }
        /// <summary>
        /// ����SQL���ӻ������ҳ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="entityType">���Ӧ��ʵ�������</param>
        /// <returns></returns>
        public DataSet GetData(string tableName, string sql)
        {
            Hashtable hs = _cache[QueryCache.GetTableKeyName(tableName)] as Hashtable;
            if (hs == null)
            {
                return null;
            }
            return hs[sql] as DataSet;
        }

        /// <summary>
        /// ͨ��SQLɾ��ĳ��
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL(string tableName,string sql) 
        {
            Hashtable hs = _cache[QueryCache.GetTableKeyName(tableName)] as Hashtable;
            if (hs == null)
            {
                return;
            }
            hs.Remove(sql);
        }
        /// <summary>
        /// ͨ������ɾ��������
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveByTableName(string tableName)
        {
            _cache.Remove(tableName);
        }
    }
}
