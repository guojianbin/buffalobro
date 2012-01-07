using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        
        [FieldOffset(0)]
        public uint Characteristics;
        /// <summary>
        /// 指向包含输入名称表(INT)的RVA 或者是属性
        /// </summary>
        [FieldOffset(0)]
        public uint OriginalFirstThunk;
        /// <summary>
        /// 32位时间标记
        /// </summary>
        [FieldOffset(4)]
        public uint TimeDateStamp;
        /// <summary>
        /// 第一个被转向的API索引，一般为0
        /// </summary>
        [FieldOffset(8)]
        public uint ForwarderChain;
        /// <summary>
        /// dll名字的指针(以00结尾的RAV地址)
        /// </summary>
        [FieldOffset(12)]
        public uint Name;
        /// <summary>
        /// 指向包含地址名称表(IAT)的RVA 或者是属性
        /// </summary>
        [FieldOffset(16)]
        public uint FirstThunk;

        public bool IsNull 
        {
            get
            {
                return OriginalFirstThunk == 0 && TimeDateStamp == 0 && ForwarderChain == 0 && Name == 0 && FirstThunk == 0;
            }
        }

        


        
    }
}
