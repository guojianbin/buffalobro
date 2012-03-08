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
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTables = "SELECT table_schema,table_name,table_type FROM `information_schema`.`TABLES` where table_schema <> 'information_schema';";
        #region IDBStructure 成员

        /// <summary>
        /// 获取所有用户表
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
        /// 添加字段的语句
        /// </summary>
        /// <returns></returns>
        public string GetAddParamSQL()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="oper"> </param>
        /// <param name="info"> </param>
        /// <param name="chileName">null则查询所有表</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT constraint_schema,constraint_name,unique_constraint_name,table_name,referenced_table_name FROM `information_schema`.`REFERENTIAL_CONSTRAINTS`;");
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
                    tinfo.SourceName = reader["constraint_schema"] as string; //todo:干嘛用？什么意思？
                    tinfo.TargetTable = reader["referenced_table_name"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            return lst;
        }


        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
