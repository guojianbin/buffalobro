using System;
using System.Collections.Generic;
using System.Text;

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
            get { return _name; }
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
    }
}
