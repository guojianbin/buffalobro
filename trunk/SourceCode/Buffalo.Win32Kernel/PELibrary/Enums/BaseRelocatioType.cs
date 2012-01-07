using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// 重定位类型
    /// </summary>
    [Flags]
    public enum BaseRelocatioType : ushort
    {
        /// <summary>
        /// Image_Rel_Based_Absolute 没有含义为了让4位字节对齐
        /// </summary>
        ImageRelBasedAbsolute = 0,

        /// <summary>
        /// Image_Rel_Based_Highlow 重定位指向的整个地址都被修正
        /// </summary>
        ImageRelBasedHighlow = 3,

        /// <summary>
        /// Image_Rel_Based_Dir64 出现在64位PE文件中，对指向的整个地址修正
        /// </summary>
        ImageRelBasedDir64 = 10
    }
}
