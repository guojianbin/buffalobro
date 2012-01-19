using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLAliasParamHandle : BQLParamHandle
    {
        private BQLValueItem _prmHandle;
        private string _aliasName;

        /// <summary>
        /// ±ðÃû
        /// </summary>
        internal string AliasName 
        {
            get 
            {
                return _aliasName;
            }
        }

        public BQLAliasParamHandle(BQLValueItem prmHandle, string aliasName)
        {
            this._prmHandle = prmHandle;
            this._aliasName = aliasName;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            //info.QueryParams[aliasName] = new ParamInfo(aliasName, aliasName, prmHandle.ValueDataType);
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            bool isWhere = info.IsWhere;

            info.IsWhere = true;
            string ret= _prmHandle.DisplayValue(info) + " as " + idba.FormatParam(_aliasName);
            info.IsWhere = isWhere;
            return ret;
        }
    }
}
