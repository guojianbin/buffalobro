using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// �ڴ滺����
    /// </summary>
    public class QueryCache
    {
        

        /// <summary>
        /// ���ɱ�����Ӧ��Key
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        internal static string GetTableKeyName(string tableName) 
        {
            StringBuilder sbRet = new StringBuilder(tableName.Length + 20);
            sbRet.Append("___Table:");
            sbRet.Append(tableName);
            return sbRet.ToString();
        }



        /// <summary>
        /// ���ݱ��������Ӧ�Ļ���
        /// </summary>
        /// <param name="tableName">����</param>
        public void ClearCache(string tableName)
        {
            //string fullName = entityType.FullName;
            //_cache.Remove(GetKeyByEntity(entityName));
            //List<string> lstViewName = QueryViewConfig.GetViewList(fullName);
            //if (lstViewName != null)//��ն�Ӧ��ͼ�Ļ��� 
            //{
            //    foreach (string viewName in lstViewName)
            //    {
            //        _cache.Remove(GetKeyByEntity(viewName));
            //    }
            //}
        }
    }
}
