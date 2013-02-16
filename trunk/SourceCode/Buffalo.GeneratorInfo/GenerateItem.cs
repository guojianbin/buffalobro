using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// 生成属性的项
    /// </summary>
    public class GenerateItem
    {
        /// <summary>
        /// 生成属性的项
        /// </summary>
        /// <param name="dicCheckItem">选中的项</param>
        /// <param name="propertyType">对应的字段类型</param>
        /// <param name="summary">注释</param>
        /// <param name="typeName">类型名</param>
        /// <param name="propertyName">对应的属性名</param>
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
        /// 选中的项
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
        /// 获取值
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
        /// 获取是否选中项
        /// </summary>
        /// <param name="itemName">项名称</param>
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
        /// 对应的字段类型
        /// </summary>
        public string PropertyType
        {
            get { return _propertyType; }
            
        }

        private string _summary;

        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }

        private string _typeName;

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        private string _propertyName;
        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
    }
}
