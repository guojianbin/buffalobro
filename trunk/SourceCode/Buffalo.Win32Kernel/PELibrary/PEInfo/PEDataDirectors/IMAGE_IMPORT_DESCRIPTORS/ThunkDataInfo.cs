using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS
{
    public class ThunkDataInfo
    {
        private IMAGE_THUNK_DATA32 thunkData;

        public IMAGE_THUNK_DATA32 ThunkData
        {
            get { return thunkData; }
            set { thunkData = value; }
        }

        private IMAGE_IMPORT_BY_NAME nameInfo;

        public IMAGE_IMPORT_BY_NAME NameInfo
        {
            get { return nameInfo; }
            set { nameInfo = value; }
        }

        PEHandle pe;

        public ThunkDataInfo(PEHandle pe) 
        {
            this.pe = pe;

            thunkData = CommonMethods.RawDeserialize<IMAGE_THUNK_DATA32>(pe.BaseStream);
        }
        /// <summary>
        /// 是否已经为空值
        /// </summary>
        internal bool IsEmpty
        {
            get
            {
                return thunkData.AddressOfData == 0;
            }
        }
        public void LoadInfo() 
        {
            uint dataValue = thunkData.ForwarderSting;
            //uint i = 2147483648;//1000 0000 0000 0000 0000 0000 0000 0000
            uint res = dataValue >> 31;
            if (res ==0 ) //如果最高位为0则此值为指向ImageImportByName的RAV
            {
                uint pos = pe.RVAToFileOffest(dataValue);
                if (pos >= pe.BaseStream.Length)
                {
                    return;
                }
                pe.BaseStream.Position = pos;
                nameInfo = new IMAGE_IMPORT_BY_NAME(pe);
                
            }
        }
    }
}
