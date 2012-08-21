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
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Windows.Forms;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;
using System.Data;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DBCheckers;
using Buffalo.DBTools.ROMHelper;

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

        ClassDesignerDocView _selectDocView;
        /// <summary>
        /// 选择的文档
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get { return _selectDocView; }
            set { _selectDocView = value; }
        }

        private ClrType _classType;

        
        private CodeElementPosition _cp;
        private string _tableName;
        private string _className;
        private string _namespace;
        private Project _currentProject;
        private Diagram _currentDiagram;
        private DBConfigInfo _currentDBConfigInfo;

        /// <summary>
        /// 数据库配置信息
        /// </summary>
        public DBConfigInfo CurrentDBConfigInfo
        {
            get 
            {
                InitDBConfig();
                return _currentDBConfigInfo;
            }
        }

        /// <summary>
        /// 初始化数据库信息
        /// </summary>
        public void InitDBConfig() 
        {
            if (_currentDBConfigInfo == null)
            {
                _currentDBConfigInfo = FrmDBSetting.GetDBConfigInfo(CurrentProject, SelectDocView, Namespace + ".DataAccess");
            }
        }
        /// <summary>
        /// 当前类图
        /// </summary>
        public Diagram CurrentDiagram
        {
            get { return _currentDiagram; }
        }


        /// <summary>
        /// 当前工程
        /// </summary>
        public Project CurrentProject
        {
            get { return _currentProject; }
            
        }

        /// <summary>
        /// 关联类型
        /// </summary>
        public ClrType ClassType
        {
            get { return _classType; }
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

        private ClrType _baseType;

        /// <summary>
        /// 基类 
        /// </summary>
        public ClrType BaseType
        {
            get { return _baseType; }
            set { _baseType = value; }
        }

        private string _baseTypeName;


        /// <summary>
        /// 基名称
        /// </summary>
        public string BaseTypeName 
        {
            get
            {

                return _baseTypeName;
            }
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
        private bool _isTable = false;
        /// <summary>
        /// 判断是否表
        /// </summary>
        public bool IsTable
        {
            get { return _isTable; }
            set { _isTable = value; }
        }
        private Dictionary<string, CodeElementPosition> _properties;

        /// <summary>
        /// 类包含的属性集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Properties
        {
            get { return _properties; }
        }

        DBConfigInfo _dbInfo;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBConfigInfo DbInfo
        {
            get
            {
                if (_dbInfo == null)
                {
                    _dbInfo = FrmDBSetting.GetDBConfigInfo(CurrentProject, SelectDocView, DBEntityInfo.GetNameSpace(SelectDocView, CurrentProject) + ".DataAccess");


                }
                return _dbInfo;
            }
        }
        private Dictionary<string, CodeElementPosition> _methods;

        /// <summary>
        /// 类包含的函数集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Methods
        {
            get { return _methods; }
        }

        private Dictionary<string, CodeElementPosition> _fields;

        /// <summary>
        /// 类包含的字段集合
        /// </summary>
        public Dictionary<string, CodeElementPosition> Fields
        {
            get { return _fields; }
        }
        /// <summary>
        /// 初始化类配置
        /// </summary>
        /// <param name="classShape">类图形</param>
        /// <param name="project">所属项目</param>
        public EntityConfig(ClrType ctype, Project project, Diagram currentDiagram) 
        {
            
            //_classShape = classShape;
            _classType = ctype;
            FillClassInfo();
            InitField();
            InitPropertys();
            InitMethods();
            _currentProject = project;
            _currentDiagram = currentDiagram;
            
        }

        /// <summary>
        /// 获取字段对应的属性
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, EntityParamField> GetParamMapField() 
        {
            Dictionary<string, EntityParamField> dic = new Dictionary<string, EntityParamField>();
            EntityConfig curType = this;
            while (curType != null)
            {

                foreach (EntityParamField field in curType.EParamFields)
                {
                    if (!string.IsNullOrEmpty(field.ParamName))
                    {
                        dic[field.ParamName] = field;
                    }
                }
                if (!IsSystemType(curType.BaseType))
                {
                    try
                    {
                        curType = new EntityConfig(curType.BaseType, CurrentProject, CurrentDiagram);
                    }
                    catch
                    {
                        curType = null;
                    }
                }
                else 
                {
                    curType = null;
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取关系对应的属性
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, EntityRelationItem> GetRelationmMapField()
        {
            Dictionary<string, EntityRelationItem> dic = new Dictionary<string, EntityRelationItem>();
            EntityConfig curType = this;
            while (curType != null)
            {
                foreach (EntityRelationItem field in curType.ERelation)
                {
                    string key = field.TypeName + ":" + field.SourceProperty + ":" + field.TargetProperty;
                    if (!string.IsNullOrEmpty(key))
                    {
                        dic[key] = field;
                    }
                }
                if (!IsSystemType(curType.BaseType))
                {
                    try
                    {
                        curType = new EntityConfig(curType.BaseType, CurrentProject, CurrentDiagram);
                    }
                    catch
                    {
                        curType = null;
                    }
                }
                else
                {
                    curType = null;
                }
            }
            return dic;
        }
        private List<EntityParam> _dbParams;
        /// <summary>
        /// 数据库所属的此类不存在的字段
        /// </summary>
        public List<EntityParam> DbParams
        {
            get { return _dbParams; }
        }
        private List<TableRelationAttribute> _dbRelations;
        /// <summary>
        /// 数据库所属的此类不存在的关系
        /// </summary>
        public List<TableRelationAttribute> DbRelations
        {
            get { return _dbRelations; }
        }
        /// <summary>
        /// 更新类信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <param name="project"></param>
        /// <param name="currentDiagram"></param>
        /// <returns></returns>
        public static EntityConfig GetEntityConfigByTable(ClrType ctype, Project project, Diagram currentDiagram, ClassDesignerDocView selectDocView) 
        {
            
            EntityConfig entity = new EntityConfig(ctype, project, currentDiagram);
            entity.SelectDocView = selectDocView;
            if (string.IsNullOrEmpty(entity.TableName)) 
            {
                return null;
            }
            DBInfo db=entity.DbInfo.CreateDBInfo();
            List<string> selTab = new List<string>();
            selTab.Add(entity.TableName);
            List<DBTableInfo> lstGen = TableChecker.GetTableInfo(db, selTab);
            if (lstGen.Count > 0) 
            {
                DBTableInfo info = lstGen[0];
                Dictionary<string, EntityParamField> dicParam = entity.GetParamMapField();
                entity._dbParams=new List<EntityParam>();
                foreach (EntityParam prm in info.Params) 
                {
                    string paramName = prm.ParamName;
                    if (dicParam.ContainsKey(paramName)) 
                    {
                        continue;
                    }
                    entity._dbParams.Add(prm);

                }
                Dictionary<string, EntityRelationItem> dicRelation = entity.GetRelationmMapField();
                entity._dbRelations = new List<TableRelationAttribute>();
                foreach (TableRelationAttribute tr in info.RelationItems)
                {

                    string key = EntityFieldBase.ToPascalName(tr.TargetTable) + ":" + EntityFieldBase.ToPascalName(tr.SourceName) + ":" + EntityFieldBase.ToPascalName(tr.TargetName);
                    if (dicRelation.ContainsKey(key))
                    {
                        continue;
                    }
                    entity._dbRelations.Add(tr);

                }
                
            }
            return entity;
        } 

        /// <summary>
        /// 获取父类信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public static ClrType GetBaseClass(ClrType ctype,out string typeName) 
        {
            InheritanceTypeRefMoveableCollection col = ctype.InheritanceTypeRefs;
            typeName = "System.Object";
            if (col != null && col.Count > 0)
            {
                typeName = col[0].Name;
                ClrType baseType = col[0].ClrType;

                return baseType;
            }

            return null;
            
        }

        /// <summary>
        /// 填充类信息
        /// </summary>
        private void FillClassInfo() 
        {
            ClrType ctype = _classType;
            _className = ctype.Name;

            _baseType = GetBaseClass(ctype,out _baseTypeName);

            _fileName = GetFileName(ctype, out _cp);
            //foreach (CodeElementPosition cp in ctype.SourceCodePositions)
            //{
            //    if (cp.FileName.IndexOf(".extend.cs") <0)
            //    {
            //        _fileName = cp.FileName;

            //        _cp = cp;
            //        break;
            //    }
            //}
            _namespace = ctype.OwnerNamespace.Name;
            _summary = ctype.DocSummary;
            _tableName =EntityFieldBase.ToCamelName(_className);
            
        }

        /// <summary>
        /// 获取实体所在的文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFileName(ClrType ctype, out CodeElementPosition ocp) 
        {
            ocp = null;
            foreach (CodeElementPosition cp in ctype.SourceCodePositions)
            {
                if (cp.FileName.IndexOf(".extend.cs") < 0)
                {
                    ocp = cp;
                    
                    return cp.FileName;
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化字段信息
        /// </summary>
        private void InitField()
        {
            List<ClrField> lstFields = GetAllMember<ClrField>(_classType, false);
            _fields = new Dictionary<string, CodeElementPosition>();
            for (int j = 0; j < lstFields.Count; j++)
            {


                ClrField field = lstFields[j];
                if (field == null)
                {
                    continue;
                }
                if (field.SourceCodePositions == null) 
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

                    _fields[field.Name] = cp;
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
                    List<ClrProperty> lstProperty = GetAllMember<ClrProperty>(_classType, true);
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
        /// 初始化函数
        /// </summary>
        private void InitMethods() 
        {
            List<ClrMethod> lstMethod = GetAllMember<ClrMethod>(_classType, false);
            _methods = new Dictionary<string, CodeElementPosition>();
            for (int j = 0; j < lstMethod.Count; j++)
            {
                object tm = lstMethod[j];

                ClrMethod method = tm as ClrMethod;
                if (method == null)
                {
                    continue;
                }
                foreach (CodeElementPosition cp in method.SourceCodePositions)
                {
                    _methods[method.Name] = cp;

                }

            }
        }

        /// <summary>
        /// 初始化属性信息
        /// </summary>
        private void InitPropertys()
        {
            List<ClrProperty> lstProperty = GetAllMember<ClrProperty>(_classType, false);
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
        /// 生成扩展代码
        /// </summary>
        private void GenerateExtenCode() 
        {
            BQLEntityGenerater bqlEntity = new BQLEntityGenerater(this);
            GenerateExtendCode();
            if (_currentDBConfigInfo.Tier == 3)
            {
                Generate3Tier g3t = new Generate3Tier(this);
                g3t.GenerateBusiness();
                if (!string.IsNullOrEmpty(this.TableName))
                {
                    g3t.GenerateIDataAccess();
                    g3t.GenerateDataAccess();
                    g3t.GenerateBQLDataAccess();
                }
            }
            bqlEntity.GenerateBQLEntityDB();
            bqlEntity.GenerateBQLEntity();
            EntityMappingConfig.SaveXML(this);
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, ClrField> AllField() 
        {
            List<ClrField> lstFields = GetAllMember<ClrField>(this._classType, false);

            Dictionary<string, ClrField> dicFields = new Dictionary<string, ClrField>();
            foreach (ClrField objField in lstFields)
            {
                dicFields[objField.Name] = objField;
            }
            return dicFields;
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        public void GenerateCode() 
        {
            //InitDBConfig();

            List<string> lstSource =CodeFileHelper.ReadFile(FileName);
            List<string> lstTarget = new List<string>(lstSource.Count);
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
                }
                else if (i == _cp.EndLine - 1) 
                {
                    string space = CutSpace(str) + "    ";
                    foreach (EntityParamField param in _eParamFields) 
                    {
                        if (param.IsGenerate) 
                        {
                            param.AddSource(lstTarget, space);
                        }
                    }
                    foreach (EntityRelationItem relation in _eRelation)
                    {
                        if (relation.IsGenerate)
                        {
                            relation.AddSource(lstTarget, space);
                        }
                    }

                    if( _dbParams!=null)
                    {
                        foreach (EntityParam param in _dbParams)
                        {
                            StringBuilder sb = new StringBuilder();
                            DBEntityInfo.AppendFieldInfo(param, sb);
                            lstTarget.Add(sb.ToString());
                            
                        }
                    }
                    if (_dbRelations != null) 
                    {
                        foreach (TableRelationAttribute er in _dbRelations)
                        {
                            StringBuilder sb = new StringBuilder();
                            DBEntityInfo.FillRelationsInfo(er, sb);
                            lstTarget.Add(sb.ToString());

                        }
                    }


                    AddContext(lstTarget);
                    lstTarget.Add(str);
                    
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
            GenerateExtenCode();
            

        }


        /// <summary>
        /// 添加到代码
        /// </summary>
        /// <param name="lstTarget"></param>
        private void AddContext(List<string> lstTarget)
        {
            if (CurrentDBConfigInfo.Tier != 1 || string.IsNullOrEmpty(TableName))
            {
                return;
            }
            string strField = "_____baseContext";

            string strPropertie = "GetContext";

            if (!ExistsMember<ClrField>( _classType,strField))
            {
                lstTarget.Add("        private static ModelContext<" + ClassName + "> " + strField + "=new ModelContext<" + ClassName + ">();");
            }

            if (!ExistsMember<ClrMethod>(_classType, strPropertie))
            {
                lstTarget.Add("        /// <summary>");
                lstTarget.Add("        /// 获取查询关联类");
                lstTarget.Add("        /// </summary>");
                lstTarget.Add("        /// <returns></returns>");
                lstTarget.Add("        public static ModelContext<" + ClassName + "> " + strPropertie + "() ");
                lstTarget.Add("        {");
                lstTarget.Add("            return " + strField + ";");
                lstTarget.Add("        }");
            }

        }

        /// <summary>
        /// 通过属性名获取信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public EntityParamField GetParamInfoByPropertyName(string propertyName)
        {
            Stack<EntityConfig> stkEntity = GetEntity(this, CurrentProject, CurrentDiagram);
            while (stkEntity.Count > 0)
            {
                EntityConfig curEntity = stkEntity.Pop();
                foreach (EntityParamField item in curEntity.EParamFields)
                {
                    if (item.PropertyName == propertyName)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取类的自身和基类集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Stack<EntityConfig> GetEntity(EntityConfig entity, Project curProject, Diagram selDiagram) 
        {
            Stack<EntityConfig> stkConfig = new Stack<EntityConfig>();

            ClrType curType = entity.ClassType;
            string typeName;
            while (curType != null)
            {

                
                stkConfig.Push(entity);
                curType = EntityConfig.GetBaseClass(curType, out typeName);
                if (EntityConfig.IsSystemType(curType))
                {
                    break;
                }
                entity = new EntityConfig(curType, curProject, selDiagram);
            }
            return stkConfig;
        }

        private static readonly string[] BaseTypes ={ 
            typeof(EntityBase).FullName, typeof(object).FullName,"Buffalo.DB.CommBase.BusinessBases.ThinModelBase" };

        /// <summary>
        /// 是否系统类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSystemType(ClrType type) 
        {
            if (type == null) 
            {
                return true;
            }
            return IsSystemTypeName(type.Name);
            
        }

        

        /// <summary>
        /// 是否系统类名
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsSystemTypeName(string typeName) 
        {
            if (string.IsNullOrEmpty(typeName)) 
            {
                return true;
            }
            foreach (string basetypeName in BaseTypes)
            {
                if (basetypeName.Equals(typeName))
                {
                    return true;
                }
            }
            return false;
        }

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
        public static void FillAllMember<T>(List<T> lst, Dictionary<string, bool> dicExistsPropertyName,
            ClrType type, bool fillBase) where T : Member
        {
            
            if (fillBase)
            {
                InheritanceTypeRefMoveableCollection col = type.InheritanceTypeRefs;
                if (col != null && col.Count > 0)
                {
                    string baseType = col[0].TypeTypeName;


                    bool isBaseType = IsSystemTypeName(baseType);

                    if (!isBaseType)
                    {
                        ClrType btype=col[0].ClrType;
                        if (btype != null)
                        {
                            FillAllMember<T>(lst, dicExistsPropertyName, btype, fillBase);
                        }
                    }

                }
            }
           

            foreach (object pro in type.Members) 
            {
                T cPro=pro as T;

                if (cPro != null && !dicExistsPropertyName.ContainsKey(cPro.Name) &&!cPro.IsStatic)
                {
                    dicExistsPropertyName[cPro.Name] = true;
                    lst.Add(cPro);
                }
            }
            
        }

        /// <summary>
        /// 判断是否存在成员
        /// </summary>
        /// <returns></returns>
        public static bool ExistsMember<T>(ClrType type,string menberName) where T : Member
        {
            bool hasName = false;

            //检查基类
            InheritanceTypeRefMoveableCollection col = type.InheritanceTypeRefs;
            if (col != null && col.Count > 0)
            {
                string baseType = col[0].TypeTypeName;
                bool isBaseType = IsSystemTypeName(baseType);
                if (!isBaseType)
                {
                    ClrType btype = col[0].ClrType;
                    if (btype != null)
                    {
                        hasName=ExistsMember<T>(btype, menberName);
                    }
                }
            }

            if (hasName) 
            {
                return hasName;
            }
            //检查本类
            foreach (object pro in type.Members)
            {
                T cPro = pro as T;

                if (cPro!=null)
                {
                    if (cPro.Name == menberName) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 所需的using
        /// </summary>
        private static string[] _needUsing ={ 
            "using System.Collections.Generic;" ,"using Buffalo.DB.CommBase;",
            "using Buffalo.Kernel.Defaults;","using Buffalo.DB.PropertyAttributes;",
            "using System.Data;","using Buffalo.DB.CommBase.BusinessBases;"
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
        private void GenerateExtendCode()
        {
            FileInfo fileInfo = new FileInfo(FileName);
            string fileName = fileInfo.DirectoryName + "\\" + fileInfo.Name.Replace(".cs", ".extend.cs");
            if (File.Exists(fileName)) 
            {
                return;
            }

            string model = Models.UserEntity;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model)) 
            {
                string tmp = null;
                while((tmp=reader.ReadLine())!=null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", Namespace);
                    tmp = tmp.Replace("<%=Summary%>", _summary);


                    string classFullName = null;
                    if (ClassType.Generic)
                    {
                        classFullName=ClassType.GenericTypeName;
                        
                    }
                    else
                    {
                        classFullName=ClassName;
                    }
                    tmp = tmp.Replace("<%=ClassFullName%>", classFullName);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem classItem = GetProjectItemByFileName(_currentProject, _cp.FileName);
            //DTEHelper.AddFileToProjectItem(item, NHBFilePath, 3);
            EnvDTE.ProjectItem newit = classItem.ProjectItems.AddFromFile(fileName);
            //EnvDTE.ProjectItem newit = _currentProject.ProjectItems.AddFromFile(fileName);
            
            newit.Properties.Item("BuildAction").Value = 3;
        }

        /// <summary>
        /// 获取项目项
        /// </summary>
        /// <param name="project">项目</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static EnvDTE.ProjectItem GetProjectItemByFileName(Project project, string fileName)
        {
            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
            {
                EnvDTE.ProjectItem res = GetProjectItemByFileName(item, fileName);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取文件所属的项目项
        /// </summary>
        /// <param name="item">项</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static EnvDTE.ProjectItem GetProjectItemByFileName(EnvDTE.ProjectItem item, string fileName)
        {
            if (item.get_FileNames(0).ToLower() == fileName.ToLower()) return item;
            if (item.ProjectItems.Count == 0) return null;
            foreach (EnvDTE.ProjectItem i in item.ProjectItems)
            {
                EnvDTE.ProjectItem res = GetProjectItemByFileName(i, fileName);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 当前表到表信息的转换
        /// </summary>
        /// <returns></returns>
        public KeyWordTableParamItem ToTableInfo() 
        {
            KeyWordTableParamItem table = new KeyWordTableParamItem(TableName, null);
            table.Description = Summary;
            table.IsView = false;
            FillParams(table);
            FillRelation(table);
            return table;
        }

        /// <summary>
        /// 填充关系信息
        /// </summary>
        /// <param name="table"></param>
        private void FillRelation(KeyWordTableParamItem table) 
        {
            table.RelationItems=new List<TableRelationAttribute>();
            foreach (EntityRelationItem er in ERelation) 
            {
                if (!er.IsGenerate) 
                {
                    continue;
                }
                table.RelationItems.Add(er.GetRelationInfo());
            }
            if (_dbRelations != null)
            {
                table.RelationItems.AddRange(_dbRelations);
            }
        }

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="table"></param>
        private void FillParams(KeyWordTableParamItem table) 
        {
            table.Params = new List<EntityParam>();
            foreach (EntityParamField field in EParamFields) 
            {
                if (!field.IsGenerate) 
                {
                    continue;
                }
                table.Params.Add(field.ToParamInfo());
            }
            if (_dbParams != null) 
            {
                table.Params.AddRange(_dbParams);
            }
        }

    }
}
