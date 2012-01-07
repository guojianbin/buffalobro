using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using System.Data;

namespace Buffalo.DB.CsqlCommon.CsqlExtendFunction
{
    public class CsqlCustomizeFunction : CsqlParamHandle
    {
        private string _funName;
        private CsqlValueItem[] _values;
        /// <summary>
        /// 自定义函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <param name="values">函数值</param>
        public CsqlCustomizeFunction(string funName, CsqlValueItem[] values)
        {
            this._funName = funName;
            this._values = values;
            this._valueDbType = DbType.Object;
        }
        /// <summary>
        /// 自定义函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <param name="values">函数值</param>
        public CsqlCustomizeFunction(string funName)
            :this(funName,null)
        {
        }

        internal override void FillInfo(KeyWordInfomation info)
        {

        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //DelCommonFunction handle = funHandle;
            StringBuilder sb = new StringBuilder();
            if (_values != null)
            {
                foreach (CsqlValueItem value in _values) 
                {
                    sb.Append(value.DisplayValue(info) + ",");
                }
                if (sb.Length > 0) 
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            StringBuilder sbRet = new StringBuilder(sb.Length+_funName.Length+5);
            sbRet.Append(_funName);
            sbRet.Append("(");
            sbRet.Append(sb.ToString());
            sbRet.Append(")");
            return sbRet.ToString();
        }
    }
}
