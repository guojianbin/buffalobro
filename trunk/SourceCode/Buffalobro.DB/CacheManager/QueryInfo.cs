using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalobro.DB.CacheManager
{
    /// <summary>
    /// 查询的项信息
    /// </summary>
    internal class QueryInfo
    {
        //private string sql;
        private object curData;
        //private int size;

        /// <summary>
        /// 此查询的结果
        /// </summary>
        public object CurData 
        {
            get
            {
                return curData;
            }
            set
            {
                curData = value;
            }
        }

        ///// <summary>
        ///// 此查询的数据占用的空间(字节)
        ///// </summary>
        //public int Size 
        //{
        //    get 
        //    {
        //        return size;
        //    }
        //    set 
        //    {
        //        size = value;
        //    }
        //}
    }
}
