using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalobro.Kernel.Defaults;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class CsqlCaseHandle:CsqlParamHandle
    {
        private CsqlValueItem caseItem;
        public CsqlCaseHandle(CsqlValueItem item)
        {
            this.caseItem = item;
            
            //this.valueType = CsqlValueType.Case;
            //this.valueDataType = DefaultType.StringType;
            this._valueDbType = DbType.String;
        }

        ///// <summary>
        ///// CaseµÄÏî
        ///// </summary>
        //internal CsqlKeyWordItem CaseItem 
        //{
        //    get 
        //    {
        //        return caseItem;
        //    }
        //}
        internal override void FillInfo(KeyWordInfomation info)
        {
            
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            KeyWordConver objCover = new KeyWordConver();
            
            StringBuilder sb=new StringBuilder();
            sb.Append("(");
            sb.Append(caseItem.DisplayValue(info));
            sb.Append(" end) ");
            return sb.ToString();
            
        }
    }
}
