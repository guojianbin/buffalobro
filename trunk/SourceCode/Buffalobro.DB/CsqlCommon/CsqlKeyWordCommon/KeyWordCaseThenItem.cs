using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordCaseThenItem : KeyWordLinkValueItemBase
    {
        private CsqlValueItem thenValue;
        private CsqlValueItem whenValue;

        /// <summary>
        /// Case��
        /// </summary>
        /// <param name="whenValue">when��ֵ</param>
        /// <param name="thenValue">then��ֵ</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordCaseThenItem(CsqlValueItem whenValue, CsqlValueItem thenValue, KeyWordLinkValueItemBase previous)
        :base(previous)
        {
            this.whenValue = whenValue;
            this.thenValue = thenValue;
        }
        ///// <summary>
        ///// then��ֵ
        ///// </summary>
        //internal CsqlValueItem ThenValue 
        //{
        //    get 
        //    {
        //        return thenValue;
        //    }
        //}
        ///// <summary>
        ///// when��ֵ
        ///// </summary>
        //internal CsqlValueItem WhenValue
        //{
        //    get
        //    {
        //        return whenValue;
        //    }
        //}

        /// <summary>
        /// When�ؼ���
        /// </summary>
        /// <param name="whenValue">when����</param>
        /// <returns></returns>
        public KeyWordCaseWhenItem When(CsqlValueItem whenValue) 
        {
            KeyWordCaseWhenItem item = new KeyWordCaseWhenItem(whenValue, this);
            return item;
        }

        /// <summary>
        /// else�ؼ���
        /// </summary>
        /// <param name="elseValue">else����</param>
        /// <returns></returns>
        public KeyWordCaseElseItem Else(CsqlValueItem elseValue)
        {
            KeyWordCaseElseItem item = new KeyWordCaseElseItem(elseValue, this);
            return item;
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
            sb.Append(" when ");
            sb.Append(whenValue.DisplayValue(info));
            sb.Append(" then ");
            sb.Append(thenValue.DisplayValue(info));
            return sb.ToString();
        }


    }
}
