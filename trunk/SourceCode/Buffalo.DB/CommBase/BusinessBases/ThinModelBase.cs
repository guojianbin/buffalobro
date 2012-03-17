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
    /// 小型架构基类
    /// </summary>
    public class ThinModelBase:EntityBase
    {
        private DataAccessSetBase _dal;

        /// <summary>
        /// 获取基础数据层
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

        #region 数据修改
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Insert() 
        {
            return Insert(true);
        }

        /// <summary>
        /// 查询时候触发
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public virtual void OnSelect(ScopeList lstScope) 
        {
            
        }
        
        /// <summary>
        /// 保存实体并填充ID
        /// </summary>
        /// <param name="fillPrimaryKey">是否填充实体</param>
        /// <returns></returns>
        public virtual int Insert(bool fillPrimaryKey)
        {
            DataAccessSetBase dal = GetBaseDataAccess();

            return dal.DoInsert(this, fillPrimaryKey);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        ///  <param name="optimisticConcurrency">是否并发控制</param>
        /// <returns></returns>
        public virtual int Update(bool optimisticConcurrency) 
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            foreach (EntityPropertyInfo epPk in dal.EntityInfo.PrimaryProperty)
            {
                object id = epPk.GetValue(this);
                if (DefaultType.IsDefaultValue(id))
                {
                    throw new Exception("主键:"+epPk.PropertyName+"的值不能为空");
                }
            }
            return dal.Update(this, null, optimisticConcurrency);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <returns></returns>
        public virtual int Update()
        {
            return Update(false);
        }

        /// <summary>
        /// 并发删除
        /// </summary>
        /// <param name="optimisticConcurrency">是否并发控制</param>
        /// <returns></returns>
        public virtual int Delete(bool optimisticConcurrency) 
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            ScopeList lstScope = new ScopeList();
            foreach (EntityPropertyInfo pInfo in dal.EntityInfo.PrimaryProperty)
            {
                object id = pInfo.GetValue(this);
                if (DefaultType.IsDefaultValue(id))
                {
                    throw new Exception("主键:" + pInfo.PropertyName + "的值不能为空");
                }
                lstScope.AddEqual(pInfo.PropertyName, id);
            }
            
            
            return dal.Delete(null, lstScope, optimisticConcurrency);
        }

        /// <summary>
        /// 并发删除
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            return Delete(false);
        }


        
        #endregion
    }
}
