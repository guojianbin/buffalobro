using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase;
using Buffalo.DB.ProxyBuilder;
using Buffalo.DB.PropertyAttributes;


namespace Buffalo.DB.EntityInfos
{


    /// <summary>
    /// �����Ϣ
    /// </summary>
    public class EntityInfoHandle 
    {
        
        private Type _entityType;
        //private string connectionKey;
        private CreateInstanceHandler _createInstanceHandler;
        private PropertyInfoCollection _propertyInfoHandles;
        private MappingInfoCollection _mappingInfoHandles;
        private List<EntityPropertyInfo> _primaryProperty;//������
        private DBInfo _dbInfo;
        private CreateInstanceHandler _createProxyInstanceHandler;
        private TableAttribute _tableInfo;
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <param name="createInstanceHandler">ʵ������ľ��</param>
        /// <param name="propertyInfoHandles">���Լ���</param>
        /// <param name="tableName">��Ӧ�ı���</param>
        /// <param name="baseListInfo">�˶���Ĳ�ѯ���漯�Ͼ��</param>
        /// <param name="connectionKey">�����ַ����ļ�</param>
        internal EntityInfoHandle(Type entityType, CreateInstanceHandler createInstanceHandler,
             TableAttribute tableInfo, DBInfo db) 
        {
            this._entityType = entityType;
            this._createInstanceHandler = createInstanceHandler;

            this._tableInfo = tableInfo;
            //this.connectionKey = connectionKey;
            this._dbInfo = db;
        }

        /// <summary>
        /// �������Ժ�ӳ����Ϣ
        /// </summary>
        /// <param name="propertyInfoHandles">������Ϣ����</param>
        /// <param name="mappingInfoHandles">ӳ����Ϣ����</param>
        /// <param name="baseListInfo">�����Ļ�����</param>
        internal void SetInfoHandles(Dictionary<string, EntityPropertyInfo> propertyInfoHandles,
            Dictionary<string, EntityMappingInfo> mappingInfoHandles) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
            
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        internal void InitProxyType(EntityProxyBuilder proxyBuilder) 
        {
            _proxyType = proxyBuilder.CreateProxyType(_entityType);
            _createProxyInstanceHandler = FastInvoke.GetInstanceCreator(_proxyType);
        }

        /// <summary>
        /// ��ǰ�������ݿ����Ϣ
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _dbInfo;
            }
        }
        /// <summary>
        /// ��ʵ�������
        /// </summary>
        public Type EntityType 
        {
            get 
            {
                return _entityType;
            }
        }
        /// <summary>
        /// ע��
        /// </summary>
        public string Description
        {
            get
            {
                return _tableInfo.Description;
            }
        }
        /// <summary>
        /// ��Ӧ�ı�����
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableInfo.TableName;
            }
        }
        ///// <summary>
        ///// �����ַ����ļ�
        ///// </summary>
        //public string ConnectionKey
        //{
        //    get
        //    {
        //        return connectionKey;
        //    }
        //}
        /// <summary>
        /// ��ȡ���Ե���Ϣ
        /// </summary>
        public PropertyInfoCollection PropertyInfo
        {
            get 
            {
                return _propertyInfoHandles;
            }
        }

        /// <summary>
        /// ��ȡӳ�����Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public MappingInfoCollection MappingInfo
        {
            get
            {
                return _mappingInfoHandles;
            }
        }

        private Type _proxyType;
        /// <summary>
        /// ������
        /// </summary>
        public Type ProxyType
        {
            get 
            { 

                return _proxyType; 
            }
        }
        

        /// <summary>
        /// ���ش����͵�ʵ��
        /// </summary>
        /// <returns></returns>
        public object CreateInstance() 
        {
            if (_createInstanceHandler != null) 
            {
                return _createInstanceHandler.Invoke();
            }
            return null;
        }
        /// <summary>
        /// ���ش����͵Ĵ�����ʵ��
        /// </summary>
        /// <returns></returns>
        public object CreateProxyInstance()
        {
            if (_createProxyInstanceHandler != null)
            {
                return _createProxyInstanceHandler.Invoke();
            }
            return null;
        }
        /// <summary>
        /// ���ش����͵Ĵ�����ʵ��(���ڲ�ѯ)
        /// </summary>
        /// <returns></returns>
        public object CreateSelectProxyInstance()
        {
            object obj = CreateProxyInstance();
            EntityBase eobj = obj as EntityBase;
            eobj.PrimaryKeyChange();
            return obj;
        }
        /// <summary>
        /// ������
        /// </summary>
        public List<EntityPropertyInfo> PrimaryProperty
        {
            get
            {
                if (_primaryProperty == null) //�ҳ�������
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


        private Dictionary<string, UpdatePropertyInfo> _dicUpdateInfo = null;

        /// <summary>
        /// ��ʼ�����Եĸ�����Ϣ
        /// </summary>
        private Dictionary<string, UpdatePropertyInfo> InitPropertyUpdateInfo() 
        {
            Dictionary<string, UpdatePropertyInfo> dic = new Dictionary<string, UpdatePropertyInfo>();
            foreach (EntityMappingInfo mapInfo in this.MappingInfo)
            {
                if (mapInfo.IsParent) //����Ǹ���ʵ��
                {
                    dic[mapInfo.PropertyName] = new UpdatePropertyInfo(mapInfo, true);
                    dic[mapInfo.SourceProperty.PropertyName] = new UpdatePropertyInfo(mapInfo, false);
                }
            }

            
            return dic;
        }

        /// <summary>
        /// ��ȡ�����ԵĹ���������Ϣ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public UpdatePropertyInfo GetUpdatePropertyInfo(string propertyName) 
        {
            if (_dicUpdateInfo == null) 
            {
                _dicUpdateInfo = InitPropertyUpdateInfo();
            }
            UpdatePropertyInfo ret = null;
            if (_dicUpdateInfo.TryGetValue(propertyName, out ret))
            {
                return ret;
            } 
            return null;
        }

    }
}
