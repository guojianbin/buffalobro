using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.PropertyAttributes;
using Buffalobro.Kernel.FastReflection;

namespace Buffalobro.DB.EntityInfos
{
    /// <summary>
    /// ʵ��ӳ���������Ϣ
    /// </summary>
    public class EntityMappingInfo : FieldInfoHandle
    {
        private TableMappingAttribute tableMappingAtt;
        /// <summary>
        /// �������Ե���Ϣ��
        /// </summary>
        /// <param name="belong">������ʵ��</param>
        /// <param name="getHandle">getί��</param>
        /// <param name="setHandle">setί��</param>
        /// <param name="tableMappingAtt">ӳ���ʶ��</param>
        /// <param name="fieldName">������</param>
        /// <param name="fieldType">��������</param>
        public EntityMappingInfo(Type belong,GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, TableMappingAttribute tableMappingAtt, string fieldName, Type fieldType)
            : base(belong, getHandle, setHandle, fieldType, fieldName)
        {
            this.tableMappingAtt = tableMappingAtt;
        }
        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName
        {
            get
            {
                return tableMappingAtt.PropertyName;
            }
        }
        /// <summary>
        /// �����Ƿ�������
        /// </summary>
        public bool IsPrimary
        {
            get
            {
                return tableMappingAtt.IsPrimary;
            }
        }

        /// <summary>
        /// Դ����
        /// </summary>
        public EntityPropertyInfo SourceProperty
        {
            get
            {

                return tableMappingAtt.SourceProperty;
            }
        }
        /// <summary>
        /// Ŀ������
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
