using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// 属性的外键信息
    /// </summary>
    public class RelationInfo
    {

        /// <summary>
        /// 关联信息
        /// </summary>
        /// <param name="targetName">目标实体的属性名</param>
        /// <param name="sourceName">本实体的关联属性</param>
        /// <param name="isParent">是否主表属性</param>
        /// <param name="type">关联属性的数值类型</param>
        public RelationInfo(string targetName,
            string sourceName, bool isParent, string type) 
        {
            _targetName = targetName;
            _sourceName = sourceName;
            _isParent = IsParent;
            _type = type;
        }
        
        private string _targetName;
        /// <summary>
        /// 目标实体的属性名
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
        }

        
        private string _sourceName;
        /// <summary>
        /// 本实体的关联属性
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }

        private string _type;
        /// <summary>
        /// 关联属性的数值类型
        /// </summary>
        public string Type
        {
            get { return _type; }
        }


        private bool _isParent;
        /// <summary>
        /// 是否主表属性
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
        }


    }
}
