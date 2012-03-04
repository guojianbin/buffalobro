using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 生成数据层
    /// </summary>
    public class Generate3Tier : GrneraterBase
    {
        DataAccessMappingConfig dmt = null;
        
        public Generate3Tier(EntityConfig entity) 
            :base(entity)
        {
            dmt = new DataAccessMappingConfig(entity);
        }

        public Generate3Tier(DBEntityInfo entity,Project project)
            : base(entity, project)
        {
            dmt = new DataAccessMappingConfig(entity,project);
        }

        /// <summary>
        /// 生成业务层
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateBusiness() 
        {
            FileInfo info = new FileInfo(EntityFileName);
            

            string dicPath = info.DirectoryName + "\\Business";
            if (!Directory.Exists(dicPath)) 
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + ClassName + "Business.cs";
            if (File.Exists(fileName)) 
            {
                return;
            }


            string model = Models.Business;
            
            string baseClass = null;
            
            string businessClassName=ClassName+"Business";


            if (!string.IsNullOrEmpty(Table.TableName))
            {
                if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
                {
                    baseClass = "BusinessModelBase<" + ClassName + ">";
                }
                else
                {
                    baseClass = BusinessNamespace + "." + EntityBaseTypeShortName + "Business<" + ClassName + ">";
                }
            }
            else 
            {
                if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
                {
                    baseClass = "BusinessModelBase<T> where T : " + ClassName + ", new()";
                }
                else
                {
                    baseClass = BusinessNamespace+"."+EntityBaseTypeShortName + "Business<T> where T : " + ClassName + ", new()";
                }
                
                businessClassName += "<T>";
            }

            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=BusinessClassName%>", businessClassName);
                    tmp = tmp.Replace("<%=ClassName%>",ClassName);
                    tmp = tmp.Replace("<%=BusinessNamespace%>", BusinessNamespace);
                    tmp = tmp.Replace("<%=BaseBusinessClass%>", baseClass);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

        /// <summary>
        /// 数据层的类型
        /// </summary>
        public static readonly ComboBoxItemCollection DataAccessTypes = InitItems();

        /// <summary>
        /// 初始化数据库类型
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitItems() 
        {
            ComboBoxItemCollection types = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("SQL Server 2000", "Sql2K");
            types.Add(item);
            item = new ComboBoxItem("SQL Server 2005 或以上", "Sql2K5");
            types.Add(item);
            item = new ComboBoxItem("Oracle 9 或以上", "Oracle9");
            types.Add(item);
            item = new ComboBoxItem("MySQL 5.0 或以上", "MySQL5");
            types.Add(item);
            item = new ComboBoxItem("SQLite", "SQLite");
            types.Add(item);
            item = new ComboBoxItem("IBM DB2 v9或以上", "DB2v9");
            types.Add(item);
            return types;
        }

        public static readonly ComboBoxItemCollection Tiers = InitTiers();
        /// <summary>
        /// 初始化架构层数
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitTiers()
        {
            ComboBoxItemCollection tiers = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("三层架构", 3);
            tiers.Add(item);
            item = new ComboBoxItem("单层架构", 1);
            tiers.Add(item);

            return tiers;
        }


        /// <summary>
        /// 生成数据层
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDataAccess()
        {
            FileInfo info = new FileInfo(EntityFileName);


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
                string fileName = dalPath + "\\" + ClassName + "DataAccess.cs";
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
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                        tmp = tmp.Replace("<%=DataBaseType%>", type);
                        tmp = tmp.Replace("<%=ClassName%>", ClassName);
                        codes.Add(tmp);
                    }
                }
                dmt.AppendDal(DataAccessNamespace + "." + type + "." + ClassName + "DataAccess", DataAccessNamespace + ".IDataAccess.I" + ClassName + "DataAccess");
                CodeFileHelper.SaveFile(fileName, codes);
                EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
                newit.Properties.Item("BuildAction").Value = 1;
            }
            dmt.SaveXML();
        }
        /// <summary>
        /// 生成IDataAccess
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateIDataAccess() 
        {
            FileInfo info = new FileInfo(EntityFileName);
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
            string fileName = idalPath + "\\I" + ClassName + "DataAccess.cs";
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
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

        /// <summary>
        /// 生成BQL数据层
        /// </summary>
        public void GenerateBQLDataAccess() 
        {
            FileInfo info = new FileInfo(EntityFileName);
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
            string fileName = idalPath + "\\" + ClassName + "DataAccess.cs";
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
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=DataBaseType%>", type);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

    }
}
