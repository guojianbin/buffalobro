using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.CommBase.DataAccessBases;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon;

namespace Buffalobro.DB.CommBase.BusinessBases
{
    public class BusinessModelBaseForSelect<T> where T : EntityBase, new()
    {
        protected readonly static DBInfo _db = EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo;

        /// <summary>
        /// 创建数据连接实例
        /// </summary>
        /// <returns></returns>
        protected DataBaseOperate CreateOperateInstance() 
        {
            DataBaseOperate oper = new DataBaseOperate(_db);
            return oper;
        }

        private DataBaseOperate _defaultOperate;

        /// <summary>
        /// 获取默认连接
        /// </summary>
        protected DataBaseOperate DefaultOperate 
        {
            get 
            {
                return _defaultOperate;
            }
        }
        /// <summary>
        /// 业务层的查询集合
        /// </summary>
        public BusinessModelBaseForSelect()
        {
            _defaultOperate = StaticConnection.GetStaticOperate(_db);
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        protected DBTransation StartTransation() 
        {

            return DefaultOperate.StartTransation() ;
        }

        /// <summary>
        /// 开始非事务的批量动作
        /// </summary>
        /// <returns></returns>
        protected BatchAction StartBatchAction()
        {

            return DefaultOperate.StarBatchAction();
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T GetEntityById(object id)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            T ret = null;
            //try
            //{
                ret=entityDao.GetEntityById(id);
            //}
            //finally
            //{
            //    oper.CloseDataBase();
            //}
            return ret;
        }



        /// <summary>
        /// 直接查询数据库视图
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="lstScope">条件</param>
        /// <param name="vParams">字段列表</param>
        /// <param name="lstSort">排序类型</param>
        /// <param name="lstSort">排序</param>
        /// <returns></returns>
        public DataSet SelectTable(string tableName,  ScopeList lstScope)
        {
            CsqlDataAccessBase<T> dao = new CsqlDataAccessBase<T>();
            return dao.SelectTable(tableName, lstScope);
        }

        /// <summary>
        /// 查询特殊表或者视图
        /// </summary>
        /// <param name="table"></param>
        /// <param name="vParams"></param>
        /// <param name="lstScope"></param>
        /// <param name="objPage"></param>
        /// <returns></returns>
        public DataSet SelectTable(CsqlOtherTableHandle table,  ScopeList lstScope)
        {
            CsqlDataAccessBase<T> dao = new CsqlDataAccessBase<T>();
            return dao.SelectTable(table, lstScope);
        }
        
        /// <summary>
        /// 根据条件查找实体
        /// </summary>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            T ret = null;
                ret = entityDao.GetUnique(lstScope);
            return ret;
        }
        #region SelectByAll
        

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <param name="lstSort">排序条件集合</param>
        /// <returns></returns>
        public virtual DataSet Select(ScopeList lstScope)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            DataSet ret = null;
                ret = entityDao.Select(lstScope);
            return ret;
        }
        
        
        /// <summary>
        /// 查找(返回集合)
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        public virtual List<T> SelectList(ScopeList lstScope)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            List<T> ret = null;
                ret = entityDao.SelectList(lstScope);
            return ret;
        }
        #endregion

        #region SelectCount
        
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public virtual long SelectCount(ScopeList scpoeList)
        {
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            long ret = 0;
                ret = entityDao.SelectCount(scpoeList);
            return ret;
        }
        #endregion

        #region SelectExists
        
        
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public virtual bool ExistsRecord(ScopeList scpoeList)
        {
            //DataBaseOperate oper = new DataBaseOperate(_db);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            bool ret = false;
                ret = entityDao.ExistsRecord(scpoeList);
            return ret;
        }
        #endregion
    }
}
