using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;



namespace Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTable = "select TABNAME,TYPE,REMARKS from syscat.tables where tabschema=current schema";
        #region IDBStructure 成员

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            ParamList lstParam = new ParamList();
            //填充表
            using (IDataReader reader = oper.Query(_sqlTable, lstParam))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (reader.IsDBNull(0))
                    {
                        continue;
                    }
                    tableInfo.Name = reader["TABNAME"] as string;
                    tableInfo.Description = reader["REMARKS"] as string;
                    tableInfo.IsView=false;
                    string tableType = reader["REMARKS"] as string;
                    if(tableType!=null && tableType.Trim().Equals("V",StringComparison.CurrentCultureIgnoreCase))
                    {
                        tableInfo.IsView = true;
                    }

                    lstName.Add(tableInfo);
                }
            }
            return lstName;
        }

        public string GetAddParamSQL()
        {
            return "ADD COLUMN";
        }

        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region 创建事件
        /// <summary>
        /// 数据库检查时候的事建
        /// </summary>
        /// <param name="arg">当前类型</param>
        /// <param name="dbInfo">数据库类型</param>
        /// <param name="type">检查类型</param>
        /// <param name="lstSQL">SQL语句</param>
        public void OnCheckEvent(object arg, DBInfo dbInfo, CheckEvent type, List<string> lstSQL)
        {

        }

        #endregion
    }
}
