using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using SQLCommon.CacheManager;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ��ѯ������
    /// </summary>
    public class QueryCache
    {
        private static Cache _cache = HttpRuntime.Cache;

        /// <summary>
        /// ��ȡSql��Key
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static string GetKeyBySql(string sql) 
        {
            StringBuilder sbRet = new StringBuilder(sql.Length + 20);
            sbRet.Append("___SQL:");
            sbRet.Append(sql);
            return sbRet.ToString();
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private static string GetKeyByEntity(string typeName) 
        {
            StringBuilder sbRet = new StringBuilder(typeName.Length + 20);
            sbRet.Append("___Entity:");
            sbRet.Append(typeName);
            return sbRet.ToString();
        }

        /// <summary>
        /// ����SQL���ӻ������ҳ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="entityType">���Ӧ��ʵ�������</param>
        /// <returns></returns>
        public static object GetData(string typeName,string sql)
        {
            Hashtable hs = _cache[GetKeyByEntity(typeName)] as Hashtable;
            if (hs == null) 
            {
                return null;
            }
            return hs[GetKeyBySql(sql)];
        }
        /// <summary>
        /// �����ݼӽ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="entityType">���Ӧ��ʵ�������</param>
        /// <param name="val">����</param>
        /// <returns></returns>
        public static void AppendData(string sql, string typeName, object val)
        {
            string key=GetKeyByEntity(typeName);
            Hashtable hs = _cache[key] as Hashtable;
            if (hs == null) 
            {
                hs = new Hashtable();
                _cache[key] = hs;
            }
            hs[sql] = val;


        }
        /// <summary>
        /// ����ʵ�������Ӧ�Ļ���
        /// </summary>
        /// <param name="entityType"></param>
        public static void ClearCache(Type entityType)
        {
            string fullName = entityType.FullName;
            _cache.Remove(GetKeyByEntity(fullName));
            List<string> lstViewName = QueryViewConfig.GetViewList(fullName);
            if (lstViewName != null)//��ն�Ӧ��ͼ�Ļ��� 
            {
                foreach (string viewName in lstViewName)
                {
                    _cache.Remove(GetKeyByEntity(viewName));
                }
            }
        }
    }
}
