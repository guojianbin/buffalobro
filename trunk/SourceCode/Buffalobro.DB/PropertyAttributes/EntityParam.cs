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
        /// 实体字段标识
        /// </summary>
        /// <param name="paramName">数据库字段名</param>
        /// <param name="propertyName">本字段对应的属性名</param>
        /// <param name="sqlType">对应的SQL数据类型</param>
        /// <param name="propertyType">属性类型</param>
        public EntityParam(string paramName, string propertyName, DbType sqlType, EntityPropertyType propertyType)
        :this(paramName,propertyName,sqlType,propertyType,0)
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
        public EntityParam(string paramName, string propertyName, DbType sqlType, EntityPropertyType propertyType,int length)
        {
            this._paramName = paramName;
            this._propertyName = propertyName;
            this._sqlType = sqlType;
            this._propertyType = propertyType;
            this._length = length;
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
    }
}
