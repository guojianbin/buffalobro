using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.Memcached
{
    public class MemDataEnumerator : IEnumerator<MemDataItem>
    {
        private MemDataItem _head;

        private MemDataItem _current;
        /// <summary>
        /// 存储项的枚举类
        /// </summary>
        /// <param name="head"></param>
        public MemDataEnumerator(MemDataItem head) 
        {
            _head = head;
        }

        #region IEnumerator<MemDataItem> 成员

        public MemDataItem Current
        {
            get 
            {
                return _current; 
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            
        }

        #endregion

        #region IEnumerator 成员

        object System.Collections.IEnumerator.Current
        {
            get { return _current; }
        }

        public bool MoveNext()
        {
            if (_current == null) 
            {
                _current = _head;
            }
            if (_current.Next == null) 
            {
                return false;
            }
            _current = _current.Next;
            return true;
        }

        public void Reset()
        {
            _current = null;
        }

        #endregion
    }
}
