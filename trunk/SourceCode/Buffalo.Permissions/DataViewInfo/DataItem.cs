using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.Permissions.DataViewInfo
{
    /// <summary>
    /// ������ͼ��������
    /// </summary>
    public class DataItem
    {
        private string _propertyName;
        /// <summary>
        /// ������
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        private Type _propertyType;
        /// <summary>
        /// ��������
        /// </summary>
        public Type PropertyType
        {
            get { return _propertyType; }
        }
        private bool _canView;
        /// <summary>
        /// �Ƿ��ܲ鿴
        /// </summary>
        public bool CanView
        {
            get { return _canView; }
            set { _canView=value; }
        }
        private bool _canAdd;
        /// <summary>
        /// �Ƿ������
        /// </summary>
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; }
        }
        private bool _canEdit;
        /// <summary>
        /// �Ƿ��ܱ༭
        /// </summary>
        public bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }
        private SumType _sumType;
        /// <summary>
        /// ͳ������
        /// </summary>
        public SumType SumType
        {
            get { return _sumType; }
            set { _sumType = value; }
        }
        private BQLParamHandle _customSum;
        /// <summary>
        /// �Զ���ͳ������(SumType=SumType.Custom)ʱ���ͳ�Ʊ��ʽ
        /// </summary>
        public BQLParamHandle CustomSum
        {
            get { return _customSum; }
            set { _customSum = value; }
        }

        /// <summary>
        /// ������ͼ��������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="propertyType">��������</param>
        /// <param name="canView">�ܲ鿴</param>
        /// <param name="canAdd">�����</param>
        /// <param name="canEdit">�ܱ༭</param>
        /// <param name="sumType">ͳ������</param>
        /// <param name="customSum">�Զ���ͳ����</param>
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
        /// ������ͼ��������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="propertyType">��������</param>
        public DataItem(string propertyName,Type propertyType) 
        {
            _propertyName = propertyName;
            _propertyType = propertyType;
        }
    }

    /// <summary>
    /// ͳ������
    /// </summary>
    public enum SumType 
    {
        /// <summary>
        /// ��
        /// </summary>
        None=1,
        /// <summary>
        /// �ܺ�
        /// </summary>
        Sum=2,
        /// <summary>
        /// ������
        /// </summary>
        Count=3,
        /// <summary>
        /// ƽ��ֵ
        /// </summary>
        Avg=4,
        /// <summary>
        /// �Զ���ͳ��
        /// </summary>
        Custom=5
    }
}
