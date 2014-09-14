using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Commons;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DBCheckers;
namespace Buffalo.DB.PropertyAttributes
{
    public class EntityParam:System.Attribute
    {
        private string _paramName;
        private string _propertyName;
        private DbType _sqlType;
        private EntityPropertyType _propertyType;
        private long _length;
        private bool _allowNull;
        private string _fieldName;
        private bool _readonly;
        private string _sequenceName;


        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        public EntityParam() 
        {
            _readonly = false;

        }
        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        /// <param name="paramName">���ݿ��ֶ���</param>
        /// <param name="propertyName">���ֶζ�Ӧ��������</param>
        /// <param name="sqlType">��Ӧ��SQL��������</param>
        /// <param name="propertyType">��������</param>
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType,
            EntityPropertyType propertyType, string description)
            : this(fieldName,paramName, propertyName, sqlType, propertyType,description, 0, true,false )
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
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType,
            EntityPropertyType propertyType, string description, int length, bool allowNull, bool isReadOnly)
        {
            this._fieldName = fieldName;
            this._paramName = paramName;
            this._propertyName = propertyName;
            this._sqlType = sqlType;
            this._propertyType = propertyType;
            this._length = length;
            this._allowNull = allowNull;
            this._readonly = isReadOnly;
            this._description = description;
        }
        /// <summary>
        /// ��Ӧ���ֶ���
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        /// <summary>
        /// �Ƿ�ֻ��
        /// </summary>
        public bool ReadOnly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        private string _description;

        /// <summary>
        /// �ֶ�ע��
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string SequenceName
        {
            get { return _sequenceName; }
            internal set { _sequenceName = value; }
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
            set 
            {
                _propertyType = value;
            }
        }
        /// <summary>
        /// �����
        /// </summary>
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public long Length
        {
            get { return _length; }
            internal set { _length = value; }
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

        public string DisplayInfo(KeyWordInfomation info, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            bool isPrimary = EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            bool isAutoIdentity = EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);

            
            bool needIdentity = false;
            bool putType = true;
            sb.Append(idba.FormatParam(ParamName) + " ");

            if (isAutoIdentity && TableChecker.IsIdentityType(SqlType))
            {
                if (idba.IdentityIsType)
                {
                    sb.Append(idba.DBIdentity(tableName, _paramName));
                    sb.Append(" ");
                    putType = false;
                }
                else
                {
                    needIdentity = true;
                }
            }
            
            
            if (putType)
            {
                sb.Append(idba.DBTypeToSQL(SqlType, Length) + " ");
            }
            
            bool allowNULL = _allowNull & (!isPrimary);
            //if (isPrimary)
            //{
            //    sb.Append(" primary key ");
            //}
            //else
            //{
            if (isPrimary && info.PrimaryKeys==1)
            {
                sb.Append(" PRIMARY KEY ");
            }
            else
            {
                if (allowNULL)
                {
                    sb.Append("NULL ");
                }
                else
                {
                    sb.Append("NOT NULL ");
                }
            }
            //}
            if (needIdentity&&isAutoIdentity && TableChecker.IsIdentityType(SqlType))
            {

                sb.Append(idba.DBIdentity(tableName, _paramName));
            }
            return sb.ToString();
        }
    }
}
