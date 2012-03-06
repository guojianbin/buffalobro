using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.Kernel
{

    public class FileEncodingInfo
    {
        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetEncodingType(string fileName)
        {
            Encoding ret = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                ret = GetEncodingType(fs);
            }
            return ret;
        }
        private static byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        private static byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        private static byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetEncodingType(Stream fs)
        {
            
            Encoding reVal = null;

            byte[] buffer = new byte[512];

            int read = fs.Read(buffer, 0, buffer.Length);
            if (IsUTF8Bytes(buffer) || IsEncoding(buffer,UTF8))
            {
                reVal = Encoding.UTF8;
            }
            else if (IsEncoding(buffer,UnicodeBIG))
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (IsEncoding(buffer,Unicode))
            {
                reVal = Encoding.Unicode;
            }

            return reVal;

        }

        /// <summary>
        /// 是否此编码
        /// </summary>
        /// <param name="buffer">内容</param>
        /// <param name="headContent">文件头内容</param>
        /// <returns></returns>
        private static bool IsEncoding(byte[] buffer, byte[] headContent) 
        {
            if (buffer.Length < headContent.Length) 
            {
                return false;
            }
            for (int i = 0; i < headContent.Length; i++) 
            {
                if (buffer[i] != headContent[i]) 
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                return false;
            }
            return true;
        }
    }
}
