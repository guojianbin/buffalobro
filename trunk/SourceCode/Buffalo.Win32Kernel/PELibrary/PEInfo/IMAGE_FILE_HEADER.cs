using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    public enum MachineType : ushort
    {
        /// <summary>
        /// ��֪��������
        /// </summary>
        Unknow = 0,

        /// <summary>
        /// uintel 80386  ����������
        /// </summary>
        Intel80386 = 0x14c,

        /// <summary>
        /// uintel 80486  ����������
        /// </summary>
        Intel80486 = 0x14d,

        /// <summary>
        /// uintel���ڴ���������
        /// </summary>
        IntelPentium = 0x14e,

        /// <summary>
        /// R3000(MIPS)��������big endian
        /// </summary>
        R3000BigEndian = 0x0160,

        /// <summary>
        /// R3000(MIPS)��������little endian
        /// </summary>
        R3000LittleEndian = 0x162,

        /// <summary>
        /// R4000(MIPS)��������little endian
        /// </summary>
        R4000LittleEndian = 0x166,

        /// <summary>
        /// R10000(MIPS)��������little endian
        /// </summary>
        R10000LittleEndian = 0x168,

        /// <summary>
        ///  DEC Alpha AXP������
        /// </summary>
        AXP = 0x184,

        /// <summary>
        /// IBM Power PC��little endian
        /// </summary>
        IBMPowerPCLittleEndian = 0x1f0,
        /// <summary>
        /// ����
        /// </summary>
        Itanium=0x0200,
        /// <summary>
        /// X64λ
        /// </summary>
        X64=0x8664
    }

    public struct IMAGE_FILE_HEADER
    {
        /// <summary>
        /// ����ƽ̨
        /// </summary>
        public MachineType Machine;

        /// <summary>
        /// �ļ���������Ŀ
        /// </summary>
        public ushort NumberOfSections;

        /// <summary>
        /// �ļ��Ĵ������ں�ʱ��
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>
        /// ָ����ű�(���ڵ���)
        /// </summary>
        public uint PouinterToSymbolTable;

        /// <summary>
        /// ���ű��еķ��Ÿ���(���ڵ���)
        /// </summary>
        public uint NumberOfSymbols;

        /// <summary>
        /// IMAGE_OPTIONAL_HEADER�Ľṹ��С
        /// </summary>
        public ushort SizeOfOptionalHeader;

        /// <summary>
        /// �ļ�����
        /// </summary>
        public IMAGE_FILE_Characteristics Characteristics;

    }
}
