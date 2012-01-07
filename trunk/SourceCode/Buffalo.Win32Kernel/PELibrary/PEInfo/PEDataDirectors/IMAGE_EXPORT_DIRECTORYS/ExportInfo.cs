using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_EXPORT_DIRECTORYS
{
    /// <summary>
    /// 输出表的信息
    /// </summary>
    public class ExportInfo
    {
        uint address;

        /// <summary>
        /// 函数地址
        /// </summary>
        public uint Address
        {
            get { return address; }
            set { address = value; }
        }
        string name;

        /// <summary>
        /// 函数名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private uint nameRAV;

        /// <summary>
        /// 名称的RAV
        /// </summary>
        public uint NameRAV
        {
            get { return nameRAV; }
            set { nameRAV = value; }
        }

    }
}
