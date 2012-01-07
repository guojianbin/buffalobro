using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalobro.Kernel.Commons;
using Buffalo.Kernel;
namespace Buffalobro.DB.PropertyAttributes
{
    public class EntityParam:System.Attribute
    {
        private string _paramName;
        private string _propertyName;
        private DbType _sqlType;
        private EntityPropertyType _propertyType;
        private int _length;


        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        /// <param name="paramName">���ݿ��ֶ���</param>
        /// <param name="propertyName">���ֶζ�Ӧ��������</param>
        /// <param name="sqlType">��Ӧ��SQL��������</param>
        /// <param name="propertyType">��������</param>
        public EntityParam(string paramName, string propertyName, DbType sqlType, EntityPropertyType propertyType)
        :this(paramName,propertyName,sqlType,propertyType,0)
        {
            
        }
        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        /// <param name="paramName">���ݿ��ֶ���</param>
        /// <param name="propertyName">���ֶζ�Ӧ��������</param>
        /// <param name="sqlType">��Ӧ��SQL��������</param>
        /// <param name="propertyType">��������</param>
        /// <param name="length">����</param>
        public EntityParam(string paramName, string propertyName, DbType sqlType, EntityPropertyType propertyType,int length)
        {
            this._paramName = paramName;
            this._propertyName = propertyName;
            this._sqlType = sqlType;
            this._propertyType = propertyType;
            this._length = length;
        }
        
        /// <summary>
        /// ��ȡ��Ӧ���ֶ���
        /// </summary>
        public string ParamName
        {
            get
            {
                return _paramName;
            }
            set 
            {
                _paramName = value;
            }
        }
        /// <summary>
        /// ��ȡ��Ӧ��������
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
            set
            {
                _propertyName = value;
            }
        }
        /// <summary>
        /// ��ȡ��Ӧ�����ݿ�����
        /// </summary>
        public DbType SqlType 
        {
            get
            {
                return _sqlType;
            }
            set
            {
                _sqlType = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public EntityPropertyType PropertyType
        {
            get
            {
                return _propertyType;
            }
            
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Length
        {
            get { return _length; }
        }
        /// <summary>
        /// ��ȡ��Ӧ���ֶ��Ƿ�����
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            }
        }

        /// <summary>
        /// �Ƿ��Զ������ֶ�
        /// </summary>
        public bool Identity
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);
            }
        }

        /// <summary>
        /// �Ƿ���ͨ�ֶ�
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Normal);
            }
        }

        /// <summary>
        /// �Ƿ�汾��Ϣ�ֶ�
        /// </summary>
        public bool IsVersion
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Version);
            }
        }
    }
}
