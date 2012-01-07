using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    public struct IMAGE_NT_HEADERS
    {
        /// <summary>
        ///  PE文件头标志:"PE\0\0"
        /// </summary>
        public uint Signature;
        /// <summary>
        /// PE文件物理分布的信息
        /// </summary>
        public IMAGE_FILE_HEADER FileHeader;
        /// <summary>
        /// PE文件逻辑分布的信息
        /// </summary>
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
    }
}
