using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// 子表标识
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
        /// 关联映射信息
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="sourceProperty">源对象属性</param>
        /// <param name="targetProperty">目标对象属性</param>
        /// <param name="sourceTableType">原对象类型</param>
        /// <param name="targetTableType">目标对象类型</param>
        /// <param name="isPrimary">源对象是否主表</param>
        public TableMappingAttribute(string propertyName, string sourceProperty, string targetProperty,
             bool isPrimary) 
        {

            _propertyName = propertyName;
            _sourceName = sourceProperty;
            _targetName = targetProperty;
            _isPrimary = isPrimary;
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="sourceEntity"></param>
        /// <param name="targetEniity"></param>
        internal void SetEntity(Type sourceType,Type targetType) 
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }

        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return _propertyName;
            }
        }
        /// <summary>
        /// 本表是否主键表
        /// </summary>
        public bool IsPrimary
        {
            get 
            {
                return _isPrimary;
            }
        }

        /// <summary>
        /// 源属性
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
        /// 目标属性
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
