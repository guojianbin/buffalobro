using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public abstract class BQLCondition : BQLValueItem
    {
        /// <summary>
        /// 正确条件(1=1)
        /// </summary>
        public static BQLComparItem TrueValue
        {
            get
            {
                BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoEqual, new BQLValueItem[] { 1,1});
                return fHandle;
            }
        }

        /// <summary>
        /// 错误条件(1=2)
        /// </summary>
        public static BQLComparItem FalseValue
        {
            get
            {
                BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoEqual, new BQLValueItem[] { 1,2 });
                return fHandle;
            }
        }
    }
}
