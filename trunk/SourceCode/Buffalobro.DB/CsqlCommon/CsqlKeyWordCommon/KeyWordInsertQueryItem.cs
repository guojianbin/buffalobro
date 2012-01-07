using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon.IdentityInfos;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;
using System.Data;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// �����ѯ��
    /// </summary>
    public class KeyWordInsertQueryItem : CsqlQuery
    {
        private CsqlAliasHandle _query;
        /// <summary>
        /// Insert���ֶιؼ�����
        /// </summary>
        /// <param name="valueHandles">ֵ�ļ���</param>
        /// <param name="previous">��һ���ؼ���</param>
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
