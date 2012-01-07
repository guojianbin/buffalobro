using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// ���������
    /// </summary>
    [Flags]
    public enum ImageOptionalMagicType : ushort
    {
        /// <summary>
        /// δ֪�ļ�
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// ROMӰ���ļ�
        /// </summary>
        RomImage = 0x0170,
        /// <summary>
        /// ��ͨ��ִ���ļ�
        /// </summary>
        CommonExe = 0x010B,
        /// <summary>
        /// PE32+�ļ�
        /// </summary>
        PE32 = 0x020B
    }
}
