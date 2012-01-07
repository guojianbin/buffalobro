using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.FastReflection;
using Buffalobro.DB.QueryConditions;
namespace Buffalobro.DB.ListExtends
{
    class SortCompartItem
    {
        private PropertyInfoHandle getValueHandler;
        private SortType curSortType;

        /// <summary>
        /// �������ȡֵ���
        /// </summary>
        public PropertyInfoHandle GetValueHandler 
        {
            get 
            {
                return getValueHandler;
            }
            set 
            {
                getValueHandler = value;
            }
        }

        /// <summary>
        /// ����ʽ
        /// </summary>
        public SortType CurSortType 
        {
            get
            {
                return curSortType;
            }
            set 
            {
                curSortType = value;
            }
        }
    }
}
