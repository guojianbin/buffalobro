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
        private static MyClass_ManClass _ManClass = AddToDB(new MyClass_ManClass()) as MyClass_ManClass;
    
        public static MyClass_ManClass ManClass
        {
            get
            {
                return _ManClass;
            }
        }
    }
    
    /// <summary>
    ///  
    /// </summary>
    public partial class MyClass_ManClass : MyClass_ManBase
    {
        private BQLEntityParamHandle _manName = null;
        /// <summary>
        /// 名称
        /// </summary>
        public BQLEntityParamHandle ManName
        {
            get
            {
                return _manName;
            }
         }


        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="entityInfo">类信息</param>
        public MyClass_ManClass( BQLEntityTableHandle parent,string propertyName) 
            :base(EntityInfoManager.GetEntityHandle(typeof(TestAddIn.ManClass)),parent,propertyName)
        {
            _manName=CreateProperty("ManName");

        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="entityInfo">类信息</param>
        public MyClass_ManClass() 
            :this(null,null)
        {
        }
    }
}
