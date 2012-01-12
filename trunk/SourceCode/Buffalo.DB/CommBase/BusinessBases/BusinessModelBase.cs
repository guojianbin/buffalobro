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
        /// <summary>
        /// ��������ǰ���¼�
        /// </summary>
        /// <param name="entity">Ҫ�����ʵ��</param>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <returns>�Ƿ�������в����¼�</returns>
        protected virtual object OnInserting(T entity) 
        {
            return null;
        }
        /// <summary>
        /// �������ݺ���¼�
        /// </summary>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <param name="retValue">����ֵ</param>
        /// <returns></returns>
        protected virtual object OnInserted()
        {
            return null;
        }

        /// <summary>
        /// ��������ǰ���¼�
        /// </summary>
        /// <param name="entity">Ҫ���µĶ���</param>
        /// <param name="lstScope">��Χ���¼���</param>
        /// <returns></returns>
        protected virtual object OnUpdateing(T entity, ScopeList lstScope) 
        {
            return null;
        }

        /// <summary>
        /// �������ݺ���¼�
        /// </summary>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <param name="retValue">����ֵ</param>
        /// <returns></returns>
        protected virtual object OnUpdated()
        {
            return null;
        }

        /// <summary>
        /// ɾ������ǰ���¼�
        /// </summary>
        /// <param name="entity">Ҫɾ���Ķ���</param>
        /// <param name="lstScope">��Χɾ������</param>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <returns></returns>
        protected virtual object OnDeleteing(T entity)
        {
            return null;
        }
        /// <summary>
        /// ɾ������ǰ���¼�
        /// </summary>
        /// <param name="entity">Ҫɾ���Ķ���</param>
        /// <param name="lstScope">��Χɾ������</param>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <returns></returns>
        protected virtual object OnDeleteing(ScopeList lstScope)
        {
            return null;
        }

        /// <summary>
        /// ɾ������ǰ���¼�
        /// </summary>
        /// <param name="id">Ҫɾ���ļ�¼ID</param>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <returns></returns>
        protected virtual object OnDeleteing(object id)
        {
            return null;
        }
        /// <summary>
        /// ɾ�����ݺ���¼�
        /// </summary>
        /// <param name="oper">���ݿ����Ӷ���</param>
        /// <param name="retValue">���ݿ����Ӷ���</param>
        /// <returns></returns>
        protected virtual object OnDeleted()
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

        #region ConcurrencyUpdate

        ///// <summary>
        ///// ��������
        ///// </summary>
        ///// <param name="entity">����</param>
        ///// <returns>����0:�������,С��0:����ʧ��</returns>
        //public virtual object ConcurrencyUpdate(T entity)
        //{
        //    DataAccessModel<T> entityDao = new DataAccessModel<T>();
        //    object ret = null;
        //    ret = Exists(entity);
        //    if (ret != null)
        //    {
        //        return ret;
        //    }

        //    ret = OnUpdateing(entity, null);
        //    if (ret != null)
        //    {
        //        return ret;
        //    }
        //    _affectedRows = entityDao.ConcurrencyUpdate(entity);
        //    ret = OnUpdated();
        //    return ret;

        //}

        

        ///// <summary>
        ///// ������������
        ///// </summary>
        ///// <param name="entity">����</param>
        ///// <returns>����0:�������,С��0:����ʧ��</returns>
        //public virtual object ConcurrencyUpdate(List<T> lst)
        //{

        //    DataAccessModel<T> entityDao = new DataAccessModel<T>();
        //    object ret = null;

        //    foreach (T entity in lst)
        //    {

        //        ret = Exists(entity);
        //        if (ret != null)
        //        {
        //            continue;
        //        }
        //        ret = OnUpdateing(entity, null);
        //        if (ret != null)
        //        {
        //            continue;
        //        }

        //        _affectedRows += entityDao.ConcurrencyUpdate(entity);
        //        ret = OnUpdated();

        //    }

        //    return ret;

        //}

        

        #endregion



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
            ret = OnUpdateing(entity, null);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.Update(entity);
            ret = OnUpdated();


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
            ret = OnUpdateing(entity, scorpList);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Update(entity, scorpList);
            ret = OnUpdated();


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
                ret = OnUpdateing(entity, null);
                if (ret != null)
                {
                    continue;
                }

                _affectedRows += entityDao.Update(entity);
                ret = OnUpdated();

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
                ret = OnUpdateing(entity, scorpList);
                if (ret != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Update(entity, scorpList);

                ret=OnUpdated();
            }

            return ret;
        }
        #endregion

        #region Insert

        /// <summary>
        /// �������²���
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public virtual object SaveOrUpdate(T entity)
        {
            object ret = null;
            Type type=typeof(T);
            EntityInfoHandle CurEntityInfo = EntityInfoManager.GetEntityHandle(type);//��ǰ���͵���Ϣ
            EntityPropertyInfo pkInfo=CurEntityInfo.PrimaryProperty;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object pkValue = pkInfo.GetValue(entity);
            if (!DefaultType.IsDefaultValue(pkValue))
            {
                ScopeList lstScope = new ScopeList();
                lstScope.AddEqual(pkInfo.PropertyName, pkValue);
                bool exists = entityDao.ExistsRecord(lstScope);
                if (exists) 
                {
                    ret = Update(entity);
                    return ret;
                }
            }
            
            ret = Insert(entity);
            return ret;
        }



        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(T entity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            ret = OnInserting(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Insert(entity);
            ret = OnInserted();

            return ret;
        }

        /// <summary>
        /// �����¼������¼�Զ�������ID
        /// </summary>
        /// <param name="entity">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object IdentityInsert(T entity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            ret = OnInserting(entity);
            if (ret != null)
            {
                return ret;
            }


            _affectedRows = entityDao.IdentityInsert(entity);
            ret = OnInserted();


            return ret;
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="lst">����</param>
        /// <returns>����0:�������,С��0:����ʧ��</returns>
        public virtual object Insert(List<T> lst)
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
                ret = OnInserting(entity);
                if (ret != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Insert(entity);

                ret = OnInserted();

            }
            return ret;
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

            ret = OnDeleteing(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Delete(entity);
            ret = OnDeleted();
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
            ret = OnDeleteing(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.DeleteById(id);
            ret = OnDeleted();

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

            ret = OnDeleteing(lstScope);
            if (ret != null)
            {
                return ret;
            }

            
            _affectedRows = entityDao.Delete(lstScope);
            ret = OnDeleted();
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

                res = OnDeleteing(entity);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Delete(entity);

                res = OnDeleted();

            }

            return res;
        }
        #endregion
    }
}