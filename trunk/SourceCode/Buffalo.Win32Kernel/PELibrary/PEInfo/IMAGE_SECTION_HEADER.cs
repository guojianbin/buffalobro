using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct IMAGE_SECTION_HEADER
    {
        /// <summary>
        /// 节表名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)IMAGE_SIZEOF_SHORT_NAME.IMAGE_SIZEOF_SHORT_NAME)]
        public string Name; //[ IMAGE_SIZEOF_SHORT_NAME ] ;  
        /// <summary>
        /// 区块尺寸
        /// </summary>
        public Misc Misc;
        /// <summary>
        /// 虚拟地址
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// 文件对齐后的尺寸
        /// </summary>
        public uint SizeOfRawData;
        /// <summary>
        /// 节基于文件的偏移量
        /// </summary>
        public uint PointerToRawData;
        /// <summary>
        /// 重定位的偏移
        /// </summary>
        public uint PointerToRelocations;
        /// <summary>
        /// 行号表的偏移
        /// </summary>
        public uint PointerToLinenumbers;
        /// <summary>
        /// 重定位项数目
        /// </summary>
        public ushort NumberOfRelocations;
        /// <summary>
        /// 行号表的数目
        /// </summary>
        public ushort NumberOfLinenumbers;
        /// <summary>
        /// 节属性
        /// </summary>
        public IMAGE_SECTION_Characteristics Characteristics;
        public override string ToString()
        {
            return Name;
        }
        public bool IsNull 
        {
            get 
            {
                return VirtualAddress == 0 && SizeOfRawData == 0 && PointerToRawData == 0 & PointerToRelocations == 0 && PointerToLinenumbers == 0 && NumberOfRelocations == 0 && NumberOfLinenumbers == 0;
            }
        }
    }
}
