using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        
        [FieldOffset(0)]
        public uint Characteristics;
        /// <summary>
        /// ָ������������Ʊ�(INT)��RVA ����������
        /// </summary>
        [FieldOffset(0)]
        public uint OriginalFirstThunk;
        /// <summary>
        /// 32λʱ����
        /// </summary>
        [FieldOffset(4)]
        public uint TimeDateStamp;
        /// <summary>
        /// ��һ����ת���API������һ��Ϊ0
        /// </summary>
        [FieldOffset(8)]
        public uint ForwarderChain;
        /// <summary>
        /// dll���ֵ�ָ��(��00��β��RAV��ַ)
        /// </summary>
        [FieldOffset(12)]
        public uint Name;
        /// <summary>
        /// ָ�������ַ���Ʊ�(IAT)��RVA ����������
        /// </summary>
        [FieldOffset(16)]
        public uint FirstThunk;

        public bool IsNull 
        {
            get
            {
                return OriginalFirstThunk == 0 && TimeDateStamp == 0 && ForwarderChain == 0 && Name == 0 && FirstThunk == 0;
            }
        }

        


        
    }
}
