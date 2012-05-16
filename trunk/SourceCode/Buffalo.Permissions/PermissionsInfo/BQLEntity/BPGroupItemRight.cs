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
    ///  ģ������Ȩ��
    /// </summary>
    public partial class PermissionDB_BPGroupItemRight : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _modelItemId = null;
        /// <summary>
        /// ��ģ��ID
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
        /// ��ID
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
        /// ������ģ��Ȩ�޼�¼ID
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
        /// ��Ȩ��
        /// </summary>
        public BQLEntityParamHandle ItemRight
        {
            get
            {
                return _itemRight;
            }
         }

        /// <summary>
        /// ��������
        /// </summary>
        public PermissionDB_BPSysModelItem BelongItem
        {
            get
            {
               return new PermissionDB_BPSysModelItem(this,"BelongItem");
            }
         }
        /// <summary>
        /// ��������
        /// </summary>
        public PermissionDB_BPGroup BelongGroup
        {
            get
            {
               return new PermissionDB_BPGroup(this,"BelongGroup");
            }
         }
        /// <summary>
        /// ������ģ��Ȩ�޼�¼
        /// </summary>
        public PermissionDB_BPGroupModelRight BelongGroupModelRight
        {
            get
            {
               return new PermissionDB_BPGroupModelRight(this,"BelongGroupModelRight");
            }
         }


		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public PermissionDB_BPGroupItemRight(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPGroupItemRight),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public PermissionDB_BPGroupItemRight(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _modelItemId=CreateProperty("ModelItemId");
            _groupId=CreateProperty("GroupId");
            _groupModelRightId=CreateProperty("GroupModelRightId");
            _itemRight=CreateProperty("ItemRight");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public PermissionDB_BPGroupItemRight() 
            :this(null,null)
        {
        }
    }
}
