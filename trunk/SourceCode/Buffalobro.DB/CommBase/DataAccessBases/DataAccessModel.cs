using System;
using System.Data;
using System.Data.SqlClient;
using SQLCommon;
using Buffalobro.DB.CommBase;
using System.Collections.Generic;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;

namespace Buffalobro.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// 数据访问层模版
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAccessModel<T> :DataAccessBaseForSelect<T>, IDataAccessModel<T> where T : EntityBase, new()
    {
        public DataAccessModel(DataBaseOperate oper):base(oper)
        {
            
        }
        public DataAccessModel():base()
        {
            
        }

        /////<summary>
        /////并发修改记录
        /////</summary>
        /////<param name="entity">实体</param>
        /////<returns></returns>
        //public int ConcurrencyUpdate(T entity)
        //{
        //    return base.Update(entity, null, true);
        //}
        
        

        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        ///<returns></returns>
        public int Update(T entity)
        {
            return base.Update(entity, null, true);
        }
        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        /// <param name="scopeList">范围更新的集合</param>
        ///<returns></returns>
        public int Update(T entity,ScopeList scopeList)
        {
            return base.Update(entity, scopeList,false);
        }
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public new int Insert(T entity)
        {
            return base.Insert(entity);
        }

        /// <summary>
        /// 插入记录且填充记录自动增长的ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public new int IdentityInsert(T entity)
        {
            return base.IdentityInsert(entity);
        }
        ///// <summary>
        ///// 插入记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="identity">自增长值</param>
        ///// <returns></returns>
        //public new int Insert(T entity, out object identity)
        //{
        //    return base.Insert(entity, out identity);
        //}
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            return base.Delete(entity, null,true);
        }

        ///// <summary>
        ///// 并发删除
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public int ConcurrencyDelete(T entity)
        //{
        //    return base.Delete(entity, null, true);
        //}
        /// <summary>
        /// 范围删除记录(慎用)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        public int Delete( ScopeList scopeList)
        {
            return base.Delete(default(T),scopeList,false);
        }

        /// <summary>
        /// 通过ID删除记录(慎用)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new int DeleteById(object id) 
        {
            return base.DeleteById(id);
        }
    }
}
