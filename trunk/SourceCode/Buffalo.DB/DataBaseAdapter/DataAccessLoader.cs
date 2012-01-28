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
    /// ���ݷ��ʲ�ļ��ض�ȡ��
    /// </summary>
    public class DataAccessLoader
    {
        
        private static Dictionary<string, Type> _dicLoaderConfig =null;//���ü�¼����
        private static Dictionary<string, Type> _dicEntityLoaderConfig = null;//����ʵ���¼����
        private static Dictionary<string, DBInfo> _dicDBInfo = null;//����ʵ���¼����
        
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
            
            return new DBInfo(name, connectionString, dbType);
        }

        /// <summary>
        /// ��Ŀ¼
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
        /// ����ģ����Ϣ
        /// </summary>
        private static void LoadModel()
        {
            List<Assembly> lstAss = GetAllAssembly();

        }

        /// <summary>
        /// ��ȡ��ģ�������г���
        /// </summary>
        /// <returns></returns>
        private static List<Assembly> GetAllAssembly() 
        {
            string baseRoot = GetBaseRoot();//����Ŀ���ڵ�·��
            Assembly[] arrAss = AppDomain.CurrentDomain.GetAssemblies();
            List<Assembly> lstAss = new List<Assembly>(arrAss.Length);
            foreach (Assembly ass in arrAss)
            {
                try
                {

                    if (ass.Location.IndexOf(baseRoot) == 0) //����˳������ڵ��ļ��ڱ���Ŀ·������ӵ������ֵ�
                    {
                        lstAss.Add(ass);
                    }
                }
                catch { }
            }
            return lstAss;
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
