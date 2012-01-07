using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.Enums
{
    /// <summary>
    /// �ض�λ����
    /// </summary>
    [Flags]
    public enum BaseRelocatioType : ushort
    {
        /// <summary>
        /// Image_Rel_Based_Absolute û�к���Ϊ����4λ�ֽڶ���
        /// </summary>
        ImageRelBasedAbsolute = 0,

        /// <summary>
        /// Image_Rel_Based_Highlow �ض�λָ���������ַ��������
        /// </summary>
        ImageRelBasedHighlow = 3,

        /// <summary>
        /// Image_Rel_Based_Dir64 ������64λPE�ļ��У���ָ���������ַ����
        /// </summary>
        ImageRelBasedDir64 = 10
    }
}
