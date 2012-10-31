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
        /// 处理代码块
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
        /// 解释代码块
        /// </summary>
        private void CompilerCode() 
        {
            Stack<char> stkChr=new Stack<char>();
            char priChar = '\0';//上一个字符
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
                            if (CurrentChar == '>') //结束符号
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
            return true;
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
