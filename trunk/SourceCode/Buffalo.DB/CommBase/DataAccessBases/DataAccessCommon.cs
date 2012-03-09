using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using Buffalo.DB.FaintnessSearchConditions;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    public class DataAccessCommon
    {

        /// <summary>
        /// ��ȡ���������ַ���
        /// </summary>
        /// <param name="objScope"></param>
        /// <returns></returns>
        public static string GetConnectString(Scope objScope)
        {
            if (objScope.ConnectType == ConnectType.And)
            {
                return "And";
            }
            return "Or";
        }


        /// <summary>
        /// ��ʽ����������
        /// </summary>
        /// <param name="pam">����</param>
        /// <param name="index">��ǰ�ı�ʶ</param>
        /// <returns></returns>
        public static string FormatParam(string pam, int index)
        {
            return pam + "_" + index.ToString();
        }

        /// <summary>
        /// ����ѯ����������������SQL���( and ��ͷ)
        /// </summary>
        /// <param name="lstParam">�����б�</param>
        /// <param name="lstScope">��Χ��ѯ����</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo,ParamList lstParam, ScopeList lstScope)
        {
            int index = 0;
            return FillCondition(curEntityInfo,lstParam, lstScope, ref index);
        }


        /// <summary>
        /// ����ѯ����������������SQL���( and ��ͷ)
        /// </summary>
        /// <param name="lstParam">�����б�</param>
        /// <param name="lstScope">��Χ��ѯ����</param>
        /// <param name="CurEntityInfo">��ǰʵ����Ϣ</param>
        /// <param name="index">����</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo, ParamList lstParam, ScopeList lstScope, ref int index)
        {
            if (lstScope == null)
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < lstScope.Count; i++)
            {
                Scope objScope = lstScope[i];
                EntityPropertyInfo info = null;
                if (!string.IsNullOrEmpty(objScope.PropertyName))
                {
                    info = curEntityInfo.PropertyInfo[objScope.PropertyName];
                }

                if (objScope.ScopeType == ScopeType.Scope)
                {
                    ScopeList lstInnerScope = objScope.Value1 as ScopeList;
                    if (lstInnerScope != null)
                    {
                        string strSql = FillCondition(curEntityInfo,lstParam, lstInnerScope, ref index);
                        string connectString = DataAccessCommon.GetConnectString(objScope);
                        ret.Append(" ");
                        ret.Append(connectString);
                        ret.Append(" (1=1 " + strSql + ")");
                    }
                }
                else
                {
                    string pName = (info != null ? info.ParamName : "");
                    DbType dbType = (info != null ? info.SqlType : DbType.Object);
                    ret.Append(FormatScorp(objScope, lstParam, pName, dbType, index, curEntityInfo.EntityType));
                }
                index++;
            }
            return ret.ToString();
        }

        /// <summary>
        /// ���ص�ǰ�������ַ���
        /// </summary>
        /// <param name="scope">������</param>
        /// <param name="list">�����б�</param>
        /// <param name="paramName">�������ֶ���</param>
        /// <param name="type">��ǰ�����ݿ�����</param>
        /// <param name="lstIndex">��ǰ�����ı�ʶδ���ͬ���ֶεĲ�����������Ϊ0</param>
        /// <param name="entityType">��ǰʵ�������</param>
        /// <returns></returns>
        public static string FormatScorp(Scope scope, ParamList list, string paramName, DbType type, int lstIndex,Type entityType)
        {
            string sql = null;
            ScopeExplainer handle=ExplainScope.GetExplainer(scope);
            if (handle != null) 
            {
                sql = handle(scope, list, entityType, paramName, type, lstIndex);
            }
            
            
            return sql;
        }

        /// <summary>
        /// �Ѳ�ѯ�����ӵ��������ַ�����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="type">���ݿ���ߵ�����</param>
        /// <returns></returns>
        public static string FormatValue(object value, DbType type,DBInfo db)
        {
            if (value == null)
            {
                return null;
            }

            switch (type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return "'" + value.ToString().Replace("'", "''") + "'";
                case DbType.Guid:
                    if (value is Guid)
                    {
                        return Buffalo.Kernel.CommonMethods.GuidToString((Guid)value);
                    }
                    return value.ToString();
                case DbType.DateTime:
                case DbType.Time:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return db.CurrentDbAdapter.GetDateTimeString(value);
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int32:
                case DbType.Int16:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Byte:
                case DbType.Currency:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Single:
                    return value.ToString().Replace(" ","");
                case DbType.Binary:
                    byte[] binaryValue = value as byte[];
                    if (binaryValue != null)
                    {
                        string hexVal = CommonMethods.BytesToHexString(binaryValue);
                        return "0x" + hexVal;
                    }
                    return "";
                case DbType.Boolean:
                    bool valBool = Convert.ToBoolean(value);
                    if (valBool == true)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                default:
                    return null;
            }
        }
    }
}
