using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.DB.DataBaseAdapter.MySQL5Adapter
{
    public class DBAdapter : IDBAdapter
    {
        /// <summary>
        /// ȫ������ʱ�������ֶ��Ƿ���ʾ���ʽ
        /// </summary>
        public virtual bool IsShowExpression
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Time:
                    return "curtime()";
                case DbType.Date:
                    return "curdate()";
                default:
                    return "now()";
            }

            return "now()";
        }

        /// <summary>
        /// �Ƿ��¼�������ֶ����ֶ�����
        /// </summary>
        public bool IsSaveIdentityParam
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        public ParamList BQLSelectParamList
        {
            get
            {
                return new ParamList();
            }
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="paramName">������</param>
        /// <param name="type">�������ݿ�����</param>
        /// <param name="paramValue">����ֵ</param>
        /// <param name="paramDir">������������</param>
        /// <returns></returns>
        public IDataParameter GetDataParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir) 
        {
            
            IDataParameter newParam = new MySqlParameter();
            newParam.ParameterName = paramName;
            newParam.DbType = type;
            newParam.Value = paramValue;
            newParam.Direction = paramDir;
            return newParam;
        }

        /// <summary>
        /// ��ȡtop�Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="sql">��ѯ�ַ���</param>
        /// <param name="top">topֵ</param>
        /// <returns></returns>
        public string GetTopSelectSql(SelectCondition sql,int top) 
        {
            StringBuilder sbSql = new StringBuilder(sql.GetSelect());
            //sbSql.Append(sql);
            sbSql.Append("LIMIT 0, " + top);
            return sbSql.ToString();
        }

        /// <summary>
        /// ����������ת���ɵ�ǰ���ݿ�֧�ֵ�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbType ToCurrentDbType(DbType type)
        {
            return type;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetCommand() 
        {
            IDbCommand comm = new MySqlCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new MySqlConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new MySqlDataAdapter();
            return adapter;
        }

        /// <summary>
        /// ��ʽ���ֶ���
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName) 
        {

            return "`" + paramName + "`";
        }

        /// <summary>
        /// ��ʽ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatTableName(string tableName)
        {
            return FormatParam(tableName);
        }
        /// <summary>
        /// ��ʽ��������
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatValueName(string pname)
        {
            return "?" + pname;
        }

        /// <summary>
        /// ��ʽ�������ļ���
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return "?" + pname;
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value) 
        {
            return " (MATCH(" + paranName + ") AGAINST("+value+"))";
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("MySQL������FreeText����");
        }
        /// <summary>
        /// �α��ҳ
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳʵ��</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        public IDataReader Query(string sql, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql,null, objPage, oper);
        }

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql,null, objPage, oper, curType);
        }
        
        
        /// <summary>
        /// �α��ҳ
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="lstParam">��������</param>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳʵ��</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        public IDataReader Query(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql, lstParam, objPage, oper);
        }

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql, lstParam, objPage, oper, curType);
        }
        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage);
        }
        
        
        /// <summary>
        /// ��ȡ�ַ���ƴ��SQl���
        /// </summary>
        /// <param name="str">�ַ�������</param>
        /// <returns></returns>
        public string ConcatString(params string[] strs)
        {
            StringBuilder sbRet = new StringBuilder();
            
            foreach (string curStr in strs)
            {
                sbRet.Append(curStr + ",");
            }
            string ret = sbRet.ToString();
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }

            return "concat("+ret+")";
        }
        /// <summary>
        /// ��ȡ�Զ�������SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info) 
        {
            return "SELECT LAST_INSERT_ID()";
        }

        /// <summary>
        /// �ѱ���ת���SQL����е�ʱ����ʽ
        /// </summary>
        /// <returns></returns>
        public string GetDateTimeString(object value)
        {
            return "'" + value.ToString().Replace("'","") + "'";
        }

        // <summary>
        /// ����ʱ���Զ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info) 
        {
            return null;
        }
        /// <summary>
        /// ����ʱ��������ֶ�ֵ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetInsertPKParamValue(EntityPropertyInfo info)
        {
            return null;
        }
        /// <summary>
        /// ��ȡ�Զ�����ֵ��SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
            return null;
        }
        
        /// <summary>
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info)
        {
            SqlServer2KAdapter.DBAdapter.ValueFromReader(reader, index, arg, info);
        }

        #region IDBAdapter ��Ա


        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return "";
        }

        public string GetSequenceName(EntityPropertyInfo info)
        {
            return null;
        }
        /// <summary>
        ///  ��ȡĬ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName)
        {
            return null;
        }
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, EntityParam prm, DataBaseOperate oper)
        {
            return null;
        }


        public string DBIdentity(string tableName, string paramName)
        {
            return "auto_increment";
        }

        public string DBTypeToSQL(DbType dbType, long length)
        {
            switch (dbType)
            {
                case DbType.Boolean:
                    return "bit";

                case DbType.Byte:
                    return "tinyint unsigned";
                    
                case DbType.SByte:
                    return "tinyint";

                case DbType.UInt16:
                    return "smallint unsigned";
                case DbType.Int16:
                    return "smallint";
                   
                case DbType.UInt32:
                    return "int unsigned";
                case DbType.Int32:
                    return "int";

                case DbType.UInt64:
                    return "bigint unsigned";
                case DbType.Int64:
                    return "bigint";

                case DbType.Single:
                    return "float";

                case DbType.Double:
                    return "double";
                case DbType.Currency:
                case DbType.VarNumeric:
                case DbType.Decimal:
                    return "decimal";

                case DbType.Date:
                    return "date";

                case DbType.DateTime:
                    return "datetime";
                case DbType.DateTimeOffset:
                case DbType.DateTime2:
                    return "timestamp";
                case DbType.Time:
                    return "time";
                case DbType.AnsiStringFixedLength:
                    if (length > 8000) 
                    {
                        return "longtext";
                    }
                    return "char("+length+")";
                case DbType.StringFixedLength:
                    if (length > 8000)
                    {
                        return "longtext";
                    }
                    return "char(" + length + ")";
                case DbType.AnsiString:
                    if (length > 8000)
                    {
                        return "longtext";
                    }
                    return "varchar(" + length + ")";
                case DbType.String:
                    if (length > 8000)
                    {
                        return "longtext";
                    }
                    return "varchar(" + length + ")";
                case DbType.Binary:
                    if (length > 8000)
                    {
                        return "longblob";
                    }
                    return "blob(" + length + ")";
                default:
                    return "";
            }
        }

        public int ToRealDbType(DbType dbType, long length)
        {

            MySqlParameter prm = new MySqlParameter();
            prm.DbType = dbType;
            prm.ParameterName = "name";
            return (int)prm.MySqlDbType;
        }
        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
            conn.Dispose();
            return true;
        }
        #endregion
    }
}
