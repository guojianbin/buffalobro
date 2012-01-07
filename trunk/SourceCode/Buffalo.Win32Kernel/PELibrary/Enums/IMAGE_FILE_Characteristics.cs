using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    [Flags]
    public enum IMAGE_FILE_Characteristics:ushort
    {
        /// <summary>
        /// 文件不存在重定位信息
        /// </summary>
        NoRelocating = 0x0001,

        /// <summary>
        /// 文件可执行，如果为0则可能链接时候出问题了
        /// </summary>
        Execute = 0x0002,

        /// <summary>
        /// 行号信息被移去
        /// </summary>
        LinenumberRemoved = 0x0004,

        /// <summary>
        /// 符号信息被移去
        /// </summary>
        SymbolRemoved = 0x0008,

        /// <summary>
        /// 应用程序地址超过2GB地址
        /// </summary>
        Flat = 0x0020,

        /// <summary>
        /// 处理机的低字节位相反
        /// </summary>
        LowBitByteInstead = 0x0080,

        /// <summary>
        /// 目标平台是32位机器
        /// </summary>
        Platform32Bit = 0x0100,

        /// <summary>
        /// DBG文件调试信息被移去
        /// </summary>
        DBGRemoved = 0x0200,

        /// <summary>
        /// 如果映象文件在可移动设备中，则先复制到交换文件后再运行
        /// </summary>
        CopyFromMobile = 0x0400,

        /// <summary>
        /// 如果映象文件在网络中，则先复制到交换文件后再运行
        /// </summary>
        CopyFromNetwork = 0x0800,

        /// <summary>
        /// 系统文件
        /// </summary>
        SystemFile = 0x1000,

        /// <summary>
        /// Dll文件
        /// </summary>
        DllFile = 0x2000,

        /// <summary>
        /// 文件只能运行在单处理器上
        /// </summary>
        SingleProcessor = 0x4000,

        /// <summary>
        /// 处理机的高位字节是相反的
        /// </summary>
        HighBitByteInstead = 0x8000
    }
}
