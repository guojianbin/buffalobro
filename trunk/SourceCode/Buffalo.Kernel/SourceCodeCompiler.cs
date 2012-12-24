using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using Buffalo.Kernel.FastReflection.ClassInfos;
using System.IO;

namespace Buffalo.Kernel
{
    /// <summary>
    /// Դ�������
    /// </summary>
    public class SourceCodeCompiler
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="code">����</param>
        /// <param name="errorMessage">������Ϣ</param>
        /// <returns></returns>
        public static Type DoCompiler(string code,string className,List<string> dllReference, StringBuilder errorMessage)
        {

            using (CodeDomProvider cdp = CodeDomProvider.CreateProvider("C#"))
            {
                // �������Ĳ��� 
                CompilerParameters cp = new CompilerParameters();
                cp.ReferencedAssemblies.Add("System.dll");
                
                foreach (string link in dllReference)
                {


                    if (!string.IsNullOrEmpty(link) )
                    {
                        string elink = link.Trim('\r', '\n', ' ');
                        if (!string.IsNullOrEmpty(elink))
                        {
                            cp.ReferencedAssemblies.Add(elink);
                        }
                    }
                }

                cp.GenerateExecutable = false;
                cp.GenerateInMemory = true;
                // ������ 
                CompilerResults cr = cdp.CompileAssemblyFromSource(cp, code);
                
                if (cr.Errors.HasErrors)
                {
                    foreach (CompilerError cerror in cr.Errors)
                    {
                        
                        errorMessage.AppendLine(cerror.ToString());
                    }
                    return null;
                }
                else
                {
                    Assembly ass = cr.CompiledAssembly;
                    Type objType=ass.GetType(className);
                    return objType;
                }
            }
        }
    }
}
