using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.IO;
using Buffalo.DBTools.HelperKernel;
using System.Data;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.DBTools
{
    /// <summary>
    /// 代码文件管理类
    /// </summary>
    public class CodeFileHelper
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.Default;




        

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SaveFile(string fileName, List<string> content)
        {
            if(File.Exists( fileName))
            {
                string bakName=fileName + ".bak";
                if(File.Exists(bakName))
                {
                    File.Delete(bakName);
                }
                File.Move(fileName, bakName);
            }
            using (StreamWriter writer = new StreamWriter(fileName, false, DefaultEncoding))
            {
                foreach (string str in content)
                {
                    writer.WriteLine(str);
                }
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> ReadFile(string fileName) 
        {
            return new List<string>(File.ReadAllLines(fileName, DefaultEncoding));
        }

    }
}
