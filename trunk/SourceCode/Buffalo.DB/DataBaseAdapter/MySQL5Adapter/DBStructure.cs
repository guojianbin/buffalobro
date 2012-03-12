using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;



namespace Buffalo.DB.DataBaseAdapter.MySQL5Adapter
{
    /// <summary>
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTables = "SELECT table_schema,table_name,table_type FROM `information_schema`.`TABLES` where table_schema <> 'information_schema';";
        #region IDBStructure ��Ա

        /// <summary>
        /// ��ȡ�����û���
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            ParamList lstParam = new ParamList();

            List<DBTableInfo> lstName = new List<DBTableInfo>();
            using (IDataReader reader = oper.Query(_sqlTables, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader[0] as string;
                    string type = reader[1] as string;
                    if (!string.IsNullOrEmpty(type))
                    {
                        if (type.Trim() == "VIEW")
                        {
                            tableInfo.IsView = true;
                        }
                    }
                    lstName.Add(tableInfo);
                }
            }
            return lstName;

        }


        /// <summary>
        /// ����ֶε����
        /// </summary>
        /// <returns></returns>
        public string GetAddParamSQL()
        {
            return "add";
        }

        /// <summary>
        /// ��ȡ���й�ϵ
        /// </summary>
        /// <param name="oper"> </param>
        /// <param name="info"> </param>
        /// <param name="childNames">null���ѯ���б�</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            StringBuilder sql = new StringBuilder();
            //sql.Append("SELECT constraint_schema,constraint_name,unique_constraint_name,table_name,referenced_table_name FROM `information_schema`.`REFERENTIAL_CONSTRAINTS`;");
            sql.Append("SELECT a.constraint_schema,a.constraint_name,a.unique_constraint_name,a.table_name,b.column_name,a.referenced_table_name,b.referenced_column_name FROM `information_schema`.`REFERENTIAL_CONSTRAINTS` a ");
            sql.Append(" left join `information_schema`.`KEY_COLUMN_USAGE` b ");
            sql.Append(" on a.constraint_name =b.constraint_name ");
            ParamList lstParam = new ParamList();

            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);

            if (!string.IsNullOrEmpty(childName))
            {
                sql.Append(" where table_name in(" + childName + ")");
            }

            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sql.ToString(), lstParam))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["constraint_name"] as string;
                    tinfo.SourceTable = reader["table_name"] as string;
                    tinfo.SourceName = reader["column_name"] as string;
                    tinfo.TargetTable = reader["referenced_table_name"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            return lst;
        }


        /// <summary>
        /// ��ȡ����Ϣ
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            throw new NotImplementedException();
            string inTable = Buffalo.DB.DataBaseAdapter.MySQL5Adapter.DBStructure.AllInTableNames(tableNames);
            ////string sql =return Resource.ResourceManager.g
            //string tableNamesSql = "";
            //if (!string.IsNullOrEmpty(inTable))
            //{
            //    tableNamesSql = " and d.[name] in(" + inTable + ")";
            //}
            //sql = sql.Replace("<%=TableNames%>", tableNamesSql);

            //List<DBTableInfo> lst = new List<DBTableInfo>();
            //Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            //using (IDataReader reader = oper.Query(sql.ToString(), new ParamList()))
            //{

            //    while (reader.Read())
            //    {
            //        string tableName = reader["tableName"] as string;
            //        if (string.IsNullOrEmpty(tableName))
            //        {
            //            continue;
            //        }
            //        DBTableInfo table = null;
            //        dicTables.TryGetValue(tableName, out table);
            //        if (table == null)
            //        {
            //            table = new DBTableInfo();
            //            table.Name = tableName;

            //            string type = reader["tabletype"] as string;
            //            table.IsView = false;
            //            if (!string.IsNullOrEmpty(type))
            //            {
            //                if (type.Trim() == "V")
            //                {
            //                    table.IsView = true;
            //                }
            //            }
            //            if (!table.IsView)
            //            {
            //                table.Description = reader["tableDescription"] as string;
            //            }
            //            table.RelationItems = new List<TableRelationAttribute>();
            //            table.Params = new List<EntityParam>();
            //            lst.Add(table);
            //            dicTables[table.Name] = table;
            //        }
            //        FillParam(table, reader);

            //    }
        }

        /// <summary>
        /// ��ȡҪIn�ı���
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        internal static string AllInTableNames(IEnumerable<string> tableNames)
        {
            if (tableNames == null)
            {
                return null;
            }
            StringBuilder sbTables = new StringBuilder();
            foreach (string tableName in tableNames)
            {
                sbTables.Append("'");
                sbTables.Append(tableName.Replace("'", "''"));
                sbTables.Append("',");
            }
            if (sbTables.Length > 0)
            {
                sbTables.Remove(sbTables.Length - 1, 1);
            }
            return sbTables.ToString();
        }
        #endregion

        #region �����¼�
        /// <summary>
        /// ���ݿ���ʱ����½�
        /// </summary>
        /// <param name="arg">��ǰ����</param>
        /// <param name="dbInfo">���ݿ�����</param>
        /// <param name="type">�������</param>
        /// <param name="lstSQL">SQL���</param>
        public void OnCheckEvent(object arg, DBInfo dbInfo, CheckEvent type, List<string> lstSQL)
        {

        }

        #endregion
    }
}
