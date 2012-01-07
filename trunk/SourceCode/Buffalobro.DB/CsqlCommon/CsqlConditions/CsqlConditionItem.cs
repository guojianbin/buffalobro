using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.Kernel.Defaults;
using System.Collections;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon
{
    public delegate string DelConditionHandle(string sourceTable, string[] paramList, DBInfo db);
    public class CsqlConditionItem : CsqlCondition
    {
        private DelConditionHandle _handle;
        private IEnumerable _paramList;
        private CsqlQuery _query;
        private CsqlValueItem _sourceHandle;
        /// <summary>
        /// 条件函数
        /// </summary>
        /// <param name="sourceHandle">发送源(字段)</param>
        /// <param name="paramList">参数列表</param>
        /// <param name="handle">关联处理函数</param>
        public CsqlConditionItem(CsqlValueItem sourceHandle, IEnumerable paramList, DelConditionHandle handle) 
        {
            this._sourceHandle = sourceHandle;
            this._handle = handle;
            this._paramList = paramList;
            this._valueDbType = DbType.Boolean;
        }
        /// <summary>
        /// 条件函数
        /// </summary>
        /// <param name="sourceHandle">发送源(字段)</param>
        /// <param name="query">查询</param>
        /// <param name="handle">关联处理函数</param>
        public CsqlConditionItem(CsqlParamHandle sourceHandle, CsqlQuery query, DelConditionHandle handle)
        {
            this._sourceHandle = sourceHandle;
            this._handle = handle;
            this._query = query;
            this._valueDbType = DbType.Boolean;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            _sourceHandle.FillInfo(info);

            if (_paramList != null)
            {
                List<CsqlValueItem> lst = new List<CsqlValueItem>();
                foreach (object item in _paramList)
                {
                    CsqlValueItem value = CsqlValueItem.ToValueItem(item);
                    lst.Add(value);
                    value.FillInfo(info);
                }
                _paramList = lst;
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            if (_handle != null)
            {
                if (_query != null) 
                {
                    KeyWordConver conver = new KeyWordConver();
                    KeyWordInfomation qInfo = info.Clone() as KeyWordInfomation;
                    return "(" + _handle(_sourceHandle.DisplayValue(info), new string[] { conver.ToConver(_query, qInfo).GetSql() },info.DBInfo) + ")";
                }
                else if (_paramList != null)
                {
                    List<string> lstPrm = new List<string>();
                    foreach (object item in _paramList)
                    {
                        lstPrm.Add(CsqlValueItem.ToValueItem(item).DisplayValue(info));
                    }
                    return "(" + _handle(_sourceHandle.DisplayValue(info), lstPrm.ToArray(),info.DBInfo) + ")";
                }
            }
            return null;
        }
    }
}
