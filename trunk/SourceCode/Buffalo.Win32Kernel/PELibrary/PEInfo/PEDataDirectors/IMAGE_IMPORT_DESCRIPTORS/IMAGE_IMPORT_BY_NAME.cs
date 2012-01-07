using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS
{
    public struct IMAGE_IMPORT_BY_NAME
    {
        public ushort Hint;
        public string Name;


        public IMAGE_IMPORT_BY_NAME(PEHandle pe) 
        {

            Hint = pe.PeReader.ReadUInt16();
            Name = pe.PeReader.ReadString();
        }
    }
}
