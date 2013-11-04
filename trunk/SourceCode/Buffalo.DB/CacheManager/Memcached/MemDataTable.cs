using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// 缓存表信息
    /// </summary>
    public class MemDataTable:IEnumerable<MemDataItem>
    {
        private string _tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }


        private List<DataColumn> _lstColumns=new List<DataColumn>();

        public List<DataColumn> LstColumns
        {
            get { return _lstColumns; }
        }

        private MemDataItem _fristItem;

        /// <summary>
        /// 第一条记录
        /// </summary>
        public MemDataItem FristItem
        {
            get { return _fristItem; }
        }

        private MemDataItem _last;

        /// <summary>
        /// 最后一条记录
        /// </summary>
        public MemDataItem Last
        {
            get { return _last; }
            set { _last = value; }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(MemDataItem item) 
        {
            if (_fristItem == null) 
            {
                _fristItem = item;
            }
            if (_last == null)
            {
                _last = item;
            }
            else 
            {
                _last.Next = item;
                _last = item;
            }
        }

        #region IEnumerable<MemDataItem> 成员

        public IEnumerator<MemDataItem> GetEnumerator()
        {
            return new MemDataEnumerator(_fristItem);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MemDataEnumerator(_fristItem);
        }

        #endregion
    }
}
