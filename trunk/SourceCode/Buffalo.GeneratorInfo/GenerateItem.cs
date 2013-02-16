using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// �������Ե���
    /// </summary>
    public class GenerateItem
    {
        /// <summary>
        /// �������Ե���
        /// </summary>
        /// <param name="dicCheckItem">ѡ�е���</param>
        /// <param name="propertyType">��Ӧ���ֶ�����</param>
        /// <param name="summary">ע��</param>
        /// <param name="typeName">������</param>
        /// <param name="propertyName">��Ӧ��������</param>
        public GenerateItem(Dictionary<string, object> dicCheckItem, string propertyType, 
            string summary, string typeName, string propertyName) 
        {
            _dicCheckItem = dicCheckItem;
            _propertyName = propertyName;
            _propertyType = propertyType;
            _summary = summary;
            _typeName=typeName;
        }

        private Dictionary<string, object> _dicCheckItem = null;

        /// <summary>
        /// ѡ�е���
        /// </summary>
        internal Dictionary<string, object> CheckItem
        {
            get
            {
                return _dicCheckItem;
            }
        }

        public int ItemCount 
        {
            get 
            {
                return _dicCheckItem.Count;
            }
        }
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName)
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                return ret.ToString();
            }
            return null;
        }
        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                if (ret is bool)
                {
                    return (bool)ret;
                }
            }
            return false;
        }
        private string _propertyType;

        /// <summary>
        /// ��Ӧ���ֶ�����
        /// </summary>
        public string PropertyType
        {
            get { return _propertyType; }
            
        }

        private string _summary;

        /// <summary>
        /// ע��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }

        private string _typeName;

        /// <summary>
        /// ������
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        private string _propertyName;
        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
    }
}
