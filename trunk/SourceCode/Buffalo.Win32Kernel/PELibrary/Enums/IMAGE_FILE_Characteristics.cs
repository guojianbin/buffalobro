using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    [Flags]
    public enum IMAGE_FILE_Characteristics:ushort
    {
        /// <summary>
        /// �ļ��������ض�λ��Ϣ
        /// </summary>
        NoRelocating = 0x0001,

        /// <summary>
        /// �ļ���ִ�У����Ϊ0���������ʱ���������
        /// </summary>
        Execute = 0x0002,

        /// <summary>
        /// �к���Ϣ����ȥ
        /// </summary>
        LinenumberRemoved = 0x0004,

        /// <summary>
        /// ������Ϣ����ȥ
        /// </summary>
        SymbolRemoved = 0x0008,

        /// <summary>
        /// Ӧ�ó����ַ����2GB��ַ
        /// </summary>
        Flat = 0x0020,

        /// <summary>
        /// ������ĵ��ֽ�λ�෴
        /// </summary>
        LowBitByteInstead = 0x0080,

        /// <summary>
        /// Ŀ��ƽ̨��32λ����
        /// </summary>
        Platform32Bit = 0x0100,

        /// <summary>
        /// DBG�ļ�������Ϣ����ȥ
        /// </summary>
        DBGRemoved = 0x0200,

        /// <summary>
        /// ���ӳ���ļ��ڿ��ƶ��豸�У����ȸ��Ƶ������ļ���������
        /// </summary>
        CopyFromMobile = 0x0400,

        /// <summary>
        /// ���ӳ���ļ��������У����ȸ��Ƶ������ļ���������
        /// </summary>
        CopyFromNetwork = 0x0800,

        /// <summary>
        /// ϵͳ�ļ�
        /// </summary>
        SystemFile = 0x1000,

        /// <summary>
        /// Dll�ļ�
        /// </summary>
        DllFile = 0x2000,

        /// <summary>
        /// �ļ�ֻ�������ڵ���������
        /// </summary>
        SingleProcessor = 0x4000,

        /// <summary>
        /// ������ĸ�λ�ֽ����෴��
        /// </summary>
        HighBitByteInstead = 0x8000
    }
}
