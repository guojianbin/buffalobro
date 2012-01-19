using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;


namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    /// <summary>
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : Buffalo.DB.DataBaseAdapter.IDbAdapters.IDBStructure
    {
        /// <summary>
        /// ��ȡ�����û���
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAllTableName(DBInfo info)
        {
            string sql = "SELECT Name FROM sysObjects Where XType='U' and Name<>'dtproperties' ORDER BY Name";
            DataBaseOperate oper = info.DefaultOperate;

            List<string> lstName = new List<string>();
            using (IDataReader reader = oper.Query(sql, new ParamList())) 
            {
                while (reader.Read())
                {
                    lstName.Add(reader[0].ToString());
                }
            }
            return lstName;
        }

        /// <summary>
        /// ����ֶε����
        /// </summary>
        /// <returns></returns>
        public virtual string GetAddParamSQL() 
        {
            return "add";
        }
    }
}
