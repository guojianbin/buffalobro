using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    public enum MachineType : ushort
    {
        /// <summary>
        /// 不知名处理器
        /// </summary>
        Unknow = 0,

        /// <summary>
        /// uintel 80386  处理器以上
        /// </summary>
        Intel80386 = 0x14c,

        /// <summary>
        /// uintel 80486  处理器以上
        /// </summary>
        Intel80486 = 0x14d,

        /// <summary>
        /// uintel奔腾处理器以上
        /// </summary>
        IntelPentium = 0x14e,

        /// <summary>
        /// R3000(MIPS)处理器，big endian
        /// </summary>
        R3000BigEndian = 0x0160,

        /// <summary>
        /// R3000(MIPS)处理器，little endian
        /// </summary>
        R3000LittleEndian = 0x162,

        /// <summary>
        /// R4000(MIPS)处理器，little endian
        /// </summary>
        R4000LittleEndian = 0x166,

        /// <summary>
        /// R10000(MIPS)处理器，little endian
        /// </summary>
        R10000LittleEndian = 0x168,

        /// <summary>
        ///  DEC Alpha AXP处理器
        /// </summary>
        AXP = 0x184,

        /// <summary>
        /// IBM Power PC，little endian
        /// </summary>
        IBMPowerPCLittleEndian = 0x1f0,
        /// <summary>
        /// 安腾
        /// </summary>
        Itanium=0x0200,
        /// <summary>
        /// X64位
        /// </summary>
        X64=0x8664
    }

    public struct IMAGE_FILE_HEADER
    {
        /// <summary>
        /// 运行平台
        /// </summary>
        public MachineType Machine;

        /// <summary>
        /// 文件的区块数目
        /// </summary>
        public ushort NumberOfSections;

        /// <summary>
        /// 文件的创建日期和时间
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>
        /// 指向符号表(用于调试)
        /// </summary>
        public uint PouinterToSymbolTable;

        /// <summary>
        /// 符号表中的符号个数(用于调试)
        /// </summary>
        public uint NumberOfSymbols;

        /// <summary>
        /// IMAGE_OPTIONAL_HEADER的结构大小
        /// </summary>
        public ushort SizeOfOptionalHeader;

        /// <summary>
        /// 文件属性
        /// </summary>
        public IMAGE_FILE_Characteristics Characteristics;

    }
}
