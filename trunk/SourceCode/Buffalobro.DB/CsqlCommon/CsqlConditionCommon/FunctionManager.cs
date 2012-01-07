using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.CommBase.DataAccessBases;
using System.Data;
using Buffalo.Kernel;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using System.Collections;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public delegate string DelFunctionHandle(CsqlComparItem handle, KeyWordInfomation info);
    public delegate string DelOperatorHandle(CsqlOperatorHandle handle, KeyWordInfomation info);
    public class FunctionManager
    {
        #region 函数解释
        
        
        /// <summary>
        /// 运算符函数
        /// </summary>
        /// <param name="fHandle">函数信息</param>
        /// <param name="connect">连接符号</param>
        /// <returns></returns>
        private static string OperatorFunction(CsqlOperatorHandle fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Double;
            return CustomerConnectFunction(fHandle.GetParameters(), connect,info);
        }
        /// <summary>
        /// 运算符函数
        /// </summary>
        /// <param name="fHandle">函数信息</param>
        /// <param name="connect">连接符号</param>
        /// <returns></returns>
        private static string ConditionsFunction(CsqlComparItem fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Boolean;
            return CustomerConnectFunction(fHandle.GetParameters(), connect, info);
        }
        /// <summary>
        /// 进行加法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAdd(CsqlOperatorHandle fHandle, KeyWordInfomation info)
        {
            CsqlValueItem[] parameters=fHandle.GetParameters();
            CsqlValueItem item1 = parameters[0];
            CsqlValueItem item2 = parameters[1];

            if (IsStringDBType(item1.ValueDbType) || IsStringDBType(item2.ValueDbType)) //字符串拼合
            {
                string value1 = item1.DisplayValue(info);
                string value2 = item2.DisplayValue(info);

                fHandle.ValueDbType = DbType.String;
                return info.DBInfo.CurrentDbAdapter.ConcatString(value1, value2);
            }
            else 
            {
                fHandle.ValueDbType = DbType.Double;
            }
            return CustomerConnectFunction(fHandle.GetParameters(), "+",info);
        }

        /// <summary>
        /// 判断是否字符串类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStringDBType(DbType type) 
        {
            if (type == DbType.AnsiString || type == DbType.AnsiStringFixedLength
                || type == DbType.String || type == DbType.StringFixedLength )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Not运算
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        internal static string DoNot(CsqlComparItem handle, KeyWordInfomation info) 
        {
            CsqlValueItem[] items = handle.GetParameters();
            if (items != null && items.Length > 0) 
            {
                return " not " + items[0].DisplayValue(info);
            }
            return "";
        }

        /// <summary>
        /// 进行加法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoSub(CsqlOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "-", info);
        }
        /// <summary>
        /// 进行乘法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMul(CsqlOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "*", info);
        }
        /// <summary>
        /// 进行除法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoDiv(CsqlOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "/", info);
        }
        /// <summary>
        /// 进行等于运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoEqual(CsqlComparItem handle, KeyWordInfomation info)
        {
            CsqlValueItem[] parameters = handle.GetParameters();
            if (parameters[1].IsNullValue())
            {
                CsqlValueItem item = parameters[0];
                return item.DisplayValue(info) + " is null";
            }
            return ConditionsFunction(handle, "=",info);
        }
        /// <summary>
        /// 不等于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoNotequal(CsqlComparItem handle, KeyWordInfomation info)
        {
            CsqlValueItem[] parameters = handle.GetParameters();
            if (parameters[1].IsNullValue())
            {
                CsqlValueItem item = parameters[0];
                return item.DisplayValue(info) + " is not null";
            }
            return ConditionsFunction(handle, "<>",info);
        }
        /// <summary>
        /// and 连接
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAnd(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " and ",info);
        }
        /// <summary>
        /// or 连接
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoOr(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " or ",info);
        }
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMore(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">",info);
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMorethen(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">=",info);
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLess(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<",info);
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLessThen(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<=",info);
        }
        /// <summary>
        /// 普通连接函数
        /// </summary>
        /// <param name="handle">函数</param>
        /// <param name="connect">连接符</param>
        /// <returns></returns>
        private static string CustomerConnectFunction(CsqlValueItem[] parameters, string connect, KeyWordInfomation info)
        {
            //CsqlValueItem[] parameters = fHandle.GetParameters();
            string values = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                CsqlValueItem item = parameters[i];

                values += item.DisplayValue(info);
                if (i < parameters.Length - 1)
                {
                    values += connect;
                }
            }

            return values;
        }
        
        #endregion
        

        
    
    }
}
