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
        private static School_ScClass _ScClass = new School_ScClass();
    
        public static School_ScClass ScClass
        {
            get
            {
                return _ScClass;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class School_ScClass : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _className = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle ClassName
        {
            get
            {
                return _className;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScClass(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestPerLib.ScClass),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScClass(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _className=CreateProperty("ClassName");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public School_ScClass() 
            :this(null,null)
        {
        }
    }
}
