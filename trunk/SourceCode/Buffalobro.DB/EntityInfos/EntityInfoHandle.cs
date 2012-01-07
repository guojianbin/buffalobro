using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.Kernel.FastReflection;
using Buffalobro.DB.DataBaseAdapter;


namespace Buffalobro.DB.EntityInfos
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
        private EntityPropertyInfo _primaryProperty;//������
        private FieldInfoHandle _baseListInfo;
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
            Dictionary<string, EntityMappingInfo> mappingInfoHandles, FieldInfoHandle baseListInfo) 
        {
            this._propertyInfoHandles = new PropertyInfoCollection(propertyInfoHandles);
            this._mappingInfoHandles = new MappingInfoCollection(mappingInfoHandles);
            _baseListInfo = baseListInfo;
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
        /// �˶���Ĳ�ѯ���漯�Ͼ��
        /// </summary>
        public FieldInfoHandle BaseListInfo 
        {
            get 
            {
                return _baseListInfo;
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
        public EntityPropertyInfo PrimaryProperty
        {
            get
            {
                if (_primaryProperty == null) //�ҳ�������
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
                        throw new Exception("�˱�û������");
                    }
                }
                return _primaryProperty;
            }

        }
    }
}
