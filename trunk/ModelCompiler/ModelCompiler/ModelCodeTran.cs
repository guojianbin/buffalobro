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
        private StringReader _content;

        
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
            _content = new StringReader(content);
           
        }

        private void Tran() 
        {
            string tmp = null;
            while ((tmp = _content.ReadLine()) != null) 
            {
                if (tmp.IndexOf("<?script") >= 0) 
                {
                    
                }
            }
        }

        /// <summary>
        /// 处理Script标签
        /// </summary>
        private void TranScript(string tag) 
        {
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>[^<]+)</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(content);
        }
        
    }
}
