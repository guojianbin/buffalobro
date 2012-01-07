using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BASE_RELOCATIONS
{
    /// <summary>
    /// 重定位类型
    /// </summary>
    public struct ImageBaseRelocationOffestType
    {
        public ushort offestValue;
        public uint VirtualAddress;
        const ushort highest = 0x1000;//最高位

        /// <summary>
        /// 重定位类型
        /// </summary>
        public BaseRelocatioType RelocatioType 
        {
            get 
            {
                ushort type = (ushort)(offestValue / highest);//截取最高位
                return (BaseRelocatioType)type;
            }
        }


        /// <summary>
        /// 地址
        /// </summary>
        public uint Address
        {
            get
            {
                if (RelocatioType == BaseRelocatioType.ImageRelBasedHighlow)
                {
                    ushort add = (ushort)(offestValue % highest);//截取相对地址
                    return VirtualAddress + add;
                }
                return 0;
            }
        }


        /// <summary>
        /// 重定位信息
        /// </summary>
        /// <param name="offestValue">值</param>
        /// <param name="virtualAddress">重定位表的RVA</param>
        public ImageBaseRelocationOffestType(ushort offestValue, uint virtualAddress) 
        {
            this.offestValue = offestValue;
            this.VirtualAddress = virtualAddress;
        }

    }
}
