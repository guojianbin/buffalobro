using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Data;
using System.Collections;
using Buffalo.Kernel;

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
        private Dictionary<string, List<string>> _dicTableToKey = new Dictionary<string, List<string>>();
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="sql">SQL��</param>
        /// <param name="dt">����</param>
        /// <returns></returns>
        public bool SetData(ICollection<string> tableNames, string sql, DataSet ds) 
        {
            string key = GetKey(sql);
            List<string> sqlItems = null;
            //��ӱ��Ӧ��SQL���ֵ
            foreach (string tableName in tableNames) 
            {
                if (!_dicTableToKey.TryGetValue(tableName, out sqlItems)) 
                {
                    using (Lock objLock = new Lock(_dicTableToKey))
                    {
                        sqlItems = new List<string>();
                        _dicTableToKey[tableName] = sqlItems;
                    }
                }
                using (Lock objLock = new Lock(sqlItems))
                {
                    sqlItems.Add(key);
                }
            }
            _cache[key] = ds;
            return true;
        }

        /// <summary>
        /// ͨ��SQL��ȡ��
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string GetKey(string sql) 
        {
            StringBuilder sbKey = new StringBuilder(100);
            sbKey.Append("BuffaloCache:");
            sbKey.Append(sql.Length );
            sbKey.Append(":");
            sbKey.Append(PasswordHash.ToSHA1String(sql));
            return sbKey.ToString();
        }

        /// <summary>
        /// ����SQL���ӻ������ҳ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableNames">��������</param>
        /// <returns></returns>
        public DataSet GetData( string sql)
        {
            string key = GetKey(sql);
            return hs[key] as DataSet;
        }

        /// <summary>
        /// ͨ��SQLɾ��ĳ��
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL( string sql) 
        {
            string key = GetKey(sql);
            hs.Remove(key);
        }
        /// <summary>
        /// ͨ������ɾ��������
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveByTableName(string tableName)
        {
            List<string> sqlItems = null;
            if (_dicTableToKey.TryGetValue(tableName, out sqlItems))
            {
                if (sqlItems == null)
                {
                    return;
                }
                using (Lock objLock = new Lock(sqlItems))
                {
                    foreach (string key in sqlItems)
                    {
                        _cache.Remove(key);
                    }
                    sqlItems.Clear();
                }
            }
        }
    }

    
}
