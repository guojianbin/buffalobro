using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BOUND_IMPORT_DESCRIPTORS;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_IMPORT_DESCRIPTORS;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_EXPORT_DIRECTORYS;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_BASE_RELOCATIONS;
using Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors.IMAGE_RESOURCE_DIRECTORYS;

namespace Buffalo.Win32Kernel.PELibrary.PEInfo.PEDataDirectors
{
    public class ImageDataDirectorys
    {
        PEHandle pe;
        IMAGE_DATA_DIRECTORY[] dataDirectorys;

        public IMAGE_DATA_DIRECTORY[] DataDirectorys
        {
            get { return dataDirectorys; }
        }

        /// <summary>
        /// 加载数据目录表信息
        /// </summary>
        /// <param name="pe">pe信息</param>
        /// <param name="stm">流</param>
        /// <param name="dataDirectorys">数据目录表</param>
        public ImageDataDirectorys(PEHandle pe, IMAGE_DATA_DIRECTORY[] dataDirectorys) 
        {
            this.pe = pe;
            this.dataDirectorys = dataDirectorys;
        }

        #region IMAGE_BOUND_IMPORT_DESCRIPTOR

        private List<BoundImportInfo> boundImportDescriptors;

        public List<BoundImportInfo> BoundImportDescriptors
        {
            get 
            {
                if (boundImportDescriptors == null) 
                {
                    LoadBoundImportDescriptors();
                }
                return boundImportDescriptors; 
            }
            set { boundImportDescriptors = value; }
        }

        /// <summary>
        /// 加载输入表
        /// </summary>
        private void LoadBoundImportDescriptors()
        {
            if (dataDirectorys[10].VirtualAddress <= 0)
            {
                return;
            }
            uint fOffest=pe.RVAToFileOffest(dataDirectorys[1].VirtualAddress);
            pe.BaseStream.Position = fOffest;
            boundImportDescriptors = new List<BoundImportInfo>();
            while (true)
            {
                BoundImportInfo ibid = new BoundImportInfo(pe);
                if (!ibid.IsNull)
                {
                    boundImportDescriptors.Add(ibid);
                }
                else
                {
                    break;
                }
            }
            

            //读入表名称
            foreach (BoundImportInfo ibid in boundImportDescriptors)
            {
                ibid.LoadInfo(fOffest);
            }
        }


        #endregion

        #region IMAGE_IMPORT_DESCRIPTOR
        private List<ImportDescriptorInfo> importDescriptors;

        /// <summary>
        /// 输入表信息
        /// </summary>
        public List<ImportDescriptorInfo> ImportDescriptors
        {
            get 
            {
                if (importDescriptors == null) 
                {
                    LoadImportDescriptors();

                }
                return importDescriptors; 
            }
            set { importDescriptors = value; }
        }

        /// <summary>
        /// 加载输入表
        /// </summary>
        private void LoadImportDescriptors() 
        {
            if (dataDirectorys[1].VirtualAddress <= 0)
            {
                return;
            }

            pe.BaseStream.Position = pe.RVAToFileOffest(dataDirectorys[1].VirtualAddress);
            importDescriptors = new List<ImportDescriptorInfo>();
            while (true)
            {
                ImportDescriptorInfo iid = new ImportDescriptorInfo(pe);
                if (!iid.ImportDescriptor.IsNull)
                {
                    importDescriptors.Add(iid);
                }
                else
                {
                    break;
                }
                
            }
            //读入表名称
            foreach (ImportDescriptorInfo iid in importDescriptors)
            {
                iid.LoadInfo();
            }
        }
        #endregion

        #region IMAGE_EXPORT_DIRECTORY
        ImageExportDirectoryInfo imageExportDirectory;
        /// <summary>
        /// 输出表信息
        /// </summary>
        public ImageExportDirectoryInfo ImageExportDirectory
        {
            get
            {
                if (imageExportDirectory == null)
                {
                    LoadExportDescriptors();

                }
                return imageExportDirectory;
            }
            set { imageExportDirectory = value; }
        }

        /// <summary>
        /// 加载输出表
        /// </summary>
        private void LoadExportDescriptors()
        {
            if (dataDirectorys[0].VirtualAddress <= 0)
            {
                return;
            }

            pe.BaseStream.Position = pe.RVAToFileOffest(dataDirectorys[0].VirtualAddress);
            imageExportDirectory = new ImageExportDirectoryInfo(pe);

            imageExportDirectory.LoadInfo();
        }

        #endregion


        #region IMAGE_BASE_RELOCATION

        private List<IMAGE_BASE_RELOCATION> baseRelocation;

        /// <summary>
        /// 基址重定位表
        /// </summary>
        public List<IMAGE_BASE_RELOCATION> BaseRelocation
        {
            get
            {
                if (baseRelocation == null)
                {
                    LoadBaseRelocation();

                }
                return baseRelocation;
            }
            set { baseRelocation = value; }
        }

        /// <summary>
        /// 加载基址重定位
        /// </summary>
        private void LoadBaseRelocation()
        {
            if (dataDirectorys[5].VirtualAddress <= 0)
            {
                return;
            }
            pe.BaseStream.Position = pe.RVAToFileOffest(dataDirectorys[5].VirtualAddress);
            baseRelocation = new List<IMAGE_BASE_RELOCATION>();
            while (true)
            {
                IMAGE_BASE_RELOCATION ibr = new IMAGE_BASE_RELOCATION(pe);
                if (!ibr.IsNull)
                {
                    baseRelocation.Add(ibr);
                }
                else
                {
                    break;
                }
            }
        }

        #endregion

        #region IMAGE_RESOURCE_DIRECTORYS
        private ImageResourceManager resourceDirector;

        /// <summary>
        /// 资源表
        /// </summary>
        public ImageResourceManager ResourceDirector
        {
            get
            {
                if (resourceDirector == null)
                {
                    LoadResourceDirector();

                }
                return resourceDirector;
            }
            set { resourceDirector = value; }
        }

         /// <summary>
        /// 把此位置的表加载成资源表
        /// </summary>
        public ImageResourceManager LoadToResourceDirector(int index) 
        {
            if (dataDirectorys[index].VirtualAddress <= 0)
            {
                return null;
            }
            uint baseOffest = pe.RVAToFileOffest(dataDirectorys[index].VirtualAddress);
            pe.BaseStream.Position = baseOffest;
            return new ImageResourceManager(pe, baseOffest);
        }


        /// <summary>
        /// 加载资源表
        /// </summary>
        private void LoadResourceDirector()
        {
            if (dataDirectorys[2].VirtualAddress <= 0)
            {
                return;
            }
            uint baseOffest=pe.RVAToFileOffest(dataDirectorys[2].VirtualAddress);
            pe.BaseStream.Position = baseOffest;
            resourceDirector = new ImageResourceManager(pe, baseOffest);
            
        }
        #endregion

    }
}
