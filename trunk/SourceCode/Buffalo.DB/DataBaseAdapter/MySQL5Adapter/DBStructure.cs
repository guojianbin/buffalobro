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
        private static string _sqlTables = "SELECT table_name,table_type FROM `information_schema`.`TABLES` where table_schema =?dbName;";
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
            
            lstParam.AddNew("?dbName", DbType.String, oper.DataBaseName);
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
            sql.Append("SELECT t1.CONSTRAINT_NAME,t1.TABLE_NAME, t1.COLUMN_NAME, t1.POSITION_IN_UNIQUE_CONSTRAINT,  t1.REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t1  INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS t2  ON t2.TABLE_SCHEMA = t1.TABLE_SCHEMA  AND t2.TABLE_NAME = t1.TABLE_NAME  AND t2.CONSTRAINT_NAME = t1.CONSTRAINT_NAME WHERE t1.TABLE_SCHEMA = ?dbName  AND t2.CONSTRAINT_TYPE = 'FOREIGN KEY'");
            
            ParamList lstParam = new ParamList();
            lstParam.AddNew("?dbName", DbType.String, oper.DataBaseName);
            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);

            if (!string.IsNullOrEmpty(childName))
            {
                sql.Append(" and t1.TABLE_NAME in(" + childName + ")");
            }

            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sql.ToString(), lstParam))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["CONSTRAINT_NAME"] as string;
                    tinfo.SourceTable = reader["TABLE_NAME"] as string;
                    tinfo.SourceName = reader["COLUMN_NAME"] as string;
                    tinfo.TargetTable = reader["REFERENCED_TABLE_NAME"] as string;
                    tinfo.TargetName = reader["REFERENCED_COLUMN_NAME"] as string;
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
            string inTable = Buffalo.DB.DataBaseAdapter.MySQL5Adapter.DBStructure.AllInTableNames(tableNames);
            string sql = "SELECT t1.TABLE_NAME,t2.TABLE_TYPE,t2.TABLE_COMMENT,t1.COLUMN_NAME,t1.COLUMN_COMMENT, t1.DATA_TYPE, t1.CHARACTER_OCTET_LENGTH, t1.NUMERIC_PRECISION, t1.NUMERIC_SCALE, CASE t1.IS_NULLABLE WHEN 'NO' THEN 0 ELSE 1 END IS_NULLABLE, t1.COLUMN_TYPE,t1.COLUMN_KEY,t1.EXTRA FROM INFORMATION_SCHEMA.COLUMNS t1 inner join INFORMATION_SCHEMA.TABLES t2 on t1.TABLE_NAME=t2.TABLE_NAME WHERE t1.TABLE_SCHEMA = ?dbName";
            string tableNamesSql = "";
            if (!string.IsNullOrEmpty(inTable))
            {
                tableNamesSql = " and d.[name] in(" + inTable + ")";
            }

            List<DBTableInfo> lst = new List<DBTableInfo>();
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            ParamList lstParam = new ParamList();
            lstParam.AddNew("?dbName", DbType.String, oper.DataBaseName);
            using (IDataReader reader = oper.Query(sql.ToString(), lstParam))
            {

                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"] as string;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        continue;
                    }
                    DBTableInfo table = null;
                    dicTables.TryGetValue(tableName, out table);
                    if (table == null)
                    {
                        table = new DBTableInfo();
                        table.Name = tableName;

                        string type = reader["TABLE_TYPE"] as string;
                        table.IsView = false;
                        if (!string.IsNullOrEmpty(type))
                        {
                            if (type.Trim() == "VIEW")
                            {
                                table.IsView = true;
                            }
                        }
                        if (!table.IsView)
                        {
                            table.Description = reader["TABLE_COMMENT"] as string;
                        }
                        table.RelationItems = new List<TableRelationAttribute>();
                        table.Params = new List<EntityParam>();
                        lst.Add(table);
                        dicTables[table.Name] = table;
                    }
                    FillParam(table, reader);

                }
            }
            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, tableNames);
            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.FillRelation(dicTables, lstRelation);

            return lst;
        }

        /// ����ֶ���Ϣ
        /// </summary>
        /// <param name="prm">�ֶ���Ϣ</param>
        /// <param name="reader">reader</param>
        private void FillParam(DBTableInfo table, IDataReader reader)
        {
            string prmName = reader["COLUMN_NAME"] as string;
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
            bool isIdentity = false;
            bool isPrimaryKey = false;
            string extra=reader["EXTRA"] as string;
            if (!string.IsNullOrEmpty(extra) && extra.Trim().Equals("auto_increment",StringComparison.CurrentCultureIgnoreCase)) 
            {
                isIdentity = true;
            }
            string columnKey = reader["COLUMN_KEY"] as string;
            if (!string.IsNullOrEmpty(columnKey) && columnKey.Trim().Equals("PRI", StringComparison.CurrentCultureIgnoreCase))
            {
                isPrimaryKey = true;
            }
            if (isPrimaryKey)
            {
                type = EntityPropertyType.PrimaryKey;

            }
            if (isIdentity)
            {
                type = type | EntityPropertyType.Identity;
            }
            prm.PropertyType = type;

            if (!(reader["CHARACTER_OCTET_LENGTH"] is DBNull)) 
            {
                prm.Length = Convert.ToInt64(reader["CHARACTER_OCTET_LENGTH"]);
            }
            
            if (!table.IsView)
            {
                prm.Description = reader["TABLE_COMMENT"] as string;
            }

            prm.AllowNull = Convert.ToInt32(reader["IS_NULLABLE"])==1;

            string strDBType = reader["COLUMN_TYPE"] as string;
            bool isUnsigned = strDBType.IndexOf("unsigned") > -1;

            string strDataType= reader["DATA_TYPE"] as string;
            prm.SqlType = GetDbType(strDBType, isUnsigned);
            table.Params.Add(prm);
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
        /// <summary>
        /// ��ȡ���ݿ�����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isUnsigned"></param>
        /// <returns></returns>
        private static DbType GetDbType(string type, bool isUnsigned)
        {
            switch (type.ToLower())
            {
                case "bit":
                    return DbType.Boolean;

                case "tinyint":
                    if (isUnsigned)
                    {
                        return DbType.Byte;
                    }
                    return DbType.SByte;

                case "smallint":
                    if (isUnsigned)
                    {
                        return DbType.UInt16;
                    }
                    return DbType.Int16;

                case "mediumint":
                case "int":
                    if (isUnsigned)
                    {
                        return DbType.UInt32;
                    }
                    return DbType.Int32;

                case "bigint":
                    if (isUnsigned)
                    {
                        return DbType.UInt64;
                    }
                    return DbType.Int64;

                case "float":
                    return DbType.Single;

                case "double":
                    return DbType.Double;

                case "decimal":
                    return DbType.Decimal;

                case "date":
                    return DbType.Date;

                case "datetime":
                    return DbType.DateTime;

                case "timestamp":
                    return DbType.DateTime;

                case "time":
                    return DbType.Time;

                case "tinytext":
                case "mediumtext":
                case "longtext":
                case "text":
                    return DbType.String;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "varchar":
                    return DbType.String;

                case "blob":
                    return DbType.Binary;
            }
            return DbType.Object;
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
