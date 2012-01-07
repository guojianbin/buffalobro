using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// 区块属性类
    /// </summary>
    [Flags]
    public enum IMAGE_SECTION_Characteristics:uint
    {
        /// <summary>
        /// 包含代码，常与10000000H一起设置
        /// </summary>
        IMAGE_SCN_CNT_CODE = 0x20,

        /// <summary>
        /// 该块包含已初始化的数据
        /// </summary>
        IMAGE_SCN_CNT_INITIALIZED_DATA = 0x40,

        /// <summary>
        /// 该块包含未初始化的数据
        /// </summary>
        IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x80,

        /// <summary>
        /// 该块可被丢弃，因为它一旦被装入后，进程就不再需要它了常见的可丢弃块寺.reloc(重定位块)
        /// </summary>
        IMAGE_SCN_MEM_DISCARDABLE = 0x2000000,

        /// <summary>
        /// 该块为共享
        /// </summary>
        IMAGE_SCN_MEM_SHARED = 0x10000000,

        /// <summary>
        /// 该块为可执行。通常当00000020H标志被设置时，该标志也被设置
        /// </summary>
        IMAGE_SCN_MEM_EXECUTE = 0x20000000,

        /// <summary>
        /// 该块可读。可执行文件中的块总是设置该块标志
        /// </summary>
        IMAGE_SCN_MEM_READ = 0x40000000,

        /// <summary>
        /// 该块可写。如果可执行文件没有设置该标志，装载程序就将在内存映象页标记为可读或可执行
        /// </summary>
        IMAGE_SCN_MEM_WRITE = 0x80000000
    }
}
