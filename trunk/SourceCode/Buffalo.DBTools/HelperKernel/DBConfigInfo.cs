using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.IO;

namespace Buffalo.DBTools.HelperKernel
{
    public class DBConfigInfo
    {
        private string _dbName;

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        private string _appNamespace;
        /// <summary>
        /// 数据层所在的命名空间
        /// </summary>
        public string AppNamespace
        {
            get { return _appNamespace; }
            set { _appNamespace = value; }
        }

        private string _assembly;
        /// <summary>
        /// 程序集(一般留空)
        /// </summary>
        public string Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }

        private string _dbType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        private string _connectionString;
        /// <summary>
        /// 数据连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        private string _fileName;

        /// <summary>
        /// 所在文件路径
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private int _tier;

        /// <summary>
        /// 层数
        /// </summary>
        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        

        /// <summary>
        /// 创建数据库信息
        /// </summary>
        /// <returns></returns>
        public DBInfo CreateDBInfo() 
        {
            DBInfo info = new DBInfo(DbName, ConnectionString, DbType);
            return info;
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="curDiagram"></param>
        public static string GetFileName(Project curProject, Diagram curDiagram) 
        {
            string dbName = curDiagram.AccessibleName;
            string proFile = curProject.FileName;
            FileInfo file = new FileInfo(proFile);
            string directory = file.DirectoryName;
            return directory + "\\" + dbName + ".xml";

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        public void SaveConfig(string path) 
        {
            XmlDocument doc = EntityMappingConfig.NewXmlDocument();
            XmlNode configNode = doc.CreateElement("config");
            doc.AppendChild(configNode);

            XmlAttribute att = doc.CreateAttribute("connectionString");
            att.InnerText = this.ConnectionString;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("dbType");
            att.InnerText = this.DbType;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("appnamespace");
            att.InnerText = this.AppNamespace;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("assembly");
            att.InnerText = this.Assembly;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("name");
            att.InnerText = this.DbName;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("tier");
            att.InnerText = this.Tier.ToString();
            configNode.Attributes.Append(att);

            EntityMappingConfig.SaveXML(path, doc);
        }

        /// <summary>
        /// 加载数据库信息
        /// </summary>
        /// <param name="curProject">当前工程</param>
        /// <param name="curDiagram">当前图</param>
        /// <returns></returns>
        public static DBConfigInfo LoadInfo(Project curProject,Diagram curDiagram) 
        {
            string xmlFieName = GetFileName(curProject, curDiagram);
            DBConfigInfo ret = null;
            XmlDocument doc = EntityMappingConfig.NewXmlDocument();
            try
            {
                doc.Load(xmlFieName);
                ret = LoadInfo(doc);
                ret.FileName = xmlFieName;
            }
            catch 
            {
                
            }
            return ret;
        }

        /// <summary>
        /// 加载数据库配置信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DBConfigInfo LoadInfo(XmlDocument doc)
        {
            XmlNodeList lstConfig = doc.GetElementsByTagName("config");
            if (lstConfig.Count > 0)
            {
                DBConfigInfo info = new DBConfigInfo();
                XmlNode config = lstConfig[0];

                XmlAttribute att = config.Attributes["connectionString"];
                if (att != null)
                {
                    info.ConnectionString = att.InnerText;
                }

                att = config.Attributes["dbType"];
                if (att != null)
                {
                    info.DbType = att.InnerText;
                }

                att = config.Attributes["appnamespace"];
                if (att != null)
                {
                    info.AppNamespace = att.InnerText;
                }
                att = config.Attributes["assembly"];
                if (att != null)
                {
                    info.Assembly = att.InnerText;
                }
                att = config.Attributes["name"];
                if (att != null)
                {
                    info.DbName = att.InnerText;
                }
                att = config.Attributes["tier"];
                if (att != null)
                {
                    int tier = 1;
                    if (int.TryParse(att.InnerText,out tier))
                    {
                        info.Tier = Convert.ToInt32(att.InnerText);
                    }
                    else 
                    {
                        info.Tier = 3;
                    }
                }
                return info;
            }
            return null;
        }
    }
}
