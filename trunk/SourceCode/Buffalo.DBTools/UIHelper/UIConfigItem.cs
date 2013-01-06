using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DBTools.HelperKernel;

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
                item.Items = GetValueItems(cnode);
                item.Type = GetItemType(cnode);
                _configItems.Add(item);
            }
        }
        /// <summary>
        /// 获取项类型
        /// </summary>
        /// <returns></returns>
        private ConfigItemType GetItemType(XmlNode cnode) 
        {
            XmlAttribute att = cnode.Attributes["type"];
            if (att == null)
            {
                return ConfigItemType.Text;
            }
            string type = att.InnerText;
            if (type.Equals("check",StringComparison.CurrentCultureIgnoreCase)) 
            {
                return ConfigItemType.Check;
            }
            if (type.Equals("combo", StringComparison.CurrentCultureIgnoreCase))
            {
                return ConfigItemType.Combo;
            }
            if (type.Equals("text", StringComparison.CurrentCultureIgnoreCase))
            {
                return ConfigItemType.Text;
            }
            return ConfigItemType.Text;
        }

        /// <summary>
        /// 获取值类型项
        /// </summary>
        /// <param name="cnode"></param>
        /// <returns></returns>
        private List<ComboBoxItem> GetValueItems(XmlNode cnode) 
        {
            XmlAttribute att = cnode.Attributes["select"];
            if (att == null) 
            {
                return null;
            }
            string selItems = att.InnerText;
            string[] items = selItems.Split(',');
            List<ComboBoxItem> ret = new List<ComboBoxItem>(items.Length);
            foreach (string strItem in items) 
            {
                string[] sitems = strItem.Split(':');
                ComboBoxItem cmbItem = new ComboBoxItem("");
                if (sitems.Length >= 1)
                {
                    cmbItem.Text = sitems[0].Trim();
                    ret.Add(cmbItem);
                }
                else 
                {
                    continue;
                }
                if (sitems.Length >= 2)
                {
                    cmbItem.Value = sitems[1].Trim();
                }
                else 
                {
                    cmbItem.Value = sitems[0].Trim();
                }
            }

            if (ret.Count <= 0) 
            {
                return null;
            }
            return ret;
        }

    }
}
