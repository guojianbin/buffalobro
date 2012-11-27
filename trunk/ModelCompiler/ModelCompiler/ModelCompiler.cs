using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ModelCompiler
{
    /// <summary>
    /// ģ�浽�����ת����
    /// </summary>
    public class ModelCompiler
    {

        private CodesManger man = new CodesManger();
        private string _content;
        private string _workspace;
        /// <summary>
        /// ģ�浽�����ת����
        /// </summary>
        /// <param name="content">ģ������</param>
        public ModelCompiler(string content,string workspace) 
        {
            _content = content;
            _workspace = workspace;
        }


        /// <summary>
        /// ����Script��ǩ
        /// </summary>
        private string TranScript() 
        {
            //string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>[^<]+)</[#]script>";
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>(.*?))</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(_content);
            foreach (Match ma in matches)
            {
                if (ma.Groups["type"] == null) 
                {
                    continue;
                }
                string type = ma.Groups["type"].Value;
                string content=ma.Groups["content"].Value;
                CodeGeneration com=new CodeGeneration(content);
                Queue<ExpressionItem> queitem=com.ExpressionItems;
                if (type.Equals("linked",StringComparison.CurrentCultureIgnoreCase)) 
                {
                    LinkOutputer outputer = new LinkOutputer();
                    List<string> str = outputer.GetCode(queitem,_workspace);
                    man.Link.AddRange(str);
                }
                else if (type.Equals("using", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    UsingOutputer outputer = new UsingOutputer();
                    string str = outputer.GetCode(queitem);
                    man.Using.Append(str);
                }
                else if (type.Equals("code", StringComparison.CurrentCultureIgnoreCase))
                {
                    CodeOutputer outputer = new CodeOutputer();
                    string str = outputer.GetCode(queitem);
                    man.Code.Append(str);
                }
                else if (type.Equals("method", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodOutputer outputer = new MethodOutputer();
                    string str = outputer.GetCode(queitem);
                    man.Method.Append(str);
                }
            }
            return man.ToCode();
        }

        /// <summary>
        /// ��ȡ�������к�Ĵ���
        /// </summary>
        /// <param name="errorMessage">��������Ϊ��Ϣ</param>
        /// <returns></returns>
        public string GetContent(StringBuilder errorMessage) 
        {
            string code = TranScript();
            string ret = null;
            Type objType = SourceCodeCompiler.DoCompiler(code, "ModelCompilerItems.CompilerClass", man.Link, errorMessage);
            if (objType != null) 
            {
                object comObject=Activator.CreateInstance(objType);
                MethodInfo mi = objType.GetMethod("DoCompiler");
                if (mi != null) 
                {
                    ret = mi.Invoke(comObject, new object[] {"NewClass1" }) as string;
                }
            }
            return ret;
        }
    }
}
