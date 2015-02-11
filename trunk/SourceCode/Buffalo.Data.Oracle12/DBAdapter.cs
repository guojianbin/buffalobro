using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using Oracle.DataAccess.Client;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    public class DBAdapter : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.DBAdapter
    {
        
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="paramName">������</param>
        /// <param name="type">�������ݿ�����</param>
        /// <param name="paramValue">����ֵ</param>
        /// <param name="paramDir">������������</param>
        /// <returns></returns>
        public override IDataParameter GetDataParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir)
        {
            IDataParameter newParam = new OracleParameter();
            newParam.ParameterName = paramName;
            newParam.DbType = type;
            newParam.Value = paramValue;
            newParam.Direction = paramDir;
            return newParam;
        }

       
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public override IDbCommand GetCommand()
        {
            IDbCommand comm = new OracleCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public override DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new OracleConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public override IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new OracleDataAdapter();
            return adapter;
        }

    }
}
