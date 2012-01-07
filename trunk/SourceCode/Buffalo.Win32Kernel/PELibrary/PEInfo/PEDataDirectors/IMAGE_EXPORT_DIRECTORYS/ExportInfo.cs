using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_EXPORT_DIRECTORYS
{
    /// <summary>
    /// ��������Ϣ
    /// </summary>
    public class ExportInfo
    {
        uint address;

        /// <summary>
        /// ������ַ
        /// </summary>
        public uint Address
        {
            get { return address; }
            set { address = value; }
        }
        string name;

        /// <summary>
        /// ������
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private uint nameRAV;

        /// <summary>
        /// ���Ƶ�RAV
        /// </summary>
        public uint NameRAV
        {
            get { return nameRAV; }
            set { nameRAV = value; }
        }

    }
}
