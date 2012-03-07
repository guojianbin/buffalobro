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
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTables = "SELECT * FROM [SQLITE_MASTER] WHERE [Name] NOT IN('sqlite_sequence')";
        #region IDBStructure ��Ա

        /// <summary>
        /// ��ȡ���ݿ��е����б�      
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            ParamList lstParam = new ParamList();
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            ///ֻ�ܻ�ȡ����,������û��
            using (IDataReader reader = oper.Query(_sqlTables, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader["name"] as string;
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
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// ��ȡ���й�ϵ
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
        /// ��ȡ����Ϣ
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
        /// ����ֶ���Ϣ
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
            prm.PropertyType = type;
            string strDBType = reader["type"] as string;
            prm.SqlType = GetDbType(strDBType);
            table.Params.Add(prm);
        }
        /// <summary>
        /// ��ȡDbType
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        private DbType GetDbType(string nativeType)
        {
            if (string.IsNullOrEmpty(nativeType))
            {
                return DbType.Object;
            }
            string typeName = nativeType.Trim().ToUpper();
            switch (typeName)
            {
                case "BOOLEAN":
                    return DbType.Boolean;
                case "BLOB":
                    return DbType.Binary;
                case "INTEGER":
                    return DbType.Int32;
                case "REAL":
                    return DbType.Double;
                case "DATE":
                    return DbType.Date;
                case "TIMESTAMP":
                    return DbType.DateTime;
                case "VARCHAR":
                case "NVARCHAR":
                case "TEXT":
                    return DbType.String;
                case "FLOAT":
                    return DbType.Single;
                case "TIME":
                    return DbType.Time;
            }
            return DbType.Object;
        #endregion
        }
    }
}
