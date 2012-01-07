using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;

namespace Buffalobro.DB.CacheManager
{
    /// <summary>
    /// 查询缓存类
    /// </summary>
    public class QueryCache
    {
        //private static int maxSize=-1;//最大缓存
        private static QueryData data=new QueryData();
        ///// <summary>
        ///// 获取最大缓存
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
        ///// 初始化最大缓存
        ///// </summary>
        //private static void InitMaxSize() 
        //{
        //    string strSize = null;
        //    if ((strSize = System.Configuration.ConfigurationManager.AppSettings["CacheSize"]) != null)
        //    {
        //        maxSize = Convert.ToInt32(strSize);
        //    }
        //    else //如果没有配置，默认为16M
        //    {
        //        maxSize = 16 * 1024 * 1024;
        //    }
        //}

        ///// <summary>
        ///// 根据sql的键删除缓存
        ///// </summary>
        ///// <param name="sql">sql</param>
        //private static void Remove(string sql)
        //{
        //    //data.LstInfos.Remove(sql);
        //    QueryInfo info = null;
        //    if (data.DicQueryCache.TryGetValue(sql, out info))
        //    {
        //        //data.CurSize = data.CurSize - info.Size;//当前占用空间减去被释放的空间
        //        data.DicQueryCache.Remove(sql);//删除指定的SQL语句
        //    }
        //}
        /// <summary>
        /// 把数据加进缓存
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="entityType">表对应的实体的类型</param>
        /// <param name="val">数据</param>
        /// <returns></returns>
        public static void AppendData(string sql, Type entityType,object val)
        {
            QueryViewConfig.RegisterView(entityType);
            data[entityType.FullName, sql]=val;
        }

        /// <summary>
        /// 根据SQL语句从缓存中找出数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="entityType">表对应的实体的类型</param>
        /// <returns></returns>
        public static object GetData(string sql,Type entityType) 
        {
            return data[entityType.FullName,sql];
        }

        /// <summary>
        /// 根据实体清除对应的缓存
        /// </summary>
        /// <param name="entityType"></param>
        public static void ClearCache(Type entityType) 
        {
            string fullName = entityType.FullName;
            data.Delete(fullName);
            List<string> lstViewName=QueryViewConfig.GetViewList(fullName);
            if (lstViewName != null)//清空对应视图的缓存 
            {
                foreach (string viewName in lstViewName) 
                {
                    data.Delete(viewName);
                }
            }
        }

        ///// <summary>
        ///// 把SQL语句移动到最新的位置
        ///// </summary>
        ///// <param name="sql"></param>
        //private static void MoveToEnd(string sql) 
        //{
            
        //    List<string> lstSql = data.LstInfos;
        //    int index=-1;//SQL语句所在的位置
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
        //        for (int i = index; i < lstSql.Count-1; i++) //把指定位置的SQL挪到最后
        //        {
        //            string tmp = lstSql[i];
        //            lstSql[i] = lstSql[i + 1];
        //            lstSql[i + 1] = tmp;
        //        }
        //    }
            
        //}

        ///// <summary>
        ///// 重新调整空间
        ///// </summary>
        //private static void ReCapacity()
        //{
        //    int max=MaxSize;
        //    while (data.CurSize > max) 
        //    {
        //        if (data.LstInfos.Count > 0) //空间不足时候删除最早的缓存
        //        {
        //            Remove(data.LstInfos[0]);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 添加缓存
        ///// </summary>
        ///// <param name="sql">缓存对应的SQL</param>
        ///// <param name="curdata">缓存对应的数据</param>
        ///// /// <param name="entityType">缓存对应的数据类型</param>
        //public static void Add(string sql, object curdata,Type entityType) 
        //{
        //    //if (size > MaxSize) //如果当前集合比总容量更大，则不缓存
        //    //{
        //    //    return;
        //    //}
        //    using (Lock locker = new Lock(data))
        //    {
        //        QueryInfo info = new QueryInfo();
        //        info.CurData = curdata;
        //        //info.Size = size;
        //        data.DicQueryCache.Add(sql, info);
        //        //data.CurSize += size;//增加占用空间
        //        //ReCapacity();//重新限制空间
        //        //data.LstInfos.Add(sql);
        //    }
        //}

        ///// <summary>
        ///// 批量删除SQL语句对应的键值
        ///// </summary>
        ///// <param name="lstSqls"></param>
        //public static void RemoveRang(List<string> lstSqls) 
        //{
        //    using (Lock locker = new Lock(data))
        //    {
        //        foreach (string sql in lstSqls) //批量删除缓存
        //        {
        //            Remove(sql);
        //        }
        //    }
        //}
    }
}
