using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public class MemDataItem:List<object>
    {
        public MemDataItem(int capacity) :base(capacity)
        {

        }


        private MemDataItem _next;

        /// <summary>
        /// ��һ����
        /// </summary>
        public MemDataItem Next
        {
            get { return _next; }
            set { _next = value; }
        }
    }
}