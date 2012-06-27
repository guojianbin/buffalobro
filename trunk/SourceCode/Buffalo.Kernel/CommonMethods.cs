
using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using Buffalo.Kernel.Replacer;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel.FastReflection;
using System.Web;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.Kernel.Win32;


namespace Buffalo.Kernel
{

    /// <summary>
    /// 常用的方法类
    /// </summary>
    public class CommonMethods
    {
        /// <summary>
        /// 常用的正则表达式
        /// </summary>
        public struct Regulars  
        {
            /// <summary>
            /// 固定电话
            /// </summary>
            public const string PhoneRegular = "0\\d{2,3}-\\d{7,8}";
            /// <summary>
            /// 所有电话
            /// </summary>
            public const string AllPhoneRegular = "(((86)|(086)|(86-)|(086-))?(((0)?13\\d{9})|((\\(\\d{3,4}\\)|\\d{3,4}-)?\\d{7,8})))((-)?\\d{0,4})";
            /// <summary>
            /// 移动电话
            /// </summary>
            public const string Mobile = "((\\(\\d{3}\\))|(\\d{3}\\-))?13\\d{9}|15[89]\\d{8}";

        }
        private CommonMethods()
        {

        }


        private static string baseRoot = null;//基目录
        /// <summary>
        /// 获取基路径
        /// </summary>
        /// <param name="configRoot"></param>
        /// <returns></returns>
        public static string GetBaseRoot(string configRoot)
        {
            
            if (baseRoot == null)
            {
                baseRoot = AppDomain.CurrentDomain.BaseDirectory;

            }
            string retRoot = baseRoot + configRoot;
            return retRoot;
        }
        /// <summary>
        /// 检测是否Web程序
        /// </summary>
        public static bool IsWebContext
        {
            get
            {
                return (HttpContext.Current != null);
            }
        }
        /// <summary>
        /// 获取应用程序的基目录
        /// </summary>
        /// <returns></returns>
        public static string GetBaseRoot()
        {
            if (IsWebContext)
            {
                return AppDomain.CurrentDomain.DynamicDirectory;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Clone(object source) 
        {
            return FieldCloneHelper.Clone(source);
        }

        /// <summary>
        /// 把文件移到回收站
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int SendToRecycleBin(string file) 
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = WFunc.FO_DELETE;
            lpFileOp.pFrom = file + "\0";
            lpFileOp.fFlags = FILEOP_FLAGS.FOF_NOCONFIRMATION | FILEOP_FLAGS.FOF_NOERRORUI | FILEOP_FLAGS.FOF_SILENT;
            lpFileOp.fFlags |= FILEOP_FLAGS.FOF_ALLOWUNDO;//允许撤销即为放进回收站
            lpFileOp.fAnyOperationsAborted = false;

            int n =WindowsAPI.SHFileOperation(ref lpFileOp);
            return n;

            
        }

        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="target">目标类</param>
        public static void CopyTo(object source, object target)
        {
            FieldCloneHelper.CopyTo(source, target);
        }
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsNull(object value) 
        {
            return object.ReferenceEquals(value, null);
        }

        /// <summary>
        /// GUID转成字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GuidToString(Guid id)
        {
            return BytesToHexString(id.ToByteArray());
        }



        /// <summary>
        /// 字符串转回GUID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Guid StringToGuid(string id)
        {
            byte[] arrId = HexStringToBytes(id);
            if (arrId.Length != 16)
            {
                throw new Exception("错误的GUID字符串");
            }
            return new Guid(arrId);
        }
        /// <summary>
        /// 把DataSet打成XML字符串
        /// </summary>
        /// <param name="ds">要处理的DataSet</param>
        /// <param name="mode">指定如何从 System.Data.DataSet 写入 XML 数据和关系架构</param>
        /// <returns></returns>
        public static string DataSetToXML(DataSet ds,XmlWriteMode mode)
        {
            string ret = null;
            using (MemoryStream stm = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(stm, System.Text.Encoding.UTF8);
                ds.WriteXml(writer, mode);
                byte[] buffer = stm.ToArray();
                ret = System.Text.Encoding.UTF8.GetString(buffer);
            }
            return ret;
        }
        /// <summary>
        /// 把DataSet打成XML字符串
        /// </summary>
        /// <param name="ds">要处理的DataSet</param>
        /// <returns></returns>
        public static string DataSetToXML(DataSet ds) 
        {
            return DataSetToXML(ds, XmlWriteMode.WriteSchema);
        }

        /// <summary>
        /// XML字符串转成DataSet
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="mode">指定如何将 XML 数据和关系架构读入 System.Data.DataSet</param>
        /// <returns></returns>
        private DataSet XMLToDataSet(string xml,XmlReadMode mode)
        {
            DataSet ds = new DataSet();
            using (MemoryStream stm = new MemoryStream())
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(xml);
                stm.Write(buffer, 0, buffer.Length);
                stm.Position = 0;
                ds.ReadXml(stm, mode);
            }
            return ds;
        }
        /// <summary>
        /// XML字符串转成DataSet
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        private DataSet XMLToDataSet(string xml)
        {

            return XMLToDataSet(xml,XmlReadMode.ReadSchema);
        }
        /// <summary>
        /// 反序列化结构体
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static object RawDeserialize(byte[] rawdatas,Type objType)
        {

            //Type anytype = typeof(T);

            int rawsize = Marshal.SizeOf(objType);
            object retobj = null;
            if (rawsize > rawdatas.Length)
            {
                return retobj;
            }
            
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.Copy(rawdatas, 0, buffer, rawsize);

                retobj = Marshal.PtrToStructure(buffer, objType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return retobj;
        }
        /// <summary>
        /// 反序列化结构体
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static T RawDeserialize<T>(byte[] rawdatas)
        {
            object obj=RawDeserialize(rawdatas,typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// 从流中读出元素
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static object RawDeserialize(Stream stm, Type objType) 
        {
            int rawsize = Marshal.SizeOf(objType);
            byte[] fbuffer = new byte[rawsize];
            rawsize=stm.Read(fbuffer, 0, rawsize);
            object retobj = null;
            
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            try
            {
                Marshal.Copy(fbuffer, 0, buffer, rawsize);

                retobj = Marshal.PtrToStructure(buffer, objType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return retobj;
        }

        /// <summary>
        /// 从流中读出元素
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static T RawDeserialize<T>(Stream stm)
        {


            return (T)RawDeserialize(stm,typeof(T));
        }

        /// <summary>
        /// 对象序列化成字节数组
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static byte[] RawSerialize(object obj)
        {

            int rawsize = Marshal.SizeOf(obj);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);

            byte[] rawdatas = null;
            try
            {
                Marshal.StructureToPtr(obj, buffer, false);

                rawdatas = new byte[rawsize];

                Marshal.Copy(buffer, rawdatas, 0, rawsize);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return rawdatas;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLocalIP() 
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = ipEntry.AddressList[0];
            return ip;
        }

        /// <summary>
        /// 实体类型转换
        /// </summary>
        /// <param name="sValue">源值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object EntityProChangeType(object sValue, Type targetType) 
        {
            Type valType = sValue.GetType();//实际值的类型
            //Type resType = info.FieldType;//字段值类型

            if (!targetType.Equals(valType))
            {


                targetType = DefaultType.GetRealValueType(targetType);
                if (targetType.IsEnum) 
                {
                    targetType = typeof(int);
                }
                sValue = ChangeType(sValue, targetType);

            }
            return sValue;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="sValue">值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType(object sValue, Type targetType) 
        {
            if (targetType == DefaultType.GUIDType) 
            {
                string str = sValue as string;
                if (!string.IsNullOrEmpty(str))
                {
                    return StringToGuid(str);
                }
            }
            return Convert.ChangeType(sValue,targetType);
        }

        /// <summary>
        /// 按照条件替换字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="conditions">替换条件集合</param>
        /// <returns></returns>
        public static string ReplaceString(string source, IEnumerable<ReplaceItem> conditions)
        {
            return MyReplacer.Replace(source, conditions);
        }

        /// <summary>
        /// 读取流的内容
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static byte[] LoadStreamData(Stream stm) 
        {
            byte[] tmp = null;
            if (stm != null && stm.Length > 0)
            {
                tmp = new byte[stm.Length];
                stm.Read(tmp, 0, tmp.Length);
            }
            return tmp;
        }
        /// <summary>
        /// 读取流的内容
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns></returns>
        public static byte[] LoadStreamData2(Stream stm)
        {
            MemoryStream tmpStm = new MemoryStream();
            byte[] buffer = new byte[512];
            if (stm != null)
            {
                //tmp = new byte[stm.Length];
                int loaded = 0;
                while ((loaded = stm.Read(buffer, 0, buffer.Length)) > 0)
                {
                    tmpStm.Write(buffer, 0, loaded);
                }
            }
            return tmpStm.ToArray();
        }
        /// <summary>
        /// 流内容复制
        /// </summary>
        /// <param name="stmSource">源</param>
        /// <param name="stmTarget">目标</param>
        public static void CopyStreamData(Stream stmSource,Stream stmTarget)
        {
            byte[] buffer = new byte[1024];
            int readed = 0;
            do
            {
                readed = stmSource.Read(buffer, 0, buffer.Length);
                if (readed > 0)
                {
                    stmTarget.Write(buffer, 0, readed);
                }
            } while (readed>0);
            
        }
        /// <summary>
        /// 获取字符串里边的所有数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAllNumber(string str) 
        {
            if (str == null || str == "")
            {
                return "0";
            }
            string ret = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str, i))
                {
                    ret += str[i];
                }
            }
            if (ret.Length <= 0) 
            {
                ret = "0";
            }
            return ret;
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsStringEmpty(string str) 
        {
            if (str == "" || str == null) 
            {
                return true;
            }
            return false;
        }

        

        /// <summary>
        /// 把字节数组转成十六进制字符串
        /// </summary>
        /// <param name="bye">字节数组</param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bye)
        {
            StringBuilder retStr = new StringBuilder(bye.Length * 2); ;
            for (int i = 0; i < bye.Length; i++)
            {
                retStr.Append(ByteToHexString(bye[i]));
            }
            return retStr.ToString();
        }

        /// <summary>
        /// 把字节转成十六进制字符串
        /// </summary>
        /// <param name="bye">字节</param>
        /// <returns></returns>
        public static string ByteToHexString(byte bye)
        {
            string curStr = bye.ToString("X");
            if (curStr.Length < 2)
            {
                curStr = "0" + curStr;
            }
            return curStr;
        }



        /// <summary>
        /// 把文字转成十六进制字符码
        /// </summary>
        /// <returns></returns>
        public static string ToByteString(string str) 
        {
            byte[] byes = System.Text.Encoding.Unicode.GetBytes(str);
            string ret = "";
            for (int i = 0; i < byes.Length; i++) 
            {
                string tmp = byes[i].ToString("X");
                if (tmp.Length < 2) 
                {
                    tmp = "0" + tmp;
                }
                ret += tmp;
            }
            return ret;
        }
        /// <summary>
        /// 把十六进制字符串转成字节数组
        /// </summary>
        /// <param name="str">十六进制字符串</param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string str)
        {
            byte[] ret = new byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                char chr1 = str[i * 2];
                char chr2 = str[i * 2 + 1];
                string strCurNum = new string(new char[] { chr1, chr2 });
                int curNum = int.Parse(strCurNum, System.Globalization.NumberStyles.AllowHexSpecifier);
                ret[i] = (byte)curNum;
            }
            return ret;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatString(object str)
        {
            if (str == null)
            {
                return "";
            }
            return str.ToString();
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatLongString(object str,int maxchr)
        {
            if (str == null)
            {
                return "";
            }
            string retStr = str.ToString();
            if (retStr.Length > maxchr)
            {
                retStr = retStr.Substring(0, maxchr-3) + "...";
            }
            return retStr;
        }
        private static char[] chrs;
        /// <summary>
        ///  随机生成字符串
        /// </summary>
        /// <param name="length">生成多少位随机字符串</param>
        /// <returns></returns>
        public static string GetCode(int length)
        {
            string lcode = "";
            if (chrs == null)//如果字符库还没初始化就初始化
            {
                chrs = new char[36];
                for (int i = 0; i < 26; i++)
                {
                    char chr = (char)('A' + i);
                    chrs[i] = chr;
                }
                for (int k = 0; k < 10; k++)
                {
                    chrs[26 + k] = (char)('0' + k);
                }
            }
            int seed = DateTime.Now.Day * 1000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute * 10 + DateTime.Now.Second;
            Random rnd = new Random(seed);
            for (int x = 0; x < 4; x++)
            {
                int ind = (int)((float)(chrs.Length) * rnd.NextDouble());
                lcode += chrs[ind].ToString();
            }

            return lcode;
        }

        /// <summary>
        /// 格式化输出的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatDateTimeString(DateTime dt)
        {

            string ret = dt.Year.ToString() + ".";

            string tmp = dt.Month.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp + ".";

            tmp = dt.Day.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp + " ";

            tmp = dt.Hour.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp + ":";

            tmp = dt.Minute.ToString();
            if (tmp.Length < 2)
            {
                tmp = "0" + tmp;
            }
            ret += tmp;
            return ret;
        }

        
        /// <summary>
        /// 判断该字符串是否整型数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsIntNumber(string str)
        {
            if (str == null || str == "")
            {
                return false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsDigit(str, i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 把集合转换成字典类
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型 </typeparam>
        /// <param name="collection">集合类</param>
        /// <param name="keyProperty">键名</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ListToDictionary<TKey, TValue>(IEnumerable<TValue> collection, string keyProperty) 
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();

            FastInvokeHandler handle=FastValueGetSet.GetGetMethodInfo(keyProperty,typeof(TValue));
            object[] emptyParams=new object[] { };
            foreach (TValue objValue in collection) 
            {
                object obj=objValue;
                if(obj==null)
                {
                    continue;
                }
                TKey key=default(TKey);
                object objKey = handle(obj, emptyParams);
                if (typeof(TKey) == DefaultType.StringType)
                {
                    object strkey = objKey.ToString();
                    dic[(TKey)strkey] = objValue;
                }
                else 
                {
                    key = (TKey)objKey;
                    dic[key] = objValue;
                }
                
            }
            return dic;
        }

        /// <summary>
        /// 判断该字符串是否整型数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (str == null || str == "")
            {
                return false;
            }
            bool hasPoint=false;
            for (int i = 0; i < str.Length; i++)
            {
                char chr = str[i];
                if (str[i] == '.')
                {
                    if (hasPoint)//如果已经出现过点的话，就返回错误
                    {
                        return false;
                    }
                    hasPoint = true;
                }
                else
                {
                    if (!char.IsDigit(chr))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 创建文件名
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileNamepart">文件名</param>
        /// <param name="extendName">扩展名</param>
        /// <returns></returns>
        public static string GetFileName(string path, string fileNamepart, string extendName)
        {
            string root = path + fileNamepart + extendName;
            int index = 0;
            string fileName = fileNamepart + index.ToString();
            while (File.Exists(root))
            {
                fileName = fileNamepart + index.ToString();
                root = path + fileName + extendName;
                index++;
            }
            return fileName;
        }
        /// <summary>
        /// 按时间生成字符串
        /// </summary>
        /// <returns></returns>
        public static string CurrentDataString()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssms");
        }

        /// <summary>
        /// 把字符串变成字节数组，然后数组每个元素减去指定的值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="bye">要减去的值</param>
        /// <returns></returns>
        public static string StringToKey(string str,byte bye) 
        {
            if (str == null)
            {
                return null;
            }
            byte[] strBye = System.Text.Encoding.UTF8.GetBytes(str);
            StringBuilder sbRet = new StringBuilder();
            foreach (byte cur in strBye) 
            {
                int curByte = (int)cur - (int)bye;
                if (curByte < 0) 
                {
                    curByte = curByte + 256;
                }
                sbRet.Append(ByteToHexString((byte)curByte));
            }
            return sbRet.ToString();
        }
        /// <summary>
        /// 把字节数组变回字符串，数组每个元素加上指定的值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="bye">要加上的值</param>
        /// <returns></returns>
        public static string KeyToString(string str, byte bye)
        {
            if (str == null) 
            {
                return null;
            }
            byte[] strBye = HexStringToBytes(str);

            for (int i = 0; i < strBye.Length;i++)
            {
                byte cur = strBye[i];
                int curByte = (int)cur + (int)bye;
                curByte = curByte % 256;
                strBye[i] = (byte)curByte;
            }
            string ret = System.Text.Encoding.UTF8.GetString(strBye);
            return ret;
        }

        /// <summary>
        /// 获取文件路径所在的文件夹
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFilePath(string filePath)
        {
            int lastIndex = filePath.LastIndexOf('\\');
            string dic = "";
            if (lastIndex >= 0)
            {
                dic = filePath.Substring(0, lastIndex + 1);
            }
            return dic;
        }
    }
}