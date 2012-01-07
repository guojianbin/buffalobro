using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// 属性集合
    /// </summary>
    public class ScopePropertyCollection:List<CsqlParamHandle>
    {
        ScopeList _belong;
        public ScopePropertyCollection(ScopeList belong) 
        {
            _belong = belong;
        }
        /// <summary>
        /// 添加属性集合
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
        /// 添加一个元素
        /// </summary>
        /// <param name="item"></param>
        public new void Add(CsqlParamHandle item) 
        {
            _belong.HasInner = true;
            base.Add(item);
        }
    }
}
