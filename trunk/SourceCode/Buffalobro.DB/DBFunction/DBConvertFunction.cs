using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.DBFunction
{
    public class DBConvertFunction
    {
        public static string ConvetTo(string value, System.Data.DbType dbType,DBInfo info) 
        {
            return info.Convert.ConvetTo(value, dbType);
        }
        public static string DateTimeToString(string dateTime, string format, DBInfo info) 
        {
            format = format.Replace("YYYY", "yyyy");
            format = format.Replace("HH", "hh");
            format = format.Replace("SS", "ss");
            format = format.Replace("MS", "ms");
            return info.Convert.DateTimeToString(dateTime, format);
        }
        public static string StringToDateTime(string value, string format, DBInfo info) 
        {
            return info.Convert.StringToDateTime(value, format);
        }
    }
}
