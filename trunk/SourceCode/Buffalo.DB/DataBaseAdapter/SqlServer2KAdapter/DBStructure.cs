using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;



namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    


    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : Buffalo.DB.DataBaseAdapter.IDbAdapters.IDBStructure
    {
        private static string _sqlTables = "SELECT [name],[xtype] FROM [sysobjects] Where [xtype] in ('U','V') and [name] not in('dtproperties','sysdiagrams') ORDER BY [xtype],[crdate] desc";
        
        /// <summary>
        /// 获取所有用户表
        /// </summary>
        /// <returns></returns>
        public virtual List<DBTableInfo> GetAllTableName(DataBaseOperate oper,DBInfo info)
        {
            ParamList lstParam = new ParamList();
            //DataBaseOperate oper = info.DefaultOperate;

            List<DBTableInfo> lstName = new List<DBTableInfo>();
            using (IDataReader reader = oper.Query(_sqlTables, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader[0] as string;
                    string type=reader[1] as string;
                    if(!string.IsNullOrEmpty(type))
                    {
                        if(type.Trim()=="V")
                        {
                            tableInfo.IsView=true;
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
        public virtual string GetAddParamSQL() 
        {
            return "add";
        }
        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="chileName">null则查询所有表</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, string childName) 
        {
            StringBuilder sql =new StringBuilder();

            sql.Append("select * from (select fk.name fkname ,ftable.[name] childname, cn.[name] childparam, rtable.[name] parentname from sysforeignkeys join sysobjects fk on sysforeignkeys.constid = fk.id join sysobjects ftable on sysforeignkeys.fkeyid = ftable.id join sysobjects rtable on sysforeignkeys.rkeyid = rtable.id join syscolumns cn on sysforeignkeys.fkeyid = cn.id and sysforeignkeys.fkey = cn.colid) a where 1=1");
            ParamList lstParam=new ParamList();
            if(!string.IsNullOrEmpty(childName))
            {
                sql.Append(" and childname=@childName");
                lstParam.AddNew("childName", DbType.AnsiString, childName);
            }

            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sql.ToString(), lstParam)) 
            {
                while (reader.Read()) 
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["fkname"] as string;
                    tinfo.SourceTable = reader["childname"] as string;
                    tinfo.SourceName = reader["childparam"] as string;
                    tinfo.TargetTable = reader["parentname"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            return lst;

        }

        /// <summary>
        /// 初始化获取表名语句
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTableParamsSQL()
        {
            
            return Resource.tableParam2000;
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
            StringBuilder sbAllTable = new StringBuilder();
            foreach (string tableName in tableNames)
            {
                sbAllTable.Append("'");
                sbAllTable.Append(tableName);
                sbAllTable.Append("',");
            }
            if (sbAllTable.Length > 0)
            {
                sbAllTable.Remove(sbAllTable.Length - 1, 1);
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from (");
            sql.Append(GetTableParamsSQL());
            sql.Append(") tmp where 1=1");
            if (sbAllTable.Length > 0)
            {
                sql.Append(" and tableName in (");
                sql.Append(sbAllTable.ToString());
                sql.Append(")");
            }
            List<DBTableInfo> lst = new List<DBTableInfo>();
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            using (IDataReader reader = oper.Query(sql.ToString(), new ParamList()))
            {
               
                while (reader.Read())
                {
                    string tableName = reader["tableName"] as string;
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
                        
                        string type = reader["tabletype"] as string;
                        table.IsView = false;
                        if (!string.IsNullOrEmpty(type))
                        {
                            if (type.Trim() == "V")
                            {
                                table.IsView = true;
                            }
                        }
                        if (!table.IsView)
                        {
                            table.Description = reader["tableDescription"] as string;
                        }
                        table.RelationItems = new List<TableRelationAttribute>();
                        table.Params = new List<EntityParam>();
                        lst.Add(table);
                        dicTables[table.Name] = table;
                    }
                    EntityParam prm = new EntityParam();
                    FillParam(table,prm, reader);
                    table.Params.Add(prm);
                }
            }

            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, null);
            DBTableInfo ptable = null;
            DBTableInfo ctable = null;

            foreach (TableRelationAttribute tinfo in lstRelation) 
            {

                if (dicTables.TryGetValue(tinfo.SourceTable, out ctable) && dicTables.TryGetValue(tinfo.TargetTable, out ptable)) //填充父项
                {
                    tinfo.TargetName=ptable.PrimaryParam.ParamName;
                    ctable.RelationItems.Add(tinfo);

                    TableRelationAttribute cinfo = new TableRelationAttribute();
                    cinfo.SourceName=tinfo.TargetName;
                    cinfo.SourceTable=tinfo.TargetTable;
                    cinfo.TargetName=tinfo.SourceName;
                    cinfo.TargetTable=tinfo.SourceTable;
                    cinfo.IsParent=false;
                    ptable.RelationItems.Add(cinfo);
                }

                
            }

            return lst;
        }

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="prm">字段信息</param>
        /// <param name="reader">reader</param>
        private void FillParam(DBTableInfo table, EntityParam prm, IDataReader reader) 
        {
           
            prm.ParamName = reader["paramName"] as string;
            
            EntityPropertyType type = EntityPropertyType.Normal;
            int isPrimary = Convert.ToInt32(reader["isPrimary"]);
            int isIdentity = Convert.ToInt32(reader["isIdentity"]);
            if (isPrimary == 1) 
            {
                type = EntityPropertyType.PrimaryKey;
            }
            if (isIdentity == 1) 
            {
                type = type | EntityPropertyType.Identity;
            }
            prm.PropertyType = type;
            prm.Length = Convert.ToInt32(reader["length"]);
            if (!table.IsView)
            {
                prm.Description = reader["paramDescription"] as string;
            }
            string strDBType= reader["dbType"] as string;
            prm.SqlType = GetDbType(strDBType);

        }

        /// <summary>
        /// 获取DbType
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        private DbType GetDbType(string nativeType)
        {
            if (string.IsNullOrEmpty(nativeType)) 
            {
                return DbType.Object;
            }
            string typeName=nativeType.Trim().ToLower();
            switch (typeName)
            {
                case "bigint":
                    return DbType.Int64;

                case "binary":
                    return DbType.Binary;

                case "bit":
                    return DbType.Boolean;

                case "char":
                    return DbType.AnsiStringFixedLength;

                case "datetime":
                    return DbType.DateTime;

                case "decimal":
                    return DbType.Decimal;

                case "float":
                    return DbType.Single;

                case "image":
                    return DbType.Boolean;

                case "int":
                    return DbType.Int32;

                case "money":
                    return DbType.Decimal;

                case "nchar":
                    return DbType.StringFixedLength;

                case "ntext":
                    return DbType.String;

                case "numeric":
                    return DbType.Decimal;

                case "nvarchar":
                    return DbType.String;

                case "real":
                    return DbType.Decimal;

                case "smalldatetime":
                    return DbType.DateTime;

                case "smallint":
                    return DbType.Int16;

                case "smallmoney":
                    return DbType.Double;

                case "sql_variant":
                    return DbType.Object;

                case "sysname":
                    return DbType.StringFixedLength;

                case "text":
                    return DbType.String;

                case "timestamp":
                    return DbType.DateTimeOffset;

                case "tinyint":
                    return DbType.Int16;

                case "uniqueidentifier":
                    return DbType.Guid;

                case "varbinary":
                    return DbType.Binary;

                case "varchar":
                    return DbType.AnsiString;

                case "xml":
                    return DbType.Xml;

                case "datetime2":
                    return DbType.DateTime2;

                case "time":
                    return DbType.Time;

                case "date":
                    return DbType.Date;

                case "datetimeoffset":
                    return DbType.DateTimeOffset;
            }
            return DbType.Object;
        }
    }
}
