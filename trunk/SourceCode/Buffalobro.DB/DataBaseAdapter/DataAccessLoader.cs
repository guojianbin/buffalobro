using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Buffalobro.DB.CommBase;
using Buffalobro.DB.CommBase.DataAccessBases;
using Buffalobro.DB.DbCommon;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.EntityInfos;
using Buffalobro.Kernel.FastReflection;
using System.IO;

namespace Buffalobro.DB.DataBaseAdapter
{
    /// <summary>
    /// ���ݷ��ʲ�ļ��ض�ȡ��
    /// </summary>
    public class DataAccessLoader
    {
        
        private static Dictionary<string, Type> _dicLoaderConfig =null;//���ü�¼����
        private static Dictionary<string, Type> _dicEntityLoaderConfig = null;//����ʵ���¼����
        private static Dictionary<string, DBInfo> _dicDBInfo = null;//����ʵ���¼����
        private static AssemblyTypeLoader _assemblyTypeLoader = null;
        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        internal static bool HasInit 
        {
            get 
            {
                return _dicLoaderConfig != null;
            }
        }

        /// <summary>
        /// ��ȡ���ݿ���Ϣ
        /// </summary>
        /// <param name="dbName">���ݿ���</param>
        /// <returns></returns>
        public static DBInfo GetDBInfo(string dbName) 
        {
            if (_dicDBInfo == null)
            {
                return null;
            }
            DBInfo ret=null;
            _dicDBInfo.TryGetValue(dbName, out ret);
            return ret;
        }

        #region ��ʼ������

        

        /// <summary>
        /// ��ʼ������
        /// </summary>
        internal static bool InitConfig()
        {
            if (HasInit) 
            {
                return true;
            }
            _dicLoaderConfig = new Dictionary<string, Type>();
            _dicEntityLoaderConfig = new Dictionary<string, Type>();
            List<ConfigInfo> docs = ConfigXmlLoader.LoadXml("DataAccessConfig");
            if (docs.Count > 0)
            {
                _dicDBInfo = new Dictionary<string, DBInfo>();
                foreach (ConfigInfo doc in docs)
                {
                    
                    LoadConfig(doc);
                }
            }
            else 
            {
                throw new Exception("û���ҵ������ļ�������config�ļ��м���DataAccessConfig�ĳ����");
            }
            return true;
        }

