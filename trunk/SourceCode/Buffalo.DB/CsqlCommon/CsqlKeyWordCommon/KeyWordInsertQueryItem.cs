using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.DB.CsqlCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using System.Data;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// 插入查询项
    /// </summary>
    public class KeyWordInsertQueryItem : CsqlQuery
    {
        private CsqlAliasHandle _query;
        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="valueHandles">值的集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordInsertQueryItem(CsqlQuery query, CsqlQuery previous)
            : base(previous) 
        {
            this._query = new CsqlAliasHandle(query,null);
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            _query.FillInfo(info);
        }
        
        
        internal override void Tran(KeyWordInfomation info)
        {
            info.Condition.Condition.Append(_query.DisplayValue(info));
        }
    }
}
