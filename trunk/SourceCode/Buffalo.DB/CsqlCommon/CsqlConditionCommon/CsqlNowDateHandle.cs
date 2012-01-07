using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CsqlCommon.CsqlConditions;
using Buffalo.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalo.DB.CsqlCommon.CsqlExtendFunction;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Data;
using System.Collections;
using Buffalo.DB.DBFunction;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
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
