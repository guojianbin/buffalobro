using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalobro.DB.CommBase;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;

namespace Buffalobro.DB.DataBaseAdapter.SqlServer2KAdapter
{
    /// <summary>
    /// 生成分页语句的类
    /// </summary>
    public class CutPageSqlCreater
    {
        /// <summary>
        /// 获取分组分页的片段
        /// </summary>
        /// <param name="objCondition"></param>
        /// <returns></returns>
        public static void GetGroupPart(SelectCondition objCondition, StringBuilder sql)
        {
            sql.Append("select ");
            sql.Append(objCondition.SqlParams.ToString());
            sql.Append(" from ");
            sql.Append(objCondition.Tables.ToString());
            if (objCondition.Condition.Length > 0)
            {
                sql.Append(" where ");
                sql.Append(objCondition.Condition.ToString());
            }

            if (objCondition.GroupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(objCondition.GroupBy.ToString());
            }
            if (objCondition.Having.Length > 0)
            {
                sql.Append(" having ");
                sql.Append(objCondition.Having.ToString());
            }
        }

        /// <summary>
        /// 生成SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <returns></returns>
        public static string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage)
        {
            if (objPage.CurrentPage < 0 || objPage.PageSize <= 0)//初始化页数
            {
                return "";
            }
            if (objPage.IsFillTotleRecords)
            {
                objPage.TotleRecords = GetTotleRecord(list, oper, objCondition, objPage);//获取总记录数
                long totlePage = (long)Math.Ceiling((double)objPage.TotleRecords / (double)objPage.PageSize);
                objPage.TotlePage = totlePage;
                if (objPage.CurrentPage >= objPage.TotlePage - 1)
                {
                    objPage.CurrentPage = objPage.TotlePage - 1;
                }
            }
            return CreateCutPageSql(objCondition, objPage);
        }

        /// <summary>
        /// 获取第一页的SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="objCondition"></param>
        /// <param name="objPage"></param>
        /// <returns></returns>
        protected internal static string GetFristPageSql(SelectCondition objCondition, PageContent objPage) 
        {
            StringBuilder sql = new StringBuilder(5000);
            sql.Append("select top ");
            sql.Append(objPage.PageSize.ToString());
            sql.Append(" ");
            
            if (!objCondition.HasGroup)
            {
                sql.Append(objCondition.SqlParams);
                sql.Append("  from ");

                sql.Append(objCondition.Tables);

                sql.Append(" ");
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
                }
                if (objCondition.Orders.Length > 0)
                {
                    sql.Append(" order by ");
                    sql.Append(objCondition.Orders.ToString());
                }
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
            }
            else 
            {
                sql.Append("*  from ");
                sql.Append("(");
                sql.Append("select ");
                sql.Append(objCondition.SqlParams.ToString());
                sql.Append(" from ");
                sql.Append(objCondition.Tables.ToString());
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }

                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
                }
                
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
                }
                sql.Append(") tmp");
                if (objCondition.Orders.Length > 0)
                {
                    sql.Append(" order by ");
                    sql.Append(objCondition.Orders.ToString());
                }
            }
            return sql.ToString();
        }

        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="part">SQL条件</param>
        /// <returns></returns>
        private static string CreateCutPageSql(SelectCondition objCondition, PageContent objPage)
        {

            string totleLine = objPage.PageSize.ToString();

            long starIndex = objPage.GetStarIndex();

            if (starIndex == 0)
            {
                //sql.Append("select top " + totleLine + " " + objCondition.SqlParams + "  from [" + objCondition.Tables + "] " + " where " + objCondition.Condition);
                return GetFristPageSql(objCondition, objPage);
            }

            StringBuilder sql = new StringBuilder(5000);
            //sql.Append("select top " + totleLine + " " + objCondition.SqlParams.ToString() + " from [" + objCondition.Tables.ToString() + "] where(not exists (select * from (select top " + starIndex.ToString() + " " + objCondition.SqlParams + " from [" + objCondition.Tables + "] where " + objCondition.Condition);
            sql.Append("select top ");
            sql.Append(totleLine);
            sql.Append(" ");
            sql.Append(objCondition.SqlParams.ToString());
            sql.Append(" from ");
            sql.Append(objCondition.Tables.ToString());
            sql.Append(" where(not exists (select * from (select top ");
            sql.Append(starIndex.ToString());
            sql.Append(" ");
            sql.Append(objCondition.SqlParams.ToString());
            sql.Append(" from ");
            sql.Append(objCondition.Tables.ToString());
            if (objCondition.Condition.Length > 0)
            {
                sql.Append(" where ");
                sql.Append(objCondition.Condition.ToString());
            }
            if (objCondition.GroupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(objCondition.GroupBy.ToString());
            }
            if (objCondition.Orders.Length > 0)
            {
                sql.Append(" order by ");
                sql.Append(objCondition.Orders.ToString());
            }
            sql.Append(") tmptable where tmptable.");
            sql.Append(objCondition.PrimaryKey.ToString() + "=");
            sql.Append(objCondition.Tables.ToString());
            sql.Append(".");
            sql.Append(objCondition.PrimaryKey.ToString());
            sql.Append(")");
            if (objCondition.Condition.Length > 0)
            {
                sql.Append(" and " + objCondition.Condition.ToString());
            }
            sql.Append(")");
            if (objCondition.GroupBy.Length > 0)
            {
                sql.Append(" group by ");
                sql.Append(objCondition.GroupBy.ToString());
            }
            if (objCondition.Orders.Length > 0)
            {
                sql.Append(" order by ");
                sql.Append(objCondition.Orders.ToString());
            }
            if (objCondition.Having.Length > 0)
            {
                sql.Append(" having ");
                sql.Append(objCondition.Having.ToString());
            }

            return sql.ToString();
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <param name="part">查询条件</param>
        /// <param name="list">变量列表</param>
        /// <param name="oper">通用类</param>
        private static long GetTotleRecord(ParamList list, DataBaseOperate oper, SelectCondition objCondition,PageContent objPage)
        {
            long totleRecords = 0;
            StringBuilder sql = new StringBuilder(5000);

            if (objPage.MaxSelectRecords > 0)
            {
                sql.Append("select count(*) from (select top " );
                sql.Append(objPage.MaxSelectRecords.ToString());
                sql.Append(" * from " );
                sql.Append(objCondition.Tables);
                sql.Append("" );
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
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
                sql.Append("select count(*) from " );
                sql.Append(objCondition.Tables );
                sql.Append("" );
                
                if (objCondition.Condition.Length > 0)
                {
                    sql.Append(" where ");
                    sql.Append(objCondition.Condition.ToString());
                }
                if (objCondition.GroupBy.Length > 0)
                {
                    sql.Append(" group by ");
                    sql.Append(objCondition.GroupBy.ToString());
                }
                if (objCondition.Having.Length > 0)
                {
                    sql.Append(" having ");
                    sql.Append(objCondition.Having.ToString());
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
