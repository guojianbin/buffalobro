using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
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
