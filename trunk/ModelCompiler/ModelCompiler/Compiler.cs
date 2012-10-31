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

        ExpressionItem _currentItem = null;
        public Compiler(string content) 
        {
            
        }

        public string GetContent() 
        {
            while (MoveNext()) 
            {
                if (CurrentChar == '<') 
                {
                    CompilerPart();
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void CompilerPart() 
        {
            if (MoveNext()) 
            {
                if (CurrentChar == '?')
                {

                    queitem.Enqueue(_currentItem);
                    _currentItem = new ExpressionItem();
                    if (MoveNext()) 
                    {
                        if (CurrentChar == '=')
                        {
                            _currentItem.Type = ExpressionType.Express;
                        }
                        else 
                        {
                            _currentItem.Type = ExpressionType.Code;
                            _currentItem.Content.Append(CurrentChar);
                        }
                    }

                }
                else 
                {
                    _currentItem.Content.Append("<" + CurrentChar);
                }
            }
        }
        /// <summary>
        /// ���ʹ����
        /// </summary>
        private void CompilerCode() 
        {
            Stack<char> stkChr=new Stack<char>();
            char priChar = '\0';//��һ���ַ�
            while (MoveNext()) 
            {
                if (CurrentChar == '\"') 
                {
                    if (stkChr.Count > 0 && stkChr.Peek() == '\"')
                    {
                        stkChr.Pop();
                    }
                    else
                    {
                        stkChr.Push('\"');
                    }
                    _currentItem.Content.Append(CurrentChar);
                }
                else if (CurrentChar == '?')
                {
                    if (stkChr.Count <= 0)
                    {
                        if (MoveNext())
                        {
                            if (CurrentChar == '>') //��������
                            {
                                return;
                            }
                            else
                            {
                                _currentItem.Content.Append("?" + CurrentChar);
                            }
                        }
                    }
                }
                
            }
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
            return true;
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
