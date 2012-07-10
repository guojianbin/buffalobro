using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// ���ݿ���Ϣ������
    /// </summary>
    public class DBInfoLocker
    {
        private static Dictionary<string, LockDBItem> _dicConn = new Dictionary<string, LockDBItem>();

        private static int _millisecondsTimeout = 15000;

        /// <summary>
        /// ������ʱʱ��
        /// </summary>
        public static int MillisecondsTimeout
        {
            get { return _millisecondsTimeout; }
            set { _millisecondsTimeout = value; }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="info">���ݿ���Ϣ</param>
        /// <returns></returns>
        public static bool LockDB(DBInfo info) 
        {
            LockDBItem item = new LockDBItem(info, _millisecondsTimeout);

            _dicConn.Add(info.Name,item);
            return true;
        }

        /// <summary>
        /// �ͷ�����
        /// </summary>
        /// <param name="info"></param>
        public static void FreeConnection(DBInfo info) 
        {
            LockDBItem item = null;
            if (_dicConn.TryGetValue(info.Name, out item)) 
            {
                item.Dispose();
                _dicConn.Remove(info.Name);
            }
        }
    }
}
