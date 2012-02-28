using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.QueryConditions;
using System.Data;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// ģ�Ͳ㸨����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelContext<T> where T:EntityBase,new()
    {
        /// <summary>
        /// ģ�Ͳ㸨����
        /// </summary>
        public ModelContext() 
        {

        }
        /// <summary>
        /// ��ȡ���ݲ����
        /// </summary>
        /// <returns></returns>
        public DataAccessBase<T> GetBaseContext()
        {
            DataAccessBase<T> baseDal = new DataAccessBase<T>();
            baseDal.Oper = StaticConnection.GetDefaultOperate<T>();
            return baseDal;
        }

        /// <summary>
        /// ��ȡִ���﷨��������
        /// </summary>
        /// <returns></returns>
        public BQLDbBase GetContext()
        {
            return GetBaseContext().ContextDAL;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DBTransation StartTransation()
        {
            DataAccessBase<T> baseDal = GetBaseContext();

            return baseDal.Oper.StartTransation();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <returns></returns>
        public DataSet Select(ScopeList lstScope)
        {
            return GetBaseContext().Select(lstScope);
        }


        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public long SelectCount(ScopeList scpoeList)
        {
            return GetBaseContext().SelectCount(scpoeList);
        }

        
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public List<T> SelectList(ScopeList scpoeList)
        {
            return GetBaseContext().SelectList(scpoeList);
        }
        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        public BatchAction StartBatchAction()
        {
            return GetBaseContext().Oper.StarBatchAction();
        }

        /// <summary>
        /// ��ǰʵ���ת��ֵ
        /// </summary>
        private T GetThisValue()
        {
            return this as T;
        }

        /// <summary>
        /// ��Χ����(����)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public int UpdateByScope(T obj, ScopeList lstScope)
        {
            return GetBaseContext().Update(obj, lstScope, false);
        }


        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public bool ExistsRecord(ScopeList scpoeList)
        {
            return GetBaseContext().ExistsRecord(scpoeList);
        }

        /// <summary>
        /// ��Χɾ��(����)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public int DeleteByScope(ScopeList lstScope)
        {
            return GetBaseContext().Delete(null, lstScope, false);
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public T GetEntityById(object id)
        {
            return GetBaseContext().GetObjectById(id);
        }
        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            return GetBaseContext().GetUnique(lstScope);
        }

    }
}
