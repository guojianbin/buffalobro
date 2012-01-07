using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// ��ѯ����
    /// </summary>
    public abstract class CsqlCondition : CsqlValueItem
    {
        /// <summary>
        /// ��ȷ����(1=1)
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
        /// ��������(1=2)
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
