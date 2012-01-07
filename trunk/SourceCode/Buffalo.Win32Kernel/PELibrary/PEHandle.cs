using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel.PELibrary.PEInfo;
using System.IO;
using Buffalo.Kernel;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors;

namespace Buffalo.Win32Kernel.PELibrary
{
    public class PEHandle
    {
        IMAGE_DOS_HEADER dosHeader;
        IMAGE_NT_HEADERS ntHeader;
        IMAGE_SECTION_HEADER[] sectionHeaders;

        /// <summary>
        /// ����
        /// </summary>
        public IMAGE_SECTION_HEADER[] SectionHeaders
        {
            get { return sectionHeaders; }
        }
        /// <summary>
        /// DOSͷ
        /// </summary>
        public IMAGE_DOS_HEADER DosHeader
        {
            get { return dosHeader; }
        }
        /// <summary>
        /// NTͷ
        /// </summary>
        public IMAGE_NT_HEADERS NtHeader
        {
            get { return ntHeader; }
        }
        

        Stream stm;

        internal Stream BaseStream 
        {
            get 
            {
                return stm;
            }
        }

        ImageDataDirectorys _PEImageDataDirectorys;

        public ImageDataDirectorys PEImageDataDirectorys
        {
            get 
            {
                if (_PEImageDataDirectorys == null) 
                {
                    _PEImageDataDirectorys = new ImageDataDirectorys(this, ntHeader.OptionalHeader.DataDirectory);
                }
                return _PEImageDataDirectorys; 
            }
        }

        public PEHandle(Stream stm) 
        {
            this.stm = stm;
            dosHeader = CommonMethods.RawDeserialize<IMAGE_DOS_HEADER>(stm);
            LoadNTHead();
            LoadSectionHeader();
        }

        //private void Load

        /// <summary>
        /// ����NTͷ
        /// </summary>
        private void LoadNTHead() 
        {
            stm.Position = dosHeader.e_lfanew;
            ntHeader = CommonMethods.RawDeserialize<IMAGE_NT_HEADERS>(stm);
        }

        private void LoadSectionHeader() 
        {
            int secs = ntHeader.FileHeader.NumberOfSections;
            sectionHeaders = new IMAGE_SECTION_HEADER[secs];
            for (int i = 0; i < secs; i++)
            {
                sectionHeaders[i]=CommonMethods.RawDeserialize<IMAGE_SECTION_HEADER>(stm);
            }
        }

        /// <summary>
        /// �ҳ���ǰ����ƫ�����ڵ�����
        /// </summary>
        /// <returns></returns>
        public IMAGE_SECTION_HEADER FindSectionHeader(uint virtualAddress) 
        {
            //IMAGE_SECTION_HEADER header;
            for (int i = 0; i < sectionHeaders.Length; i++)
            {

                if (i >= sectionHeaders.Length - 1)
                {
                    return sectionHeaders[i];
                }
                else if (sectionHeaders[i].VirtualAddress <= virtualAddress && sectionHeaders[i + 1].VirtualAddress > virtualAddress)
                {
                    return sectionHeaders[i];
                }
            }
            return new IMAGE_SECTION_HEADER();
        }

        /// <summary>
        /// ����ƫ��ת�����ļ����λ��
        /// </summary>
        /// <param name="rav">�ڴ���Ե�ַ</param>
        /// <returns></returns>
        public uint RVAToFileOffest(uint rav)
        {
            uint imageBase = ntHeader.OptionalHeader.ImageBase;
            //uint offsetRav = rav - imageBase;
            //IMAGE_SECTION_HEADER belongHeader = null;
            //�ҳ�����������

            IMAGE_SECTION_HEADER sectionHead = FindSectionHeader(rav);

            if (!sectionHead.IsNull)
            {

                uint k = sectionHead.VirtualAddress - sectionHead.PointerToRawData;
                return rav - k;
            }
            return 0;
        }

        /// <summary>
        /// �����ַת�����ļ����λ��
        /// </summary>
        /// <param name="rav">�ڴ���Ե�ַ</param>
        /// <returns></returns>
        public uint VaToFileOffest(uint rav)
        {
            uint imageBase = NtHeader.OptionalHeader.ImageBase;
            uint offsetRav = rav - imageBase;
            IMAGE_SECTION_HEADER sectionHead = FindSectionHeader(offsetRav);


            if (!sectionHead.IsNull)
            {
                uint k = sectionHead.VirtualAddress - sectionHead.PointerToRawData;
                return rav - imageBase - k;
            }
            return 0;
        }

        #region ��ȡ����
        private BinaryReader _peReader;

        public BinaryReader PeReader
        {
            get 
            {
                if (_peReader == null) 
                {
                    _peReader = new BinaryReader(stm);
                }
                return _peReader; 
            }
            
        }
        
        #endregion
    }
}
