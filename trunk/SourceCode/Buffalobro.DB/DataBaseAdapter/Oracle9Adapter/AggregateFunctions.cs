using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// 聚合函数处理
    /// </summary>
    public class AggregateFunctions : SQLCommon.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        public override string DoStdDev(string paramName)
        {
            return "stddev(" + paramName + ")";
        }
    }
}
