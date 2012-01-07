using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordCaseElseItem : KeyWordCaseItem
    {
        /// <summary>
        /// Case的Else关键字项
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordCaseElseItem(CsqlValueItem value, KeyWordLinkValueItemBase previous)
            : base(value,previous) 
        {
            
        }
        /// <summary>
        /// 结束case语句
        /// </summary>
        public CsqlCaseHandle End
        {
            get
            {
                return new CsqlCaseHandle(this);
            }
        }


        internal override void FillInfo(KeyWordInfomation info)
        {
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            if (!Commons.CommonMethods.IsNull(Previous)) 
            {
                sb.Append(Previous.DisplayValue(info));
            }
            
            sb.Append(" else ");
            sb.Append(itemValue.DisplayValue(info));
            return sb.ToString();
        }

    }
}
