using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo
{
    public struct IMAGE_NT_HEADERS
    {
        /// <summary>
        ///  PE�ļ�ͷ��־:"PE\0\0"
        /// </summary>
        public uint Signature;
        /// <summary>
        /// PE�ļ�����ֲ�����Ϣ
        /// </summary>
        public IMAGE_FILE_HEADER FileHeader;
        /// <summary>
        /// PE�ļ��߼��ֲ�����Ϣ
        /// </summary>
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
    }
}
