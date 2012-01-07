using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.QueryConditions
{
    public class Sort
    {
        private string _propertyName;
        private SortType _sortType;
        private CsqlOrderByHandle _orderHandle;
        /// <summary>
        /// 当前的排序的属性名
        /// </summary>
        [Description("当前的排序的属性名")]
        [NotifyParentProperty(true)]
        public string PropertyName 
        {
            get 
            {
                return _propertyName;
            }
            set 
            {
                _propertyName = value;
            }
        }
        /// <summary>
        /// 当前的排序方式
        /// </summary>
        [Description("当前的排序方式")]
        [NotifyParentProperty(true)]
        public SortType SortType
        {
            get
            {
                return _sortType;
            }
            set
            {
                _sortType = value;
            }
        }
        /// <summary>
        /// CSQL排序条件
        /// </summary>
        public CsqlOrderByHandle OrderHandle
        {
            get
            {
                return _orderHandle;
            }
            set
            {
                _orderHandle = value;
            }
        }
    }
}
