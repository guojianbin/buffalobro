using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CommBase;
using Buffalobro.DB.EntityInfos;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class CsqlEntityHandle<T> where T:EntityBase
    {
        EntityInfoHandle entityInfo;
        public CsqlEntityHandle() 
        {
            entityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
        }
    }
}
