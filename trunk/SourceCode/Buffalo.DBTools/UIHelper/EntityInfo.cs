using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.HelperKernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 实体信息
    /// </summary>
    public class EntityInfo
    {
        private string _fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
       private ClassDesignerInfo _designerInfo;
        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }
        private string _namespace;
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }

        private string _className;
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
        /// <summary>
        /// 类全名
        /// </summary>
        public string FullName 
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_namespace)) 
                {
                    sb.Append(_namespace);
                    sb.Append(".");
                }
                sb.Append(_className);
                return sb.ToString();
            }
        }

        private string _summary;

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _summary; }
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
        private ClrType _classType;
        /// <summary>
        /// 类类型
        /// </summary>
        public ClrType ClassType
        {
            get { return _classType; }
        }

        List<UIModelItem> lstProperty;

        /// <summary>
        /// 属性集合
        /// </summary>
        public List<UIModelItem> Propertys
        {
            get { return lstProperty; }
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
        /// <summary>
        /// 初始化类配置
        /// </summary>
        /// <param name="classShape">类图形</param>
        /// <param name="project">所属项目</param>
        public EntityInfo(ClrType ctype, ClassDesignerInfo info) 
        {
            _classType = ctype;
            _designerInfo = info;
            FillClassInfo();
            InitPropertys();
            
        }
        Dictionary<string, List<string>> _dicGenericInfo = new Dictionary<string, List<string>>();
        /// <summary>
        /// 泛型信息
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }

        /// <summary>
        /// 初始化属性
        /// </summary>
        private void InitPropertys() 
        {
            lstProperty = new List<UIModelItem>();
            List<ClrProperty> lstClrProperty = EntityConfig.GetAllMember<ClrProperty>(_classType, true);
            foreach (ClrProperty property in lstClrProperty) 
            {
                UIModelItem item = new UIModelItem(property);
                lstProperty.Add(item);
            }
        }

        /// <summary>
        /// 填充类信息
        /// </summary>
        private void FillClassInfo()
        {
            ClrType ctype = _classType;
            _className = ctype.Name;

            _baseType = EntityConfig.GetBaseClass(ctype, out _baseTypeName);
            CodeElementPosition ocp = null;
            _fileName = EntityConfig.GetFileName(ctype, out ocp);

            _namespace = ctype.OwnerNamespace.Name;
            _summary = ctype.DocSummary;
            if (ctype.Generic)
            {
                EntityConfig.InitGeneric(ctype, _dicGenericInfo);
            }
        }
    }
}
