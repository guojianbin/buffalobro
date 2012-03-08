using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;



namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {

        private static string _sqlTables = "select \"TABLE_NAME\"  from user_tables";
        private static string _sqlViews = "select \"VIEW_NAME\" from user_views";
        #region IDBStructure 成员

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            ParamList lstParam = new ParamList();

            List<DBTableInfo> lstName = new List<DBTableInfo>();
            //填充表
            using (IDataReader reader = oper.Query(_sqlTables, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if(reader.IsDBNull(0))
                    {
                        continue;
                    }
                    tableInfo.Name = reader[0] as string;
                    tableInfo.IsView = false;
                    
                    lstName.Add(tableInfo);
                }
            }

            //填充视图
            using (IDataReader reader = oper.Query(_sqlViews, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (reader.IsDBNull(0))
                    {
                        continue;
                    }
                    tableInfo.Name = reader[0] as string;
                    tableInfo.IsView = false;

                    lstName.Add(tableInfo);
                }
            }

            return lstName;
        }

        public string GetAddParamSQL()
        {
            return "add";
        }


        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            StringBuilder sql = new StringBuilder(1024);
            ParamList lstParam = new ParamList();
            sql.Append("select b.table_name as pktable_name,b.column_name pkcolumn_name,c.table_name fktable_name,c.column_name fkcolumn_name,c.position ke_seq,c.constraint_name fk_name from (select * from user_cons_columns ) b left join (select * from user_constraints where user_constraints.constraint_type='R' ) a on  b.constraint_name=a.r_constraint_name left join user_cons_columns c on  c.constraint_name=a.constraint_name where c.position is not null and c.position=b.position ");

            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);
            if (!string.IsNullOrEmpty(childName)) 
            {
                sql.Append("and c.table_name in(" + childName + ")");
            }
            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sql.ToString(), lstParam))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["FK_NAME"] as string;
                    tinfo.SourceTable = reader["FKTABLE_NAME"] as string;
                    tinfo.SourceName = reader["FKCOLUMN_NAME"] as string;
                    tinfo.TargetTable = reader["PKTABLE_NAME"] as string;

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
