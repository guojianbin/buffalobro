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
        private static PermissionDB_BPGroup _BPGroup = new PermissionDB_BPGroup();
    
        public static PermissionDB_BPGroup BPGroup
        {
            get
            {
                return _BPGroup;
            }
        }
    }

    /// <summary>
    ///  权限组
    /// </summary>
    public partial class PermissionDB_BPGroup : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _groupID = null;
        /// <summary>
        /// 来自外部系统的组标识
        /// </summary>
        public BQLEntityParamHandle GroupID
        {
            get
            {
                return _groupID;
            }
         }
        private BQLEntityParamHandle _groupName = null;
        /// <summary>
        /// 来自外部系统的表名
        /// </summary>
        public BQLEntityParamHandle GroupName
        {
            get
            {
                return _groupName;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPGroup(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPGroup),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPGroup(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _groupID=CreateProperty("GroupID");
            _groupName=CreateProperty("GroupName");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public PermissionDB_BPGroup() 
            :this(null,null)
        {
        }
    }
}
