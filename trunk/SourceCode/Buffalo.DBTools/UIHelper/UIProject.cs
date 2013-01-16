using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 项目项
    /// </summary>
    public class UIProject
    {
        private string _name;
        /// <summary>
        /// 项目名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _namespace;
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        private List<UIProjectItem> _lstItems=new List<UIProjectItem>();

        /// <summary>
        /// 项目项
        /// </summary>
        public List<UIProjectItem> LstItems
        {
            get { return _lstItems; }
        }
    }
}
