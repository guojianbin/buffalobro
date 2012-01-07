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
    /// 数据访问层的加载读取类
    /// </summary>
    public class DataAccessLoader
    {
        
        private static Dictionary<string, Type> _dicLoaderConfig =null;//配置记录集合
        private static Dictionary<string, Type> _dicEntityLoaderConfig = null;//配置实体记录集合
        private static Dictionary<string, DBInfo> _dicDBInfo = null;//配置实体记录集合
        private static AssemblyTypeLoader _assemblyTypeLoader = null;
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        internal static bool HasInit 
        {
            get 
            {
                return _dicLoaderConfig != null;
            }
        }

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        /// <param name="dbName">数据库名</param>
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

        #region 初始化配置

        

        /// <summary>
        /// 初始化配置
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
                throw new Exception("没有找到配置文件，请在config文件中加入DataAccessConfig的程序键");
            }
            return true;
        }

        /// <summary>
        /// 获取当前配置文件的数据库信息
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
                throw new Exception("找不到配置文件");
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
                throw new Exception("配置文件没有config节点");
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
        /// 加载信息
        /// </summary>
        /// <param name="info">配置信息</param>
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
        /// 缓存加载
        /// </summary>
        /// <param name="cInfo"></param>
        /// <param name="dbinfo"></param>
        private static void CacheLoad(ConfigInfo cInfo, DBInfo dbinfo) 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(cInfo.CacheFilePath);
            //数据层
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

            //实体层
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
        /// 普通加载
        /// </summary>
        /// <param name="cInfo">配置信息</param>
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
            //普通加载
            XmlNodeList lstEntity = cInfo.Document.GetElementsByTagName("dataAccess");
            if (lstEntity.Count > 0)
            {
                LoadDataAccesses(lstEntity, dbinfo);
            }
            LoadTypes(cInfo, dbinfo);

            SaveConfigToCache(cInfo);
        }

        /// <summary>
        /// 保存配置到缓存
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
        /// 加载类型信息
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
                if (LoadDataAccesses(objType, cInfo, dbinfo)) //加载业务层
                {
                    continue;
                }

                LoadEntity(objType,cInfo, dbinfo);//加载实体
            }

        }

        private static Type _entityBaseType = typeof(EntityBase);

        /// <summary>
        /// 加载实体
        /// </summary>
        /// <param name="objType">类型</param>
        /// <param name="entityNamespaces">实体命名空间</param>
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
        /// 加载程序集中的数据层
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
        /// 加载XML中的数据层
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
        /// 把信息添加到管理器
        /// </summary>
        /// <param name="interfaceName">接口名</param>
        /// <param name="classname">类名</param>
        /// <param name="dbinfo">数据库类型</param>
        private static void AddToManager(string interfaceName, string classname, DBInfo dbinfo) 
        {
            if (!_dicLoaderConfig.ContainsKey(interfaceName))
            {
                string typeName = classname;

                Type curType = _assemblyTypeLoader.LoadType(typeName);
                if (curType == null)
                {
                    throw new Exception("找不到类型:" + typeName + "请检查配置文件的程序域");
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
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType, DataBaseOperate oper)
        {
            InitConfig();
            Type objType = null;
            if (!_dicEntityLoaderConfig.TryGetValue(entityType.FullName, out objType))
            {
                throw new Exception("找不到对应的类型" + entityType.FullName+"的配置，请检查配置文件");
            }
            return Activator.CreateInstance(objType, oper);
        }

        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T),oper) as IDataAccessModel<T>;
        }
        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T), oper) as IViewDataAccess<T>;
        }
        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="interfaceType">实体类型</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType, DataBaseOperate oper) 
        {
            InitConfig();
            Type objType = null;
            if (!_dicLoaderConfig.TryGetValue(interfaceType.FullName, out objType)) 
            {
                throw new Exception("找不到接口" + interfaceType.FullName + "的对应配置，请检查配置文件");
            }
            return Activator.CreateInstance(objType, oper);
        }
       
        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="DataBaseOperate">链接</param>
        /// <returns></returns>
        public static T GetInstance<T>(DataBaseOperate oper)
        {
            return (T)GetInstance(typeof(T),oper);
        }



        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType)
        {
            InitConfig();
            Type objType = null;
            if (!_dicEntityLoaderConfig.TryGetValue(entityType.FullName, out objType))
            {
                throw new Exception("找不到对应的类型");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>()
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T)) as IDataAccessModel<T>;
        }
        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>()
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T)) as IViewDataAccess<T>;
        }
        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="interfaceType">实体类型</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType)
        {
            InitConfig();
            Type objType = null;
            if (!_dicLoaderConfig.TryGetValue(interfaceType.FullName, out objType))
            {
                throw new Exception("找不到对应的类型");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// 根据实体类型获取对应的数据访问层的实例
        /// </summary>
        /// <param name="DataBaseOperate">链接</param>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
    }
}
