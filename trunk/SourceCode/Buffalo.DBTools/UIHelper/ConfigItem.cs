using System;
using System.Collections.Generic;
using System.Text;

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
            get { return _name; }
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
    }
}
