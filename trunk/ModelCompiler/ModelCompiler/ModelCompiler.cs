using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ModelCompiler
{
    /// <summary>
    /// ģ�浽�����ת����
    /// </summary>
    public class ModelCompiler
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
        public ModelCompiler(string content) 
        {
            _content = new StringReader(content);
           
        }

        private void Tran() 
        {
            string tmp = null;
            while ((tmp = _content.ReadLine()) != null) 
            {
                if (tmp.IndexOf("<#script") >= 0) 
                {
                    TranScript(tmp);
                }
            }
        }

        /// <summary>
        /// ����Script��ǩ
        /// </summary>
        private void TranScript(string tag) 
        {
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>[^<]+)</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(tag);
            foreach (Match ma in matches)
            {
                if (ma.Groups["type"] == null) 
                {
                    continue;
                }
                string type = ma.Groups["type"].Value;
                if (type.Equals("linked",StringComparison.CurrentCultureIgnoreCase)) 
                {
                    
                    
                }
            }
        }
        
    }
}
