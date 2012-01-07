using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Buffalo.Kernel
{
    public class PasswordHash
    {
        /// <summary>
        /// ���ַ�������MD5����
        /// </summary>
        /// <param name="str">Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string ToMD5String(string str)
        {

            return ToMD5String(Encoding.Default.GetBytes(str));
        }

        /// <summary>
        /// ���ַ�������MD5����
        /// </summary>
        /// <param name="str">Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content)
        {
            MD5 hash = MD5CryptoServiceProvider.Create();
            byte[] retBytes = hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes);
        }

        /// <summary>
        /// ���ַ�������MD5����
        /// </summary>
        /// <param name="str">Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string ToSHA1String(string str)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str));
        }
        /// <summary>
        /// ���ַ�������MD5����
        /// </summary>
        /// <param name="str">Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content)
        {
            SHA1 hash = SHA1CryptoServiceProvider.Create();
            byte[] retBytes = hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes);
        }
    }
}
