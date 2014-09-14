using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.PostgreSQL9Adapter
{
    /// <summary>
    /// ���ɷ�ҳ������
    /// </summary>
    public class CutPageSqlCreater
    {
       
        /// <summary>
        /// ����SQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public static string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage, bool useCache)
        {

            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//��ʼ��ҳ��
            {
                return "";
            }
            string sql = objCondition.GetSelect();
            if (objPage.IsFillTotleRecords)
            {
                objPage.TotleRecords = GetTotleRecord(list, oper, objCondition.GetSelect(false), objPage.MaxSelectRecords,
                    (useCache?objCondition.CacheTables:null));//��ȡ�ܼ�¼��
                long totlePage = (long)Math.Ceiling((double)objPage.TotleRecords / (double)objPage.PageSize);
                objPage.TotlePage = totlePage;
                if (objPage.CurrentPage >= objPage.TotlePage - 1)
                {
                    objPage.CurrentPage = objPage.TotlePage - 1;

                }
            }
            return GetCutPageSql(sql, objPage);
        }
        /// <summary>
        /// ��ȡ��ҳ���
        /// </summary>
        /// <param name="sql">Ҫ����ҳ��SQL</param>
        /// <param name="objCondition">��ҳ��</param>
        /// <returns></returns>
        public static string GetCutPageSql(string sql, PageContent objPage) 
        {
            long starIndex = objPage.GetStarIndex();
            string tmpsql = sql;
            tmpsql += " limit " + objPage.PageSize + " offset " + starIndex;
            return tmpsql;
        }

        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        public static long GetTotleRecord(ParamList list, DataBaseOperate oper,string sql,
            long maxRecords,Dictionary<string,bool> cacheTables)
        {
            long totleRecords = 0;
            //string tmpsql = "select count(*) from (" + sql + ")tmp";
            StringBuilder tmpsql = new StringBuilder(2000);
            if (maxRecords > 0)
            {
                tmpsql.Append("select count(*) from (select * from (");
                tmpsql.Append(sql);
                tmpsql.Append(")tmp1 limit " + maxRecords + " offset 0");
                tmpsql.Append(maxRecords.ToString());
                tmpsql.Append(")tmp");
            }
            else
            {
                tmpsql.Append("select count(*) from (");
                tmpsql.Append(sql);
                tmpsql.Append(")tmp");
            }
            IDataReader reader = oper.Query(tmpsql.ToString(), list, cacheTables);
            try
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        totleRecords = reader.GetInt64(0);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return totleRecords;
        }
    
    }
}
