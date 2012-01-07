using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BOUND_IMPORT_DESCRIPTORS
{
    public class BoundImportInfo
    {
        private PEHandle pe;

        private IMAGE_BOUND_IMPORT_DESCRIPTOR boundImport;

        public IMAGE_BOUND_IMPORT_DESCRIPTOR BoundImport
        {
            get { return boundImport; }
            set { boundImport = value; }
        }
        private string moduleName;

        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
        
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        public bool IsNull
        {
            get
            {
                return boundImport.TimeDateStamp == 0 && boundImport.OffsetModuleName == 0 && boundImport.NumberOfModuleForwarderRefs == 0;
            }
        }

        public BoundImportInfo(PEHandle pe) 
        {
            //uint offest = pe.RVAToFileOffest(dataDir.VirtualAddress);
            //pe.BaseStream.Position = offest;
            boundImport = CommonMethods.RawDeserialize<IMAGE_BOUND_IMPORT_DESCRIPTOR>(pe.BaseStream);
            this.pe = pe;
            
        }
        /// <summary>
        /// 加载绑定表的信息
        /// </summary>
        /// <param name="fristIBIDOffest">第一个结构体的文件偏移</param>
        internal void LoadInfo(uint fristIBIDOffest) 
        {

            pe.BaseStream.Position = fristIBIDOffest + boundImport.OffsetModuleName;
            moduleName = pe.PeReader.ReadString();
        
        }
    }
}
