using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLComparItem : BQLCondition
    {
        private DelFunctionHandle function;
        private BQLValueItem[] parameters;

        public BQLComparItem(DelFunctionHandle function, BQLValueItem[] parameters) 
        {
            this.parameters = parameters;
            this.function = function;
            //this.valueType = BQLValueType.Function;
        }
        /// <summary>
        /// ²ÎÊý
        /// </summary>
        internal BQLValueItem[] GetParameters()
        {
            
            return parameters;
            
        }
        
        internal override void FillInfo(KeyWordInfomation info)
        {
            foreach (BQLValueItem value in parameters) 
            {
                value.FillInfo(info);
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelFunctionHandle degFHandle = function;
            if (degFHandle != null)
            {
                return  "(" + degFHandle(this,info) + ")";
            }
            
            return null;
            
        }
    }
}
