using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter
{
    /// <summary>
    /// �ۺϺ�������
    /// </summary>
    public class AggregateFunctions :Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        public override string DoStdDev(string paramName)
        {
            return "stddev(" + paramName + ")";
        }
    }
}
