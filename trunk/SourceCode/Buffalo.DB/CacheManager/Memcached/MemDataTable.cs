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
        private int _count;
        /// <summary>
        /// ��¼��
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        private List<DataColumn> _lstColumns=new List<DataColumn>();

        public List<DataColumn> Columns
        {
            get { return _lstColumns; }
        }

        private MemDataItem _fristItem;

        /// <summary>
        /// ��һ����¼
        /// </summary>
        protected internal MemDataItem FristItem
        {
            get { return _fristItem; }
        }

        private MemDataItem _last;

        /// <summary>
        /// ���һ����¼
        /// </summary>
        protected internal MemDataItem Last
        {
            get { return _last; }
        }
        private MemDataItem _current;

        /// <summary>
        /// ��ǰ��¼
        /// </summary>
        public MemDataItem Current
        {
            get { return _current; }
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
            _count++;
        }
        /// <summary>
        /// �ƶ�����һ��
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() 
        {
            if (_current == null) 
            {
                _current = _fristItem;
                
            }
            else if (_current.Next == null)
            {
                return false;
            }
            else 
            {
                _current = _current.Next;
            }
            if (_current == null) 
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// �����ж�ȡ
        /// </summary>
        public void Reset() 
        {
            _current = null;
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="item"></param>
        public void AddItemData(IEnumerable<object> datas)
        {
            MemDataItem item = new MemDataItem();
            item.AddRange(datas);
            AddItem(item);
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
