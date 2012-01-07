using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.Kernel;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordCaseItem : KeyWordLinkValueItemBase
    {
        protected CsqlValueItem itemValue;

       

        /// <summary>
        /// Case关键字项
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordCaseItem(CsqlValueItem value, KeyWordLinkValueItemBase previous)
            :base(previous)
        {
            this.itemValue = value;
        }

        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal CsqlValueItem ItemValue
        //{
        //    get 
        //    {
        //        return itemValue;
        //    }
        //}
        /// <summary>
        /// When关键字
        /// </summary>
        /// <param name="whenValue">when条件</param>
        /// <returns></returns>
        public KeyWordCaseWhenItem When(CsqlValueItem whenValue)
        {
            KeyWordCaseWhenItem item = new KeyWordCaseWhenItem(whenValue, this);
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
            sb.Append("case ");
            if (!CommonMethods.IsNull(itemValue))
            {
                sb.Append(itemValue.DisplayValue(info) + " ");
            }
            //return sb.ToString();
            return sb.ToString();
        }

        
    }
}
