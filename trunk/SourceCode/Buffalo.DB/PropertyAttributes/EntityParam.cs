using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Commons;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
namespace Buffalo.DB.PropertyAttributes
{
    public class EntityParam:System.Attribute
    {
        private string _paramName;
        private string _propertyName;
        private DbType _sqlType;
        private EntityPropertyType _propertyType;
        private int _length;
        private bool _allowNull;
        private string _fieldName;

        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        public EntityParam() { }
        /// <summary>
        /// ʵ���ֶα�ʶ
        /// </summary>
        /// <param name="paramName">���ݿ��ֶ���</param>
        /// <param name="propertyName">���ֶζ�Ӧ��������</param>
        /// <param name="sqlType">��Ӧ��SQL��������</param>
        /// <param name="propertyType">��������</param>
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType, 
            EntityPropertyType propertyType)
            : this(fieldName,paramName, propertyName, sqlType, propertyType, 0, true)
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
        public EntityParam(string fieldName, string paramName, string propertyName,
            DbType sqlType, EntityPropertyType propertyType,int length,bool allowNull)
        {
            this._fieldName = fieldName;
            this._paramName = paramName;
            this._propertyName = propertyName;
            this._sqlType = sqlType;
            this._propertyType = propertyType;
            this._length = length;
            this._allowNull = allowNull;
        }
        /// <summary>
        /// ��Ӧ���ֶ���
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
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

        public string DisplayInfo(KeyWordInfomation info, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            sb.Append(idba.FormatTableName(ParamName) + " ");
            sb.Append(idba.DBTypeToSQL(SqlType, Length) + " ");

            bool isPrimary = EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            bool isAutoIdentity = EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);

            if (isAutoIdentity)
            {
                sb.Append(idba.DBIdentity(tableName, _paramName));
            }

            if (isPrimary)
            {
                sb.Append(" primary key ");
            }
            else
            {
                if (_allowNull)
                {
                    sb.Append("NULL");
                }
                else
                {
                    sb.Append("NOT NULL");
                }
            }
            return sb.ToString();
        }
    }
}
