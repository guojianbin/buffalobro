using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    
    
    /// <summary>
    /// ��ѡ��ͷ(IMAGE_OPTIONAL_HEADER)
    /// </summary>
    public struct IMAGE_OPTIONAL_HEADER32
    {
        /// <summary>
        /// �����
        /// </summary>
        public ImageOptionalMagicType Magic;
        /// <summary>
        /// �����������汾��
        /// </summary>
        public byte MajorLinkerVersion;
        /// <summary>
        /// �������Ĵΰ汾��
        /// </summary>
        public byte MinorLinkerVersion;
        /// <summary>
        /// ��ִ�д���ĳ���
        /// </summary>
        public uint SizeOfCode;
        /// <summary>
        /// ��ʼ�����ݵĳ��ȣ����ݶΣ�
        /// </summary>
        public uint SizeOfInitializedData;
        /// <summary>
        /// δ��ʼ�����ݵĳ��ȣ�bss�Σ�
        /// </summary>
        public uint SizeOfUninitializedData;
        /// <summary>
        /// ��������RVA��ַ������������ʼִ�У�����Ϊ�����ԭ��ڵ�OEP��Original Entry Pouint��
        /// </summary>
        public uint AddressOfEntryPouint;
        /// <summary>
        /// ��ִ�д�����ʼλ��
        /// </summary>
        public uint BaseOfCode;
        /// <summary>
        /// ��ʼ��������ʼλ��
        /// </summary>
        public uint BaseOfData;
        /// <summary>
        /// ���������ѡ��RVA��ַ�������ַ�ɱ�Loader�ı�
        /// </summary>
        public uint ImageBase;
        /// <summary>
        /// �μ��غ����ڴ��еĶ���ֵ
        /// </summary>
        public uint SectionAlignment;
        /// <summary>
        /// �����ļ��еĶ��뷽ʽ
        /// </summary>
        public uint FileAlignment;
        /// <summary>
        /// ����ϵͳ��Ͱ汾�ŵ����汾��
        /// </summary>
        public ushort MajorOperatingSystemVersion;
        /// <summary>
        /// ����ϵͳ��Ͱ汾�ŵĴΰ汾��
        /// </summary>
        public ushort MinorOperatingSystemVersion;
        /// <summary>
        /// ��������汾��
        /// </summary>
        public ushort MajorImageVersion;
        /// <summary>
        /// ������Ӱ汾��
        /// </summary>
        public ushort MinorImageVersion;
        /// <summary>
        /// Ҫ�������ϵͳ�汾�����汾��
        /// </summary>
        public ushort MajorSubsystemVersion;
        /// <summary>
        /// Ҫ�������ϵͳ�汾�Ĵΰ汾��
        /// </summary>
        public ushort MinorSubsystemVersion;
        /// <summary>
        /// ���ֵ����Ϊ0
        /// </summary>
        public uint Win32VersionValue;
        /// <summary>
        /// ��������ռ���ڴ��С���ֽڣ����������жεĳ���֮��
        /// </summary>
        public uint SizeOfImage;
        /// <summary>
        /// �����ļ�ͷ����֮�ͣ������ڴ��ļ���ʼ����һ���ε�ԭʼ����֮��Ĵ�С
        /// </summary>
        public uint SizeOfHeaders;
        /// <summary>
        /// У��ͣ����������������У��ڿ�ִ���ļ��п���Ϊ0�����ļ��㷽��Microsoft����������imagehelp.dll�е�CheckSumMappedFile()�������Լ�����
        /// </summary>
        public uint CheckSum;
        /// <summary>
        /// һ��������ִ���ļ�����������ϵͳ��ö��ֵ
        /// </summary>
        public SubSystemType Subsystem;
        /// <summary>
        /// DLL״̬
        /// </summary>
        public ushort DllCharacteristics;
        /// <summary>
        /// ������ջ��С
        /// </summary>
        public uint SizeOfStackReserve;
        /// <summary>
        /// ������ʵ������Ķ�ջ��������ʵ��������
        /// </summary>
        public uint SizeOfStackCommit;
        /// <summary>
        /// �����Ѵ�С
        /// </summary>
        public uint SizeOfHeapReserve;
        /// <summary>
        /// ʵ�ʶѴ�С
        /// </summary>
        public uint SizeOfHeapCommit;
        /// <summary>
        /// ���ر�־(������йأ�Ĭ��Ϊ0)
        /// </summary>
        public uint LoaderFlags;
        /// <summary>
        /// Ŀ¼����ڸ��������ֵҲ���ɿ������ó���
        /// </summary>
        public uint NumberOfRvaAndSizes;
        /// <summary>
        /// ����Ŀ¼����
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public IMAGE_DATA_DIRECTORY[] DataDirectory;
    }
}
