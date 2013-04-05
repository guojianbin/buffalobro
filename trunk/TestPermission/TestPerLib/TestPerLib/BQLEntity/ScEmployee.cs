using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestPerLib.BQLEntity
{

    public partial class School 
    {
        private static School_ScEmployee _ScEmployee = new School_ScEmployee();
    
        public static School_ScEmployee ScEmployee
        {
            get
            {
                return _ScEmployee;
            }
        }
    }

    /// <summary>
    ///  登陆员工
    /// </summary>
    public partial class School_ScEmployee : School_ScBase
    {
        private BQLEntityParamHandle _name = null;
        /// <summary>
        /// 登录名称
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }
        private BQLEntityParamHandle _realName = null;
        /// <summary>
        /// 真实名称
        /// </summary>
        public BQLEntityParamHandle RealName
        {
            get
            {
                return _realName;
            }
         }
        private BQLEntityParamHandle _password = null;
        /// <summary>
        /// 密码
        /// </summary>
        public BQLEntityParamHandle Password
        {
            get
            {
                return _password;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScEmployee(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestPerLib.ScEmployee),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScEmployee(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _name=CreateProperty("Name");
            _realName=CreateProperty("RealName");
            _password=CreateProperty("Password");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public School_ScEmployee() 
            :this(null,null)
        {
        }
    }
}
