using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.FastReflection;
using Buffalobro.DB.QueryConditions;


namespace Buffalobro.DB.ListExtends
{
    public class CompareItemInfo
    {
        private PropertyInfoHandle pInfo;
        private Scope objScope;
        public CompareItemInfo(PropertyInfoHandle pInfo, Scope objScope) 
        {
            this.pInfo = pInfo;
            this.objScope = objScope;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public PropertyInfoHandle PropertyInfo 
        {
            get 
            {
                return pInfo;
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Scope ScopeInfo
        {
            get
            {
                return objScope;
            }
        }
    }
}
