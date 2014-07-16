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
        /// �ڱ�����
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)IMAGE_SIZEOF_SHORT_NAME.IMAGE_SIZEOF_SHORT_NAME)]
        public string Name; //[ IMAGE_SIZEOF_SHORT_NAME ] ;  
        /// <summary>
        /// ����ߴ�
        /// </summary>
        public Misc Misc;
        /// <summary>
        /// �����ַ
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// �ļ������ĳߴ�
        /// </summary>
        public uint SizeOfRawData;
        /// <summary>
        /// �ڻ����ļ���ƫ����
        /// </summary>
        public uint PointerToRawData;
        /// <summary>
        /// �ض�λ��ƫ��
        /// </summary>
        public uint PointerToRelocations;
        /// <summary>
        /// �кű��ƫ��
        /// </summary>
        public uint PointerToLinenumbers;
        /// <summary>
        /// �ض�λ����Ŀ
        /// </summary>
        public ushort NumberOfRelocations;
        /// <summary>
        /// �кű����Ŀ
        /// </summary>
        public ushort NumberOfLinenumbers;
        /// <summary>
        /// ������
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
