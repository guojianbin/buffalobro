using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// �ӱ��ʶ
    /// </summary>
    public class TableRelationAttribute : System.Attribute
    {
        private string _name;
        private string _propertyName;
        private EntityPropertyInfo _sourceProperty;
        private EntityPropertyInfo _targetProperty;
        private bool _isParent;

        private string _sourceName;
        private string _targetName;
        private Type _sourceType;
        private Type _targetType;
        private string _sourceTable;
        private string _targetTable;

         /// <summary>
        /// ����ӳ����Ϣ
        /// </summary>
        public TableRelationAttribute() { }
        /// <summary>
        /// ����ӳ����Ϣ
        /// </summary>
        /// <param name="propertyName">����</param>
        /// <param name="sourceProperty">Դ��������</param>
        /// <param name="targetProperty">Ŀ���������</param>
        /// <param name="sourceTableType">ԭ��������</param>
        /// <param name="targetTableType">Ŀ���������</param>
        /// <param name="isParent">�Ƿ���������</param>
        public TableRelationAttribute(string name, string sourceTable, string targetTable,
            string sourceParam, string targetParam, string propertyName, bool isParent) 
        {

            _propertyName = propertyName;
            _sourceTable = sourceTable;
            _sourceName = sourceParam;
            _targetTable = targetTable;
            _targetName = targetParam;

            _isParent = isParent;
        }
        /// <summary>
        /// �����������
        /// </summary>
        public void CreateName()
        {
            Name = "FK_" + SourceTable + "_" + TargetTable + "_" + SourceName;
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="sourceEntity"></param>
        /// <param name="targetEniity"></param>
        internal void SetEntity(Type sourceType,Type targetType) 
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }
        /// <summary>
        /// Լ����
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return _propertyName;
            }
            set 
            {
                _propertyName=value;
            }
        }
        /// <summary>
        /// Դ����
        /// </summary>
        public string SourceTable
        {
            get { return _sourceTable; }
            set { _sourceTable = value; }
        }
        /// <summary>
        /// Ŀ�����
        /// </summary>
        public string TargetTable
        {
            get { return _targetTable; }
            set { _targetTable = value; }
        }
        /// <summary>
        /// Ŀ��������
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
            set { _targetName = value; }
        }

        /// <summary>
        /// Դ������
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
            set { _sourceName = value; }
        }
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
            set { _isParent = value; }
        }

        /// <summary>
        /// Դ����
        /// </summary>
        public EntityPropertyInfo SourceProperty 
        {
            get 
            {
                if (_sourceProperty == null)
                {
                    _sourceProperty = EntityInfoManager.GetEntityHandle(_sourceType).PropertyInfo[_sourceName];
                }
                return _sourceProperty;
            }
        }
        /// <summary>
        /// Ŀ������
        /// </summary>
        public EntityPropertyInfo TargetProperty 
        {
            get
            {
                if (_targetProperty == null)
                {
                    _targetProperty = EntityInfoManager.GetEntityHandle(_targetType).PropertyInfo[_targetName];
                }
                return _targetProperty;
            }
        }

    }
}
