using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
namespace TestAddIn
{
    /// <summary>
    /// Ա��
    /// </summary>
    public partial class ManEmployee:ManBase
    {
        /// <summary>
        /// Ա����
        /// </summary>
        private string _name;
        /// <summary>
        /// Ա����
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name=value;
                OnPropertyUpdated("Name");
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        private string _pwd;
        /// <summary>
        /// ����
        /// </summary>
        public string Pwd
        {
            get
            {
                return _pwd;
            }
            set
            {
                _pwd=value;
                OnPropertyUpdated("Pwd");
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        private EmpType _type;
        /// <summary>
        /// ����
        /// </summary>
        public EmpType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type=value;
                OnPropertyUpdated("Type");
            }
        }
        /// <summary>
        /// �༶ID
        /// </summary>
        private int? _classId;
        /// <summary>
        /// �༶ID
        /// </summary>
        public int? ClassId
        {
            get
            {
                return _classId;
            }
            set
            {
                _classId=value;
                OnPropertyUpdated("ClassId");
            }
        }

        /// <summary>
        /// ��ע
        /// </summary>
        private string _remark;
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark=value;
                OnPropertyUpdated("Remark");
            }
        }

        /// <summary>
        /// ���֤��
        /// </summary>
        private string _IDCard;
        /// <summary>
        /// ���֤��
        /// </summary>
        public string IDCard
        {
            get
            {
                return _IDCard;
            }
            set
            {
                _IDCard=value;
                OnPropertyUpdated("IDCard");
            }
        }

        /// <summary>
        /// �����༶
        /// </summary>
        protected ManClass _belongClass = null;
        /// <summary>
        /// �����༶
        /// </summary>
        public ManClass BelongClass
        {
            get
            {
               if (_belongClass == null)
               {
                   FillParent("BelongClass");
               }
                return _belongClass;
            }
        }

        
    }


}
