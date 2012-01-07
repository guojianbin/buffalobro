using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// ����������
    /// </summary>
    [Flags]
    public enum IMAGE_SECTION_Characteristics:uint
    {
        /// <summary>
        /// �������룬����10000000Hһ������
        /// </summary>
        IMAGE_SCN_CNT_CODE = 0x20,

        /// <summary>
        /// �ÿ�����ѳ�ʼ��������
        /// </summary>
        IMAGE_SCN_CNT_INITIALIZED_DATA = 0x40,

        /// <summary>
        /// �ÿ����δ��ʼ��������
        /// </summary>
        IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x80,

        /// <summary>
        /// �ÿ�ɱ���������Ϊ��һ����װ��󣬽��̾Ͳ�����Ҫ���˳����Ŀɶ�������.reloc(�ض�λ��)
        /// </summary>
        IMAGE_SCN_MEM_DISCARDABLE = 0x2000000,

        /// <summary>
        /// �ÿ�Ϊ����
        /// </summary>
        IMAGE_SCN_MEM_SHARED = 0x10000000,

        /// <summary>
        /// �ÿ�Ϊ��ִ�С�ͨ����00000020H��־������ʱ���ñ�־Ҳ������
        /// </summary>
        IMAGE_SCN_MEM_EXECUTE = 0x20000000,

        /// <summary>
        /// �ÿ�ɶ�����ִ���ļ��еĿ��������øÿ��־
        /// </summary>
        IMAGE_SCN_MEM_READ = 0x40000000,

        /// <summary>
        /// �ÿ��д�������ִ���ļ�û�����øñ�־��װ�س���ͽ����ڴ�ӳ��ҳ���Ϊ�ɶ����ִ��
        /// </summary>
        IMAGE_SCN_MEM_WRITE = 0x80000000
    }
}
