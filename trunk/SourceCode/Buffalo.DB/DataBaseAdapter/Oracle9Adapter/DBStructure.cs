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
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : IDBStructure
    {

        #region IDBStructure ��Ա

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetAddParamSQL()
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, string childName)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #endregion
    }
}
