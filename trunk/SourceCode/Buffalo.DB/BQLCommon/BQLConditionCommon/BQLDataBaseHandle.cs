using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.Kernel.FastReflection;
using System.Reflection;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    
    public class BQLDataBaseHandle<T>
    {
        private static DBInfo _db = GetDB();

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        /// <returns></returns>
        public static DBInfo GetDBinfo()
        {
            return _db;
        }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool _isInit=false;

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitDB() 
        {
            //if (_isInit) 
            //{
            //    return;
            //}
            Type type = typeof(T);
            Type baseType=typeof(BQLEntityTableHandle);
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo info in infos) 
            {
                Type objType = info.PropertyType;
                if (!objType.IsSubclassOf(baseType)) 
                {
                    continue;
                }
                AddToDB(FastValueGetSet.GetGetMethodInfo(info.Name, type).Invoke(null, new object[] { }) as BQLEntityTableHandle);
            }
            DataAccessLoader.InitConfig();
        }

        /// <summary>
        /// 默认连接
        /// </summary>
        public static DataBaseOperate DefaultOperate 
        {
            get 
            {
                return _db.DefaultOperate;
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static DBTransation StartTransation()
        {

            return DefaultOperate.StartTransation() ;
        }
        /// <summary>
        /// 开始非事务的批量动作
        /// </summary>
        /// <returns></returns>
        public static BatchAction StartBatchAction()
        {

            return DefaultOperate.StarBatchAction();
        }

        /// <summary>
        /// 添加到库信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected static BQLEntityTableHandle AddToDB(BQLEntityTableHandle table) 
        {
            _db.AddToDB(table);
            return table;
        }

        /// <summary>
        /// 创新数据库连接
        /// </summary>
        /// <returns></returns>
        public static DataBaseOperate CreateOperate() 
        {
            DataBaseOperate oper = _db.CreateOperate();
            return oper;
        }

        

        private static DBInfo GetDB() 
        {
            Type cType=typeof(T);
            DataBaseAttribute att=FastInvoke.GetClassAttribute<DataBaseAttribute>(cType);
            if(att==null)
            {
                throw new Exception(cType.FullName+"类还没配置DataBaseAttribute标签");
            }
            string dbName=att.DataBaseName;
            DataAccessLoader.InitConfig();
            return DataAccessLoader.GetDBInfo(dbName);
        }
    }
}
