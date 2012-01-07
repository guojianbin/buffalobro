using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.CsqlCommon.CsqlBaseFunction;
using Buffalobro.DB.CsqlCommon.CsqlExtendFunction;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.DbCommon;
using System.Data;
using Buffalobro.DB.DataFillers;
using Buffalobro.DB.CsqlCommon.CsqlConditions;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.CsqlCommon.CsqlAggregateFunctions;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.CommBase;
using Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalobro.DB.CsqlCommon
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
