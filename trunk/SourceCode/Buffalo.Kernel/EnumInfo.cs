using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class EnumInfo
    {
        private object value;
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        
        private string fieldName;
        /// <summary>
        /// 常量名
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        private string description;
        /// <summary>
        /// 注释[Description("内容")]的内容
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string _displayName;
        /// <summary>
        /// 获取[DisplayName("显示名")]的内容
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
