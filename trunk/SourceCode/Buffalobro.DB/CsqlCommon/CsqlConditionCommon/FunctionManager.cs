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
        #region ��������
        
        
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="fHandle">������Ϣ</param>
        /// <param name="connect">���ӷ���</param>
        /// <returns></returns>
        private static string OperatorFunction(CsqlOperatorHandle fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Double;
            return CustomerConnectFunction(fHandle.GetParameters(), connect,info);
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="fHandle">������Ϣ</param>
        /// <param name="connect">���ӷ���</param>
        /// <returns></returns>
        private static string ConditionsFunction(CsqlComparItem fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Boolean;
            return CustomerConnectFunction(fHandle.GetParameters(), connect, info);
        }
        /// <summary>
        /// ���мӷ�����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAdd(CsqlOperatorHandle fHandle, KeyWordInfomation info)
        {
            CsqlValueItem[] parameters=fHandle.GetParameters();
            CsqlValueItem item1 = parameters[0];
            CsqlValueItem item2 = parameters[1];

            if (IsStringDBType(item1.ValueDbType) || IsStringDBType(item2.ValueDbType)) //�ַ���ƴ��
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
        /// �ж��Ƿ��ַ�������
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
        /// Not����
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
        /// ���мӷ�����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoSub(CsqlOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "-", info);
        }
        /// <summary>
        /// ���г˷�����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMul(CsqlOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "*", info);
        }
        /// <summary>
        /// ���г�������
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoDiv(CsqlOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "/", info);
        }
        /// <summary>
        /// ���е�������
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
        /// ������
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
        /// and ����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAnd(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " and ",info);
        }
        /// <summary>
        /// or ����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoOr(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " or ",info);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMore(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">",info);
        }
        /// <summary>
        /// ���ڵ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMorethen(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">=",info);
        }
        /// <summary>
        /// С��
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLess(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<",info);
        }
        /// <summary>
        /// С�ڵ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLessThen(CsqlComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<=",info);
        }
        /// <summary>
        /// ��ͨ���Ӻ���
        /// </summary>
        /// <param name="handle">����</param>
        /// <param name="connect">���ӷ�</param>
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
