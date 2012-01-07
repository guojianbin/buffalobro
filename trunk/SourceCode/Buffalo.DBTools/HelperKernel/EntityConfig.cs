using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Project;
using EnvDTE;
using Buffalo.DB.CommBase;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityConfig
    {


        private List<string> _sourceCode = null;

        private EntityParamFieldCollection _eParamFields = new EntityParamFieldCollection();
        /// <summary>
        /// 实体字段
        /// </summary>
        public EntityParamFieldCollection EParamFields
        {
            get { return _eParamFields; }
        }
        private EntityRelationCollection _eRelation = new EntityRelationCollection();
        /// <summary>
        /// 映射属性
        /// </summary>
        public EntityRelationCollection ERelation
        {
            get { return _eRelation; }
        }
        private ClrTypeShape _classShape;
        private CodeElementPosition _cp;
        private string _tableName;
        private string _className;
        private string _namespace;
        private Project _currentProject;

        public Project CurrentProject
        {
            get { return _currentProject; }
            
        }

        
        

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }
        

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        private string _baseType;

        /// <summary>
        /// 基类 
        /// </summary>
        public string BaseType
        {
            get { return _baseType; }
            set { _baseType = value; }
        }

        private List<string> _interfaces;

        /// <summary>
        /// 接口
        /// </summary>
        public List<string> Interfaces
        {
            get { return _interfaces; }
            set { _interfaces = value; }
        }

        private string _fileName;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private string _summary;

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private Dictionary<string, CodeElementPosition> _properties;

        /// <summary>
        /// 类包含的属性集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Properties
        {
            get { return _properties; }
        }
        /// <summary>
        /// 初始化类配置
        /// </summary>
        /// <param name="classShape">类图形</param>
        /// <param name="project">所属项目</param>
        public EntityConfig(ClrTypeShape classShape,Project project) 
        {
            _classShape = classShape;
            FillClassInfo();
            InitFleld();
            InitPropertys();
            _currentProject = project;
        }

        /// <summary>
        /// 获取父类信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public static string GetBaseClass(ClrType ctype) 
        {
            InheritanceTypeRefMoveableCollection col = ctype.InheritanceTypeRefs;
            if (col != null && col.Count > 0)
            {
                string baseType = col[0].Name;
                if (baseType.Equals("System.Object", StringComparison.CurrentCultureIgnoreCase))
                {
                    return "System.Object";
                }
                else
                {
                    return baseType;
                }
            }

            return "System.Object";
            
        }

        /// <summary>
        /// 填充类信息
        /// </summary>
        private void FillClassInfo() 
        {
            ClrType ctype = _classShape.AssociatedType;
            _className = ctype.Name;

            _baseType = GetBaseClass(ctype);
            if (_baseType == "System.Object") 
            {
                _baseType = "EntityBase";
            }

            

            InterfaceImplementationTypeRefMoveableCollection itms = ctype.ImplementationTypeRefs;

            if (itms != null && itms.Count > 0)
            {
                _interfaces = new List<string>();
                foreach (InterfaceImplementationTypeRef itm in itms) 
                {
                    _interfaces.Add(itm.Name);
                }
            }

            foreach (CodeElementPosition cp in ctype.SourceCodePositions)
            {
                if (cp.FileName.IndexOf(".extend.cs") <0)
                {
                    _fileName = cp.FileName;

                    _cp = cp;
                }
            }
            _namespace = ctype.OwnerNamespace.Name;
            _summary = ctype.DocSummary;
            _tableName =EntityFieldBase.ToCamelName(_className);
            
        }

        /// <summary>
        /// 初始化字段信息
        /// </summary>
        private void InitFleld()
        {
            List<ClrField> lstFields = GetAllMember<ClrField>(_classShape.AssociatedType, false);
            for (int j = 0; j < lstFields.Count; j++)
            {


                ClrField field = lstFields[j];
                if (field == null)
                {
                    continue;
                }
                foreach (CodeElementPosition cp in field.SourceCodePositions)
                {
                    if (!IsManyOne(field))
                    {
                        EntityParamField epf = new EntityParamField(cp, field, this);
                        _eParamFields.Add(epf);
                    }
                    else 
                    {
                        EntityRelationItem erf = new EntityRelationItem(cp, field, this);
                        _eRelation.Add(erf);
                    }
                }
                _eParamFields.SortItem();
                _eRelation.SortItem();
            }
            EntityMappingConfig.LoadConfigInfo(this);


        }

        /// <summary>
        /// 判断是否多对一
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private bool IsManyOne(Member source) 
        {
            DataTypeInfos info = EntityFieldBase.GetTypeInfo(source);


            return info==null;
        }
        private List<string> _allPropertyNames;

        /// <summary>
        /// 所有字段名集合
        /// </summary>
        public List<string> AllPropertyNames 
        {
            get 
            {
                if (_allPropertyNames == null)
                {
                    _allPropertyNames = new List<string>();
                    List<ClrProperty> lstProperty = GetAllMember<ClrProperty>(_classShape.AssociatedType, true);
                    foreach (ClrProperty prot in lstProperty)
                    {
                        if (!IsManyOne(prot))
                        {
                            _allPropertyNames.Add(prot.Name);
                        }
                    }
                }
                return _allPropertyNames;
            }
        }

        /// <summary>
        /// 初始化属性信息
        /// </summary>
        private void InitPropertys()
        {
            List<ClrProperty> lstProperty = GetAllMember<ClrProperty>(_classShape.AssociatedType, false);
            _properties = new Dictionary<string, CodeElementPosition>();
            for (int j = 0; j < lstProperty.Count; j++)
            {
                object tm = lstProperty[j];

                ClrProperty property = tm as ClrProperty;
                if (property == null)
                {
                    continue;
                }
                foreach (CodeElementPosition cp in property.SourceCodePositions)
                {
                    _properties[property.Name] = cp;
                   
                }

            }


        }


       

        /// <summary>
        /// 切出开头的空格数
        /// </summary>
        /// <returns></returns>
        public static string CutSpace(string str) 
        {
            StringBuilder sbRet = new StringBuilder(str.Length);
            foreach (char chr in str) 
            {
                if (chr == ' ')
                {
                    sbRet.Append(chr);
                }
                else 
                {
                    break;
                }
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        public void GenerateCode() 
        {
            string extendName = FileName.Replace(this.ClassName + ".cs", this.ClassName + ".extend.cs");
            if (!File.Exists(extendName)) 
            {
                GenerateExtendCode(extendName);
            }
            
            List<string> lstSource =CodeFileHelper.ReadFile(FileName);
            List<string> lstTarget = new List<string>(lstSource.Count);
            int codeIndex = 0;
            int relationIndex = 0;
            bool isUsing = true;
            Dictionary<string, bool> dicUsing = new Dictionary<string, bool>();
            for (int i = 0; i < lstSource.Count; i++) 
            {
                string str=lstSource[i];
                if (i == _cp.StartLine-1) 
                {
                    if (str.IndexOf("class")>0) 
                    {
                        if (str.IndexOf(" partial ") < 0) 
                        {
                            str = str.Replace("class", "partial class");
                        }
                    }
                    lstTarget.Add(str);
                    //while (str.Trim().IndexOf("{") != 0) 
                    //{
                    //    i++;
                    //    str = lstSource[i];
                    //}
                    
                    //AddSource(lstTarget,CutSpace(str));
                    //lstTarget.Add(CutSpace(str)+"{");
                }
                else if (codeIndex<_eParamFields.Count && i == _eParamFields[codeIndex].StarLine-1 )
                {
                    for (int k = i; k < _eParamFields[codeIndex].EndLine; k++)
                    {
                        lstTarget.Add(lstSource[k]);
                    }
                    if (_eParamFields[codeIndex].IsGenerate)
                    {
                        _eParamFields[codeIndex].AddSource(lstTarget, CutSpace(str));
                        
                    }
                    
                    i = _eParamFields[codeIndex].EndLine - 1;
                    codeIndex++;
                }
                else if (relationIndex < _eRelation.Count && i == _eRelation[relationIndex].StarLine - 1)
                {
                    if (_eRelation[relationIndex].IsGenerate)
                    {
                        _eRelation[relationIndex].AddSource(lstTarget, CutSpace(str));

                    }
                    else
                    {
                        for (int k = i; k < _eRelation[relationIndex].EndLine; k++)
                        {
                            lstTarget.Add(lstSource[k]);
                        }

                    }
                    i = _eRelation[relationIndex].EndLine - 1;
                    relationIndex++;
                }
                else if (isUsing && str.IndexOf("namespace " + Namespace) >= 0) 
                {
                    AddSqlCommonUsing(dicUsing, lstTarget);
                    lstTarget.Add(str);
                    isUsing = false;
                }
                else
                {
                    if (isUsing)
                    {
                        if (str.IndexOf("using ") >= 0)
                        {
                            dicUsing[str.Trim()] = true;
                        }
                    }
                    lstTarget.Add(str);
                }
            }
            CodeFileHelper.SaveFile(FileName, lstTarget);

            EntityMappingConfig.SaveXML(this);

        }

        private static readonly string[] BaseTypes ={ typeof(EntityBase).Name, typeof(object).Name };


        /// <summary>
        /// 获取所有成员
        /// </summary>
        /// <typeparam name="T">成员类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="fillBase">是否级联父类</param>
        /// <returns></returns>
        public static List<T> GetAllMember<T>( ClrType type,bool fillBase) where T : Member
        {
            List<T> lst = new List<T>();
            Dictionary<string, bool> dicExistsPropertyName = new Dictionary<string, bool>();
            FillAllMember<T>(lst,dicExistsPropertyName, type, fillBase);
            return lst;
        }

        /// <summary>
        /// 获取该类的所有成员
        /// </summary>
        /// <typeparam name="T">成员类型</typeparam>
        /// <param name="lst">集合</param>
        /// <param name="type">类型</param>
        /// <param name="fillBase">是否级联父类</param>
        public static void FillAllMember<T>(List<T> lst, Dictionary<string, bool> dicExistsPropertyName, ClrType type, bool fillBase) where T : Member
        {
            if (fillBase)
            {
                InheritanceTypeRefMoveableCollection col = type.InheritanceTypeRefs;
                if (col != null && col.Count > 0)
                {
                    string baseType = col[0].TypeTypeName;

                    if (!string.IsNullOrEmpty(baseType))
                    {
                        bool isBaseType = false;
                        foreach (string typeName in BaseTypes)
                        {
                            if (baseType.EndsWith(typeName))
                            {
                                isBaseType = true;
                                break;
                            }
                        }
                        if (!isBaseType)
                        {
                            FillAllMember<T>(lst,dicExistsPropertyName, col[0].ClrType,fillBase);
                        }
                    }
                }
            }
           

            foreach (object pro in type.Members) 
            {
                T cPro=pro as T;
                if (cPro != null && !dicExistsPropertyName.ContainsKey(cPro.Name))
                {
                    dicExistsPropertyName[cPro.Name] = true;
                    lst.Add(cPro);
                }
            }
            
        }

        /// <summary>
        /// 所需的using
        /// </summary>
        private static string[] _needUsing ={ 
            "using System.Collections.Generic;" ,"using Buffalo.DB.CommBase;",
            "using Buffalo.Kernel.Defaults;","using Buffalo.DB.PropertyAttributes;","using System.Data;"
        };

        /// <summary>
        /// 添加类库必要的using
        /// </summary>
        /// <param name="dicUsing">已有的using集合</param>
        /// <param name="lstTarget">目标源码</param>
        private void AddSqlCommonUsing(Dictionary<string, bool> dicUsing, List<string> lstTarget) 
        {
            foreach (string usingStr in _needUsing) 
            {
                if (!dicUsing.ContainsKey(usingStr)) 
                {
                    lstTarget.Add(usingStr);
                }
            }
        }

        /// <summary>
        /// 生成扩展类代码文件
        /// </summary>
        private void GenerateExtendCode(string fileName)
        {
            string space = "";
            List<string> codes = new List<string>();
            codes.Add("using System;");
            codes.Add("using System.Data;");
            codes.Add("using System.Collections.Generic;");
            codes.Add("using Buffalo.DB.CommBase;");
            codes.Add("using Buffalo.Kernel.Defaults;");
            codes.Add("using Buffalo.DB.PropertyAttributes;");

            codes.Add("namespace "+Namespace);
            codes.Add("{");
            space+="	";
            codes.Add(space + "/// <summary>");
            codes.Add(space + "///  " + _summary);
            codes.Add(space + "/// </summary>");
            codes.Add(space + "public partial class "+ClassName);
            codes.Add(space + "{");
            codes.Add(space + "}");
            codes.Add("}");
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = _currentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

    }
}
