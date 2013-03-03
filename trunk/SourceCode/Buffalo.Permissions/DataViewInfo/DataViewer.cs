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
        public string GetClassName()
        {
             return GetClassType().FullName; 
        }
        /// <summary>
        /// 所属的类类型
        /// </summary>
        public Type GetClassType()
        {
             return _entityHandle.GetEntityInfo().EntityType;
        }
        private bool _canView;
        /// <summary>
        /// 是否能查看
        /// </summary>
        public bool IsCanView()
        {
            return _canView; 
        }
        private bool _canAdd;
        /// <summary>
        /// 是否能添加
        /// </summary>
        public bool IsCanAdd()
        {
            return _canAdd; 
        }
        private bool _canEdit;
        /// <summary>
        /// 是否能编辑
        /// </summary>
        public bool IsCanEdit()
        {
            return _canEdit; 
            
        }



        private List<DataItem> _lstDataItem = new List<DataItem>();
        private BQLEntityTableHandle _entityHandle;
        /// <summary>
        /// 实体信息
        /// </summary>
        public BQLEntityTableHandle GetEntityHandle()
        {
            return _entityHandle; 
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
            bool canEdit) 
        {
            _entityHandle = entityHandle;

            _canAdd = canAdd;
            _canEdit = canEdit;
            _canView = canView;

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
            bool canView, bool canAdd, bool canEdit,
            SumType sumType, BQLParamHandle customSum) 
        {
            DataItem item = new DataItem(propertyName,propertyType, canView, canAdd,
                canEdit,sumType, customSum,this);
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

        public int GetDataItemCount()
        {

            return _lstDataItem.Count;

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
