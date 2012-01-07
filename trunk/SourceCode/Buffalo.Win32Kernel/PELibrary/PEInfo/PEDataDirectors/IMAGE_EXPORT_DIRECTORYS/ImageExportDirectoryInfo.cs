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
        /// �����Ϣ
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
        /// ������Ϣ
        /// </summary>
        internal void LoadInfo()
        {
            imageExportInfo = ReadExportAddressTable();//������Ϣ����
            ExportInfo[] ent = FindExportNameTable(imageExportInfo);
            FillExportName(ent);
        }

        /// <summary>
        /// �ҳ�����ַ����ĺ�����Ϣ����
        /// </summary>
        /// <returns></returns>
        private ExportInfo[] ReadExportAddressTable()
        {
            ExportInfo[] eat = new ExportInfo[imageExportDirector.NumberOfFunctions];
            //��ȡ������������ĵ�ַ
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
        /// �ҳ�����������ĺ�����Ϣ����
        /// </summary>
        /// <returns></returns>
        private ExportInfo[] FindExportNameTable(ExportInfo[] eat)
        {
            //�ҳ�����������ĺ�����Ϣ
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
        /// ������������ĺ�����Ϣ�������
        /// </summary>
        /// <param name="eat"></param>
        private void FillExportName(ExportInfo[] eat)
        {
            

            //��ȡ���Ƶ�RVA
            pe.BaseStream.Position = pe.RVAToFileOffest(imageExportDirector.AddressOfNames);
            for (int i = 0; i < eat.Length; i++)
            {
                eat[i].NameRAV = pe.PeReader.ReadUInt32();
            }

            //��ʼ�������
            for (int i = 0; i < eat.Length; i++)
            {
                pe.BaseStream.Position = pe.RVAToFileOffest(eat[i].NameRAV);
                eat[i].Name = pe.PeReader.ReadString();
            }

        }
    }
}
