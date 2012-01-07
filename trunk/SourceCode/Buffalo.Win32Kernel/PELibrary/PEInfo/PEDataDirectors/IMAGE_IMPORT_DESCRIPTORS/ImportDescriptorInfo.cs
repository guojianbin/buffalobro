using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS
{
    public class ImportDescriptorInfo
    {
        private IMAGE_IMPORT_DESCRIPTOR importDescriptor;

        /// <summary>
        /// 输入表信息
        /// </summary>
        public IMAGE_IMPORT_DESCRIPTOR ImportDescriptor
        {
            get { return importDescriptor; }
            set { importDescriptor = value; }
        }
        
        private ThunkDataInfo[] iatInfos;

        /// <summary>
        /// 地址名称表(IAT)数组
        /// </summary>
        public ThunkDataInfo[] IatInfos
        {
            get { return iatInfos; }
            set { iatInfos = value; }
        }

        private ThunkDataInfo[] intInfos;
        /// <summary>
        /// 输入名称表(INT)数组
        /// </summary>
        public ThunkDataInfo[] IntInfos
        {
            get { return intInfos; }
            set { intInfos = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private PEHandle pe;
        

        public ImportDescriptorInfo(PEHandle pe) 
        {
            //uint offest = pe.RVAToFileOffest(dataDir.VirtualAddress);
            //pe.BaseStream.Position = offest;
            importDescriptor = CommonMethods.RawDeserialize<IMAGE_IMPORT_DESCRIPTOR>(pe.BaseStream);
            this.pe = pe;
            
        }

        public void LoadInfo() 
        {
            SetDescriptorName();
            LoadInputAddressThunk();
            LoadInputNameThunk();
            
        }

        /// <summary>
        /// 设置dll名称的值
        /// </summary>
        /// <param name="nameValue"></param>
        private void SetDescriptorName()
        {
            pe.BaseStream.Position = pe.RVAToFileOffest(importDescriptor.Name);
            this.name = pe.PeReader.ReadString();
        }


        /// <summary>
        /// 加载输入地址表(IAT)
        /// </summary>
        private void LoadInputAddressThunk()
        {
            LoadThunk(importDescriptor.FirstThunk);
        }

        /// <summary>
        /// 加载输入名称表(INT)
        /// </summary>
        private void LoadInputNameThunk()
        {
            LoadThunk(importDescriptor.OriginalFirstThunk);
        }

        /// <summary>
        /// 加载输入表
        /// </summary>
        internal void LoadThunk(uint rav)
        {
            if (rav <= 0)
            {
                return;
            }
            pe.BaseStream.Position = pe.RVAToFileOffest(rav);
            List<ThunkDataInfo> lstItd = new List<ThunkDataInfo>();
            while (true)
            {
                ThunkDataInfo itd = new ThunkDataInfo(pe);
                if (itd.IsEmpty) //判断是否已经结束
                {
                    break;
                }
                lstItd.Add(itd);
            }
            ThunkDataInfo[] ret = lstItd.ToArray();

            //加载名称
            foreach (ThunkDataInfo itd in lstItd)
            {
                itd.LoadInfo();
            }
        }


    }
}
