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
        /// 实体字段标识
        /// </summary>
        public EntityParam() { }
        /// <summary>
        /// 实体字段标识
        /// </summary>
        /// <param name="paramName">数据库字段名</param>
        /// <param name="propertyName">本字段对应的属性名</param>
        /// <param name="sqlType">对应的SQL数据类型</param>
        /// <param name="propertyType">属性类型</param>
        public EntityParam(string fieldName, string paramName, string propertyName, DbType sqlType, 
            EntityPropertyType propertyType)
            : this(fieldName,paramName, propertyName, sqlType, propertyType, 0, true)
        {
            
        }
        /// <summary>
        /// 实体字段标识
        /// </summary>
        /// <param name="paramName">数据库字段名</param>
        /// <param name="propertyName">本字段对应的属性名</param>
        /// <param name="sqlType">对应的SQL数据类型</param>
        /// <param name="propertyType">属性类型</param>
        /// <param name="length">长度</param>
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
        /// 对应的字段名
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
        }
        

        /// <summary>
        /// 获取对应的字段名
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
        /// 获取对应的属性名
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
        /// 获取对应的数据库类型
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
        /// 属性类型
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
        /// 允许空
        /// </summary>
        public bool AllowNull
        {
            get { return _allowNull; }
            set { _allowNull = value; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return _length; }
        }
        /// <summary>
        /// 获取对应的字段是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.PrimaryKey);
            }
        }

        /// <summary>
        /// 是否自动增长字段
        /// </summary>
        public bool Identity
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Identity);
            }
        }

        /// <summary>
        /// 是否普通字段
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return EnumUnit.ContainerValue((int)_propertyType, (int)EntityPropertyType.Normal);
            }
        }

        /// <summary>
        /// 是否版本信息字段
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
