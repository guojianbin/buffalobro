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
    /// ��½Ա��
    /// </summary>
    public partial class ScEmployee:ScBase
    {
        /// <summary>
        /// ��¼����
        /// </summary>
        private string _name;
        /// <summary>
        /// ��ʵ����
        /// </summary>
        private string _realName;
        /// <summary>
        /// ����
        /// </summary>
        private string _password;
        /// <summary>
        /// ��¼����
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name=value;
            }
        }
        /// <summary>
        /// ��ʵ����
        /// </summary>
        public virtual string RealName
        {
            get
            {
                return _realName;
            }
            set
            {
                _realName=value;
               
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public virtual string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password=value;
                
            }
        }
    }
}
