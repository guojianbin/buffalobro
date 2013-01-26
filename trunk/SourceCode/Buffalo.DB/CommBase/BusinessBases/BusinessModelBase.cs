using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.CacheManager;

namespace Buffalo.DB.CommBase.BusinessBases
{
    public abstract class BusinessModelBase<T> : BusinessModelBaseForSelect<T>
        where T : EntityBase, new()
    {
        public BusinessModelBase()
		{
			
		}
        /// <summary>
        /// ���º����ʱ���жϸö����Ƿ��Ѿ�����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object Exists(T entity) 
        {
            return null;
        }

        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object HasChild(ScopeList lstScope) 
        {
            return null;
        }

        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object HasChild(T entity)
        {
            return null;
        }
        /// <summary>
        /// ɾ��ʱ���жϸö����Ƿ񻹴����Ӽ�¼
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="oper">�������Ӷ���</param>
        /// <returns>û�д��ھͷ���null�����򷵻�һ��ֵ</returns>
        protected virtual object HasChild(object id)
        {
            return null;
        }
        

        protected int _affectedRows=-1;

        /// <summary>
        /// �˴�ִ�е�Ӱ������
        /// </summary>
        public int AffectedRows
        {
            get
            {
                return _affectedRows;
            }
        }

        



        #region Update
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Update(T entity)
        {


            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.Update(entity);


            return ret;

        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="scorpList">��Χ���µ��б�</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Update(T entity, ScopeList scorpList)
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Update(entity, scorpList);


            return ret;

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="scorpList">��Χ���µ��б�</param>
        /// <param name="lstValue">setʵ��</param>
        /// <param name="optimisticConcurrency">��������</param>
        /// <returns></returns>
        public virtual object Update(T entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Update(entity, scorpList, lstValue, optimisticConcurrency);


            return ret;

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">����</param>
        /// <param name="scorpList">��Χ���µ��б�</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Update(T entity, ScopeList scorpList, ValueSetList lstValue)
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Update(entity, scorpList, lstValue, false);


            return ret;

        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns></returns>
        public virtual object Update(List<T> lst)
        {


            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            foreach (T entity in lst)
            {


                ret = Exists(entity);
                if (ret != null)
                {
                    continue;
                }

                _affectedRows += entityDao.Update(entity);

            }

            return ret;

        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="lst">Ҫ���µ�ʵ�弯��</param>
        /// <param name="scorpList"></param>
        /// <returns></returns>
        public virtual object Update(List<T> lst, ScopeList scorpList)
        {


            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            foreach (T entity in lst)
            {

                ret = Exists(entity);
                if (ret != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Update(entity, scorpList);
            }

            return ret;
        }
        #endregion

        #region Insert

        

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(T entity)
        {

            return Insert(entity,false);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(T entity,bool fillIdentity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Insert(entity, fillIdentity);

            return ret;
        }
        /// ����һ������
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(T entity,ValueSetList setList, bool fillIdentity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Insert(entity, setList, fillIdentity);

            return ret;
        }
        
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="lst">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(List<T> lst, bool fillIdentity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            foreach (T entity in lst)
            {

                ret = Exists(entity);

                if (ret != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Insert(entity, fillIdentity);

            }
            return ret;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="lst">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(List<T> lst)
        {
            return Insert(lst, true);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns></returns>
        public virtual object Delete(T entity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChild(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Delete(entity);
            return ret;
        }

        

        /// <summary>
        /// ����IDɾ����¼
        /// </summary>
        /// <param name="id">Ҫɾ���ļ�¼ID</param>
        /// <returns></returns>
        public virtual object DeleteById(object id)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChild(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.DeleteById(id);

            return ret;
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="lstScope">����ɾ������������</param>
        /// <returns>����0:ɾ�����,С��0:ɾ��ʧ��</returns>
        public virtual object Delete(ScopeList lstScope)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();


            object ret = HasChild(lstScope);
            if (ret != null)
            {
                return ret;
            }

            
            _affectedRows = entityDao.Delete(lstScope);
            return ret;
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="lst">���ݼ���</param>
        /// <returns>����0:ɾ�����,С��0:ɾ��ʧ��</returns>
        public virtual object Delete(List<T> lst)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object res = null;
            foreach (T entity in lst)
            {
                res = HasChild(entity);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Delete(entity);


            }

            return res;
        }
        #endregion
    }
}