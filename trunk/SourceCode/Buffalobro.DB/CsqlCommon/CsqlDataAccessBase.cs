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
    /// Csql���ݷ��ʲ�Ļ���
    /// </summary>
    public class CsqlDataAccessBase<T> : CsqlDbBase
        where T : EntityBase, new()
    {
        /// <summary>
        /// ���ݲ����
        /// </summary>
        public CsqlDataAccessBase()
            : base(typeof(T))
        {
        }
        /// <summary>
        /// ���ݲ����
        /// </summary>
        /// <param name="oper"></param>
        public CsqlDataAccessBase(DataBaseOperate oper)
            : base(oper)
        {

        }
        
        /// <summary>
        /// ��ȡ��һ����¼
        /// </summary>
        /// <param name="csql"></param>
        /// <returns></returns>
        public virtual T GetUnique(CsqlQuery csql) 
        {
            return this.GetUnique<T>(csql);
        }

        /// <summary>
        /// ִ��sql��䣬��ҳ����List
        /// </summary>
        /// <param name="csql">Csql</param>
        /// <param name="objPage">��ҳ����</param>
        /// <returns></returns>
        public virtual List<T> QueryPageList(CsqlQuery csql, PageContent objPage)
        {
            return QueryPageList<T>(csql, objPage, null);
        }
        /// <summary>
        /// ִ��sql��䣬����List
        /// </summary>
        /// <typeparam name="E">ʵ������</typeparam>
        /// <param name="csql">Csql</param>
        /// <returns></returns>
        public virtual List<T> QueryList(CsqlQuery csql)
        {
            return QueryList<T>(csql, null);
        }

        
    }
}
