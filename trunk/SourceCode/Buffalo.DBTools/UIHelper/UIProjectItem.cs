using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI工程项
    /// </summary>
    public class UIProjectItem
    {
        private string _modelPath;
        /// <summary>
        /// 模版路径
        /// </summary>
        public string ModelPath
        {
            get { return _modelPath; }
        }

        private FileGenType _genType;
        /// <summary>
        /// 生成类型
        /// </summary>
        public FileGenType GenType
        {
            get { return _genType; }
        }

        private string _targetPath;
        /// <summary>
        /// 生成路径
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
        }

        private List<UIProjectItem> _childItems=new List<UIProjectItem>();

        /// <summary>
        /// 项目子项
        /// </summary>
        public List<UIProjectItem> ChildItems
        {
            get { return _childItems; }
        }
    }
}
