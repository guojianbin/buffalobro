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
    /// ���ݷ��ʲ�ģ��
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
        /////�����޸ļ�¼
        /////</summary>
        /////<param name="entity">ʵ��</param>
        /////<returns></returns>
        //public int ConcurrencyUpdate(T entity)
        //{
        //    return base.Update(entity, null, true);
        //}
        
        

        ///<summary>
        ///�޸ļ�¼
        ///</summary>
        ///<param name="entity">ʵ��</param>
        ///<returns></returns>
        public int Update(T entity)
        {
            return base.Update(entity, null, true);
        }
        ///<summary>
        ///�޸ļ�¼
        ///</summary>
        ///<param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χ���µļ���</param>
        ///<returns></returns>
        public int Update(T entity,ScopeList scopeList)
        {
            return base.Update(entity, scopeList,false);
        }
        /// <summary>
        /// �����¼
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <returns></returns>
        public new int Insert(T entity)
        {
            return base.Insert(entity);
        }

        /// <summary>
        /// �����¼������¼�Զ�������ID
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <returns></returns>
        public new int IdentityInsert(T entity)
        {
            return base.IdentityInsert(entity);
        }
        ///// <summary>
        ///// �����¼
        ///// </summary>
        ///// <param name="entity">ʵ��</param>
        ///// <param name="identity">������ֵ</param>
        ///// <returns></returns>
        //public new int Insert(T entity, out object identity)
        //{
        //    return base.Insert(entity, out identity);
        //}
        /// <summary>
        /// ɾ����¼
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χɾ���ļ���</param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            return base.Delete(entity, null,true);
        }

        ///// <summary>
        ///// ����ɾ��
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public int ConcurrencyDelete(T entity)
        //{
        //    return base.Delete(entity, null, true);
        //}
        /// <summary>
        /// ��Χɾ����¼(����)
        /// </summary>
        /// <param name="entity">ʵ��</param>
        /// <param name="scopeList">��Χɾ���ļ���</param>
        /// <returns></returns>
        public int Delete( ScopeList scopeList)
        {
            return base.Delete(default(T),scopeList,false);
        }

        /// <summary>
        /// ͨ��IDɾ����¼(����)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new int DeleteById(object id) 
        {
            return base.DeleteById(id);
        }
    }
}
