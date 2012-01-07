using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordCaseElseItem : KeyWordCaseItem
    {
        /// <summary>
        /// Case��Else�ؼ�����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordCaseElseItem(CsqlValueItem value, KeyWordLinkValueItemBase previous)
            : base(value,previous) 
        {
            
        }
        /// <summary>
        /// ����case���
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
