using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// 数据行信息
    /// </summary>
    public class MemDataItem:List<object>
    {
        private MemDataItem _next;

        /// <summary>
        /// 下一个项
        /// </summary>
        public MemDataItem Next
        {
            get { return _next; }
            set { _next = value; }
        }
    }
}
