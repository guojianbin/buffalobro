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
            if (type == DbType.AnsiString || type == DbType.AnsiStringFixedLength || type == DbType.String ||
                type == DbType.StringFixedLength || type == DbType.Guid)
            {
                return "'" + value.ToString().Replace("'", "''") + "'";
            }
            else if(type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return db.CurrentDbAdapter.GetDateTimeString(value);
            }
            else if (type == DbType.Int64||
                type == DbType.Decimal || type == DbType.Double || type == DbType.Int32 ||
            type == DbType.Int16 || type == DbType.Double ||
            type == DbType.SByte || type == DbType.Byte || type == DbType.Currency || type == DbType.UInt16
            || type == DbType.UInt32 || type == DbType.UInt64 || type == DbType.VarNumeric
                )
            {
                return value.ToString().Replace(" ","");
            }
            else if (type == DbType.Binary) 
            {
                byte[] binaryValue = value as byte[];
                if (binaryValue != null)
                {
                    string val = CommonMethods.BytesToHexString(binaryValue);
                    return "0x" + val;
                }
                
            }
            else if (type == DbType.Boolean)
            {
                bool val = Convert.ToBoolean(value);
                if (val == true)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            return null;
        }
    }
}
