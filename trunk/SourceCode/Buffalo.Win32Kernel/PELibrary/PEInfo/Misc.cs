using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Misc
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public uint PhysicalAddress;
        [FieldOffset(0)]
        public uint VirtualSize;
    };
}
