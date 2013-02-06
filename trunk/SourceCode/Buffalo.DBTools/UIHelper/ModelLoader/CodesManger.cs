using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public class CodesManger
    {

        private List<string> _sbLink = new List<string>();
        /// <summary>
        /// 关联类库
        /// </summary>
        public List<string> Link
        {
            get { return _sbLink; }
        }

        private StringBuilder _sbUsing = new StringBuilder();
        /// <summary>
        /// 需要using的类
        /// </summary>
        public StringBuilder Using
        {
            get { return _sbUsing; }
        }

        private StringBuilder _sbCode = new StringBuilder();
        /// <summary>
        /// 生成的输出模版
        /// </summary>
        public StringBuilder Code
        {
            get { return _sbCode; }
        }
        private StringBuilder _sbMethod = new StringBuilder();
        /// <summary>
        /// 函数块
        /// </summary>
        public StringBuilder Method
        {
            get { return _sbMethod; }
        }

        public const string CompilerNamespace = "Buffalo.DBTools.UIHelper.ModelLoaderItems";

        public string ToCode(string className) 
        {
            StringBuilder sbCode = new StringBuilder(65535);
            //using
            GetUsingCode(sbCode);


            sbCode.AppendLine("namespace " + CompilerNamespace);
            sbCode.AppendLine("{");
            sbCode.AppendLine("public class "+className);
            sbCode.AppendLine("     {");
            //GetGreanMain(sbCode);
            GetGreanCode(sbCode);

            sbCode.AppendLine(Method.ToString());

            sbCode.AppendLine("     }");
            sbCode.AppendLine("}");
            return sbCode.ToString();
        }

        /// <summary>
        /// 获取Using部分代码
        /// </summary>
        /// <param name="sbCode"></param>
        private void GetUsingCode(StringBuilder sbCode) 
        {
            sbCode.AppendLine("using System;");
            sbCode.AppendLine("using System.Collections.Generic;");
            sbCode.AppendLine("using System.Text;");
            sbCode.AppendLine(Using.ToString());
        }

        /// <summary>
        /// 获取生成的代码
        /// </summary>
        private void GetGreanCode(StringBuilder sbCode) 
        {
            sbCode.AppendLine("public string DoCompiler(EntityInfo Entity, UIConfigItem ClassConfig,List<UIModelItem> SelectedPropertys)");
            sbCode.AppendLine("{");
            sbCode.AppendLine(" StringBuilder SystemOut = new StringBuilder(65535);");
            sbCode.AppendLine(Code.ToString());
            sbCode.AppendLine("return SystemOut.ToString();");
            sbCode.AppendLine("}");
        }

        ///// <summary>
        ///// 获取生成的代码
        ///// </summary>
        //private void GetGreanMain(StringBuilder sbCode)
        //{
        //    sbCode.AppendLine("public static void Main()");
        //    sbCode.AppendLine("{");
        //    sbCode.AppendLine("}");
        //}
    }
}
