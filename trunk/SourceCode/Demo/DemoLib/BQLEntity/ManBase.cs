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
        private static MyClass_ManBase _ManBase = AddToDB(new MyClass_ManBase()) as MyClass_ManBase;
    
        public static MyClass_ManBase ManBase
        {
            get
            {
                return _ManBase;
            }
        }
    }
    
    /// <summary>
    ///  
    /// </summary>
    public partial class MyClass_ManBase : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _state = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle State
        {
            get
            {
                return _state;
            }
         }
        private BQLEntityParamHandle _lastUpdate = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
         }


        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="entityInfo">����Ϣ</param>
        public MyClass_ManBase(BQLEntityTableHandle parent,string propertyName) 
            :base(EntityInfoManager.GetEntityHandle(typeof(TestAddIn.ManBase)),parent,propertyName)
        {
            _id=CreateProperty("Id");
            _state=CreateProperty("State");
            _lastUpdate=CreateProperty("LastUpdate");

        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="entityInfo">����Ϣ</param>
        public MyClass_ManBase() 
            :this(null,null)
        {
        }
    }
}
