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
        /// �ƶ�����һ���ַ�
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
        /// ��ǰ�ַ�
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
