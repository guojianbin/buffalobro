using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.DB.DBCheckers
{
    public class TableChecker
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="info">���ݿ�</param>
        /// <param name="tableName">����</param>
        /// <param name="lstParams">�ֶ�</param>
        /// <returns></returns>
        public static string CheckTable(DBInfo info, List<KeyWordTableParamItem> tableInfos) 
        {
            
            List<string> tables = info.DBStructure.GetAllTableName(info);
            BQLDbBase db = new BQLDbBase(info.CreateOperate());
            List<KeyWordTableParamItem> lstNotExists = new List<KeyWordTableParamItem>();
            List<KeyWordTableParamItem> lstExists = new List<KeyWordTableParamItem>();
             FilteExistsTable(tableInfos, tables,lstExists,lstNotExists);

            StringBuilder sbRet = new StringBuilder();
            sbRet.Append(CreateTableSQL(info, lstNotExists));

            foreach (KeyWordTableParamItem existsTable in lstExists)
            {
                sbRet.Append(CheckTableStruct(info, existsTable));
            }
            foreach (KeyWordTableParamItem table in tableInfos)
            {
                sbRet.Append(CheckRelation(info, table));
            }
            return sbRet.ToString();
        }
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="notExists"></param>
        /// <returns></returns>
        private static string CreateTableSQL(DBInfo dbInfo, List<KeyWordTableParamItem> notExists)
        {
            StringBuilder sbSql = new StringBuilder();
            
            foreach (KeyWordTableParamItem table in notExists)
            {
                BQLQuery bql = BQL.CreateTable(table.TableName).Param(table.Params);
                AbsCondition con = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true);
                sbSql.Append(con.GetSql());
                sbSql.Append(";");
            }
            return sbSql.ToString();
        }

        /// <summary>
        /// �ҳ������ڵı�
        /// </summary>
        /// <param name="tableName">Ҫͬ���ı�</param>
        /// <param name="dbTables">���ݿ�ı�</param>
        private static void FilteExistsTable(List<KeyWordTableParamItem> tableNames, 
            List<string> dbTables,List<KeyWordTableParamItem> lstExists,
            List<KeyWordTableParamItem> lstNotExists) 

        {
            
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (string dbTable in dbTables) 
            {
                dic[dbTable.ToLower()] = true;
            }

            foreach (KeyWordTableParamItem table in tableNames) 
            {
                string key = table.TableName.ToLower();
                if (!dic.ContainsKey(key))
                {
                    lstNotExists.Add(table);
                }
                else 
                {
                    lstExists.Add(table);
                }
            }
        }
        
        /// <summary>
        /// ����ṹ
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string CheckTableStruct(DBInfo dbInfo,KeyWordTableParamItem table) 
        {
            
            string tableName = table.TableName;
            BQLQuery bql=BQL.Select(BQL.ToTable(tableName)._).From(BQL.ToTable(tableName));
            SelectCondition con = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true) as SelectCondition;
            string sql = dbInfo.CurrentDbAdapter.GetTopSelectSql(con, 1);
            Dictionary<string, bool> dic = new Dictionary<string, bool>();

            using (IDataReader reader = dbInfo.DefaultOperate.Query(sql, new Buffalo.DB.DbCommon.ParamList())) 
            {
                for (int i = 0; i < reader.FieldCount; i++) 
                {
                    dic[reader.GetName(i).ToLower()] = true;
                }
            }

            StringBuilder sbSql = new StringBuilder();
            foreach (EntityParam pInfo in table.Params) 
            {
                if (!dic.ContainsKey(pInfo.ParamName.ToLower())) 
                {
                    bql = BQL.AlterTable(tableName).AddParam(pInfo);
                    AbsCondition acon = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true);
                    sbSql.Append(acon.GetSql());
                    sbSql.Append(";");
                }
            }
            

            return sbSql.ToString();
        }

        /// <summary>
        /// ����ϵ
        /// </summary>
        /// <param name="dbInfo">���ݿ�</param>
        /// <param name="table">Ҫ���ı�</param>
        /// <returns></returns>
        private static string CheckRelation(DBInfo dbInfo, KeyWordTableParamItem table) 
        {
            List<TableRelationAttribute> lstRelation = dbInfo.DBStructure.GetRelation(dbInfo, table.TableName);
            StringBuilder  sbSql=new StringBuilder();
            foreach (TableRelationAttribute item in table.RelationItems) 
            {
                bool exists = false;
                foreach (TableRelationAttribute existsItem in lstRelation) 
                {
                    if (item.SourceName.Equals(existsItem.SourceName, StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists) 
                {
                    item.CreateName();
                    BQLQuery bql = BQL.AlterTable(table.TableName).AddConstraint(item);
                    AbsCondition con = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true);
                    sbSql.Append(con.GetSql() + ";");
                }
            }


            return sbSql.ToString();

        }



    }

    
}
