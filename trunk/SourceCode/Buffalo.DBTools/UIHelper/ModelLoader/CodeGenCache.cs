using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 模版编译缓存
    /// </summary>
    public class CodeGenCache
    {
        private static Dictionary<string, CodeGenInfo> _dicCodeCache = new Dictionary<string, CodeGenInfo>();

        /// <summary>
        /// 根据文件路径获取生成器
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static CodeGenInfo GetGenerationer(string path) 
        {
            CodeGenInfo ret = null;
            if(!_dicCodeCache.TryGetValue(path,out ret))
            {
                ret = CreateGenerationer(path);
                _dicCodeCache[path] = ret;
            }
            return ret;
        }
        private static int _classCount = 0;
        /// <summary>
        /// 生成生成器
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static CodeGenInfo CreateGenerationer(string path) 
        {
            FileInfo file = new FileInfo(path);
            string workspace = file.DirectoryName;
            string content=File.ReadAllText(path,Encoding.Default);
            ModelCompiler compiler = new ModelCompiler(content, workspace);
            string className = "ModelCompilerClass" + _classCount;
            StringBuilder sbError=new StringBuilder();
            Type classType=compiler.GetCompileType(className, sbError);
            if (sbError.Length > 0) 
            {
                throw new Exception("模版编译错误:\n" + sbError.Length);
            }
            CodeGenInfo info = new CodeGenInfo(classType);
            
            _classCount++;
            return info;
        }
    }
}
