using System;
namespace Buffalobro.DB.CommBase.DataAccessBases
{

    public interface IDataAccessModel<T> : IViewDataAccess<T> 
     where T : SQLCommon.CommBase.EntityBase, new()
    {
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        int Delete(T entity);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        int Delete(SQLCommon.QueryConditions.ScopeList scopeList);

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        int DeleteById(object id);

        /// <summary>
        /// 插入记录且填充记录自动增长的ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        int IdentityInsert(T entity);

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        int Insert(T entity);

        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        /// <param name="scopeList">范围更新的集合</param>
        ///<returns></returns>
        int Update(T entity, SQLCommon.QueryConditions.ScopeList scopeList);
        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        ///<returns></returns>
        int Update(T entity);
    }
}
