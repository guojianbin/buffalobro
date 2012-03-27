using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon;

namespace Buffalo.DB.CommBase.BusinessBases
{
    public class BusinessModelBaseForSelect<T> where T : EntityBase, new()
    {
        protected readonly static EntityInfoHandle _curEntityInfo = EntityInfoManager.GetEntityHandle(typeof(T));

        protected readonly static DBInfo _db = _curEntityInfo.DBInfo;

        /// <summary>
        /// 创建数据连接实例
        /// </summary>
        /// <returns></returns>
        protected DataBaseOperate CreateOperateInstance() 
        {
            DataBaseOperate oper = _db.CreateOperate();
            return oper;
        }

        /// <summary>
        /// 执行查询之前触发的事件
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        protected virtual void OnSelect(ScopeList lstScope) 
        {
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
            _defaultOperate = _db.DefaultOperate;
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
            ScopeList lstScope = new ScopeList();
            PrimaryKeyInfo info = id as PrimaryKeyInfo;
            if (info == null)
            {
                lstScope.AddEqual(_curEntityInfo.PrimaryProperty[0].PropertyName, id);
            }
            else
            {
                info.FillScope(_curEntityInfo.PrimaryProperty, lstScope, true);
            }
            OnSelect(lstScope);
            return GetUnique(lstScope);
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
            OnSelect(lstScope);
            BQLDataAccessBase<T> dao = new BQLDataAccessBase<T>();
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
        public DataSet SelectTable(BQLOtherTableHandle table,  ScopeList lstScope)
        {
            OnSelect(lstScope);
            BQLDataAccessBase<T> dao = new BQLDataAccessBase<T>();
            return dao.SelectTable(table, lstScope);
        }
        
        /// <summary>
        /// 根据条件查找实体
        /// </summary>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            OnSelect(lstScope);
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
            OnSelect(lstScope);
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
            OnSelect(lstScope);
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
            OnSelect(scpoeList);
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
            OnSelect(scpoeList);
            DataAccessBaseForSelect<T> entityDao = new DataAccessBaseForSelect<T>();
            bool ret = false;
                ret = entityDao.ExistsRecord(scpoeList);
            return ret;
        }
        #endregion
    }
}
