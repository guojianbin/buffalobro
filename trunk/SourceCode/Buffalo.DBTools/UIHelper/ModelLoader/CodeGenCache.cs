using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// ģ����뻺��
    /// </summary>
    public class CodeGenCache
    {
        private static Dictionary<string, CodeGenInfo> _dicCodeCache = new Dictionary<string, CodeGenInfo>();

        /// <summary>
        /// �����ļ�·����ȡ������
        /// </summary>
        /// <param name="path">�ļ�·��</param>
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
        /// ����������
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
                throw new Exception("ģ��������:\n" + sbError.Length);
            }
            CodeGenInfo info = new CodeGenInfo(classType);
            
            _classCount++;
            return info;
        }
    }
}
