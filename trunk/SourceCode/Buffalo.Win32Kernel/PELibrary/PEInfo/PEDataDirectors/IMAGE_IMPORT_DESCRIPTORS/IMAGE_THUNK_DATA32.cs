using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct IMAGE_THUNK_DATA32
    {
        [FieldOffset(0)]
        public uint ForwarderSting;
        [FieldOffset(0)]
        public uint Function;
        [FieldOffset(0)]
        public uint Ordinal;
        [FieldOffset(0)]
        public uint AddressOfData;


        
    }
}
