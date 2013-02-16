using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// Ҫ����ʵ����Ϣ
    /// </summary>
    public class GeneratorEntity
    {
        /// <summary>
        /// Ҫ����ʵ����Ϣ
        /// </summary>
        /// <param name="fileName">���ļ���</param>
        /// <param name="nameSpace">�����ռ�</param>
        /// <param name="className">����</param>
        /// <param name="summary">ע��</param>
        /// <param name="baseTypeName">������</param>
        /// <param name="dicGenericInfo">������Ϣ</param>
        /// <param name="model">��ѡ����Ϣ</param>
        public GeneratorEntity(string fileName,string nameSpace, string className, string summary, string baseTypeName,
            Dictionary<string, List<string>> dicGenericInfo, GenerateItem model) 
        {
            _fileName = fileName;
            _baseTypeName = baseTypeName;
            _className = className;
            _dicGenericInfo = dicGenericInfo;
            _fileName = fileName;
            _model = model;
            _namespace = nameSpace;
            _summary = summary;
        }


        private string _fileName;
        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
        private string _namespace;
        /// <summary>
        /// �����ռ�
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }

        private string _className;
        /// <summary>
        /// ����
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
        /// <summary>
        /// ��ȫ��
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
        /// ע��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }
        private string _baseTypeName;
        /// <summary>
        /// ������
        /// </summary>
        public string BaseTypeName
        {
            get
            {

                return _baseTypeName;
            }
        }

        private Dictionary<string, List<string>> _dicGenericInfo =null;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }

        private GenerateItem _model=null;


        /// <summary>
        /// ѡ�е���
        /// </summary>
        internal Dictionary<string, object> CheckItem
        {
            get
            {
                return _model.CheckItem;
            }
        }
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName)
        {
            return _model.GetValue(itemName);
        }

        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {

            return _model.HasItem(itemName);
        }

    }
}
