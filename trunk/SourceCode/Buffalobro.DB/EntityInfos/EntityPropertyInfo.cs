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
    /// 实体的属性信息
    /// </summary>
    public class EntityPropertyInfo:FieldInfoHandle
    {
        private EntityParam ep;
        private EntityInfoHandle _belong;

        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">所属的实体信息</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="ep">字段标识类</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldName">字段名</param>
        public EntityPropertyInfo(EntityInfoHandle belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, EntityParam ep, Type fieldType, string fieldName,DBInfo info)
            : base(belong.EntityType, getHandle, setHandle,fieldType,fieldName)
        {
            ep.SqlType = info.CurrentDbAdapter.ToCurrentDbType(ep.SqlType);//转换成本数据库支持的数据类型
            this.ep = ep;
            _belong = belong;
        }

        /// <summary>
        /// 所属实体类型
        /// </summary>
        public EntityInfoHandle BelongInfo
        {
            get 
            {
                return _belong;
            }
        }

        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return ep.PropertyName;
            }
        }
        

        /// <summary>
        /// 获取对应的字段的SQL类型
        /// </summary>
        public DbType SqlType
        {
            get
            {
                return ep.SqlType;
            }
        }

        /// <summary>
        /// 获取对应的字段的名字
        /// </summary>
        public string ParamName
        {
            get
            {
                return ep.ParamName;
            }
        }

        /// <summary>
        /// 获取对应的字段是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return ep.IsPrimaryKey;
            }
        }

        /// <summary>
        /// 是否自动增长字段
        /// </summary>
        public bool Identity
        {
            get
            {
                return ep.Identity;
            }
        }

        /// <summary>
        /// 是否自动普通字段
        /// </summary>
        public bool IsNormal
        {
            get
            {
                return ep.IsNormal;
            }
        }
        
        /// <summary>
        /// 是否版本信息字段
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
