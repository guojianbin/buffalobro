using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    public struct IMAGE_DATA_DIRECTORY
    {
        /// <summary>
        /// 起始块RAV
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// 数据块长度
        /// </summary>
        public uint Size;

        public override string ToString()
        {
            return "RAV:"+VirtualAddress+",Size:"+Size;
        }
    }
}
