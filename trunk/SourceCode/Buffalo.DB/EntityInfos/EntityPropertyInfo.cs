using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Commons;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ʵ���������Ϣ
    /// </summary>
    public class EntityPropertyInfo:FieldInfoHandle
    {
        private EntityParam _paramInfo;


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
        public EntityPropertyInfo(EntityInfoHandle belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, EntityParam paramInfo, Type fieldType, string fieldName)
            : base(belong.EntityType, getHandle, setHandle,fieldType,fieldName)
        {
            paramInfo.SqlType = belong.DBInfo.CurrentDbAdapter.ToCurrentDbType(paramInfo.SqlType);//ת���ɱ����ݿ�֧�ֵ���������
            this._paramInfo = paramInfo;
            _belong = belong;
        }

        /// <summary>
        /// �ֶ�������Ϣ
        /// </summary>
        public EntityParam ParamInfo
        {
            get { return _paramInfo; }
        }
        /// <summary>
        /// ���ؿ�������
        /// </summary>
        /// <param name="belong">�¸���������ʵ��</param>
        /// <returns></returns>
        public EntityPropertyInfo Copy(EntityInfoHandle belong) 
        {

            EntityPropertyInfo info = new EntityPropertyInfo(belong, _getHandle,
                _setHandle, _paramInfo, _fieldType, _fieldName);
            return info;
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
                return _paramInfo.PropertyName;
            }
        }
        

        /// <summary>
        /// ��ȡ��Ӧ���ֶε�SQL����
        /// </summary>
        public DbType SqlType
        {
            get
            {
                return _paramInfo.SqlType;
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ���ֶε�����
        /// </summary>
        public string ParamName
        {
            get
            {
                return _paramInfo.ParamName;
            }
        }

        /// <summary>
        /// ��ȡ��Ӧ���ֶ��Ƿ�����
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return _paramInfo.IsPrimaryKey;
            }
        }

        /// <summary>
        /// �Ƿ��Զ������ֶ�
        /// </summary>
        public bool Identity
        {
            get
            {
                return _paramInfo.Identity;
            }
        }

        /// <summary>
        /// �Ƿ��Զ���ͨ�ֶ�
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return _paramInfo.IsNormal;
            }
        }
        
        /// <summary>
        /// �Ƿ�汾��Ϣ�ֶ�
        /// </summary>
        public bool IsVersion
        {
            get
            {
                return _paramInfo.IsVersion;
            }
        }
    }
}
