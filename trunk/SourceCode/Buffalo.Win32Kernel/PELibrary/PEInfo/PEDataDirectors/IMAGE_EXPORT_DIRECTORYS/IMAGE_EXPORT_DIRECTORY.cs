using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_EXPORT_DIRECTORYS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_EXPORT_DIRECTORY
    {
        /// <summary>
        /// 没有用途，总是为0
        /// </summary>
        public uint Characteristics;
        /// <summary>
        /// 文件被产生时刻
        /// </summary>
        public uint TimeDateStamp;
        /// <summary>
        /// 主版本号，总是为0
        /// </summary>
        public ushort MajorVersion;
        /// <summary>
        /// 次版本号总是为0
        /// </summary>
        public ushort MinorVersion;
        /// <summary>
        /// RVA，指向一个DLL文件名称
        /// </summary>
        public uint Name;
        /// <summary>
        /// 起始序号
        /// </summary>
        public uint Base;
        /// <summary>
        /// 输出函数个数
        /// </summary>
        public uint NumberOfFunctions;
        /// <summary>
        /// 以名称输出的函数个数
        /// </summary>
        public uint NumberOfNames;
        /// <summary>
        /// RVA, 指向一个由函数地址构成的数组
        /// </summary>
        public uint AddressOfFunctions;     // RVA from base of image
        /// <summary>
        /// RVA, 指向一个由字符串指针所构成的数组
        /// </summary>
        public uint AddressOfNames;     // RVA from base of image
        /// <summary>
        /// 输出序数表的RAV
        /// </summary>
        public uint AddressOfNameOrdinals;  // RVA from base of image
    }
}
