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
        


       
        public EntityParamField(CodeElementPosition cp,ClrField fInfo,EntityConfig belongEntity) 
        {
            _cp = cp;
            _fInfo = fInfo;
            GetEntityParamInfo(fInfo);
            _belongEntity = belongEntity;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public EntityPropertyType EntityPropertyType
        {
            get { return _entityPropertyType; }
            set { _entityPropertyType = value; }
        }
        /// <summary>
        /// ��ȡ�ֶε�������Ϣ
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
        /// �ֶ�����
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
        /// �ֶ���
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }
       
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// ת�����ֶ���Ϣ
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
            return ep;
        }


        /// <summary>
        /// ��ӵ�Դ��
        /// </summary>
        /// <param name="source">Դ���б�</param>
        /// <param name="spaces">�ո�</param>
        public void AddSource(List<string> source,string spaces)
        {
            
            //string att = spaces + "[EntityParam(\"" + ParamName + "\",\"" + PropertyName + "\", DbType." + DbType + ", " + EnumUnit.EnumString(_entityPropertyType) + lenStr + ")]";
            //source.Add(att);
            //string fieldSource = spaces + "protected " + TypeName + " " + FieldName + " = " + GetMemberVariableDefaultValue(TypeName) + ";";
            //source.Add(fieldSource);
            
            //���ɶ�Ӧ����
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
