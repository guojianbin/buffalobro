using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalobro.DB.CacheManager
{
    /// <summary>
    /// �����������
    /// </summary>
    internal class QueryData
    {
        //private int curSize = 0;//��ǰ����Ĵ�С
        //private List<string> lstInfos = new List<string>();//��¼��ѯ����SQL����˳��(�����Ⱥ�˳��)
        private Dictionary<string, Dictionary<string, QueryInfo>> dicQueryCache = new Dictionary<string,Dictionary<string,QueryInfo>>();

        ///// <summary>
        ///// ��ǰ����Ĵ�С
        ///// </summary>
        //public int CurSize 
        //{
        //    get 
        //    {
        //        return curSize;
        //    }
        //    set 
        //    {
        //        curSize = value;
        //    }
        //}

        ///// <summary>
        ///// ��¼��ѯ����SQL����˳��(�����Ⱥ�˳��)
        ///// </summary>
        //public List<string> LstInfos
        //{
        //    get
        //    {
        //        return lstInfos;
        //    }
            
        //}

        /// <summary>
        /// ���û��ȡ���������
        /// </summary>
        /// <param name="typeName">��Ӧ���͵�FullName</param>
        /// <param name="sql">��Ӧ��SQL���</param>
        /// <returns></returns>
        public object this[string typeName,string sql]
        {
            get
            {
                object ret = null;
                using (Lock locker = new Lock(dicQueryCache))
                {
                    Dictionary<string, QueryInfo> dicInfos = null;
                    if (dicQueryCache.TryGetValue(typeName,out dicInfos))
                    {
                        QueryInfo info = null;
                        if (dicInfos.TryGetValue(sql,out info))
                        {
                            ret=info.CurData;
                        }
                    }
                }
                return ret;
            }
            set 
            {
                using (Lock locker = new Lock(dicQueryCache))
                {
                    Dictionary<string, QueryInfo> dicInfos = null;
                    if (!dicQueryCache.TryGetValue(typeName,out dicInfos))
                    {
                        dicInfos = new Dictionary<string, QueryInfo>();
                        dicQueryCache.Add(typeName, dicInfos);
                    }
                    QueryInfo info = null;
                    if (!dicInfos.TryGetValue(sql,out info))
                    {
                        info = new QueryInfo();
                        dicInfos.Add(sql, info);
                    }
                    
                    info.CurData = value;
                }
                
            }
        }

        /// <summary>
        /// ɾ�������Ͷ�ӦSQL��������
        /// </summary>
        /// <param name="typeName">������</param>
        /// <param name="sql">SQL���</param>
        public void Delete(string typeName, string sql) 
        {
            using (Lock locker = new Lock(dicQueryCache))
            {
                Dictionary<string, QueryInfo> dicInfos = null;
                if (dicQueryCache.TryGetValue(typeName,out dicInfos))
                {
                    dicInfos.Remove(typeName);
                }
            }
        }
        /// <summary>
        /// ɾ�������͵�����
        /// </summary>
        /// <param name="typeName">������</param>
        /// <param name="sql">SQL���</param>
        public void Delete(string typeName)
        {
            using (Lock locker = new Lock(dicQueryCache))
            {
                dicQueryCache.Remove(typeName);
            }
        }

        /// <summary>
        /// ��ջ���
        /// </summary>
        public void ClearAll() 
        {
            dicQueryCache.Clear();
        }

        ///// <summary>
        ///// ���漯
        ///// </summary>
        //public Dictionary<string, QueryInfo> DicQueryCache
        //{
        //    get
        //    {
        //        return dicQueryCache;
        //    }
            
        //}

    }
}
