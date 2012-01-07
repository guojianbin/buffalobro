using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class UpdateSetParamItemList : List<UpdateSetParamItem>
    {
        /// <summary>
        /// ���һ��������
        /// </summary>
        /// <param name="parameter">��������ֶ�</param>
        /// <param name="valueItem">����ֵ</param>
        public void Add(CsqlParamHandle parameter, CsqlValueItem valueItem) 
        {
            this.Add(new UpdateSetParamItem(parameter,valueItem));
        }
    }
}
