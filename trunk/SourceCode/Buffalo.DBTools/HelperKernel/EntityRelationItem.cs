using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ӳ����Ϣ
    /// </summary>
    public class EntityRelationItem : EntityFieldBase
    {
        
        
        
        

        private string _sourceProperty;



        private string _targetProperty;



        

        /// <summary>
        /// Ŀ������
        /// </summary>
        public string TargetProperty
        {
            get { return _targetProperty; }
            set { _targetProperty = value; }
        }
        /// <summary>
        /// Դ����
        /// </summary>
        public string SourceProperty
        {
            get { return _sourceProperty; }
            set { _sourceProperty = value; }
        }


        

        

        private List<string> _targetPropertyList;

        /// <summary>
        /// ��ȡĿ��ʵ�����Ϣ����
        /// </summary>
        public List<string> TargetPropertyList
        {
            get 
            {
                if (_targetPropertyList == null)
                {
                    List<ClrProperty> lstPropertys = EntityConfig.GetAllMember<ClrProperty>(FInfo.MemberType, true);
                    _targetPropertyList = new List<string>(lstPropertys.Count);
                    foreach (ClrProperty pro in lstPropertys)
                    {
                        _targetPropertyList.Add(pro.Name);
                    }
                }
                return _targetPropertyList;
            }
        }

        public EntityRelationItem(CodeElementPosition cp, ClrField fInfo, EntityConfig belongEntity) 
        {
            _cp = cp;
            _fInfo = fInfo;
            GetInfo(fInfo);
            _belongEntity = belongEntity;
        }
        /// <summary>
        /// ��ȡ�ֶε�������Ϣ
        /// </summary>
        /// <param name="fInfo"></param>
        private void GetInfo(ClrField fInfo)
        {
            _propertyName = ToPascalName(FieldName);
        }

        /// <summary>
        /// ��ӵ�Դ��
        /// </summary>
        /// <param name="source">Դ���б�</param>
        /// <param name="spaces">�ո�</param>
        public void AddSource(List<string> source, string spaces)
        {
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
                source.Add(spaces + "       if (" + FieldName + " == null)");
                source.Add(spaces + "       {");
                source.Add(spaces + "           FillParent(\"" + PropertyName + "\");");
                source.Add(spaces + "       }");
                source.Add(spaces + "        return " + FieldName + ";");
                source.Add(spaces + "    }");
                source.Add(spaces + "}");
            }
        }
    }
}
