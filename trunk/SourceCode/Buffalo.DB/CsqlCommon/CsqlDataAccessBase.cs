using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CsqlCommon.CsqlBaseFunction;
using Buffalo.DB.CsqlCommon.CsqlExtendFunction;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataFillers;
using Buffalo.DB.CsqlCommon.CsqlConditions;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CsqlCommon.CsqlAggregateFunctions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalo.DB.CsqlCommon
{
    /// <summary>
    /// Csql数据访问层的基类
    /// </summary>
    public class CsqlDataAccessBase<T> : CsqlDbBase
        where T : EntityBase, new()
    {
        /// <summary>
        /// 数据层基类
        /// </summary>
        public CsqlDataAccessBase()
            : base(typeof(T))
        {
        }
        /// <summary>
        /// 数据层基类
        /// </summary>
        /// <param name="oper"></param>
        public CsqlDataAccessBase(DataBaseOperate oper)
            : base(oper)
        {

        }
        
        /// <summary>
        /// 获取第一条记录
        /// </summary>
        /// <param name="csql"></param>
        /// <returns></returns>
        public virtual T GetUnique(CsqlQuery csql) 
        {
            return this.GetUnique<T>(csql);
        }

        /// <summary>
        /// 执行sql语句，分页返回List
        /// </summary>
        /// <param name="csql">Csql</param>
        /// <param name="objPage">分页数据</param>
        /// <returns></returns>
        public virtual List<T> QueryPageList(CsqlQuery csql, PageContent objPage)
        {
            return QueryPageList<T>(csql, objPage, null);
        }
        /// <summary>
        /// 执行sql语句，返回List
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="csql">Csql</param>
        /// <returns></returns>
        public virtual List<T> QueryList(CsqlQuery csql)
        {
            return QueryList<T>(csql, null);
        }

        
    }
}
