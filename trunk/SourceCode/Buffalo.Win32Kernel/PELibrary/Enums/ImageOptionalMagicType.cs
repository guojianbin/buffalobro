using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// 标记字类型
    /// </summary>
    [Flags]
    public enum ImageOptionalMagicType : ushort
    {
        /// <summary>
        /// 未知文件
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// ROM影像文件
        /// </summary>
        RomImage = 0x0170,
        /// <summary>
        /// 普通可执行文件
        /// </summary>
        CommonExe = 0x010B,
        /// <summary>
        /// PE32+文件
        /// </summary>
        PE32 = 0x020B
    }
}
