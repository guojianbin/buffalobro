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
    /// 源码编译器
    /// </summary>
    public class SourceCodeCompiler
    {
        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        public static Type DoCompiler(string code,string className,List<string> dllReference, StringBuilder errorMessage)
        {

            using (CodeDomProvider cdp = CodeDomProvider.CreateProvider("C#"))
            {
                // 编译器的参数 
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
                // 编译结果 
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
