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
        private static MyClass_ManMessage _ManMessage = AddToDB(new MyClass_ManMessage()) as MyClass_ManMessage;
    
        public static MyClass_ManMessage ManMessage
        {
            get
            {
                return _ManMessage;
            }
        }
    }
    
    /// <summary>
    ///  
    /// </summary>
    public partial class MyClass_ManMessage : MyClass_ManBase
    {
        private BQLEntityParamHandle _title = null;
        /// <summary>
        /// 标题
        /// </summary>
        public BQLEntityParamHandle Title
        {
            get
            {
                return _title;
            }
         }
        private BQLEntityParamHandle _content = null;
        /// <summary>
        /// 内容
        /// </summary>
        public BQLEntityParamHandle Content
        {
            get
            {
                return _content;
            }
         }
        private BQLEntityParamHandle _belongUser = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle BelongUser
        {
            get
            {
                return _belongUser;
            }
         }

        /// <summary>
        /// 
        /// </summary>
        public MyClass_ManUsers Belong
        {
            get
            {
               return new MyClass_ManUsers(this,"Belong");
            }
         }

        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="entityInfo">类信息</param>
        public MyClass_ManMessage( BQLEntityTableHandle parent,string propertyName) 
            :base(EntityInfoManager.GetEntityHandle(typeof(TestAddIn.ManMessage)),parent,propertyName)
        {
            _title=CreateProperty("Title");
            _content=CreateProperty("Content");
            _belongUser=CreateProperty("BelongUser");

        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="entityInfo">类信息</param>
        public MyClass_ManMessage() 
            :this(null,null)
        {
        }
    }
}
