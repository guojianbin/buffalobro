using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_EXPORT_DIRECTORYS
{
    public class ImageExportDirectoryInfo
    {
        private IMAGE_EXPORT_DIRECTORY imageExportDirector;


        public IMAGE_EXPORT_DIRECTORY ImageExportDirector
        {
            get { return imageExportDirector; }
            set { imageExportDirector = value; }
        }

        ExportInfo[] imageExportInfo;
        /// <summary>
        /// 输出信息
        /// </summary>
        public ExportInfo[] ImageExportInfo
        {
            get { return imageExportInfo; }
            set { imageExportInfo = value; }
        }


        private PEHandle pe;

        public ImageExportDirectoryInfo(PEHandle pe) 
        {
            //uint offest = pe.RVAToFileOffest(dataDir.VirtualAddress);
            //pe.BaseStream.Position = offest;
            imageExportDirector = CommonMethods.RawDeserialize<IMAGE_EXPORT_DIRECTORY>(pe.BaseStream);
            this.pe = pe;
            
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        internal void LoadInfo()
        {
            imageExportInfo = ReadExportAddressTable();//创建信息数组
            ExportInfo[] ent = FindExportNameTable(imageExportInfo);
            FillExportName(ent);
        }

        /// <summary>
        /// 找出按地址输出的函数信息集合
        /// </summary>
        /// <returns></returns>
        private ExportInfo[] ReadExportAddressTable()
        {
            ExportInfo[] eat = new ExportInfo[imageExportDirector.NumberOfFunctions];
            //读取所有输出函数的地址
            pe.BaseStream.Position = pe.RVAToFileOffest(imageExportDirector.AddressOfFunctions);
            for (int i = 0; i < eat.Length; i++)
            {
                ExportInfo eInfo = new ExportInfo();
                
                eInfo.Address = pe.PeReader.ReadUInt32();

                eat[i] = eInfo;
            }
            return eat;
        }

        /// <summary>
        /// 找出按名字输出的函数信息集合
        /// </summary>
        /// <returns></returns>
        private ExportInfo[] FindExportNameTable(ExportInfo[] eat)
        {
            //找出按名字输出的函数信息
            ExportInfo[] ent = new ExportInfo[imageExportDirector.NumberOfNames];
            pe.BaseStream.Position = pe.RVAToFileOffest(imageExportDirector.AddressOfNameOrdinals);
            for (int i = 0; i < ent.Length; i++)
            {
                ushort index = pe.PeReader.ReadUInt16();

                ent[i] = eat[index];
            }
            return ent;
        }

        /// <summary>
        /// 给按名称输出的函数信息填充名字
        /// </summary>
        /// <param name="eat"></param>
        private void FillExportName(ExportInfo[] eat)
        {
            

            //读取名称的RVA
            pe.BaseStream.Position = pe.RVAToFileOffest(imageExportDirector.AddressOfNames);
            for (int i = 0; i < eat.Length; i++)
            {
                eat[i].NameRAV = pe.PeReader.ReadUInt32();
            }

            //开始填充名字
            for (int i = 0; i < eat.Length; i++)
            {
                pe.BaseStream.Position = pe.RVAToFileOffest(eat[i].NameRAV);
                eat[i].Name = pe.PeReader.ReadString();
            }

        }
    }
}
