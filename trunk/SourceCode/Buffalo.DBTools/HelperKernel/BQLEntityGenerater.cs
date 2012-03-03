using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// BQL实体生成
    /// </summary>
    public class BQLEntityGenerater:GrneraterBase
    {
        public BQLEntityGenerater(DBEntityInfo config, Project project)
            : base(config, project)
        {
        } 

        public BQLEntityGenerater(EntityConfig config) :base(config)
        {
        }

        /// <summary>
        /// 生成DB声明
        /// </summary>
        public void GenerateBQLEntityDB() 
        {
            FileInfo info = new FileInfo(EntityFileName);
            string dicPath = info.DirectoryName + "\\BQLEntity";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + DBName + ".cs";
            
            string model = Models.BQLDB;

            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    tmp = tmp.Replace("<%=DBName%>", DBName);
                    
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

        /// <summary>
        /// 生成BQL实体
        /// </summary>
        public void GenerateBQLEntity()
        {
            FileInfo info = new FileInfo(EntityFileName);
            string dicPath = info.DirectoryName + "\\BQLEntity";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }

            string fileName = dicPath + "\\"+ ClassName + ".cs";
            
            string idal = Models.BQLEntity;
            List<string> codes = new List<string>();
            string baseType = null;
            if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
            {
                baseType = "BQLEntityTableHandle";
            }
            else 
            {
                baseType = DBName + "_" + EntityBaseTypeShortName;
            }
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DBName%>", DBName);
                    tmp = tmp.Replace("<%=BQLEntityBaseType%>", baseType);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    string entityClassName = ClassName;
                    tmp = tmp.Replace("<%=EntityClassName%>", entityClassName);
                    tmp = tmp.Replace("<%=PropertyDetail%>", GenProperty());
                    tmp = tmp.Replace("<%=RelationDetail%>", GenRelation());
                    tmp = tmp.Replace("<%=PropertyInit%>", GenInit());
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }
        /// <summary>
        /// 生成属性
        /// </summary>
        /// <returns></returns>
        private string GenProperty() 
        {
            StringBuilder sbProperty = new StringBuilder();

            foreach (EntityParam epf in Table.Params) 
            {
                //if (!epf.IsGenerate)
                //{
                //    continue;
                //}
                sbProperty.Append("        private BQLEntityParamHandle " + epf.FieldName + " = null;\n");
                sbProperty.Append("        /// <summary>\n");
                sbProperty.Append("        /// " + epf.Description + "\n");
                sbProperty.Append("        /// </summary>\n");
                sbProperty.Append("        public BQLEntityParamHandle " + epf.PropertyName + "\n");
                sbProperty.Append("        {\n");
                sbProperty.Append("            get\n");
                sbProperty.Append("            {\n");
                sbProperty.Append("                return " + epf.FieldName + ";\n");
                sbProperty.Append("            }\n");
                sbProperty.Append("         }\n");
            }
            return sbProperty.ToString();
        }

        /// <summary>
        /// 生成映射属性
        /// </summary>
        /// <returns></returns>
        private string GenRelation()
        {
            StringBuilder sbRelation = new StringBuilder();

            foreach (TableRelationAttribute er in Table.RelationItems)
            {
                //if (!er.IsGenerate)
                //{
                //    continue;
                //}
                //string targetType = er.FInfo.MemberTypeShortName;
                if (er.IsParent)
                {
                    string targetType = er.FieldTypeName;
                    sbRelation.Append("        /// <summary>\n");
                    sbRelation.Append("        /// " + er.Description + "\n");
                    sbRelation.Append("        /// </summary>\n");
                    sbRelation.Append("        public " + DBName + "_" + targetType + " " + er.PropertyName + "\n");
                    sbRelation.Append("        {\n");
                    sbRelation.Append("            get\n");
                    sbRelation.Append("            {\n");
                    sbRelation.Append("               return new " + DBName + "_" + targetType + "(this,\"" + er.PropertyName + "\");\n");
                    sbRelation.Append("            }\n");
                    sbRelation.Append("         }\n");
                }
                
            }
            return sbRelation.ToString();
        }

        /// <summary>
        /// 生成映射属性
        /// </summary>
        /// <returns></returns>
        private string GenInit()
        {
            StringBuilder sbInit = new StringBuilder();

            foreach (EntityParam epf in Table.Params)
            {
                //if (!epf.IsGenerate) 
                //{
                //    continue;
                //}
                sbInit.Append("            " + epf.FieldName + "=CreateProperty(\"" + epf.PropertyName + "\");\n");
                
            }
            return sbInit.ToString();
        }
    }
}
