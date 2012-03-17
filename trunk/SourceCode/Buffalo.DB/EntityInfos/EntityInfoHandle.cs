using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DataBaseAdapter;


namespace Buffalo.DB.EntityInfos
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
        private List<EntityPropertyInfo> _primaryProperty;//主属性
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
            Dictionary<string, EntityMappingInfo> mappingInfoHandles) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
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
        public List<EntityPropertyInfo> PrimaryProperty
        {
            get
            {
                if (_primaryProperty == null) //找出主属性
                {

                    _primaryProperty = new List<EntityPropertyInfo>();
                    foreach (EntityPropertyInfo info in _propertyInfoHandles)
                    {
                        
                        if (info.IsPrimaryKey) 
                        {
                            _primaryProperty.Add(info);
                        }
                    }
                    
                }
                return _primaryProperty;
            }

        }

        /// <summary>
        /// 子属性对应的映射
        /// </summary>
        private Dictionary<string,EntityMappingInfo> _dicSourceNameMap=null;

        /// <summary>
        /// 初始化子属性
        /// </summary>
        private void InitSourceNameMap() 
        {
            _dicSourceNameMap = new Dictionary<string, EntityMappingInfo>();
            foreach (EntityMappingInfo info in _mappingInfoHandles) 
            {
                if (info.IsParent) 
                {
                    _dicSourceNameMap[info.SourceProperty.PropertyName] = info;
                }
            }
        }

        /// <summary>
        /// 通过属性名获取映射
        /// </summary>
        /// <returns></returns>
        public EntityMappingInfo GetMappingBySourceName(string sourcePropertyName) 
        {
            if (_dicSourceNameMap == null) 
            {
                InitSourceNameMap();
            }
            EntityMappingInfo ret = null;
            _dicSourceNameMap.TryGetValue(sourcePropertyName, out ret);
            return ret;
        }
    }
}
