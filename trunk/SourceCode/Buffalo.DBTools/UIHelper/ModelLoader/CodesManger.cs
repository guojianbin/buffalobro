using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public class CodesManger
    {

        private List<string> _sbLink = new List<string>();
        /// <summary>
        /// �������
        /// </summary>
        public List<string> Link
        {
            get { return _sbLink; }
        }

        private StringBuilder _sbUsing = new StringBuilder();
        /// <summary>
        /// ��Ҫusing����
        /// </summary>
        public StringBuilder Using
        {
            get { return _sbUsing; }
        }

        private StringBuilder _sbCode = new StringBuilder();
        /// <summary>
        /// ���ɵ����ģ��
        /// </summary>
        public StringBuilder Code
        {
            get { return _sbCode; }
        }
        private StringBuilder _sbMethod = new StringBuilder();
        /// <summary>
        /// ������
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
        /// ��ȡUsing���ִ���
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
        /// ��ȡ���ɵĴ���
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
        ///// ��ȡ���ɵĴ���
        ///// </summary>
        //private void GetGreanMain(StringBuilder sbCode)
        //{
        //    sbCode.AppendLine("public static void Main()");
        //    sbCode.AppendLine("{");
        //    sbCode.AppendLine("}");
        //}
    }
}
