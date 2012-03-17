using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
namespace Buffalo.DB.DataBaseAdapter.SQLiteAdapter
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
            return "CURRENT_TIMESTAMP";
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
            IDataParameter newParam = new SQLiteParameter();
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
        public string GetTopSelectSql(SelectCondition sql, int top)
        {
            StringBuilder sbSql = new StringBuilder(sql.GetSelect());
            //sbSql.Append(sql);
            sbSql.Append(" LIMIT 0, " + top);
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
            IDbCommand comm = new SQLiteCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            DbConnection conn = new SQLiteConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new SQLiteDataAdapter();
            return adapter;
        }

        /// <summary>
        /// ��ʽ���ֶ���
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName)
        {

            return "[" + paramName + "]";
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
            return "@" + pname;
        }

        /// <summary>
        /// ��ʽ�������ļ���
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return pname;
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            return " like '%" + value + "%'";
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
            return CursorPageCutter.Query(sql, null, objPage, oper);
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
            return CursorPageCutter.QueryDataTable(sql, null, objPage, oper, curType);
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
        public IDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
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
        public DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
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
        /// ��ȡ��ʼ���Ʒֺ�����SQL���
        /// </summary>
        /// <returns></returns>
        public string GetInitPointFunSql()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ�жϼƷֺ����Ƿ���ڵ�SQL
        /// </summary>
        /// <returns></returns>
        public string GetPiontFunSqlExistsSql()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ�Ʒֺ���������
        /// </summary>
        public string PointFunName
        {
            get
            {
                return null;
            }
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
                sbRet.Append(curStr + "||");
            }
            string ret = sbRet.ToString();
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 2);
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ�Զ�������SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info)
        {
            return "select LAST_INSERT_ROWID()";
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
        /// �ѱ���ת���SQL����е�ʱ����ʽ
        /// </summary>
        /// <returns></returns>
        public string GetDateTimeString(object value)
        {
            DateTime dt;
            if (!(value is DateTime))
            {
                dt = Convert.ToDateTime(value);
            }
            else
            {
                dt = (DateTime)value;
            }
            return "'" + dt.ToString("s") + "'";
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

        public string GetSequenceName(string tableName, string paramName)
        {
            return null;
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, DataBaseOperate oper)
        {
            return null;
        }

        #endregion

        #region IDBAdapter ��Ա


        public string DBIdentity(string tableName, string paramName)
        {
            return "AUTOINCREMENT";
        }

        public string DBTypeToSQL(DbType dbType, int length)
        {
            //int type = ToRealDbType(dbType, length);            
            //SqliteType stype = (SqlDbType)type;            
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    if (length > 8000)
                    {
                        return "TEXT";
                    }
                    else
                    {
                        return "VARCHAR" + "(" + length + ")";
                    }
                case DbType.Binary:
                    return "BLOB";
                case DbType.Boolean:
                    return "BOOLEAN";
                case DbType.Byte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.UInt16:
                case DbType.SByte:
                    return "INTEGER";
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.Double:
                case DbType.Int64:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return "REAL";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Guid:
                    return "VARCHAR(64)";
                case DbType.Single:
                    return "FLOAT";
                case DbType.String:
                case DbType.StringFixedLength:
                    return "NVARCHAR(" + length + ")";
                case DbType.Time:
                    return "TIME";
                default:
                    return "NUMERIC";
            }

        }

        public int ToRealDbType(DbType dbType, int length)
        {
            return (int)dbType;
        }

        #endregion
    }
}
