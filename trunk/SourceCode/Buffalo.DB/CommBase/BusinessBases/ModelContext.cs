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
    /// 模型层辅助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelContext<T> where T:EntityBase,new()
    {
        /// <summary>
        /// 模型层辅助类
        /// </summary>
        public ModelContext() 
        {

        }
        /// <summary>
        /// 获取数据层基类
        /// </summary>
        /// <returns></returns>
        public DataAccessBase<T> GetBaseContext()
        {
            DataAccessBase<T> baseDal = new DataAccessBase<T>();
            baseDal.Oper = StaticConnection.GetDefaultOperate<T>();
            return baseDal;
        }

        /// <summary>
        /// 获取执行语法的上下文
        /// </summary>
        /// <returns></returns>
        public BQLDbBase GetContext()
        {
            return GetBaseContext().ContextDAL;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public DBTransation StartTransation()
        {
            DataAccessBase<T> baseDal = GetBaseContext();

            return baseDal.Oper.StartTransation();
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        public DataSet Select(ScopeList lstScope)
        {
            return GetBaseContext().Select(lstScope);
        }


        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public long SelectCount(ScopeList scpoeList)
        {
            return GetBaseContext().SelectCount(scpoeList);
        }

        
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public List<T> SelectList(ScopeList scpoeList)
        {
            return GetBaseContext().SelectList(scpoeList);
        }
        /// <summary>
        /// 开始非事务的批量动作
        /// </summary>
        /// <returns></returns>
        public BatchAction StartBatchAction()
        {
            return GetBaseContext().Oper.StarBatchAction();
        }

        /// <summary>
        /// 当前实体的转换值
        /// </summary>
        private T GetThisValue()
        {
            return this as T;
        }

        /// <summary>
        /// 范围更新(慎用)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public int UpdateByScope(T obj, ScopeList lstScope)
        {
            return GetBaseContext().Update(obj, lstScope, false);
        }


        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public bool ExistsRecord(ScopeList scpoeList)
        {
            return GetBaseContext().ExistsRecord(scpoeList);
        }

        /// <summary>
        /// 范围删除(慎用)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public int DeleteByScope(ScopeList lstScope)
        {
            return GetBaseContext().Delete(null, lstScope, false);
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T GetEntityById(object id)
        {
            return GetBaseContext().GetObjectById(id);
        }
        /// <summary>
        /// 根据条件查找实体
        /// </summary>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            return GetBaseContext().GetUnique(lstScope);
        }

    }
}
