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
        /// ��ȡ���ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        public static DBInfo GetDBinfo()
        {
            return _db;
        }

        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        private static bool _isInit=false;

        /// <summary>
        /// ��ʼ�����ݿ�
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
        /// Ĭ������
        /// </summary>
        public static DataBaseOperate DefaultOperate 
        {
            get 
            {
                return _db.DefaultOperate;
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
        protected static BQLEntityTableHandle AddToDB(BQLEntityTableHandle table) 
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
