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
        /// 把原文转换成HTML文字
        /// </summary>
        /// <param name="str">原文</param>
        /// <returns></returns>
        public static string HTMLEncode(object objStr)
        {
            if (objStr == null)
            {
                return "";
            }
            string str = objStr.ToString();
            //&符号
            str = str.Replace("&", "&amp;");
            // 处理空格
            str = str.Replace(" ", "&nbsp;");
            //处理双引号
            str = str.Replace("\"", "&quot;");
            //html标记符
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\'", "&#39;");
            str = str.Replace("\"", "&quot;");
            //换行
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\n", "<br/>");
            return str;
        }

        /// <summary>
        /// 获取客户端的IP
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
        /// 把HTML文字转换成原文
        /// </summary>
        /// <param name="strHTML">html文字</param>
        /// <returns></returns>
        public static string HTMLDecode(string str)
        {
            if (str == null)
            {
                return "";
            }
            //html标记符
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            // 处理空格
            str = str.Replace(" ", "&nbsp;");
            //处理双引号
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&#39;", "\'");
            str = str.Replace("&quot;", "\"");
            //&符号
            str = str.Replace("&amp;", "&");
            return str;
        }
        /// <summary>
        /// 格式化表格/层的显示字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        public static string ForamtStringCount(object str, int count) 
        {
            string strRet = ToBreakWord(str, count);
            strRet = HTMLEncode(strRet);
            return strRet;
        }
        /// <summary>
        /// 给字符串自动加空格以让表格自动换行
        /// </summary>
        /// <param name="str">字符串</param>
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
        /// 获取系统的虚拟目录
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
        /// 当前页面名
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
        /// 获取系统的域名+虚拟目录
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
        /// 是否允许该扩展名上传
        /// </summary>
        /// <param name="hifile">上传控件</param>
        /// <returns></returns>
        public static bool IsAllowedExtension(HttpPostedFile hifile)
        {
            if (hifile.FileName == "" || hifile.FileName == null)
            {
                return false;
            }
            string strOldFilePath = "", strExtension = "";

            //允许上传的扩展名，可以改成从配置文件中读出
            string[] arrExtension = { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };

            if (hifile.FileName != string.Empty)
            {
                strOldFilePath = hifile.FileName;
                //取得上传文件的扩展名
                strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));
                //判断该扩展名是否合法
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
        /// 判断上传文件大小是否超过最大值
        /// </summary>
        /// <param name="hifile">HtmlInputFile控件</param>
        /// <returns>超过最大值返回false,否则返回true.</returns>
        public static bool IsAllowedLength(HttpPostedFile hifile)
        {

            //允许上传文件大小的最大值,可以保存在xml文件中,单位为KB
            int i = 2 * 1024;
            //如果上传文件的大小超过最大值,返回flase,否则返回true.
            if (hifile.ContentLength > i * 1024)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 把文件写到回传对象
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void ShowFile(byte[] file, string fileName)
        {
            ShowFile("application/octet-stream", file, fileName);
        }

        /// <summary>
        /// 把文件写到回传对象
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
        /// 格式化JS的字符串
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
