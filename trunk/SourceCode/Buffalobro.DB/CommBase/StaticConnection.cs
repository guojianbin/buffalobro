using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DbCommon;
using System.Web;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.EntityInfos;
using Buffalo.Kernel;
using System.Diagnostics;

namespace Buffalobro.DB.CommBase
{
    /// <summary>
    /// 静态连接管理类
    /// </summary>
    public class StaticConnection
    {
        private const string SessionName = "___StaticConnections___";

        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static DataBaseOperate GetOperate(string name)
        {

            if (Commons.CommonMethods.IsWebContext)
            {
                return (DataBaseOperate)System.Web.HttpContext.Current.Items[name];
            }
            else
            {
                return (DataBaseOperate)System.Runtime.Remoting.Messaging.CallContext.GetData(name);
            }
        }

        /// <summary>
        /// 设置数据连接
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static void SetOperate(DataBaseOperate value,string name)
        {

            if (CommonMethods.IsWebContext)
            {
                System.Web.HttpContext.Current.Items[name] = value;
            }
            else
            {
                System.Runtime.Remoting.Messaging.CallContext.SetData(name, value);
            }
            
        }

        /// <summary>
        /// 获取数据库的静态连接
        /// </summary>
        /// <param name="db">数据库信息</param>
        /// <returns></returns>
        public static DataBaseOperate GetStaticOperate(DBInfo db) 
        {
            string key = SessionName+db.Name;
            DataBaseOperate oper = GetOperate(key) as DataBaseOperate;
            if (oper==null) 
            {
                oper = new DataBaseOperate(db, true);
#if DEBUG
                Debug.WriteLine("SQLCommon.NewConnection:DB=" + db.Name);
#endif
                SetOperate(oper,key);
            }
            return oper;
        }

        /// <summary>
        /// 获取此实体的默认连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataBaseOperate GetDefaultOperate<T>() 
        {
            return StaticConnection.GetStaticOperate(EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo);
        }

    }
}
