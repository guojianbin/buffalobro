using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    public class Sort
    {
        private string _propertyName;
        private SortType _sortType;
        private BQLOrderByHandle _orderHandle;
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
        /// BQL��������
        /// </summary>
        public BQLOrderByHandle OrderHandle
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
