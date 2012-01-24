using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// �������ݲ�
    /// </summary>
    public class Generate3Tier : GrneraterBase
    {
        
        
        public Generate3Tier(EntityConfig entity) 
            :base(entity)
        {
            
        }



        /// <summary>
        /// ����ҵ���
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateBusiness() 
        {
            FileInfo info = new FileInfo(_entity.FileName);
            

            string dicPath = info.DirectoryName + "\\Business";
            if (!Directory.Exists(dicPath)) 
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + _entity.ClassName + "Business.cs";
            if (File.Exists(fileName)) 
            {
                return;
            }


            string model = Models.Business;
            
            string baseClass = null;
            ClrType baseType=_entity.BaseType;
            if (EntityConfig.IsSystemType(baseType))
            {
                baseClass = "BusinessModelBase<"+ClassName+">";
            }
            else 
            {
                

                baseClass = BusinessNamespace+"." + baseType.Name+"Business";
            }
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Summary);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=BusinessNamespace%>", BusinessNamespace);
                    tmp = tmp.Replace("<%=BaseBusinessClass%>", baseClass);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = _entity.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

        /// <summary>
        /// ���ݲ������
        /// </summary>
        public static readonly ComboBoxItemCollection DataAccessTypes = InitItems();

        /// <summary>
        /// ��ʼ�����ݿ�����
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitItems() 
        {
            ComboBoxItemCollection types = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("SQL Server 2000", "Sql2K");
            types.Add(item);
            item = new ComboBoxItem("SQL Server 2005 ������", "Sql2K5");
            types.Add(item);
            item = new ComboBoxItem("Oracle 9 ������", "Oracle9");
            types.Add(item);
            item = new ComboBoxItem("MySQL 5.0 ������", "MySQL5");
            types.Add(item);
            item = new ComboBoxItem("SQLite", "SQLite");
            types.Add(item);
            item = new ComboBoxItem("IBM DB2 v9������", "DB2v9");
            types.Add(item);
            return types;
        }

        public static readonly ComboBoxItemCollection Tiers = InitTiers();
        /// <summary>
        /// ��ʼ���ܹ�����
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitTiers()
        {
            ComboBoxItemCollection tiers = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("����ܹ�", 3);
            tiers.Add(item);
            item = new ComboBoxItem("����ܹ�", 1);
            tiers.Add(item);

            return tiers;
        }


        /// <summary>
        /// �������ݲ�
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDataAccess()
        {
            FileInfo info = new FileInfo(_entity.FileName);


            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string dal = Models.DataAccess;
            foreach (ComboBoxItem itype in DataAccessTypes) 
            {
                string type = itype.Value.ToString();
                string dalPath = dicPath + "\\" + type;
                if (!Directory.Exists(dalPath))
                {
                    Directory.CreateDirectory(dalPath);
                }
                string fileName = dalPath + "\\" + _entity.ClassName + "DataAccess.cs";
                if (File.Exists(fileName))
                {
                    continue;
                }

                List<string> codes = new List<string>();
                using (StringReader reader = new StringReader(dal))
                {
                    string tmp = null;
                    while ((tmp = reader.ReadLine()) != null)
                    {
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Summary);
                        tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                        tmp = tmp.Replace("<%=DataBaseType%>", type);
                        tmp = tmp.Replace("<%=ClassName%>", ClassName);
                        codes.Add(tmp);
                    }
                }
                CodeFileHelper.SaveFile(fileName, codes);
                EnvDTE.ProjectItem newit = _entity.CurrentProject.ProjectItems.AddFromFile(fileName);
                newit.Properties.Item("BuildAction").Value = 1;
            }
            
        }
        /// <summary>
        /// ����IDataAccess
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateIDataAccess() 
        {
            FileInfo info = new FileInfo(_entity.FileName);
            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string idalPath = dicPath + "\\IDataAccess";
            if (!Directory.Exists(idalPath))
            {
                Directory.CreateDirectory(idalPath);
            }
            string fileName = idalPath + "\\I" + _entity.ClassName + "DataAccess.cs";
            if (File.Exists(fileName))
            {
                return;
            }
            string idal = Models.IDataAccess;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Summary);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = _entity.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

        /// <summary>
        /// ����BQL���ݲ�
        /// </summary>
        public void GenerateBQLDataAccess() 
        {
            FileInfo info = new FileInfo(_entity.FileName);
            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string type = "Bql";
            string idalPath = dicPath + "\\" + type;
            if (!Directory.Exists(idalPath))
            {
                Directory.CreateDirectory(idalPath);
            }
            string fileName = idalPath + "\\" + _entity.ClassName + "DataAccess.cs";
            if (File.Exists(fileName))
            {
                return;
            }
            string idal = Models.BQLDataAccess;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Summary);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=DataBaseType%>", type);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = _entity.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

    }
}
