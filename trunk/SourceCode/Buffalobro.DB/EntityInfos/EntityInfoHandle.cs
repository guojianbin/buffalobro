using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.FastReflection;
using Buffalobro.DB.DataBaseAdapter;


namespace Buffalobro.DB.EntityInfos
{
    /// <summary>
    /// 类的信息
    /// </summary>
    public class EntityInfoHandle 
    {
        
        private Type entityType;
        private string tableName;
        //private string connectionKey;
        private CreateInstanceHandler createInstanceHandler;
        private PropertyInfoCollection _propertyInfoHandles;
        private MappingInfoCollection _mappingInfoHandles;
        private EntityPropertyInfo _primaryProperty;//主属性
        private FieldInfoHandle _baseListInfo;
        private DBInfo _dbInfo;
        /// <summary>
        /// 类的信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="createInstanceHandler">实例化类的句柄</param>
        /// <param name="propertyInfoHandles">属性集合</param>
        /// <param name="tableName">对应的表名</param>
        /// <param name="baseListInfo">此对象的查询缓存集合句柄</param>
        /// <param name="connectionKey">连接字符串的键</param>
        internal EntityInfoHandle(Type entityType, CreateInstanceHandler createInstanceHandler, 
             string tableName,  DBInfo db) 
        {
            this.entityType = entityType;
            this.createInstanceHandler = createInstanceHandler;
            
            this.tableName = tableName;
            //this.connectionKey = connectionKey;
            this._dbInfo = db;
        }

        /// <summary>
        /// 设置属性和映射信息
        /// </summary>
        /// <param name="propertyInfoHandles">属性信息集合</param>
        /// <param name="mappingInfoHandles">映射信息集合</param>
        /// <param name="baseListInfo">所属的基集合</param>
        internal void SetInfoHandles(Dictionary<string, EntityPropertyInfo> propertyInfoHandles,
            Dictionary<string, EntityMappingInfo> mappingInfoHandles, FieldInfoHandle baseListInfo) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
            _baseListInfo = baseListInfo;
        }

        /// <summary>
        /// 当前所属数据库的信息
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _dbInfo;
            }
        }

        /// <summary>
        /// 本实体的类型
        /// </summary>
        public Type EntityType 
        {
            get 
            {
                return entityType;
            }
        }

        /// <summary>
        /// 此对象的查询缓存集合句柄
        /// </summary>
        public FieldInfoHandle BaseListInfo 
        {
            get 
            {
                return _baseListInfo;
            }
        }

        /// <summary>
        /// 对应的表名名
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
        }
        ///// <summary>
        ///// 连接字符串的键
        ///// </summary>
        //public string ConnectionKey
        //{
        //    get
        //    {
        //        return connectionKey;
        //    }
        //}
        /// <summary>
        /// 获取属性的信息
        /// </summary>
        public PropertyInfoCollection PropertyInfo
        {
            get 
            {
                return _propertyInfoHandles;
            }
        }

        /// <summary>
        /// 获取映射的信息
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public MappingInfoCollection MappingInfo
        {
            get
            {
                return _mappingInfoHandles;
            }
        }

        

        /// <summary>
        /// 返回此类型的实例
        /// </summary>
        /// <returns></returns>
        public object CreateInstance() 
        {
            if (createInstanceHandler != null) 
            {
                return createInstanceHandler.Invoke();
            }
            return null;
        }

        /// <summary>
        /// 主属性
        /// </summary>
        public EntityPropertyInfo PrimaryProperty
        {
            get
            {
                if (_primaryProperty == null) //找出主属性
                {
                    //Dictionary<string, EntityPropertyInfo>.Enumerator enums = propertyInfoHandles.GetPropertyEnumerator();
                    foreach (EntityPropertyInfo info in _propertyInfoHandles)
                    {
                        //EntityPropertyInfo info = enums.Current.Value;
                        if (info.IsPrimaryKey) 
                        {
                            _primaryProperty = info;
                            break;
                        }
                    }
                    if (_primaryProperty == null) 
                    {
                        throw new Exception("此表没有主键");
                    }
                }
                return _primaryProperty;
            }

        }
    }
}
