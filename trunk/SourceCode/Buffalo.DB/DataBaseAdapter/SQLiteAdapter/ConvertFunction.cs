using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.SQLiteAdapter
{
    /// <summary>
    /// ��ֵת������
    /// </summary>
    public class ConvertFunction :Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction
    {

        /// <summary>
        /// ��ʽ���ַ���
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
        /// ����ת�ַ���
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
        /// �ַ���ת������
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            return "datetime(" + value + ")";
        }

        /// <summary>
        /// ������ת��ָ������
        /// </summary>
        /// <param name="value">����</param>
        /// <param name="dbType">ָ������</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            //SqlDbType sdb = (SqlDbType)DbAdapterLoader.CurrentDbAdapter.ToCurrentDbType(dbType);
            return value;
        }
    }
}
