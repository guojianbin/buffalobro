using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DBTools.HelperKernel;
using System.IO;

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

        private List<UIProject> _projects;
        /// <summary>
        /// 所属工程
        /// </summary>
        public List<UIProject> Projects
        {
            get { return _projects; }
        }
        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }
        /// <summary>
        /// UI配置信息
        /// </summary>
        /// <param name="config">配置的XML</param>
        public UIConfigItem(XmlDocument config,ClassDesignerInfo desinfo) 
        {
            _designerInfo = desinfo;
            InitConfig(config);
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
                if (node.Name.Equals("models", StringComparison.CurrentCultureIgnoreCase))
                {
                    FillProjectItems(node);
                }
            }
        }

        /// <summary>
        /// 填充项目项
        /// </summary>
        /// <param name="node"></param>
        private void FillProjectItems(XmlNode node) 
        {
            _projects = new List<UIProject>();
            foreach (XmlNode projectNode in node.ChildNodes) 
            {
                if (!projectNode.Name.Equals("project", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    continue;
                }
                UIProject objProject = new UIProject();
                XmlAttribute att = projectNode.Attributes["name"];
                if (att != null) 
                {
                    objProject.Name = att.InnerText;
                }
                att = projectNode.Attributes["namespace"];
                if (att != null)
                {
                    objProject.Namespace = att.InnerText;
                }

            }
        }

        /// <summary>
        /// 填充项目项
        /// </summary>
        /// <param name="node"></param>
        private void FillProjectItem(XmlNode node,List<UIProjectItem> items) 
        {
            foreach (XmlNode projectNode in node.ChildNodes)
            {
                if (!projectNode.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                UIProjectItem item = new UIProjectItem();
                XmlAttribute att = projectNode.Attributes["path"];
                if (att != null)
                {
                    item.ModelPath = att.InnerText;
                }
                att = projectNode.Attributes["type"];
                if (att == null)
                {
                    item.ModelPath = att.InnerText;
                }
            }
        }


        /// <summary>
        /// 格式化参数名字
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string FormatParameter(string parameter, ClassDesignerInfo classInfo, 
            EntityConfig entityInfo)
        {
            string projectName = classInfo.CurrentProject.Name;
            FileInfo projectFile = new FileInfo(classInfo.CurrentProject.FileName);
            string projectPath = projectFile.Directory.Name;

            string ret = parameter;
            ret = ret.Replace("{ProjectName}", projectName);
            ret = ret.Replace("{ProjectPath}", projectPath);
            ret = ret.Replace("{ClassName}", entityInfo.ClassName);
            return ret;
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
