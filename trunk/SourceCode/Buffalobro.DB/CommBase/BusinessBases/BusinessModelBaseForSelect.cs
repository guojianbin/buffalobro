using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.CommBase.DataAccessBases;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon;

namespace Buffalobro.DB.CommBase.BusinessBases
{
    public class BusinessModelBaseForSelect<T> where T : EntityBase, new()
    {
        protected readonly static DBInfo _db = EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo;

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <returns></returns>
        protected DataBaseOperate CreateOperateInstance() 
        {
            DataBaseOperate oper = new DataBaseOperate(_db);
            return oper;
        }

        private DataBaseOperate _defaultOperate;

        /// <summary>
        /// ��ȡĬ������
        /// </summary>
        protected DataBaseOperate DefaultOperate 
        {
            get 
            {
                return _defaultOperate;
            }
        }
        /// <summary>
        /// ҵ���Ĳ�ѯ����
        /// </summary>
        public BusinessModelBaseForSelect()
        {
            _defaultOperate = StaticConnection.GetStaticOperate(_db);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        protected DBTransation StartTransation() 
        {

            return DefaultOperate.StartTransation() ;
        }

        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        protected BatchAction StartBatchAction()
        {

            return DefaultOperate.StarBatchAction();
        }

        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public T GetEntityById(object id)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            T ret = null;
            //try
            //{
                ret=entityDao.GetEntityById(id);
            //}
            //finally
            //{
            //    oper.CloseDataBase();
            //}
            return ret;
        }



        /// <summary>
        /// ֱ�Ӳ�ѯ���ݿ���ͼ
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="lstScope">����</param>
        /// <param name="vParams">�ֶ��б�</param>
        /// <param name="lstSort">��������</param>
        /// <param name="lstSort">����</param>
        /// <returns></returns>
        public DataSet SelectTable(string tableName,  ScopeList lstScope)
        {
            CsqlDataAccessBase<T> dao = new CsqlDataAccessBase<T>();
            return dao.SelectTable(tableName, lstScope);
        }

        /// <summary>
        /// ��ѯ����������ͼ
        /// </summary>
        /// <param name="table"></param>
        /// <param name="vParams"></param>
        /// <param name="lstScope"></param>
        /// <param name="objPage"></param>
        /// <returns></returns>
        public DataSet SelectTable(CsqlOtherTableHandle table,  ScopeList lstScope)
        {
            CsqlDataAccessBase<T> dao = new CsqlDataAccessBase<T>();
            return dao.SelectTable(table, lstScope);
        }
        
        /// <summary>
        /// ������������ʵ��
        /// </summary>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            T ret = null;
                ret = entityDao.GetUnique(lstScope);
            return ret;
        }
        #region SelectByAll
        

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <param name="lstSort">������������</param>
        /// <returns></returns>
        public virtual DataSet Select(ScopeList lstScope)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            DataSet ret = null;
                ret = entityDao.Select(lstScope);
            return ret;
        }
        
        
        /// <summary>
        /// ����(���ؼ���)
        /// </summary>
        /// <param name="lstScope">��Χ����</param>
        /// <returns></returns>
        public virtual List<T> SelectList(ScopeList lstScope)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            List<T> ret = null;
                ret = entityDao.SelectList(lstScope);
            return ret;
        }
        #endregion

        #region SelectCount
        
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual long SelectCount(ScopeList scpoeList)
        {
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            long ret = 0;
                ret = entityDao.SelectCount(scpoeList);
            return ret;
        }
        #endregion

        #region SelectExists
        
        
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scpoeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public virtual bool ExistsRecord(ScopeList scpoeList)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            bool ret = false;
                ret = entityDao.ExistsRecord(scpoeList);
            return ret;
        }
        #endregion
    }
}
