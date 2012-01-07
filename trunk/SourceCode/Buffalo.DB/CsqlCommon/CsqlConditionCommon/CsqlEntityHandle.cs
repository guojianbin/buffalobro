using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
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
