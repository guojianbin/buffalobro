using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 实体映射的属性信息
    /// </summary>
    public class EntityMappingInfo : FieldInfoHandle
    {
        private TableRelationAttribute _mappingInfo;

        
        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">所属的实体</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="tableMappingAtt">映射标识类</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldType">属性类型</param>
        public EntityMappingInfo(Type belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle,
            TableRelationAttribute mappingInfo,string fieldName, Type fieldType)
            : base(belong, getHandle, setHandle, fieldType, fieldName)
        {
            this._mappingInfo = mappingInfo;
        }
        /// <summary>
        /// 映射信息
        /// </summary>
        public TableRelationAttribute MappingInfo
        {
            get { return _mappingInfo; }
            internal set { _mappingInfo = value; }
        }
        /// <summary>
        /// 拷贝副本
        /// </summary>
        /// <param name="belong">所属实体</param>
        /// <returns></returns>
        public EntityMappingInfo Copy(Type belong) 
        {
            EntityMappingInfo info = new EntityMappingInfo(belong, GetHandle, SetHandle, _mappingInfo, _fieldName, _fieldType);
            return info;
        }

        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _mappingInfo.PropertyName;
            }
        }
        /// <summary>
        /// 是否主表属性
        /// </summary>
        public bool IsParent
        {
            get
            {
                return _mappingInfo.IsParent;
            }
        }

        /// <summary>
        /// 源属性
        /// </summary>
        public EntityPropertyInfo SourceProperty
        {
            get
            {

                return _mappingInfo.SourceProperty;
            }
        }
        /// <summary>
        /// 目标属性
        /// </summary>
        public EntityPropertyInfo TargetProperty
        {
            get
            {

                return _mappingInfo.TargetProperty;
            }
        }
    }
}
