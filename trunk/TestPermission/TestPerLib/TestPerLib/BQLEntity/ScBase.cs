using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestPerLib.BQLEntity
{
    /// <summary>
    ///  
    /// </summary>
    public partial class School_ScBase : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// ID
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _lastDate = null;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public BQLEntityParamHandle LastDate
        {
            get
            {
                return _lastDate;
            }
         }
        private BQLEntityParamHandle _createDate = null;
        /// <summary>
        /// 创建时间
        /// </summary>
        public BQLEntityParamHandle CreateDate
        {
            get
            {
                return _createDate;
            }
         }
        private BQLEntityParamHandle _state = null;
        /// <summary>
        /// 状态
        /// </summary>
        public BQLEntityParamHandle State
        {
            get
            {
                return _state;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScBase(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestPerLib.ScBase),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public School_ScBase(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _lastDate=CreateProperty("LastDate");
            _createDate=CreateProperty("CreateDate");
            _state=CreateProperty("State");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public School_ScBase() 
            :this(null,null)
        {
        }
    }
}
