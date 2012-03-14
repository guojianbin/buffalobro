using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.Kernel;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityParamField : EntityFieldBase
    {
        
        private string _dbType = null;
        private EntityPropertyType _entityPropertyType = EntityPropertyType.Normal;

        private string _paramName;
        private EntityConfig _belongEntity;
        private int _length;
        private bool _readonly;


       
        public EntityParamField(CodeElementPosition cp,ClrField fInfo,EntityConfig belongEntity) 
        {
            _cp = cp;
            _fInfo = fInfo;
            GetEntityParamInfo(fInfo);
            _belongEntity = belongEntity;
        }
        /// <summary>
        /// 属性类型
        /// </summary>
        public EntityPropertyType EntityPropertyType
        {
            get { return _entityPropertyType; }
            set { _entityPropertyType = value; }
        }
        /// <summary>
        /// 获取字段的配置信息
        /// </summary>
        /// <param name="fInfo"></param>
        private void GetEntityParamInfo(ClrField fInfo) 
        {
            
                _propertyName = ToPascalName(FieldName);
                _paramName = FieldName.Trim('_');
                DataTypeInfos info = EntityFieldBase.GetTypeInfo(fInfo);
                if (info != null)
                {
                    _dbType = info.DbTypes[0].ToString();
                    _length = info.DbLength;
                }
            
        }
        
        

        /// <summary>
        /// 字段类型
        /// </summary>
        public string PropertyType
        {
            get 
            {
                ComboBoxItem item=EntityFieldBase.PropertyTypeItems.FindByValue(_entityPropertyType);
                if (item != null)
                {
                    return item.Text;
                }
                return "";
            }
            set 
            {
                ComboBoxItem item = EntityFieldBase.PropertyTypeItems.FindByText(value);
                if (item != null)
                {
                    _entityPropertyType = (EntityPropertyType)item.Value;
                }
            }
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }
       
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        /// <summary>
        /// 转换成字段信息
        /// </summary>
        /// <returns></returns>
        public EntityParam ToParamInfo() 
        {
            EntityParam ep = new EntityParam();
            ep.FieldName = FieldName;
            ep.Description = Summary;
            ep.ParamName = ParamName;
            ep.PropertyName = PropertyName;
            ep.SqlType = (DbType)EnumUnit.GetEnumInfoByName(typeof(DbType), DbType).Value;
            ep.PropertyType = EntityPropertyType;
            ep.AllowNull = true;
            ep.ReadOnly = ReadOnly;
            return ep;
        }


        /// <summary>
        /// 添加到源码
        /// </summary>
        /// <param name="source">源码列表</param>
        /// <param name="spaces">空格</param>
        /// <param name="greanFiled">是否要生成字段</param>
        public void AddSource(List<string> source,string spaces)
        {
            
            //string att = spaces + "[EntityParam(\"" + ParamName + "\",\"" + PropertyName + "\", DbType." + DbType + ", " + EnumUnit.EnumString(_entityPropertyType) + lenStr + ")]";
            //source.Add(att);
            //if (greanFiled)
            //{
            //    string fieldSource = spaces + "private " + TypeName + " " + FieldName + ";";
            //    source.Add(fieldSource);
            //}
            //生成对应属性
            if (!_belongEntity.Properties.ContainsKey(PropertyName))
            {
                source.Add(spaces + "/// <summary>");
                source.Add(spaces + "/// " + Summary);
                source.Add(spaces + "/// </summary>");
                string propertyText = spaces + "public " + TypeName + " " + PropertyName;
                source.Add(propertyText);
                source.Add(spaces + "{");
                source.Add(spaces + "    get");
                source.Add(spaces + "    {");
                source.Add(spaces + "        return " + FieldName + ";");
                source.Add(spaces + "    }");
                source.Add(spaces + "    set");
                source.Add(spaces + "    {");
                source.Add(spaces + "        " + FieldName + "=value;");
                source.Add(spaces + "        OnPropertyUpdated(\"" + PropertyName + "\");");
                source.Add(spaces + "    }");
                source.Add(spaces + "}");
            }
        }

    }
}
