using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using System.Data;

namespace Buffalo.DBTools.ROMHelper
{
    /// <summary>
    /// 数据库到实体的信息
    /// </summary>
    public class DBEntityInfo
    {
        private Project _currentProject;
        private Diagram _currentDiagram;
        private DBConfigInfo _currentDBConfigInfo;

        /// <summary>
        /// 数据库配置信息
        /// </summary>
        public DBConfigInfo CurrentDBConfigInfo
        {
            get { return _currentDBConfigInfo; }
        }


        private DBTableInfo _belongTable;

        /// <summary>
        /// 所属表
        /// </summary>
        public DBTableInfo BelongTable
        {
            get { return _belongTable; }
        }

        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <param name="docView">类设计图</param>
        /// <param name="project">工程</param>
        /// <returns></returns>
        public static string GetNameSpace(ClassDesignerDocView docView, Project project) 
        {
            FileInfo docFile = new FileInfo(docView.DocData.FileName);
            FileInfo projectFile = new FileInfo(project.FileName);

            string dic = docFile.Directory.Name.Replace(projectFile.Directory.Name, "");
            string ret = dic.Replace("\\", ".");
            ret = ret.Trim('.');
            ret = project.Name + "." + ret;
            return ret;
        }


        /// <summary>
        /// 实体信息
        /// </summary>
        /// <param name="belong">所属的数据库信息</param>
        public DBEntityInfo(string entityNamespace, DBTableInfo belong) 
        {
            _belongTable = belong;
            _entityNamespace = entityNamespace;
            InitInfo();
        }

        /// <summary>
        /// 初始化类信息
        /// </summary>
        private void InitInfo() 
        {
            _className = _belongTable.Name;
        }

        private string _entityNamespace;

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public string EntityNamespace
        {
            get
            {
                return _entityNamespace;
            }
            
        }

        private string _className;
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName 
        {
            get 
            {
                return _className;
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="code"></param>
        public void GreanCode(List<string> code, int tiers) 
        {
            string model = Buffalo.DBTools.Models.Entity;

            string baseType = "EntityBase";

            if (tiers == 1) 
            {
                baseType = "";
            }

            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", _belongTable.Description);
                    tmp = tmp.Replace("<%=EntityBaseType%>", baseType);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=EntityFields%>", BildFields());
                    codes.Add(tmp);
                }
            }
        }

        /// <summary>
        /// 创建关系
        /// </summary>
        /// <returns></returns>
        private string BildRelations() 
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableRelationAttribute er in _belongTable.RelationItems) 
            {

            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成字段
        /// </summary>
        /// <returns></returns>
        private string BildFields() 
        {
            StringBuilder sb = new StringBuilder();
            foreach (EntityParam prm in _belongTable.Params) 
            {
                prm.FieldName = "_"+EntityFieldBase.ToCamelName(prm.ParamName);
                sb.Append("		///<summary>\n");
                sb.Append("		///" + prm.Description + "\n");
                sb.Append("		///</summary>\n");
                sb.Append("		private " + ToCSharpType(prm.SqlType) + " " + prm.FieldName + ";\n");
                sb.Append("		\n");
                sb.Append("		/// <summary>\n");
                sb.Append("		///"+prm.Description+"\n");
                sb.Append("		///</summary>\n");

                prm.PropertyName = EntityFieldBase.ToPascalName(prm.ParamName);

                sb.Append("     public string "+prm.PropertyName+"\n");
                sb.Append("     {");
                sb.Append("          get");
                sb.Append("          {");
                sb.Append("               return " + prm.FieldName + ";");
                sb.Append("          }");
                sb.Append("          set");
                sb.Append("          {");
                sb.Append("              " + prm.FieldName + "=value;");
                sb.Append("              OnPropertyUpdated(\"" + prm.PropertyName + "\");");
                sb.Append("          }");
                sb.Append("     }");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取C#类型
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <returns></returns>
        private string ToCSharpType(DbType type) 
        {
            switch (type)
            {
                case DbType.AnsiString: return "string";
                case DbType.AnsiStringFixedLength: return "string";
                case DbType.Binary: return "byte[]";
                case DbType.Boolean: return "bool?";
                case DbType.Byte: return "byte?";
                case DbType.Currency: return "decimal?";
                case DbType.Date: return "DateTime?";
                case DbType.DateTime: return "DateTime?";
                case DbType.Decimal: return "decimal?";
                case DbType.Double: return "double?";
                case DbType.Guid: return "Guid?";
                case DbType.Int16: return "short?";
                case DbType.Int32: return "int?";
                case DbType.Int64: return "long?";
                case DbType.Object: return "object";
                case DbType.SByte: return "sbyte?";
                case DbType.Single: return "float?";
                case DbType.String: return "string";
                case DbType.StringFixedLength: return "string";
                case DbType.Time: return "TimeSpan?";
                case DbType.UInt16: return "ushort?";
                case DbType.UInt32: return "uint?";
                case DbType.UInt64: return "ulong?";
                case DbType.VarNumeric: return "decimal?";
                default:
                    {
                        return "object" ;
                    }
            }
        }
    }
}
