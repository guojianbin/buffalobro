using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// ��ʽֵ���ͻ���
    /// </summary>
    public abstract class KeyWordLinkValueItemBase : CsqlValueItem
    {
        /// <summary>
        /// ��ʽֵ���ͻ���
        /// </summary>
        /// <param name="previous">��һ��ֵ</param>
        public KeyWordLinkValueItemBase(KeyWordLinkValueItemBase previous)
        {
            _previous = previous;
        }
        private KeyWordLinkValueItemBase _previous;

        /// <summary>
        /// ��һ��ֵ
        /// </summary>
        public KeyWordLinkValueItemBase Previous
        {
            get { return _previous; }
        }
    }
}
