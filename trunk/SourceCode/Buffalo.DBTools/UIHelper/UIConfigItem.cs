using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI������Ϣ
    /// </summary>
    public class UIConfigItem
    {
        private List<ConfigItem> _configItems;
        /// <summary>
        /// ��ѡ��
        /// </summary>
        public List<ConfigItem> ConfigItems
        {
            get { return _configItems; }
        }

        private List<UIProjectItem> _projects;
        /// <summary>
        /// ��������
        /// </summary>
        public List<UIProjectItem> Projects
        {
            get { return _projects; }
        }

        /// <summary>
        /// UI������Ϣ
        /// </summary>
        /// <param name="config">���õ�XML</param>
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
        /// ���������
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
