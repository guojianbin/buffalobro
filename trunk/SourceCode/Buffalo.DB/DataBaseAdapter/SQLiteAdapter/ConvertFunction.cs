using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.SQLiteAdapter
{
    /// <summary>
    /// 数值转换函数
    /// </summary>
    public class ConvertFunction :Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction
    {

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <returns></returns>
        private static string GetFormat(string format)
        {
            StringBuilder ret = new StringBuilder(format);
            ret.Replace("%", "%%");
            ret.Replace("yyyy", "%Y");
            ret.Replace("yy", "%Y");
            ret.Replace("YYYY", "%Y");
            ret.Replace("MM", "%m");
            ret.Replace("dd", "%d");
            ret.Replace("hh", "%H");
            ret.Replace("mm", "%M");
            ret.Replace("ss.ms", "%f");
            ret.Replace("ss", "%S");
            
            return ret.ToString();
        }

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string DateTimeToString(string dateTime, string format) 
        {

            if (format != null)
            {
                return "strftime('" + GetFormat(format) + "'," + dateTime + ")";
            }
            return "strftime('%Y-%m-%d %H:%M:%f'," + dateTime + ")";
        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            return "datetime(" + value + ")";
        }

        /// <summary>
        /// 把数据转成指定类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="dbType">指定类型</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            //SqlDbType sdb = (SqlDbType)DbAdapterLoader.CurrentDbAdapter.ToCurrentDbType(dbType);
            return value;
        }
    }
}
