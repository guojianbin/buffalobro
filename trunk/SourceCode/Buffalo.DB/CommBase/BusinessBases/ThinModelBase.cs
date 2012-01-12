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
    /// 小型架构基类
    /// </summary>
    public class ThinModelBase<T>:EntityBase
        where T : EntityBase, new()
    {
        //private static DataAccessBase<T> GetBaseDal() = InitBaseDal();

        /// <summary>
        /// 获取执行语法的上下文
        /// </summary>
        /// <returns></returns>
        public static BQLDbBase GetContext() 
        {
            return GetBaseContext().ContextDAL;
        }


        /// <summary>
        /// 初始化数据层基类
        /// </summary>
        /// <returns></returns>
        public static DataAccessBase<T> GetBaseContext() 
        {
            DataAccessBase<T> baseDal = new DataAccessBase<T>();
            baseDal.Oper = StaticConnection.GetDefaultOperate<T>();
            
            return baseDal;
        }

        /// <summary>
        /// 当前实体的转换值
        /// </summary>
        private T GetThisValue()
        {
            return this as T;
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public static T GetEntityById(object id)
        {
            return GetBaseContext().GetObjectById(id);
        }
        /// <summary>
        /// 根据条件查找实体
        /// </summary>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public static T GetUnique(ScopeList lstScope)
        {
            return GetBaseContext().GetUnique(lstScope);
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static DBTransation StartTransation()
        {
            DataAccessBase<T> baseDal = GetBaseContext();

            return baseDal.Oper.StartTransation();
        }

        /// <summary>
        /// 开始非事务的批量动作
        /// </summary>
        /// <returns></returns>
        public static BatchAction StartBatchAction()
        {
            return GetBaseContext().Oper.StarBatchAction();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        public static DataSet Select(ScopeList lstScope)
        {
            return GetBaseContext().Select(lstScope);
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public static List<T> SelectList(ScopeList scpoeList)
        {
            return GetBaseContext().SelectList(scpoeList);
        }

        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public static long SelectCount(ScopeList scpoeList)
        {
            return GetBaseContext().SelectCount(scpoeList);
        }

        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public static bool ExistsRecord(ScopeList scpoeList)
        {
            return GetBaseContext().ExistsRecord(scpoeList);
        }

        #region 数据修改
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert() 
        {
            return Insert(true);
        }

        /// <summary>
        /// 保存实体并填充ID
        /// </summary>
        /// <param name="fillPrimaryKey">是否填充实体</param>
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
        /// 更新实体
        /// </summary>
        ///  <param name="optimisticConcurrency">是否并发控制</param>
        /// <returns></returns>
        public virtual int Update(bool optimisticConcurrency) 
        {
            object id = DataAccessBase<T>.CurEntityInfo.PrimaryProperty.GetValue(this);
            if (DefaultType.IsDefaultValue(id)) 
            {
                throw new Exception("更新必须要有主键");
            }
            return GetBaseContext().Update(GetThisValue(), null, optimisticConcurrency);
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
            EntityPropertyInfo pInfo=DataAccessBase<T>.CurEntityInfo.PrimaryProperty;
            object id = pInfo.GetValue(this);
            if (DefaultType.IsDefaultValue(id))
            {
                throw new Exception("删除必须要有主键");
            }
            ScopeList lstScope = new ScopeList();
            lstScope.AddEqual(pInfo.PropertyName, id);
            return GetBaseContext().Delete(null, lstScope, optimisticConcurrency);
        }

        /// <summary>
        /// 并发删除
        /// </summary>
        /// <returns></returns>
        public virtual int Delete()
        {
            return Delete(false);
        }

        /// <summary>
        /// 范围删除(慎用)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public static int DeleteByScope(ScopeList lstScope) 
        {
            return GetBaseContext().Delete(null, lstScope, false);
        }

        /// <summary>
        /// 范围更新(慎用)
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
