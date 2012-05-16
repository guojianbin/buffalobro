using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.Permissions.PermissionsInfo.BQLEntity
{
    /// <summary>
    ///  权限基类
    /// </summary>
    public partial class PermissionDB_BPBase : BQLEntityTableHandle
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
        private BQLEntityParamHandle _laseUpdate = null;
        /// <summary>
        /// 更新时间
        /// </summary>
        public BQLEntityParamHandle LaseUpdate
        {
            get
            {
                return _laseUpdate;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPBase(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPBase),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPBase(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _laseUpdate=CreateProperty("LaseUpdate");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public PermissionDB_BPBase() 
            :this(null,null)
        {
        }
    }
}
