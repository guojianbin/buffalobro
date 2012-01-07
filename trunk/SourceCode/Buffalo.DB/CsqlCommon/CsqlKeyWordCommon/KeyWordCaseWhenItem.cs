using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordCaseWhenItem
    {
        private CsqlValueItem value;
        private KeyWordLinkValueItemBase previous;

        /// <summary>
        /// Case��
        /// </summary>
        /// <param name="value">������ֵ</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordCaseWhenItem(CsqlValueItem value, KeyWordLinkValueItemBase previous)
        {
            this.value = value;
            this.previous = previous;
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="thenValue"></param>
        /// <returns></returns>
        public KeyWordCaseThenItem Then(CsqlValueItem thenValue) 
        {
            KeyWordCaseThenItem item = new KeyWordCaseThenItem(value, thenValue, previous);
            return item;
        }
        ///// <summary>
        ///// From��Щ��
        ///// </summary>
        ///// <param name="tables">��</param>
        ///// <returns></returns>
        //public KeyWordFromItem From(params CsqlTableHandle[] tables)
        //{
        //    KeyWordFromItem fromItem = new KeyWordFromItem(tables,this);
        //    return fromItem;
        //}

        
    }
}
