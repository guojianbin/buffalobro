using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// ���Ե������Ϣ
    /// </summary>
    public class RelationInfo
    {

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="targetName">Ŀ��ʵ���������</param>
        /// <param name="sourceName">��ʵ��Ĺ�������</param>
        /// <param name="isParent">�Ƿ���������</param>
        /// <param name="type">�������Ե���ֵ����</param>
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
        /// Ŀ��ʵ���������
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
        }

        
        private string _sourceName;
        /// <summary>
        /// ��ʵ��Ĺ�������
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }

        private string _type;
        /// <summary>
        /// �������Ե���ֵ����
        /// </summary>
        public string Type
        {
            get { return _type; }
        }


        private bool _isParent;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
        }


    }
}
