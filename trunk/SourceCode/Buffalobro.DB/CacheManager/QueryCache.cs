using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;

namespace Buffalobro.DB.CacheManager
{
    /// <summary>
    /// ��ѯ������
    /// </summary>
    public class QueryCache
    {
        //private static int maxSize=-1;//��󻺴�
        private static QueryData data=new QueryData();
        ///// <summary>
        ///// ��ȡ��󻺴�
        ///// </summary>
        //private static int MaxSize 
        //{
        //    get 
        //    {
        //        if (maxSize < 0) 
        //        {
        //            InitMaxSize();
        //        }
        //        return maxSize;
        //    }
        //}

        ///// <summary>
        ///// ��ʼ����󻺴�
        ///// </summary>
        //private static void InitMaxSize() 
        //{
        //    string strSize = null;
        //    if ((strSize = System.Configuration.ConfigurationManager.AppSettings["CacheSize"]) != null)
        //    {
        //        maxSize = Convert.ToInt32(strSize);
        //    }
        //    else //���û�����ã�Ĭ��Ϊ16M
        //    {
        //        maxSize = 16 * 1024 * 1024;
        //    }
        //}

        ///// <summary>
        ///// ����sql�ļ�ɾ������
        ///// </summary>
        ///// <param name="sql">sql</param>
        //private static void Remove(string sql)
        //{
        //    //data.LstInfos.Remove(sql);
        //    QueryInfo info = null;
        //    if (data.DicQueryCache.TryGetValue(sql, out info))
        //    {
        //        //data.CurSize = data.CurSize - info.Size;//��ǰռ�ÿռ��ȥ���ͷŵĿռ�
        //        data.DicQueryCache.Remove(sql);//ɾ��ָ����SQL���
        //    }
        //}
        /// <summary>
        /// �����ݼӽ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="entityType">���Ӧ��ʵ�������</param>
        /// <param name="val">����</param>
        /// <returns></returns>
        public static void AppendData(string sql, Type entityType,object val)
        {
            QueryViewConfig.RegisterView(entityType);
            data[entityType.FullName, sql]=val;
        }

        /// <summary>
        /// ����SQL���ӻ������ҳ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="entityType">���Ӧ��ʵ�������</param>
        /// <returns></returns>
        public static object GetData(string sql,Type entityType) 
        {
            return data[entityType.FullName,sql];
        }

        /// <summary>
        /// ����ʵ�������Ӧ�Ļ���
        /// </summary>
        /// <param name="entityType"></param>
        public static void ClearCache(Type entityType) 
        {
            string fullName = entityType.FullName;
            data.Delete(fullName);
            List<string> lstViewName=QueryViewConfig.GetViewList(fullName);
            if (lstViewName != null)//��ն�Ӧ��ͼ�Ļ��� 
            {
                foreach (string viewName in lstViewName) 
                {
                    data.Delete(viewName);
                }
            }
        }

        ///// <summary>
        ///// ��SQL����ƶ������µ�λ��
        ///// </summary>
        ///// <param name="sql"></param>
        //private static void MoveToEnd(string sql) 
        //{
            
        //    List<string> lstSql = data.LstInfos;
        //    int index=-1;//SQL������ڵ�λ��
        //    for (int i = 0; i < lstSql.Count; i++) 
        //    {
        //        if (lstSql[i] == sql) 
        //        {
        //            index = i;
        //            break;
        //        }
        //    }
        //    if (index >= 0) 
        //    {
        //        for (int i = index; i < lstSql.Count-1; i++) //��ָ��λ�õ�SQLŲ�����
        //        {
        //            string tmp = lstSql[i];
        //            lstSql[i] = lstSql[i + 1];
        //            lstSql[i + 1] = tmp;
        //        }
        //    }
            
        //}

        ///// <summary>
        ///// ���µ����ռ�
        ///// </summary>
        //private static void ReCapacity()
        //{
        //    int max=MaxSize;
        //    while (data.CurSize > max) 
        //    {
        //        if (data.LstInfos.Count > 0) //�ռ䲻��ʱ��ɾ������Ļ���
        //        {
        //            Remove(data.LstInfos[0]);
        //        }
        //    }
        //}

        ///// <summary>
        ///// ��ӻ���
        ///// </summary>
        ///// <param name="sql">�����Ӧ��SQL</param>
        ///// <param name="curdata">�����Ӧ������</param>
        ///// /// <param name="entityType">�����Ӧ����������</param>
        //public static void Add(string sql, object curdata,Type entityType) 
        //{
        //    //if (size > MaxSize) //�����ǰ���ϱ������������򲻻���
        //    //{
        //    //    return;
        //    //}
        //    using (Lock locker = new Lock(data))
        //    {
        //        QueryInfo info = new QueryInfo();
        //        info.CurData = curdata;
        //        //info.Size = size;
        //        data.DicQueryCache.Add(sql, info);
        //        //data.CurSize += size;//����ռ�ÿռ�
        //        //ReCapacity();//�������ƿռ�
        //        //data.LstInfos.Add(sql);
        //    }
        //}

        ///// <summary>
        ///// ����ɾ��SQL����Ӧ�ļ�ֵ
        ///// </summary>
        ///// <param name="lstSqls"></param>
        //public static void RemoveRang(List<string> lstSqls) 
        //{
        //    using (Lock locker = new Lock(data))
        //    {
        //        foreach (string sql in lstSqls) //����ɾ������
        //        {
        //            Remove(sql);
        //        }
        //    }
        //}
    }
}
