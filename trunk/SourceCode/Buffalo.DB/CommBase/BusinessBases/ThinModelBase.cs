using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// С�ͼܹ�����
    /// </summary>
    public class ThinModelBase:EntityBase
    {
        private DataAccessSetBase _dal;

        /// <summary>
        /// ��ȡ�������ݲ�
        /// </summary>
        /// <returns></returns>
        private DataAccessSetBase GetBaseDataAccess() 
        {
            if (_dal == null) 
            {
                EntityInfoHandle handle=EntityInfoManager.GetEntityHandle(this.GetType());
                _dal = new DataAccessSetBase(handle);
                _dal.Oper = StaticConnection.GetStaticOperate(handle.DBInfo);
            }
            return _dal;
        }

        #region �����޸�
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Insert() 
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
            DataAccessSetBase dal = GetBaseDataAccess();

            return dal.DoInsert(this, fillPrimaryKey);
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        ///  <param name="optimisticConcurrency">�Ƿ񲢷�����</param>
        /// <returns></returns>
        public virtual int Update(bool optimisticConcurrency) 
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            object id = dal.EntityInfo.PrimaryProperty.GetValue(this);
            if (DefaultType.IsDefaultValue(id)) 
            {
                throw new Exception("���±���Ҫ������");
            }
            return dal.Update(this, null, optimisticConcurrency);
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
            DataAccessSetBase dal = GetBaseDataAccess();
            EntityPropertyInfo pInfo=dal.EntityInfo.PrimaryProperty;
            object id = pInfo.GetValue(this);
            if (DefaultType.IsDefaultValue(id))
            {
                throw new Exception("ɾ������Ҫ������");
            }
            ScopeList lstScope = new ScopeList();
            lstScope.AddEqual(pInfo.PropertyName, id);
            return dal.Delete(null, lstScope, optimisticConcurrency);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            return Delete(false);
        }


        
        #endregion
    }
}
