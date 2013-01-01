using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;
using System.ComponentModel;



namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 可选项
    /// </summary>
    public class ConfigItem
    {
        private string _name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return _name;}
            set { _name = value; }
        }

        private string _summary;
        /// <summary>
        /// 说明
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
        /// 项类型
        /// </summary>
        public ConfigItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }

    /// <summary>
    /// 项类型
    /// </summary>
    public enum ConfigItemType 
    {
        /// <summary>
        /// 选择框
        /// </summary>
        [Description("选择框")]
        [DisplayName("check")]
        Check,
        /// <summary>
        /// 下拉框
        /// </summary>
        [Description("下拉框")]
        [DisplayName("combo")]
        Combo,
        /// <summary>
        /// 文本框
        /// </summary>
        [Description("文本框")]
        [DisplayName("text")]
        Text
    }
}
