using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.PropertyAttributes;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.Kernel.FastReflection;
using Buffalobro.Kernel.Commons;

namespace Buffalobro.DB.EntityInfos
{
    /// <summary>
    /// ʵ���������Ϣ
    /// </summary>
    public class EntityPropertyInfo:FieldInfoHandle
    {
        private EntityParam ep;
        private EntityInfoHandle _belong;

        /// <summary>
        /// �������Ե���Ϣ��
        /// </summary>
        /// <param name="belong">������ʵ����Ϣ</param>
        /// <param name="getHandle">getί��</param>
        /// <param name="setHandle">setί��</param>
        /// <param name="ep">�ֶα�ʶ��</param>
        /// <param name="fieldType">�ֶ�����</param>
        /// <param name="fieldName">�ֶ���</param>
        public EntityPropertyInfo(EntityInfoHandle belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, EntityParam ep, Type fieldType, string fieldName,DBInfo info)
            : base(belong.EntityType, getHandle, setHandle,fieldType,fieldName)
        {
            ep.SqlType = info.CurrentDbAdapter.ToCurrentDbType(ep.SqlType);//ת���ɱ����ݿ�֧�ֵ���������
            this.ep = ep;
            _belong = belong;
        }

        /// <summary>
        /// ����ʵ������
        /// </summary>
        public EntityInfoHandle BelongInfo
        {
            get 
            {
                return _belong;
            }
        }

        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return ep.PropertyName;
            }
        }
        

        /// <summary>
        /// ��ȡ��Ӧ���ֶε�SQL����
        /// </summary>
        public DbType SqlType
        {
            get
            {
                return ep.SqlType;
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ���ֶε�����
        /// </summary>
        public string ParamName
        {
            get
            {
                return ep.ParamName;
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ���ֶ��Ƿ�����
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return ep.IsPrimaryKey;
            }
        }

        /// <summary>
        /// �Ƿ��Զ������ֶ�
        /// </summary>
        public bool Identity
        {
            get
            {
                return ep.Identity;
            }
        }

        /// <summary>
        /// �Ƿ��Զ���ͨ�ֶ�
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return ep.IsNormal;
            }
        }
        
        /// <summary>
        /// �Ƿ�汾��Ϣ�ֶ�
        /// </summary>
        public bool IsVersion
        {
            get
            {
                return ep.IsVersion;
            }
        }
    }
}
