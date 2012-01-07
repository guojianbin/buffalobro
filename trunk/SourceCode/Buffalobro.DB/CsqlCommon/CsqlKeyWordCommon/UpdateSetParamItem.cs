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
        /// Set关键字更新的项
        /// </summary>
        /// <param name="tables">表集合</param>
        /// <param name="previous">上一个关键字</param>
        internal UpdateSetParamItem(CsqlParamHandle parameter, CsqlValueItem valueItem)
        {
            this.parameter = parameter;
            this.valueItem = valueItem;
        }
        /// <summary>
        /// 被设置的字段
        /// </summary>
        internal CsqlParamHandle Parameter
        {
            get
            {
                return parameter;
            }
        }

        /// <summary>
        /// 要设置的值
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
