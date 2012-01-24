using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestAddIn.BQLEntity
{
    public partial class MyClass
    {
        private static MyClass_ManEmployee _ManEmployee = AddToDB(new MyClass_ManEmployee()) as MyClass_ManEmployee;
    
        public static MyClass_ManEmployee ManEmployee
        {
            get
            {
                return _ManEmployee;
            }
        }
    }
    
    /// <summary>
    ///  员工
    /// </summary>
    public partial class MyClass_ManEmployee : MyClass_ManBase
    {
        private BQLEntityParamHandle _name = null;
        /// <summary>
        /// 员工名
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }
        private BQLEntityParamHandle _pwd = null;
        /// <summary>
        /// 密码
        /// </summary>
        public BQLEntityParamHandle Pwd
        {
            get
            {
                return _pwd;
            }
         }
        private BQLEntityParamHandle _classId = null;
        /// <summary>
        /// 班级ID
        /// </summary>
        public BQLEntityParamHandle ClassId
        {
            get
            {
                return _classId;
            }
         }
        private BQLEntityParamHandle _remark = null;
        /// <summary>
        /// 备注
        /// </summary>
        public BQLEntityParamHandle Remark
        {
            get
            {
                return _remark;
            }
         }
        private BQLEntityParamHandle _IDCard = null;
        /// <summary>
        /// 身份证号
        /// </summary>
        public BQLEntityParamHandle IDCard
        {
            get
            {
                return _IDCard;
            }
         }

        /// <summary>
        /// 所属班级
        /// </summary>
        public MyClass_ManClass BelongClass
        {
            get
            {
               return new MyClass_ManClass(this,"BelongClass");
            }
         }

        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="entityInfo">类信息</param>
        public MyClass_ManEmployee( BQLEntityTableHandle parent,string propertyName) 
            :base(EntityInfoManager.GetEntityHandle(typeof(TestAddIn.ManEmployee)),parent,propertyName)
        {
            _name=CreateProperty("Name");
            _pwd=CreateProperty("Pwd");
            _classId=CreateProperty("ClassId");
            _remark=CreateProperty("Remark");
            _IDCard=CreateProperty("IDCard");

        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="entityInfo">类信息</param>
        public MyClass_ManEmployee() 
            :this(null,null)
        {
        }
    }
}
