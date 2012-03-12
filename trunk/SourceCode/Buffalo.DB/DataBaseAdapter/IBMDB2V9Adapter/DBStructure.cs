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

        #region IDBStructure 成员

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetAddParamSQL()
        {
            throw new Exception("The method or operation is not implemented.");
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
