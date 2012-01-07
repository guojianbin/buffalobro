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
        /// ��Դ����
        /// </summary>
        public ResourceType Type
        {
            get { return type; }
            set { type = value; }
        }

        private List<ResourceEntry> entrys = new List<ResourceEntry>();

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public List<ResourceEntry> Entrys
        {
            get { return entrys; }
        }


    }
}
