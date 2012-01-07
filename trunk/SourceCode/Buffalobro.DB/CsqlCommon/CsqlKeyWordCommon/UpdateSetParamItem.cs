using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class UpdateSetParamItem 
    {
        protected CsqlParamHandle parameter;
        protected CsqlValueItem valueItem;
        /// <summary>
        /// Set�ؼ��ָ��µ���
        /// </summary>
        /// <param name="tables">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal UpdateSetParamItem(CsqlParamHandle parameter, CsqlValueItem valueItem)
        {
            this.parameter = parameter;
            this.valueItem = valueItem;
        }
        /// <summary>
        /// �����õ��ֶ�
        /// </summary>
        internal CsqlParamHandle Parameter
        {
            get
            {
                return parameter;
            }
        }

        /// <summary>
        /// Ҫ���õ�ֵ
        /// </summary>
        internal CsqlValueItem ValueItem
        {
            get
            {
                return valueItem;
            }
        }
        
    }

    
}
