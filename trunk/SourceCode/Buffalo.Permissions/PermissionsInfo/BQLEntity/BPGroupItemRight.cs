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
        private static PermissionDB_BPGroupItemRight _BPGroupItemRight = new PermissionDB_BPGroupItemRight();
    
        public static PermissionDB_BPGroupItemRight BPGroupItemRight
        {
            get
            {
                return _BPGroupItemRight;
            }
        }
    }

    /// <summary>
    ///  模块子项权限
    /// </summary>
    public partial class PermissionDB_BPGroupItemRight : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _modelItemId = null;
        /// <summary>
        /// 子模块ID
        /// </summary>
        public BQLEntityParamHandle ModelItemId
        {
            get
            {
                return _modelItemId;
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
        private BQLEntityParamHandle _groupModelRightId = null;
        /// <summary>
        /// 所属的模块权限记录ID
        /// </summary>
        public BQLEntityParamHandle GroupModelRightId
        {
            get
            {
                return _groupModelRightId;
            }
         }
        private BQLEntityParamHandle _itemRight = null;
        /// <summary>
        /// 项权限
        /// </summary>
        public BQLEntityParamHandle ItemRight
        {
            get
            {
                return _itemRight;
            }
         }

        /// <summary>
        /// 所属子项
        /// </summary>
        public PermissionDB_BPSysModelItem BelongItem
        {
            get
            {
               return new PermissionDB_BPSysModelItem(this,"BelongItem");
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
        /// 所属的模块权限记录
        /// </summary>
        public PermissionDB_BPGroupModelRight BelongGroupModelRight
        {
            get
            {
               return new PermissionDB_BPGroupModelRight(this,"BelongGroupModelRight");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPGroupItemRight(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPGroupItemRight),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPGroupItemRight(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _modelItemId=CreateProperty("ModelItemId");
            _groupId=CreateProperty("GroupId");
            _groupModelRightId=CreateProperty("GroupModelRightId");
            _itemRight=CreateProperty("ItemRight");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public PermissionDB_BPGroupItemRight() 
            :this(null,null)
        {
        }
    }
}
