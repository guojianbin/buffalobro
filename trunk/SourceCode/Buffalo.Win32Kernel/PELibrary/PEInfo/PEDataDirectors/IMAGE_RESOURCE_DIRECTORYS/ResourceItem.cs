using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.PELibrary.Enums;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_RESOURCE_DIRECTORYS
{
    public class ResourceItem
    {
        ResourceType type;

        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceType Type
        {
            get { return type; }
            set { type = value; }
        }

        private List<ResourceEntry> entrys = new List<ResourceEntry>();

        /// <summary>
        /// 信息集合
        /// </summary>
        public List<ResourceEntry> Entrys
        {
            get { return entrys; }
        }


    }
}
