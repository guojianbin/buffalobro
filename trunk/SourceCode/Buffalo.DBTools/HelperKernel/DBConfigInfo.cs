using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DBTools.DocSummary;

namespace Buffalo.DBTools.HelperKernel
{
    public class DBConfigInfo
    {
        private string _dbName;

        /// <summary>
        /// ���ݿ���
        /// </summary>
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        private string _appNamespace;
        /// <summary>
        /// ���ݲ����ڵ������ռ�
        /// </summary>
        public string AppNamespace
        {
            get { return _appNamespace; }
            set { _appNamespace = value; }
        }

        private string _assembly;
        /// <summary>
        /// ����(һ������)
        /// </summary>
        public string Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }

        private string _dbType;
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        private string _connectionString;
        /// <summary>
        /// ���������ַ���
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        private string _fileName;

        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private int _tier;

        /// <summary>
        /// ����
        /// </summary>
        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        private SummaryShowItem _summaryShow;

        /// <summary>
        /// ��ʾ��ʽ
        /// </summary>
        public SummaryShowItem SummaryShow
        {
            get { return _summaryShow; }
            set { _summaryShow = value; }
        }

        private bool _isAllDal;

        /// <summary>
        /// �Ƿ������������ݲ�
        /// </summary>
        public bool IsAllDal
        {
            get { return _isAllDal; }
            set { _isAllDal = value; }
        }

        /// <summary>
        /// �������ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DBInfo CreateDBInfo() 
        {
            DBInfo info = new DBInfo(DbName, ConnectionString, DbType);
            return info;
        }



        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="curDiagram"></param>
        public static string GetFileName(ClassDesignerInfo info) 
        {
            string dbName = GetDbName(info);
            string proFile = info.CurrentProject.FileName;
            FileInfo file = new FileInfo(proFile);
            string directory = file.DirectoryName;
            return directory + "\\" + dbName + ".xml";

        }

        /// <summary>
        /// ��ȡ��ǰ��ͼ�Ŀ���
        /// </summary>
        /// <param name="docView"></param>
        /// <returns></returns>
        public static string GetDbName(ClassDesignerInfo info) 
        {
            FileInfo docFile = new FileInfo(info.SelectDocView.DocData.FileName);
            string dbName = docFile.Name;
            int dot = dbName.IndexOf('.');
            if (dot > 0)
            {
                dbName = dbName.Substring(0, dot);
            }
            return dbName;
        }

        /// <summary>
        /// ����
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

            att = doc.CreateAttribute("isAllDal");
            att.InnerText = this.IsAllDal?"1":"0";
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("summary");
            att.InnerText = ((int)this.SummaryShow).ToString();
            configNode.Attributes.Append(att);

            EntityMappingConfig.SaveXML(path, doc);
        }

        /// <summary>
        /// �������ݿ���Ϣ
        /// </summary>
        /// <param name="curProject">��ǰ����</param>
        /// <param name="curDiagram">��ǰͼ</param>
        /// <returns></returns>
        public static DBConfigInfo LoadInfo(ClassDesignerInfo info) 
        {
            if (info.CurrentProject == null || info.SelectDocView == null) 
            {
                return null;
            }
            string xmlFieName = GetFileName(info);
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
        /// �������ݿ�������Ϣ
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

                att = config.Attributes["isAllDal"];
                if (att != null)
                {
                    info.IsAllDal = (att.InnerText == "1");
                }
                att = config.Attributes["summary"];
                if (att != null)
                {
                    int summary = (int)SummaryShowItem.All;
                    if (int.TryParse(att.InnerText, out summary))
                    {
                        info.SummaryShow = (SummaryShowItem)summary;
                    }
                    else
                    {
                        info.SummaryShow = SummaryShowItem.All;
                    }
                }
                return info;
            }
            return null;
        }
    }
}
