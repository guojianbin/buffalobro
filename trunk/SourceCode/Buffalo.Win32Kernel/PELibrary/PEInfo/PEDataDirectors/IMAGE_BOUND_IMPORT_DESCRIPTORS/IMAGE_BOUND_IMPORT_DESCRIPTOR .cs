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
        /// 被输入dll的时间戳
        /// </summary>
        public uint TimeDateStamp;
        /// <summary>
        /// 指向dll的名称的偏移(跟第一个IBID)
        /// </summary>
        public ushort OffsetModuleName;
        /// <summary>
        /// 给出这个DLL文件为它的中转使用的DLL文件数
        /// </summary>
        public ushort NumberOfModuleForwarderRefs;
    }
}
