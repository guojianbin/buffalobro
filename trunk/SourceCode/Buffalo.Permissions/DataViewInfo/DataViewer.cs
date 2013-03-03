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
        /// <summary>
        /// ����������
        /// </summary>
        public string GetClassName()
        {
             return GetClassType().FullName; 
        }
        /// <summary>
        /// ������������
        /// </summary>
        public Type GetClassType()
        {
             return _entityHandle.GetEntityInfo().EntityType;
        }
        private bool _canView;
        /// <summary>
        /// �Ƿ��ܲ鿴
        /// </summary>
        public bool IsCanView()
        {
            return _canView; 
        }
        private bool _canAdd;
        /// <summary>
        /// �Ƿ������
        /// </summary>
        public bool IsCanAdd()
        {
            return _canAdd; 
        }
        private bool _canEdit;
        /// <summary>
        /// �Ƿ��ܱ༭
        /// </summary>
        public bool IsCanEdit()
        {
            return _canEdit; 
            
        }



        private List<DataItem> _lstDataItem = new List<DataItem>();
        private BQLEntityTableHandle _entityHandle;
        /// <summary>
        /// ʵ����Ϣ
        /// </summary>
        public BQLEntityTableHandle GetEntityHandle()
        {
            return _entityHandle; 
        }

        /// <summary>
        /// ������ͼ
        /// </summary>
        /// <param name="entityHandle">������</param>
        /// <param name="canView">�Ƿ��ܲ鿴</param>
        /// <param name="canAdd">�Ƿ������</param>
        /// <param name="canEdit">�Ƿ��ܱ༭</param>
        /// <param name="summary">��ע��</param>
        /// <param name="entityHandle">������ʵ����Ϣ</param>
        public DataViewer(BQLEntityTableHandle entityHandle, bool canView, bool canAdd,
            bool canEdit) 
        {
            _entityHandle = entityHandle;

            _canAdd = canAdd;
            _canEdit = canEdit;
            _canView = canView;

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

        public int GetDataItemCount()
        {

            return _lstDataItem.Count;

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
