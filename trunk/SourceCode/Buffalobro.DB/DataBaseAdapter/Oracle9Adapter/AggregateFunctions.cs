using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// �ۺϺ�������
    /// </summary>
    public class AggregateFunctions : SQLCommon.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        public override string DoStdDev(string paramName)
        {
            return "stddev(" + paramName + ")";
        }
    }
}
