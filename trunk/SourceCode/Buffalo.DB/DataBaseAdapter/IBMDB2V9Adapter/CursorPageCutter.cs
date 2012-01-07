using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using Buffalo.DB.CommBase;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter
{
    /// <summary>
    /// �α��ҳ
    /// </summary>
    public class CursorPageCutter
    {
        /// <summary>
        /// ��ѯ���ҷ��ؼ���(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <returns></returns>
        public static IDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {

            objPage.TotleRecords = CutPageSqlCreater.GetTotleRecord(lstParam, oper, sql, objPage.MaxSelectRecords);
            long totlePage = (long)Math.Ceiling((double)objPage.TotleRecords / (double)objPage.PageSize);
            objPage.TotlePage = totlePage;
            if (objPage.CurrentPage >= objPage.TotlePage - 1)
            {
                objPage.CurrentPage = objPage.TotlePage - 1;
            }
            IDataReader reader = null;

            string qsql = GetCursorPageSql(sql, objPage);
            reader = oper.Query(qsql, lstParam);

            return reader;
        }

        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="part">SQL����</param>
        /// <returns></returns>
        private static string GetCursorPageSql(string sql, PageContent objPage)
        {
            long starIndex = objPage.GetStarIndex() + 1;
            string rowNumberName =  "\"__cur_rowNumber" + objPage.PagerIndex+"\"";
            long endIndex = objPage.PageSize * (objPage.CurrentPage + 1);
            StringBuilder sb = new StringBuilder(5000);
            sb.Append("select * from (");
            sb.Append("select ROW_NUMBER() over() as " + rowNumberName + ",tmp.*  from (");
            sb.Append(sql);
            sb.Append(") tmp) tmp1 where " + rowNumberName + " >=" + starIndex + " and " + rowNumberName + " <=" + endIndex);
            return sb.ToString();
        }
        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            objPage.TotleRecords = CutPageSqlCreater.GetTotleRecord(lstParam, oper, sql, objPage.MaxSelectRecords);
            long totlePage = (long)Math.Ceiling((double)objPage.TotleRecords / (double)objPage.PageSize);
            objPage.TotlePage = totlePage;
            if (objPage.CurrentPage >= objPage.TotlePage - 1)
            {
                objPage.CurrentPage = objPage.TotlePage - 1;
            }
            if (objPage.CurrentPage >= objPage.TotlePage - 1)
            {
                objPage.CurrentPage = objPage.TotlePage - 1;
            }

            DataTable ret = new DataTable();
            IDataReader reader = null;
            try
            {
                string qsql = GetCursorPageSql(sql, objPage);
                reader = oper.Query(qsql, lstParam);
                
                if (curType == null)
                {
                    ret = CacheReader.GenerateDataTable(reader, "newDt", false);
                }
                else 
                {
                    ret = CacheReader.GenerateDataTable(reader, "newDt",curType, false);
                }
            }
            finally
            {
                reader.Close();
                //oper.CloseDataBase();
            }
            return ret;
        }
    }
}
