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
        private static School_ScStudent _ScStudent = new School_ScStudent();
    
        public static School_ScStudent ScStudent
        {
            get
            {
                return _ScStudent;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class School_ScStudent : School_ScBase
    {
        private BQLEntityParamHandle _name = null;
        /// <summary>
        /// 学生名
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }
        private BQLEntityParamHandle _age = null;
        /// <summary>
        /// 年龄
        /// </summary>
        public BQLEntityParamHandle Age
        {
            get
            {
                return _age;
            }
         }
        private BQLEntityParamHandle _classId = null;
        /// <summary>
        /// 所属班级ID
        /// </summary>
        public BQLEntityParamHandle ClassId
        {
            get
            {
                return _classId;
            }
         }

        /// <summary>
        /// 所属班级
        /// </summary>
        public School_ScClass BelongClass
        {
            get
            {
               return new School_ScClass(this,"BelongClass");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScStudent(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestPerLib.ScStudent),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScStudent(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _name=CreateProperty("Name");
            _age=CreateProperty("Age");
            _classId=CreateProperty("ClassId");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public School_ScStudent() 
            :this(null,null)
        {
        }
    }
}
