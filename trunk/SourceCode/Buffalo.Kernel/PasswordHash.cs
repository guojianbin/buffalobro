using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Buffalo.Kernel
{
    public class PasswordHash
    {
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToMD5String(string str)
        {

            return ToMD5String(Encoding.Default.GetBytes(str));
        }

        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content)
        {
            MD5 hash = MD5CryptoServiceProvider.Create();
            byte[] retBytes = hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes);
        }

        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(string str)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str));
        }
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content)
        {
            SHA1 hash = SHA1CryptoServiceProvider.Create();
            byte[] retBytes = hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes);
        }
    }
}
