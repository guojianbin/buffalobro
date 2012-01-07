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
