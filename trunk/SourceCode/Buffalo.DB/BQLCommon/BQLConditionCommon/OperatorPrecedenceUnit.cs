using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 运算符优先级处理类
    /// </summary>
    public class OperatorPrecedenceUnit
    {
        /// <summary>
        /// 左边的数值是否需要加括号
        /// </summary>
        /// <param name="left">左边数值</param>
        /// <param name="operLevel">运算符优先级</param>
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
        /// 右边的数值是否需要加括号
        /// </summary>
        /// <param name="right">右边数值</param>
        /// <param name="operLevel">运算符优先级</param>
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
