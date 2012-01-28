using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.FastReflection;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// 数据访问层的加载读取类
    /// </summary>
    public class DataAccessLoader
    {
        
        private static Dictionary<string, Type> _dicLoaderConfig =null;//配置记录集合
        private static Dictionary<string, Type> _dicEntityLoaderConfig = null;//配置实体记录集合
        private static Dictionary<string, DBInfo> _dicDBInfo = null;//配置实体记录集合
        
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
            Dictionary<string, XmlDocument> dicEntityConfig = new Dictionary<string, XmlDocument>();
            List<ConfigInfo> docs = ConfigXmlLoader.LoadXml("DataAccessConfig");
            if (docs.Count > 0)
            {
                _dicDBInfo = new Dictionary<string, DBInfo>();
                foreach (ConfigInfo doc in docs)
                {
                    XmlDocument docInfo = doc.Document;
                    DBInfo dbinfo = GetDBInfo(docInfo);
                    _dicDBInfo[dbinfo.Name] = dbinfo;
                    
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
            
            return new DBInfo(name, connectionString, dbType);
        }

        /// <summary>
        /// 基目录
        /// </summary>
        /// <returns></returns>
        private string GetBaseRoot()
        {
            if (CommonMethods.IsWebContext)
            {
                return AppDomain.CurrentDomain.DynamicDirectory;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        
        /// <summary>
        /// 加载模块信息
        /// </summary>
        private static void LoadModel()
        {
            List<Assembly> lstAss = GetAllAssembly();

        }

        /// <summary>
        /// 获取本模块下所有程序集
        /// </summary>
        /// <returns></returns>
        private static List<Assembly> GetAllAssembly() 
        {
            string baseRoot = GetBaseRoot();//本项目所在的路径
            Assembly[] arrAss = AppDomain.CurrentDomain.GetAssemblies();
            List<Assembly> lstAss = new List<Assembly>(arrAss.Length);
            foreach (Assembly ass in arrAss)
            {
                try
                {

                    if (ass.Location.IndexOf(baseRoot) == 0) //如果此程序集所在的文件在本项目路径下则加到程序集字典
                    {
                        lstAss.Add(ass);
                    }
                }
                catch { }
            }
            return lstAss;
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
