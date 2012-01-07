using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.PropertyAttributes;
using Buffalobro.Kernel.FastReflection;

namespace Buffalobro.DB.EntityInfos
{
    /// <summary>
    /// 实体映射的属性信息
    /// </summary>
    public class EntityMappingInfo : FieldInfoHandle
    {
        private TableMappingAttribute tableMappingAtt;
        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">所属的实体</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="tableMappingAtt">映射标识类</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldType">属性类型</param>
        public EntityMappingInfo(Type belong,GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, TableMappingAttribute tableMappingAtt, string fieldName, Type fieldType)
            : base(belong, getHandle, setHandle, fieldType, fieldName)
        {
            this.tableMappingAtt = tableMappingAtt;
        }
        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName
        {
            get
            {
                return tableMappingAtt.PropertyName;
            }
        }
        /// <summary>
        /// 本表是否主键表
        /// </summary>
        public bool IsPrimary
        {
            get
            {
                return tableMappingAtt.IsPrimary;
            }
        }

        /// <summary>
        /// 源属性
        /// </summary>
        public EntityPropertyInfo SourceProperty
        {
            get
            {

                return tableMappingAtt.SourceProperty;
            }
        }
        /// <summary>
        /// 目标属性
        /// </summary>
        public EntityPropertyInfo TargetProperty
        {
            get
            {

                return tableMappingAtt.TargetProperty;
            }
        }
    }
}
