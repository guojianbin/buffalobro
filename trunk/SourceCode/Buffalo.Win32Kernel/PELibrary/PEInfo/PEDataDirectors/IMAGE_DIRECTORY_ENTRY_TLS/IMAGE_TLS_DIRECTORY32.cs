using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_DIRECTORY_ENTRY_TLS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_TLS_DIRECTORY32
    {
        /// <summary>
        /// 内存起始地址
        /// </summary>
        public uint StartAddressOfRawData; 
        /// <summary>
        /// 内存终止地址
        /// </summary>
        public uint EndAddressOfRawData; 
        /// <summary>
        /// 运行库使用这个索引来定位局部数据
        /// </summary>
        public uint AddressOfIndex; 
        /// <summary>
        /// PIMAGE_TLS_CALLBACK函数指针数组地址
        /// </summary>
        public uint AddressOfCallBacks; 
        /// <summary>
        /// 后边跟零的个数
        /// </summary>
        public uint SizeOfZeroFill; 
        /// <summary>
        /// 保留，设为0
        /// </summary>
        public uint Characteristics;
    }
}
