using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��Ŀ��
    /// </summary>
    public class UIProject
    {
        private string _name;
        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _namespace;
        /// <summary>
        /// �����ռ�
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        private List<UIProjectItem> _lstItems=new List<UIProjectItem>();

        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public List<UIProjectItem> LstItems
        {
            get { return _lstItems; }
        }
    }
}
