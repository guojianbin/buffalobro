using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.QueryConditions
{
    /// <summary>
    /// ���Լ���
    /// </summary>
    public class ScopePropertyCollection:List<CsqlParamHandle>
    {
        ScopeList _belong;
        public ScopePropertyCollection(ScopeList belong) 
        {
            _belong = belong;
        }
        /// <summary>
        /// ������Լ���
        /// </summary>
        /// <param name="prms"></param>
        public void AddPropertys(params CsqlParamHandle[] prms) 
        {
            foreach (CsqlParamHandle handle in prms) 
            {
                Add(handle);
            }
        }

        /// <summary>
        /// ���һ��Ԫ��
        /// </summary>
        /// <param name="item"></param>
        public new void Add(CsqlParamHandle item) 
        {
            _belong.HasInner = true;
            base.Add(item);
        }
    }
}
