using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.Permissions.PermissionsInfo.BQLEntity
{

    public partial class PermissionDB
    {
        private static PermissionDB_BPGroupModelRight _BPGroupModelRight = new PermissionDB_BPGroupModelRight();
    
        public static PermissionDB_BPGroupModelRight BPGroupModelRight
        {
            get
            {
                return _BPGroupModelRight;
            }
        }
    }

    /// <summary>
    ///  用户组的模块权限
    /// </summary>
    public partial class PermissionDB_BPGroupModelRight : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _modelId = null;
        /// <summary>
        /// 组模块ID
        /// </summary>
        public BQLEntityParamHandle ModelId
        {
            get
            {
                return _modelId;
            }
         }
        private BQLEntityParamHandle _groupId = null;
        /// <summary>
        /// 组ID
        /// </summary>
        public BQLEntityParamHandle GroupId
        {
            get
            {
                return _groupId;
            }
         }
        private BQLEntityParamHandle _ModelRight = null;
        /// <summary>
        /// 模块权限
        /// </summary>
        public BQLEntityParamHandle ModelRight
        {
            get
            {
                return _ModelRight;
            }
         }

        /// <summary>
        /// 所属模块
        /// </summary>
        public PermissionDB_BPSysModel BelongModel
        {
            get
            {
               return new PermissionDB_BPSysModel(this,"BelongModel");
            }
         }
        /// <summary>
        /// 所属分组
        /// </summary>
        public PermissionDB_BPGroup BelongGroup
        {
            get
            {
               return new PermissionDB_BPGroup(this,"BelongGroup");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPGroupModelRight(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPGroupModelRight),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPGroupModelRight(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _modelId=CreateProperty("ModelId");
            _groupId=CreateProperty("GroupId");
            _ModelRight=CreateProperty("ModelRight");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public PermissionDB_BPGroupModelRight() 
            :this(null,null)
        {
        }
    }
}
