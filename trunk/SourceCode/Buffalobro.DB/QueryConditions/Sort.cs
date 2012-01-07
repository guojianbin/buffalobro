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
        /// ��ǰ�������������
        /// </summary>
        [Description("��ǰ�������������")]
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
        /// ��ǰ������ʽ
        /// </summary>
        [Description("��ǰ������ʽ")]
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
        /// CSQL��������
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
