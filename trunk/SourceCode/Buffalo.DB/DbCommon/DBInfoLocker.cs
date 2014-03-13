using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Buffalo.DB.DataBaseAdapter;
using System.Data;
using Buffalo.DB.DataBaseAdapter.SQLiteAdapter;
using System.Data.SQLite;

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// ���ݿ���Ϣ������
    /// </summary>
    public class DBInfoLocker
    {
        private static Dictionary<string, DbConnection> _dicConn = new Dictionary<string, DbConnection>();



        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="info">���ݿ���Ϣ</param>
        /// <returns></returns>
        public static DbConnection LockDB(DBInfo info) 
        {
            //LockDBItem item = new LockDBItem(info, _millisecondsTimeout);
            DbConnection conn = null;
            if (!_dicConn.TryGetValue(info.Name, out conn)) 
            {
                conn = new SQLiteConnection();
                _dicConn[info.Name] = conn;
            }
            if (conn == null) 
            {
                conn = new SQLiteConnection();
                _dicConn[info.Name] = conn;
            }
            //_dicConn.Add(info.Name,item);
            return conn;
        }

    }
}
