using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BOUND_IMPORT_DESCRIPTORS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_BOUND_IMPORT_DESCRIPTOR 
    {
        /// <summary>
        /// ������dll��ʱ���
        /// </summary>
        public uint TimeDateStamp;
        /// <summary>
        /// ָ��dll�����Ƶ�ƫ��(����һ��IBID)
        /// </summary>
        public ushort OffsetModuleName;
        /// <summary>
        /// �������DLL�ļ�Ϊ������תʹ�õ�DLL�ļ���
        /// </summary>
        public ushort NumberOfModuleForwarderRefs;
    }
}
