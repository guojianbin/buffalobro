using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.Permissions.DataViewInfo
{
    /// <summary>
    /// 数据视图
    /// </summary>
    public class DataViewer:IEnumerable<DataItem>
    {
        /// <summary>
        /// 所属的类名
        /// </summary>
        public string ClassName
        {
            get { return ClassType.FullName; }
        }
        /// <summary>
        /// 所属的类类型
        /// </summary>
        public Type ClassType
        {
            get { return _entityHandle.GetEntityInfo().EntityType; }
        }
        private bool _canView;
        /// <summary>
        /// 是否能查看
        /// </summary>
        public bool CanView
        {
            get { return _canView; }
            set { _canView = value; }
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
        private string _summary;
        /// <summary>
        /// 备注
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }


        private List<DataItem> _lstDataItem = new List<DataItem>();
        private BQLEntityTableHandle _entityHandle;
        /// <summary>
        /// 实体信息
        /// </summary>
        public BQLEntityTableHandle EntityHandle
        {
            get { return _entityHandle; }
        }

        /// <summary>
        /// 数据视图
        /// </summary>
        /// <param name="entityHandle">类类型</param>
        /// <param name="canView">是否能查看</param>
        /// <param name="canAdd">是否能添加</param>
        /// <param name="canEdit">是否能编辑</param>
        /// <param name="summary">类注释</param>
        /// <param name="entityHandle">所属的实体信息</param>
        public DataViewer(BQLEntityTableHandle entityHandle, bool canView, bool canAdd,
            bool canEdit,string summary) 
        {
            _entityHandle = entityHandle;
            _className = className;

            _canAdd = canAdd;
            _canEdit = canEdit;
            _canView = canView;
            _summary = summary;
        }
        /// <summary>
        /// 创建视图数据项
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="canView">能查看</param>
        /// <param name="canAdd">能添加</param>
        /// <param name="canEdit">能编辑</param>
        /// <param name="sumType">统计类型</param>
        /// <param name="customSum">自定义统计项</param>
        /// <returns>数据项</returns>
        protected DataItem CreateDataItem(string propertyName, Type propertyType,
            bool canView, bool canAdd, bool canEdit, string summary,int columnWidth,
            SumType sumType, BQLParamHandle customSum) 
        {
            DataItem item = new DataItem(propertyName,propertyType, canView, canAdd,
                canEdit,summary,columnWidth, sumType, customSum,this);
            _lstDataItem.Add(item);
            return item;
        }

        /// <summary>
        /// 返回视图数据项
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public DataItem this[int index] 
        {
            get 
            {
                return _lstDataItem[index];
            }
        }

        /// <summary>
        /// 返回视图数据项
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public DataItem this[string propertyName]
        {
            get
            {
                int index = FindDataItemIndex(propertyName);
                if (index > 0) 
                {
                    return _lstDataItem[index];
                }
                return null;
            }
        }

        public int Count 
        {
            get 
            {
                return _lstDataItem.Count;
            }
        }

        /// <summary>
        /// 根据索引删除项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Remove(int index) 
        {
            if (index > _lstDataItem.Count - 1) 
            {
                return false;
            }
            
            _lstDataItem.RemoveAt(index);
            return true;
        }
        /// <summary>
        /// 根据属性名删除项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool Remove(string propertyName)
        {

            int index = FindDataItemIndex(propertyName);
            if (index > 0)
            {
                _lstDataItem.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查找视图项的索引
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private int FindDataItemIndex(string propertyName) 
        {
            for (int i = 0; i < _lstDataItem.Count; i++) 
            {
                if (_lstDataItem[i].PropertyName == propertyName) 
                {
                    return i;
                }
            }
            return -1;
        }

        #region IEnumerable<DataItem> 成员

        public IEnumerator<DataItem> GetEnumerator()
        {
            return _lstDataItem.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _lstDataItem.GetEnumerator();
        }

        #endregion
    }
}
