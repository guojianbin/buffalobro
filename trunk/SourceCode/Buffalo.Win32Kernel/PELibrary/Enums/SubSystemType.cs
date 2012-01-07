using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// 执行文件所期望的子系统的枚举值
    /// </summary>
    [Flags]
    public enum SubSystemType : ushort
    {
        /// <summary>
        /// 未知文件
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// 不需要子系统
        /// </summary>
        UnNecessary = 1,
        /// <summary>
        /// 图形接口子系统
        /// </summary>
        GUI = 2,
        /// <summary>
        /// 字符子系统
        /// </summary>
        CUI = 3,
        /// <summary>
        /// OS/2字符子系统
        /// </summary>
        OS2CUI = 5,
        /// <summary>
        /// POSIX字符子系统
        /// </summary>
        POSIX = 7,
        /// <summary>
        /// 保留
        /// </summary>
        Reserved = 8,
        /// <summary>
        /// Windows CE图形界面
        /// </summary>
        WinCE
    }
}
