using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;
using System.ComponentModel;



namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��ѡ��
    /// </summary>
    public class ConfigItem
    {
        private string _name;
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name;}
            set { _name = value; }
        }

        private string _summary;
        /// <summary>
        /// ˵��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private List<ComboBoxItem> _lstItems = null;

        public List<ComboBoxItem> Items
        {
            get { return _lstItems; }
            set { _lstItems = value; }
        }
        private ConfigItemType _type;
        /// <summary>
        /// ������
        /// </summary>
        public ConfigItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public enum ConfigItemType 
    {
        /// <summary>
        /// ѡ���
        /// </summary>
        [Description("ѡ���")]
        [DisplayName("check")]
        Check,
        /// <summary>
        /// ������
        /// </summary>
        [Description("������")]
        [DisplayName("combo")]
        Combo,
        /// <summary>
        /// �ı���
        /// </summary>
        [Description("�ı���")]
        [DisplayName("text")]
        Text
    }
}
