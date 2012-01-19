using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{

    public class BQLOperatorHandle:BQLParamHandle
    {
        private DelOperatorHandle function;
        private BQLValueItem[] parameters;

        public BQLOperatorHandle(DelOperatorHandle function, BQLValueItem[] parameters) 
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
            
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelOperatorHandle degFHandle = function;
            if (degFHandle != null)
            {
                return "(" + degFHandle(this, info) + ")";
            }

            return null;
        }
    }
}
