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
        /// <summary>
        /// 获取所有用户表
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAllTableName(DBInfo info)
        {
            string sql = "SELECT Name FROM sysObjects Where XType='U' and Name<>'dtproperties' ORDER BY Name";
            DataBaseOperate oper = info.DefaultOperate;

            List<string> lstName = new List<string>();
            using (IDataReader reader = oper.Query(sql, new ParamList())) 
            {
                while (reader.Read())
                {
                    lstName.Add(reader[0].ToString());
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
        /// <param name="chileName">子表名，查询有这里为nulls</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DBInfo info,string childName) 
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
                    lst.Add(tinfo);
                }
            }
            return lst;

        }

    }
}
