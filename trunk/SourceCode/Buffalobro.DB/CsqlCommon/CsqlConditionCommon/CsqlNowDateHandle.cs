using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.CsqlCommon.CsqlConditions;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalobro.DB.CsqlCommon.CsqlExtendFunction;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.Kernel.Defaults;
using System.Data;
using System.Collections;
using Buffalobro.DB.DBFunction;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// 当前时间的句柄
    /// </summary>
    public class CsqlNowDateHandle : CsqlParamHandle
    {
        public CsqlNowDateHandle(DbType dbType) 
        {
            ValueDbType = dbType;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            return idba.GetNowDate(ValueDbType);


        }
    }
}