        /// <summary>
        /// ��ȡ��ǰ�����ļ������ݿ���Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DBInfo GetDBInfo(XmlDocument doc) 
        {
            string dbType = null;
            string connectionString = null;
            string name = null;
            //string output = null;
            string[] attNames = null;
            //Dictionary<string, string> extendDatabaseConnection = new Dictionary<string,string>();
            if (doc == null)
            {
                throw new Exception("�Ҳ��������ļ�");
            }
            XmlNodeList lstConfig = doc.GetElementsByTagName("config");
            if (lstConfig.Count > 0)
            {
                XmlNode node = lstConfig[0];
                foreach (XmlAttribute att in node.Attributes)
                {
                    if (att.Name.Equals("dbType",StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbType = att.InnerText;
                    }
                    //else if (att.Name.Equals("output", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    output = att.InnerText;
                    //}
                    else if (att.Name.Equals("name",StringComparison.CurrentCultureIgnoreCase))
                    {
                        name=att.InnerText;
                    }
                    else if (att.Name.Equals("connectionString",StringComparison.CurrentCultureIgnoreCase))
                    {
                        connectionString = att.InnerText;
                    }
                    else if (att.Name.Equals("assembly",StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        string ass=att.InnerText;
                        
                        if(!string.IsNullOrEmpty(ass))
                        {
                            attNames = ass.Split(new char[] { '|' });
                        }
                       
                    }
                }
            }
            else
            {
                throw new Exception("�����ļ�û��config�ڵ�");
            }
            if (attNames == null)
            {
                _assemblyTypeLoader = AssemblyTypeLoader.Default;
            }
            else
            {
                _assemblyTypeLoader = new AssemblyTypeLoader(attNames);
            }
            return new DBInfo(name, connectionString, dbType);
        }



        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="info">������Ϣ</param>
        private static void LoadConfig(ConfigInfo info)
        {

            XmlDocument doc = info.Document;
            DBInfo dbinfo=GetDBInfo(doc);
            _dicDBInfo[dbinfo.Name]=dbinfo;

            if (File.Exists(info.CacheFilePath))
            {
                CacheLoad(info, dbinfo);
            }
            else
            {
                NormalLoad(info, dbinfo);
            }
            _assemblyTypeLoader = null;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="cInfo"></param>
        /// <param name="dbinfo"></param>
        private static void CacheLoad(ConfigInfo cInfo, DBInfo dbinfo) 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(cInfo.CacheFilePath);
            //���ݲ�
            XmlNodeList lstConfig = doc.GetElementsByTagName("dataaccess");
            if (lstConfig.Count > 0)
            {
                XmlNode node = lstConfig[0];
                foreach (XmlNode dnode in node.ChildNodes) 
                {
                    Type objType = LoadTypeInfo.LoadFromXmlNode(dnode, _assemblyTypeLoader);
                    if(objType!=null)
                    {
                        LoadDataAccesses(objType, cInfo, dbinfo);
                    }
                }
            }

            //ʵ���
            lstConfig = doc.GetElementsByTagName("entity");
            if (lstConfig.Count > 0)
            {
                XmlNode node = lstConfig[0];
                foreach (XmlNode dnode in node.ChildNodes)
                {
                    Type objType = LoadTypeInfo.LoadFromXmlNode(dnode, _assemblyTypeLoader);
                    if (objType != null)
                    {
                        LoadEntity(objType, cInfo, dbinfo);
                    }
                }
            }
        }

        /// <summary>
        /// ��ͨ����
        /// </summary>
        /// <param name="cInfo">������Ϣ</param>
        /// <param name="dbinfo"></param>
        private static void NormalLoad(ConfigInfo cInfo, DBInfo dbinfo) 
        {
            XmlDocument doc = cInfo.Document;
            XmlNodeList lstConfig = doc.GetElementsByTagName("config");
            if (lstConfig.Count > 0)
            {
                XmlNode node = lstConfig[0];
                XmlAttribute attNamespace = node.Attributes["appnamespace"];
                if (attNamespace != null)
                {
                    string aNamespaces = attNamespace.Value;
                    cInfo.AddDalNamespaces(aNamespaces);
                }

                attNamespace = node.Attributes["entitynamespace"];
                if (attNamespace != null)
                {
                    string aNamespaces = attNamespace.Value;
                    cInfo.AddEntityNamespaces(aNamespaces);
                }
            }
            //��ͨ����
            XmlNodeList lstEntity = cInfo.Document.GetElementsByTagName("dataAccess");
            if (lstEntity.Count > 0)
            {
                LoadDataAccesses(lstEntity, dbinfo);
            }
            LoadTypes(cInfo, dbinfo);

            SaveConfigToCache(cInfo);
        }

        /// <summary>
        /// �������õ�����
        /// </summary>
        /// <param name="cInfo"></param>
        private static void SaveConfigToCache(ConfigInfo cInfo) 
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode=doc.CreateElement("cache");
            doc.AppendChild(rootNode);

            XmlNode dataaccess = doc.CreateElement("dataaccess");
            rootNode.AppendChild(dataaccess);
            foreach (KeyValuePair<string, Type> pair in _dicLoaderConfig) 
            {
                Type objType = pair.Value;
                string typeName = objType.FullName;
                if (cInfo.IsDalNamespace(typeName))
                {
                    LoadTypeInfo.AppendNode(dataaccess, objType);
                }
            }

            XmlNode entity = doc.CreateElement("entity");
            rootNode.AppendChild(entity);

            foreach (KeyValuePair<string, EntityInfoHandle> pair in EntityInfoManager.AllEntity)
            {
                Type objType = pair.Value.EntityType;
                string typeName = objType.FullName;
                if (cInfo.IsEntityNamespace(typeName))
                {
                    LoadTypeInfo.AppendNode(entity, objType);
                }
            }
            doc.Save(cInfo.CacheFilePath);
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="cInfo"></param>
        /// <param name="dbinfo"></param>
        private static void LoadTypes(ConfigInfo cInfo, DBInfo dbinfo) 
        {

            List<Type> assTypes = _assemblyTypeLoader.GetTypes();
            foreach (Type objType in assTypes)
            {
                //dbinfo.SqlOutputer.OutPut("Type:", objType.FullName);
                if (!objType.IsClass || objType.IsGenericType ||objType.IsAbstract) 
                {
                    continue;
                }
                if (LoadDataAccesses(objType, cInfo, dbinfo)) //����ҵ���
                {
                    continue;
                }

                LoadEntity(objType,cInfo, dbinfo);//����ʵ��
            }

        }

        private static Type _entityBaseType = typeof(EntityBase);

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="objType">����</param>
        /// <param name="entityNamespaces">ʵ�������ռ�</param>
        /// <param name="dbinfo"></param>
        /// <returns></returns>
        private static bool LoadEntity(Type objType, ConfigInfo cInfo, DBInfo dbinfo) 
        {
            bool isBaseType = DefaultType.IsInherit(objType, _entityBaseType);
            if (!isBaseType) 
            {
                return false;
            }
            string typeName = objType.FullName;




            if (cInfo.IsEntityNamespace(typeName))
            {

                EntityInfoManager.SetEntityHandle(objType, dbinfo);
            }
            return true;
        }

        /// <summary>
        /// ���س����е����ݲ�
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="cInfo"></param>
        /// <param name="dbinfo"></param>
        private static bool LoadDataAccesses(Type objType,ConfigInfo cInfo, DBInfo dbinfo)
        {
            object[] atts = objType.GetCustomAttributes(typeof(IDalAttribute), false);
            if (atts.Length <= 0)
            {
                return false;
            }

            IDalAttribute att = atts[0] as IDalAttribute;
            string typeName = objType.FullName;


            if (cInfo.IsDalNamespace(typeName)) 
            {
                _dicLoaderConfig.Add(att.InterfaceType.FullName, objType);

                
                Type[] gTypes = DefaultType.GetGenericType(objType, true);
                if (gTypes != null && gTypes.Length > 0)
                {
                    Type gType = gTypes[0];
                    _dicEntityLoaderConfig[gType.FullName] = objType;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// ����XML�е����ݲ�
        /// </summary>
        /// <param name="lstEntity"></param>
        /// <param name="dbinfo"></param>
        private static void LoadDataAccesses(XmlNodeList lstEntity, DBInfo dbinfo) 
        {
            foreach (XmlNode node in lstEntity)
            {
                XmlAttribute attName = node.Attributes["classname"];
                XmlAttribute attInterface = node.Attributes["interface"];
                if (attName != null && attInterface != null)
                {
                    string classname = attName.Value;
                    string interfaceName = attInterface.Value;

                    AddToManager(interfaceName, classname, dbinfo);
                }
            }
        }

        /// <summary>
        /// ����Ϣ��ӵ�������
        /// </summary>
        /// <param name="interfaceName">�ӿ���</param>
        /// <param name="classname">����</param>
        /// <param name="dbinfo">���ݿ�����</param>
        private static void AddToManager(string interfaceName, string classname, DBInfo dbinfo) 
        {
            if (!_dicLoaderConfig.ContainsKey(interfaceName))
            {
                string typeName = classname;

                Type curType = _assemblyTypeLoader.LoadType(typeName);
                if (curType == null)
                {
                    throw new Exception("�Ҳ�������:" + typeName + "���������ļ��ĳ�����");
                }
                _dicLoaderConfig.Add(interfaceName, curType);

                Type[] gTypes = DefaultType.GetGenericType(curType, true);
                if (gTypes != null && gTypes.Length > 0)
                {
                    Type gType = gTypes[0];
                    _dicEntityLoaderConfig[gType.FullName] = curType;
                }
            }
        }


        #endregion

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType, DataBaseOperate oper)
        {
            InitConfig();
            Type objType = null;
            if (!_dicEntityLoaderConfig.TryGetValue(entityType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������" + entityType.FullName+"�����ã����������ļ�");
            }
            return Activator.CreateInstance(objType, oper);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T),oper) as IDataAccessModel<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T), oper) as IViewDataAccess<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="interfaceType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType, DataBaseOperate oper) 
        {
            InitConfig();
            Type objType = null;
            if (!_dicLoaderConfig.TryGetValue(interfaceType.FullName, out objType)) 
            {
                throw new Exception("�Ҳ����ӿ�" + interfaceType.FullName + "�Ķ�Ӧ���ã����������ļ�");
            }
            return Activator.CreateInstance(objType, oper);
        }
       
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="DataBaseOperate">����</param>
        /// <returns></returns>
        public static T GetInstance<T>(DataBaseOperate oper)
        {
            return (T)GetInstance(typeof(T),oper);
        }



        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType)
        {
            InitConfig();
            Type objType = null;
            if (!_dicEntityLoaderConfig.TryGetValue(entityType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>()
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T)) as IDataAccessModel<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>()
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T)) as IViewDataAccess<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="interfaceType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType)
        {
            InitConfig();
            Type objType = null;
            if (!_dicLoaderConfig.TryGetValue(interfaceType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="DataBaseOperate">����</param>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
    }
}
