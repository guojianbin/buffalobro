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
        private string _content;

        private int _currentIndex;
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
            _content = content;
            _currentIndex = 0;
        }

        public bool MoveNext() 
        {

        }
    }
}
