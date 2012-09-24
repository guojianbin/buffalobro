using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ModelCompiler
{
    /// <summary>
    /// ģ�浽�����ת����
    /// </summary>
    public class ModelCodeTran
    {
        private StringReader _content;

        
        /// <summary>
        /// ���ɵĴ���������
        /// </summary>
        private StringBuilder _sbContainer = new StringBuilder();
        /// <summary>
        /// ģ�浽�����ת����
        /// </summary>
        /// <param name="content">ģ������</param>
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
        /// ����Script��ǩ
        /// </summary>
        private void TranScript(string tag) 
        {
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>[^<]+)</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(content);
        }
        
    }
}
