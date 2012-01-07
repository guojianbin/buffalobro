using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IMPORT_DESCRIPTOR_MISC
    {
        [FieldOffset(0)]
        public uint Characteristics;
        [FieldOffset(0)]
        public uint OriginalFirstThunk;
    }
}
