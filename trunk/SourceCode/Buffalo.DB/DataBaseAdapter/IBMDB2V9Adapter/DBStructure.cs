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
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTable = "select TABNAME,TYPE,REMARKS from syscat.tables where tabschema=current schema";
        #region IDBStructure ��Ա

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            ParamList lstParam = new ParamList();
            //����
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
