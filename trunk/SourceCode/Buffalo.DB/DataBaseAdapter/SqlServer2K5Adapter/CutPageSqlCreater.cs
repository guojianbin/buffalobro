using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter
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
        public static string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage)
        {
            //if (objCondition.Condition == null || objCondition.Condition == "")//��ʼ����ѯ����
            //{
            //    objCondition.Condition = "1=1";
            //}
            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//��ʼ��ҳ��
            {
                return "";
            }
            if (objPage.IsFillTotleRecords)
            {
                objPage.TotleRecords = GetTotleRecord(list, oper, objCondition,objPage);//��ȡ�ܼ�¼��
                long totlePage = (long)Math.Ceiling((double)objPage.TotleRecords / (double)objPage.PageSize);
                objPage.TotlePage = totlePage;
                if (objPage.CurrentPage >= objPage.TotlePage - 1)
                {
                    objPage.CurrentPage = objPage.TotlePage - 1;
                    //objCondition.CurrentPage = objPage.CurrentPage;
                }
            }
            return CreateCutPageSql(objCondition, objPage);
        }

        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="part">SQL����</param>
        /// <returns></returns>
        private static string CreateCutPageSql(SelectCondition objCondition, PageContent objPage)
        {
            string orderBy = null;
            if (objCondition.Orders.Length>0)//�����������ñ�������
            {
                orderBy = objCondition.Orders.ToString();
            }
            else//���û�о�����������
            {
                orderBy = objCondition.PrimaryKey.ToString();
            }
            StringBuilder sql = new StringBuilder(5000);

            if (objPage.GetStarIndex() == 0)
            {
                return Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetFristPageSql(objCondition, objPage);
            }

            long starIndex = objPage.GetStarIndex() + 1;
            string rowNumberName = "[cur_rowNumber" + objPage.PagerIndex+"]";
            //long endIndex = objCondition.PageSize * (objCondition.CurrentPage + 1);
            sql.Append("select top " + objPage.PageSize + " * from(");

            sql.Append("select row_number() over(order by " + orderBy + ") as " +
                rowNumberName + "," );

            

            if (!objCondition.HasGroup)
            {
                sql.Append(objCondition.SqlParams.ToString());
                sql.Append("  from ");
                sql.Append(objCondition.Tables.ToString());
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where " + objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by " + objCondition.GroupBy.ToString());
                }
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
                
            }
            else 
            {
                sql.Append("[_tmpInnerTable].*");
                sql.Append("  from (");
               Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                sql.Append(") [_tmpInnerTable]");
            }
            sql.Append(") tmp where " + rowNumberName + " >=" + starIndex);
            return sql.ToString();
        }

        

        /// <summary>
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <param name="part">��ѯ����</param>
        /// <param name="list">�����б�</param>
        /// <param name="oper">ͨ����</param>
        private static long GetTotleRecord(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage)
        {
            long totleRecords = 0;
            StringBuilder sql = new StringBuilder();
            if (objPage.MaxSelectRecords > 0)
            {

                sql.Append("select count(*) from (select top " + objPage.MaxSelectRecords + " * from " );

                if (!objCondition.HasGroup)
                {
                    sql.Append(objCondition.Tables).ToString();
                    if (objCondition.Condition.Length > 0)
                    {
                        sql.Append(" where " + objCondition.Condition.ToString());
                    }
                    if (objCondition.GroupBy.Length > 0)
                    {
                        sql.Append(" group by " + objCondition.GroupBy.ToString());
                    }
                    if (objCondition.Having.Length > 0)
                    {
                        sql.Append(" having ");
                        sql.Append(objCondition.Having.ToString());
                    }
                    sql.Append(") tab");
                }
                else 
                {
                   Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                    sql.Append(") tab");
                }
            }
            else
            {
                sql.Append("select count(*) from ");
                if (!objCondition.HasGroup)
                {
                    sql.Append(objCondition.Tables.ToString());
                    if (objCondition.Condition.Length > 0)
                    {
                        sql.Append(" where " + objCondition.Condition.ToString());
                    }
                    if (objCondition.GroupBy.Length > 0)
                    {
                        sql.Append(" group by " + objCondition.GroupBy.ToString());
                    }
                    if (objCondition.Having.Length > 0)
                    {
                        sql.Append(" having ");
                        sql.Append(objCondition.Having.ToString());
                    }
                }
                else 
                {
                    sql.Append("(");
                   Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CutPageSqlCreater.GetGroupPart(objCondition, sql);
                    sql.Append(") tmp");

                }
            }
            IDataReader reader = oper.Query(sql.ToString(), list);
            try
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        totleRecords = Convert.ToInt64(reader[0]);
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
