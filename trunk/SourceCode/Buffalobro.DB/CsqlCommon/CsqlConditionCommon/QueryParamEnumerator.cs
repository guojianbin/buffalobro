using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.FastReflection.ClassInfos;


namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// 属性的枚举类
    /// </summary>
    internal class QueryParamEnumerator : DictionaryEnumerator<string, ParamInfo>
    {
        public QueryParamEnumerator(Dictionary<string, ParamInfo> dic)
            :base(dic)
        {

        }
    }
}
