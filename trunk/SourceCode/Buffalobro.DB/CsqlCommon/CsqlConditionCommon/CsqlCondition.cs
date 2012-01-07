using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public abstract class CsqlCondition : CsqlValueItem
    {
        /// <summary>
        /// 正确条件(1=1)
        /// </summary>
        public static CsqlComparItem TrueValue
        {
            get
            {
                CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoEqual, new CsqlValueItem[] { 1,1});
                return fHandle;
            }
        }

        /// <summary>
        /// 错误条件(1=2)
        /// </summary>
        public static CsqlComparItem FalseValue
        {
            get
            {
                CsqlComparItem fHandle = new CsqlComparItem(FunctionManager.DoEqual, new CsqlValueItem[] { 1,2 });
                return fHandle;
            }
        }
    }
}
