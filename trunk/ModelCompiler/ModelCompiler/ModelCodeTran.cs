using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ModelCompiler
{
    /// <summary>
    /// 模版到代码的转换器
    /// </summary>
    public class ModelCodeTran
    {
        private string _content;

        private int _currentIndex;
        /// <summary>
        /// 生成的代码存放容器
        /// </summary>
        private StringBuilder _sbContainer = new StringBuilder();
        /// <summary>
        /// 模版到代码的转换器
        /// </summary>
        /// <param name="content">模版内容</param>
        public ModelCodeTran(string content) 
        {
            _content = content;
            _currentIndex = 0;
        }

        public bool MoveNext() 
        {

        }
    }
}
