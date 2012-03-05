using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace AddInSetup
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
    public class AddInInfo
    {
        private string _fileName;
        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private string _version;

        /// <summary>
        /// �汾��
        /// </summary>
        public string Version
        {
            get { return _version; }
        }

        private string _name;

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private bool _isSetup;
        /// <summary>
        /// �Ƿ�װ
        /// </summary>
        public bool IsSetup
        {
            get { return _isSetup; }
        }

        /// <summary>
        /// ��ť�ı�
        /// </summary>
        public string ButtonText 
        {
            get 
            {
                if (_isSetup) 
                {
                    return "ж��";
                }
                return "��װ";
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        public const string AddInName = "Buffalo.DBTools.AddIn";

        /// <summary>
        /// ��·��
        /// </summary>
        private static readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        /// <summary>
        /// �ҵ��ĵ�·��
        /// </summary>
        private string _myDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).TrimEnd('\\');
        /// <summary>
        /// ��װ
        /// </summary>
        /// <returns></returns>
        public string Install() 
        {
            if (IsInstall()) 
            {
                return null;
            }
            
            string fileName = _basePath + "\\" + _fileName;
            if (!File.Exists(fileName)) 
            {
                return "�������ļ�:" + fileName;
            }
            string fileContent = AddInSetup.Properties.Resources.DBTools;

            fileContent=fileContent.Replace("<%=Version%>", Version);
            fileContent=fileContent.Replace("<%=FileName%>", fileName);

            string dic=GetAddInDirectory();
            if (!Directory.Exists(dic)) 
            {
                Directory.CreateDirectory(dic);
            }
            File.WriteAllText(GetAddInFileName(), fileContent, Encoding.BigEndianUnicode);
            _isSetup = true;
            return null;
        }



        /// <summary>
        /// ����װ
        /// </summary>
        /// <returns></returns>
        public string UnInstall() 
        {
            if (IsInstall()) 
            {
                File.Delete(GetAddInFileName());
            }
            _isSetup = false;
            return null;
        }
        /// <summary>
        /// ��ȡ���������·��
        /// </summary>
        /// <returns></returns>
        private string GetAddInFileName()
        {
            return GetAddInDirectory() + AddInName;
        }
        /// <summary>
        /// ��ȡ������ļ���·��
        /// </summary>
        /// <returns></returns>
        private string GetAddInDirectory() 
        {
            return _myDocumentPath + "\\" + _name + "\\AddIns\\";
        }

        /// <summary>
        /// �ж��Ƿ��Ѿ���װ
        /// </summary>
        /// <returns></returns>
        public bool IsInstall() 
        {
            string addInFile = GetAddInFileName();
            return File.Exists(addInFile);
        }

        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <returns></returns>
        public static List<AddInInfo> GetAddInInfo() 
        {
            string infoFile = _basePath + "\\AddInConfig.xml";
            if (!File.Exists(infoFile)) 
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(infoFile);
            XmlNodeList addinNodes = doc.GetElementsByTagName("AddIn");
            List<AddInInfo> lstAddIn = new List<AddInInfo>();
            foreach (XmlNode node in addinNodes)
            {
                AddInInfo info = new AddInInfo();
                XmlAttribute att = node.Attributes["file"];
                if (att != null) 
                {
                    info._fileName = att.InnerText;
                }
                att = node.Attributes["version"];
                if (att != null)
                {
                    info._version = att.InnerText;
                }
                att = node.Attributes["addin"];
                if (att != null)
                {
                    info._name = att.InnerText;
                }
                info._isSetup = info.IsInstall();
                lstAddIn.Add(info);
            }
            return lstAddIn;
        }
    }
}
