using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper.ModelLoader
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
        public string GetCode(string className) 
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
                    AddBuffaloLink(str);
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
            return man.ToCode(className);
        }

        /// <summary>
        /// ��ӱ���Ŀ������
        /// </summary>
        /// <param name="dll"></param>
        private void AddBuffaloLink(List<string> dll) 
        {
            Assembly ass = null;

            //ass = typeof(CodeGeneration).Assembly;
            //string path = new Uri(ass.CodeBase).LocalPath;
            //FileInfo info = new FileInfo(path);
            //DirectoryInfo dic = info.Directory;
            //FileInfo[] files = dic.GetFiles();
            //foreach (FileInfo finfo in files) 
            //{
            //    string fpath = finfo.FullName;
            //    if (fpath.LastIndexOf(".dll") >= 0) 
            //    {
            //        dll.Add(fpath);
            //    }
            //}
            ass = typeof(Buffalo.GeneratorInfo.GenerateItem).Assembly;
            string path=new Uri(ass.CodeBase).LocalPath;
            dll.Add(path);
            FileInfo file=new FileInfo(path);
            string fileName=CommonMethods.GetBaseRoot(file.Name);
            if (!File.Exists(fileName)) 
            {
                File.Copy(path, fileName);
            }
            //ass = typeof(Buffalo.DB.QueryConditions.ScopeList).Assembly;
            //dll.Add(new Uri(ass.CodeBase).LocalPath);

            //ass = typeof(CodeGeneration).Assembly;
            //dll.Add(new Uri(ass.CodeBase).LocalPath);

            
        }


        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="errorMessage">��������Ϊ��Ϣ</param>
        /// <returns></returns>
        public CodeGenInfo GetCompileType(string className,StringBuilder codeCache, StringBuilder errorMessage)
        {
            string code = GetCode(className);
            codeCache.Append(code);
            string ret = null;
            CodeGenInfo info = SourceCodeCompiler.DoCompiler(code, CodesManger.CompilerNamespace + "." + className, man.Link, errorMessage);

            return info;
        }
    }
}
