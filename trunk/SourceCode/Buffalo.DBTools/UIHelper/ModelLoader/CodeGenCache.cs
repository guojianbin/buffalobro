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
        public static CodeGenInfo GetGenerationer(string path, EntityInfo entityInfo) 
        {
            CodeGenInfo ret = null;
            if(!_dicCodeCache.TryGetValue(path,out ret))
            {
                ret = CreateGenerationer(path,entityInfo);
                _dicCodeCache[path] = ret;
            }
            return ret;
        }

        /// <summary>
        /// ������л���
        /// </summary>
        public static void Clear() 
        {
            _dicCodeCache.Clear();
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="path">�����Ӧ·��</param>
        public static void DeleteGenerationer(string path) 
        {
            _dicCodeCache.Remove(path);
        }

        private static int _classCount = 0;
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static CodeGenInfo CreateGenerationer(string path, EntityInfo entityInfo) 
        {
            FileInfo file = new FileInfo(path);
            string workspace = file.DirectoryName;
            string content=File.ReadAllText(path,Encoding.Default);
            ModelCompiler compiler = new ModelCompiler(content, entityInfo);
            string className = "ModelCompilerClass" + _classCount;
            StringBuilder sbError=new StringBuilder();
            StringBuilder lastCodeCache = new StringBuilder();
            CodeGenInfo info = compiler.GetCompileType(className, lastCodeCache, sbError);
            if (sbError.Length > 0) 
            {
                CompileException ex=new CompileException("ģ��������:\n" + sbError);
                ex.Code = lastCodeCache.ToString();
                throw ex;
            }
            
            
            _classCount++;
            return info;
        }
    }
}
