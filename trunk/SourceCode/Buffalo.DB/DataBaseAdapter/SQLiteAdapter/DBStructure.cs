using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;


namespace Buffalo.DB.DataBaseAdapter.SQLiteAdapter
{
    /// <summary>
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : IDBStructure
    {

        #region IDBStructure ��Ա

        public List<string> GetAllTableName(DBInfo info)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetAddParamSQL()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
