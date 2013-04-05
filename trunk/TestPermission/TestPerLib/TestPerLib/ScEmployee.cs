using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestPerLib
{
    /// <summary>
    /// 登陆员工
    /// </summary>
    public partial class ScEmployee:ScBase
    {
        /// <summary>
        /// 登录名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 真实名称
        /// </summary>
        private string _realName;
        /// <summary>
        /// 密码
        /// </summary>
        private string _password;
        /// <summary>
        /// 登录名称
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
        /// 真实名称
        /// </summary>
        public string RealName
        {
            get
            {
                return _realName;
            }
            set
            {
                _realName=value;
                OnPropertyUpdated("RealName");
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password=value;
                OnPropertyUpdated("Password");
            }
        }

    }
}
