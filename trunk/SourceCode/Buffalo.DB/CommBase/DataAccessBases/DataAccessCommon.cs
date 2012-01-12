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
        /// 获取条件连接字符串
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
        /// 格式化变量名称
        /// </summary>
        /// <param name="pam">变量</param>
        /// <param name="index">当前的标识</param>
        /// <returns></returns>
        public static string FormatParam(string pam, int index)
        {
            return pam + "_" + index.ToString();
        }

        /// <summary>
        /// 填充查询条件并返回条件的SQL语句( and 开头)
        /// </summary>
        /// <param name="lstParam">参数列表</param>
        /// <param name="lstScope">范围查询集合</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo,ParamList lstParam, ScopeList lstScope)
        {
            int index = 0;
            return FillCondition(curEntityInfo,lstParam, lstScope, ref index);
        }


        /// <summary>
        /// 填充查询条件并返回条件的SQL语句( and 开头)
        /// </summary>
        /// <param name="lstParam">参数列表</param>
        /// <param name="lstScope">范围查询集合</param>
        /// <param name="CurEntityInfo">当前实体信息</param>
        /// <param name="index">索引</param>
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
        /// 返回当前条件的字符串
        /// </summary>
        /// <param name="scope">条件类</param>
        /// <param name="list">参数列表</param>
        /// <param name="paramName">所属的字段名</param>
        /// <param name="type">当前的数据库类型</param>
        /// <param name="lstIndex">当前索引的标识未辨别同名字段的参数，可设置为0</param>
        /// <param name="entityType">当前实体的类型</param>
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
        /// 把查询条件加到条件的字符串里
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">数据库里边的类型</param>
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
