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
        /// û����;������Ϊ0
        /// </summary>
        public uint Characteristics;
        /// <summary>
        /// �ļ�������ʱ��
        /// </summary>
        public uint TimeDateStamp;
        /// <summary>
        /// ���汾�ţ�����Ϊ0
        /// </summary>
        public ushort MajorVersion;
        /// <summary>
        /// �ΰ汾������Ϊ0
        /// </summary>
        public ushort MinorVersion;
        /// <summary>
        /// RVA��ָ��һ��DLL�ļ�����
        /// </summary>
        public uint Name;
        /// <summary>
        /// ��ʼ���
        /// </summary>
        public uint Base;
        /// <summary>
        /// �����������
        /// </summary>
        public uint NumberOfFunctions;
        /// <summary>
        /// ����������ĺ�������
        /// </summary>
        public uint NumberOfNames;
        /// <summary>
        /// RVA, ָ��һ���ɺ�����ַ���ɵ�����
        /// </summary>
        public uint AddressOfFunctions;     // RVA from base of image
        /// <summary>
        /// RVA, ָ��һ�����ַ���ָ�������ɵ�����
        /// </summary>
        public uint AddressOfNames;     // RVA from base of image
        /// <summary>
        /// ����������RAV
        /// </summary>
        public uint AddressOfNameOrdinals;  // RVA from base of image
    }
}
