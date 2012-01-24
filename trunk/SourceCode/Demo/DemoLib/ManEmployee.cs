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
    /// 员工
    /// </summary>
    public partial class ManEmployee:ManBase
    {
        /// <summary>
        /// 员工名
        /// </summary>
        private string _name;
        /// <summary>
        /// 员工名
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
        /// 密码
        /// </summary>
        private string _pwd;
        /// <summary>
        /// 密码
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
        /// 类型
        /// </summary>
        private EmpType _type;
        /// <summary>
        /// 类型
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
        /// 班级ID
        /// </summary>
        private int? _classId;
        /// <summary>
        /// 班级ID
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
        /// 备注
        /// </summary>
        private string _remark;
        /// <summary>
        /// 备注
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
        /// 身份证号
        /// </summary>
        private string _IDCard;
        /// <summary>
        /// 身份证号
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
        /// 所属班级
        /// </summary>
        protected ManClass _belongClass = null;
        /// <summary>
        /// 所属班级
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
