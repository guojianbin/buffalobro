using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public class Compiler
    {
        private string _content;
        private int index = -1;
        Queue<ExpressionItem> _queitem = new Queue<ExpressionItem>();

        /// <summary>
        /// 代码树
        /// </summary>
        public Queue<ExpressionItem> ExpressionItems
        {
            get { return _queitem; }
        }

        ExpressionItem _currentItem = null;
        public Compiler(string content) 
        {
            _content = content;
            DoCompiler();
        }

        private void DoCompiler() 
        {
            _currentItem = new ExpressionItem();
            _currentItem.Type = ExpressionType.String;
            _queitem.Enqueue(_currentItem);

            while (MoveNext()) 
            {
                if (CurrentChar == '<')
                {
                    CompilerPart();
                }
                else 
                {
                    if (_currentItem == null) 
                    {
                        _currentItem = new ExpressionItem();
                        _currentItem.Type = ExpressionType.String;
                        _queitem.Enqueue(_currentItem);

                    }
                    _currentItem.Content.Append(CurrentChar);
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
                    _currentItem = new ExpressionItem();
                    _queitem.Enqueue(_currentItem);
                    
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
                        CompilerCode();
                        
                        _currentItem = null;
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
            while (MoveNext()) 
            {
                if (CurrentChar == '\"') 
                {
                    if (stkChr.Count > 0 && (stkChr.Peek() == '\"' || stkChr.Peek() == '\\'))
                    {
                        stkChr.Pop();
                    }
                    else
                    {
                        stkChr.Push('\"');
                    }
                    _currentItem.Content.Append(CurrentChar);
                }
                else if (CurrentChar == '\\') 
                {
                    if (stkChr.Count > 0 && (stkChr.Peek() == '\"'))//在字符串里边
                    {
                        stkChr.Push('\\');
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
                    else
                    {
                        _currentItem.Content.Append(CurrentChar);
                    }
                }
                else 
                {
                    _currentItem.Content.Append(CurrentChar);
                }
            }
        }

        /// <summary>
        /// 移动到下一个字符
        /// </summary>
        /// <returns></returns>
        private bool MoveNext() 
        {
            if (_content.Length <= index+1) 
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
