using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Buffalo.DB.DataBaseAdapter;
using System.Data.SQLite;
using System.Data;

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// 数据库信息锁定器
    /// </summary>
    public class DBInfoLocker
    {
        private static Dictionary<string, DbConnection> _dicConn = new Dictionary<string, DbConnection>();

        private static int _millisecondsTimeout = 15000;

        /// <summary>
        /// 锁定超时时间
        /// </summary>
        public static int MillisecondsTimeout
        {
            get { return _millisecondsTimeout; }
            set { _millisecondsTimeout = value; }
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="info">数据库信息</param>
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

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="info"></param>
        public static void FreeConnection(DBInfo info) 
        {
            //LockDBItem item = null;
            //if (_dicConn.TryGetValue(info.Name, out item)) 
            //{
            //    item.Dispose();
            //    _dicConn.Remove(info.Name);
            //}
        }
    }
}
