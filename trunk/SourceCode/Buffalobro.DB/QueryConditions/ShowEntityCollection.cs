using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.QueryConditions
{
    public class ShowEntityCollection : List<CsqlEntityTableHandle>
    {
        private ScopeList _belongList;//所属的集合

        /// <summary>
        /// 要现实的实体集合
        /// </summary>
        /// <param name="belong"></param>
        internal ShowEntityCollection(ScopeList belong) 
        {
            _belongList = belong;
        }

        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="prm"></param>
        public new void Add(CsqlEntityTableHandle prm)
        {
            _belongList.HasInner = true;
            this.Add(prm);
        }

        /// <summary>
        /// 添加一个输出字段
        /// </summary>
        /// <param name="prm"></param>
        public void AddRang(params CsqlEntityTableHandle[] prms)
        {
            _belongList.HasInner = true;
            this.AddRang(prms);
        }
    }
}
