using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.Xml;
using Buffalo.GeneratorInfo;
using Buffalo.WinFormsControl.Editors;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI��Ҫ����Ϣ
    /// </summary>
    public class UIModelItem
    {
        private ClrProperty _belongProperty;

        /// <summary>
        /// UI��Ϣ
        /// </summary>
        /// <param name="dicCheckItem">ѡ����</param>
        /// <param name="belongProperty">��������</param>
        public UIModelItem(ClrProperty belongProperty) 
        {
            _belongProperty = belongProperty;
        }
        /// <summary>
        /// UI��Ϣ
        /// </summary>
        public UIModelItem()
        {
        }
        private Dictionary<string, object> _dicCheckItem=new Dictionary<string,object>();

        /// <summary>
        /// ѡ�е���
        /// </summary>
        internal Dictionary<string, object> CheckItem 
        {
            get 
            {
                return _dicCheckItem;
            }
        }

        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret)) 
            {
                if(ret is bool)
                {
                    return (bool)ret;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                return ret.ToString();
            }
            return null;
        }
        private bool _isGenerate;
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsGenerate
        {
            get { return _isGenerate; }
            set { _isGenerate = value; }
        }

        /// <summary>
        /// д��XML�ڵ�
        /// </summary>
        /// <param name="node"></param>
        public void WriteNode(XmlNode node) 
        {
            XmlDocument doc=node.OwnerDocument;
            XmlAttribute att = doc.CreateAttribute("name");
            if (_belongProperty != null)
            {
                att.InnerText = PropertyName;
                node.Attributes.Append(att);
            }
            

            foreach (KeyValuePair<string, object> kvp in _dicCheckItem) 
            {
                XmlNode inode = doc.CreateElement("item");
                att = doc.CreateAttribute("name");
                att.InnerText = kvp.Key;
                inode.Attributes.Append(att);

                string value = null;
                if (kvp.Value is bool)
                {
                    value = (bool)kvp.Value ? "1" : "0";
                }
                else 
                {
                    value = kvp.Value as string;
                }
                att = doc.CreateAttribute("value");
                att.InnerText = value;
                inode.Attributes.Append(att);
                node.AppendChild(inode);
            }

        }

        /// <summary>
        /// ��ȡ�ڵ���Ϣ
        /// </summary>
        /// <param name="node">�ڵ�</param>
        public void ReadItem(XmlNode node,Dictionary<string, ConfigItem> dicConfig) 
        {
            string name=null;
            object value=null;
            ConfigItem item = null;
            foreach (XmlNode cnode in node.ChildNodes)
            {
                XmlAttribute att = cnode.Attributes["name"];
                if (att == null)
                {
                    continue;
                }
                name = att.InnerText;
                att = cnode.Attributes["value"];
                if (att == null)
                {
                    continue;
                }
                value = att.InnerText;
                if (dicConfig.TryGetValue(name, out item))
                {
                    if (item.Type == ConfigItemType.Check)
                    {
                        bool bvalue = false;
                        if (!bool.TryParse(att.InnerText, out bvalue))
                        {
                            bvalue = att.InnerText == "1";
                        }
                        value = bvalue;
                    }
                }
                _dicCheckItem[name] = value;
            }
        }

        /// <summary>
        /// ��Ӧ���ֶ�����
        /// </summary>
        public string FieldType
        {
            get
            {

                return _belongProperty.MemberTypeShortName;

            }
        }
        /// <summary>
        /// ע��
        /// </summary>
        public string Summary
        {
            get { return _belongProperty.DocSummary; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string TypeName
        {
            get { return _belongProperty.MemberTypeShortName; }
        }
        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName
        {
            get { return _belongProperty.Name; }
        }

        /// <summary>
        /// �����GeneratItem��
        /// </summary>
        /// <returns></returns>
        public GenerateItem ToGeneratItem() 
        {
            GenerateItem item = null;
            if (_belongProperty == null)
            {
                item = new GenerateItem(_dicCheckItem, "", "", "", "");
            }
            else
            {
                item = new GenerateItem(_dicCheckItem, FieldType, Summary, TypeName, PropertyName);
            }
            return item;
        }
    }
}
