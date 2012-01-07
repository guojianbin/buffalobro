using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class CsqlComparItem : CsqlCondition
    {
        private DelFunctionHandle function;
        private CsqlValueItem[] parameters;

        public CsqlComparItem(DelFunctionHandle function, CsqlValueItem[] parameters) 
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
            foreach (CsqlValueItem value in parameters) 
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
