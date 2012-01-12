using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// С�ͼܹ�����
    /// </summary>
    public class ThinModelBase<T>:EntityBase
        where T : EntityBase, new()
    {
        //private static DataAccessBase<T> GetBaseDal() = InitBaseDal();

        /// <summary>
        /// ��ȡִ���﷨��������
        /// </summary>
        /// <returns></returns>
        public static BQLDbBase GetContext() 
        {
            return GetBaseContext().ContextDAL;
        }


        /// <summary>
        /// ��ʼ�����ݲ����
        /// </summary>
        /// <returns></returns>
        public static DataAccessBase<T> GetBaseContext() 
        {
            DataAccessBase<T> baseDal = new DataAccessBase<T>();
            baseDal.Oper = StaticConnection.GetDefaultOperate<T>();
            
            return baseDal;
        }

        /// <summary>
        /// ��ǰʵ���ת��ֵ
        /// </summary>
        private T GetThisValue()
        {
            return this as T;
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public static T GetEntityById(object id)
        {
            return GetBaseContext().GetObjectById(id);
        }
        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public static T GetUnique(ScopeList lstScope)
        {
            return GetBaseContext().GetUnique(lstScope);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public static DBTransation StartTransation()
        {
            DataAccessBase<T> baseDal = GetBaseContext();

            return baseDal.Oper.StartTransation();
        }

        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        public static BatchAction StartBatchAction()
        {
            return GetBaseContext().Oper.StarBatchAction();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <returns></returns>
        public static DataSet Select(ScopeList lstScope)
        {
            return GetBaseContext().Select(lstScope);
        }
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public static List<T> SelectList(ScopeList scpoeList)
        {
            return GetBaseContext().SelectList(scpoeList);
        }

        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public static long SelectCount(ScopeList scpoeList)
        {
            return GetBaseContext().SelectCount(scpoeList);
        }

        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public static bool ExistsRecord(ScopeList scpoeList)
        {
            return GetBaseContext().ExistsRecord(scpoeList);
        }

        #region �����޸�
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert() 
        {
            return Insert(true);
        }

        /// <summary>
        /// ����ʵ�岢���ID
        /// </summary>
        /// <param name="fillPrimaryKey">�Ƿ����ʵ��</param>
        /// <returns></returns>
        public virtual int Insert(bool fillPrimaryKey)
        {
            DataAccessBase<T> baseDal = GetBaseContext();
            if (fillPrimaryKey)
            {
                return baseDal.IdentityInsert(GetThisValue());
            }
            return baseDal.Insert(GetThisValue());
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        ///  <param name="optimisticConcurrency">�Ƿ񲢷�����</param>
        /// <returns></returns>
        public virtual int Update(bool optimisticConcurrency) 
        {
            object id = DataAccessBase<T>.CurEntityInfo.PrimaryProperty.GetValue(this);
            if (DefaultType.IsDefaultValue(id)) 
            {
                throw new Exception("���±���Ҫ������");
            }
            return GetBaseContext().Update(GetThisValue(), null, optimisticConcurrency);
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <returns></returns>
        public virtual int Update()
        {
            return Update(false);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="optimisticConcurrency">�Ƿ񲢷�����</param>
        /// <returns></returns>
        public virtual int Delete(bool optimisticConcurrency) 
        {
            EntityPropertyInfo pInfo=DataAccessBase<T>.CurEntityInfo.PrimaryProperty;
            object id = pInfo.GetValue(this);
            if (DefaultType.IsDefaultValue(id))
            {
                throw new Exception("ɾ������Ҫ������");
            }
            ScopeList lstScope = new ScopeList();
            lstScope.AddEqual(pInfo.PropertyName, id);
            return GetBaseContext().Delete(null, lstScope, optimisticConcurrency);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            return Delete(false);
        }

        /// <summary>
        /// ��Χɾ��(����)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public static int DeleteByScope(ScopeList lstScope) 
        {
            return GetBaseContext().Delete(null, lstScope, false);
        }

        /// <summary>
        /// ��Χ����(����)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public static int UpdateByScope(T obj,ScopeList lstScope)
        {
            return GetBaseContext().Update(obj, lstScope, false);
        }
        #endregion
    }
}
