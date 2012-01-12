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
        /// 更新和添加时候判断该对象是否已经存在
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object Exists(T entity) 
        {
            return null;
        }

        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(ScopeList lstScope) 
        {
            return null;
        }

        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(T entity)
        {
            return null;
        }
        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(object id)
        {
            return null;
        }
        /// <summary>
        /// 插入数据前的事件
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        /// <param name="oper">数据库连接对象</param>
        /// <returns>是否继续进行插入事件</returns>
        protected virtual object OnInserting(T entity) 
        {
            return null;
        }
        /// <summary>
        /// 插入数据后的事件
        /// </summary>
        /// <param name="oper">数据库连接对象</param>
        /// <param name="retValue">返回值</param>
        /// <returns></returns>
        protected virtual object OnInserted()
        {
            return null;
        }

        /// <summary>
        /// 更新数据前的事件
        /// </summary>
        /// <param name="entity">要更新的对象</param>
        /// <param name="lstScope">范围更新集合</param>
        /// <returns></returns>
        protected virtual object OnUpdateing(T entity, ScopeList lstScope) 
        {
            return null;
        }

        /// <summary>
        /// 插入数据后的事件
        /// </summary>
        /// <param name="oper">数据库连接对象</param>
        /// <param name="retValue">返回值</param>
        /// <returns></returns>
        protected virtual object OnUpdated()
        {
            return null;
        }

        /// <summary>
        /// 删除数据前的事件
        /// </summary>
        /// <param name="entity">要删除的对象</param>
        /// <param name="lstScope">范围删除集合</param>
        /// <param name="oper">数据库连接对象</param>
        /// <returns></returns>
        protected virtual object OnDeleteing(T entity)
        {
            return null;
        }
        /// <summary>
        /// 删除数据前的事件
        /// </summary>
        /// <param name="entity">要删除的对象</param>
        /// <param name="lstScope">范围删除集合</param>
        /// <param name="oper">数据库连接对象</param>
        /// <returns></returns>
        protected virtual object OnDeleteing(ScopeList lstScope)
        {
            return null;
        }

        /// <summary>
        /// 删除数据前的事件
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <param name="oper">数据库连接对象</param>
        /// <returns></returns>
        protected virtual object OnDeleteing(object id)
        {
            return null;
        }
        /// <summary>
        /// 删除数据后的事件
        /// </summary>
        /// <param name="oper">数据库连接对象</param>
        /// <param name="retValue">数据库连接对象</param>
        /// <returns></returns>
        protected virtual object OnDeleted()
        {
            return null;
        }

        protected int _affectedRows=-1;

        /// <summary>
        /// 此次执行的影响行数
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
        ///// 并发更新
        ///// </summary>
        ///// <param name="entity">对象</param>
        ///// <returns>大于0:更新完毕,小于0:更新失败</returns>
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
        ///// 批量并发更新
        ///// </summary>
        ///// <param name="entity">对象</param>
        ///// <returns>大于0:更新完毕,小于0:更新失败</returns>
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
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>大于0:更新完毕,小于0:更新失败</returns>
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
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表</param>
        /// <returns>大于0:更新完毕,小于0:更新失败</returns>
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
        /// 批量更新
        /// </summary>
        /// <param name="entity">对象</param>
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
        /// 批量更新
        /// </summary>
        /// <param name="lst">要更新的实体集合</param>
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
        /// 保存或更新操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public virtual object SaveOrUpdate(T entity)
        {
            object ret = null;
            Type type=typeof(T);
            EntityInfoHandle CurEntityInfo = EntityInfoManager.GetEntityHandle(type);//当前类型的信息
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
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
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
        /// 插入记录且填充记录自动增长的ID
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
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
        /// 插入一组数据
        /// </summary>
        /// <param name="lst">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
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
        /// 删除
        /// </summary>
        /// <param name="entity">对象</param>
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
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
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
        /// 删除
        /// </summary>
        /// <param name="lstScope">批量删除的条件集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
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
        /// 删除一组数据
        /// </summary>
        /// <param name="lst">数据集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
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