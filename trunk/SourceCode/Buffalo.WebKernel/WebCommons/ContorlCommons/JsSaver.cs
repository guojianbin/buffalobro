using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using Buffalo.WebKernel;

namespace Buffalo.WebKernel.WebCommons.ContorlCommons
{
    /// <summary>
    /// �Զ�����JS����
    /// </summary>
    public class JsSaver
    {
        /// <summary>
        /// Ĭ�ϵ�JS����ļ���
        /// </summary>
        private const string defaultDirectory = "/script/";

        /// <summary>
        /// ��ȡJS�ļ�·��
        /// </summary>
        /// <param name="fileName">JS�ļ���</param>
        /// <returns></returns>
        public static string GetDefualtJsUrl(string fileName) 
        {
            return WebCommon.GetVirPath() + defaultDirectory+fileName;
        }

        /// <summary>
        /// ����JS�ļ��ķ���
        /// </summary>
        /// <param name="fileName">JS�ļ���</param>
        /// <param name="jsContent">JS����</param>
        public static void SaveJS(string fileName,string jsContent) 
        {
            string dir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + defaultDirectory);
            string root = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + defaultDirectory + fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(root))
            {
                StreamWriter writer = new StreamWriter(root, false, System.Text.Encoding.UTF8);
                writer.Write(jsContent);
                writer.Close();
            }
        }
    }
}
