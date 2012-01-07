using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.QueryConditions;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// ��ʾ��¼�ķ�Χ
    /// </summary>
    public class KeyWordLimitItem : CsqlQuery
    {
        uint star = 0;
        uint totleRecords = 0;
        /// <summary>
        /// ��ʾ��¼�ķ�Χ
        /// </summary>
        /// <param name="star">��ʼ��¼</param>
        /// <param name="totleRecords">Ҫ��ʾ��������¼</param>
        /// <param name="previous">��һ�����</param>
        public KeyWordLimitItem(uint star, uint totleRecords, CsqlQuery previous)
            : base(previous) 
        {
            this.star = star;
            this.totleRecords = totleRecords;
        }

        internal override void LoadInfo(KeyWordInfomation info)
        {
            //info.IsPage = true;
        }

        internal override void Tran(KeyWordInfomation info)
        {
            PageContent objPage = new PageContent();
            objPage.StarIndex = star;
            info.Infos.PagerCount++;
            objPage.PagerIndex = info.Infos.PagerCount;
            
            objPage.PageSize = totleRecords;
            objPage.IsFillTotleRecords = false;
            info.Condition.PageContent = objPage;
        }
    }
}
