using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BASE_RELOCATIONS
{
    /// <summary>
    /// �ض�λ����
    /// </summary>
    public struct ImageBaseRelocationOffestType
    {
        public ushort offestValue;
        public uint VirtualAddress;
        const ushort highest = 0x1000;//���λ

        /// <summary>
        /// �ض�λ����
        /// </summary>
        public BaseRelocatioType RelocatioType 
        {
            get 
            {
                ushort type = (ushort)(offestValue / highest);//��ȡ���λ
                return (BaseRelocatioType)type;
            }
        }


        /// <summary>
        /// ��ַ
        /// </summary>
        public uint Address
        {
            get
            {
                if (RelocatioType == BaseRelocatioType.ImageRelBasedHighlow)
                {
                    ushort add = (ushort)(offestValue % highest);//��ȡ��Ե�ַ
                    return VirtualAddress + add;
                }
                return 0;
            }
        }


        /// <summary>
        /// �ض�λ��Ϣ
        /// </summary>
        /// <param name="offestValue">ֵ</param>
        /// <param name="virtualAddress">�ض�λ���RVA</param>
        public ImageBaseRelocationOffestType(ushort offestValue, uint virtualAddress) 
        {
            this.offestValue = offestValue;
            this.VirtualAddress = virtualAddress;
        }

    }
}
