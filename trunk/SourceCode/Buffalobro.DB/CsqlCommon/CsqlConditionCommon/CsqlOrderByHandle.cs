using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.QueryConditions;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class CsqlOrderByHandle : CsqlParamHandle
    {
        private SortType _sortType;
        private CsqlParamHandle _arg;
        internal CsqlOrderByHandle(CsqlParamHandle arg, SortType sortType)
        {
            
            this._sortType = sortType;
            this._arg = arg;
            //this.valueType = CsqlValueType.OrderBy;
        }

        /// <summary>
        /// 项的排序类型
        /// </summary>
        internal SortType SortType
        {
            get
            {
                return _sortType;
            }
        }

        /// <summary>
        /// 项的属性
        /// </summary>
        internal CsqlParamHandle Param
        {
            get
            {
                return _arg;
            }
        }
       
        internal override string DisplayValue(KeyWordInfomation info)
        {
            info.IsWhere = true;
            string orderby = _arg.DisplayValue(info);
            if (_sortType == SortType.DESC)
            {
                orderby += " desc";
            }
            info.IsWhere = false;
            return orderby;
        }


        internal override void FillInfo(KeyWordInfomation info)
        {
            _arg.FillInfo(info);
        }
    }
}