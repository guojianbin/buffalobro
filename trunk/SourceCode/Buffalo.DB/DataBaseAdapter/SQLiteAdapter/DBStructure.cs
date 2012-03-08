using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;



namespace Buffalo.DB.DataBaseAdapter.SQLiteAdapter
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTables = "SELECT * FROM [SQLITE_MASTER] WHERE [Name] NOT IN('sqlite_sequence')";
        #region IDBStructure 成员

        /// <summary>
        /// 获取数据库中的所有表      
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            ParamList lstParam = new ParamList();
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            ///只能获取表名,其它的没用
            using (IDataReader reader = oper.Query(_sqlTables, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader["name"] as string;
                    tableInfo.IsView = (reader["type"] as string) == "view";
                    lstName.Add(tableInfo);
                }
            }
            return lstName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddParamSQL()
        {
            return "add column";
        }
        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, string childName)
        {
            return null;
        }


        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, List<string> tableNames)
        {
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            List<DBTableInfo> lst = new List<DBTableInfo>();
            foreach (string tableName in tableNames)
            {
                String sql = String.Format("PRAGMA table_info([{0}])", tableName);

                DBTableInfo table = null;
                dicTables.TryGetValue(tableName, out table);
                if (table == null)
                {
                    table = new DBTableInfo();
                    table.Name = tableName;
                    table.RelationItems = new List<TableRelationAttribute>();
                    table.Params = new List<EntityParam>();
                    lst.Add(table);
                    dicTables[table.Name] = table;
                }
                using (IDataReader reader = oper.Query(sql.ToString(), new ParamList()))
                {
                    while (reader.Read())
                    {
                        FillParam(table, reader);
                    }
                }


            }
            return lst;
        }
        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="reader"></param>
        private void FillParam(DBTableInfo table, IDataReader reader)
        {
            string prmName = reader["name"] as string;
            if (string.IsNullOrEmpty(prmName))
            {
                return;
            }

            foreach (EntityParam ep in table.Params)
            {
                if (ep.ParamName == prmName)
                {
                    return;
                }
            }

            EntityParam prm = new EntityParam();
            prm.ParamName = prmName;

            EntityPropertyType type = EntityPropertyType.Normal;
            int isPrimary = Convert.ToInt32(reader["pk"]);
            if (isPrimary == 1)
            {
                type = EntityPropertyType.PrimaryKey;
                type = type | EntityPropertyType.Identity;
            }

            bool allowNull = Convert.ToInt32(reader["notnull"])==0;
            prm.AllowNull = allowNull;
            prm.PropertyType = type;
            string strDBType = reader["type"] as string;
            FillDbType(strDBType,prm);
            table.Params.Add(prm);
        }



        /// <summary>
        /// 填充数据库类型
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        private void FillDbType(string nativeType,EntityParam prm)
        {
            if (string.IsNullOrEmpty(nativeType))
            {
                return;
            }
            string typeName=null;
            int length=0;

            //截取长度
            int index = nativeType.IndexOf("(");
            int lastIndex=nativeType.IndexOf(")");
            if (index > 0 && lastIndex>index) 
            {
                typeName = nativeType.Substring(0, index);

                string strLen = nativeType.Substring(index + 1, lastIndex - index);

                int.TryParse(strLen, out length);
            }


            DbType type=DbType.Object;

            //string typeName = nativeType.Trim().ToUpper();
            switch (typeName)
            {
                case "BOOLEAN":
                    type=DbType.Boolean;
                    break;
                case "BLOB":
                    type= DbType.Binary;
                    break;
                case "INTEGER":
                    type= DbType.Int32;
                    break;
                case "REAL":
                    type= DbType.Double;
                    break;
                case "DATE":
                    type= DbType.Date;
                    break;
                case "TIMESTAMP":
                    type= DbType.DateTime;
                    break;
                case "VARCHAR":
                case "NVARCHAR":
                case "TEXT":
                    type= DbType.String;
                    break;
                case "FLOAT":
                    type= DbType.Single;
                    break;
                case "TIME":
                    type= DbType.Time;
                    break;
                default:
                    break;
            }
            
            prm.SqlType=type;
            prm.Length=length;
        }

        #endregion
    }
}
