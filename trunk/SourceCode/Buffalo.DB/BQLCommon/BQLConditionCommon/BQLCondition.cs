using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ��ѯ����
    /// </summary>
    public abstract class BQLCondition : BQLValueItem
    {
        /// <summary>
        /// ��ȷ����(1=1)
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
        /// ��������(1=2)
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
