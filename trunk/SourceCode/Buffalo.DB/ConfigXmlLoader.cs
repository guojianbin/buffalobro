using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using System.Diagnostics;
namespace Buffalo.DB
{
    /// <summary>
    /// ���ü�����
    /// </summary>
    public class ConfigXmlLoader
    {
        /// <summary>
        /// ��ȡ����XML
        /// </summary>
        /// <param name="appRoot">����·���ļ�</param>
        /// <returns></returns>
        public static List<ConfigInfo> LoadXml(string appRoot)
        {
            string configRoot = System.Configuration.ConfigurationManager.AppSettings[appRoot];
            List<ConfigInfo> docs = new List<ConfigInfo>();
            if (!string.IsNullOrEmpty(configRoot))
            {
                string[] configs = configRoot.Split('|');
                foreach (string config in configs)
                {
                    string path = Buffalo.Kernel.CommonMethods.GetBaseRoot(config);
                    if (!File.Exists(path))
                    {
                        continue;
                    }
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(path);

                        ConfigInfo info = new ConfigInfo(path, doc);
                        docs.Add(info);
                    }
                    catch(Exception ex)
                    {
#if DEBUG
                        Debug.Write(ex.ToString());
#endif
                    }

                }
            }
            return docs;
        }
    }
}
