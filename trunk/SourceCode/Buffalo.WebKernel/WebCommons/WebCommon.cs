using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using Commons;
using System.Web.UI;

using System.Collections;
using Buffalo.Kernel.FastReflection;
using System.Web.UI.WebControls;

namespace Buffalo.WebKernel.WebCommons
{
    public class WebCommon
    {
        
        /// <summary>
        /// ��ԭ��ת����HTML����
        /// </summary>
        /// <param name="str">ԭ��</param>
        /// <returns></returns>
        public static string HTMLEncode(object objStr)
        {
            if (objStr == null)
            {
                return "";
            }
            string str = objStr.ToString();
            //&����
            str = str.Replace("&", "&amp;");
            // ����ո�
            str = str.Replace(" ", "&nbsp;");
            //����˫����
            str = str.Replace("\"", "&quot;");
            //html��Ƿ�
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\'", "&#39;");
            str = str.Replace("\"", "&quot;");
            //����
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\n", "<br/>");
            return str;
        }

        /// <summary>
        /// ��ȡ�ͻ��˵�IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;

        }
        /// <summary>
        /// ��HTML����ת����ԭ��
        /// </summary>
        /// <param name="strHTML">html����</param>
        /// <returns></returns>
        public static string HTMLDecode(string str)
        {
            if (str == null)
            {
                return "";
            }
            //html��Ƿ�
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            // ����ո�
            str = str.Replace(" ", "&nbsp;");
            //����˫����
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&#39;", "\'");
            str = str.Replace("&quot;", "\"");
            //&����
            str = str.Replace("&amp;", "&");
            return str;
        }
        /// <summary>
        /// ��ʽ�����/�����ʾ�ַ���
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public static string ForamtStringCount(object str, int count) 
        {
            string strRet = ToBreakWord(str, count);
            strRet = HTMLEncode(strRet);
            return strRet;
        }
        /// <summary>
        /// ���ַ����Զ��ӿո����ñ���Զ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ToBreakWord(object str, int count) 
        {
            if (str == null) 
            {
                return "";
            }
            if(count<=0)
            {
                return str.ToString();
            }
            int linemax = count - 1;
            string strSource = str.ToString();
            StringBuilder sbRet = new StringBuilder();
            for (int i=0;i<strSource.Length;i++) 
            {
                char chr = strSource[i];
                sbRet.Append(chr);
                if ((i > 0) && (i % linemax) == 0) 
                {
                    sbRet.Append(' ');
                }
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// ��ȡϵͳ������Ŀ¼
        /// </summary>
        /// <returns></returns>
        public static string GetVirPath()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            if (appPath == "/" || appPath == "//")
            {
                appPath = "";
            }
            
            return appPath;
        }

        /// <summary>
        /// ��ǰҳ����
        /// </summary>
        public static string CurrentPageName 
        {
            get 
            {
                string pageName = HttpContext.Current.Request.Url.LocalPath;
                pageName = pageName.Substring(pageName.LastIndexOf("/", pageName.Length) + 1);
                return pageName;
            }
        }

        /// <summary>
        /// ��ȡϵͳ������+����Ŀ¼
        /// </summary>
        /// <returns></returns>
        public static string GetVirRoot()
        {
            string port = "";
            if (HttpContext.Current.Request.Url.Port != 80)
            {
                port = ":"+HttpContext.Current.Request.Url.Port.ToString();
            }

            string url = "http://" + HttpContext.Current.Request.Url.Host + port + GetVirPath();
            return url;
        }

        /// <summary>
        /// �Ƿ��������չ���ϴ�
        /// </summary>
        /// <param name="hifile">�ϴ��ؼ�</param>
        /// <returns></returns>
        public static bool IsAllowedExtension(HttpPostedFile hifile)
        {
            if (hifile.FileName == "" || hifile.FileName == null)
            {
                return false;
            }
            string strOldFilePath = "", strExtension = "";

            //�����ϴ�����չ�������Ըĳɴ������ļ��ж���
            string[] arrExtension = { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };

            if (hifile.FileName != string.Empty)
            {
                strOldFilePath = hifile.FileName;
                //ȡ���ϴ��ļ�����չ��
                strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));
                //�жϸ���չ���Ƿ�Ϸ�
                for (int i = 0; i < arrExtension.Length; i++)
                {
                    if (strExtension.ToLower() == arrExtension[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// �ж��ϴ��ļ���С�Ƿ񳬹����ֵ
        /// </summary>
        /// <param name="hifile">HtmlInputFile�ؼ�</param>
        /// <returns>�������ֵ����false,���򷵻�true.</returns>
        public static bool IsAllowedLength(HttpPostedFile hifile)
        {

            //�����ϴ��ļ���С�����ֵ,���Ա�����xml�ļ���,��λΪKB
            int i = 2 * 1024;
            //����ϴ��ļ��Ĵ�С�������ֵ,����flase,���򷵻�true.
            if (hifile.ContentLength > i * 1024)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// ���ļ�д���ش�����
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void ShowFile(byte[] file, string fileName)
        {
            ShowFile("application/octet-stream", file, fileName);
        }

        /// <summary>
        /// ���ļ�д���ش�����
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void ShowFile(string contentType,byte[] file,string fileName) 
        {
            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(fileName));

            HttpContext.Current.Response.OutputStream.Write(file, 0, file.Length);
        }
        
        /// <summary>
        /// ��ʽ��JS���ַ���
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static string JSStringEncode(string js)
        {
            js = js.Replace("\"", "\\\"");
            js = js.Replace("\r", "\\r");
            js = js.Replace("\n", "\\n");
            return js;
        }
    }
}
