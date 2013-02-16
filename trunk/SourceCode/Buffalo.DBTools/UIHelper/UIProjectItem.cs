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
            set { _modelPath = value; }
        }

        private BuildAction _genType;
        /// <summary>
        /// 生成类型
        /// </summary>
        public BuildAction GenType
        {
            get { return _genType; }
            set { _genType = value; }
        }

        private string _targetPath;
        /// <summary>
        /// 生成路径
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
            set { _targetPath = value; }
        }

        /// <summary>
        /// 生成的代码缓存
        /// </summary>
        private Type _genCodeCache;

        

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
