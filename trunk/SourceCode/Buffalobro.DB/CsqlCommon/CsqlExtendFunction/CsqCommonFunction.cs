using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon.CsqlAggregateFunctions;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.QueryConditions;

namespace Buffalobro.DB.CsqlCommon.CsqlExtendFunction
{
    public delegate string DelCommonFunction(string[] items,DBInfo info);
    public class CsqCommonFunction : CsqlParamHandle
    {
        public DelCommonFunction funHandle;
        private CsqlValueItem[] values;
        public CsqCommonFunction(CsqlValueItem[] values, DelCommonFunction funHandle,DbType dbType)
        {
            this.funHandle = funHandle;
            this.values = values;
            this._valueDbType = dbType;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            foreach (CsqlValueItem item in values) 
            {
                item.FillInfo(info);
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelCommonFunction handle = funHandle;
            if (handle != null)
            {
                List<string> lstParams = new List<string>();
                foreach (CsqlValueItem item in values) 
                {
                    lstParams.Add(item.DisplayValue(info));
                }
                SelectCondition con = info.Condition as SelectCondition;
                if (con != null) 
                {
                    con.HasGroup = true;
                }
                return handle(lstParams.ToArray(),info.DBInfo);
            }
            return null;
        }
    }
}
