using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalobro.DB.CacheManager
{
    /// <summary>
    /// ��ѯ������Ϣ
    /// </summary>
    internal class QueryInfo
    {
        //private string sql;
        private object curData;
        //private int size;

        /// <summary>
        /// �˲�ѯ�Ľ��
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
        ///// �˲�ѯ������ռ�õĿռ�(�ֽ�)
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
