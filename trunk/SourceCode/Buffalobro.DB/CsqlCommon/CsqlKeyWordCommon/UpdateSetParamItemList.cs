using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class UpdateSetParamItemList : List<UpdateSetParamItem>
    {
        /// <summary>
        /// 添加一个更新项
        /// </summary>
        /// <param name="parameter">更新项的字段</param>
        /// <param name="valueItem">更新值</param>
        public void Add(CsqlParamHandle parameter, CsqlValueItem valueItem) 
        {
            this.Add(new UpdateSetParamItem(parameter,valueItem));
        }
    }
}
