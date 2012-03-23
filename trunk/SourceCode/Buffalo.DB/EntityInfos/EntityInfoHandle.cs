using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase;


namespace Buffalo.DB.EntityInfos
{


    /// <summary>
    /// �����Ϣ
    /// </summary>
    public class EntityInfoHandle 
    {
        
        private Type entityType;
        private string tableName;
        //private string connectionKey;
        private CreateInstanceHandler createInstanceHandler;
        private PropertyInfoCollection _propertyInfoHandles;
        private MappingInfoCollection _mappingInfoHandles;
        private List<EntityPropertyInfo> _primaryProperty;//������
        private DBInfo _dbInfo;
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
             string tableName,  DBInfo db) 
        {
            this.entityType = entityType;
            this.createInstanceHandler = createInstanceHandler;
            
            this.tableName = tableName;
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
            _dicUpdateInfo = InitPropertyUpdateInfo();
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
                return entityType;
            }
        }

        /// <summary>
        /// ��Ӧ�ı�����
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
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

        

        /// <summary>
        /// ���ش����͵�ʵ��
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
            UpdatePropertyInfo ret = null;
            if (_dicUpdateInfo.TryGetValue(propertyName, out ret))
            {
                return ret;
            } 
            return null;
        }

    }
}
