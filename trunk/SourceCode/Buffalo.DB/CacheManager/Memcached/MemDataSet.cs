using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// 缓存数据集信息
    /// </summary>
    public class MemDataSet
    {
        private List<MemDataTable> _tables=new List<MemDataTable>();

        public List<MemDataTable> Tables
        {
            get { return _tables; }
        }
    }
}
