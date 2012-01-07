using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalobro.DB.CacheManager
{
    /// <summary>
    /// 缓存的数据类
    /// </summary>
    internal class QueryData
    {
        //private int curSize = 0;//当前缓存的大小
        //private List<string> lstInfos = new List<string>();//记录查询缓存SQL语句的顺序(根据先后顺序)
        private Dictionary<string, Dictionary<string, QueryInfo>> dicQueryCache = new Dictionary<string,Dictionary<string,QueryInfo>>();

        ///// <summary>
        ///// 当前缓存的大小
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
        ///// 记录查询缓存SQL语句的顺序(根据先后顺序)
        ///// </summary>
        //public List<string> LstInfos
        //{
        //    get
        //    {
        //        return lstInfos;
        //    }
            
        //}

        /// <summary>
        /// 设置或获取缓存的数据
        /// </summary>
        /// <param name="typeName">对应类型的FullName</param>
        /// <param name="sql">对应的SQL语句</param>
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
        /// 删除该类型对应SQL语句的数据
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="sql">SQL语句</param>
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
        /// 删除该类型的数据
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="sql">SQL语句</param>
        public void Delete(string typeName)
        {
            using (Lock locker = new Lock(dicQueryCache))
            {
                dicQueryCache.Remove(typeName);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearAll() 
        {
            dicQueryCache.Clear();
        }

        ///// <summary>
        ///// 缓存集
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
