using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// �������ݼ���Ϣ
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
