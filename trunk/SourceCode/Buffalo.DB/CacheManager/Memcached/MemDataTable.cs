using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// �������Ϣ
    /// </summary>
    public class MemDataTable:IEnumerable<MemDataItem>
    {
        private string _tableName;
        /// <summary>
        /// ����
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
        /// ��һ����¼
        /// </summary>
        public MemDataItem FristItem
        {
            get { return _fristItem; }
        }

        private MemDataItem _last;

        /// <summary>
        /// ���һ����¼
        /// </summary>
        public MemDataItem Last
        {
            get { return _last; }
            set { _last = value; }
        }

        /// <summary>
        /// �����
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

        #region IEnumerable<MemDataItem> ��Ա

        public IEnumerator<MemDataItem> GetEnumerator()
        {
            return new MemDataEnumerator(_fristItem);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MemDataEnumerator(_fristItem);
        }

        #endregion
    }
}
