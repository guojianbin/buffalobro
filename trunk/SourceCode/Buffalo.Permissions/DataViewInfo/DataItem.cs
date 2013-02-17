using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.Permissions.DataViewInfo
{
    /// <summary>
    /// 数据视图的数据项
    /// </summary>
    public class DataItem
    {
        private string _propertyName;
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        private Type _propertyType;
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type PropertyType
        {
            get { return _propertyType; }
        }
        private bool _canView;
        /// <summary>
        /// 是否能查看
        /// </summary>
        public bool CanView
        {
            get { return _canView; }
            set { _canView=value; }
        }
        private bool _canAdd;
        /// <summary>
        /// 是否能添加
        /// </summary>
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; }
        }
        private bool _canEdit;
        /// <summary>
        /// 是否能编辑
        /// </summary>
        public bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }
        private SumType _sumType;
        /// <summary>
        /// 统计类型
        /// </summary>
        public SumType SumType
        {
            get { return _sumType; }
            set { _sumType = value; }
        }
        private BQLParamHandle _customSum;
        /// <summary>
        /// 自定义统计类型(SumType=SumType.Custom)时候的统计表达式
        /// </summary>
        public BQLParamHandle CustomSum
        {
            get { return _customSum; }
            set { _customSum = value; }
        }

        /// <summary>
        /// 数据视图的数据项
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyType">属性类型</param>
        /// <param name="canView">能查看</param>
        /// <param name="canAdd">能添加</param>
        /// <param name="canEdit">能编辑</param>
        /// <param name="sumType">统计类型</param>
        /// <param name="customSum">自定义统计项</param>
        public DataItem(string propertyName, Type propertyType, bool canView, bool canAdd, bool canEdit, SumType sumType, BQLParamHandle customSum) 
            :this(propertyName,propertyType)
        {
            _canAdd = canAdd;
            _canEdit = canEdit;
            _canView = canView;
            _customSum = customSum;
            _sumType = sumType;
        }

        /// <summary>
        /// 数据视图的数据项
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyType">属性类型</param>
        public DataItem(string propertyName,Type propertyType) 
        {
            _propertyName = propertyName;
            _propertyType = propertyType;
        }
    }

    /// <summary>
    /// 统计类型
    /// </summary>
    public enum SumType 
    {
        /// <summary>
        /// 无
        /// </summary>
        None=1,
        /// <summary>
        /// 总和
        /// </summary>
        Sum=2,
        /// <summary>
        /// 总条数
        /// </summary>
        Count=3,
        /// <summary>
        /// 平均值
        /// </summary>
        Avg=4,
        /// <summary>
        /// 自定义统计
        /// </summary>
        Custom=5
    }
}
