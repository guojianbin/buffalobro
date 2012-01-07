using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// ִ���ļ�����������ϵͳ��ö��ֵ
    /// </summary>
    [Flags]
    public enum SubSystemType : ushort
    {
        /// <summary>
        /// δ֪�ļ�
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// ����Ҫ��ϵͳ
        /// </summary>
        UnNecessary = 1,
        /// <summary>
        /// ͼ�νӿ���ϵͳ
        /// </summary>
        GUI = 2,
        /// <summary>
        /// �ַ���ϵͳ
        /// </summary>
        CUI = 3,
        /// <summary>
        /// OS/2�ַ���ϵͳ
        /// </summary>
        OS2CUI = 5,
        /// <summary>
        /// POSIX�ַ���ϵͳ
        /// </summary>
        POSIX = 7,
        /// <summary>
        /// ����
        /// </summary>
        Reserved = 8,
        /// <summary>
        /// Windows CEͼ�ν���
        /// </summary>
        WinCE
    }
}
