using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// �ӱ��ʶ
    /// </summary>
    public class TableMappingAttribute : System.Attribute
    {
        private string _propertyName;
        private EntityPropertyInfo _sourceProperty;
        private EntityPropertyInfo _targetProperty;
        private bool _isPrimary;

        private string _sourceName;
        private string _targetName;
        private Type _sourceType;
        private Type _targetType;

        /// <summary>
        /// ����ӳ����Ϣ
        /// </summary>
        /// <param name="propertyName">����</param>
        /// <param name="sourceProperty">Դ��������</param>
        /// <param name="targetProperty">Ŀ���������</param>
        /// <param name="sourceTableType">ԭ��������</param>
        /// <param name="targetTableType">Ŀ���������</param>
        /// <param name="isPrimary">Դ�����Ƿ�����</param>
        public TableMappingAttribute(string propertyName, string sourceProperty, string targetProperty,
             bool isPrimary) 
        {

            _propertyName = propertyName;
            _sourceName = sourceProperty;
            _targetName = targetProperty;
            _isPrimary = isPrimary;
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
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return _propertyName;
            }
        }
        /// <summary>
        /// �����Ƿ�������
        /// </summary>
        public bool IsPrimary
        {
            get 
            {
                return _isPrimary;
            }
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
