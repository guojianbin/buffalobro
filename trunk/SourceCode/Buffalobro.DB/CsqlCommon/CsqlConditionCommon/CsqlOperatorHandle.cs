using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{

    public class CsqlOperatorHandle:CsqlParamHandle
    {
        private DelOperatorHandle function;
        private CsqlValueItem[] parameters;

        public CsqlOperatorHandle(DelOperatorHandle function, CsqlValueItem[] parameters) 
        {
            this.parameters = parameters;
            this.function = function;

            
            //this.valueType = CsqlValueType.Function;
        }
        /// <summary>
        /// ²ÎÊý
        /// </summary>
        internal CsqlValueItem[] GetParameters()
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
