using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI������
    /// </summary>
    public class UIProjectItem
    {
        private string _modelPath;
        /// <summary>
        /// ģ��·��
        /// </summary>
        public string ModelPath
        {
            get { return _modelPath; }
        }

        private FileGenType _genType;
        /// <summary>
        /// ��������
        /// </summary>
        public FileGenType GenType
        {
            get { return _genType; }
        }

        private string _targetPath;
        /// <summary>
        /// ����·��
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
        }

        private List<UIProjectItem> _childItems=new List<UIProjectItem>();

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public List<UIProjectItem> ChildItems
        {
            get { return _childItems; }
        }
    }
}
