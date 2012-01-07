using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BASE_RELOCATIONS
{
    /// <summary>
    /// 基址重定位
    /// </summary>
    public struct IMAGE_BASE_RELOCATION
    {
        public uint VirtualAddress;
        public uint SizeOfBlock;
        public ImageBaseRelocationOffestType[] TypeOffest;

        /// <summary>
        /// 判断此结构是否为空
        /// </summary>
        internal bool IsNull
        {
            get
            {
                return VirtualAddress == 0;
            }
        }
        
        public IMAGE_BASE_RELOCATION(PEHandle pe) 
        {
            VirtualAddress = pe.PeReader.ReadUInt32();
            SizeOfBlock = pe.PeReader.ReadUInt32();
            TypeOffest = null;
            if (IsNull)
            {
                return;
            }
            uint length = (SizeOfBlock - 8) / 2;
            
            if (length > 0)
            {
                TypeOffest = new ImageBaseRelocationOffestType[length];
                for (int i = 0; i < TypeOffest.Length; i++)
                {
                    TypeOffest[i] = new ImageBaseRelocationOffestType(pe.PeReader.ReadUInt16(), VirtualAddress);
                }
            }
            
        }

    }
}
