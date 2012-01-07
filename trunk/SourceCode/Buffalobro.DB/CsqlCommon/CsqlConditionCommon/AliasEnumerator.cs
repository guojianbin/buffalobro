using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.FastReflection.ClassInfos;


namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// ���Ե�ö����
    /// </summary>
    internal class AliasEnumerator : DictionaryEnumerator<string, QueryParamCollection>
    {
        internal AliasEnumerator(Dictionary<string, QueryParamCollection> dic)
            :base(dic)
        {

        }
    }
}
