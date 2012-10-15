using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ModelCompiler
{
    public class Compiler
    {
        private string _content;
        private int index = -1;
        Queue<ExpressionItem> queitem = new Queue<ExpressionItem>();


        public Compiler(string content) 
        {
            
        }

        public string GetContent() 
        {
            
        }

        /// <summary>
        /// 移动到下一个字符
        /// </summary>
        /// <returns></returns>
        private bool MoveNext() 
        {
            if (_content.Length <= index - 1) 
            {
                return false;
            }
            index++;
        }

        /// <summary>
        /// 当前字符
        /// </summary>
        private char CurrentChar 
        {
            get 
            {
                return _content[index];
            }
        }
    }
}
