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
        /// �ڴ���ʼ��ַ
        /// </summary>
        public uint StartAddressOfRawData; 
        /// <summary>
        /// �ڴ���ֹ��ַ
        /// </summary>
        public uint EndAddressOfRawData; 
        /// <summary>
        /// ���п�ʹ�������������λ�ֲ�����
        /// </summary>
        public uint AddressOfIndex; 
        /// <summary>
        /// PIMAGE_TLS_CALLBACK����ָ�������ַ
        /// </summary>
        public uint AddressOfCallBacks; 
        /// <summary>
        /// ��߸���ĸ���
        /// </summary>
        public uint SizeOfZeroFill; 
        /// <summary>
        /// ��������Ϊ0
        /// </summary>
        public uint Characteristics;
    }
}
