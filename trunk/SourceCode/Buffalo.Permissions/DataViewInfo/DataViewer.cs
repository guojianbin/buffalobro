using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.Permissions.DataViewInfo
{
    /// <summary>
    /// ������ͼ
    /// </summary>
    public class DataViewer:IEnumerable<DataItem>
    {
        private List<DataItem> _lstDataItem = new List<DataItem>();
        private BQLEntityTableHandle _entityHandle;
        /// <summary>
        /// ʵ����Ϣ
        /// </summary>
        public BQLEntityTableHandle EntityHandle
        {
            get { return _entityHandle; }
        }

        /// <summary>
        /// ������ͼ
        /// </summary>
        /// <param name="entityHandle">������ʵ����Ϣ</param>
        public DataViewer(BQLEntityTableHandle entityHandle) 
        {
            _entityHandle = entityHandle;
        }
        /// <summary>
        /// ������ͼ������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="canView">�ܲ鿴</param>
        /// <param name="canAdd">�����</param>
        /// <param name="canEdit">�ܱ༭</param>
        /// <param name="sumType">ͳ������</param>
        /// <param name="customSum">�Զ���ͳ����</param>
        /// <returns>������</returns>
        protected DataItem CreateDataItem(string propertyName,string propertyType,
            bool canView, bool canAdd, bool canEdit, 
            SumType sumType, BQLParamHandle customSum) 
        {
            DataItem item = new DataItem(propertyName,propertyType, 
                canView, canAdd, canEdit, sumType, customSum);
            _lstDataItem.Add(item);
            return item;
        }

        /// <summary>
        /// ������ͼ������
        /// </summary>
        /// <param name="index">����</param>
        /// <returns></returns>
        public DataItem this[int index] 
        {
            get 
            {
                return _lstDataItem[index];
            }
        }

        /// <summary>
        /// ������ͼ������
        /// </summary>
        /// <param name="propertyName">������</param>
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
        /// ��������ɾ����
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
        /// ����������ɾ����
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
        /// ������ͼ�������
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

        #region IEnumerable<DataItem> ��Ա

        public IEnumerator<DataItem> GetEnumerator()
        {
            return _lstDataItem.GetEnumerator();
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _lstDataItem.GetEnumerator();
        }

        #endregion
    }
}
