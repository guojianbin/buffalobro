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
        /// ��ȡ���ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        public static DBInfo GetDBinfo()
        {
            
            return _db;
            
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public static DataBaseOperate DefaultOperate 
        {
            get 
            {
                return StaticConnection.GetStaticOperate(_db);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public static DBTransation StartTransation()
        {

            return DefaultOperate.StartTransation() ;
        }
        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        public static BatchAction StartBatchAction()
        {

            return DefaultOperate.StarBatchAction();
        }

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected static CsqlEntityTableHandle AddToDB(CsqlEntityTableHandle table) 
        {
            _db.AddToDB(table);
            return table;
        }

        /// <summary>
        /// �������ݿ�����
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
                throw new Exception(cType.FullName+"�໹û����DataBaseAttribute��ǩ");
            }
            string dbName=att.DataBaseName;
            DataAccessLoader.InitConfig();
            return DataAccessLoader.GetDBInfo(dbName);
        }
    }
}
