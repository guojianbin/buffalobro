using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    
    
    /// <summary>
    /// 可选择头(IMAGE_OPTIONAL_HEADER)
    /// </summary>
    public struct IMAGE_OPTIONAL_HEADER32
    {
        /// <summary>
        /// 标记字
        /// </summary>
        public ImageOptionalMagicType Magic;
        /// <summary>
        /// 链接器的主版本号
        /// </summary>
        public byte MajorLinkerVersion;
        /// <summary>
        /// 链接器的次版本号
        /// </summary>
        public byte MinorLinkerVersion;
        /// <summary>
        /// 可执行代码的长度
        /// </summary>
        public uint SizeOfCode;
        /// <summary>
        /// 初始化数据的长度（数据段）
        /// </summary>
        public uint SizeOfInitializedData;
        /// <summary>
        /// 未初始化数据的长度（bss段）
        /// </summary>
        public uint SizeOfUninitializedData;
        /// <summary>
        /// 代码的入口RVA地址，程序从这儿开始执行，常称为程序的原入口点OEP（Original Entry Pouint）
        /// </summary>
        public uint AddressOfEntryPouint;
        /// <summary>
        /// 可执行代码起始位置
        /// </summary>
        public uint BaseOfCode;
        /// <summary>
        /// 初始化数据起始位置
        /// </summary>
        public uint BaseOfData;
        /// <summary>
        /// 载入程序首选的RVA地址。这个地址可被Loader改变
        /// </summary>
        public uint ImageBase;
        /// <summary>
        /// 段加载后在内存中的对齐值
        /// </summary>
        public uint SectionAlignment;
        /// <summary>
        /// 段在文件中的对齐方式
        /// </summary>
        public uint FileAlignment;
        /// <summary>
        /// 操作系统最低版本号的主版本号
        /// </summary>
        public ushort MajorOperatingSystemVersion;
        /// <summary>
        /// 操作系统最低版本号的次版本号
        /// </summary>
        public ushort MinorOperatingSystemVersion;
        /// <summary>
        /// 程序的主版本号
        /// </summary>
        public ushort MajorImageVersion;
        /// <summary>
        /// 程序的子版本号
        /// </summary>
        public ushort MinorImageVersion;
        /// <summary>
        /// 要求最低子系统版本的主版本号
        /// </summary>
        public ushort MajorSubsystemVersion;
        /// <summary>
        /// 要求最低子系统版本的次版本号
        /// </summary>
        public ushort MinorSubsystemVersion;
        /// <summary>
        /// 这个值总是为0
        /// </summary>
        public uint Win32VersionValue;
        /// <summary>
        /// 程序调入后占用内存大小（字节），等于所有段的长度之和
        /// </summary>
        public uint SizeOfImage;
        /// <summary>
        /// 所有文件头长度之和，它等于从文件开始到第一个段的原始数据之间的大小
        /// </summary>
        public uint SizeOfHeaders;
        /// <summary>
        /// 校验和，仅用在驱动程序中，在可执行文件中可能为0。它的计算方法Microsoft不公开，在imagehelp.dll中的CheckSumMappedFile()函数可以计算它
        /// </summary>
        public uint CheckSum;
        /// <summary>
        /// 一个标明可执行文件所期望的子系统的枚举值
        /// </summary>
        public SubSystemType Subsystem;
        /// <summary>
        /// DLL状态
        /// </summary>
        public ushort DllCharacteristics;
        /// <summary>
        /// 保留堆栈大小
        /// </summary>
        public uint SizeOfStackReserve;
        /// <summary>
        /// 启动后实际申请的堆栈数，可随实际情况变大
        /// </summary>
        public uint SizeOfStackCommit;
        /// <summary>
        /// 保留堆大小
        /// </summary>
        public uint SizeOfHeapReserve;
        /// <summary>
        /// 实际堆大小
        /// </summary>
        public uint SizeOfHeapCommit;
        /// <summary>
        /// 加载标志(与调试有关，默认为0)
        /// </summary>
        public uint LoaderFlags;
        /// <summary>
        /// 目录表入口个数，这个值也不可靠，可用常数
        /// </summary>
        public uint NumberOfRvaAndSizes;
        /// <summary>
        /// 数据目录表集合
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public IMAGE_DATA_DIRECTORY[] DataDirectory;
    }
}
