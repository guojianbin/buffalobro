using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI配置信息
    /// </summary>
    public class UIConfigItem
    {
        private List<ConfigItem> _configItems;
        /// <summary>
        /// 可选项
        /// </summary>
        public List<ConfigItem> ConfigItems
        {
            get { return _configItems; }
        }

        private List<UIProjectItem> _projects;
        /// <summary>
        /// 所属工程
        /// </summary>
        public List<UIProjectItem> Projects
        {
            get { return _projects; }
        }

        /// <summary>
        /// UI配置信息
        /// </summary>
        /// <param name="config">配置的XML</param>
        public UIConfigItem(XmlDocument config) 
        {

        }

        private void InitConfig(XmlDocument config) 
        {
            XmlNodeList roots=config.GetElementsByTagName("root");
            if (roots.Count <= 0) 
            {
                return;
            }
            XmlNode root = roots[0];
            foreach (XmlNode node in root.ChildNodes) 
            {
                if (node.Name.Equals("configItems", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    FillConfigItems(node);
                }
            }
        }

        /// <summary>
        /// 填充配置项
        /// </summary>
        /// <param name="node"></param>
        private void FillConfigItems(XmlNode node) 
        {
            _configItems = new List<ConfigItem>(node.ChildNodes.Count);

            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (!cnode.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    continue;
                }
                ConfigItem item = new ConfigItem();
                item.Name = cnode.Attributes["name"] != null ? cnode.Attributes["name"].InnerText : "";
                item.Summary = cnode.Attributes["summary"] != null ? cnode.Attributes["summary"].InnerText : "";
                _configItems.Add(item);
            }
        }
    }
}
