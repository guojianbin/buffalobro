using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DataBaseAdapter;
using System.Data;

namespace Buffalobro.DB.DBFunction
{
    public class DBCommonFunction
    {
        public static string IsNull(string[] values,DBInfo info) 
        {
            return info.Common.IsNull(values);
        }
        public static string Len(string[] values, DBInfo info) 
        {
            return info.Common.Len(values);
        }
        public static string Distinct(string[] values, DBInfo info)
        {
            return info.Common.Distinct(values);
        }

    }
}
