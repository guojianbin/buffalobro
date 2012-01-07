using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalobro.DB.DataBaseAdapter;
using System.Diagnostics;
namespace Buffalobro.DB
{
    /// <summary>
    /// ≈‰÷√º”‘ÿ∆˜
    /// </summary>
    public class ConfigXmlLoader
    {
        /// <summary>
        /// ªÒ»°≈‰÷√XML
        /// </summary>
        /// <param name="appRoot">≈‰÷√¬∑æ∂µƒº¸</param>
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
                    string path = Commons.CommonMethods.GetBaseRoot(config);
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
