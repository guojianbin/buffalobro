using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
namespace Buffalo.DB.DbCommon
{
    public class DBConn
    {
        //private static Dictionary<string, string> dicConnString = new Dictionary<string, string>();//�����ַ������ݴ��
        

        /// <summary>
        /// ��ȡָ������
        /// </summary>
        /// <param name="connectionKey">ָ�����ӵ������ַ����ļ�(���ҪĬ�ϼ����Ϊnull)</param>
        /// <returns></returns>
        public static IDbConnection GetConnection(DBInfo db)
        {
            string connectionString = db.ConnectionString;
            
            IDbConnection conn = db.CurrentDbAdapter.GetConnection();
            conn.ConnectionString = connectionString;
            return conn;
        }
    }
}
