using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ��������ȼ�������
    /// </summary>
    public class OperatorPrecedenceUnit
    {
        /// <summary>
        /// ��ߵ���ֵ�Ƿ���Ҫ������
        /// </summary>
        /// <param name="left">�����ֵ</param>
        /// <param name="operLevel">��������ȼ�</param>
        /// <returns></returns>
        public static bool LeftNeedBreak(IOperatorPriorityLevel left,int operLevel) 
        {
            if (left.PriorityLevel < operLevel) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �ұߵ���ֵ�Ƿ���Ҫ������
        /// </summary>
        /// <param name="right">�ұ���ֵ</param>
        /// <param name="operLevel">��������ȼ�</param>
        /// <returns></returns>
        public static bool RightNeedBreak(IOperatorPriorityLevel right, int operLevel)
        {
            if (right.PriorityLevel <= operLevel)
            {
                return true;
            }
            return false;
        }
    }
}
