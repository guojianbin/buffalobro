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
        private static MyClass_ManUsers _ManUsers = AddToDB(new MyClass_ManUsers()) as MyClass_ManUsers;
    
        public static MyClass_ManUsers ManUsers
        {
            get
            {
                return _ManUsers;
            }
        }
    }
    
    /// <summary>
    ///  
    /// </summary>
    public partial class MyClass_ManUsers : MyClass_ManBase
    {
        private BQLEntityParamHandle _name = null;
        /// <summary>
        /// ����
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }
        private BQLEntityParamHandle _remark = null;
        /// <summary>
        /// �û���ע
        /// </summary>
        public BQLEntityParamHandle Remark
        {
            get
            {
                return _remark;
            }
         }
        private BQLEntityParamHandle _userType = null;
        /// <summary>
        /// �û�����
        /// </summary>
        public BQLEntityParamHandle UserType
        {
            get
            {
                return _userType;
            }
         }
        private BQLEntityParamHandle _classId = null;
        /// <summary>
        /// �༶ID
        /// </summary>
        public BQLEntityParamHandle ClassId
        {
            get
            {
                return _classId;
            }
         }
        private BQLEntityParamHandle _image = null;
        /// <summary>
        /// ͷ��
        /// </summary>
        public BQLEntityParamHandle Image
        {
            get
            {
                return _image;
            }
         }
        private BQLEntityParamHandle _age = null;
        /// <summary>
        /// ����
        /// </summary>
        public BQLEntityParamHandle Age
        {
            get
            {
                return _age;
            }
         }
        private BQLEntityParamHandle _password = null;
        /// <summary>
        /// ����
        /// </summary>
        public BQLEntityParamHandle Password
        {
            get
            {
                return _password;
            }
         }

        /// <summary>
        /// �����༶
        /// </summary>
        public MyClass_ManClass BelongClass
        {
            get
            {
               return new MyClass_ManClass(this,"BelongClass");
            }
         }

        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="entityInfo">����Ϣ</param>
        public MyClass_ManUsers( BQLEntityTableHandle parent,string propertyName) 
            :base(EntityInfoManager.GetEntityHandle(typeof(TestAddIn.ManUsers)),parent,propertyName)
        {
            _name=CreateProperty("Name");
            _remark=CreateProperty("Remark");
            _userType=CreateProperty("UserType");
            _classId=CreateProperty("ClassId");
            _image=CreateProperty("Image");
            _age=CreateProperty("Age");
            _password=CreateProperty("Password");

        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="entityInfo">����Ϣ</param>
        public MyClass_ManUsers() 
            :this(null,null)
        {
        }
    }
}
