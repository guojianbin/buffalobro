using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
{
    
    public class CsqlDataBaseHandle<T>
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
        /// 默认连接
        /// </summary>
        public static DataBaseOperate DefaultOperate 
        {
            get 
            {
                return StaticConnection.GetStaticOperate(_db);
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
        protected static CsqlEntityTableHandle AddToDB(CsqlEntityTableHandle table) 
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
