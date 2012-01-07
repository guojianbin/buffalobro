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
        /// 此排序的取值句柄
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
        /// 排序方式
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
