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
