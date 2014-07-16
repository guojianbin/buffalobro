using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    public struct IMAGE_DATA_DIRECTORY
    {
        /// <summary>
        /// ��ʼ��RAV
        /// </summary>
        public uint VirtualAddress;
        /// <summary>
        /// ���ݿ鳤��
        /// </summary>
        public uint Size;

        public override string ToString()
        {
            return "RAV:"+VirtualAddress+",Size:"+Size;
        }
    }
}
